namespace SignalRDashboard.Data.Milliman.DataSources.Azure.Authentication
{
    public class AzureActiveDirectory
    {
        public string Resource { get; set; }
        public string ClientId { get; set; }
        public string AuthenticationEndpoint { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AuthenticationEndpointWithTenant { get; set; }
    }
}