using System;

namespace SignalRDashboard.Data.Milliman.DataSources.TeamCity
{
    public class TeamCityFullBuildResult
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public TeamCityBuildStatus Status { get; set; }
        public Uri WebUri { get; set; }
        public bool Personal { get; set; }
        public bool History { get; set; }
        public bool Pinned { get; set; }
        public string StatusText { get; set; }
        public string BuildTypeId { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        public string AgentId { get; set; }
    }
}