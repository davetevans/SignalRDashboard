using System.Collections.Generic;
using System.Linq;
using SignalRDashboard.Data.Milliman.DataSources.Models.TeamCity;
using TeamCitySharp;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class TeamCityStatusProvider : ITeamCityStatusProvider
    {
        private readonly bool _isInitialised = false;
        private readonly TeamCityClient _client;
        private readonly IList<ProjectData> _teamCityData = new List<ProjectData>();
        private readonly List<string> _includedProjects = new List<string>();

        public TeamCityStatusProvider()
        {
            _includedProjects.Add("Releases");
            _includedProjects.Add("Documentation");
            _includedProjects.Add("HTML");
            _includedProjects.Add("Server");

            var baseUrl = "acbuild2.cloudapp.net";
            var username = "build.monitor";
            var password = "Pairing0_!";

            _client = new TeamCityClient(baseUrl, true);
            _client.Connect(username, password);
        }
        
        public IEnumerable<ProjectData> GetTeamCityStatus()
        {
            if (!_isInitialised)
            {
                foreach (var project in _client.Projects.All().Where(p => _includedProjects.Contains(p.Name)))
                {
                    var item = new ProjectData
                    {
                        ProjectId = $"{project.Id}",
                        ProjectName = $"{project.Name}",
                        BuildConfigs = new List<BuildConfigData>()
                    };

                    foreach (var config in _client.BuildConfigs.ByProjectId(project.Id))
                    {
                        foreach (var build in _client.Builds.ByBuildConfigId(config.Id))
                        {
                            item.BuildConfigs.Add(new BuildConfigData
                            {
                                ConfigId = config.Id,
                                ConfigName = $"{config.Name} - {build.Number} - {build.Status}"
                            });
                        }
                        

                        _teamCityData.Add(item);
                    }
                }
            }

            return _teamCityData;
        }

    }
}