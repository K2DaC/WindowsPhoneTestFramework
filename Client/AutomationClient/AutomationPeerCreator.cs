// ----------------------------------------------------------------------
// <copyright file="AutomationPeerCreator.cs" company="Expensify">
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
using System.Net;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WindowsPhoneTestFramework.AutomationClient
{
    public static class AutomationPeerCreator
    {
        private static readonly Dictionary<Type, Func<UIElement, AutomationPeer>> Lookups = new Dictionary<Type, Func<UIElement, AutomationPeer>>();

        static AutomationPeerCreator()
        {
            Lookups.Add(typeof(Button), (element) => new ButtonAutomationPeer((Button)element));
            Lookups.Add(typeof(CheckBox), (element) => new CheckBoxAutomationPeer((CheckBox)element));
            Lookups.Add(typeof(HyperlinkButton), (element) => new HyperlinkButtonAutomationPeer((HyperlinkButton)element));
            Lookups.Add(typeof(Image), (element) => new ImageAutomationPeer((Image)element));
            Lookups.Add(typeof(ListBox), (element) => new ListBoxAutomationPeer((ListBox)element));
            Lookups.Add(typeof(PasswordBox), (element) => new PasswordBoxAutomationPeer((PasswordBox)element));
            Lookups.Add(typeof(ProgressBar), (element) => new ProgressBarAutomationPeer((ProgressBar)element));
            Lookups.Add(typeof(RadioButton), (element) => new RadioButtonAutomationPeer((RadioButton)element));
            Lookups.Add(typeof(Slider), (element) => new SliderAutomationPeer((Slider)element));
            Lookups.Add(typeof(TextBlock), (element) => new TextBlockAutomationPeer((TextBlock)element));
            Lookups.Add(typeof(TextBox), (element) => new TextBoxAutomationPeer((TextBox)element));

            // start here tomorrow (Tuesday)
            // - consider hard-coding the size of the emulator...
            // - consider sending messages direct from the code...
            // TODO... the only way to send back... and right... and click...
            // ... and other things...
            // ... is to automate clicks on the emulator skin!
            // go for it...
            // - orientation is relatively easy... except if keyboard is on!
            // - back button is relatively easy... except if orientation is needed...
            // - do as much as you can...
            // - findWindow and SendMessage...
            // http://msdn.microsoft.com/en-us/library/ff754352(v=VS.92).aspx - just use keyboard mappings.
        }

        public static void AddPeerFactory(Type t, Func<UIElement, AutomationPeer> factory)
        {
            lock (Lookups)
            {
                Lookups[t] = factory;
            }
        }

        public static AutomationPeer GetPeer(UIElement element)
        {
            if (element == null)
                return null;

            Func<UIElement, AutomationPeer> func;
            lock (Lookups)
            {
                if (!Lookups.TryGetValue(element.GetType(), out func))
                    return null;
            }

            if (func == null)
                return null;

            return func(element);
        }
    }

}
