// ----------------------------------------------------------------------
// <copyright file="CommandLineCommandAttribute.cs" company="Expensify">
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
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandLineCommandAttribute : Attribute
    {
        public string Name { get; set; }

        public CommandLineCommandAttribute()
        {            
        }

        public CommandLineCommandAttribute(string name)
        {
            Name = name;
        }
    }
}