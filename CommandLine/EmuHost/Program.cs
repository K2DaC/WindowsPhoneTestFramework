// ----------------------------------------------------------------------
// <copyright file="Program.cs" company="Expensify">
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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Args;
using Args.Help;
using Args.Help.Formatters;
using WindowsInput.Native;
using WindowsPhoneTestFramework.CommandLineHost;
using WindowsPhoneTestFramework.EmuAutomationController.Interfaces;
using WindowsPhoneTestFramework.EmuDriver;

namespace WindowsPhoneTestFramework.EmuHost
{
    public class Program : AutomationUsingProgramBase
    {
        public static void Main(string[] args)
        {
            AppLaunchingCommandLine commandLine;
            IModelBindingDefinition<AppLaunchingCommandLine> modelBindingDefinition = null;
            try
            {
                modelBindingDefinition = Configuration.Configure<AppLaunchingCommandLine>();
                commandLine = modelBindingDefinition.CreateAndBind(args);

                if (commandLine.ProductId == Guid.Empty)
                {
                    Console.WriteLine("No productId supplied");
                    throw new ApplicationException("Help!");
                }
            }
            catch (Exception /*exception*/)
            {
                if (modelBindingDefinition != null)
                {
                    var help = new HelpProvider();
                    var formatter = new ConsoleHelpFormatter();

                    var sw = new StringWriter();
                    var text = help.GenerateModelHelp(modelBindingDefinition);
                    formatter.WriteHelp(text, sw);
                    Console.Write(sw.ToString());
                }
                else
                {
                    Console.Write("Sorry - no help available!");
                }
                return;
            }

            try
            {
                Console.WriteLine("AutomationHost starting");
                using (var program = new Program(commandLine))
                {
                    Console.WriteLine("To show help, enter 'help'");
                    program.Run();
                }
            }
            catch (QuitNowPleaseException)
            {
                Console.WriteLine("Goodbye");
            }
            catch (Exception exception)
            {
                Console.WriteLine(string.Format("Exception seen {0} {1}", exception.GetType().FullName, exception.Message));
            }
        }

        private readonly AppLaunchingCommandLine _commandLine;
        private IEmuAutomationController _emuAutomationController;

        public Program(AppLaunchingCommandLine commandLine)
        {
            _commandLine = commandLine;
            StartEmuAutomationController();
        }

        private void StartEmuAutomationController()
        {
            Console.WriteLine("-> controller will listen for connection on " + _commandLine.Binding);
            Console.WriteLine("-> controller will identify controls using " + _commandLine.AutomationIdentification);
            _emuAutomationController = new EmuAutomationController.EmuAutomationController();
            _emuAutomationController.Trace += (sender, args) => Console.WriteLine("-> " + args.Message);
            _emuAutomationController.Start(new Uri(_commandLine.Binding), _commandLine.AutomationIdentification);
            PhoneAutomationController = _emuAutomationController.PhoneAutomationController;
            Console.WriteLine("-> controller started");
            Console.WriteLine();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_emuAutomationController != null)
                {
                    _emuAutomationController.Dispose();
                    _emuAutomationController = null;
                }
            }
            base.Dispose(isDisposing);
        }

        [DisplayName("install")]
        [Description("installs the app - e.g. 'install'")]
        public void Install(string ignored)
        {
            var result = _emuAutomationController.Driver.Install(_commandLine.ProductId, _commandLine.Name, _commandLine.IconPath, _commandLine.XapPath);
            Console.WriteLine("install:" + result);
        }

        [DisplayName("forceInstall")]
        [Description("installs the app - shutting it down first if required - e.g. 'forceInstall'")]
        public void ForceInstall(string ignored)
        {
            var result = _emuAutomationController.Driver.ForceInstall(_commandLine.ProductId, _commandLine.Name, _commandLine.IconPath, _commandLine.XapPath);
            Console.WriteLine("forceInstall:" + result);
        }

        [DisplayName("uninstall")]
        [Description("uninstalls the app - e.g. 'uninstall'")]
        public void Uninstall(string ignored)
        {
            var result = _emuAutomationController.Driver.Uninstall(_commandLine.ProductId);
            Console.WriteLine("uninstall:" + result);
        }

        [DisplayName("forceUninstall")]
        [Description("uninstalls the app - shutting it down first if required - e.g. 'forceUninstall'")]
        public void ForceUninstall(string ignored)
        {
            var result = _emuAutomationController.Driver.ForceUninstall(_commandLine.ProductId);
            Console.WriteLine("forceUninstall:" + result);
        }

        [DisplayName("launch")]
        [Description("launches the app - e.g. 'launch'")]
        public void Launch(string ignored)
        {
            var result = _emuAutomationController.Driver.Start(_commandLine.ProductId);
            Console.WriteLine("launch:" + result);
        }

        [DisplayName("stop")]
        [Description("stop the app - e.g. 'stop'")]
        public void Stop(string ignored)
        {
            var result = _emuAutomationController.Driver.Stop(_commandLine.ProductId);
            Console.WriteLine("stop:" + result);
        }

        [DisplayName("hardwareButton")]
        [Description("press a hardware button - Back, Start, Search, Camera, VolumeUp, VolumeDown, Power - e.g. 'hardwareButton Back'")]
        public void PressHardware(string whichButton)
        {
            var parsedButton = (WindowsPhoneHardwareButton)Enum.Parse(typeof(WindowsPhoneHardwareButton), whichButton);
            _emuAutomationController.DisplayInputController.EnsureWindowIsInForeground();
            _emuAutomationController.DisplayInputController.EnsureHardwareKeyboardEnabled();
            _emuAutomationController.DisplayInputController.PressHardwareButton(parsedButton);
            Console.WriteLine("hardwareButton: Completed");
        }

        [DisplayName("textEntry")]
        [Description("enter text - e.g. 'enterText Hello World'")]
        public void TextEntry(string text)
        {
            _emuAutomationController.DisplayInputController.EnsureWindowIsInForeground();
            _emuAutomationController.DisplayInputController.EnsureHardwareKeyboardEnabled();
            _emuAutomationController.DisplayInputController.TextEntry(text);
            Console.WriteLine("textEntry: Completed");
        }

        [DisplayName("sendKeyPress")]
        [Description("enter a specific virtual key code - e.g. 'enterText VK_U'")]
        public void SendKeyPress(string whichCode)
        {
            var vk = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), whichCode);
            _emuAutomationController.DisplayInputController.EnsureWindowIsInForeground();
            _emuAutomationController.DisplayInputController.EnsureHardwareKeyboardEnabled();
            _emuAutomationController.DisplayInputController.SendKeyPress(vk);
            Console.WriteLine("sendKeyPress: Completed");
        }

        [DisplayName("listKeyCodes")]
        [Description("lists all defined virtual key code - for info on key codes mapped for emulator, see http://msdn.microsoft.com/en-us/library/ff754352(v=VS.92).aspx - e.g. 'listKeyCodes'")]
        public void ListKeyCodes(string ignore)
        {
            foreach (var vk in Enum.GetValues(typeof(VirtualKeyCode)))
            {
                Console.WriteLine(vk);
            }
            Console.WriteLine("listKeyCodes: Completed");
        }

        enum SwipeDirection
        {
            LeftToRight,
            RightToLeft
        }

        [DisplayName("disableHardwareKeyboard")]
        [Description("disable the PC keyboard - the soft keyboard will then be available - note that other commands - textEntry, keyPress, hardwareButton - reset this as they use the PC keyboard buffers - e.g. 'disableHardwareKeyboard'")]
        public void DisableHardwareKeyboard(string ignored)
        {
            _emuAutomationController.DisplayInputController.EnsureHardwareKeyboardDisabled();
            Console.WriteLine("disableHardwareKeyboard: Completed");
        }

        [DisplayName("doSwipe")]
        [Description("completes a mouse swipe across the screen - currently only LeftToRight or RightToLeft across the horizontal and vertical middle of the screen supported - e.g. 'sendSwipe LeftToRight'")]
        public void SendSwipe(string whichSwipe)
        {
            var orientation = _emuAutomationController.DisplayInputController.GuessOrientation();

            var parsed = (SwipeDirection) Enum.Parse(typeof (SwipeDirection), whichSwipe);
            IGesture gesture = null;
            switch (parsed)
            {
                case SwipeDirection.LeftToRight:
                    gesture = orientation == WindowsPhoneOrientation.Portrait480By800
                                  ? SwipeGesture.LeftToRightPortrait()
                                  : SwipeGesture.LeftToRightLandscape();
                    break;

                case SwipeDirection.RightToLeft:
                    gesture = orientation == WindowsPhoneOrientation.Portrait480By800
                                  ? SwipeGesture.RightToLeftPortrait()
                                  : SwipeGesture.RightToLeftLandscape();
                    break;

                default:
                    throw new ArgumentException("Unexpected swipe " + parsed);
            }

            _emuAutomationController.DisplayInputController.DoGesture(gesture);
            Console.WriteLine("doSwipe: Completed");
        }
    }
}
