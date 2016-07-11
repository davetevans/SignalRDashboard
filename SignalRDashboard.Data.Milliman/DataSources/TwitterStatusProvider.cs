using SignalRDashboard.Data.Milliman.DataSources.Models;
using SignalRDashboard.Data.Milliman.DataSources.Twitter;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class TwitterStatusProvider : ITwitterStatusProvider
    {
        private readonly bool _isInitialised = false;
        private readonly TwitterClient _client = new TwitterClient();
        private TwitterStatusData _twitterData = new TwitterStatusData();

        public TwitterStatusProvider()
        {
            _client.ScreenName = "sandwichvanspam";
            _client.OAuthConsumerKey = "7SsZrUGslefI2XnC51hUQyGnI";
            _client.OAuthConsumerSecret = "SgqayvwaOi58o2bMbgPLmVZGz0V5gwoRVciXPHueQpn7KUZBMS";
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