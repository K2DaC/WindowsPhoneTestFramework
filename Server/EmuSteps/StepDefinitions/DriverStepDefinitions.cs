// ----------------------------------------------------------------------
// <copyright file="DriverStepDefinitions.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using NUnit.Framework;
using TechTalk.SpecFlow;
using WindowsPhoneTestFramework.EmuDriver;

namespace WindowsPhoneTestFramework.EmuSteps.StepDefinitions
{
    [Binding]
    public class DriverStepDefinitions : EmuDefinitionBase
    {
        public DriverStepDefinitions()
            : base()
        {
        }

        public DriverStepDefinitions(IConfiguration configuration)
            : base(configuration)
        {
        }

        [Given(@"my app is uninstalled$")]
        public void GivenMyAppIsUninstalled()
        {
            var result = Emu.Driver.ForceUninstall(Configuration.ProductId);
            Assert.That(result == UninstallationResult.NotInstalled || result == UninstallationResult.Success);
        }

        [Given(@"my app is installed$")]
        public void GivenMyAppIsInstalled()
        {
            var result = Emu.Driver.Install(Configuration.ProductId, Configuration.ApplicationName, Configuration.IconPath, Configuration.XapPath);
            Assert.That(result == InstallationResult.AlreadyInstalled || result == InstallationResult.Success);
        }

        [Given(@"my app is not running$")]
        public void GivenMyAppIsNotRunning()
        {
            var result = Emu.Driver.Stop(Configuration.ProductId);
            Assert.That(result == StopResult.Success || result == StopResult.NotRunning || result == StopResult.NotInstalled);
        }

        [Given(@"my app is running$")]
        public void GivenMyAppIsRunning()
        {
            var start = Emu.Driver.Start(Configuration.ProductId);
            Assert.That(start == StartResult.Success || start == StartResult.AlreadyRunning, "Failed to start the device - response was {0}", start);

            var ping = Emu.PhoneAutomationController.WaitIsAlive();
            Assert.IsTrue(ping, "App started, but failed to ping the app");
        }

        [Then(@"my app is running")]
        public void ThenMyAppIsAlive()
        {
            var ping = Emu.PhoneAutomationController.LookIsAlive();
            Assert.IsTrue(ping, "App not alive - failed to ping");
        }
    }
}
