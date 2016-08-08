using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Hubs.Models
{
    public class NetworkStatus : DashboardHubModel
    {
        private bool _canAccessInternet;
        public bool CanAccessInternet
        {
            get { return _canAccessInternet; }
            set { SetProperty(ref _canAccessInternet, value); }
        }
        private bool _canAccessAzure;
        public bool CanAccessAzure
        {
            get { return _canAccessAzure; }
            set { SetProperty(ref _canAccessAzure, value); }
        }
        private bool _canAccessTeamCity;
        public bool CanAccessTeamCity
        {
            get { return _canAccessTeamCity; }
            set { SetProperty(ref _canAccessTeamCity, value); }
        }
    }
}