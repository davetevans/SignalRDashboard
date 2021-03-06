using System.Collections.Generic;
using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Hubs.Models.Azure
{
    public class AzureGroupStatus : DashboardHubModel
    {
        private string _groupId;
        private string _groupName;
        private string _location;
        private IList<AzureStatStatus> _clusterStats;
        private IList<AzureStatStatus> _sqlStats;

        public string GroupId
        {
            get { return _groupId; }
            set { SetProperty(ref _groupId, value); }
        }

        public string GroupName
        {
            get { return _groupName; }
            set { SetProperty(ref _groupName, value); }
        }

        public string Location
        {
            get { return _location; }
            set { SetProperty(ref _location, value); }
        }

        public IList<AzureStatStatus> ClusterStats
        {
            get { return _clusterStats; }
            set { SetProperty(ref _clusterStats, value); }
        }

        public IList<AzureStatStatus> SqlStats
        {
            get { return _sqlStats; }
            set { SetProperty(ref _sqlStats, value); }
        }
    }
}