// ----------------------------------------------------------------------
// <copyright file="WindowsPhoneOrientationExtensionMethods.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Drawing;

namespace WindowsPhoneTestFramework.EmuDriver
{
    // this might be a bad name - really this contains a RectangleF extension method
    public static class WindowsPhoneOrientationExtensionMethods
    {
        public static Point ScreenMiddle(this WindowsPhoneOrientation orientation)
        {
            var size = orientation.ScreenSize();
            return new Point(size.Width/2, size.Height/2);
        }

        public static Size ScreenSize(this WindowsPhoneOrientation orientation)
        {
            switch (orientation)
            {
                case WindowsPhoneOrientation.Landscape800By480:
                    return new Size(800, 480);

                case WindowsPhoneOrientation.Portrait480By800:
                    return new Size(480, 800);

                default:
                    throw new ArgumentException("unknown orientation " + orientation);
            }
        }

        public static bool IsVisible(this RectangleF position, WindowsPhoneOrientation orientation)
        {
            if (position.IsEmpty)
                return false;

            var height = 0.0;
            var width = 0.0;

            switch (orientation)
            {
                case WindowsPhoneOrientation.Landscape800By480:
                    height = 480.0;
                    width = 800.0;
                    break;
                case WindowsPhoneOrientation.Portrait480By800:
                    height = 800.0;
                    width = 480.0;
                    break;
                default:
                    throw new ArgumentException("unknown orientation " + orientation);
            }

            if (position.X + position.Width <= 0)
                return false;

            if (position.Y + position.Height <= 0)
                return false;

            if (position.X >= width)
                return false;

            if (position.Y >= height)
                return false;
                
            return true;
        }
    }
}