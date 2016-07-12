using System.Collections.Generic;
using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Hubs.Models.TeamCity
{
    public class TeamCityProject : DashboardHubModel
    {
        private string _projectId;
        private string _projectName;
        private IList<TeamCityBuildConfig> _buildConfigs; 

        public string ProjectId
        {
            get { return _projectId; }
            set { SetProperty(ref _projectId, value); }
        }

        public string ProjectName
        {
            get { return _projectName; }
            set { SetProperty(ref _projectName, value); }
        }

        public IList<TeamCityBuildConfig> BuildConfigs
        {
            get { return _buildConfigs; }
            set { SetProperty(ref _buildConfigs, value); }
        }
    }
}