using System.Collections.Generic;

namespace SignalRDashboard.Data.Milliman.DataSources.Models.TeamCity
{
    public class ProjectData
    {
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }

        public IList<BuildData> BuildConfigs { get; set; } 
    }
}