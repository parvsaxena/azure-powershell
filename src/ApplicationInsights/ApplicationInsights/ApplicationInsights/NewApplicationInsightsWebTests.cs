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
using Microsoft.Azure.Management.ApplicationInsights.Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace Microsoft.Azure.Commands.ApplicationInsights
{
    [Cmdlet("New", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "ApplicationInsightsWebTests", SupportsShouldProcess = true), OutputType(typeof(PSWebTest))]
    public class NewApplicationInsightsWebTests : ApplicationInsightsBaseCmdlet
    {
        [Parameter(
            Mandatory = true,
            HelpMessage = "Resource Group Name.")]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "Application Insights Resource Name.")]
        [ResourceNameCompleter("Microsoft.Insights/components", nameof(ResourceGroupName))]
        [ValidateNotNullOrEmpty]
        public string ApplicationInsightsName { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "Application Insights Web Tests Resource Name.")]
        [ResourceNameCompleter("Microsoft.Insights/components/webtests", nameof(ResourceGroupName), nameof(ApplicationInsightsName))]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "Application Insights Web Tests Location.")]
        [LocationCompleter("Microsoft.Insights/components/webtests")]
        [ValidateNotNullOrEmpty]
        public string Location { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "List of Locations where this Web Test runs.")]
        [LocationCompleter("Microsoft.Insights/components/webtests")]
        [ValidateNotNullOrEmpty]
        public String[] Location2Run { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "Application Insights Web Tests type: Ping/MultiStep.")]
        [PSArgumentCompleter("Ping", "MultiStep")]
        [ValidateNotNullOrEmpty]
        public String Kind { get; set; }

        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();

            string FullName = string.Format("{0}-{1}", Name, ApplicationInsightsName);
            WebTestKind webTestKind = Kind == "Ping" ? WebTestKind.Ping : WebTestKind.Multistep;
            var subscription = DefaultContext.Subscription?.Id;
            string webTestsResourceId = string.Format("/subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Insights/webtests/{2}", subscription, ResourceGroupName, FullName);
            string applicationInsightsResourceId = string.Format("/subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Insights/components/{2}", subscription, ResourceGroupName, ApplicationInsightsName);
            IDictionary<string, string> tags = new Dictionary<string, string>();
            tags.Add(string.Format("hidden-link:{0}", applicationInsightsResourceId), "Resource");
            IList<WebTestGeolocation> locations = Location2Run.Select(l => new WebTestGeolocation(l)).ToList();
            const string type = "Microsoft.Insights/webtests";

            WebTest WebTestDefinition = new WebTest(Location, FullName, FullName,webTestKind, locations, null, null, type, tags);

            if (ShouldProcess(FullName, String.Format("create new webtest {0} under resource group: {1}", FullName, this.ResourceGroupName)))
            {
                var WebTestsResponse = this.AppInsightsManagementClient
                                       .WebTests
                                       .CreateOrUpdateWithHttpMessagesAsync(ResourceGroupName, FullName, WebTestDefinition)
                                       .Result;
                this.WriteObject(WebTestsResponse.Body);
            }
        }
    }
}

//TODO: Optional parameters, and configuration