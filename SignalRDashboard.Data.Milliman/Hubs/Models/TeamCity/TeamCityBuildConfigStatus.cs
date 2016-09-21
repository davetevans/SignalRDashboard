using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Hubs.Models.TeamCity
{
    public class TeamCityBuildConfigStatus : DashboardHubModel
    {
        private string _configId;
        private string _configName;
        private string _buildNumber;
        private bool _buildFailed;
        private string _buildTime;
        private bool _buildRunning;
        private decimal _percentageComplete;
        private int _buildFailedMessageReceivedCount;
        private bool _buildNewlyFailed;

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

        public bool BuildRunning
        {
            get { return _buildRunning; }
            set { SetProperty(ref _buildRunning, value); }
        }

        public decimal PercentageComplete
        {
            get { return _percentageComplete; }
            set { SetProperty(ref _percentageComplete, value); }
        }

        public int BuildFailedMessageReceivedCount
        {
            get { return _buildFailedMessageReceivedCount; }
            set { SetProperty(ref _buildFailedMessageReceivedCount, value); }
        }

        public bool BuildNewlyFailed
        {
            get { return _buildNewlyFailed; }
            set { SetProperty(ref _buildNewlyFailed, value); }
        }
    }
}
