// ----------------------------------------------------------------------
// <copyright file="GenericCommand.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using WindowsPhoneTestFramework.AutomationController.Interfaces;

namespace WindowsPhoneTestFramework.AutomationController.Commands
{
    public class GenericCommand : AutomationElementCommandBase
    {
        public string PleaseDo { get; set; }
        public string Parameter0 { get; set; }
        public string Parameter1 { get; set; }
        public string Parameter2 { get; set; }
        public string Parameter3 { get; set; }
        public string Parameter4 { get; set; }
    }
}