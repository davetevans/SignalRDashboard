namespace SignalRDashboard.Data.Milliman.DataSources.Models.TeamCity
{
    public class BuildData
    {
        public string ConfigId { get; set; }
        public string ConfigName { get; set; }
        public string BuildNumber { get; set; }
        public bool BuildFailed { get; set; }
        public string BuildTime { get; set; }
    }
}
