using System;
using System.Collections.Generic;

namespace SignalRDashboard.Data.Milliman.Clients.TeamCity
{
    public class BuildProject : Summary
    {
        public Uri WebUri { get; set; }
        public string Description { get; set; }
        public bool Archived { get; set; }
        public IEnumerable<Summary> BuildTypes { get; set; }
        public int DocOrder { get; set; }
    }
}
