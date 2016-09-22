using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Hubs;
using SignalRDashboard.Data.Milliman.Hubs.Models;
using SignalRDashboard.Data.Milliman.Pollers;

namespace SignalRDashboard.Data.Milliman.Hubs
{
    [HubName("confidenceStatus")]
    public class ConfidenceStatusHub : PollingHub<ConfidenceStatus>
    {
        public ConfidenceStatusHub() 
            : base(new TrackConnectedUsersStrategy(),
                  ConfidenceStatusPoller.Instance)
        {
        }
    }
}
