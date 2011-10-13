// ----------------------------------------------------------------------
// <copyright file="DescribedMethod.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;

namespace WindowsPhoneTestFramework.CommandLineHost
{
    public class DescribedMethod
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Action<string> Action { get; set; }
    }
}