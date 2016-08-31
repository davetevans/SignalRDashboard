using System.Configuration;
using SignalRDashboard.Data.Milliman.Clients;
using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class TwitterStatusProvider
    {
        private readonly bool _isInitialised = false;
        private readonly TwitterClient _client = new TwitterClient();
        private TwitterStatusData _twitterData = new TwitterStatusData();

        public TwitterStatusProvider()
        {
            var config = ConfigurationManager.AppSettings;
            _client.ScreenName = config["TwitterScreenName"];
            _client.OAuthConsumerKey = config["TwitterKey"];
            _client.OAuthConsumerSecret = config["TwitterSecret"];
            _client.OAuthAccessToken = null;
        }

        public TwitterStatusData GetTwitterStatus()
        {
            if (!_isInitialised)
            {
                _twitterData = _client.GetLatestTweet(_client.OAuthAccessToken).Result;
            }

            return _twitterData;
        }
        
    }
}