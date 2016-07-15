using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SignalRDashboard.Data.Milliman.DataSources.Models.TeamCity;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class TeamCityStatusProvider
    {
        private readonly bool _isInitialised = false;
        private readonly TeamCityClient _client;
        private readonly IList<ProjectData> _teamCityData = new List<ProjectData>();
        private readonly List<string> _includedProjects = new List<string>();

        public TeamCityStatusProvider()
        {
            var config = ConfigurationManager.AppSettings;
            var baseUrl = config["TeamCityUrl"];
            var username = config["TeamCityUsername"];
            var password = config["TeamCityPassword"];
            foreach (var p in config["TeamCityIncludedProjects"].Split(','))
            {
                _includedProjects.Add(p);
            }

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
                        ProjectId = project.Id,
                        ProjectName = project.Name,
                        BuildConfigs = new List<BuildData>()
                    };

                    foreach (var config in _client.BuildConfigs.ByProjectId(project.Id))
                    {
                        var latestBuild = _client.Builds.LastBuildByBuildConfigId(config.Id);

                        item.BuildConfigs.Add(new BuildData
                        {
                            ConfigId = config.Id,
                            ConfigName = config.Name,
                            BuildNumber = latestBuild.Number,
                            BuildFailed = latestBuild.Status == BuildStatus.Failure,
                            PercentageComplete = GetBuildProgress(latestBuild)
                        });
                    }

                    _teamCityData.Add(item);
                }
            }

            return _teamCityData;
        }

        private decimal GetBuildProgress(Build latestBuild)
        {
            var startTime = latestBuild.StartDate;

            return 10 * new Random().Next(0, 10);
        }

    }
}