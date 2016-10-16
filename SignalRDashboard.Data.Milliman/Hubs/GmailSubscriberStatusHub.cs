﻿using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Hubs;
using SignalRDashboard.Data.Milliman.Hubs.Models;
using SignalRDashboard.Data.Milliman.Subscribers;

namespace SignalRDashboard.Data.Milliman.Hubs
{
    [HubName("gmailSubscriberStatus")]
    public class GmailSubscriberStatusHub : SubscribingHub<GmailStatus>
    {
        public GmailSubscriberStatusHub()
            : base(new TrackConnectedUsersStrategy(),
                  GmailStatusSubscriber.Instance)
        {
        }
    }
}