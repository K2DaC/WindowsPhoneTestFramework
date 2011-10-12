// ----------------------------------------------------------------------
// <copyright file="IEmuAutomationController.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using WindowsInput.Native;
using WindowsPhoneTestFramework.AutomationController.Interfaces;
using WindowsPhoneTestFramework.EmuDriver;
using WindowsPhoneTestFramework.Utils;

namespace WindowsPhoneTestFramework.EmuAutomationController.Interfaces
{
    public interface IEmuAutomationController : IDisposable, ITrace
    {
        void Start(Uri bindingAddress = null, AutomationIdentification automationIdentification = AutomationIdentification.TryEverything);
        void Stop();

        IPhoneAutomationController PhoneAutomationController { get; }
        IDriver Driver { get; }
        IDisplayInputController DisplayInputController {get;}
    }
}
