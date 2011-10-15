// ----------------------------------------------------------------------
// <copyright file="GetPositionCommand.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

namespace WindowsPhoneTestFramework.AutomationController.Commands
{
    public class GetPositionCommand : AutomationElementCommandBase
    {
        public bool ReturnEmptyIfNotVisible { get; set; }

        public GetPositionCommand()
        {
            ReturnEmptyIfNotVisible = true; // default is that the command will report {0,0,0,0} for any control with visibility off
        }
    }
}