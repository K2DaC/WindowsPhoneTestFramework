// ----------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="Expensify">
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
using System.Windows.Threading;
using Microsoft.Phone.Controls;

namespace ExampleApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            GoButton.IsEnabled = false;
            GoButton.Content = "Waiting...";
            var timer = new DispatcherTimer();
            timer.Tick += (sender, args) =>
                              {
                                  GoButton.IsEnabled = true;
                                  GoButton.Content = "Go!";
                                  timer.Stop();
                                  timer = null;
                              };
            timer.Interval = TimeSpan.FromSeconds(5.0);
            timer.Start();

            base.OnNavigatedTo(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ChildPivotPage.xaml", UriKind.Relative));
        }
    }
}