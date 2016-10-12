using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Hubs;
using SignalRDashboard.Data.Milliman.Hubs.Models;
using SignalRDashboard.Data.Milliman.Subscribers;

namespace SignalRDashboard.Data.Milliman.Hubs
{
    [HubName("gmailStatus")]
    public class GmailStatusHub : SubscribingHub<GmailStatus>
    {
        public GmailStatusHub()
            : base(new TrackConnectedUsersStrategy(),
                  GmailStatusSubscriber.Instance)
        {
        }
    }
}
