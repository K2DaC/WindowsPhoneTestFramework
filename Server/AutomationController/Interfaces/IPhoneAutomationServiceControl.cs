// ----------------------------------------------------------------------
// <copyright file="IPhoneAutomationServiceControl.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using WindowsPhoneTestFramework.AutomationController.Commands;
using WindowsPhoneTestFramework.AutomationController.Results;

namespace WindowsPhoneTestFramework.AutomationController.Interfaces
{
    public interface IPhoneAutomationServiceControl
    {
        void AddCommand(CommandBase command, Action<ResultBase> onResult);
        void AddCommand(CommandBase command, Action<ResultBase> onResult, TimeSpan sendCommandWithin, TimeSpan expectResultWithin);
        void Clear();
    }
}