// ----------------------------------------------------------------------------------
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

using Microsoft.Azure.Commands.ApplicationInsights.Models;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Management.Internal.Resources.Utilities.Models;
using System;
using System.Management.Automation;
using System.Linq;
using Microsoft.WindowsAzure.Commands.Utilities.Common;

namespace Microsoft.Azure.Commands.ApplicationInsights
{
    [Cmdlet("Get", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "ApplicationInsightsWebTests", DefaultParameterSetName = ByResourceGroupParameterSet), OutputType(typeof(PSWebTest))]
    public class GetApplicationInsightsWebTests : ApplicationInsightsBaseCmdlet
    {
        const string ByResourceGroupParameterSet = "ByResourceGroupParameterSet";

        const string ByParentNameParameterSet = "ByParentNameParameterSet";

        const string ByParentInputObjectParameterSet = "ByParentInputObjectParameterSet";

        const string ByResourceIdParameterSet = "ByResourceIdParameterSet";

        [Parameter(
            ParameterSetName = ByResourceGroupParameterSet,
            Mandatory = false,
            HelpMessage = "list all web tests resources under the provided resource group; list all web tests under current subscription if not provided")]
        [Parameter(
            ParameterSetName = ByParentNameParameterSet,
            Mandatory = true,
            HelpMessage = "Resource Group Name.")]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        [Parameter(
            ParameterSetName = ByParentNameParameterSet,
            Mandatory = true,
            HelpMessage = "Application Insights Name.")]
        [ResourceNameCompleter("Microsoft.Insights/components", nameof(ResourceGroupName))]
        [ValidateNotNullOrEmpty]
        public string ApplicationInsightsName { get; set; }

        [Parameter(
            ParameterSetName = ByParentInputObjectParameterSet,
            Mandatory = true,
            ValueFromPipeline = true,
            HelpMessage = "Parent Application Insights Object.")]
        [ValidateNotNullOrEmpty]
        public PSApplicationInsightsComponent ParentObject { get; set; }

        [Parameter(
            ParameterSetName = ByParentNameParameterSet,
            Mandatory = true,
            HelpMessage = "Web Tests Name.")]
        [Parameter(
            ParameterSetName = ByParentInputObjectParameterSet,
            Mandatory = true,
            HelpMessage = "Web Tests Name.")]
        public string Name { get; set; }

        [Parameter(
            ParameterSetName = ByResourceIdParameterSet,
            Mandatory = true,
            HelpMessage = "Resource Id.")]
        public string ResourceId { get; set; }

        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();

            if (ParameterSetName.Equals(ByResourceGroupParameterSet))
            {
                if (!this.IsParameterBound(c => c.ResourceGroupName))
                {
                    var WebTestsResponse = this.AppInsightsManagementClient
                                   .WebTests
                                   .ListWithHttpMessagesAsync()
                                   .Result;
                    WriteObject(WebTestsResponse.Body, true);
                }
                else
                {
                    var WebTestsResponse = this.AppInsightsManagementClient
                                   .WebTests
                                   .ListByResourceGroupWithHttpMessagesAsync(ResourceGroupName)
                                   .Result;
                    
                    WriteObject(WebTestsResponse.Body.Select(w => WebTestsUtil.getPSWebTests(w)).ToArray(), true);
                }
            }
            else if (ParameterSetName.Equals(ByParentNameParameterSet) || ParameterSetName.Equals(ByParentInputObjectParameterSet) || ParameterSetName.Equals(ByResourceIdParameterSet))
            {
                string FullName = string.Format("{0}-{1}", this.Name, this.ApplicationInsightsName);

                if (ParameterSetName.Equals(ByParentInputObjectParameterSet))
                {
                    this.ApplicationInsightsName = ParentObject.Name;
                    FullName = string.Format("{0}-{1}", this.Name, this.ApplicationInsightsName);
                }
                else if (ParameterSetName.Equals(ByResourceIdParameterSet))
                {
                    ResourceIdentifier identifier = new ResourceIdentifier(this.ResourceId);
                    this.ResourceGroupName = identifier.ResourceGroupName;
                    FullName = identifier.ResourceName;
                }

                var WebTestsResponse = this.AppInsightsManagementClient
                                   .WebTests
                                   .GetWithHttpMessagesAsync(ResourceGroupName, FullName)
                                   .Result;
                WriteObject(WebTestsUtil.getPSWebTests(WebTestsResponse.Body));
            }
            else
            {
                throw new ArgumentException("Bad ParameterSet");
            }

        }
    }
}