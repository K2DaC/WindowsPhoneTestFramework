// ----------------------------------------------------------------------
// <copyright file="ClickCommand.cs" company="Expensify">
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
using System.Linq;
using System.Reflection;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace WindowsPhoneTestFramework.AutomationClient.Remote
{
    public partial class ClickCommand
    {
        private static List<Func<AutomationPeer, bool>> PatternTesters;

        static ClickCommand()
        {
            var providers =
                from method in typeof (ClickCommand).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                where method.ReturnType == typeof (bool)
                      && method.GetParameters().Length == 1
                      && method.GetParameters()[0].ParameterType == typeof (AutomationPeer)
                select new Func<AutomationPeer,bool>((AutomationPeer peer) => (bool)method.Invoke(null, new object[] {peer}));

            PatternTesters = new List<Func<AutomationPeer, bool>>(providers);
        }
        
        protected override void DoImpl()
        {
            var element = AutomationElementFinder.FindElement(AutomationIdentifier);
            if (element == null)
            {
                SendNotFoundResult();
                return;
            }

            // automate the click
            var peer = AutomationPeerCreator.GetPeer(element);
            if (peer == null)
                throw new TestAutomationException("No automation peer found for " + element.GetType().FullName);

            foreach (var patternTester in PatternTesters)
            {
                if (patternTester(peer))
                {
                    SendSuccessResult();
                    return;
                }
            }

            throw new TestAutomationException("No invoke pattern found for " + element.GetType().FullName);
        }

        private static bool TryInvokePatternAutomation(AutomationPeer peer)
        {
            var pattern = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            if (pattern == null)
                return false;

            try
            {
                pattern.Invoke();
            }
            catch (Exception exception)
            {
                throw new TestAutomationException("Exception while invoking pattern", exception);
            }

            return true;
        }

        private static bool TryTogglePatternAutomation(AutomationPeer peer)
        {
            var pattern = peer.GetPattern(PatternInterface.Toggle) as IToggleProvider;
            if (pattern == null)
                return false;

            try
            {
                pattern.Toggle();
            }
            catch (Exception exception)
            {
                throw new TestAutomationException("Exception while invoking pattern", exception);
            }

            return true;
        }
    }
}