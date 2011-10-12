// ----------------------------------------------------------------------
// <copyright file="MultipointGestureBase.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;

namespace WindowsPhoneTestFramework.EmuDriver
{
    public abstract class MultipointGestureBase : GestureBase
    {
        public int NumberOfIntermediatePoints { get; set; }
        public TimeSpan PeriodBetweenPoints { get; set; }

        protected MultipointGestureBase()
        {
            NumberOfIntermediatePoints = 10;
            PeriodBetweenPoints = TimeSpan.FromMilliseconds(20.0);
        }
    }
}