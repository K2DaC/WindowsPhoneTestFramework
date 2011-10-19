// ----------------------------------------------------------------------
// <copyright file="ServiceHostController.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using WindowsPhoneTestFramework.AutomationController.Interfaces;
using WindowsPhoneTestFramework.AutomationController.Service;
using WindowsPhoneTestFramework.Utils;

namespace WindowsPhoneTestFramework.AutomationController
{
    public class ServiceHostController : TraceBase, IDisposable
    {
        public static readonly Uri DefaultBindingAddress = new Uri("http://localhost:8085/phoneAutomation");

        private ServiceHost _serviceHost;
        private IPhoneAutomationController _automationController;

        public Uri BindingAddress { get; set; }
        public AutomationIdentification AutomationIdentification { get; set; }

        public IPhoneAutomationController Controller
        {
            get
            {
                if (_automationController == null)
                    throw new InvalidOperationException("AutomationController not available");

                return _automationController;
            }
        }

        public ServiceHostController()
        {
            BindingAddress = DefaultBindingAddress;
            AutomationIdentification = AutomationIdentification.TryEverything;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                Stop();
            }
        }

        public void Start()
        {
            if (_serviceHost != null)
                throw new InvalidOperationException("_serviceHost already started");

            if (_automationController != null)
                throw new InvalidOperationException("_automationController already created");

            InvokeTrace("building host...");

            // build the service
            var phoneAutomationService = new PhoneAutomationService();
            phoneAutomationService.Trace += (sender, args) => InvokeTrace(args);
            var serviceHost = new ServiceHost(phoneAutomationService, BindingAddress);

            // Enable metadata publishing
            var smb = new ServiceMetadataBehavior
            {
                HttpGetEnabled = true,
                MetadataExporter = { PolicyVersion = PolicyVersion.Policy15 }
            };
            serviceHost.Description.Behaviors.Add(smb);

            // build SOAP ServiceEndpoint
            serviceHost.AddServiceEndpoint(
                                            typeof(IPhoneAutomationService),
                                            GetHttpBinding(),
                                            BindingAddress + "/automate");

            // build JSON ServiceEndpoint
            var jsonServiceEndpoint = serviceHost.AddServiceEndpoint(
                                                        typeof(IPhoneAutomationService),
                                                        GetWebHttpBinding(),
                                                        BindingAddress + "/jsonAutomate");
            var webHttpBehavior = new WebHttpBehavior()
                                      {
                                          DefaultOutgoingRequestFormat = WebMessageFormat.Json,
                                          DefaultOutgoingResponseFormat = WebMessageFormat.Json,
                                          DefaultBodyStyle = WebMessageBodyStyle.Wrapped
                                      };
            jsonServiceEndpoint.Behaviors.Add(webHttpBehavior);

            // open the host
            InvokeTrace("opening host...");
            serviceHost.Open();
            InvokeTrace("host open");

            _automationController = new PhoneAutomationController(phoneAutomationService, AutomationIdentification);
            _serviceHost = serviceHost;
        }

        public void Stop()
        {
            _automationController = null;
            if (_serviceHost != null)
            {
                InvokeTrace("closing host");
                _serviceHost.Close();
                InvokeTrace("host closed");
                _serviceHost = null;
            }
        }

        private static Binding GetHttpBinding()
        {
            var binding = new BasicHttpBinding()
            {
                Name = "SOAP",
                MaxReceivedMessageSize = 1000000,
                MaxBufferSize = 1000000,
                ReaderQuotas = { MaxStringContentLength = 1000000 },
                HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                Security = { Mode = BasicHttpSecurityMode.None }
            };
            return binding;
        }

        private static Binding GetWebHttpBinding()
        {
            var binding = new WebHttpBinding()
            {
                Name = "JSON",
                MaxReceivedMessageSize = 1000000,
                MaxBufferSize = 1000000,
                ReaderQuotas = { MaxStringContentLength = 1000000 },
                HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                Security = { Mode = WebHttpSecurityMode.None }
            };
            return binding;
        }
    }
}
