using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Hubs.Models
{
    public class TeamCityStatuses : DashboardHubModel, IEnumerable<TeamCityStatus>
    {
        public override bool HasChanged { get; protected set; }

        private readonly List<TeamCityStatus> _projects = new List<TeamCityStatus>();

        public void UpdateOrAddSite(string id, string name, bool include)
        {
            TeamCityStatus project = _projects.FirstOrDefault(s => s.ProjectId == id);
            if (project == null)
            {
                project = new TeamCityStatus
                {
                    ProjectId = id,
                    ProjectName = name,
                    Include = include
                };
                _projects.Add(project);
                HasChanged = true;
            }
            else
            {
                project.Include = include;
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

        public TeamCityStatus[] GetProjects => _projects.ToArray();

        public IEnumerator<TeamCityStatus> GetEnumerator()
        {
            return _projects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}