// ----------------------------------------------------------------------
// <copyright file="AutomationWaitStepDefinitions.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Threading;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace WindowsPhoneTestFramework.EmuSteps.StepDefinitions
{
    [Binding]
    public class AutomationWaitStepDefinitions : EmuDefinitionBase
    {
        public AutomationWaitStepDefinitions()
            : base()
        {
        }

        public AutomationWaitStepDefinitions(IConfiguration configuration)
            : base(configuration)
        {
        }

        [Then(@"I wait for the control ""([^\""]*)"" to appear$")]
        public void ThenIWaitForControlToAppear(string controlId)
        {
            var result = Emu.PhoneAutomationController.WaitForControl(controlId);
            Assert.IsTrue(result, "Failed to wait for control '{0}'", controlId);
        }

        [Then(@"I wait for the text ""([^\""]*)"" to appear$")]
        public void ThenIWaitForTextToAppear(string text)
        {
            var result = Emu.PhoneAutomationController.WaitForText(text);
            Assert.IsTrue(result, "Failed to wait for text '{0}'", text);
        }

        [Then(@"I wait (\d+\.?\d*) seconds$")]
        public void ThenIWait(double countSeconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(countSeconds));
        }

        [Then(@"I wait 1 second$")]
        public void ThenIWait()
        {
            ThenIWait(1);
        }
    }
}