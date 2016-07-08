using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Hubs;
using SignalRDashboard.Data.Milliman.Hubs.Models;
using SignalRDashboard.Data.Milliman.Pollers;

namespace SignalRDashboard.Data.Milliman.Hubs
{
    [HubName("teamCityStatus")]
    public class TeamCityStatusHub : PollingHub<TeamCityStatus>
    {
        public TeamCityStatusHub() 
            : base(new TrackConnectedUsersStrategy(),
                  TeamCityStatusPoller.Instance)
        {
        }
    }
}
