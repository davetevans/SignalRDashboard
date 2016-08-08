using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Hubs;
using SignalRDashboard.Data.Milliman.Hubs.Models;
using SignalRDashboard.Data.Milliman.Pollers;

namespace SignalRDashboard.Data.Milliman.Hubs
{
    [HubName("networkStatus")]
    public class NetworkStatusHub : PollingHub<NetworkStatus>
    {
        public NetworkStatusHub() 
            : base(new TrackConnectedUsersStrategy(),
                  NetworkStatusPoller.Instance)
        {
        }
    }
}
