// ----------------------------------------------------------------------
// <copyright file="CommandBase.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;

namespace WindowsPhoneTestFramework.AutomationController.Commands
{
    public class CommandBase
    {
        public Guid Id { get; set; }
        public string Type { get; set; }

        public CommandBase()
        {
            Id = Guid.NewGuid();
            Type = this.GetType().FullName;
        }
    }
}