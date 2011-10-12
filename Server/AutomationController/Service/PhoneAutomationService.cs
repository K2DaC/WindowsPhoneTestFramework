// ----------------------------------------------------------------------
// <copyright file="PhoneAutomationService.cs" company="Expensify">
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
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using WindowsPhoneTestFramework.AutomationController.Commands;
using WindowsPhoneTestFramework.AutomationController.Utils;
using WindowsPhoneTestFramework.AutomationController.Interfaces;
using WindowsPhoneTestFramework.AutomationController.Results;
using WindowsPhoneTestFramework.Utils;

namespace WindowsPhoneTestFramework.AutomationController.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class PhoneAutomationService : TraceBase, IPhoneAutomationService, IPhoneAutomationServiceControl, IDisposable
    {
        private enum State
        {
            Empty,
            CommandQueued,
            CommandSent,
            Error
        }

        private const int WatchdogPeriodInMilliseconds = 100;
 
        private static readonly TimeSpan DefaultCommandTimeout = TimeSpan.FromSeconds(5.0);
        private static readonly TimeSpan DefaultResultTimeout = TimeSpan.FromSeconds(15.0);

        private readonly ManualResetEvent _commandAvailableEvent = new ManualResetEvent(false);
        private readonly Timer _checkTimer;

        private State _state = State.Empty;

        private CommandBase _currentCommand;
        private Action<ResultBase> _resultCallback;
        private TimeSpan _timeAllowedForSend;
        private TimeSpan _timeAllowedForResult;
        private DateTime _timeCommandAdded;
        private DateTime _timeCommandSent;

        public static PhoneAutomationService CurrentInstance { get; private set; }

        static PhoneAutomationService()
        {
            KnownTypeProvider.RegisterDerivedTypesOf<CommandBase>(typeof(CommandBase).Assembly);
            KnownTypeProvider.RegisterDerivedTypesOf<ResultBase>(typeof(ResultBase).Assembly);
        }

        public PhoneAutomationService()
        {
            Clear();
            _checkTimer = new Timer(WatchdogTimerTick, null, WatchdogPeriodInMilliseconds, WatchdogPeriodInMilliseconds);
            CurrentInstance = this;
        }

        public void Dispose()
        {
            if (CurrentInstance == this)
                CurrentInstance = null;
        }

        #region IPhoneAutomationController

        public void AddCommand(CommandBase command, Action<ResultBase> onResult)
        {
            AddCommand(command, onResult, DefaultCommandTimeout, DefaultResultTimeout);
        }

        public void AddCommand(CommandBase command, Action<ResultBase> onResult, TimeSpan sendCommandWithin, TimeSpan expectResultWithin)
        {
            lock (this)
            {
                if (_state != State.Empty)
                    throw new InvalidOperationException("Command cannot be set - currently in state " + _state);

                _currentCommand = command;
                _resultCallback = onResult;
                _timeCommandAdded = DateTime.UtcNow;
                _timeCommandSent = DateTime.MinValue;
                _commandAvailableEvent.Set();
                _timeAllowedForSend = sendCommandWithin;
                _timeAllowedForResult = expectResultWithin;
                _state = State.CommandQueued;
            }
        }

        public void Clear()
        {
            lock (this)
            {
                _state = State.Empty;
                _timeCommandSent = DateTime.MinValue;
                _timeCommandAdded = DateTime.MinValue;
                _currentCommand = null;
                _resultCallback = null;
                _commandAvailableEvent.Reset();
            }
        }

        #endregion // IPhoneAutomationController

        #region IPhoneAutomationService

        public CommandBase GetNextCommand(int timeoutInMilliseconds)
        {
            if (_commandAvailableEvent.WaitOne(timeoutInMilliseconds))
            {
                lock (this)
                {
                    if (_state != State.CommandQueued)
                    {
                        InvokeTrace("Internal logic problem seen - invalid _state in GetNextCommand");
                        throw new InvalidOperationException("How did we get here?");
                    }

                    _commandAvailableEvent.Reset();
                    _state = State.CommandSent;
                    _timeCommandSent = DateTime.UtcNow;
                    InvokeTrace("Command sent to client");
                    return _currentCommand;
                }
            }
            return new NullCommand();
        }

        public bool ContinueProcessing(Guid commandId)
        {
            lock (this)
            {
                return IsCurrentCommand(commandId);
            }
        }

        public void SubmitResult(ResultBase result)
        {
            lock (this)
            {
                if (_state != State.CommandSent)
                {
                    // do nothing
                    InvokeTrace("Result from client ignored - _state is {0}", _state);
                    return;
                }

                if (result == null)
                {
                    // do nothing
                    InvokeTrace("Result from client ignored - null result");
                    return;
                }

                if (!IsCurrentCommand(result.Id))
                {
                    // do nothing
                    InvokeTrace("Result from client ignored - not current command id");
                    return;
                }

                InvokeTrace("Result from client received - processing...");
                InvokeCallbackAndClear(result);
            }
        }

        #endregion // IPhoneAutomationService

        #region Private methods

        private void WatchdogTimerTick(object ignored)
        {
            lock (this)
            {
                switch (_state)
                {
                    case State.CommandQueued:
                        if (DateTime.UtcNow - _timeCommandAdded > _timeAllowedForSend)
                        {
                            InvokeTrace("Command timed out waiting for send");
                            InvokeCallbackAndClear(new TimeoutFailedResult() { Id = _currentCommand.Id });
                        }
                        break;
                    case State.CommandSent:
                        if (DateTime.UtcNow - _timeCommandSent > _timeAllowedForResult)
                        {
                            InvokeTrace("Command timed out waiting for response");
                            InvokeCallbackAndClear(new TimeoutFailedResult() { Id = _currentCommand.Id });
                        }
                        break;
                    default:
                        // nothing to do
                        break;
                }
            }
        }

        private void InvokeCallbackAndClear(ResultBase result)
        {
            lock(this)
            {
                if (_resultCallback != null)
                {
                    // invoke callback on a separate thread
                    // note that we copy callback to a local variable (to ensure its not null when the thread is invoked)
                    var callback = _resultCallback;
                    ThreadPool.QueueUserWorkItem((ignored) => callback(result));
                }

                Clear();
            }
        }

        private bool IsCurrentCommand(Guid commandId)
        {
            return _currentCommand != null
                    && _currentCommand.Id == commandId;
        }

        #endregion // Private methods

    }
}
