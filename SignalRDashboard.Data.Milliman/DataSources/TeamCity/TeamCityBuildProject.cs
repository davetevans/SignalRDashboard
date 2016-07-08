using SignalRDashboard.Data.Milliman.Core;

namespace SignalRDashboard.Data.Milliman.DataSources.TeamCity
{
    using System;
    using System.Collections.Generic;

    public class TeamCityBuildProject : Summary
    {
        public Uri WebUri { get; set; }
        public string Description { get; set; }
        public bool Archived { get; set; }
        public IEnumerable<Summary> BuildTypes { get; set; }
        public int DocOrder { get; set; }
    }
}