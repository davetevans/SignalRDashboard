using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Pollers;
using SignalRDashboard.Data.Milliman.DataSources;
using SignalRDashboard.Data.Milliman.Hubs;
using SignalRDashboard.Data.Milliman.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Pollers
{
    public class TwitterStatusPoller : DatasourcePoller<TwitterStatus, TwitterStatusHub>
    {
        private static readonly Lazy<TwitterStatusPoller> PollerInstance = new Lazy<TwitterStatusPoller>(() => new TwitterStatusPoller(GlobalHost.ConnectionManager.GetHubContext<TwitterStatusHub>().Clients));
        private readonly TwitterStatusProvider _provider;
        private int _lastTweetId;

        private TwitterStatusPoller(IHubConnectionContext<dynamic> clients)
            : base(clients, TimeSpan.FromSeconds(10), new PollOnlyWhenUsersAreConnectedStrategy())
        {
            _provider = new TwitterStatusProvider();
        }

        public static TwitterStatusPoller Instance => PollerInstance.Value;

        protected override void RefreshData(TwitterStatus model)
        {
            var data = _provider.GetTwitterStatus();
            var lastTweet = data.LastTweet.Trim();
            var lastTweetId = data.LastTweetId;
            var lastTweetTime = $"{data.LastTweetDateTime:t}";
            model.TweetIsNew = false;

            if (_lastTweetId != lastTweetId)
            {
                model.TweetIsNew = true;
                model.LastTweet = lastTweet;
                model.LastTweetTime = lastTweetTime;
                _lastTweetId = lastTweetId;
            }
        }

        protected override void BroadcastData(TwitterStatus model)
        {
            Clients.All.updateTwitterStatus(model);
        }
    }
}