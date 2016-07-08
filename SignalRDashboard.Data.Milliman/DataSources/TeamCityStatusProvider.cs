using System.Collections.Generic;
using System.Linq;
using SignalRDashboard.Data.Milliman.Core;
using SignalRDashboard.Data.Milliman.DataSources.Models;
using SignalRDashboard.Data.Milliman.DataSources.TeamCity;
using TeamCitySharp;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class TeamCityStatusProvider : ITeamCityStatusProvider
    {
        private readonly bool _isInitialised = false;
        ////private readonly XXTeamCityClient _teamCityClient = new XXTeamCityClient();
        private IEnumerable<TeamCityStatusData> _teamCityData = new List<TeamCityStatusData>();

        //private string[] BuildProjectIds { get; set; }
        //private readonly List<string> _completedBuildIds = new List<string>();
        //private readonly List<string> _inprogressBuildIds = new List<string>();
        //private TeamCityBuildProgress _inProgressBuild;

        //private List<TeamCityStatusData> _buildProjects;

        private readonly TeamCityClient _teamCityClient;

        public TeamCityStatusProvider()
        {
            var baseUrl = "https://acbuild2.cloudapp.net/";
            var urlPrefix = @"httpAuth/app/rest/";
            var username = "build.monitor";
            var password = "Pairing0_!";

            // _teamCityClient = new TeamCitySharp.TeamCityClient(baseUrl, true);
            //_teamCityClient.Connect(username, password);
            //_teamCityClient.Authenticate();



            //_teamCityClient.Accessor = new HttpXmlAccessor(baseUrl, urlPrefix, username, password);
            //LoadBuildProjectCache();
            //Poll();
        }

        //private void LoadBuildProjectCache()
        //{
        //    var projectSummaries = _teamCityClient.GetAllProjectSummaries();
        //    var projects = projectSummaries.Select(p => _teamCityClient.GetProject(p.Id)).OrderBy(o => o.Description).ToArray();

        //    //IEnumerable<TeamCityBuildType> types = BuildProjectIds.Any()
        //    //    ? projects.Where(s => BuildProjectIds.Contains(s.Id))
        //    //        .SelectMany(p => p.BuildTypes)
        //    //        .Select(t => _teamCityClient.GetBuildType(t.Id))
        //    //        .OrderByDescending(o => o.Name)
        //    //        .ToArray()
        //    //    : projects.SelectMany(p => p.BuildTypes).Select(t => _teamCityClient.GetBuildType(t.Id)).ToArray();
        //    //_buildProjects = MergeProjectsAndTypes(projects, types).ToList();

        //    _buildProjects = MergeProjectsAndTypes(projects, null).ToList();
        //}

        //private static IEnumerable<TeamCityStatusData> MergeProjectsAndTypes(IEnumerable<TeamCityBuildProject> projects, IEnumerable<TeamCityBuildType> types)
        //{
        //    // filter projects to only those with a type...
        //    //var requiredProjects = projects.Where(p => types.Count(t => t.BuildProjectId == p.Id) > 0);

        //    var r = projects.Select(project => new TeamCityStatusData
        //    {
        //        ProjectId = project.Id,
        //        ProjectName = project.Name
        //    });

        //    //var r = requiredProjects.Select(project => new TeamCityBuildProjectModel
        //    //{
        //    //    Id = project.Id,
        //    //    Name = project.Name,
        //    //    WebUri = project.WebUri,
        //    //    Description = project.Description,
        //    //    Archived = project.Archived,
        //    //    BuildTypes = types.Where(t => project.BuildTypes.Count(bt => bt.Id == t.Id) > 0)
        //    //         .Select(b => new TeamCityBuildTypeModel
        //    //         {
        //    //             Id = b.Id,
        //    //             Name = b.Name,
        //    //             Description = b.Description,
        //    //             Paused = b.Paused,
        //    //             RunParameters = b.RunParameters,
        //    //             WebUri = b.WebUri
        //    //         }).ToList()
        //    //}).ToArray();

        //    // set refs from types back to projects
        //    //foreach (var project in r)
        //    //{
        //    //    foreach (var buildType in project.BuildTypes)
        //    //    {
        //    //        buildType.BuildProject = project;
        //    //    }
        //    //}

        //    return r;
        //}

        public IEnumerable<TeamCityStatusData> GetTeamCityStatus()
        {
            if (!_isInitialised)
            {
                //_teamCityData = _teamCityClient.Projects.All().Select(p => new TeamCityStatusData
                //{
                //  ProjectId  = p.Id,
                //  ProjectName = p.Name
                //});
            }

            return _teamCityData;
        }

    }
}