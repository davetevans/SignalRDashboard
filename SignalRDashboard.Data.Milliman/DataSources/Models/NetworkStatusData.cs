using System;

namespace SignalRDashboard.Data.Milliman.DataSources.Models
{
    public class NetworkStatusData
    {
        public bool CanAccessInternet { get; set; }
        public bool CanAccessAzure { get; set; }
        public bool CanAccessTeamCity { get; set; }
    }
}