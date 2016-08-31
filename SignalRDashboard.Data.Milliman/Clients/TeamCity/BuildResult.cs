namespace SignalRDashboard.Data.Milliman.Clients.TeamCity
{
    using System;
    public class BuildResult
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public BuildStatus Status { get; set; }
        public string BuildTypeId { get; set; }
        public Uri WebUri { get; set; }
        public bool IsRunning { get; set; }
        public decimal PercentageComplete { get; set; }
    }
}
