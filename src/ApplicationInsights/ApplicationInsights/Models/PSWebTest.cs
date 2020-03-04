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

using System.Collections.Generic;

namespace Microsoft.Azure.Commands.ApplicationInsights.Models
{
    public class PSWebTest : PSWebTestsResource
    {
        public PSWebTest(string location, string syntheticMonitorId, string webTestName, PSWebTestKind webTestKind, PSWebTestKind kind, IList<PSWebTestGeoLocation> locations, string id, string name, string type, IDictionary<string, string> tags = default(IDictionary<string, string>), string description = default(string), bool? enabled = default(bool?), int? frequency = default(int?), int? timeout = default(int?), bool? retryEnabled = default(bool?), PSWebTestPropertiesConfiguration configuration = default(PSWebTestPropertiesConfiguration), string provisioningState = default(string))
            : base(location, id, name, type, tags)
        {
            this.WebTestKind = webTestKind;
            this.Kind = kind;
            this.SyntheticMonitorId = syntheticMonitorId;
            this.WebTestName = webTestName;
            this.Description = description;
            this.Enabled = enabled;
            this.Frequency = frequency;
            this.Timeout = timeout;
            this.RetryEnabled = retryEnabled;
            this.Locations = locations;
            this.Configuration = configuration;
            this.ProvisioningState = provisioningState;
        }

        public PSWebTestKind? Kind { get; set; }

        public string SyntheticMonitorId { get; set; }

        public string WebTestName { get; set; }

        public string Description { get; set; }

        public bool? Enabled { get; set; }

        public int? Frequency { get; set; }

        public int? Timeout { get; set; }

        public PSWebTestKind WebTestKind { get; set; }

        public bool? RetryEnabled { get; set; }

        public IList<PSWebTestGeoLocation> Locations { get; set; }

        public PSWebTestPropertiesConfiguration Configuration { get; set; }

        public string ProvisioningState { get; private set; }
    }
}