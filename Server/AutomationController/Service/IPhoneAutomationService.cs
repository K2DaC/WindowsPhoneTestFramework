// ----------------------------------------------------------------------
// <copyright file="IPhoneAutomationService.cs" company="Expensify">
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
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WindowsPhoneTestFramework.AutomationController.Commands;
using WindowsPhoneTestFramework.AutomationController.Results;
using WindowsPhoneTestFramework.AutomationController.Utils;

namespace WindowsPhoneTestFramework.AutomationController.Service
{
    [ServiceContract]
    public interface IPhoneAutomationService
    {
        [OperationContract]
        [ServiceKnownType("GetKnownTypes", typeof(KnownTypeProvider))]
        CommandBase GetNextCommand(int timeoutInMilliseconds);

        [OperationContract]
        bool ContinueProcessing(Guid commandId);

        [OperationContract]
        [ServiceKnownType("GetKnownTypes", typeof(KnownTypeProvider))]
        void SubmitResult(ResultBase result);
    }
}
