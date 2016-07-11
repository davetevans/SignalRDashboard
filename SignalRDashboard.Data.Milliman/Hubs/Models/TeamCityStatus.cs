using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Hubs.Models
{
    public class TeamCityStatus : DashboardHubModel
    {
        private string _projectId;
        private string _projectName;
        private bool _include;

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
        
        public bool Include
        {
            get { return _include; }
            set
            {
                SetProperty(ref _include, value);
            }
        }
    }
}