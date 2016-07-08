using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.DataSources.Twitter
{
    public class TwitterClient
    {
        public string ScreenName { get; set; }
        public string OAuthConsumerSecret { get; set; }
        public string OAuthConsumerKey { get; set; }
        public string OAuthAccessToken { get; set; }

        public async Task<TwitterStatusData> GetLatestTweet(string accessToken = null)
        {
            if (accessToken == null)
            {
                accessToken = await GetAccessToken();
                OAuthAccessToken = accessToken;
            }

            var requestUserTimeline = new HttpRequestMessage(HttpMethod.Get,
                $"https://api.twitter.com/1.1/statuses/user_timeline.json?count=1&screen_name={ScreenName}&trim_user=1&exclude_replies=1");

            requestUserTimeline.Headers.Add("Authorization", "Bearer " + accessToken);
            var httpClient = new HttpClient();
            var serializer = new JavaScriptSerializer();
            var resultData = new TwitterStatusData();

            try
            {
                var responseUserTimeLine = await httpClient.SendAsync(requestUserTimeline);
                var json = serializer.Deserialize<List<dynamic>>(await responseUserTimeLine.Content.ReadAsStringAsync());
                resultData.LastTweetId = json == null ? 0 : (int)json[0]["id"];
                resultData.LastTweet = json == null ? string.Empty : json[0]["text"];
            }
            catch (Exception ex)
            {
                resultData.LastTweetId = -1;
                resultData.LastTweet = ex.Message;
            }

            return resultData;
        }

        private async Task<string> GetAccessToken()
        {
            var httpClient = new HttpClient();
            var customerInfo = Convert.ToBase64String(new UTF8Encoding().GetBytes(OAuthConsumerKey + ":" + OAuthConsumerSecret));
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/token ");
            request.Headers.Add("Authorization", "Basic " + customerInfo);
            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var serializer = new JavaScriptSerializer();
            dynamic item = serializer.Deserialize<object>(json);
            return item["access_token"];
        }
    }
}