using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using SignalRDashboard.Data.Milliman.Clients;
using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class NetworkStatusProvider
    {
        private readonly bool _isInitialised = false;
        private readonly NetworkStatusData _networkData = new NetworkStatusData();
        private static readonly string[] Addresses = { "google.com", "twitter.com" };
        private readonly TeamCityClient _client;

        public NetworkStatusProvider()
        {
            var config = ConfigurationManager.AppSettings;

            _client = new TeamCityClient
            {
                TeamCityUrl = config["TeamCityUrl"],
                TeamCityUsername = config["TeamCityUsername"],
                TeamCityPassword = config["TeamCityPassword"]
            };

            _client.Init();
        }

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
            bool internetResult;

            try
            {
                var pingTasks = Addresses.Select(PingAsync).ToList();

                Task.WaitAll(pingTasks.ToArray());

                var pingResults = pingTasks.Select(pingTask => pingTask.Result?.Status == IPStatus.Success).ToList();

                internetResult = pingResults.TrueForAll(p => true);

            }
            catch (Exception)
            {
                internetResult = false;
            }

            return internetResult;
        }

        private static Task<PingReply> PingAsync(string address)
        {
            var tcs = new TaskCompletionSource<PingReply>();
            var ping = new Ping();
            ping.PingCompleted += (obj, sender) =>
            {
                tcs.SetResult(sender.Reply);
            };
            ping.SendAsync(address, new object());
            return tcs.Task;
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

        private bool CheckTeamCityConnection()
        {
            bool teamCityResult;

            try
            {
                teamCityResult = _client.TestConnection();
            }
            catch (Exception ex)
            {
                teamCityResult = false;
            }

            return teamCityResult;
        }
    }
}