using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using FluentTc;
using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class NetworkStatusProvider
    {
        private readonly bool _isInitialised = false;
        private readonly NetworkStatusData _networkData = new NetworkStatusData();
        private static readonly string[] Addresses = { "google.com", "twitter.com" };

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

                var pingResults = pingTasks.Select(pingTask => pingTask.Result.Status == IPStatus.Success).ToList();

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