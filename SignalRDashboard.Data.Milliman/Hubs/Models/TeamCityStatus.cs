using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SignalRDashboard.Data.Core.Hubs.Models;
using SignalRDashboard.Data.Milliman.DataSources.Models.TeamCity;
using SignalRDashboard.Data.Milliman.Hubs.Models.TeamCity;

namespace SignalRDashboard.Data.Milliman.Hubs.Models
{
    public class TeamCityStatus : DashboardHubModel, IEnumerable<TeamCityProject>
    {
        public override bool HasChanged { get; protected set; }

        private readonly List<TeamCityProject> _projects = new List<TeamCityProject>();

        public void UpdateOrAddProject(ProjectData p)
        {
            var project = _projects.FirstOrDefault(s => s.ProjectId == p.ProjectId);
            if (project == null)
            {
                project = new TeamCityProject
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    BuildConfigs = p.BuildConfigs.Select(bc => new TeamCityBuildConfig
                    {
                        ConfigId = bc.ConfigId,
                        ConfigName = bc.ConfigName,
                        BuildNumber = bc.BuildNumber,
                        BuildFailed = bc.BuildFailed,
                        PercentageComplete = bc.PercentageComplete
                    }).ToList()
                };
                _projects.Add(project);
                HasChanged = true;
            }
            else
            {
                project.ProjectId = p.ProjectId;
                project.ProjectName = p.ProjectName;
                project.BuildConfigs = p.BuildConfigs.Select(bc => new TeamCityBuildConfig
                {
                    ConfigId = bc.ConfigId,
                    ConfigName = bc.ConfigName,
                    BuildNumber = bc.BuildNumber,
                    BuildFailed = bc.BuildFailed,
                    PercentageComplete = bc.PercentageComplete
                }).ToList();
            }

            HasChanged = HasChanged || project.HasChanged;
        }

        public override void ResetChangedState()
        {
            HasChanged = false;
            foreach (var project in _projects)
            {
                project.ResetChangedState();
            }
        }

        public TeamCityProject[] GetProjects => _projects.ToArray();

        public IEnumerator<TeamCityProject> GetEnumerator()
        {
            return _projects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}