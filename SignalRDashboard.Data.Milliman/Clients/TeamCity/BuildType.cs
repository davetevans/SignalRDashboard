using System;
using System.Collections.Generic;

namespace SignalRDashboard.Data.Milliman.Clients.TeamCity
{
    public class BuildType
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
