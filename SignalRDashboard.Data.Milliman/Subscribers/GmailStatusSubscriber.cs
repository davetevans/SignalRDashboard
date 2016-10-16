using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Subscribers;
using SignalRDashboard.Data.Milliman.Hubs;
using SignalRDashboard.Data.Milliman.Hubs.Models;
using ImapX;

namespace SignalRDashboard.Data.Milliman.Subscribers
{
    public class GmailStatusSubscriber : DatasourceSubscriber<GmailStatus, GmailSubscriberStatusHub>
    {
        private static readonly Lazy<GmailStatusSubscriber> SubscriberInstance = new Lazy<GmailStatusSubscriber>(() => new GmailStatusSubscriber(GlobalHost.ConnectionManager.GetHubContext<GmailStatusHub>().Clients));

        private long _lastMailId;

        private static readonly List<string> InterestingKeywords = new List<string>
        {
            "coffee", "cafe", "caffe", "cofe", "cofee", "coffe", "cafe2u",
            "curry", "ice", "cream", "fire", "alarm", "van", "man",
            "french", "terrace",
            "is here",
            "test", "hello"
        };

        public static GmailStatusSubscriber Instance => SubscriberInstance.Value;

        private GmailStatusSubscriber(IHubConnectionContext<dynamic> clients)
            : base(clients)
        {
            var config = ConfigurationManager.AppSettings;
            var client = new ImapClient("imap.gmail.com", 993, true, false) { Behavior = { NoopIssueTimeout = 300 } };

            if (client.Connect())
            {
                if (client.Login(config["GmailAddress"], config["GmailPassword"]))
                {
                    client.Folders.Inbox.OnNewMessagesArrived += OnNewMessage;
                    client.Folders.Inbox.StartIdling();
                }
            }
            else
            {
                // connection not successful
            }
        }

        private void OnNewMessage(object sender, IdleEventArgs e)
        {
            var newMessage = e.Messages[0];

            if (newMessage != null && IsInteresting(newMessage.Subject))
            {
                var newMailMessage = newMessage.Subject?.Trim();
                var newMailId = newMessage.UId;
                var newMailTime = $"{(newMessage.Date ?? DateTime.Now.AddHours(-1)).AddHours(1):t}";

                if (newMailId > _lastMailId)
                {
                    var model = new GmailStatus
                    {
                        MailIsNew = true,
                        LastMail = newMailMessage,
                        LastMailTime = newMailTime
                    };
                    _lastMailId = newMailId;

                    RefreshData(model);
                }
            }
        }

        private static bool IsInteresting(string content)
        {
            return InterestingKeywords.Any(content.Contains);
        }

        protected override void RefreshData(GmailStatus model)
        {
            BroadcastData(model);
        }

        protected override void BroadcastData(GmailStatus model)
        {
            Clients.All.updateGmailStatus(model);
        }
    }
}