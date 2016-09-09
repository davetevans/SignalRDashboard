using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Hubs.Models.Azure
{
    public class AzureClusterStatStatus : DashboardHubModel
    {
        private string _clusterState;
        private int _clusterCount;
        
        public string ClusterState
        {
            get { return _clusterState; }
            set { SetProperty(ref _clusterState, value); }
        }

        public int ClusterCount
        {
            get { return _clusterCount; }
            set { SetProperty(ref _clusterCount, value); }
        }
    }
}
