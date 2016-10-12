using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AE.Net.Mail;
using AE.Net.Mail.Imap;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Subscribers;
using SignalRDashboard.Data.Milliman.Hubs;
using SignalRDashboard.Data.Milliman.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Subscribers
{
    public class GmailStatusSubscriber : DatasourceSubscriber<GmailStatus, GmailStatusHub>
    {
        private static readonly Lazy<GmailStatusSubscriber> SubscriberInstance = new Lazy<GmailStatusSubscriber>(() => new GmailStatusSubscriber(GlobalHost.ConnectionManager.GetHubContext<GmailStatusHub>().Clients));

        private readonly ImapClient _client;
        private DateTime _lastMailDateTime;
        private string _lastMailId;

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
            _client = new ImapClient("imap.gmail.com", config["GmailAddress"], config["GmailPassword"], AuthMethods.Login, 993, true);
            _client.SelectMailbox("Inbox");
            _client.NewMessage += OnNewMessage;
        }

        private void OnNewMessage(object sender, MessageEventArgs e)
        {
            var newMessage = _client.GetMessage(e.MessageCount - 1);

            if (newMessage != null && IsInteresting(newMessage.Subject))
            {
                var lastMail = newMessage.Subject?.Trim();
                var lastMailId = newMessage.MessageID;
                var lastMailTime = $"{newMessage.Date:t}";
                var lastMailDateTime = newMessage.Date;

                if (lastMailDateTime > _lastMailDateTime && lastMailId != _lastMailId)
                {
                    var model = new GmailStatus
                    {
                        MailIsNew = true,
                        LastMail = lastMail,
                        LastMailTime = lastMailTime
                    };
                    _lastMailDateTime = lastMailDateTime;
                    _lastMailId = lastMailId;

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