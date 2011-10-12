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
        bool ConfirmAlive();
        bool WaitForText(string text);
        bool WaitForText(string text, TimeSpan timeout);
        bool LookForText(string text);
        bool TryGetText(string controlId, out string text);
        bool SetText(string controlId, string text);
        bool Click(string controlId);
        RectangleF GetPosition(string controlId);
        bool SetFocus(string controlId);
        bool TakePicture(string controlId, out Bitmap bitmap);
        bool TakePicture(out Bitmap bitmap);
    }
}