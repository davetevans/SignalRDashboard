using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Pollers;
using SignalRDashboard.Data.Milliman.DataSources;
using SignalRDashboard.Data.Milliman.Hubs;
using SignalRDashboard.Data.Milliman.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Pollers
{
    public class ConfidenceStatusPoller : DatasourcePoller<ConfidenceStatus, ConfidenceStatusHub>
    {
        private static readonly Lazy<ConfidenceStatusPoller> PollerInstance = new Lazy<ConfidenceStatusPoller>(() => new ConfidenceStatusPoller(GlobalHost.ConnectionManager.GetHubContext<ConfidenceStatusHub>().Clients));
        private readonly ConfidenceStatusProvider _provider;

        private ConfidenceStatusPoller(IHubConnectionContext<dynamic> clients)
            : base(clients, TimeSpan.FromSeconds(900), new PollOnlyWhenUsersAreConnectedStrategy())
        {
            _provider = new ConfidenceStatusProvider();
        }

        public static ConfidenceStatusPoller Instance => PollerInstance.Value;

        protected override void RefreshData(ConfidenceStatus model)
        {
            var data = _provider.GetConfidenceStatus();
            model.TeamCityConfidence = data.TeamCityConfidence;
        }

        protected override void BroadcastData(ConfidenceStatus model)
        {
            Clients.All.updateConfidenceStatus(model);
        }
    }
}