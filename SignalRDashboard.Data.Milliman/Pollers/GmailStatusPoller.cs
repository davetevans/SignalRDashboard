using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Pollers;
using SignalRDashboard.Data.Milliman.DataSources;
using SignalRDashboard.Data.Milliman.Hubs;
using SignalRDashboard.Data.Milliman.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Pollers
{
    public class GmailStatusPoller : DatasourcePoller<GmailStatus, GmailStatusHub>
    {
        private static readonly Lazy<GmailStatusPoller> PollerInstance = new Lazy<GmailStatusPoller>(() => new GmailStatusPoller(GlobalHost.ConnectionManager.GetHubContext<GmailStatusHub>().Clients));
        private readonly GmailStatusProvider _provider;
        private DateTime _lastMailDateTime = DateTime.Now;
        private long _lastMailId;

        private GmailStatusPoller(IHubConnectionContext<dynamic> clients)
            : base(clients, TimeSpan.FromSeconds(15), new PollOnlyWhenUsersAreConnectedStrategy())
        {
            _provider = new GmailStatusProvider();
        }

        public static GmailStatusPoller Instance => PollerInstance.Value;

        protected override void RefreshData(GmailStatus model)
        {
            var data = _provider.GetGmailStatus();
            var lastMail = data.LastMessage?.Trim();
            var lastMailId = data.LastMailId;
            var lastMailDateTime = data.LastMailDateTime;
            var lastMailTime = $"{lastMailDateTime:t}";
            model.MailIsNew = false;

            if (lastMailDateTime > _lastMailDateTime)
            {
                model.MailIsNew = lastMailId == 0 || lastMailId > _lastMailId;
                model.LastMail = lastMail;
                model.LastMailTime = lastMailTime;
                _lastMailDateTime = lastMailDateTime;
                _lastMailId = lastMailId;
            }
        }

        protected override void BroadcastData(GmailStatus model)
        {
            Clients.All.updateGmailStatus(model);
        }
    }
}