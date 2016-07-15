using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class AzureStatusProvider
    {
        private readonly bool _isInitialised = false;
        private readonly AzureClient _client = new AzureClient();
        private AzureStatusData _data = new AzureStatusData();
        
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