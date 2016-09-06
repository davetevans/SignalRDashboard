using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

namespace SignalRDashboard.Data.Milliman.Clients.Azure
{
    public static class AuthenticationHelpers
    {
        const string ArmResource = "https://management.core.windows.net/";
        const string TokenEndpoint = "https://login.windows.net/{0}/oauth2/token";
        const string SpnPayload = "resource={0}&client_id={1}&grant_type=client_credentials&client_secret={2}";

        public static string AcquireTokenBySpn(string tenantId, string clientId, string clientSecret)
        {
            var payload = string.Format(SpnPayload,
                                        WebUtility.UrlEncode(ArmResource),
                                        WebUtility.UrlEncode(clientId),
                                        WebUtility.UrlEncode(clientSecret));

            var serializer = new JavaScriptSerializer();
            var responseContent = HttpPost(tenantId, payload);
            var auth = serializer.Deserialize<AzureAuthentication>(responseContent);
            
            return auth.access_token;
        }

        private static dynamic HttpPost(string tenantId, string payload)
        {
            using (var client = new HttpClient())
            {
                var address = string.Format(TokenEndpoint, tenantId);
                var content = new StringContent(payload, Encoding.UTF8, "application/x-www-form-urlencoded");
                using (var response =  client.PostAsync(address, content).Result)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Status:  {0}", response.StatusCode);
                        Console.WriteLine("Content: {0}", response.Content.ReadAsStringAsync().Result);
                    }

                    response.EnsureSuccessStatusCode();

                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }
    }
}
