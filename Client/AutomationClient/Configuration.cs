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

using System;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Windows.Threading;
using WindowsPhoneTestFramework.AutomationClient.Remote;

namespace WindowsPhoneTestFramework.AutomationClient
{
    public class Configuration : IConfiguration
    {
        public const string DefaultRemoteUrl = "http://localhost:8085/phoneAutomation/automate";
        public static readonly TimeSpan DefaultTestRemoteTimeout = TimeSpan.FromSeconds(1.0);

        public string RemoteUrl { get; set; }
        public Dispatcher UiDispatcher { get; set; }

        public Configuration()
        {
            RemoteUrl = DefaultRemoteUrl;
        }

        private Uri RemoteUri
        {
            get
            {
                return new Uri(RemoteUrl);
            }
        }

        private Uri RootRemoteUri
        {
            get
            {
                return new Uri(RemoteUri, "/");
            }
        }

        public bool TestIfRemoteAvailable()
        {
            // to test if a remote is available, we just send a badly formed WCF request to the HTTP url
            // if we get any kind of http response, then it means that there is an http server listening on that server and port
            // what we really expect to get is a BadRequest - that's what a WCF service should answer
            try
            {
                var successful = false;

                var manualResetEvent = new ManualResetEvent(false);
                var request = WebRequest.Create(RootRemoteUri);

                request.BeginGetResponse((asyncResult)=>
                                             {
                                                 try
                                                 {
                                                     var response =
                                                         request.EndGetResponse(asyncResult) as HttpWebResponse;
                                                     
                                                     // for our normal WCF servers, we really shouldn't get here... 
                                                     // ...but if we do then something is listening on that port!
                                                     successful = true;
                                                 }
                                                 catch (WebException webException)
                                                 {
                                                     // if we have a webException, check that it is due to us sending a bad request
                                                     successful =
                                                         TestForHttpBadRequest(webException.Response as HttpWebResponse);
                                                 }
                                                 catch (ThreadAbortException)
                                                 {
                                                     successful = false;
                                                     throw;
                                                 }
                                                 catch (Exception)
                                                 {
                                                     successful = false;
                                                 }
                                                 finally
                                                 {
                                                     manualResetEvent.Set();
                                                 }
                                             }, null);

                if (!manualResetEvent.WaitOne(DefaultTestRemoteTimeout))
                {
                    request.Abort();
                    return false;
                }

                return successful;
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool TestForHttpBadRequest(HttpWebResponse httpWebResponse)
        {
            if (httpWebResponse == null)
                return false;

            return httpWebResponse.StatusCode == HttpStatusCode.BadRequest;
        }

        public PhoneAutomationServiceClient CreateClient()
        {
            var binding = new BasicHttpBinding()
                              {
                                  Name = "binding1_IPhoneAutomationService",
                                  MaxBufferSize = 2147483647,
                                  MaxReceivedMessageSize = 2147483647,
                              };
            binding.Security.Mode = BasicHttpSecurityMode.None;

            var endpoint = new EndpointAddress(RemoteUrl);
            return new PhoneAutomationServiceClient(binding, endpoint);
        }

        /*
          
            WCF configuration to build is:
          
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