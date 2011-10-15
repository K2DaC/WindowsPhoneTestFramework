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