// ----------------------------------------------------------------------
// <copyright file="AutomationIdentifier.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

namespace WindowsPhoneTestFramework.AutomationController.Interfaces
{
    public class AutomationIdentifier
    {
        public string AutomationName { get; set; }
        public string ElementName { get; set; }
        public string DisplayedText { get; set; }

        public AutomationIdentifier()
        {
        }

        public AutomationIdentifier(string text, AutomationIdentification automationIdentification)
        {
            Fill(text, automationIdentification);
        }

        public void Fill(string text, AutomationIdentification automationIdentification)
        {
            AutomationName =
                automationIdentification.HasFlag(AutomationIdentification.TryAutomationName)
                    ? text
                    : null;

            ElementName =
                automationIdentification.HasFlag(AutomationIdentification.TryElementName)
                    ? text
                    : null;

            DisplayedText =
                automationIdentification.HasFlag(AutomationIdentification.TryDisplayedText)
                    ? text
                    : null;
        }
    }
}