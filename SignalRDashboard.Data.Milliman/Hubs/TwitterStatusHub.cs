using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Hubs;
using SignalRDashboard.Data.Milliman.Hubs.Models;
using SignalRDashboard.Data.Milliman.Pollers;

namespace SignalRDashboard.Data.Milliman.Hubs
{
    [HubName("twitterStatus")]
    public class TwitterStatusHub : PollingHub<TwitterStatus>
    {
        public TwitterStatusHub() 
            : base(new TrackConnectedUsersStrategy(),
                  TwitterStatusPoller.Instance)
        {
        }
    }
}
