using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using SignalRDashboard.Data.Milliman.Clients.Azure;
using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.Clients
{
    public class AzureClient
    {
        public string AzureSubscriptionId { get; set; }
        public string AzureApplicationId { get; set; }
        public string AzureServicePrincipalPassword { get; set; }
        public string AzureTenantId { get; set; }

        private static string AuthToken { get; set; }
        private static HttpClient HttpClient { get; set; }
        private static JavaScriptSerializer Serializer { get; set; }

        private const string DefaultApiVersion = "2015-01-01";
        private const string HdInsightApiVersion = "2015-03-01-preview";
        

        public AzureClient()
        {
            Serializer = new JavaScriptSerializer();
        }
        
        public void Authenticate()
        {
            AuthToken = AuthenticationHelpers.AcquireTokenBySpn(AzureTenantId, AzureApplicationId, AzureServicePrincipalPassword);
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + AuthToken);
            HttpClient.BaseAddress = new Uri("https://management.azure.com/");
        }

        public IEnumerable<AzureResourceGroupData> GetAzureMetrics(List<string> includedGroups)
        {
            var resGroups = GetAzureResourceGroups(DefaultApiVersion).Where(g => includedGroups.Contains(g.Name)).ToArray();
            var clusters = resGroups.SelectMany(g => GetHdInsightClustersForResourceGroup(HdInsightApiVersion, g.Name)).ToArray();

            var test = resGroups.Select(g => new AzureResourceGroupData
            {
                Id = g.Id,
                Name = g.Name,
                Location = g.Location,
                Stats = clusters.Where(c => c.id == g.Name)
                    .Select(c => new AzureResourceGroupStatData
                    {
                        ClusterEtag = c.etag,
                        ClusterName = $"{c.name} ({c.properties.osType} - {c.properties.clusterVersion})",
                        ClusterSize = c.properties.computeProfile.roles.Select(r => $"{r.name} ({r.targetInstanceCount} x {r.hardwareProfile.vmSize})").FirstOrDefault(),
                        ClusterState = c.properties.clusterState,
                        ClusterDate = c.properties.createdDate
                    })
            }).ToList();

            return test;
        }


        private IEnumerable<AzureResourceGroup> GetAzureResourceGroups(string apiVersion)
        {
            using (var response = HttpClient.GetAsync($"/subscriptions/{AzureSubscriptionId}/resourcegroups?api-version={apiVersion}").Result)
            {
                string responseContent;
                var stream = response.Content.ReadAsStreamAsync().Result;

                using (var reader = new StreamReader(stream))
                {
                    responseContent = reader.ReadToEnd();
                }

                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    return new List<AzureResourceGroup>();
                }

                var data = JObject.Parse(responseContent).SelectToken("value").ToString();

                return Serializer.Deserialize<List<AzureResourceGroup>>(data);
            }
        }

        private IEnumerable<HdInsightCluster> GetHdInsightClustersForResourceGroup(string apiVersion, string groupName)
        {
            using (var response = HttpClient.GetAsync($"/subscriptions/{AzureSubscriptionId}/resourceGroups/{groupName}/providers/Microsoft.HDInsight/clusters/?api-version={apiVersion}").Result)
            {
                string responseContent;
                var stream = response.Content.ReadAsStreamAsync().Result;

                using (var reader = new StreamReader(stream))
                {
                    responseContent = reader.ReadToEnd();
                }

                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    return new List<HdInsightCluster>();
                }

                var data = JObject.Parse(responseContent).SelectToken("value").ToString();

                var clusterData = Serializer.Deserialize<List<HdInsightCluster>>(data);

                // set id to be the resource group name so we can identify them
                foreach (var cd in clusterData)
                {
                    cd.id = groupName;
                }

                return clusterData;
            }
        }
    }
}