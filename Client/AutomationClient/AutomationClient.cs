// ----------------------------------------------------------------------
// <copyright file="AutomationClient.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

// try to suppress annoying debugger exception reports - just can't get this to work cleanly - so not defined yet
//#define USE_CONNECTION_CHECK_BEFORE_STARTING

using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WindowsPhoneTestFramework.AutomationClient.Remote;

namespace WindowsPhoneTestFramework.AutomationClient
{
    public class AutomationClient : IAutomationClient
    {
        private const int GetNextCommandTimeoutInMilliseconds = 2000;
        private const int ErrorSleepTimeoutInMilliseconds = 500;
        private const int CheckServerSleepTimeoutInMilliseconds = 500;
        private const int NullCommandSleepTimeoutInMilliseconds = 100;

        private readonly IConfiguration _configuration;
        private readonly ManualResetEvent _stopPlease;
        private Thread _thread;
        
        public AutomationClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _stopPlease = new ManualResetEvent(false);
        }

        public void Dispose()
        {
            Stop();
        }

        public void Start()
        {
            _thread = new Thread(Run);
            _thread.Start();
        }

        public void Stop()
        {
            if (_thread != null)
            {
                _stopPlease.Set();
                _thread.Join();
                _thread = null;
                _stopPlease.Reset();
            }
        }

        private void Run()
        {
            bool isServerAvailable = true; // default to try to connect first time...
            while (_stopPlease.WaitOne(0) == false)
            {
                try
                {
                    if (isServerAvailable)
                        GetAndProcessNextCommand();
#if USE_CONNECTION_CHECK_BEFORE_STARTING
                    else
                        isServerAvailable = _configuration.TestIfRemoteAvailable();

                    if (!isServerAvailable)
                        Thread.Sleep(TimeSpan.FromMilliseconds(CheckServerSleepTimeoutInMilliseconds));
#endif //USE_CONNECTION_CHECK_BEFORE_STARTING
                }
                catch (ThreadAbortException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    // probably means server not present - so sleep for a second
                    // TODO - improve this...
                    Debug.WriteLine(string.Format("Exception seen {0} {1}", 
                                                  exception.GetType().FullName,
                                                  exception.Message));
                    Thread.Sleep(TimeSpan.FromMilliseconds(ErrorSleepTimeoutInMilliseconds));
#if USE_CONNECTION_CHECK_BEFORE_STARTING
                    isServerAvailable = false;
#endif //USE_CONNECTION_CHECK_BEFORE_STARTING
                }
            }
        }

        private void GetAndProcessNextCommand()
        {
            var serviceClient = _configuration.CreateClient();

            var commandProcessed = new ManualResetEvent(false);
            serviceClient.GetNextCommandCompleted += (obj, args) =>
            {
                if (args.Error != null)
                {
                    // TODO! Log something somehow!
                }
                else
                {
                    ProcessNextCommand(args.Result);
                }
                commandProcessed.Set();
            };

            serviceClient.GetNextCommandAsync(GetNextCommandTimeoutInMilliseconds);
            commandProcessed.WaitOne();
        }

        private void ProcessNextCommand(CommandBase command)
        {
            if (command == null)
            {
                // TODO - log
                return;
            }

            var mre = new ManualResetEvent(false);
            _configuration.UiDispatcher.BeginInvoke(
                () =>
                    {
                        command.Configuration = _configuration;
                        command.Do();
                        mre.Set();
                    });
            mre.WaitOne();

            if (command is NullCommand)
            {
                Thread.Sleep(NullCommandSleepTimeoutInMilliseconds);
                return;
            }
        }
    }
}
