// ----------------------------------------------------------------------
// <copyright file="InputStepDefinitions.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using NUnit.Framework;
using TechTalk.SpecFlow;
using WindowsPhoneTestFramework.EmuDriver;

namespace WindowsPhoneTestFramework.EmuSteps.StepDefinitions
{
    [Binding]
    public class InputStepDefinitions : EmuDefinitionBase
    {
        public InputStepDefinitions()
            : base()
        {
        }

        public InputStepDefinitions(IConfiguration configuration)
            : base(configuration)
        {
        }

        [Then(@"I swipe ""([^\""]*)""$")]
        public void ThenISwipe(string swipeDirection)
        {
            IGesture gesture = null;
            switch (swipeDirection)
            {
                case "LeftToRight":
                    gesture = SwipeGesture.LeftToRightPortrait();
                    break;
                case "RightToLeft":
                    gesture = SwipeGesture.RightToLeftPortrait();
                    break;
                default:
                    Assert.Fail("Unknown swipe " + swipeDirection);
                    break;
            }

            Emu.DisplayInputController.DoGesture(gesture);
        }

        [Then(@"I go back")]
        public void ThenIGoBack()
        {
            Emu.DisplayInputController.PressHardwareButton(WindowsPhoneHardwareButton.Back);
        }

        [Then(@"I longpress the backbutton")]
        public void ThenILongPressBack()
        {
            Emu.DisplayInputController.LongpressHardwareButton(WindowsPhoneHardwareButton.Back);
        }

        [Then(@"I go home")]
        public void ThenIGoHome()
        {
            Emu.DisplayInputController.PressHardwareButton(WindowsPhoneHardwareButton.Home);
        }

        [Then(@"I press hardware button ""([^\""]*)""$")]
        public void ThenIPressHardwareButton(string whichButton)
        {
            WindowsPhoneHardwareButton parsedButton;
            Assert.IsTrue(Enum.TryParse(whichButton, true, out parsedButton), "failed to parse button name " + whichButton);
            Emu.DisplayInputController.PressHardwareButton(parsedButton);
        }

        [Then(@"I click on the middle of the screen")]
        public void ThenITapOnPosition() 
        {
            IGesture gesture = TapGesture.TapOnPosition(240,400);
            Emu.DisplayInputController.DoGesture(gesture);
        }
        // /^I click on screen (\d+)% from the left and (\d+)% from the top$/

        // /^I press "([^\"]*)"$/

        // /^I press button number (\d+)$/

        // /^I press the "([^\"]*)" button$/

        // /^I press view with name "([^\"]*)"$/

        // /^I press image button number (\d+)$/

        // /^I press list item number (\d+)$/

        // /^I toggle checkbox number (\d+)$/



        // /^I enter "([^\"]*)" as "([^\"]*)"$/

        // /^I enter "([^\"]*)" into "([^\"]*)"$/

        // /^I enter "([^\"]*)" into input field number (\d+)$/

        // /^I clear "([^\"]*)"$/

        // /^I clear input field number (\d+)$/

        // /^I wait for "([^\"]*)" to appear$/

        // /^I wait for (\d+) seconds$/

        // /^I wait for dialog to close$/

        // /^I wait for progress$/

        // /^I wait for the "([^\"]*)" button to appear$/

        // /^I wait$/

        // /^I go back$/
    }
}