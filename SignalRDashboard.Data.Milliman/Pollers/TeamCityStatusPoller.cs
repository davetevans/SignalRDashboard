using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Pollers;
using SignalRDashboard.Data.Milliman.DataSources;
using SignalRDashboard.Data.Milliman.Hubs;
using SignalRDashboard.Data.Milliman.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Pollers
{
    public class TeamCityStatusPoller : DatasourcePoller<TeamCityStatuses, TeamCityStatusHub>
    {
        private static readonly Lazy<TeamCityStatusPoller> PollerInstance = new Lazy<TeamCityStatusPoller>(() => new TeamCityStatusPoller(GlobalHost.ConnectionManager.GetHubContext<TeamCityStatusHub>().Clients));
        private readonly ITeamCityStatusProvider _provider;

        private TeamCityStatusPoller(IHubConnectionContext<dynamic> clients)
            : base(clients, TimeSpan.FromSeconds(15), new PollOnlyWhenUsersAreConnectedStrategy())
        {
            _provider = new TeamCityStatusProvider();
        }

        public static TeamCityStatusPoller Instance => PollerInstance.Value;
        
        protected override void RefreshData(TeamCityStatuses model)
        {
            var data = _provider.GetTeamCityStatus();
            foreach (var d in data)
            {
                Model.UpdateOrAddSite(d.ProjectId, d.ProjectName, true);
            }
        }

        protected override void BroadcastData(TeamCityStatuses model)
        {
            Clients.All.updateTeamCityStatus(model.GetProjects);
        }
    }
}