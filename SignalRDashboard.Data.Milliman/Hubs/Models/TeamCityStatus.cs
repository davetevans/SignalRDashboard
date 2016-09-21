using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SignalRDashboard.Data.Core.Hubs.Models;
using SignalRDashboard.Data.Milliman.DataSources.Models;
using SignalRDashboard.Data.Milliman.Hubs.Models.TeamCity;

namespace SignalRDashboard.Data.Milliman.Hubs.Models
{
    public class TeamCityStatus : DashboardHubModel, IEnumerable<TeamCityProjectStatus>
    {
        public override bool HasChanged { get; protected set; }

        private readonly List<TeamCityProjectStatus> _projects = new List<TeamCityProjectStatus>();

        public void UpdateOrAddProject(TeamCityProjectData webProject)
        {
            var dashProject = _projects.FirstOrDefault(s => s.ProjectId == webProject.ProjectId);
            if (dashProject == null)
            {
                dashProject = new TeamCityProjectStatus
                {
                    ProjectId = webProject.ProjectId,
                    ProjectName = webProject.ProjectName,
                    BuildConfigs = webProject.BuildConfigs.Select(bc => new TeamCityBuildConfigStatus
                    {
                        ConfigId = bc.ConfigId,
                        ConfigName = bc.ConfigName,
                        BuildNumber = bc.BuildNumber,
                        BuildFailed = bc.BuildFailed,
                        PercentageComplete = bc.PercentageComplete,
                        BuildFailedMessageReceivedCount = bc.BuildFailed ? 1 : 0,
                        BuildNewlyFailed = bc.BuildFailed
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

        private void UpdateOrAddBuildConfig(TeamCityProjectStatus dashProject, TeamCityBuildConfigData webBuild)
        {
            var dashBuild = dashProject.BuildConfigs.FirstOrDefault(s => s.ConfigId == webBuild.ConfigId);
            if (dashBuild == null)
            {
                dashProject.BuildConfigs.Add(new TeamCityBuildConfigStatus
                {
                    ConfigId = webBuild.ConfigId,
                    ConfigName = webBuild.ConfigName,
                    BuildNumber = webBuild.BuildNumber,
                    BuildFailed = webBuild.BuildFailed,
                    BuildRunning = webBuild.BuildRunning,
                    PercentageComplete = webBuild.PercentageComplete,
                    BuildFailedMessageReceivedCount = webBuild.BuildFailed ? 1: 0,
                    BuildNewlyFailed = webBuild.BuildFailed
                });
                HasChanged = true;
            }
            else
            {
                dashBuild.ConfigId = webBuild.ConfigId;
                dashBuild.ConfigName = webBuild.ConfigName;
                dashBuild.BuildNumber = webBuild.BuildNumber;
                dashBuild.BuildFailed = webBuild.BuildFailed;
                dashBuild.BuildRunning = webBuild.BuildRunning;
                dashBuild.PercentageComplete = webBuild.PercentageComplete;
                dashBuild.BuildFailedMessageReceivedCount = SetBuildFailedMessageReceivedCount(dashBuild.BuildFailedMessageReceivedCount, webBuild.BuildFailed);
                dashBuild.BuildNewlyFailed = dashBuild.BuildFailedMessageReceivedCount == 1;
                HasChanged = dashBuild.HasChanged;
            }
        }

        private static int SetBuildFailedMessageReceivedCount(int currentCount, bool buildFailed)
        {
            return currentCount == 0
                ? (buildFailed ? 1 : 0)
                : (buildFailed ? currentCount + 1 : 0);
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

        public TeamCityProjectStatus[] GetProjects => _projects.ToArray();

        public IEnumerator<TeamCityProjectStatus> GetEnumerator()
        {
            return _projects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}