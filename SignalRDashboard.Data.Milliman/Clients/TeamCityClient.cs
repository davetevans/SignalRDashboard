using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SignalRDashboard.Data.Milliman.Clients.TeamCity;
using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.Clients
{
    public class TeamCityClient
    {
        public string TeamCityUrl { get; set; }
        public string TeamCityUsername { get; set; }
        public string TeamCityPassword { get; set; }
        private HttpXmlAccessor Accessor { get; set; }

        public void Init()
        {
            Accessor = new HttpXmlAccessor("https://" + TeamCityUrl, "/httpAuth/app/rest/", TeamCityUsername, TeamCityPassword);
        }

        public IEnumerable<TeamCityProjectData> GetIncludedProjects(List<string> includedProjects, List<string> excludedBuildConfigs)
        {
            if (!includedProjects.Any()) return null;

            var projectSummaries = GetAllProjectSummaries().Where(p => includedProjects.Contains(p.Id)).ToArray();
            var projects = projectSummaries.Select(p => GetProject(p.Id)).OrderBy(o => o.Description).ToArray();
            var types = projects.SelectMany(p => p.BuildTypes).Select(t => GetBuildType(t.Id)).OrderByDescending(o => o.Id).Where(b => !excludedBuildConfigs.Contains(b.Id)).ToArray();
            var latestBuilds = types.Select(t => GetLatestBuild(t.Id)).ToArray();

            return projects.Select(
                p => new TeamCityProjectData
                {
                    ProjectId = p.Id,
                    ProjectName = $"{(p.Name == "Cloud Service" ? "Compute" : "IDM")} {p.Name}",
                    BuildConfigs = types.Where(t => t.BuildProjectId == p.Id)
                                        .Select(t => new TeamCityBuildConfigData
                                        {
                                            ConfigId = t.Id,
                                            ConfigName = t.Name,
                                            BuildNumber = GetBuildNumber(latestBuilds, t.Id),
                                            BuildRunning = GetBuildRunning(latestBuilds, t.Id),
                                            PercentageComplete = GetPercentageComplete(latestBuilds, t.Id),
                                            BuildFailed = GetBuildFailed(latestBuilds, t.Id)
                                        })
                                        .OrderBy(t => t.ConfigName).ToList()
                }).ToList();
        }

        public bool TestConnection()
        {
            return Accessor.GetXml("users/username:build.monitor") != null;
        }

        private IEnumerable<Summary> GetAllProjectSummaries()
        {
            var xml = Accessor.GetXml("projects");

            return xml.Elements("project").Select(
                p => new Summary
                {
                    Id = p.Attribute("id").Value,
                    Name = p.Attribute("name").Value
                });
        }

        private BuildProject GetProject(string projectId)
        {
            var xml = Accessor.GetXml("projects/id:" + projectId);

            return new BuildProject
            {
                Id = xml.Attribute("id").Value,
                Name = xml.Attribute("name").Value,
                WebUri = new Uri(xml.Attribute("webUrl").Value),
                Archived = bool.Parse(xml.Attribute("archived") != null ? xml.Attribute("archived").Value : "false"),
                Description = xml.Attribute("description") != null ? xml.Attribute("description").Value : "",
                BuildTypes = xml.Element("buildTypes").Elements("buildType").Select(b => new Summary { Id = b.Attribute("id").Value, Name = b.Attribute("name").Value }),
                DocOrder = GetDocOrder(xml.Attribute("description") != null ? xml.Attribute("description").Value : "")
            };
        }

        private BuildType GetBuildType(string buildTypeId)
        {
            var xml = Accessor.GetXml("buildTypes/id:" + buildTypeId);

            var b = new BuildType
            {
                Id = xml.Attribute("id").Value,
                Name = xml.Attribute("name").Value,
                BuildProjectId = xml.Attribute("projectId").Value,
                Description = xml.Attribute("description") != null ? xml.Attribute("description").Value : "",
                WebUri = new Uri(xml.Attribute("webUrl").Value),
                Paused = xml.Attribute("paused") != null && bool.Parse(xml.Attribute("paused").Value)
            };

            return b;
        }

        private BuildResult GetLatestBuild(string buildTypeId)
        {
            var runningBuildXml = Accessor.GetXml($"buildTypes/id:{buildTypeId}/builds/running:true");
            var completedBuildXml = Accessor.GetXml($"buildTypes/id:{buildTypeId}/builds/running:false");
            var xml = runningBuildXml ?? completedBuildXml;

            if (xml == null) return new BuildResult();

            return new BuildResult
            {
                BuildTypeId = xml.Attribute("buildTypeId").Value,
                Id = xml.Attribute("id").Value,
                Number = xml.Attribute("number").Value,
                Status = ConvertBuildStatusString(xml.Attribute("status").Value),
                WebUri = new Uri(xml.Attribute("webUrl").Value),
                IsRunning = runningBuildXml != null,
                PercentageComplete = runningBuildXml != null ? (xml.Element("running-info").Attribute("percentageComplete") != null ? decimal.Parse(xml.Element("running-info").Attribute("percentageComplete").Value) : 0) : 100
            };
        }

        public BuildProgress GetInProgressBuild(string buildTypeId)
        {
            var xml = Accessor.GetXml("buildTypes/id:" + buildTypeId + "/builds/running:true");

            if (xml == null) return null;

            return new BuildProgress
            {
                Id = xml.Attribute("id").Value,
                Number = xml.Attribute("number").Value,
                Status = ConvertBuildStatusString(xml.Attribute("status").Value),
                WebUri = new Uri(xml.Attribute("webUrl").Value),
                Personal = bool.Parse(xml.Attribute("personal").Value),
                History = bool.Parse(xml.Attribute("history").Value),
                Pinned = bool.Parse(xml.Attribute("pinned").Value),
                StatusText = xml.Element("statusText").Value,
                Started = ConvertXmlDateTime(xml.Element("startDate").Value),
                BuildTypeId = xml.Element("buildType").Attribute("id").Value,
                AgentId = xml.Element("agent").Attribute("id").Value,
                PercentageComplete = xml.Element("running-info").Attribute("percentageComplete") != null ? decimal.Parse(xml.Element("running-info").Attribute("percentageComplete").Value) : 0,
                ElapsedRunTime = TimeSpan.FromSeconds(Double.Parse(xml.Element("running-info").Attribute("elapsedSeconds").Value)),
                EstimatedRunTime = xml.Element("running-info").Attribute("estimatedTotalSeconds") != null ? TimeSpan.FromSeconds(Double.Parse(xml.Element("running-info").Attribute("estimatedTotalSeconds").Value)) : TimeSpan.FromSeconds(0),
                CurrentStage = xml.Element("running-info").Attribute("currentStageText").Value,
                Outdated = bool.Parse(xml.Element("running-info").Attribute("outdated").Value),
                ProbablyHanging = bool.Parse(xml.Element("running-info").Attribute("probablyHanging").Value)
            };
        }

        public FullBuildResult GetCancelledBuild(string buildId)
        {
            var xml = Accessor.GetXml("builds/id:" + buildId);

            if (xml == null) return null;

            var statusTextNode = xml.Descendants("statusText").FirstOrDefault();
            if (statusTextNode == null) return null;

            if (!statusTextNode.Value.ToLowerInvariant().StartsWith("cancelled")) return null;

            return new FullBuildResult
            {
                Id = xml.Attribute("id").Value,
                Number = xml.Attribute("number").Value,
                Status = BuildStatus.Cancelled,
                WebUri = new Uri(xml.Attribute("webUrl").Value),
                Personal = bool.Parse(xml.Attribute("personal").Value),
                History = bool.Parse(xml.Attribute("history").Value),
                Pinned = bool.Parse(xml.Attribute("pinned").Value),
                StatusText = xml.Element("statusText").Value,
                Started = ConvertXmlDateTime(xml.Element("startDate").Value),
                Finished = ConvertXmlDateTime(xml.Element("finishDate").Value),
                BuildTypeId = xml.Element("buildType").Attribute("id").Value,
                AgentId = xml.Element("agent").Attribute("id").Value
            };
        }
        public IEnumerable<BuildResult> GetCompletedBuilds()
        {
            var xml = Accessor.GetXml("builds?count=1000");

            return
                xml.Elements("build").Select(
                    x =>
                    new BuildResult
                    {
                        BuildTypeId = x.Attribute("buildTypeId").Value,
                        Id = x.Attribute("id").Value,
                        Number = x.Attribute("number").Value,
                        Status = ConvertBuildStatusString(x.Attribute("status").Value),
                        WebUri = new Uri(x.Attribute("webUrl").Value)
                    });
        }

        //return new FullBuildResult
        //{
        //    Id = xml.Attribute("id").Value,
        //    Number = xml.Attribute("number").Value,
        //    Status = ConvertBuildStatusString(xml.Attribute("status").Value),
        //    WebUri = new Uri(xml.Attribute("webUrl").Value),
        //    Personal = bool.Parse(xml.Attribute("personal").Value),
        //    History = bool.Parse(xml.Attribute("history").Value),
        //    Pinned = bool.Parse(xml.Attribute("pinned").Value),
        //    StatusText = xml.Element("statusText").Value,
        //    Started = ConvertXmlDateTime(xml.Element("startDate").Value),
        //    Finished = ConvertXmlDateTime(xml.Element("finishDate").Value),
        //    BuildTypeId = xml.Element("buildType").Attribute("id").Value,
        //    AgentId = xml.Element("agent").Attribute("id") != null ? xml.Element("agent").Attribute("id").Value : null
        //};

        public FullBuildResult GetCompletedBuild(string buildId)
        {
            var xml = Accessor.GetXml("builds/running:true,id:" + buildId);

            if (xml == null) return null;

            return new FullBuildResult
            {
                Id = xml.Attribute("id").Value,
                Number = xml.Attribute("number").Value,
                Status = ConvertBuildStatusString(xml.Attribute("status").Value),
                WebUri = new Uri(xml.Attribute("webUrl").Value),
                Personal = bool.Parse(xml.Attribute("personal").Value),
                History = bool.Parse(xml.Attribute("history").Value),
                Pinned = bool.Parse(xml.Attribute("pinned").Value),
                StatusText = xml.Element("statusText").Value,
                Started = ConvertXmlDateTime(xml.Element("startDate").Value),
                Finished = ConvertXmlDateTime(xml.Element("finishDate").Value),
                BuildTypeId = xml.Element("buildType").Attribute("id").Value,
                AgentId = xml.Element("agent").Attribute("id") != null ? xml.Element("agent").Attribute("id").Value : null
            };
        }

        private static BuildStatus ConvertBuildStatusString(string value)
        {
            switch (value)
            {
                case "SUCCESS":
                    return BuildStatus.Success;
                case "ERROR":
                    return BuildStatus.Error;
                case "FAILURE":
                    return BuildStatus.Failure;
                default:
                    return BuildStatus.Unknown;
            }
        }

        private const string XmlDateTimeFormatString = "yyyyMMddTHHmmsszzz";
        private static readonly CultureInfo Info = new CultureInfo("en-GB", true);

        private static DateTime ConvertXmlDateTime(string dateTimeString)
        {
            return DateTime.ParseExact(dateTimeString, XmlDateTimeFormatString, Info);
        }

        private static int GetDocOrder(string value)
        {
            int docOrder;

            if (string.IsNullOrEmpty(value))
                return 0;

            return int.TryParse(value.Substring(0, 1), out docOrder) ? docOrder : 0;
        }

        private string GetBuildNumber(IEnumerable<BuildResult> latestBuilds, string buildTypeId)
        {
            var latestBuild = latestBuilds.SingleOrDefault(b => b.BuildTypeId == buildTypeId);
            return latestBuild != null ? latestBuild.Number : "0";
        }

        private bool GetBuildRunning(IEnumerable<BuildResult> latestBuilds, string buildTypeId)
        {
            var latestBuild = latestBuilds.SingleOrDefault(b => b.BuildTypeId == buildTypeId);
            return latestBuild != null && latestBuild.IsRunning;
        }

        private bool GetBuildFailed(IEnumerable<BuildResult> latestBuilds, string buildTypeId)
        {
            var latestBuild = latestBuilds.SingleOrDefault(b => b.BuildTypeId == buildTypeId);
            return latestBuild != null && latestBuild.Status != BuildStatus.Success;
        }

        private decimal GetPercentageComplete(IEnumerable<BuildResult> latestBuilds, string buildTypeId)
        {
            var latestBuild = latestBuilds.SingleOrDefault(b => b.BuildTypeId == buildTypeId);
            return latestBuild?.PercentageComplete ?? 100;
        }

    }
}