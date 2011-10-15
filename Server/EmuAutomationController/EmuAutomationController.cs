// ----------------------------------------------------------------------
// <copyright file="EmuAutomationController.cs" company="Expensify">
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
using System.Linq;
using System.Text;
using WindowsPhoneTestFramework.AutomationController;
using WindowsPhoneTestFramework.AutomationController.Interfaces;
using WindowsPhoneTestFramework.EmuAutomationController.Interfaces;
using WindowsPhoneTestFramework.EmuDriver;
using WindowsPhoneTestFramework.Utils;

namespace WindowsPhoneTestFramework.EmuAutomationController
{
    public class EmuAutomationController : TraceBase, IEmuAutomationController
    {
        private ServiceHostController _hostController;

        public IPhoneAutomationController PhoneAutomationController { get { return _hostController == null ? null : _hostController.Controller; } }
        public IDriver Driver { get; set; }
        public IDisplayInputController DisplayInputController { get { return Driver.DisplayInputController; } }

        public void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }

        public void Start(Uri bindingAddress = null, AutomationIdentification automationIdentification = AutomationIdentification.TryEverything)
        {
            if (_hostController != null)
                throw new InvalidOperationException("hostController already initialised");

            if (Driver != null)
                throw new InvalidOperationException("Driver already initialised");

            StartDriver();
            StartPhoneAutomationController(automationIdentification, bindingAddress);
        }

        private void StartDriver()
        {
            var driver = new EmulatorDriver();
            driver.Trace += (sender, args) => InvokeTrace(args);
            if (!driver.TryConnect())
                throw new EmuAutomationException("Unable to connect to emulator driver");
            Driver = driver;
        }

        private void StartPhoneAutomationController(AutomationIdentification automationIdentification, Uri bindingAddress)
        {
            try
            {
                var hostController = new ServiceHostController()
                {
                    AutomationIdentification = automationIdentification,
                };

                if (bindingAddress != null)
                    hostController.BindingAddress = bindingAddress;

                hostController.Trace += (sender, args) => InvokeTrace(args);

                hostController.Start();

                _hostController = hostController;
            }
            catch (Exception exception)
            {
                throw new EmuAutomationException("Failed to start PhoneAutomationController", exception);
            }
        }

        public void Stop()
        {
            if (_hostController != null)
            {
                _hostController.Stop();
                _hostController = null;
            }
            if (Driver != null)
            {
                Driver.ReleaseDeviceConnection();
                Driver = null;
            }
        }

    }
}
