using System.Collections.Generic;

namespace SignalRDashboard.Data.Milliman.DataSources.Models
{
    public class AzureResourceGroupData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public IEnumerable<AzureResourceGroupStatData> Stats { get; set; } 
    }
}