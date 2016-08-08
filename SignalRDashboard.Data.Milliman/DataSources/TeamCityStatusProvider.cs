using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SignalRDashboard.Data.Milliman.DataSources.Models.TeamCity;
using FluentTc;
using FluentTc.Domain;
using FluentTc.Locators;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class TeamCityStatusProvider
    {
        private readonly bool _isInitialised = false;
        private readonly IList<ProjectData> _teamCityData = new List<ProjectData>();
        private readonly List<string> _includedProjects = new List<string>();
        private readonly IConnectedTc _client;

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
            
            _client = new RemoteTc().Connect(a => a.ToHost(baseUrl).UseSsl().AsUser(username, password));
        }
        
        public IEnumerable<ProjectData> GetTeamCityStatus()
        {
            if (!_isInitialised)
            {
                _teamCityData.Clear();

                try
                {
                    foreach (var project in _client.GetAllProjects().Where(p => _includedProjects.Contains(p.Name)))
                    {
                        var item = new ProjectData
                        {
                            ProjectId = project.Id,
                            ProjectName = project.Name,
                            BuildConfigs = new List<BuildData>()
                        };
                        
                        foreach (var config in _client.GetBuildConfigurations(_ => _.Project(__ => __.Id(project.Id))))
                        {
                            var latestBuild = _client.GetLastBuild(_ => _.BuildConfiguration(__ => __.Id(config.Id)));

                            item.BuildConfigs.Add(new BuildData
                            {
                                ConfigId = config.Id,
                                ConfigName = config.Name,
                                BuildNumber = latestBuild.Number,
                                BuildFailed = latestBuild.Status != BuildStatus.Success,
                                BuildTime = GetBuildProgress(latestBuild)
                            });
                        }

                        _teamCityData.Add(item);
                    }

                }
                catch (Exception ex)
                {
                    // do nothing as loss of connection to teamCity is handled elsewhere
                    var sdf = ex;
                }
            }

            return _teamCityData;
        }

        private static string GetBuildProgress(IBuild latestBuild)
        {
            var progress = string.Empty;

            // if build is running get the time taken
            if (latestBuild.FinishDate >= DateTime.Now)
            {
                var buildRunTime = DateTime.Now - latestBuild.StartDate;
                progress = $"{buildRunTime.Minutes}:{buildRunTime.Seconds}";
            }

            return progress;
        }

    }
}