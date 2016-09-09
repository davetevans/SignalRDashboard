using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SignalRDashboard.Data.Core.Hubs.Models;
using SignalRDashboard.Data.Milliman.DataSources.Models;
using SignalRDashboard.Data.Milliman.Hubs.Models.Azure;

namespace SignalRDashboard.Data.Milliman.Hubs.Models
{
    public class AzureStatus : DashboardHubModel, IEnumerable<AzureGroupStatus>
    {
        public override bool HasChanged { get; protected set; }

        private readonly List<AzureGroupStatus> _groups = new List<AzureGroupStatus>();

        public void UpdateOrAddGroup(AzureResourceGroupData webGroup)
        {
            var dashGroup = _groups.FirstOrDefault(s => s.GroupId == webGroup.Id);
            if (dashGroup == null)
            {
                dashGroup = new AzureGroupStatus
                {
                    GroupId = webGroup.Id,
                    GroupName = webGroup.Name,
                    Location = webGroup.Location,
                    ClusterStats = webGroup.Stats.GroupBy(g => g.ClusterState).Select(s => new AzureClusterStatStatus
                    {
                        ClusterState = s.Key,
                        ClusterCount = s.Count()
                    }).ToList()
                };
                _groups.Add(dashGroup);
                HasChanged = true;
            }
            else
            {
                dashGroup.GroupId = webGroup.Id;
                dashGroup.GroupName = webGroup.Name;
                dashGroup.Location = webGroup.Location;

                foreach (var webStat in webGroup.Stats.GroupBy(g => g.ClusterState).Select(s => new AzureClusterStatStatus
                {
                    ClusterState = s.Key,
                    ClusterCount = s.Count()
                }))
                {
                    UpdateOrAddGroupStat(dashGroup, webStat);
                }
            }

            HasChanged = HasChanged || dashGroup.HasChanged;
        }
        private void UpdateOrAddGroupStat(AzureGroupStatus dashGroup, AzureClusterStatStatus webStat)
        {
            var dashStat = dashGroup.ClusterStats.FirstOrDefault(s => s.ClusterState == webStat.ClusterState);

            if (dashStat == null)
            {
                dashGroup.ClusterStats.Add(webStat);
                HasChanged = true;
            }
            else
            {
                dashStat.ClusterCount = webStat.ClusterCount;
                HasChanged = dashStat.HasChanged;
            }
        }

        public override void ResetChangedState()
        {
            HasChanged = false;
            foreach (var project in _groups)
            {
                project.ResetChangedState();
            }
        }

        public AzureGroupStatus[] GetGroups => _groups.ToArray();

        public IEnumerator<AzureGroupStatus> GetEnumerator()
        {
            return _groups.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}