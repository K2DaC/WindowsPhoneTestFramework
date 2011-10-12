// ----------------------------------------------------------------------
// <copyright file="TakePictureCommand.cs" company="Expensify">
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
    public partial class TakePictureCommand
    {
        public byte[] DoCapture()
        {
            FrameworkElement toSnap;

            if (AutomationIdentifier == null)
            {
                // find the current page
                var rootVisual = (PhoneApplicationFrame) Application.Current.RootVisual;
                if (rootVisual == null)
                    return null;

                var currentPage = rootVisual.Content as PhoneApplicationPage;
                if (currentPage == null)
                    return null;

                toSnap = currentPage;
            }
            else
            {
                toSnap = GetFrameworkElement();
                if (toSnap == null)
                    return null;
            }

            // Save to bitmap
            var bmp = new WriteableBitmap((int)toSnap.ActualWidth, (int)toSnap.ActualHeight);
            bmp.Render(toSnap, null);
            bmp.Invalidate();

            // Get memoryStream from bitmap
            using (var memoryStream = new MemoryStream())
            {
                bmp.SaveJpeg(memoryStream, bmp.PixelWidth, bmp.PixelHeight, 0, 95);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return memoryStream.GetBuffer();
            }
        }

        protected override void DoImpl()
        {
            var bytes = DoCapture();

            if (bytes == null)
                throw new TestAutomationException("Screen capture fail");

            SendPictureResult(bytes);
        }
    }
}