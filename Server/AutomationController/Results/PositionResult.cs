// ----------------------------------------------------------------------
// <copyright file="PositionResult.cs" company="Expensify">
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
    public class PositionResult : ResultBase
    {
        [DataMember]
        public double Left { get; set; }
        [DataMember]
        public double Top { get; set; }
        [DataMember]
        public double Width { get; set; }
        [DataMember]
        public double Height { get; set; }
    }
}