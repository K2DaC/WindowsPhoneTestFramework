// ----------------------------------------------------------------------
// <copyright file="Constants.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

//use DEBUGGING_CLIENT_APP in order to allow you to debug command processing within a WP7 App
//#define DEBUGGING_CLIENT_APP

using System;

namespace WindowsPhoneTestFramework.AutomationController.Interfaces
{
    public static class Constants
    {
#if DEBUGGING_CLIENT_APP
        public static readonly TimeSpan DefaultCommandWaitTimeout = TimeSpan.FromMinutes(5.0);
        public static readonly TimeSpan DefaultCommandSendTimeout = TimeSpan.FromMinutes(5.0);
#else // DEBUGGING_CLIENT_APP
        public static readonly TimeSpan DefaultCommandWaitTimeout = TimeSpan.FromSeconds(10.0);
        public static readonly TimeSpan DefaultCommandSendTimeout = TimeSpan.FromSeconds(3.0);
#endif // DEBUGGING_CLIENT_APP

        public static readonly TimeSpan DefaultConfirmAliveTimeout = TimeSpan.FromSeconds(30.0);
        public static readonly TimeSpan DefaultWaitForClientAppActionTimeout = TimeSpan.FromSeconds(30.0);
    }
}