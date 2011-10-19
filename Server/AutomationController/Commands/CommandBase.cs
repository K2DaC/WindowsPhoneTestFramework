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
using System.Runtime.Serialization;

namespace WindowsPhoneTestFramework.AutomationController.Commands
{
    [DataContract]
    public class CommandBase
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Type { get; set; }

        public CommandBase()
        {
            Id = Guid.NewGuid();
            Type = this.GetType().FullName;
        }
    }
}