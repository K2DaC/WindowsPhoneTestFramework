// ----------------------------------------------------------------------
// <copyright file="IDriver.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using WindowsPhoneTestFramework.Utils;

namespace WindowsPhoneTestFramework.EmuDriver
{
    public interface IDriver : IDisposable, ITrace
    {
        IDisplayInputController DisplayInputController { get; }

        bool TryConnect();
        void ReleaseDeviceConnection();

        bool IsInstalled(Guid productId);
        
        InstallationResult ForceInstall(Guid productId, string applicationName, string applicationIconPath, string applicationXapPath);
        InstallationResult ForceInstall(Guid productId, Guid instanceId, string applicationName, string applicationIconPath, string applicationXapPath);
        InstallationResult Install(Guid productId, string applicationName, string applicationIconPath, string applicationXapPath);
        InstallationResult Install(Guid productId, Guid instanceId, string applicationName, string applicationIconPath, string applicationXapPath);

        UninstallationResult ForceUninstall(Guid productId);
        UninstallationResult Uninstall(Guid productId);
        
        StopResult Stop(Guid productId);
        StartResult Start(Guid productId);
        StartResult ForceStart(Guid productId);
    }
}