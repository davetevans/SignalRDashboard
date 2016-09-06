using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignalRDashboard.Data.Milliman.Clients.Azure
{
    public class HdInsightCluster
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string location { get; set; }
        public string etag { get; set; }
        public object tags { get; set; }

        [JsonProperty("properties")]
        public ClusterProperties properties { get; set; }
    }

    public class ClusterProperties
    {
        public string clusterVersion { get; set; }
        public string osType { get; set; }
        public ClusterDefinition clusterDefinition { get; set; }
        public ComputeProfile computeProfile { get; set; }
        public string provisioningState { get; set; }
        public string clusterState { get; set; }
        public string createdDate { get; set; }
        public QuotaInfo quotaInfo { get; set; }
        public List<ConnectivityEndpoint> connectivityEndpoints { get; set; }
        public string tier { get; set; }
    }

    public class ClusterDefinition
    {
        public string blueprint { get; set; }
        public string kind { get; set; }
    }

    public class HardwareProfile
    {
        public string vmSize { get; set; }
    }

    public class LinuxOperatingSystemProfile
    {
        public string username { get; set; }
    }

    public class OsProfile
    {
        public LinuxOperatingSystemProfile linuxOperatingSystemProfile { get; set; }
    }

    public class Role
    {
        public string name { get; set; }
        public int targetInstanceCount { get; set; }
        public HardwareProfile hardwareProfile { get; set; }
        public OsProfile osProfile { get; set; }
    }

    public class ComputeProfile
    {
        public List<Role> roles { get; set; }
    }

    public class QuotaInfo
    {
        public int coresUsed { get; set; }
    }

    public class ConnectivityEndpoint
    {
        public string name { get; set; }
        public string protocol { get; set; }
        public string location { get; set; }
        public int port { get; set; }
    }

}
