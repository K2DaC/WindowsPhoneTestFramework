// ----------------------------------------------------------------------
// <copyright file="AutomationElementCommandBase.cs" company="Expensify">
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
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;

namespace WindowsPhoneTestFramework.AutomationClient.Remote
{
    public partial class AutomationElementCommandBase
    {
        protected bool AutomationIdIsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(AutomationIdentifier.AutomationName)
                       && string.IsNullOrEmpty(AutomationIdentifier.ElementName)
                       && string.IsNullOrEmpty(AutomationIdentifier.DisplayedText);
            }
        }

        protected UIElement GetUIElement(bool sendNotFoundResultOnFail = true)
        {
            var element = AutomationElementFinder.FindElement(AutomationIdentifier);
            if (element == null)
            {
                if (sendNotFoundResultOnFail)
                    SendNotFoundResult();
                return null;
            }

            return element;
        }

        protected FrameworkElement GetFrameworkElement(bool sendNotFoundResultOnFail = true)
        {
            var element = AutomationElementFinder.FindElement(AutomationIdentifier) as FrameworkElement;
            if (element == null)
            {
                if (sendNotFoundResultOnFail)
                    SendNotFoundResult();
                return null;
            }

            return element;
        }

        protected FrameworkElement GetApplicationRootVisual()
        {
            var rootVisual = (PhoneApplicationFrame)Application.Current.RootVisual;
            if (rootVisual == null)
                return null;

            return rootVisual;
        }
    }
}