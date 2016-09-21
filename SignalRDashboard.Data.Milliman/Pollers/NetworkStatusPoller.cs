using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Pollers;
using SignalRDashboard.Data.Milliman.DataSources;
using SignalRDashboard.Data.Milliman.Hubs;
using SignalRDashboard.Data.Milliman.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Pollers
{
    public class NetworkStatusPoller : DatasourcePoller<NetworkStatus, NetworkStatusHub>
    {
        private static readonly Lazy<NetworkStatusPoller> PollerInstance = new Lazy<NetworkStatusPoller>(() => new NetworkStatusPoller(GlobalHost.ConnectionManager.GetHubContext<NetworkStatusHub>().Clients));
        private readonly NetworkStatusProvider _provider;

        private NetworkStatusPoller(IHubConnectionContext<dynamic> clients)
            : base(clients, TimeSpan.FromSeconds(30), new PollOnlyWhenUsersAreConnectedStrategy())
        {
            _provider = new NetworkStatusProvider();
        }

        public static NetworkStatusPoller Instance => PollerInstance.Value;

        protected override void RefreshData(NetworkStatus model)
        {
            var data = _provider.GetNetworkStatus();
            model.CanAccessInternet = data.CanAccessInternet;
            model.CanAccessAzure = data.CanAccessAzure;
            model.CanAccessTeamCity = data.CanAccessTeamCity;
        }

        protected override void BroadcastData(NetworkStatus model)
        {
            Clients.All.updateNetworkStatus(model);
        }
    }
}