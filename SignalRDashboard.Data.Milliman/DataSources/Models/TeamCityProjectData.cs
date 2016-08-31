using System.Collections.Generic;

namespace SignalRDashboard.Data.Milliman.DataSources.Models
{
    public class TeamCityProjectData
    {
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }

        public IList<TeamCityBuildConfigData> BuildConfigs { get; set; } 
    }
}