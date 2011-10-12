// ----------------------------------------------------------------------
// <copyright file="AppLaunchingCommandLine.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.ComponentModel;
using Args;
using Args.Help;
using WindowsPhoneTestFramework.AutomationController.Interfaces;
using WindowsPhoneTestFramework.CommandLineHost;

namespace WindowsPhoneTestFramework.EmuHost
{
    [Description("emuhost - provides communications with the Windows Phone Emulator")]
    public class AppLaunchingCommandLine
    {
        [ArgsMemberSwitch("bind")]
        [DefaultValue("http://localhost:8085/phoneAutomation")]
        [Description("Url and port binding for the phone automation host")]
        public string Binding { get; set; }

        [ArgsMemberSwitch("id")]
        [DefaultValue(AutomationIdentification.TryEverything)]
        [Description("Mechanism for identifying phone controls - defaults to TryEverything")]
        public AutomationIdentification AutomationIdentification { get; set; }

        [ArgsMemberSwitch("pid")]
        [Description("The Product Id (Guid)")]
        public Guid ProductId { get; set; }

        [ArgsMemberSwitch("icon")]
        [DefaultValue("ApplicationIcon.png")]
        [Description("Path to application icon file")]
        public string IconPath { get; set; }

        [ArgsMemberSwitch("xap")]
        [Description("Path to application xap file")]
        public string XapPath { get; set; }

        [ArgsMemberSwitch("name")]
        [DefaultValue("Test Application")]
        [Description("Application name")]
        public string Name { get; set; }
    }
}