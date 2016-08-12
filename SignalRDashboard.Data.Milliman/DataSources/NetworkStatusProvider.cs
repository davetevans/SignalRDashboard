using System;
using System.Configuration;
using System.Net;
using System.Net.NetworkInformation;
using FluentTc;
using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class NetworkStatusProvider
    {
        private readonly bool _isInitialised = false;
        private readonly NetworkStatusData _networkData = new NetworkStatusData();

        public NetworkStatusData GetNetworkStatus()
        {
            if (!_isInitialised)
            {
                _networkData.CanAccessInternet = CheckInternetConnection();
                _networkData.CanAccessAzure = CheckAzureConnection();
                _networkData.CanAccessTeamCity = CheckTeamCityConnection();
            }

            return _networkData;
        }

        private static bool CheckInternetConnection()
        {
            var ping = new Ping();
            bool internetResult;

            try
            {
                var pingGoogle = ping.Send("www.google.com", 5000);
                var pingTwitter = ping.Send("www.twitter.com", 5000);
                internetResult = pingGoogle?.Status == IPStatus.Success && pingTwitter?.Status == IPStatus.Success;
            }
            catch (Exception ex)
            {
                internetResult = false;
            }

            return internetResult;
        }

        private static bool CheckAzureConnection()
        {
            bool azureResult;

            try
            {
                var request = WebRequest.Create("https://portal.azure.com");
                var response = (HttpWebResponse)request.GetResponse();
                azureResult = response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                azureResult = false;
            }

            return azureResult;
        }

        private static bool CheckTeamCityConnection()
        {
            bool teamCityResult;
            var config = ConfigurationManager.AppSettings;
            var baseUrl = config["TeamCityUrl"];
            var username = config["TeamCityUsername"];
            var password = config["TeamCityPassword"];
            
            try
            {
                var teamCityClient = new RemoteTc().Connect(a => a.ToHost(baseUrl).UseSsl().AsUser(username, password));
                var testGetUser = teamCityClient.GetUser(_ => _.Username(username));
                teamCityResult = testGetUser != null;
            }
            catch (Exception ex)
            {
                teamCityResult = false;
            }

            return teamCityResult;
        }
    }
}