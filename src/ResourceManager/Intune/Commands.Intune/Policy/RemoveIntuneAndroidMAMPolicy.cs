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
    using System.Globalization;  
    using System.Management.Automation;
    using Microsoft.Azure.Commands.Intune.Properties;

    /// <summary>
    /// A cmdlet that removes Intune Andriod MAM Policy.
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "AzureRmIntuneAndroidMAMPolicy", SupportsShouldProcess = true, DefaultParameterSetName = IntuneBaseCmdlet.DefaultParameterSet), OutputType(typeof(PSObject))]
    public class RemoveIntuneAndroidMAMPolicyCmdlet : IntuneBaseCmdlet
    {
        #region params
        /// <summary>
        /// The Android Policy Name
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Android Policy Name (Policy Guid) to remove the policy.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "Don't ask for confirmation.")]
        public SwitchParameter Force { get; set; }

        #endregion params

        /// <summary>
        /// Executes the cmdlet.
        /// </summary>
        protected override void ProcessRecord()
        {
            this.ConfirmAction
            (
                this.Force,
                string.Format(CultureInfo.CurrentCulture, Resources.DeleteResource_ActionMessage, Resources.AndroidPolicy, this.Name),
                string.Format(CultureInfo.CurrentCulture, Resources.DeleteResource_ProcessMessage, Resources.AndroidPolicy, this.Name),
                this.Name,
                () =>
                {
                    var res = this.IntuneClientWrapper.DeleteAndroidMAMPolicyWithHttpMessagesAsync(this.AsuHostName, this.Name);
                
                    if (res.Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        this.WriteObject(Resources.OneItemDeleted);
                    }
                    else if (res.Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        this.WriteObject(Resources.NoItemsDeleted);
                    }
                }
            );
        }
    }
}