using System.Configuration;
using SignalRDashboard.Data.Milliman.Clients;
using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class GmailStatusProvider
    {
        private readonly bool _isInitialised = false;
        private readonly GmailClient _client;
        private GmailStatusData _gmailData = new GmailStatusData();

        public GmailStatusProvider()
        {
            var config = ConfigurationManager.AppSettings;
            _client = new GmailClient(config["GmailAddress"], config["GmailPassword"]);
        }

        public GmailStatusData GetGmailStatus()
        {
            if (!_isInitialised)
            {
                _gmailData = _client.GetLatestMail();
            }

            return _gmailData;
        }

    }
}