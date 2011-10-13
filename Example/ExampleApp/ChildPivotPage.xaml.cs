// ----------------------------------------------------------------------
// <copyright file="ChildPivotPage.xaml.cs" company="Expensify">
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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace ExampleApp
{
    public partial class ChildPivotPage : PhoneApplicationPage
    {
        public ChildPivotPage()
        {
            InitializeComponent();
        }

        private void TextBoxInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (BreakThingsCheckBox.IsChecked ?? false)
                return;

            var input = TextBoxInput.Text;
            var reversed = new string(input.Reverse().ToArray());
            TextBoxOutput.Text = reversed;
        }
    }
}