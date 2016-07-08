//using System;
//using System.Collections.Generic;

//namespace SignalRDashboard.Data.Milliman.DataSources.TeamCity
//{
//    public class TeamCityBuildProjectModel
//    {
//        private string _id;

//        public string Id
//        {
//            get { return _id; }
//            set { SetField(ref _id, value, () => Id); }
//        }

//        private string _Name;

//        public string Name
//        {
//            get { return _Name; }
//            set { SetField(ref _Name, value, () => Name); }
//        }

//        private Uri _WebUri;

//        public Uri WebUri
//        {
//            get { return _WebUri; }
//            set { SetField(ref _WebUri, value, () => WebUri); }
//        }

//        private string _Description;

//        public string Description
//        {
//            get { return _Description; }
//            set { SetField(ref _Description, value, () => Description); }
//        }

//        private bool _Archived;

//        public bool Archived
//        {
//            get { return _Archived; }
//            set { SetField(ref _Archived, value, () => Archived); }
//        }

//        private List<TeamCityBuildTypeModel> _BuildTypes;

//        public List<TeamCityBuildTypeModel> BuildTypes
//        {
//            get { return _BuildTypes; }
//            set { SetField(ref _BuildTypes, value, () => BuildTypes); }
//        }
//    }
//}