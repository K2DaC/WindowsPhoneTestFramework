// ----------------------------------------------------------------------
// <copyright file="Program.cs" company="Expensify">
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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Args;
using Args.Help;
using Args.Help.Formatters;
using WindowsPhoneTestFramework.CommandLineHost;
using WindowsPhoneTestFramework.CommandLineHost.Commands;
using WindowsPhoneTestFramework.EmuAutomationController.Interfaces;
using WindowsPhoneTestFramework.EmuHost.Commands;

namespace WindowsPhoneTestFramework.EmuHost
{
    public class Program : ProgramBase
    {
        public static void Main(string[] args)
        {
            AppLaunchingCommandLine commandLine;
            IModelBindingDefinition<AppLaunchingCommandLine> modelBindingDefinition = null;
            try
            {
                modelBindingDefinition = Configuration.Configure<AppLaunchingCommandLine>();
                commandLine = modelBindingDefinition.CreateAndBind(args);

                if (commandLine.ProductId == Guid.Empty)
                {
                    Console.WriteLine("No productId supplied");
                    throw new ApplicationException("Help!");
                }
            }
            catch (Exception /*exception*/)
            {
                if (modelBindingDefinition != null)
                {
                    var help = new HelpProvider();
                    var formatter = new ConsoleHelpFormatter();

                    var sw = new StringWriter();
                    var text = help.GenerateModelHelp(modelBindingDefinition);
                    formatter.WriteHelp(text, sw);
                    Console.Write(sw.ToString());
                }
                else
                {
                    Console.Write("Sorry - no help available!");
                }
                return;
            }

            try
            {
                Console.WriteLine("AutomationHost starting");
                using (var program = new Program(commandLine))
                {
                    Console.WriteLine("To show help, enter 'help'");
                    program.Run();
                }
            }
            catch (QuitNowPleaseException)
            {
                Console.WriteLine("Goodbye");
            }
            catch (Exception exception)
            {
                Console.WriteLine(string.Format("Exception seen {0} {1}", exception.GetType().FullName, exception.Message));
            }
        }

        private readonly AppLaunchingCommandLine _commandLine;
        private IEmuAutomationController _emuAutomationController;

        public Program(AppLaunchingCommandLine commandLine)
        {
            _commandLine = commandLine;
            StartEmuAutomationController();

            var driverCommands = new DriverCommands()
                                     {
                                         Driver = _emuAutomationController.Driver
                                     };
            AddCommands(driverCommands);

            var inputCommands = new DisplayInputCommands()
                                    {
                                        DisplayInputController = _emuAutomationController.DisplayInputController
                                    };
            AddCommands(inputCommands);

            var phoneAutomationCommands = new PhoneAutomationCommands()
                                              {
                                                  PhoneAutomationController =
                                                      _emuAutomationController.PhoneAutomationController
                                              };
            AddCommands(phoneAutomationCommands);

        }

        private void StartEmuAutomationController()
        {
            Console.WriteLine("-> controller will listen for connection on " + _commandLine.Binding);
            Console.WriteLine("-> controller will identify controls using " + _commandLine.AutomationIdentification);
            _emuAutomationController = new EmuAutomationController.EmuAutomationController();
            _emuAutomationController.Trace += (sender, args) => Console.WriteLine("-> " + args.Message);
            _emuAutomationController.Start(new Uri(_commandLine.Binding), _commandLine.AutomationIdentification);
            Console.WriteLine("-> controller started");
            Console.WriteLine();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_emuAutomationController != null)
                {
                    _emuAutomationController.Dispose();
                    _emuAutomationController = null;
                }
            }
            base.Dispose(isDisposing);
        }

    }
}
