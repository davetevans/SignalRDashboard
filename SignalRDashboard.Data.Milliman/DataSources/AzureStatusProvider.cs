using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SignalRDashboard.Data.Milliman.Clients;
using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class AzureStatusProvider
    {
        private readonly bool _isInitialised = false;
        private readonly IList<AzureResourceGroupData> _azureData = new List<AzureResourceGroupData>();
        private readonly List<string> _includedResourceGroups = new List<string>();
        private readonly AzureClient _client;

        public AzureStatusProvider()
        {
            var config = ConfigurationManager.AppSettings;

            _client = new AzureClient
            {
                AzureSubscriptionId = config["AzureSubscriptionId"],
                AzureApplicationId = config["AzureApplicationId"],
                AzureServicePrincipalPassword = config["AzureServicePrincipalPassword"],
                AzureTenantId = config["AzureTenantId"]
            };

            _client.Authenticate();

            foreach (var p in config["AzureIncludedResourceGroups"].Split(','))
            {
                _includedResourceGroups.Add(p);
            }
        }

        public IEnumerable<AzureResourceGroupData> GetAzureStatus()
        {
            if (!_isInitialised)
            {
                _azureData.Clear();

                try
                {
                    foreach (var group in _client.GetAzureMetrics(_includedResourceGroups).ToList())
                    {
                        _azureData.Add(group);
                    }

                }
                catch (Exception ex)
                {
                    var exx = ex;
                    // do nothing as loss of connection to Azure is handled elsewhere
                }
            }

            return _azureData;
        }
        
    }
}