using Newtonsoft.Json;

namespace SignalRDashboard.Data.Milliman.Clients.Azure
{
    public class AzureResourceGroup
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("properties")]
        public GroupProperties Properties { get; set; }
    }

    public class GroupProperties
    {
        [JsonProperty("provisioningState")]
        public string ProvisioningState { get; set; }
    }
}
