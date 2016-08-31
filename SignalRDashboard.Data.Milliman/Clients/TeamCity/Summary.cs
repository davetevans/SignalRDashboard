namespace SignalRDashboard.Data.Milliman.Clients.TeamCity
{
    public class Summary
    {
        public Summary(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public Summary()
        {
        }

        public string Name { get; set; }
        public string Id { get; set; }
    }
}
