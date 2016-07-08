using SignalRDashboard.Data.Milliman.DataSources.Models;
using SignalRDashboard.Data.Milliman.DataSources.Twitter;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class TwitterStatusProvider : ITwitterStatusProvider
    {
        private readonly bool _isInitialised = false;
        private readonly TwitterClient _twitterClient = new TwitterClient();
        private TwitterStatusData _twitterData = new TwitterStatusData();

        public TwitterStatusProvider()
        {
            _twitterClient.ScreenName = "sandwichvanspam";
            _twitterClient.OAuthConsumerKey = "7SsZrUGslefI2XnC51hUQyGnI";
            _twitterClient.OAuthConsumerSecret = "SgqayvwaOi58o2bMbgPLmVZGz0V5gwoRVciXPHueQpn7KUZBMS";
            _twitterClient.OAuthAccessToken = null;
        }

        public TwitterStatusData GetTwitterStatus()
        {
            if (!_isInitialised)
            {
                _twitterData = _twitterClient.GetLatestTweet(_twitterClient.OAuthAccessToken).Result;
            }

            return _twitterData;
        }
        
    }
}