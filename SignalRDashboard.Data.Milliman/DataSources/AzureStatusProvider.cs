using System.Configuration;
using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class AzureStatusProvider
    {
        private readonly bool _isInitialised = false;
        private readonly AzureClient _client = new AzureClient();
        private AzureStatusData _data = new AzureStatusData();

        public AzureStatusProvider()
        {
            var config = ConfigurationManager.AppSettings;
            _client.AzureSubscriptionId = config["AzureSubscriptionId"];
            _client.AzureApplicationId = config["AzureApplicationId"];
            _client.AzureServicePrincipalPassword = config["AzureServicePrincipalPassword"];
            _client.AzureTenantId = config["AzureTenantId"];
            _client.Authenticate();
        }

        public AzureStatusData GetAzureStatus()
        {
            if (!_isInitialised)
            {
                _data = _client.GetAzureMetrics();
            }

            return _data;
        }
        
    }
}