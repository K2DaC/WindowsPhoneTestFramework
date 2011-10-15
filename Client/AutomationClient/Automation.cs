// ----------------------------------------------------------------------
// <copyright file="Automation.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation.Peers;
using WindowsPhoneTestFramework.AutomationClient.Remote;

namespace WindowsPhoneTestFramework.AutomationClient
{
    public class Automation
    {
        public static readonly Automation Instance = new Automation();

        private bool _initialised;

        public void Initialise(string remoteUrl = "")
        {
            if (_initialised)
                return;

            if (Application.Current.RootVisual == null)
                throw new TestAutomationException("Automation client initialised too early");

            var configuration = new Configuration()
                                    {
                                        RemoteUrl =
                                            string.IsNullOrEmpty(remoteUrl) ? Configuration.DefaultRemoteUrl : remoteUrl,
                                        UiDispatcher = Application.Current.RootVisual.Dispatcher
                                    };

            var automationClient = new AutomationClient(configuration);
            automationClient.Start();
            Application.Current.Exit += (sender, args) => automationClient.Stop();

            _initialised = true;
        }

        public void AddStringPropertyNameForTextLookup(string propertyName)
        {
            AutomationElementFinder.StringPropertyNamesToTestForText.Add(propertyName);
        }

        public void AddObjectPropertyNameForTextLookup(string propertyName)
        {
            AutomationElementFinder.ObjectPropertyNamesToTestForText.Add(propertyName);
        }

        public void AddAutomationPeerHandlerForTapAction(Func<AutomationPeer, bool> handler)
        {
            InvokeControlTapActionCommand.PatternTesters.Insert(0, handler);
        }

        public void AddUIElementHandlerForTapAction(Func<UIElement, bool> handler)
        {
            InvokeControlTapActionCommand.UIElementTesters.Insert(0, handler);
        }
    }
}