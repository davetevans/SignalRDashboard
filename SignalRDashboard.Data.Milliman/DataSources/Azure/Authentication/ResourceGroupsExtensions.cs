using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;

namespace SignalRDashboard.Data.Milliman.DataSources.Azure.Authentication
{
    public static class ResourceGroupsExtensions
    {
        public static void EnsureResourceGroup(this AzureConfiguration configuration, string resourceGroupName, string location)
        {
            var resourceGroupClient = CreateResourceGroupClient(configuration.AzureResourceManager, configuration.SubscriptionId);
            var resourceGroup = new ResourceGroup(location);

            if (!resourceGroupClient.ResourceGroups.CheckExistence(resourceGroupName).GetValueOrDefault(false))
            {
                resourceGroupClient.ResourceGroups.CreateOrUpdate(resourceGroupName, resourceGroup);
            }
        }

        static IResourceManagementClient CreateResourceGroupClient(AzureResourceManager configuration, string subscriptionId)
        {
            var client = new ResourceManagementClient(configuration.GetTokenCredentials())
            {
                SubscriptionId = subscriptionId
            };

            return client;
        }
    }
}
