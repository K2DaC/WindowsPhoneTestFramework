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

using System;
using System.Collections.Generic;

namespace WindowsPhoneTestFramework.AutomationClient.Remote
{
    public partial class GenericCommand
    {
        private readonly static Dictionary<string, Func<GenericCommand, Action>> _handlerFactories = new Dictionary<string, Func<GenericCommand, Action>>();

        public static void AddHandlerFactory(string key, Func<GenericCommand, Action> handlerFactory)
        {
            lock (_handlerFactories)
            {
                _handlerFactories[key] = handlerFactory;
            }
        }

        public static void RemoveHandlerFactory(string key)
        {
            lock (_handlerFactories)
            {
                if (_handlerFactories.ContainsKey(key))
                    _handlerFactories.Remove(key);
            }
        }

        protected override void DoImpl()
        {
            Action processor = null;

            lock (_handlerFactories)
            {
                foreach (var handler in _handlerFactories)
                {
                    processor = handler.Value(this);
                    if (processor != null)
                        break;
                }
            }

            if (processor != null)
            {
                processor();
                return;
            }

            // if we've reached this stage, then this is an error...
            // and this DoImpl() call will almost certainly result in an exception
            base.DoImpl();
        }
    }
}