namespace SignalRDashboard.Data.Milliman.DataSources.Azure.Authentication
{
    public class AzureResourceManager
    {
        public string AuthenticationEndpoint { get; set; }
        public string ClientId { get; set; }
        public string Resource { get; set; }
        public string ClientSecret { get; set; }
    }
}