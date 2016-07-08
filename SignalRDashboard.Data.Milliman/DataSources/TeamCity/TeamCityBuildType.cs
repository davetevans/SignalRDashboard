namespace SignalRDashboard.Data.Milliman.DataSources.TeamCity
{
    using System;
    using System.Collections.Generic;

    public class TeamCityBuildType
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public Uri WebUri { get; set; }
        public string Description { get; set; }
        public bool Paused { get; set; }
        public string BuildProjectId { get; set; }
        public Dictionary<string, string> RunParameters { get; set; }
    }
}