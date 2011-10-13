// ----------------------------------------------------------------------
// <copyright file="IDisplayInputController.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using WindowsInput.Native;
using WindowsPhoneTestFramework.Utils;

namespace WindowsPhoneTestFramework.EmuDriver
{
    public interface IDisplayInputController : ITrace
    {
        void EnsureWindowIsInForeground();
        void ReleaseWindowFromForeground();
        void EnsureHardwareKeyboardEnabled();
        void EnsureHardwareKeyboardDisabled();
        WindowsPhoneOrientation GuessOrientation();
        void PressHardwareButton(WindowsPhoneHardwareButton whichHardwareButton);
        void DoGesture(IGesture gesture);
        void SendKeyPress(VirtualKeyCode hardwareButtonToKeyCode);
        void TextEntry(string text);
    }
}