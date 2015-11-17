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
    using Management.Intune;
    using Management.Intune.Models;
    using Microsoft.Azure.Commands.Intune.Properties;
    using System.Globalization;
    using System.Management.Automation;

    /// <summary>
    /// A cmdlet to link a group to Android Intune MAM policy Azure resource.
    /// </summary>
    [Cmdlet(VerbsCommon.Add, "AzureRmIntuneAndroidMAMPolicyGroup", SupportsShouldProcess = true), OutputType(typeof(PSObject))]
    public sealed class AddIntuneAndroidMAMPolicyGroupCmdlet : IntuneBaseCmdlet
    {
        /// <summary>
        /// Gets or sets the policy name
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Android policy name.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Group name
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Android group name to link to.")]
        [ValidateNotNullOrEmpty]
        public string GroupName { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "Don't ask for confirmation.")]
        public SwitchParameter Force { get; set; }
        /// <summary>
        /// Executes the cmdlet.
        /// </summary>
        protected override void ProcessRecord()
        {
            this.ConfirmAction(
                this.Force,
                string.Format(
                    CultureInfo.CurrentCulture, 
                    Resources.AddLinkedResouce_ActionMessage, 
                    Resources.Group, 
                    this.GroupName, 
                    Resources.AndroidPolicy, 
                    this.Name),
                string.Format(
                    CultureInfo.CurrentCulture, 
                    Resources.AddLinkedResources_ProcessMessage, 
                    Resources.Group, 
                    this.GroupName, 
                    Resources.AndroidPolicy, 
                    this.Name),
                this.Name,
                () =>
                {
<<<<<<< HEAD
                    this.IntuneClient.Android.AddGroupForPolicy(this.AsuHostName, this.Name, this.GroupName, PrepareMAMPolicyAppIdGroupIdPayload());
=======
                    var payLoad = AppOrGroupPayloadMaker.PrepareMAMPolicyPayload(this.IntuneClient, LinkType.Group, this.AsuHostName, this.GroupName);
                    this.IntuneClient.Android.AddGroupForMAMPolicy(this.AsuHostName, this.Name, this.GroupName, payLoad);
>>>>>>> dfd85ca0cbb0a218ef089946056f3659c39f7308
                    this.WriteObject(Resources.OperationCompletedMessage);
                });
        }
    }
}
