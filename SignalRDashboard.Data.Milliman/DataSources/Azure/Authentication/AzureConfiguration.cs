namespace SignalRDashboard.Data.Milliman.DataSources.Azure.Authentication
{
    public class AzureConfiguration
    {
        public AzureConfiguration()
        {
            AzureActiveDirectory = new AzureActiveDirectory();
            AzureResourceManager = new AzureResourceManager();
        }

        public string SubscriptionId { get; set; }
        public AzureActiveDirectory AzureActiveDirectory { get; set; }
        public AzureResourceManager AzureResourceManager { get; set; }
    }
}
