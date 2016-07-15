﻿using System;

namespace SignalRDashboard.Data.Milliman.DataSources.Models
{
    public class TwitterStatusData
    {
        public int LastTweetId { get; set; }
        public string LastTweet { get; set; }
        public DateTime LastTweetDateTime { get; set; }
    }
}