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

        public void UpdateOrAddProject(ProjectData webProject)
        {
            var dashProject = _projects.FirstOrDefault(s => s.ProjectId == webProject.ProjectId);
            if (dashProject == null)
            {
                dashProject = new TeamCityProject
                {
                    ProjectId = webProject.ProjectId,
                    ProjectName = webProject.ProjectName,
                    BuildConfigs = webProject.BuildConfigs.Select(bc => new TeamCityBuildConfig
                    {
                        ConfigId = bc.ConfigId,
                        ConfigName = bc.ConfigName,
                        BuildNumber = bc.BuildNumber,
                        BuildFailed = bc.BuildFailed,
                        BuildTime = bc.BuildTime
                    }).ToList()
                };
                _projects.Add(dashProject);
                HasChanged = true;
            }
            else
            {
                dashProject.ProjectId = webProject.ProjectId;
                dashProject.ProjectName = webProject.ProjectName;

                foreach (var webBuild in webProject.BuildConfigs)
                {
                    UpdateOrAddBuildConfig(dashProject, webBuild);
                }
            }
            
            HasChanged = HasChanged || dashProject.HasChanged;
        }

        private void UpdateOrAddBuildConfig(TeamCityProject dashProject, BuildData webBuild)
        {
            var dashBuild = dashProject.BuildConfigs.FirstOrDefault(s => s.ConfigId == webBuild.ConfigId);
            if (dashBuild == null)
            {
                dashProject.BuildConfigs.Add(new TeamCityBuildConfig
                {
                    ConfigId = webBuild.ConfigId,
                    ConfigName = webBuild.ConfigName,
                    BuildNumber = webBuild.BuildNumber,
                    BuildFailed = webBuild.BuildFailed,
                    BuildTime = webBuild.BuildTime
                });
                HasChanged = true;
            }
            else
            {
                dashBuild.ConfigId = webBuild.ConfigId;
                dashBuild.ConfigName = webBuild.ConfigName;
                dashBuild.BuildNumber = webBuild.BuildNumber;
                dashBuild.BuildFailed = webBuild.BuildFailed;
                dashBuild.BuildTime = webBuild.BuildTime;
                HasChanged = dashBuild.HasChanged;
            }
        }

        public override
            void ResetChangedState()
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