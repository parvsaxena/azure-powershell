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
using Microsoft.Azure.Management.ApplicationInsights.Management.Models;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Azure.Commands.ApplicationInsights
{
    internal static class WebTestsUtil
    {
        internal static PSWebTest getPSWebTests(WebTest webTest)
        {
            return new PSWebTest(webTest.Location, 
                                 webTest.SyntheticMonitorId, 
                                 webTest.WebTestName, 
                                 getPSWebTestKind(webTest.WebTestKind),
                                 getPSWebTestKind(webTest.Kind),
                                 getPSWebTestGeoLocationList(webTest.Locations), 
                                 webTest.Id, 
                                 webTest.Name, 
                                 webTest.Type,
                                 webTest.Tags,
                                 webTest.Description,
                                 webTest.Enabled,
                                 webTest.Frequency,
                                 webTest.Timeout,
                                 webTest.RetryEnabled,
                                 getPSWebTestPropertiesConfiguration(webTest.Configuration),
                                 webTest.ProvisioningState);
        }   

        internal static PSWebTestKind getPSWebTestKind(WebTestKind? webTestKind)
        {   
            PSWebTestKind result;
            if (WebTestKind.Multistep == webTestKind)
            {
                result = PSWebTestKind.Multistep;
            }
            else
            {
                result = PSWebTestKind.Ping;
            }

            return result;
        }

        internal static PSWebTestGeoLocation getPSWebTestGeoLocation(WebTestGeolocation webTestGeolocation)
        {
            return new PSWebTestGeoLocation(webTestGeolocation.Location);
        }

        internal static IList<PSWebTestGeoLocation> getPSWebTestGeoLocationList(IList<WebTestGeolocation> locations)
        {
            return locations.Select(l => getPSWebTestGeoLocation(l)).ToList();
        }

        internal static PSWebTestPropertiesConfiguration getPSWebTestPropertiesConfiguration(WebTestPropertiesConfiguration configuration)
        {
            return new PSWebTestPropertiesConfiguration(configuration.WebTest);
        }
    }
}