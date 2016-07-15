using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Hubs;
using SignalRDashboard.Data.Milliman.Hubs.Models;
using SignalRDashboard.Data.Milliman.Pollers;

namespace SignalRDashboard.Data.Milliman.Hubs
{
    [HubName("azureStatus")]
    public class AzureStatusHub : PollingHub<AzureStatus>
    {
        public AzureStatusHub() 
            : base(new TrackConnectedUsersStrategy(),
                  AzureStatusPoller.Instance)
        {
        }
    }
}
