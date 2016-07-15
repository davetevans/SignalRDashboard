using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Pollers;
using SignalRDashboard.Data.Milliman.DataSources;
using SignalRDashboard.Data.Milliman.Hubs;
using SignalRDashboard.Data.Milliman.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Pollers
{
    public class AzureStatusPoller : DatasourcePoller<AzureStatus, AzureStatusHub>
    {
        private static readonly Lazy<AzureStatusPoller> PollerInstance = new Lazy<AzureStatusPoller>(() => new AzureStatusPoller(GlobalHost.ConnectionManager.GetHubContext<AzureStatusHub>().Clients));
        private readonly AzureStatusProvider _provider;

        private AzureStatusPoller(IHubConnectionContext<dynamic> clients)
            : base(clients, TimeSpan.FromSeconds(15), new PollOnlyWhenUsersAreConnectedStrategy())
        {
            _provider = new AzureStatusProvider();
        }

        public static AzureStatusPoller Instance => PollerInstance.Value;

        protected override void RefreshData(AzureStatus model)
        {
            var data = _provider.GetAzureStatus();
            model.Id = data.Id;
            model.Name = data.Name;
        }

        protected override void BroadcastData(AzureStatus model)
        {
            Clients.All.updateAzureStatus(model);
        }
    }
}