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
        protected UIElement GetUIElement()
        {
            var element = AutomationElementFinder.FindElement(AutomationIdentifier);
            if (element == null)
            {
                SendNotFoundResult();
                return null;
            }

            return element;
        }

        protected FrameworkElement GetFrameworkElement()
        {
            var element = AutomationElementFinder.FindElement(AutomationIdentifier) as FrameworkElement;
            if (element == null)
            {
                SendNotFoundResult();
                return null;
            }

            return element;
        }
    }
}