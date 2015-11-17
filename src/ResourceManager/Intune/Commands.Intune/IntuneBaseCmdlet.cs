﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------
namespace Microsoft.Azure.Commands.Intune
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Management.Automation;
    using Management.Intune;
    using Microsoft.Azure.Commands.Intune.Properties;
    using Microsoft.Azure.Commands.ResourceManager.Common;
    using Microsoft.Azure.Common.Authentication;
    using Microsoft.Azure.Common.Authentication.Models;
    using Microsoft.Azure.Management.Intune.Models;
    // using Microsoft.Rest.Serialization;

    /// <summary>
    /// Base class for all commandlets. Helps ceate an instance of the client that commandlets can leverage. 
    /// </summary>
    public abstract class IntuneBaseCmdlet : AzureRMCmdlet
    {
        /// <summary>
        /// Contains the errors that encountered while satisfying the request.
        /// </summary>
        internal static readonly ConcurrentBag<ErrorRecord> errors = new ConcurrentBag<ErrorRecord>();

        private static IIntuneResourceManagementClient intuneClient;

        public IIntuneResourceManagementClientWrapper intuneClientWrapper;

        /// <summary>
        /// The default parameter set.
        /// </summary>
        internal const string DefaultParameterSet = "Default Parameter Set for Intune MAM Policy cmdlets.";

        public IIntuneResourceManagementClient IntuneClient
        {
            get
            {
                if (intuneClient == null)
                {
                    intuneClient = GetIntuneManagementClient(this.DefaultContext, ApiVersion);
                }

                return intuneClient;
            }

            set
            {
                intuneClient = value;
            }
        }

        public IIntuneResourceManagementClientWrapper IntuneClientWrapper
        {
            get
            {
                if (intuneClientWrapper == null)
                {
                    intuneClientWrapper = new IntuneResourceManagementClientWrapper();
                    intuneClientWrapper.Initialize(this.DefaultContext, ApiVersion);
                }

                return intuneClientWrapper;
            }

            set
            {
                intuneClientWrapper = value;
            }
        }

        /// <summary>
        /// Gets or sets the API version.
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "When set, indicates the version of the resource provider API to use. If not specified, the API version is automatically determined as the latest available.")]
        [ValidateNotNullOrEmpty]
        public string ApiVersion { get; set; }

        /// <summary>
        /// ASU host name for the tenant
        /// </summary>
        private static string asuHostName;

        internal string AsuHostName
        {
            get
            {
                if (asuHostName == null)
                {
                    Location location = IntuneClientWrapper.GetLocationByHostName();
                    asuHostName = location.HostName;
                }

                return asuHostName;
            }
        }

        /// <summary>
        /// Gets a new instance of the <see cref="IntuneResourceManagementClient"/>.
        /// </summary>
        /// <param name="context">The azure profile.</param>
        /// <param name="apiVersion">The apiVersion of the service.</param>
        internal static IntuneResourceManagementClient GetIntuneManagementClient(AzureContext context, string apiVersion = null)
        {
            var endpoint = context.Environment.GetEndpoint(AzureEnvironment.Endpoint.ResourceManager);
            ApiVersionHandler apiVersionHandler = null;
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ApplicationException(Resources.ARMEndpointNotSetErrorMessage);
            }

            var endpointUri = new Uri(endpoint, UriKind.Absolute);

            if (!string.IsNullOrEmpty(apiVersion))
            {
                apiVersionHandler = new ApiVersionHandler(apiVersion);
                AzureSession.ClientFactory.AddHandler<ApiVersionHandler>(apiVersionHandler);
            }

            var intuneClient = AzureSession.ClientFactory.CreateArmClient<IntuneResourceManagementClient>(context, AzureEnvironment.Endpoint.ResourceManager);

            if (!string.IsNullOrEmpty(apiVersion))
            {
                AzureSession.ClientFactory.RemoveHandler(apiVersionHandler.GetType());
            }

            intuneClient.BaseUri = endpointUri;
            return intuneClient;
        }
    }
}
