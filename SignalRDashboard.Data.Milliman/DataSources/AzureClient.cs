using Microsoft.Azure.Management.ResourceManager;
using SignalRDashboard.Data.Milliman.DataSources.Azure.Authentication;
using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class AzureClient
    {
        static string subscriptionId = "744d70a8-9ad2-4960-8b87-bfaa01f52c16";

        public AzureStatusData GetAzureMetrics()
        {
            //var restCredentials = Config.RmAccount.GetAzureServiceCredentials();
            //var cloudCredentials = Config.StorageAccount.GetAzureCredentials(subscriptionId);

            //var res = new ResourceManagementClient(restCredentials) { SubscriptionId = subscriptionId };

            //var hdis = HdInsights(cloudCredentials);
            //var allResources = res.Resources.List();
            //var databases = new List<DatabaseResource>();

            var data = new AzureStatusData {Id = "test", Name = "Test"};
            return data;
        }
    }
}