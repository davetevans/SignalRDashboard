using Newtonsoft.Json;

namespace SignalRDashboard.Data.Milliman.Clients.Azure
{
    public class SqlDatabase
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string location { get; set; }
        public string kind { get; set; }

        [JsonIgnore]
        public string resourceGroupName { get; set; }
        [JsonIgnore]
        public string sqlServerName { get; set; }

        [JsonProperty("properties")]
        public DatabaseProperties properties { get; set; }
    }

    public class DatabaseProperties
    {
        public string databaseId { get; set; }
        public string edition { get; set; }
        public string status { get; set; }
        public string serviceLevelObjective { get; set; }
        public string collation { get; set; }
        public string maxSizeBytes { get; set; }
        public string creationDate { get; set; }
        public string currentServiceObjectiveId { get; set; }
        public string requestedServiceObjectiveId { get; set; }
        public string requestedServiceObjectiveName { get; set; }
        public string defaultSecondaryLocation { get; set; }
        public string earliestRestoreDate { get; set; }
        public object elasticPoolName { get; set; }
        public int containmentState { get; set; }
    }
}
