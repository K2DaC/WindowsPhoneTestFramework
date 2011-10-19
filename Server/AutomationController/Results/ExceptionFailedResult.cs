// ----------------------------------------------------------------------
// <copyright file="ExceptionFailedResult.cs" company="Expensify">
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

namespace WindowsPhoneTestFramework.AutomationController.Results
{
    [DataContract]
    public class ExceptionFailedResult : FailedResultBase
    {
        [DataMember]
        public string ExceptionType { get; set; }
        [DataMember]
        public string ExceptionMessage { get; set; }
    }
}