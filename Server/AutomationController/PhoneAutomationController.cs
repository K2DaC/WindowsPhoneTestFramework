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

        public bool WaitIsAlive()
        {
            return WaitForTestSuccess(LookIsAlive, Constants.DefaultConfirmAliveTimeout);
        }

        public bool LookIsAlive()
        {
            var command = new ConfirmAliveCommand();
            var result = SyncExecuteCommand(command);
            return result is SuccessResult;
        }

        public bool LookForText(string text)
        {
            var command = new LookForTextCommand() { Text = text };
            var result = SyncExecuteCommand(command);
            return result is SuccessResult;
        }

        public bool WaitForText(string text)
        {
            return WaitForText(text, Constants.DefaultWaitForClientAppActionTimeout);
        }

        public bool WaitForText(string text, TimeSpan timeout)
        {
            return WaitForTestSuccess(() => LookForText(text), timeout);
        }

        public bool WaitForControlOrText(string textOrControlId)
        {
            return WaitForControlOrText(textOrControlId, Constants.DefaultWaitForClientAppActionTimeout);
        }

        public bool WaitForControlOrText(string textOrControlId, TimeSpan timeout)
        {
            return WaitForTestSuccess(() => LookForControlOrText(textOrControlId), timeout);
        }

        public bool LookForControlOrText(string textOrControlId)
        {
            var automationIdentifier = CreateControlOrTextAutomationIdentifier(textOrControlId);
            return LookForAutomationIdentifer(automationIdentifier);
        }

        public bool WaitForControl(string controlId)
        {
            return WaitForControl(controlId, Constants.DefaultWaitForClientAppActionTimeout);
        }

        public bool WaitForControl(string controlId, TimeSpan timeout)
        {
            return WaitForTestSuccess(() => LookForControl(controlId), timeout);
        }

        public bool LookForControl(string controlId)
        {
            var automationIdentifier = CreateAutomationIdentifier(controlId);
            return LookForAutomationIdentifer(automationIdentifier);
        }
        
        public bool TryGetTextFromControl(string controlId, out string text)
        {
            return TryGetTextFromAutomationIdentifier(CreateAutomationIdentifier(controlId), out text);
        }

        public bool SetTextOnControl(string controlId, string text)
        {
            return SetTextOnAutomationIdentification(CreateAutomationIdentifier(controlId), text);
        }

        public bool InvokeControlTapAction(string controlId)
        {
            var automationIdentifier = CreateAutomationIdentifier(controlId);
            return InvokeControlTapActionAutomationIdentifer(automationIdentifier);
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

        public RectangleF GetPositionOfText(string text)
        {
            return GetPositionOfAutomationIdentifier(CreateTextOnlyAutomationIdentifier(text));
        }

        public RectangleF GetPositionOfControl(string controlId)
        {
            return GetPositionOfAutomationIdentifier(CreateAutomationIdentifier(controlId));
        }

        public RectangleF GetPositionOfControlOrText(string textOrControlId)
        {
            return GetPositionOfAutomationIdentifier(CreateControlOrTextAutomationIdentifier(textOrControlId));
        }

        private RectangleF GetPositionOfAutomationIdentifier(AutomationIdentifier automationIdentifier)
        {
            var command = new GetPositionCommand()
            {
                AutomationIdentifier = automationIdentifier
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

        private static AutomationIdentifier CreateTextOnlyAutomationIdentifier(string text)
        {
            return new AutomationIdentifier()
                       {
                           DisplayedText = text
                       };
        }

        private AutomationIdentifier CreateControlOrTextAutomationIdentifier(string textOrControlId)
        {
            var toReturn = CreateAutomationIdentifier(textOrControlId);
            toReturn.DisplayedText = textOrControlId;
            return toReturn;
        }
            
        private AutomationIdentifier CreateAutomationIdentifier(string id)
        {
            return new AutomationIdentifier(id, _automationIdentification);
        }

        private bool LookForAutomationIdentifer(AutomationIdentifier automationIdentifier)
        {
            var command = new GetPositionCommand() { AutomationIdentifier = automationIdentifier, ReturnEmptyIfNotVisible = true };
            var result = SyncExecuteCommand(command) as PositionResult;
            if (result == null)
                return false;

            // check that position is not empty
            return (result.Width + result.Height > 0.0);
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

        private static bool WaitForTestSuccess(Func<bool> test, TimeSpan timeout)
        {
            var start = DateTime.UtcNow;

            do
            {
                if (test())
                {
                    return true;
                }
            } while (DateTime.UtcNow - start < timeout);

            return false;
        }

        private bool InvokeControlTapActionAutomationIdentifer(AutomationIdentifier automationIdentifier)
        {
            var command = new InvokeControlTapActionCommand()
            {
                AutomationIdentifier = automationIdentifier
            };
            var result = SyncExecuteCommand(command);
            return result is SuccessResult;
        }

        private bool SetTextOnAutomationIdentification(AutomationIdentifier automationIdentifier, string text)
        {
            var command = new SetTextCommand()
            {
                AutomationIdentifier = automationIdentifier,
                Text = text
            };

            var result = SyncExecuteCommand(command);
            var successResult = result as SuccessResult;
            return (successResult != null);
        }

        private bool TryGetTextFromAutomationIdentifier(AutomationIdentifier automationIdentifier, out string text)
        {
            text = null;
            var command = new GetTextCommand()
            {
                AutomationIdentifier = automationIdentifier
            };
            var result = SyncExecuteCommand(command);
            var successResult = result as SuccessResult;
            if (successResult == null)
                return false;

            text = successResult.ResultText;
            return true;
        }

        #endregion
    }
}