using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WindowsPhoneTestFramework.CommandLineHost.Commands
{
    public class ConsoleCommands
    {
        public Dictionary<string, DescribedMethod> ActionList;

        [DisplayName("quit")]
        [Description("shutdown this server - e.g. 'quit'")]
        public void Quit(string ignored)
        {
            throw new QuitNowPleaseException();
        }

        [DisplayName("help")]
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