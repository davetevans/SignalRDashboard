using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Hubs.Models.TeamCity
{
    public class TeamCityBuildConfig : DashboardHubModel
    {
        private string _configId;
        private string _configName;
        private string _buildNumber;
        private bool _buildFailed;
        private decimal _percentageComplete;

        public string ConfigId
        {
            get { return _configId; }
            set { SetProperty(ref _configId, value); }
        }

        public string ConfigName
        {
            get { return _configName; }
            set { SetProperty(ref _configName, value); }
        }

        public string BuildNumber
        {
            get { return _buildNumber; }
            set { SetProperty(ref _buildNumber, value); }
        }

        public bool BuildFailed
        {
            get { return _buildFailed; }
            set { SetProperty(ref _buildFailed, value); }
        }

        public decimal PercentageComplete
        {
            get { return _percentageComplete; }
            set { SetProperty(ref _percentageComplete, value); }
        }
    }
}
