// ----------------------------------------------------------------------
// <copyright file="StepFlowOutputHelpers.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.IO;

namespace WindowsPhoneTestFramework.EmuSteps
{
    public static class StepFlowOutputHelpers
    {
        public enum WriteType
        {
            Trace,
            Warning,
            Error
        } 

        public static void Write(string message, params object[] args)
        {
            Write(WriteType.Trace, message, args);
        }

        public static void Write(WriteType type, string message, params object[] args)
        {
            var template = string.Format("_startEmu{0}_->{1}_endEmu{0}_", type, message);
            Console.WriteLine(string.Format(template, args));
        }

        public static void WriteException(string message, Exception exception)
        {
            Write(WriteType.Error, "Exception : {0} : {1} : {2}", message, exception.GetType().FullName, exception.Message);
        }
    }
}