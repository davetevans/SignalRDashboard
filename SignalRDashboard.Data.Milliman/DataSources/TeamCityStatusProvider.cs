using System.Collections.Generic;
using System.Linq;
using SignalRDashboard.Data.Milliman.DataSources.Models;
using TeamCitySharp;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class TeamCityStatusProvider : ITeamCityStatusProvider
    {
        private readonly bool _isInitialised = false;
        private readonly TeamCityClient _client;
        private IList<TeamCityStatusData> _teamCityData = new List<TeamCityStatusData>();
        
        public TeamCityStatusProvider()
        {
            var baseUrl = "acbuild2.cloudapp.net";
            var username = "build.monitor";
            var password = "Pairing0_!";

            _client = new TeamCityClient(baseUrl, true);
            _client.Connect(username, password);
        }
        
        public IEnumerable<TeamCityStatusData> GetTeamCityStatus()
        {
            if (!_isInitialised)
            {
                foreach (var project in _client.BuildConfigs.All())
                {
                    _teamCityData.Add(new TeamCityStatusData
                    {
                        ProjectId = $"{project.ProjectId} - {project.Id}",
                        ProjectName = $"{project.ProjectName} - {project.Name}"
                    });
                }
            }

            return _teamCityData;
        }

    }
}