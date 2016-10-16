using System;

namespace SignalRDashboard.Data.Milliman.DataSources.Models
{
    public class GmailStatusData
    {
        public long LastMailId { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastMailDateTime { get; set; }
    }
}