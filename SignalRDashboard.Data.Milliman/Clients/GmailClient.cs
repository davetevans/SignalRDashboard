using System;
using System.Collections.Generic;
using System.Linq;
using AE.Net.Mail;
using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.Clients
{
    public class GmailClient
    {
        private string EmailAddress { get; set; }
        private string EmailPassword { get; set; }

        public GmailClient(string emailAddress, string emailPassword)
        {
            EmailAddress = emailAddress;
            EmailPassword = emailPassword;
        }

        public GmailStatusData GetLatestMail()
        {
            var resultData = new GmailStatusData();
            var now = DateTime.Now;

            try
            {
                // hack for 8am Seattle awake alert
                if (now.Hour == 15 && now.Minute == 00)
                {
                    resultData.LastMessage = "Good morning Seattle!";
                    resultData.LastMailId = "1600";
                    resultData.LastMailDateTime = now;
                }
                else
                {
                    using (var imap = new ImapClient("imap.gmail.com", EmailAddress, EmailPassword, AuthMethods.Login, 993, true))
                    {
                        imap.SelectMailbox("Inbox");
                        var messageCount = imap.GetMessageCount("Inbox");
                        var latestMessage = imap.GetMessage(messageCount - 1);
                        if (IsInterestingMail(latestMessage.Subject))
                        {
                            resultData.LastMessage = latestMessage.Subject;
                            resultData.LastMailId = latestMessage.MessageID;
                            resultData.LastMailDateTime = latestMessage.Date;
                        }
                    }
                }
            }
            catch (Exception)
            {
                resultData.LastMailId = string.Empty;
                resultData.LastMessage = "Check Gmail!";
                resultData.LastMailDateTime = DateTime.Now;
            }

            return resultData;
        }

        private static bool IsInterestingMail(string content)
        {
            return InterestingKeywords.Any(content.Contains);
        }

        private static readonly List<string> InterestingKeywords = new List<string>
        {
            "coffee", "cafe", "caffe", "cofe", "cofee", "coffe", "cafe2u",
            "curry", "ice", "cream", "fire", "alarm", "van", "man",
            "french", "terrace",
            "is here"
        };
    }
}