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

using System.Runtime.Serialization;
using WindowsPhoneTestFramework.AutomationController.Interfaces;

namespace WindowsPhoneTestFramework.AutomationController.Commands
{
    [DataContract]
    public class GenericCommand : AutomationElementCommandBase
    {
        [DataMember]
        public string PleaseDo { get; set; }
        [DataMember]
        public string Parameter0 { get; set; }
        [DataMember]
        public string Parameter1 { get; set; }
        [DataMember]
        public string Parameter2 { get; set; }
        [DataMember]
        public string Parameter3 { get; set; }
        [DataMember]
        public string Parameter4 { get; set; }
    }
}