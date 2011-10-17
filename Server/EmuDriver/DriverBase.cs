// ----------------------------------------------------------------------
// <copyright file="DriverBase.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
//     
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.SmartDevice.Connectivity;
using WindowsPhoneTestFramework.Utils;

namespace WindowsPhoneTestFramework.EmuDriver
{
    // code inspired by http://justinangel.net/WindowsPhone7EmulatorAutomation - thanks Justin
    public class DriverBase : TraceBase, IDriver
    {
        public IDisplayInputController DisplayInputController { get; protected set; }

        public string WpSdkName
        {
            get;
            set;
        }

        public string WpDeviceNameBase { get; private set; }

        public bool TryConnect()
        {
            try
            {
                // to ensure a connection exists, just try to connect to the device
                var device = Device;
                if (Device == null)
                    throw new InvalidOperationException("device not connected!");

                return true;
            }
            catch (Exception exception)
            {
                InvokeTrace("problem during connection {0} - {1}", exception.GetType().FullName, exception.Message);
            }

            return false;
        }

        public DriverBase(string deviceNameBase)
        {
            WpSdkName = "Windows Phone 7";
            WpDeviceNameBase = deviceNameBase;
        }

        ~DriverBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // nothing special to do...
            }

            // release the device regardless of whether or not we are disposing
            ExceptionSafe.ExecuteConsoleWriteAnyException(
                    ReleaseDeviceConnection,
                    "exception shutting down COM driver");
        }

        private Device _device;

        private Device Device
        {
            get
            {
                // return an existing device
                if (_device != null)
                    return _device;

                // Get CoreCon DataStore
                InvokeTrace("creating datastore");
                var dsmgrObj = new DatastoreManager(1033);

                // Get the plaform
                InvokeTrace("getting platform");
                var platforms = dsmgrObj.GetPlatforms();
                InvokeTrace("{0} platform(s) found", platforms.Count);
                Platform phoneSdk = platforms.Single(p => p.Name == WpSdkName);
                InvokeTrace("platform '{0}' found", WpSdkName);

                // find the device
                InvokeTrace(string.Format("looking for device '{0}'", WpDeviceNameBase));
                var devices = phoneSdk.GetDevices();
                InvokeTrace(string.Format("{0} devices found", devices.Count));
                var device = phoneSdk.GetDevices().FirstOrDefault(d => d.Name == WpDeviceNameBase);

                if (device == null)
                {
                    InvokeTrace("device {0} not found - looking for similar matches", WpDeviceNameBase);
                    device = phoneSdk.GetDevices().FirstOrDefault(d => d.Name.StartsWith(WpDeviceNameBase));
                }

                if (device == null)
                {
                    InvokeTrace("device {0} not found - and no similar matches found", WpDeviceNameBase);
                    InvokeTrace("available devices were", WpDeviceNameBase);
                    foreach (var d in phoneSdk.GetDevices())
                        InvokeTrace("    " + d.Name);

                    // TODO - need a better exception than this!
                    throw new ApplicationException("Aborting - could not find device " + device);
                }

                // make the connection
                InvokeTrace("connecting to device...");
                device.Connect();
                InvokeTrace("device Connected...");

                _device = device;
                return _device;
            }
        }

        public void ReleaseDeviceConnection()
        {
            if (_device != null)
            {
                _device.Disconnect();
                _device = null;
            }
        }

        public bool IsInstalled(Guid productId)
        {
            return Device.IsApplicationInstalled(productId);
        }

        public InstallationResult ForceInstall(Guid productId, string applicationName, string applicationIconPath, string applicationXapPath)
        {
            return ForceInstall(productId, productId, applicationName, applicationIconPath, applicationXapPath);
        }

        public InstallationResult ForceInstall(Guid productId, Guid instanceId, string applicationName, string applicationIconPath, string applicationXapPath)
        {
            ForceUninstall(productId);
            return Install(productId, instanceId, applicationName, applicationIconPath, applicationXapPath);
        }

        public InstallationResult Install(Guid productId, string applicationName, string applicationIconPath, string applicationXapPath)
        {
            return Install(productId, productId, applicationName, applicationIconPath, applicationXapPath);
        }

        public InstallationResult Install(Guid productId, Guid instanceId, string applicationName, string applicationIconPath, string applicationXapPath)
        {
            if (IsInstalled(productId))
                return InstallationResult.AlreadyInstalled;

            // Install XAP
            InvokeTrace("installing xap to device...");

            Device.InstallApplication(
                productId,
                instanceId,
                applicationName,
                applicationIconPath,
                applicationXapPath);

            InvokeTrace("xap installed");
            return InstallationResult.Success;
        }

        public UninstallationResult ForceUninstall(Guid productId)
        {
            Stop(productId);
            return Uninstall(productId);
        }

        public UninstallationResult Uninstall(Guid productId)
        {
            if (!Device.IsApplicationInstalled(productId))
                return UninstallationResult.NotInstalled;

            InvokeTrace("uninstalling xap from device...");
            var app = SafeGetApplication(productId);
            app.Uninstall();
            InvokeTrace("xap uninstalled from  device");

            return UninstallationResult.Success;
        }

        public StopResult Stop(Guid productId)
        {
            InvokeTrace("ensuring application is stopped...");

            if (!Device.IsApplicationInstalled(productId))
            {
                InvokeTrace("application is not installed");
                return StopResult.NotInstalled;
            }

            var app = SafeGetApplication(productId);
            if (app == null)
            {
                InvokeTrace("failed to get application");
                return StopResult.NotInstalled; // really this is an error case - but just return NotInstalled for now
            }

            /*
             IsRunning is not supported on WP7
            if (!app.IsRunning())
            {
                InvokeTrace("application is not running");
                return StopResult.NotRunning;
            }
             */

            InvokeTrace("stopping application...");
            app.TerminateRunningInstances();
            InvokeTrace("application stopped");
            return StopResult.Success;
        }

        private RemoteApplication SafeGetApplication(Guid productId)
        {
            try
            {
                var app = Device.GetApplication(productId);
                if (app == null)
                    throw new InvalidOperationException("Unexpected return - null app - sorry");
                return app;
            }
            catch (Exception exception)
            {
                InvokeTrace("Exception seen {0} - {1}", exception.GetType().FullName, exception.Message);
                return null;
            }
        }

        public StartResult Start(Guid productId)
        {
            InvokeTrace("launching app...");
            var app = SafeGetApplication(productId);
            if (app == null)
            {
                InvokeTrace("not installed");
                return StartResult.NotInstalled;
            }

            /*
             app.IsRunning is not supported for WP7
            if (app.IsRunning())
            {
                InvokeTrace("already running");
                return StartResult.AlreadyRunning;
            }
             */

            app.Launch();
            InvokeTrace("app launched");
            return StartResult.Success;
        }

        public StartResult ForceStart(Guid productId)
        {
            Stop(productId);
            return Start(productId);
        }
    }
}