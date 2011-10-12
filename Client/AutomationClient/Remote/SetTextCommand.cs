// ----------------------------------------------------------------------
// <copyright file="SetTextCommand.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

namespace WindowsPhoneTestFramework.AutomationClient.Remote
{
    public partial class SetTextCommand
    {
        protected override void DoImpl()
        {
            var element = GetUIElement();
            if (element == null)
                return;

            if (AutomationElementFinder.SetElementProperty<string>(element, "Text", Text))
            {
                SendSuccessResult();
                return;
            }

            if (AutomationElementFinder.SetElementProperty<string>(element, "Password", Text))
            {
                SendSuccessResult();
                return;
            }

            if (AutomationElementFinder.SetElementProperty<object>(element, "Content", Text))
            {
                SendSuccessResult();
                return;
            }

            // if text, password and content are all missing... then give up
            SendNotFoundResult();
        }
    }
}