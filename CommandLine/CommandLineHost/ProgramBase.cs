// ----------------------------------------------------------------------
// <copyright file="ProgramBase.cs" company="Expensify">
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
using System.Linq;
using System.Reflection;
using System.Threading;

namespace WindowsPhoneTestFramework.CommandLineHost
{
    public class ProgramBase : IDisposable
    {
        private Dictionary<string, DescribedMethod> _actions;

        public ProgramBase()
        {
            LoadActions();
        }

        protected class DescribedMethod
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public Action<string> Action { get; set; }
        }

        private void LoadActions()
        {
            _actions = new Dictionary<string, DescribedMethod>();
            var actions =
                from method in
                    this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                let displayName =
                    (DisplayNameAttribute)
                    method.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault()
                let description =
                    (DescriptionAttribute)
                    method.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault()
                where displayName != null
                select new
                           {
                               Name = displayName.DisplayName,
                               Description = description == null ? string.Empty : description.Description,
                               Method = method
                           };

            foreach (var thing in actions)
            {
                var thatThing = thing;
                _actions[thatThing.Name] = new DescribedMethod()
                                               {
                                                   Name = thatThing.Name,
                                                   Description = thatThing.Description,
                                                   Action = input => 
                                                       {
                                                           try
                                                           {
                                                               thatThing.Method.Invoke(this, new object[] { input });
                                                           }
                                                           catch (TargetInvocationException tie)
                                                           {
                                                               throw tie.InnerException;
                                                           }
                                                       }
                                               };
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // nothing to do...
            }
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Next action?");
                var nextCommand = Console.ReadLine();
                if (nextCommand == null)
                    continue;

                var split = nextCommand.Split(new char[] { ' ' }, 2);
                string command = split[0];
                string argument = string.Empty;
                if (split.Length == 2)
                {
                    argument = split[1];
                }

                DescribedMethod describedMethod;
                if (_actions.TryGetValue(command, out describedMethod))
                {
                    ExceptionSafe(() => describedMethod.Action(argument));
                }
                else
                {
                    Console.WriteLine("Unknown command: " + command);
                }
            }
        }

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
            foreach (var action in _actions)
            {
                Console.WriteLine();
                Console.WriteLine("-> " + action.Key);
                Console.WriteLine(action.Value.Description);
            }
        }

        protected void ExceptionSafe(Action todo)
        {
            try
            {
                todo();
            }
            catch (QuitNowPleaseException)
            {
                throw;
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                Console.WriteLine(string.Format("Exception seen - {0} - {1}", exception.GetType().FullName, exception.Message));
            }
        }
    }
}