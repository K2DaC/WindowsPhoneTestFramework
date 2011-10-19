// ----------------------------------------------------------------------
// <copyright file="AutomationUsingProgramBase.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using WindowsPhoneTestFramework.AutomationController.Interfaces;

namespace WindowsPhoneTestFramework.CommandLineHost.Commands
{
    public class PhoneAutomationCommands
    {
        public IPhoneAutomationController PhoneAutomationController { get; set; }

#warning Delete click... it's there only for people who watched the "how to" video
        [CommandLineCommand("click")]
        [Description("deprecated... this method is now replaced by invokeTap - sorry!")]
        public void Click(string whatToClick)
        {
            Console.WriteLine("Click: this method is now replaced by invokeTap - sorry!");
        }

        [CommandLineCommand("invokeTap")]
        [Description("invoke the action that a tap would normally do on the identified control - for a button this is Click, for a checkbox this is Toggle, for other controls you'll need to setup the mapping yourself using AddAutomationPeerHandlerForTapAction and AddUIElementHandlerForTapAction methods - e.g. 'invoke Button1'")]
        public void InvokeTap(string whatToClick)
        {
            var result = PhoneAutomationController.InvokeControlTapAction(whatToClick);
            Console.WriteLine("invokeTap:" + result.ToString());
        }

        [CommandLineCommand("ping")]
        [Description("sends an 'are you alive?' message to the application to test connectivity to the app - e.g. 'ping'")]
        public void ConfirmAlive(string ignored)
        {
            var result = PhoneAutomationController.LookIsAlive();
            Console.WriteLine("Alive:" + result.ToString());
        }

        [CommandLineCommand("lookForText")]
        [Description("looks for displayed text within the app UI - e.g. 'lookForText Page 1'")]
        public void LookForText(string whatToLookFor)
        {
            var result = PhoneAutomationController.LookForText(whatToLookFor);
            Console.WriteLine("LookForText:" + result.ToString());
        }

        [CommandLineCommand("waitForText")]
        [Description("waits for up to 1 minute for the text to be displayed within the app UI - e.g. 'waitForText Page 2'")]
        public void WhatForText(string whatToWaitFor)
        {
            var result = PhoneAutomationController.WaitForText(whatToWaitFor);
            Console.WriteLine("WaitForText:" + result.ToString());
        }

        [CommandLineCommand("getText")]
        [Description("gets text from the named control in the app UI - e.g. 'getText TextBox1'")]
        public void GetText(string whatToGet)
        {
            string text;
            var result = PhoneAutomationController.TryGetTextFromControl(whatToGet, out text);
            Console.WriteLine("GetText:" + (result ? text : "FAIL"));
        }

        [CommandLineCommand("setText")]
        [Description("sets text on the named control in the app UI - e.g. 'setText TextBox1=Hello World'")]
        public void SetText(string whatToSetAndValue)
        {
            var items = whatToSetAndValue.Split(new char[] { '=' }, 2);
            if (items.Count() != 2)
            {
                Console.WriteLine("Incorrect syntax - require setText id=value");
                return;
            }

            var result = PhoneAutomationController.SetTextOnControl(items[0], items[1]);
            Console.WriteLine("SetText:" + result);
        }

        [CommandLineCommand("setFocus")]
        [Description("sets the focus to the specified control - e.g. 'setFocus TextBox1'")]
        public void SetFocus(string whichControl)
        {
            var result = PhoneAutomationController.SetFocus(whichControl);
            Console.WriteLine("setFocus: " + result);
        }

        [CommandLineCommand("getPosition")]
        [Description("gets the position of the specified control as device screen location - e.g. 'getPosition TextBox1'")]
        public void GetPosition(string whichControl)
        {
            var position = PhoneAutomationController.GetPositionOfControl(whichControl);
            if (position == RectangleF.Empty)
            {
                Console.WriteLine("getPosition: failed");
                return;
            }

            Console.WriteLine(string.Format("getPosition: {0:0.0} {1:0.0} {2:0.0} {3:0.0}", position.Left, position.Top, position.Width, position.Height));
        }

        [CommandLineCommand("screenshot")]
        [Description("requests a screenshot from the running application - provide an optional control to just picture that control - e.g. 'screenshot' or 'screenshot TextBox1'")]
        public void TakeScreenshot(string optionalControlId)
        {
            Bitmap bitmap;
            if (string.IsNullOrWhiteSpace(optionalControlId))
                optionalControlId = null;
            var result = PhoneAutomationController.TakePicture(optionalControlId, out bitmap);
            Console.WriteLine("TakePicture: " + result);
            if (result)
            {
                var fileName = Path.GetTempFileName() + ".jpg";
                try
                {
                    bitmap.Save(fileName);
                    var process = new Process
                                      {
                                          StartInfo =
                                              {
                                                  FileName = fileName
                                              }
                                      };

                    if (!process.Start())
                        return;

                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5.0));

                    if (!process.WaitForExit(60000)) // one minute
                        process.Kill();

                    File.Delete(fileName);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(string.Format("Exception seen {0} - {1}", exception.GetType().FullName, exception.Message));
                }
            }
        }
    }
}