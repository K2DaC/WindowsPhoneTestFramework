// ----------------------------------------------------------------------
// <copyright file="GetTextCommand.cs" company="Expensify">
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
    public partial class GetTextCommand
    {
        protected override void DoImpl()
        {
            var element = GetUIElement();
            if (element == null)
                return;

            var text = AutomationElementFinder.GetElementProperty<string>(element, "Text");
            if (text != null)
            {
                SendTextResult(text);
                return;
            }

            var password = AutomationElementFinder.GetElementProperty<string>(element, "Password");
            if (password != null)
            {
                SendTextResult(password);
                return;
            }

            var content = AutomationElementFinder.GetElementProperty<object>(element, "Content");
            if (content != null)
            {
                if (content is string)
                {
                    SendTextResult(content.ToString());
                    return;
                }
            }

            // if text, password and content are all missing... then give up
            SendNotFoundResult();
        }
    }
}