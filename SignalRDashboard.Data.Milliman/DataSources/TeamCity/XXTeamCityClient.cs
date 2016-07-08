//using System;
//using System.Globalization;
//using SignalRDashboard.Data.Milliman.Core;

//namespace SignalRDashboard.Data.Milliman.DataSources.TeamCity
//{
//    using System.Collections.Generic;
//    using System.Linq;

//    public class XXTeamCityClient
//    {
//        public IXmlAccessor Accessor { get; set; }

//        public IEnumerable<Summary> GetAllProjectSummaries()
//        {
//            var xml = Accessor.GetXml("projects");

//            return xml.Elements("project").Select(
//                p => new Summary
//                {
//                    Id = p.Attribute("id").Value,
//                    Name = p.Attribute("name").Value
//                });
//        }

//        public TeamCityBuildProject GetProject(string projectId)
//        {
//            var xml = Accessor.GetXml("projects/id:" + projectId);

//            return new TeamCityBuildProject
//            {
//                Id = xml.Attribute("id").Value,
//                Name = xml.Attribute("name").Value,
//                WebUri = new Uri(xml.Attribute("webUrl").Value),
//                Archived = bool.Parse(xml.Attribute("archived").Value),
//                Description = xml.Attribute("description").Value,
//                BuildTypes = xml.Element("buildTypes").Elements("buildType").Select(b => new Summary { Id = b.Attribute("id").Value, Name = b.Attribute("name").Value }),
//                DocOrder = GetDocOrder(xml.Attribute("description").Value)
//            };
//        }

//        public TeamCityBuildType GetBuildType(string buildTypeId)
//        {
//            var xml = Accessor.GetXml("buildTypes/id:" + buildTypeId);

//            return new TeamCityBuildType
//            {
//                Id = xml.Attribute("id").Value,
//                Name = xml.Attribute("name").Value,
//                BuildProjectId = xml.Element("project").Attribute("id").Value,
//                Description = xml.Attribute("description") != null ? xml.Attribute("description").Value : "",
//                WebUri = new Uri(xml.Attribute("webUrl").Value),
//                Paused = bool.Parse(xml.Attribute("paused").Value),
//                RunParameters = xml.Element("runParameters") == null ? new Dictionary<string, string>()
//                   : xml.Element("runParameters").Elements("property").ToDictionary(p => p.Attribute("name").Value, p => p.Attribute("value").Value)
//            };
//        }

//        public IEnumerable<TeamCityBuildResult> GetCompletedBuilds()
//        {
//            var xml = Accessor.GetXml("builds?count=1000");

//            return
//                xml.Elements("build").Select(
//                    x =>
//                    new TeamCityBuildResult
//                    {
//                        BuildTypeId = x.Attribute("buildTypeId").Value,
//                        Id = x.Attribute("id").Value,
//                        Number = x.Attribute("number").Value,
//                        Status = ConvertBuildStatusString(x.Attribute("status").Value),
//                        WebUri = new Uri(x.Attribute("webUrl").Value)
//                    });
//        }

//        public TeamCityBuildProgress GetInProgressBuild(string buildTypeId)
//        {
//            //http://build.caternet.co.uk:8100/httpAuth/app/rest/buildTypes/id:bt8/builds/running:true
//            var xml = Accessor.GetXml("buildTypes/id:" + buildTypeId + "/builds/running:true");

//            if (xml == null) return null;

//            return new TeamCityBuildProgress
//            {
//                Id = xml.Attribute("id").Value,
//                Number = xml.Attribute("number").Value,
//                Status = ConvertBuildStatusString(xml.Attribute("status").Value),
//                WebUri = new Uri(xml.Attribute("webUrl").Value),
//                Personal = bool.Parse(xml.Attribute("personal").Value),
//                History = bool.Parse(xml.Attribute("history").Value),
//                Pinned = bool.Parse(xml.Attribute("pinned").Value),
//                StatusText = xml.Element("statusText").Value,
//                Started = ConvertXmlDateTime(xml.Element("startDate").Value),
//                BuildTypeId = xml.Element("buildType").Attribute("id").Value,
//                AgentId = xml.Element("agent").Attribute("id").Value,
//                PercentageComplete = xml.Element("running-info").Attribute("percentageComplete") != null ? decimal.Parse(xml.Element("running-info").Attribute("percentageComplete").Value) : 0,
//                ElapsedRunTime = TimeSpan.FromSeconds(Double.Parse(xml.Element("running-info").Attribute("elapsedSeconds").Value)),
//                EstimatedRunTime = xml.Element("running-info").Attribute("estimatedTotalSeconds") != null ? TimeSpan.FromSeconds(Double.Parse(xml.Element("running-info").Attribute("estimatedTotalSeconds").Value)) : TimeSpan.FromSeconds(0),
//                CurrentStage = xml.Element("running-info").Attribute("currentStageText").Value,
//                Outdated = bool.Parse(xml.Element("running-info").Attribute("outdated").Value),
//                ProbablyHanging = bool.Parse(xml.Element("running-info").Attribute("probablyHanging").Value)
//            };
//        }

//        public TeamCityFullBuildResult GetCancelledBuild(string buildId)
//        {
//            var xml = Accessor.GetXml("builds/id:" + buildId);

//            if (xml == null) return null;

//            var statusTextNode = xml.Descendants("statusText").FirstOrDefault();
//            if (statusTextNode == null) return null;

//            if (!statusTextNode.Value.ToLowerInvariant().StartsWith("cancelled")) return null;

//            return new TeamCityFullBuildResult
//            {
//                Id = xml.Attribute("id").Value,
//                Number = xml.Attribute("number").Value,
//                Status = TeamCityBuildStatus.Cancelled,
//                WebUri = new Uri(xml.Attribute("webUrl").Value),
//                Personal = bool.Parse(xml.Attribute("personal").Value),
//                History = bool.Parse(xml.Attribute("history").Value),
//                Pinned = bool.Parse(xml.Attribute("pinned").Value),
//                StatusText = xml.Element("statusText").Value,
//                Started = ConvertXmlDateTime(xml.Element("startDate").Value),
//                Finished = ConvertXmlDateTime(xml.Element("finishDate").Value),
//                BuildTypeId = xml.Element("buildType").Attribute("id").Value,
//                AgentId = xml.Element("agent").Attribute("id").Value
//            };
//        }

//        public TeamCityFullBuildResult GetCompletedBuild(string buildId)
//        {
//            var xml = Accessor.GetXml("builds/running:true,id:" + buildId);

//            if (xml == null) return null;

//            return new TeamCityFullBuildResult
//            {
//                Id = xml.Attribute("id").Value,
//                Number = xml.Attribute("number").Value,
//                Status = ConvertBuildStatusString(xml.Attribute("status").Value),
//                WebUri = new Uri(xml.Attribute("webUrl").Value),
//                Personal = bool.Parse(xml.Attribute("personal").Value),
//                History = bool.Parse(xml.Attribute("history").Value),
//                Pinned = bool.Parse(xml.Attribute("pinned").Value),
//                StatusText = xml.Element("statusText").Value,
//                Started = ConvertXmlDateTime(xml.Element("startDate").Value),
//                Finished = ConvertXmlDateTime(xml.Element("finishDate").Value),
//                BuildTypeId = xml.Element("buildType").Attribute("id").Value,
//                AgentId = xml.Element("agent").Attribute("id") != null ? xml.Element("agent").Attribute("id").Value : null
//            };
//        }

//        private static TeamCityBuildStatus ConvertBuildStatusString(string value)
//        {
//            switch (value)
//            {
//                case "SUCCESS":
//                    return TeamCityBuildStatus.Success;
//                case "ERROR":
//                    return TeamCityBuildStatus.Error;
//                case "FAILURE":
//                    return TeamCityBuildStatus.Failure;
//                default:
//                    return TeamCityBuildStatus.Unknown;
//            }
//        }

//        private const string XmlDateTimeFormatString = "yyyyMMddTHHmmsszzz";
//        private static readonly CultureInfo Info = new CultureInfo("en-GB", true);

//        private static DateTime ConvertXmlDateTime(string dateTimeString)
//        {
//            return DateTime.ParseExact(dateTimeString, XmlDateTimeFormatString, Info);
//        }

//        private static int GetDocOrder(string value)
//        {
//            int docOrder;

//            if (String.IsNullOrEmpty(value))
//                return 0;

//            return int.TryParse(value.Substring(0, 1), out docOrder) ? docOrder : 0;
//        }
//    }
//}
