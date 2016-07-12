using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Hubs.Models.TeamCity
{
    public class TeamCityBuildConfig : DashboardHubModel
    {
        private string _configId;
        private string _configName;

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
    }
}
