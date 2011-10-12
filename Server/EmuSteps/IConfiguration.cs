// ----------------------------------------------------------------------
// <copyright file="IConfiguration.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using WindowsPhoneTestFramework.AutomationController.Interfaces;

namespace WindowsPhoneTestFramework.EmuSteps
{
    public interface IConfiguration
    {
        string BindingAddress { get; }
        AutomationIdentification AutomationIdentification { get; }
        Guid ProductId { get; }
        string ApplicationName { get; }
        string IconPath { get; }
        string XapPath { get; }
    }
}