using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Hubs.Models
{
    public class TwitterStatus : DashboardHubModel
    {
        private string _lastTweet;
        public string LastTweet
        {
            get { return _lastTweet; }
            set { SetProperty(ref _lastTweet, value); }
        }
        private bool _tweetIsNew;
        public bool TweetIsNew
        {
            get { return _tweetIsNew; }
            set { SetProperty(ref _tweetIsNew, value); }
        }
    }
}