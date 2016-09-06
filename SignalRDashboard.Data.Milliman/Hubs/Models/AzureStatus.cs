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
                    Stats = webGroup.Stats.Select(s => new AzureResourceGroupStatStatus
                    {
                        ClusterName = s.ClusterName,
                        ClusterSize = s.ClusterSize,
                        ClusterState = s.ClusterState,
                        ClusterDate = s.ClusterDate
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

                foreach (var webStat in webGroup.Stats)
                {
                    UpdateOrAddGroupStat(dashGroup, webStat);
                }
            }

            HasChanged = HasChanged || dashGroup.HasChanged;
        }
        private void UpdateOrAddGroupStat(AzureGroupStatus dashGroup, AzureResourceGroupStatData webStat)
        {
            var dashStat = dashGroup.Stats.FirstOrDefault(s => s.ClusterEtag == webStat.ClusterEtag);
            if (dashStat == null)
            {
                dashGroup.Stats.Add(new AzureResourceGroupStatStatus
                {
                    ClusterEtag = webStat.ClusterEtag,
                    ClusterName = webStat.ClusterName,
                    ClusterSize = webStat.ClusterSize,
                    ClusterState = webStat.ClusterState,
                    ClusterDate = webStat.ClusterDate
                });
                HasChanged = true;
            }
            else
            {
                dashStat.ClusterEtag = webStat.ClusterEtag;
                dashStat.ClusterName = webStat.ClusterName;
                dashStat.ClusterSize = webStat.ClusterSize;
                dashStat.ClusterState = webStat.ClusterState;
                dashStat.ClusterDate = webStat.ClusterDate;
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