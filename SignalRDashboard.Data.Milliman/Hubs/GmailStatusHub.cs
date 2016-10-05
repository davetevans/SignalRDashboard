using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Hubs;
using SignalRDashboard.Data.Milliman.Hubs.Models;
using SignalRDashboard.Data.Milliman.Pollers;

namespace SignalRDashboard.Data.Milliman.Hubs
{
    [HubName("gmailStatus")]
    public class GmailStatusHub : PollingHub<GmailStatus>
    {
        public GmailStatusHub() 
            : base(new TrackConnectedUsersStrategy(),
                  GmailStatusPoller.Instance)
        {
        }
    }
}
