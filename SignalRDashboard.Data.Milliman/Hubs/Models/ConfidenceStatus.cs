using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Hubs.Models
{
    public class ConfidenceStatus : DashboardHubModel
    {
        private decimal _teamCityConfidence;
        public decimal TeamCityConfidence
        {
            get { return _teamCityConfidence; }
            set { SetProperty(ref _teamCityConfidence, value); }
        }
    }
}