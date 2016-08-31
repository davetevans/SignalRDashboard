using System;
using System.Collections.Generic;
using System.Configuration;
using SignalRDashboard.Data.Milliman.DataSources.Models;
using SignalRDashboard.Data.Milliman.Clients;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class TeamCityStatusProvider
    {
        private readonly bool _isInitialised = false;
        private readonly IList<TeamCityProjectData> _teamCityData = new List<TeamCityProjectData>();
        private readonly List<string> _includedProjects = new List<string>();
        private readonly TeamCityClient _client;

        public TeamCityStatusProvider()
        {
            var config = ConfigurationManager.AppSettings;

            _client = new TeamCityClient
            {
                TeamCityUrl = config["TeamCityUrl"],
                TeamCityUsername = config["TeamCityUsername"],
                TeamCityPassword = config["TeamCityPassword"]
            };

            _client.Init();

            foreach (var p in config["TeamCityIncludedProjects"].Split(','))
            {
                _includedProjects.Add(p);
            }
        }
        
        public IEnumerable<TeamCityProjectData> GetTeamCityStatus()
        {
            if (!_isInitialised)
            {
                _teamCityData.Clear();

                try
                {
                    foreach (var project in _client.GetIncludedProjects(_includedProjects))
                    {
                        _teamCityData.Add(project);
                    }

                }
                catch (Exception ex)
                {
                    var exx = ex;
                    // do nothing as loss of connection to teamCity is handled elsewhere
                }
            }

            return _teamCityData;
        }
    }
}