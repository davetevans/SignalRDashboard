using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
                    ClusterStats = webGroup.ClusterStats
                        .Select(x => new { x.Size, TimeAlive = CalcTimeAlive(x.Date), x.State })
                        .GroupBy(g => g.Size)
                        .Select(s => new AzureStatStatus
                        {
                            GroupName = s.Key,
                            Count = RenderStateCounts(s.Count(x => ClusterState.Provisioning.Contains(x.State)), 
                                                      s.Count(x => ClusterState.Running.Contains(x.State)), 
                                                      s.Count(x => ClusterState.Deleting.Contains(x.State)), 
                                                      s.Count(x => ClusterState.Errored.Contains(x.State))),
                            AliveTime = new TimeSpan(s.Sum(a => a.TimeAlive.Ticks)).ToString("d'd 'h'h 'm'm'")
                        }).ToList(),
                    SqlStats = webGroup.SqlStats
                        .Select(x => new { x.Size, TimeAlive = CalcTimeAlive(x.Date), x.State })
                        .GroupBy(g => g.Size)
                        .Select(s => new AzureStatStatus
                        {
                            GroupName = s.Key,
                            Count = s.Count().ToString(),
                            AliveTime = new TimeSpan(s.Sum(a => a.TimeAlive.Ticks)).ToString("d'd 'h'h 'm'm'")
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

                foreach (var webStat in webGroup.ClusterStats
                        .Select(x => new { x.Size, TimeAlive = CalcTimeAlive(x.Date), x.State })
                        .GroupBy(g => g.Size)
                        .Select(s => new AzureStatStatus
                        {
                            GroupName = s.Key,
                            Count = RenderStateCounts(s.Count(x => ClusterState.Provisioning.Contains(x.State)),
                                                      s.Count(x => ClusterState.Running.Contains(x.State)),
                                                      s.Count(x => ClusterState.Deleting.Contains(x.State)),
                                                      s.Count(x => ClusterState.Errored.Contains(x.State))),
                            AliveTime = new TimeSpan(s.Sum(a => a.TimeAlive.Ticks)).ToString("d'd 'h'h 'm'm'")
                        }))
                {
                    UpdateOrAddClusterStat(dashGroup, webStat);
                }

                var clusterStatsToRemove = dashGroup.ClusterStats.Where(s => webGroup.ClusterStats.All(w => w.Name != s.GroupName)).ToList();
                foreach (var dashStat in clusterStatsToRemove)
                {
                    dashGroup.ClusterStats.Remove(dashStat);
                    HasChanged = true;
                }

                foreach (var webStat in webGroup.SqlStats
                        .Select(x => new { x.Size, TimeAlive = CalcTimeAlive(x.Date), x.State })
                        .GroupBy(g => g.Size)
                        .Select(s => new AzureStatStatus
                        {
                            GroupName = s.Key,
                            Count = s.Count().ToString(),
                            AliveTime = new TimeSpan(s.Sum(a => a.TimeAlive.Ticks)).ToString("d'd 'h'h 'm'm'")
                        }))
                {
                    UpdateOrAddSqlStat(dashGroup, webStat);
                }
                
                var sqlStatsToRemove = dashGroup.SqlStats.Where(s => webGroup.SqlStats.All(w => w.Name != s.GroupName)).ToList();
                foreach (var dashStat in sqlStatsToRemove)
                {
                    dashGroup.SqlStats.Remove(dashStat);
                    HasChanged = true;
                }
            }

            if (!dashGroup.ClusterStats.Any() && !dashGroup.SqlStats.Any())
            {
                _groups.Remove(dashGroup);
                HasChanged = true;
            }

            HasChanged = HasChanged || dashGroup.HasChanged;
        }

        private void UpdateOrAddClusterStat(AzureGroupStatus dashGroup, AzureStatStatus webStat)
        {
            var dashStat = dashGroup.ClusterStats.FirstOrDefault(s => s.GroupName == webStat.GroupName);

            if (dashStat == null)
            {
                dashGroup.ClusterStats.Add(webStat);
                HasChanged = true;
            }
            else
            {
                dashStat.Count = webStat.Count;
                dashStat.AliveTime = webStat.AliveTime;
                HasChanged = dashStat.HasChanged;
            }
        }

        private void UpdateOrAddSqlStat(AzureGroupStatus dashGroup, AzureStatStatus webStat)
        {
            var dashStat = dashGroup.SqlStats.FirstOrDefault(s => s.GroupName == webStat.GroupName);

            if (dashStat == null)
            {
                dashGroup.SqlStats.Add(webStat);
                HasChanged = true;
            }
            else
            {
                dashStat.Count = webStat.Count;
                dashStat.AliveTime = webStat.AliveTime;
                HasChanged = dashStat.HasChanged;
            }
        }

        private static TimeSpan CalcTimeAlive(string createdDate)
        {
            return DateTime.Now - DateTime.Parse(createdDate, CultureInfo.InvariantCulture);
        }

        private static string RenderStateCounts(int provisioningCount, int runningCount, int deletingCount, int erroredCount)
        {
            return $"{provisioningCount+runningCount+deletingCount+erroredCount}&nbsp;&nbsp;&nbsp;<span class=\"clusterProvisioning\">{provisioningCount}</span>&nbsp;<span class=\"clusterRunning\">{runningCount}</span>&nbsp;<span class=\"clusterDeleting\">{deletingCount}</span>&nbsp;<span class=\"clusterErrored\">{erroredCount}</span>";
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