// ----------------------------------------------------------------------
// <copyright file="PhoneAutomationController.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Drawing;
using System.IO;
using System.Threading;
using WindowsPhoneTestFramework.AutomationController.Commands;
using WindowsPhoneTestFramework.AutomationController.Interfaces;
using WindowsPhoneTestFramework.AutomationController.Results;

namespace WindowsPhoneTestFramework.AutomationController
{
    public class PhoneAutomationController : IPhoneAutomationController
    {
        private readonly IPhoneAutomationServiceControl _serviceControl;
        private readonly AutomationIdentification _automationIdentification;

        public PhoneAutomationController(IPhoneAutomationServiceControl serviceControl, AutomationIdentification automationIdentification)
        {
            _serviceControl = serviceControl;
            _automationIdentification = automationIdentification;
        }

        #region IPhoneAutomationController

        public bool ConfirmAlive()
        {
            var command = new ConfirmAliveCommand();
            var result = SyncExecuteCommand(command);
            return result is SuccessResult;
        }

        public bool WaitForText(string text)
        {
            return WaitForText(text, Constants.DefaultWaitTimeout);
        }

        public bool WaitForText(string text, TimeSpan timeout)
        {
            var start = DateTime.UtcNow;
            while (DateTime.UtcNow - start < timeout)
            {
                if (LookForText(text))
                {
                    return true;
                }
            }

            return false;
        }

        public bool LookForText(string text)
        {
            var command = new LookForTextCommand() { Text = text };
            var result = SyncExecuteCommand(command);
            return result is SuccessResult;
        }

        public bool TryGetText(string controlId, out string text)
        {
            text = null;
            var command = new GetTextCommand()
                              {
                                  AutomationIdentifier = CreateAutomationIdentifier(controlId)
                              };
            var result = SyncExecuteCommand(command);
            var successResult = result as SuccessResult;
            if (successResult == null)
                return false;

            text = successResult.ResultText;
            return true;
        }

        public bool SetText(string controlId, string text)
        {
            var command = new SetTextCommand()
                              {
                                  AutomationIdentifier = CreateAutomationIdentifier(controlId),
                                  Text = text
                              };

            var result = SyncExecuteCommand(command);
            var successResult = result as SuccessResult;
            return (successResult != null);
        }

        public bool Click(string controlId)
        {
            var command = new ClickCommand()
                              {
                                  AutomationIdentifier = CreateAutomationIdentifier(controlId)
                              };
            var result = SyncExecuteCommand(command);
            return result is SuccessResult;
        }

        public bool TakePicture(string controlId, out Bitmap bitmap)
        {
            var command = new TakePictureCommand()
                              {
                                  AutomationIdentifier = CreateAutomationIdentifier(controlId)
                              };
            var result = SyncExecuteCommand(command);
            var pictureResult = result as PictureResult;
            if (pictureResult == null)
            {
                // TODO - should log the result here really
                bitmap = null;
                return false;
            }

            var bytes = Convert.FromBase64String(pictureResult.EncodedPictureBytes);
            var memoryStream = new MemoryStream(bytes);
            bitmap = new Bitmap(memoryStream);

            return true;
        }

        public bool TakePicture(out Bitmap bitmap)
        {
            return TakePicture(null, out bitmap);
        }

        public RectangleF GetPosition(string controlId)
        {
            var command = new GetPositionCommand()
            {
                AutomationIdentifier = CreateAutomationIdentifier(controlId)
            };
            var result = SyncExecuteCommand(command);
            var positionResult = result as PositionResult;
            if (positionResult == null)
            {
                // TODO - should log the result here really
                return RectangleF.Empty;
            }

            return new RectangleF((float)positionResult.Left, (float)positionResult.Top, (float)positionResult.Width, (float)positionResult.Height);
        }

        public bool SetFocus(string controlId)
        {
            var command = new SetFocusCommand()
            {
                AutomationIdentifier = CreateAutomationIdentifier(controlId)
            };
            var result = SyncExecuteCommand(command);
            return result is SuccessResult;
        }

        #endregion // IPhoneAutomationController

        #region Private methods

        private AutomationIdentifier CreateAutomationIdentifier(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            return new AutomationIdentifier(id, _automationIdentification);
        }

        private ResultBase SyncExecuteCommand(CommandBase command)
        {
            ResultBase toReturn = null;
            var manualResetEvent = new ManualResetEvent(false);
            _serviceControl.AddCommand(command, (result) =>
                                    {
                                        toReturn = result;
                                        manualResetEvent.Set();
                                    });
            manualResetEvent.WaitOne();
            return toReturn;
        }

        #endregion
    }
}