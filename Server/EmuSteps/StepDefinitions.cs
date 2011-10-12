// ----------------------------------------------------------------------
// <copyright file="StepDefinitions.cs" company="Expensify">
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using TechTalk.SpecFlow;
using WindowsPhoneTestFramework.EmuAutomationController;
using WindowsPhoneTestFramework.EmuDriver;

namespace WindowsPhoneTestFramework.EmuSteps
{
    [Binding]
    public class StepDefinitions : EmuDefinitionBase
    {
        public StepDefinitions()
            : base()
        {
        }

        public StepDefinitions(IConfiguration configuration) : base(configuration)
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
            Assert.That(start == StartResult.Success || start == StartResult.AlreadyRunning);

            var ping = Emu.PhoneAutomationController.ConfirmAlive();
            Assert.IsTrue(ping);
        }

        [Then(@"I wait for ""([^\""]*)"" to appear$")]
        public void ThenIWaitForTextToAppear(string text)
        {
            var result = Emu.PhoneAutomationController.WaitForText(text);
            Assert.IsTrue(result);
        }

        [Then(@"I press the ""([^\""]*)"" button$")]
        public void ThenIPressTheNamedButton(string named)
        {
            var result = Emu.PhoneAutomationController.Click(named);
            Assert.IsTrue(result);
        }

        [Then(@"take a picture$")]
        public void ThenTakeAPicture()
        {
            var fileName = StepFlowContextHelpers.GetNextPictureName();
            Bitmap picture;
            Emu.PhoneAutomationController.TakePicture(out picture);
            picture.Save(fileName, ImageFormat.Png);

            Console.WriteLine(string.Format("-> Picture saved to [__picture:{0}]</a>", fileName));
        }

        [Then(@"I see the ""([^\""]*)"" field contains ""([^\""]*)""$")]
        public void ThenISeeTheNamedFieldWithContent(string namedField, string expectedContents)
        {
            string actualContents;
            var result = Emu.PhoneAutomationController.TryGetText(namedField, out actualContents);
            Assert.IsTrue(result);
            Assert.AreEqual(expectedContents, actualContents);
        }

        // TODO - this doesn't quite match the other platforms... as this replaces the contents...
        [Then(@"I enter ""([^\""]*)"" into ""([^\""]*)""$")]
        public void ThenIEnterTextIntoTheNamedField(string contents, string namedField)
        {
            var result = Emu.PhoneAutomationController.SetText(namedField, contents);
            Assert.IsTrue(result);
        }

        [Then(@"I see ""([^\""]*)""$")]
        public void ThenISeeText(string contents)
        {
            var result = Emu.PhoneAutomationController.LookForText(contents);
            Assert.IsTrue(result);

            var position = Emu.PhoneAutomationController.GetPosition(contents);
            var phoneOrientation = Emu.DisplayInputController.GuessOrientation();
            Assert.True(position.IsVisible(phoneOrientation));
        }

        [Then(@"I don't see ""([^\""]*)""$")]
        public void ThenIDontSeeText(string contents)
        {
            var result = Emu.PhoneAutomationController.LookForText(contents);
            if (!result)
                return;

            var position = Emu.PhoneAutomationController.GetPosition(contents);
            var phoneOrientation = Emu.DisplayInputController.GuessOrientation();
            Assert.False(position.IsVisible(phoneOrientation));
        }

        [Then(@"I see ""([^\""]*)"" is left of ""([^\""]*)""$")]
        public void ThenISeeTextOnTheLeftOf(string leftControlId, string rightControlId)
        {
            var leftPosition = Emu.PhoneAutomationController.GetPosition(leftControlId);
            var rightPosition = Emu.PhoneAutomationController.GetPosition(rightControlId);
            Assert.Less(leftPosition.X, rightPosition.X);
            Assert.LessOrEqual(leftPosition.X + leftPosition.Width, rightPosition.X);
        }

        [Then(@"I wait (\d+) seconds$")]
        public void ThenIWait(int countSeconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(countSeconds));
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
