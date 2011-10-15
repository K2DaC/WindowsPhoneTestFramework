// ----------------------------------------------------------------------
// <copyright file="TraceBase.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;

namespace WindowsPhoneTestFramework.Utils
{
    public class TraceBase : ITrace
    {
        public event EventHandler<SimpleMessageEventArgs> Trace;

        protected void InvokeTrace(string message, params object[] args)
        {
            InvokeTrace(new SimpleMessageEventArgs() {Message = string.Format(message, args)});
        }

        protected void InvokeTrace(SimpleMessageEventArgs e)
        {
            EventHandler<SimpleMessageEventArgs> handler = Trace;
            if (handler != null) handler(this, e);
        }    
    }
}