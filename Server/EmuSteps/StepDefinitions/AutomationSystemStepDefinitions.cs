// ----------------------------------------------------------------------
// <copyright file="AutomationSystemStepDefinitions.cs" company="Expensify">
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
using System.Drawing.Imaging;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace WindowsPhoneTestFramework.EmuSteps.StepDefinitions
{
    [Binding]
    public class AutomationSystemStepDefinitions : EmuDefinitionBase
    {
        public AutomationSystemStepDefinitions()
        {
        }

        public AutomationSystemStepDefinitions(IConfiguration configuration)
            : base(configuration)
        {
        }

        [Then(@"take a picture$")]
        public void ThenTakeAPicture()
        {
            var fileName = StepFlowContextHelpers.GetNextPictureName();
            Bitmap picture;
            Assert.IsTrue(Emu.PhoneAutomationController.TakePicture(out picture), "Failed to get screenshot");
            picture.Save(fileName, ImageFormat.Png);

            StepFlowOutputHelpers.Write("Picture saved to _startEmuShot_{0}_endEmuShot_", fileName);
        }
    }
}