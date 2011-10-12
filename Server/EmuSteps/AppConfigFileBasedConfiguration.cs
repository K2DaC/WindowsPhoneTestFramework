// ----------------------------------------------------------------------
// <copyright file="AppConfigFileBasedConfiguration.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Configuration;
using WindowsPhoneTestFramework.AutomationController.Interfaces;

namespace WindowsPhoneTestFramework.EmuSteps
{
    public class AppConfigFileBasedConfiguration : IConfiguration
    {
        public string BindingAddress { get; set; }
        public AutomationIdentification AutomationIdentification { get; set; }
        public Guid ProductId { get; set; }
        public string ApplicationName { get; set; }
        public string IconPath { get; set; }
        public string XapPath { get; set; }

        public AppConfigFileBasedConfiguration()
        {
            BindingAddress = ConfigurationManager.AppSettings["EmuSteps.BindingAddress"];

            AutomationIdentification automationIdentification;
            if (Enum.TryParse(ConfigurationManager.AppSettings["EmuSteps.AutomationIdentification"], true, out automationIdentification))
                AutomationIdentification = automationIdentification;
            else
                AutomationIdentification = AutomationIdentification.TryEverything;

            Guid productId;
            if (Guid.TryParse(ConfigurationManager.AppSettings["EmuSteps.ProductId"], out productId))
                ProductId = productId;
            else
                ProductId = Guid.Empty;

            IconPath = ConfigurationManager.AppSettings["EmuSteps.IconPath"];
            ApplicationName = ConfigurationManager.AppSettings["EmuSteps.ApplicationName"];
            XapPath = ConfigurationManager.AppSettings["EmuSteps.XapPath"];
        }
    }
}