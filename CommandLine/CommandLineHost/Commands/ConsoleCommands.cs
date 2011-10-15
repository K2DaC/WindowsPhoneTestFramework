// ----------------------------------------------------------------------
// <copyright file="ConsoleCommands.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WindowsPhoneTestFramework.CommandLineHost.Commands
{
    public class ConsoleCommands
    {
        public Dictionary<string, DescribedMethod> ActionList;

        [CommandLineCommand("quit")]
        [Description("shutdown this server - e.g. 'quit'")]
        public void Quit(string ignored)
        {
            throw new QuitNowPleaseException();
        }

        [CommandLineCommand("help")]
        [Description("shows help text - e.g. 'help'")]
        public void ShowHelp(string ignored)
        {
            Console.WriteLine("Available actions are:");
            foreach (var action in ActionList)
            {
                Console.WriteLine();
                Console.WriteLine("-> " + action.Key);
                Console.WriteLine(action.Value.Description);
            }
        }
    }
}