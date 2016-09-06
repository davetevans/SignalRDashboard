using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Hubs.Models.Azure
{
    public class AzureResourceGroupStatStatus : DashboardHubModel
    {
        private string _clusterName;
        private string _clusterSize;
        private string _clusterState;
        private string _clusterDate;
        private string _clusterEtag;
        
        public string ClusterEtag
        {
            get { return _clusterEtag; }
            set { SetProperty(ref _clusterEtag, value); }
        }

        public string ClusterName
        {
            get { return _clusterName; }
            set { SetProperty(ref _clusterName, value); }
        }
        public string ClusterSize
        {
            get { return _clusterSize; }
            set { SetProperty(ref _clusterSize, value); }
        }
        public string ClusterState
        {
            get { return _clusterState; }
            set { SetProperty(ref _clusterState, value); }
        }
        public string ClusterDate
        {
            get { return _clusterDate; }
            set { SetProperty(ref _clusterDate, value); }
        }
    }
}
