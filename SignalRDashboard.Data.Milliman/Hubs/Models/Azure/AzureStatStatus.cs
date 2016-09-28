using System;
using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Hubs.Models.Azure
{
    public class AzureStatStatus : DashboardHubModel
    {
        private string _groupName;
        private string _count;
        private string _aliveTime;

        public string GroupName
        {
            get { return _groupName; }
            set { SetProperty(ref _groupName, value); }
        }

        public string Count
        {
            get { return _count; }
            set { SetProperty(ref _count, value); }
        }

        public string AliveTime
        {
            get { return _aliveTime; }
            set { SetProperty(ref _aliveTime, value); }
        }
    }
}
