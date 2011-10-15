// ----------------------------------------------------------------------
// <copyright file="IPhoneAutomationController.cs" company="Expensify">
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
using WindowsPhoneTestFramework.AutomationController.Commands;
using WindowsPhoneTestFramework.AutomationController.Results;
using WindowsPhoneTestFramework.Utils;

namespace WindowsPhoneTestFramework.AutomationController.Interfaces
{
    public interface IPhoneAutomationController
    {
        bool WaitIsAlive();
        bool LookIsAlive();
        bool WaitForControlOrText(string textOrControlId);
        bool WaitForControlOrText(string textOrControlId, TimeSpan timeout);
        bool WaitForControl(string controlId);
        bool WaitForControl(string controlId, TimeSpan timeout);
        bool LookForControl(string controlId);
        bool WaitForText(string text);
        bool WaitForText(string text, TimeSpan timeout);
        bool LookForText(string text);
        bool TryGetTextFromControl(string controlId, out string text);
        bool SetTextOnControl(string controlId, string text);
        bool InvokeControlTapAction(string controlId);
        RectangleF GetPositionOfControlOrText(string textOrControlId);
        RectangleF GetPositionOfControl(string controlId);
        RectangleF GetPositionOfText(string text);
        bool SetFocus(string controlId);
        bool TakePicture(string controlId, out Bitmap bitmap);
        bool TakePicture(out Bitmap bitmap);
    }
}