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
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace WindowsPhoneTestFramework.AutomationClient.Remote
{
    public partial class ClickCommand
    {
        protected override void DoImpl()
        {
            var element = AutomationElementFinder.FindElement(AutomationIdentifier);
            if (element == null)
            {
                SendNotFoundResult();
                return;
            }

            // automate the click?
            var peer = AutomationPeerCreator.GetPeer(element);
            if (peer == null)
                throw new TestAutomationException("No automation peer found for " + element.GetType().FullName);

            var pattern = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            if (pattern == null)
                throw new TestAutomationException("No invoke pattern found for " + element.GetType().FullName);

            try
            {
                pattern.Invoke();
            }
            catch (Exception exception)
            {
                throw new TestAutomationException("Exception while invoking pattern", exception);
            }

            SendSuccessResult();
        }
    }
}