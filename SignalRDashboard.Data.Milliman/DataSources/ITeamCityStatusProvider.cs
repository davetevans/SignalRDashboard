using System.Collections.Generic;
using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public interface ITeamCityStatusProvider
    {
        IEnumerable<TeamCityStatusData> GetTeamCityStatus();
    }
}
