using System.Collections.Generic;

namespace SignalRDashboard.Data.Milliman.Hubs.Models.Azure
{
    public static class ClusterState
    {
        public static List<string> Provisioning = new List<string>
        {
            "Accepted",
            "AzureVMConfiguration",
            "CertRolloverQueued",
            "ClusterCustomization",
            "ClusterStorageProvisioned",
            "ResizeQueued",
            "HDInsightConfiguration",
            "PatchQueued",
            "ReadyForDeployment"
        };

        public static List<string> Running = new List<string>
        {
            "Running",
            "Operational"
        };

        public static List<string> Deleting = new List<string>
        {
            "DeletePending",
            "Deleting",
            "TimedOut"
        };

        public static List<string> Errored = new List<string>
        {
            "Error",
            "Unknown",
            "TimedOut"
        };
    }
}
