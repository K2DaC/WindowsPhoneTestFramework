// ----------------------------------------------------------------------
// <copyright file="StepFlowContextHelpers.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.IO;
using NUnit.Framework;
using TechTalk.SpecFlow;
using WindowsPhoneTestFramework.EmuAutomationController.Interfaces;

namespace WindowsPhoneTestFramework.EmuSteps
{
    public static class StepFlowContextHelpers
    {
        private const string EmuShotPrefix = "_EmuShot_";
        private const string EmuControllerKey = "Emu.EmuAutomationController";
        private const string EmuPictureIndexKey = "Emu.PictureIndex";

        public static IEmuAutomationController GetEmuAutomationController(ScenarioContext context, IConfiguration configuration)
        {
            Assert.That(context != null);
            Assert.That(configuration != null);

            lock (context)
            {
                IEmuAutomationController emu = null;
                if (context.TryGetValue(EmuControllerKey, out emu))
                    return emu;

                emu = new EmuAutomationController.EmuAutomationController();
                emu.Trace += (sender, args) => StepFlowOutputHelpers.Write(args.Message);
                emu.Start(
                    configuration.BindingAddress == null ? null : new Uri(configuration.BindingAddress),
                    configuration.AutomationIdentification);
                if (emu.DisplayInputController != null)
                    emu.DisplayInputController.EnsureWindowIsInForeground();
                context[EmuControllerKey] = emu;
                return emu;
            }
        }

        public static string GetNextPictureName()
        {
            return GetNextPictureName(FeatureContext.Current, ScenarioContext.Current);
        }

        public static string GetNextPictureName(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            Assert.NotNull(featureContext);
            Assert.NotNull(scenarioContext);

            lock (featureContext)
            lock (scenarioContext)
            {
                object objectPictureIndex;
                if (!scenarioContext.TryGetValue(EmuPictureIndexKey, out objectPictureIndex))
                    objectPictureIndex = 0;
                var pictureIndex = (int) objectPictureIndex;
                scenarioContext[EmuPictureIndexKey] = ++pictureIndex;

                var fileName = String.Format("{0}{1}_{2}_{3}.png",
                                                EmuShotPrefix,
                                                featureContext.FeatureInfo.Title,
                                                scenarioContext.ScenarioInfo.Title,
                                                pictureIndex);

                foreach (var ch in Path.GetInvalidFileNameChars())
                    fileName = fileName.Replace(ch, '_');

                return fileName;
            }
        }

        public static void DisposeOfEmu(ScenarioContext context)
        {
            Assert.That(context != null);

            lock (context)
            {
                IEmuAutomationController emu = null;
                if (!context.TryGetValue(EmuControllerKey, out emu))
                    return;
                
                if (emu.DisplayInputController != null)
                    emu.DisplayInputController.ReleaseWindowFromForeground();

                emu.Dispose();
                context.Remove(EmuControllerKey);
            }
        }
    }
}