// ----------------------------------------------------------------------
// <copyright file="AutomationElementFinder.cs" company="Expensify">
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
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WindowsPhoneTestFramework.AutomationClient.Remote;

namespace WindowsPhoneTestFramework.AutomationClient
{
    public static class AutomationElementFinder
    {
        public readonly static List<string> StringPropertyNamesToTestForText = new List<string>()
                                                                       {
                                                                            "Text",
                                                                            "Password",
                                                                            "Message"
                                                                       };
        public readonly static List<string> ObjectPropertyNamesToTestForText = new List<string>()
                                                                       {
                                                                            "Content"    
                                                                       };

        public static UIElement FindElement(AutomationIdentifier identifier)
        {
            if (!string.IsNullOrEmpty(identifier.AutomationName))
            {
                var candidate = FindElementByAutomationTag(identifier.AutomationName);
                if (candidate != null)
                    return candidate;
            }

            if (!string.IsNullOrEmpty(identifier.ElementName))
            {
                var candidate = FindElementByElementName(identifier.ElementName);
                if (candidate != null)
                    return candidate;
            }

            if (!string.IsNullOrEmpty(identifier.DisplayedText))
            {
                var candidate = FindElementByDisplayedText(identifier.DisplayedText);
                if (candidate != null)
                    return candidate;
            }
            // not found - return null :/
            return null;
        }

        public static T GetElementProperty<T>(object target, string name)
        {
            var propertyInfo = GetPropertyInfo(target, name);

            if (propertyInfo == null)
                return default(T);

            if (!typeof(T).IsAssignableFrom(propertyInfo.PropertyType))
                return default(T);

            return (T)propertyInfo.GetValue(target, new object[0]);
        }

        public static bool SetElementProperty<T>(object target, string name, T value)
        {
            var propertyInfo = GetPropertyInfo(target, name);

            if (propertyInfo == null)
                return false;

            if (!typeof(T).IsAssignableFrom(propertyInfo.PropertyType))
                return false;

            propertyInfo.SetValue(target, value, new object[0]);
            return true;
        }

        private static PropertyInfo GetPropertyInfo(object target, string name)
        {
            var targetType = target.GetType();
            return targetType.GetProperty(name,
                                          BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance |
                                          BindingFlags.FlattenHierarchy);
        }

        private static UIElement SearchFrameworkElementTreeFor(UIElement parentElement, Func<UIElement, bool> elementTest)
        {
            if (parentElement == null)
                return null;

            if (elementTest(parentElement))
                return parentElement;

            var childrenCount = VisualTreeHelper.GetChildrenCount(parentElement);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parentElement, i) as FrameworkElement;
                if (child != null)
                {
                    var candidate = SearchFrameworkElementTreeFor(child, elementTest);
                    if (candidate != null)
                        return candidate;
                }
            }

            return null;
        }

        public static UIElement FindElementByAutomationTag(string automationName)
        {
            var rootVisual = Application.Current.RootVisual;
            return SearchFrameworkElementTreeFor(rootVisual, (element) =>
            {
                var frameworkElement = element as FrameworkElement;
                if (frameworkElement == null)
                    return false;
                var tag = frameworkElement.Tag;
                if (tag == null)
                    return false;

                var tagString = tag.ToString();
                if (tagString.Length <= "auto:".Length || !tagString.StartsWith("auto:"))
                    return false;

                if (tagString.Substring("auto:".Length) != automationName)
                    return false;

                return true;
            });
        }

        public static UIElement FindElementByElementName(string elementName)
        {
            var rootVisual = Application.Current.RootVisual;
            return SearchFrameworkElementTreeFor(rootVisual, (element) =>
            {
                var frameworkElement = element as FrameworkElement;
                if (frameworkElement == null)
                    return false;

                var name = frameworkElement.Name;
                if (string.IsNullOrEmpty(name))
                    return false;

                if (name != elementName)
                    return false;

                return true;
            });
        }

        public static UIElement FindElementByDisplayedText(string displayedText)
        {
            var rootVisual = Application.Current.RootVisual;
            return SearchFrameworkElementTreeFor(rootVisual, (element) =>
            {
                var frameworkElement = element as FrameworkElement;
                if (frameworkElement == null)
                    return false;

                if (frameworkElement.Visibility == Visibility.Collapsed)
                    return false;

                if (frameworkElement.Opacity == 0.0)
                    return false;

                foreach (var textName in StringPropertyNamesToTestForText)
                {
                    var stringPropertyValue = GetElementProperty<string>(frameworkElement, textName);
                    if (!string.IsNullOrEmpty(stringPropertyValue))
                        if (stringPropertyValue == displayedText)
                            return true;
                }

                foreach (var objectName in ObjectPropertyNamesToTestForText)
                {
                    var objectPropertyValue = GetElementProperty<object>(frameworkElement, objectName);
                    if (objectPropertyValue != null
                        && objectPropertyValue is string
                        && !string.IsNullOrEmpty(objectPropertyValue.ToString()))
                        if (objectPropertyValue.ToString() == displayedText)
                            return true;
                }

                return false;
            });
        }

        public static string GetTextForFrameworkElement(FrameworkElement frameworkElement)
        {
            foreach (var textName in StringPropertyNamesToTestForText)
            {
                var stringPropertyValue = GetElementProperty<string>(frameworkElement, textName);
                if (stringPropertyValue != null)
                   return stringPropertyValue;
            }

            foreach (var objectName in ObjectPropertyNamesToTestForText)
            {
                var objectPropertyValue = GetElementProperty<object>(frameworkElement, objectName);
                if (objectPropertyValue != null
                    && objectPropertyValue is string)
                    return (string) objectPropertyValue;
            }

            return null;
        }
    }
}
