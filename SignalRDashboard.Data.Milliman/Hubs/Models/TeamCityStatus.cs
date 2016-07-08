using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Hubs.Models
{
    public class TeamCityStatus : DashboardHubModel
    {
        private string _projectId;
        public string ProjectId
        {
            get { return _projectId; }
            set { SetProperty(ref _projectId, value); }
        }
        private string _projectName;
        public string ProjectName
        {
            get { return _projectName; }
            set { SetProperty(ref _projectName, value); }
        }
    }
}