// ----------------------------------------------------------------------
// <copyright file="NotFoundFailedResult.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;

namespace WindowsPhoneTestFramework.AutomationController.Results
{
    public class NotFoundFailedResult : FailedResultBase
    {
        public NotFoundFailedResult()
        {
            FailureText = "Not found";
        }
    }
}