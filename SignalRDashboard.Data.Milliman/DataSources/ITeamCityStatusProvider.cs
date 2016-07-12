using System.Collections.Generic;
using SignalRDashboard.Data.Milliman.DataSources.Models.TeamCity;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public interface ITeamCityStatusProvider
    {
        IEnumerable<ProjectData> GetTeamCityStatus();
    }
}
