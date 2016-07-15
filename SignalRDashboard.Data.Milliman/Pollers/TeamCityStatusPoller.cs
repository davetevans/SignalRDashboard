using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Pollers;
using SignalRDashboard.Data.Milliman.DataSources;
using SignalRDashboard.Data.Milliman.Hubs;
using SignalRDashboard.Data.Milliman.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Pollers
{
    public class TeamCityStatusPoller : DatasourcePoller<TeamCityStatus, TeamCityStatusHub>
    {
        private static readonly Lazy<TeamCityStatusPoller> PollerInstance = new Lazy<TeamCityStatusPoller>(() => new TeamCityStatusPoller(GlobalHost.ConnectionManager.GetHubContext<TeamCityStatusHub>().Clients));
        private readonly TeamCityStatusProvider _provider;

        private TeamCityStatusPoller(IHubConnectionContext<dynamic> clients)
            : base(clients, TimeSpan.FromSeconds(15), new PollOnlyWhenUsersAreConnectedStrategy())
        {
            _provider = new TeamCityStatusProvider();
        }
        
        protected override void RefreshData(TeamCityStatus model)
        {
            foreach (var project in _provider.GetTeamCityStatus())
            {
                model.UpdateOrAddProject(project);
            }
        }

        public static TeamCityStatusPoller Instance => PollerInstance.Value;

        protected override void BroadcastData(TeamCityStatus model)
        {
            Clients.All.updateTeamCityStatus(model.GetProjects);
        }
    }
}