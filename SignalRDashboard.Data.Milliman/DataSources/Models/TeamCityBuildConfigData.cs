namespace SignalRDashboard.Data.Milliman.DataSources.Models
{
    public class TeamCityBuildConfigData
    {
        public string ConfigId { get; set; }
        public string ConfigName { get; set; }
        public string BuildNumber { get; set; }
        public bool BuildFailed { get; set; }
        public bool BuildRunning { get; set; }
        public decimal PercentageComplete { get; set; }
    }
}
