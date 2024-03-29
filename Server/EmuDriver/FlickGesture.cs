﻿// ----------------------------------------------------------------------
// <copyright file="FlickGesture.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System.Collections.Generic;
using System.Drawing;

namespace WindowsPhoneTestFramework.EmuDriver
{
    public class FlickGesture : MultipointGestureBase
    {
        public Point SwipeStartPosition { get; set; }
        public Point SwipeEndPosition { get; set; }

        public FlickGesture()
        {
            // default is a horizontal left to right swipe at height 100
            SwipeStartPosition = new Point(100, 100);
            SwipeEndPosition = new Point(400, 100);
        }

        public void ReverseDirection()
        {
            var oldStart = SwipeStartPosition;
            SwipeStartPosition = SwipeEndPosition;
            SwipeEndPosition = oldStart;
        }

        public static FlickGesture LeftToRightPortrait(int height = 400)
        {
            return new FlickGesture()
                       {
                           SwipeStartPosition = new Point(120, height),
                           SwipeEndPosition = new Point(360, height)
                       };
        }

        public static FlickGesture LeftToRightLandscape(int height = 240)
        {
            return new FlickGesture()
                       {
                           SwipeStartPosition = new Point(200, height),
                           SwipeEndPosition = new Point(400, height)
                       };
        }

        public static FlickGesture RightToLeftPortrait(int height = 400)
        {
            var toReturn = LeftToRightPortrait(height);
            toReturn.ReverseDirection();
            return toReturn;
        }

        public static FlickGesture RightToLeftLandscape(int height = 240)
        {
            var toReturn = LeftToRightLandscape(height);
            toReturn.ReverseDirection();
            return toReturn;
        }

        protected override IEnumerable<Point> GetScreenPoints()
        {
            var list = new List<Point>();

            list.Add(SwipeStartPosition);
            for (int i = 0; i < NumberOfIntermediatePoints; i++)
            {
                list.Add(GenerateIntermediatePoint(i));
            }
            list.Add(SwipeEndPosition);
            
            return list;
        }

        private Point GenerateIntermediatePoint(int zeroBasedIndex)
        {
            // zero-indexed point i of N is (i+1)/(N+1) along the way
            // e.g. point if there are 3 intermediate points:
            //      intermediate 0 will be at 1/4
            //      intermediate 1 will be at 2/4
            //      intermediate 2 will be at 3/4
            var ratio = (zeroBasedIndex + 1.0) / (NumberOfIntermediatePoints + 1.0);

            return new Point()
                       {
                           X = SwipeStartPosition.X + (int) (ratio * (SwipeEndPosition.X - SwipeStartPosition.X)),
                           Y = SwipeStartPosition.Y + (int) (ratio * (SwipeEndPosition.Y - SwipeStartPosition.Y))
                       };
        }
    }
}