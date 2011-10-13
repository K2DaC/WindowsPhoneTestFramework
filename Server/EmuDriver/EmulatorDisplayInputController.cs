// ----------------------------------------------------------------------
// <copyright file="EmulatorDisplayInputController.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
//     
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;
using WindowsPhoneTestFramework.Utils;

namespace WindowsPhoneTestFramework.EmuDriver
{
    public class EmulatorDisplayInputController : TraceBase, IDisplayInputController
    {
        // constants - used for WindowsInput mouse positioning
        private const double VirtualScreenWidth = 65535.0;
        private const double VirtualScreenHeight = 65535.0;

        // pauses
        private static readonly TimeSpan DefaultPauseDurationAfterAction = TimeSpan.FromMilliseconds(100.0);
        public TimeSpan PauseDurationAfterSendingKeyPress { get; set; }
        public TimeSpan PauseDurationAfterSettingForegroundWindow { get; set; }
        public TimeSpan PauseDurationAfterPerformingGesture { get; set; }
        public TimeSpan PauseDurationAfterTextEntry { get; set; }

        // Emulator Win32 Windows process name, window class names and window titles
        // - these are made public in case your environment uses different class/window names
        public string EmulatorWindowClassName { get; set; }
        public string EmulatorWindowWindowName { get; set; }
        public string EmulatorSkinWindowClassName { get; set; }
        public string EmulatorSkinWindowWindowName { get; set; }
        public string EmulatorProcessName { get; set; }

        // reference to input simulator from WindowsInput
        private readonly IInputSimulator _inputSimulator;

        public EmulatorDisplayInputController()
            : this(new InputSimulator())
        {
            EmulatorWindowClassName = "XDE_LCDWindow";
            EmulatorWindowWindowName = string.Empty;

            EmulatorSkinWindowWindowName = "Windows Phone Emulator";
            EmulatorSkinWindowClassName = "XDE_SkinWindow";

            EmulatorProcessName = "XDE";
        }

        public EmulatorDisplayInputController(IInputSimulator inputSimulator)
        {
            _inputSimulator = inputSimulator;
            PauseDurationAfterSendingKeyPress = DefaultPauseDurationAfterAction;
            PauseDurationAfterSettingForegroundWindow = DefaultPauseDurationAfterAction;
            PauseDurationAfterPerformingGesture = DefaultPauseDurationAfterAction;
            PauseDurationAfterTextEntry = DefaultPauseDurationAfterAction;
        }

        public void EnsureWindowIsInForeground()
        {
            /*
             * this code left over from attempt to use the skin window - the attempt failed
            var result = NativeMethods.EnsureWindowIsInForeground(EmulatorSkinWindowClassName, EmulatorSkinWindowClassName);
            if (!result)
                throw new ManipulationFailedException("Failed to bring emulator skin window to foreground");
            */

            var topMostResult = NativeMethods.MakeWindowTopMost(EmulatorProcessName);
            if (!topMostResult)
                throw new ManipulationFailedException("Failed to bring emulator skin window to topMost");

            var result = NativeMethods.EnsureWindowIsInForeground(EmulatorWindowClassName, EmulatorWindowWindowName);
            if (!result)
                throw new ManipulationFailedException("Failed to bring emulator window to foreground");

            Pause(PauseDurationAfterSettingForegroundWindow);
        }

        public void ReleaseWindowFromForeground()
        {
            if (!NativeMethods.RevokeWindowTopMost(EmulatorProcessName))
                InvokeTrace("Failed to revoke emulator window topmost"); // this is ignored for now..

            Pause(PauseDurationAfterSettingForegroundWindow);
        }

        public void EnsureHardwareKeyboardEnabled()
        {
            SendKeyPress(VirtualKeyCode.PRIOR);
        }

        public void EnsureHardwareKeyboardDisabled()
        {
            InvokeTrace("Warning - EnsureHardwareKeyboardDisabled method is not currently operational - not sure why");
            SendKeyPress(VirtualKeyCode.NEXT);
        }

        public void PressHardwareButton(WindowsPhoneHardwareButton whichHardwareButton)
        {
            SendKeyPress(HardwareButtonToKeyCode(whichHardwareButton));
        }

        public void DoGesture(IGesture gesture)
        {
            gesture.Perform(this);
            Pause(PauseDurationAfterPerformingGesture);
        }

        public void PerformMouseDownMoveUp(IEnumerable<Point> points, TimeSpan periodBetweenPoints)
        {
            // convert to array to ensure we don't perform too much linq
            var array = points.ToArray();

            if (array.Length < 2)
                throw new ManipulationFailedException("Requested PerformMouseDownMoveUp with too few points - {0}", array.Length);

            // mouse down at the start point
            var startPoint = array.First();
            _inputSimulator.Mouse.MoveMouseTo(startPoint.X, startPoint.Y);
            _inputSimulator.Mouse.LeftButtonDown();

            foreach (var point in array.Skip(1).Take(array.Length - 2))
            {
                _inputSimulator.Mouse.MoveMouseTo(point.X, point.Y);
                Pause(periodBetweenPoints);
            }

            var endPoint = array.First();
            _inputSimulator.Mouse.MoveMouseTo(endPoint.X, endPoint.Y);
            _inputSimulator.Mouse.LeftButtonUp();
        }

        public WindowsPhoneOrientation GuessOrientation()
        {
            var rect = NativeMethods.GetWindowRectangle(EmulatorWindowClassName, EmulatorWindowWindowName);
            if (rect.IsEmpty)
                throw new ManipulationFailedException("Failed to get emulator window rectangle");

            return GuessOrientation(rect);
        }


        public IEnumerable<Point> TranslatePhonePositionsToHostPositions(IEnumerable<Point> points)
        {
            var rect = NativeMethods.GetWindowRectangle(EmulatorWindowClassName, EmulatorWindowWindowName);
            if (rect.IsEmpty)
                throw new ManipulationFailedException("Failed to get emulator window rectangle");

            var emulatorScaleRatio = EstimateScaleRatio(rect);

            var screenRect = NativeMethods.GetDesktopRectangle();
            var hostPoints = from p in points
                             select new Point()
                                        {
                                            X = (int)(VirtualScreenWidth * (rect.X + emulatorScaleRatio * p.X) / screenRect.Width),
                                            Y = (int)(VirtualScreenHeight * (rect.Y + emulatorScaleRatio * p.Y) / screenRect.Height)
                                        };

            return hostPoints;
        }

        private static double EstimateScaleRatio(Rectangle rect)
        {
            var orientation = GuessOrientation(rect);
            switch (orientation)
            {
                case WindowsPhoneOrientation.Landscape800By480:
                    return (double)rect.Width / 800.0;

                case WindowsPhoneOrientation.Portrait480By800:
                    return (double)rect.Width / 480.0;
            }

            throw new ManipulationFailedException("Unexpected orientation " + orientation);
        }

        private static WindowsPhoneOrientation GuessOrientation(Rectangle rect)
        {
            var ratio = ((double)rect.Width) / ((double)rect.Height);
            if (Math.Abs(ratio - 800.0 / 480.0) < 0.01)
                return WindowsPhoneOrientation.Landscape800By480;
            if (Math.Abs(ratio - 480.0 / 800.0) < 0.01)
                return WindowsPhoneOrientation.Portrait480By800;
            throw new ManipulationFailedException("Unable to guess ratio for width {0} height {1}", rect.Width, rect.Height);
        }

        private static VirtualKeyCode HardwareButtonToKeyCode(WindowsPhoneHardwareButton whichHardwareButton)
        {
            VirtualKeyCode vk;
            switch (whichHardwareButton)
            {
                case WindowsPhoneHardwareButton.Back:
                    vk = VirtualKeyCode.F1;
                    break;
                case WindowsPhoneHardwareButton.Home:
                    vk = VirtualKeyCode.F2;
                    break;
                case WindowsPhoneHardwareButton.Search:
                    vk = VirtualKeyCode.F3;
                    break;
                case WindowsPhoneHardwareButton.Camera:
                    vk = VirtualKeyCode.F7;
                    break;
                case WindowsPhoneHardwareButton.VolumeUp:
                    vk = VirtualKeyCode.F9;
                    break;
                case WindowsPhoneHardwareButton.VolumeDown:
                    vk = VirtualKeyCode.F10;
                    break;
                case WindowsPhoneHardwareButton.Power:
                    vk = VirtualKeyCode.PRINT;
                    break;
                default:
                    throw new ManipulationFailedException("Unknown Hardware Button " + whichHardwareButton);
            }

            return vk;
        }

        public void SendKeyPress(VirtualKeyCode virtualKeyCode)
        {
            _inputSimulator.Keyboard.KeyPress(virtualKeyCode);
            Pause(PauseDurationAfterSendingKeyPress);
        }

        public void TextEntry(string text)
        {
            InvokeTrace("Warning - TextEntry method is not currently operational - not sure why");
            _inputSimulator.Keyboard.TextEntry(text);
            Pause(PauseDurationAfterTextEntry);
        }

        private static void Pause(TimeSpan duration)
        {
            if (duration > TimeSpan.Zero)
                Thread.Sleep(duration);
        }
    }
}