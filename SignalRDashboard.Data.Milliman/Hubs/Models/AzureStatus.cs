using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Hubs.Models
{
    public class AzureStatus : DashboardHubModel
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
    }
}