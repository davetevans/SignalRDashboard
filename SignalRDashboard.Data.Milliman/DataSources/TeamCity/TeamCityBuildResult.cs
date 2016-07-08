using System;

namespace SignalRDashboard.Data.Milliman.DataSources.TeamCity
{
    public class TeamCityBuildResult
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public TeamCityBuildStatus Status { get; set; }
        public string BuildTypeId { get; set; }
        public Uri WebUri { get; set; }
    }
}