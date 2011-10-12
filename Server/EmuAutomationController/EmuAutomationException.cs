// ----------------------------------------------------------------------
// <copyright file="EmuAutomationException.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace WindowsPhoneTestFramework.EmuAutomationController
{
    [Serializable]
    public class EmuAutomationException : Exception
    {
        public EmuAutomationException()
        {
            
        }

        public EmuAutomationException(string message, params object[] args)
            : base(string.Format(message, args))
        {            
        }

        public EmuAutomationException(string message, Exception innerException)
            : base (message, innerException)
        {            
        }

        public EmuAutomationException(SerializationInfo info, StreamingContext context)
            : base (info, context)
        {            
        }    
    }
}