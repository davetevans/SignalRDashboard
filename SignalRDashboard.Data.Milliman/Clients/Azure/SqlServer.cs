using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignalRDashboard.Data.Milliman.Clients.Azure
{
    public class SqlServer
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string location { get; set; }
        public string kind { get; set; }

        [JsonIgnore]
        public string resourceGroupName { get; set; }

        [JsonProperty("properties")]
        public SqlServerProperties properties { get; set; }

        [JsonIgnore]
        public IEnumerable<SqlDatabase> Databases { get; set; }
    }

    public class SqlServerProperties
    {
        public string fullyQualifiedDomainName { get; set; }
        public string administratorLogin { get; set; }
        public object administratorLoginPassword { get; set; }
        public object externalAdministratorLogin { get; set; }
        public object externalAdministratorSid { get; set; }
        public string version { get; set; }
        public string state { get; set; }
    }
}
