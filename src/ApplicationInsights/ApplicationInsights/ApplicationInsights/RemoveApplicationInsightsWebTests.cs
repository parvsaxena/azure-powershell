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
using System.Management.Automation;

namespace Microsoft.Azure.Commands.ApplicationInsights
{
    [Cmdlet("Remove", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "ApplicationInsightsWebTests", DefaultParameterSetName = ByParentNameParameterSet, SupportsShouldProcess = true), OutputType(typeof(bool))]
    public class ApplRemoveicationInsightsWebTests : ApplicationInsightsBaseCmdlet
    {
        const string ByParentNameParameterSet = "ByParentNameParameterSet";

        const string ByParentInputObjectParameterSet = "ByParentInputObjectParameterSet";

        const string ByResourceIdParameterSet = "ByResourceIdParameterSet";

        const string ByInputObjectParameterSet = "ByInputObjectParameterSet";

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
            ParameterSetName = ByInputObjectParameterSet,
            Mandatory = true,
            ValueFromPipeline = true,
            HelpMessage = "WebTests Object.")]
        [ValidateNotNullOrEmpty]
        public PSWebTest InputObject { get; set; }

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
            else if (ParameterSetName.Equals(ByInputObjectParameterSet))
            {
                ResourceIdentifier identifier = new ResourceIdentifier(this.InputObject.Id);
                this.ResourceGroupName = identifier.ResourceGroupName;
                FullName = this.InputObject.SyntheticMonitorId;
            }

            if (ShouldProcess(FullName, string.Format("delete webtest {0} from resource group: {1}", FullName, this.ResourceGroupName)))
            {
                var WebTestsResponse = this.AppInsightsManagementClient
                               .WebTests
                               .DeleteWithHttpMessagesAsync(ResourceGroupName, FullName)
                               .Result;
                WriteObject(true);
            } 
        }
    }
}
//TODO: follow example