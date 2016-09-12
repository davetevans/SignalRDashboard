using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using SignalRDashboard.Data.Milliman.Clients.Azure;
using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.Clients
{
    public class AzureClient
    {
        public string AzureSubscriptionId { private get; set; }
        public string AzureApplicationId { private get; set; }
        public string AzureServicePrincipalPassword { private get; set; }
        public string AzureTenantId { private get; set; }

        private static string AuthToken { get; set; }
        private static HttpClient HttpClient { get; set; }
        private static JavaScriptSerializer Serializer { get; set; }

        private const string DefaultApiVersion = "2015-01-01";
        private const string HdInsightApiVersion = "2015-03-01-preview";
        private const string SqlServerApiVersion = "2014-04-01-preview";

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
            var resGroups = GetResourceGroups().Where(g => includedGroups.Contains(g.Name)).ToArray();
            var clusters = resGroups.SelectMany(g => GetClusters(g.Name)).ToArray();
            var databases = resGroups.SelectMany(g => GetDatabases(g.Name)).ToArray();

            var groupsWithStats = resGroups.Select(g => new AzureResourceGroupData
            {
                Id = g.Id,
                Name = g.Name,
                Location = g.Location,
                ClusterStats = clusters.Where(c => c.resourceGroupName == g.Name)
                    .Select(c => new AzureStatData
                    {
                        Name = $"{c.name} ({c.properties.osType} - {c.properties.clusterVersion})",
                        Size = string.Join(" | ", c.properties.computeProfile.roles.Where(r => r.name != "zookeepernode").Select(r => $"{r.targetInstanceCount} x {r.hardwareProfile.vmSize}")),
                        State = c.properties.clusterState,
                        Date = c.properties.createdDate
                    }),
                SqlStats = databases.Where(d => d.resourceGroupName == g.Name)
                    .Select(d => new AzureStatData
                    {
                        Name = $"{d.name} ({d.sqlServerName})",
                        Size = d.properties.serviceLevelObjective,
                        State = d.properties.status,
                        Date = d.properties.creationDate
                    })
            }).ToList();

            return groupsWithStats.Where(g => g.ClusterStats.Any() || g.SqlStats.Any());
        }


        private IEnumerable<AzureResourceGroup> GetResourceGroups()
        {
            var request = $"/subscriptions/{AzureSubscriptionId}/resourcegroups?api-version={DefaultApiVersion}";
            return GetAzureResponse<List<AzureResourceGroup>>(request);
        }

        private IEnumerable<HdInsightCluster> GetClusters(string groupName)
        {
            var request = $"/subscriptions/{AzureSubscriptionId}/resourceGroups/{groupName}/providers/Microsoft.HDInsight/clusters/?api-version={HdInsightApiVersion}";
            var clusterData = GetAzureResponse<List<HdInsightCluster>>(request);
            
            foreach (var cd in clusterData)
            {
                cd.resourceGroupName = groupName;
            }

            return clusterData;
        }

        private IEnumerable<SqlDatabase> GetDatabases(string groupName)
        {
            var databases = new List<SqlDatabase>();
            var request = $"/subscriptions/{AzureSubscriptionId}/resourceGroups/{groupName}/providers/Microsoft.Sql/servers?api-version={SqlServerApiVersion}";
            var sqlServers = GetAzureResponse<List<SqlServer>>(request);

            foreach (var s in sqlServers)
            {
                request = $"/subscriptions/{AzureSubscriptionId}/resourceGroups/{groupName}/providers/Microsoft.Sql/servers/{s.name}/databases?api-version={SqlServerApiVersion}";
                databases = GetAzureResponse<List<SqlDatabase>>(request);

                foreach (var d in databases.Where(e => e.properties.edition != "System"))
                {
                    d.resourceGroupName = groupName;
                    d.sqlServerName = s.name;
                }
            }
            
            return databases;
        }

        private static T GetAzureResponse<T>(string request)
        {
            using (var response = HttpClient.GetAsync(request).Result)
            {
                string responseContent;
                var stream = response.Content.ReadAsStreamAsync().Result;

                using (var reader = new StreamReader(stream))
                {
                    responseContent = reader.ReadToEnd();
                }

                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    return (T)Activator.CreateInstance(typeof(T));
                }

                var data = JObject.Parse(responseContent).SelectToken("value").ToString();

                return Serializer.Deserialize<T>(data);
            }
        }
    }
}