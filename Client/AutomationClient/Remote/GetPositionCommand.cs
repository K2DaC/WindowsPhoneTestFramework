// ----------------------------------------------------------------------
// <copyright file="GetPositionCommand.cs" company="Expensify">
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
using System.Windows.Media;

namespace WindowsPhoneTestFramework.AutomationClient.Remote
{
    public partial class GetPositionCommand
    {
        protected override void DoImpl()
        {
            var element = GetFrameworkElement();
            if (element == null)
                return;

            // if element is not visible, then return an empty position
            if (ReturnEmptyIfNotVisible)
                if (element.Visibility == Visibility.Collapsed)
                {
                    SendPositionResult(0.0, 0.0, 0.0, 0.0);
                    return;
                }

            try
            {
                // this answer is based on answer in http://forums.silverlight.net/t/12160.aspx
                // please note that for some weird transformations (skewing while rotating while... ) then it 
                // may not yield perfect answers...

                // Obtain transform information based off root element
                GeneralTransform gt = element.TransformToVisual(Application.Current.RootVisual);

                // Find the four corners of the element
                Point topLeft = gt.Transform(new Point(0, 0));
                Point topRight = gt.Transform(new Point(element.RenderSize.Width, 0));
                Point bottomLeft = gt.Transform(new Point(0, element.RenderSize.Height));
                Point bottomRight = gt.Transform(new Point(element.RenderSize.Width, element.RenderSize.Height));

                var left = Math.Min(Math.Min(Math.Min(topLeft.X, topRight.X), bottomLeft.X), bottomRight.X);
                var top = Math.Min(Math.Min(Math.Min(topLeft.Y, topRight.Y), bottomLeft.Y), bottomRight.Y);

                SendPositionResult(left, top, element.ActualWidth, element.ActualHeight);
            }
            catch(Exception exc)
            {
                // TODO - could log the exception
                SendExceptionFailedResult(exc);
            }
        }
    }
}