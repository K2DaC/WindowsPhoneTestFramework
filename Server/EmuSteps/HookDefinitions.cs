// ----------------------------------------------------------------------
// <copyright file="HookDefinitions.cs" company="Expensify">
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
using System.Text;
using TechTalk.SpecFlow;

namespace WindowsPhoneTestFramework.EmuSteps
{
    [Binding]
    public class HookDefinitions : EmuDefinitionBase
    {
        public HookDefinitions()
        {                
        }

        public HookDefinitions(IConfiguration configuration)
            : base(configuration)
        {                
        }

        [BeforeScenario]
        public void BeforeAnyScenario()
        {

        }

        [AfterScenario]
        public void AfterAnyScenario()
        {
            DisposeOfEmu();            
        }
    }
}
