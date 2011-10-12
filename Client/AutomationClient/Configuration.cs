// ----------------------------------------------------------------------
// <copyright file="Configuration.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System.ServiceModel;
using System.Windows.Threading;
using WindowsPhoneTestFramework.AutomationClient.Remote;

namespace WindowsPhoneTestFramework.AutomationClient
{
    public class Configuration : IConfiguration
    {
        public string RemoteUrl { get; set; }
        public Dispatcher UiDispatcher { get; set; }

        public PhoneAutomationServiceClient CreateClient()
        {
            var binding = new BasicHttpBinding()
                              {
                                  Name = "binding1_IPhoneAutomationService",
                                  MaxBufferSize = 2147483647,
                                  MaxReceivedMessageSize = 2147483647,
                              };
            binding.Security.Mode = BasicHttpSecurityMode.None;

            EndpointAddress endpoint;
            if (string.IsNullOrEmpty(RemoteUrl))
                endpoint = new EndpointAddress("http://localhost:8085/phoneAutomation/automate");
            else
                endpoint = new EndpointAddress(RemoteUrl);

            return new PhoneAutomationServiceClient(binding, endpoint);
        }

        /*
<configuration>
    <system.serviceModel>
        <bindings>
            <basicHttpBinding>
                <binding name="binding1_IPhoneAutomationService" maxBufferSize="2147483647"
                    maxReceivedMessageSize="2147483647">
                    <security mode="None" />
                </binding>
            </basicHttpBinding>
        </bindings>
        <client>
            <endpoint address="http://localhost:8085/phoneAutomation/automate"
                binding="basicHttpBinding" bindingConfiguration="binding1_IPhoneAutomationService"
                contract="Remote.IPhoneAutomationService" name="binding1_IPhoneAutomationService" />
        </client>
    </system.serviceModel>
</configuration>
         */
    }
}