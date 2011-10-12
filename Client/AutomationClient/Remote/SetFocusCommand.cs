// ----------------------------------------------------------------------
// <copyright file="SetFocusCommand.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System.Windows.Controls;

namespace WindowsPhoneTestFramework.AutomationClient.Remote
{
    public partial class SetFocusCommand
    {
        protected override void DoImpl()
        {
            var element = GetFrameworkElement();
            if (element == null)
                return;

            var control = element as Control;
            if (control == null)
            {
                SendNotFoundResult();
                return;
            }

            control.Focus();
            SendSuccessResult();
        }
    }
}