// ----------------------------------------------------------------------
// <copyright file="SwipeGesture.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Authors - K2DaC, slodge
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;

namespace WindowsPhoneTestFramework.EmuDriver
{
    public class TapGesture : MultipointGestureBase
    {
        private static readonly TimeSpan DefaultTapDuration = TimeSpan.FromMilliseconds(500.0);

        public Point TapPosition { get; set; }

        public TapGesture(int x, int y)
            : this (x, y, DefaultTapDuration)
        {            
        }

        public TapGesture(int x, int y, TimeSpan tapDuration)
        {
            TapPosition = new Point(x, y);       
            NumberOfIntermediatePoints = 0;
            PeriodBetweenPoints = tapDuration;
        }

        public static TapGesture TapOnPosition(int x, int y)
        {
            return new TapGesture(x, y);
        }

        protected override IEnumerable<Point> GetScreenPoints()
        {
            return new Point[]
                       {
                           TapPosition,
                           TapPosition
                       };
        }
    }
}
