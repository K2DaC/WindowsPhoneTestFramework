// ----------------------------------------------------------------------
// <copyright file="ExceptionSafe.cs" company="Expensify">
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
    public static class ExceptionSafe
    {
        public static void ExecuteConsoleWriteAnyException(Action action, string exceptionMessageBase)
        {
            ExceptionSafe.Execute(
                    action,
                    (exc) => 
                        ExceptionSafe.Execute(
                            () =>
                                {
                                    string message = string.Format("{0} : {1} - {2}", 
                                                                   exceptionMessageBase, exc.GetType().FullName, exc.Message);
                                    Console.WriteLine(message);
                                }));
        }
        public static void Execute(Action action, Action<Exception> exceptionAction = null)
        {
            try
            {
                if (action != null)
                    action();
            }
            catch (Exception exception)
            {
                if (exceptionAction != null)
                    exceptionAction(exception);
            }
        }
    }
}