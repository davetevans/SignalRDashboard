using System;
using System.Collections.Generic;
using System.Linq;
using ImapX;
using ImapX.Enums;
using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.Clients
{
    public class GmailClient
    {
        private string EmailAddress { get; set; }
        private string EmailPassword { get; set; }

        private static readonly List<string> InterestingKeywords = new List<string>
        {
            "coffee", "cafe", "caffe", "cofe", "cofee", "coffe", "cafe2u",
            "curry", "ice", "cream", "fire", "alarm", "van", "man",
            "french", "terrace",
            "is here",
            "test", "hello", "aids",
            "reception", "visitor", "parcel", "package"
        };

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
                    resultData.LastMailId = 0;
                    resultData.LastMailDateTime = now;
                }
                else
                {
                    using (var client = new ImapClient("imap.gmail.com", 993, true, false))
                    {
                        if (client.Connect())
                        {
                            if (client.Login(EmailAddress, EmailPassword))
                            {
                                var inbox = client.Folders.Inbox;
                                var messages = inbox.Search("ALL", MessageFetchMode.ClientDefault, 1);
                                var latestMessage = messages[0];
                                var newMailMessage = latestMessage.Subject?.Trim();
                                var newMailId = latestMessage.UId;
                                var newMailDateTime = latestMessage.Date ?? now;

                                if (IsInterestingMail(newMailMessage))
                                {
                                    resultData.LastMessage = newMailMessage;
                                    resultData.LastMailId = newMailId;
                                    resultData.LastMailDateTime = newMailDateTime;
                                }
                            }
                            else
                            {
                                resultData.LastMailId = -1;
                                resultData.LastMessage = "Can't login to Gmail!";
                                resultData.LastMailDateTime = now;
                            }
                        }
                        else
                        {
                            resultData.LastMailId = -2;
                            resultData.LastMessage = "Can't connect to Gmail!";
                            resultData.LastMailDateTime = now;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultData.LastMailId = -3;
                resultData.LastMessage = "Gmail Error!";
                resultData.LastMailDateTime = now;
            }

            return resultData;
        }

        private static bool IsInterestingMail(string content)
        {
            return InterestingKeywords.Any(content.ToLower().Contains);
        }
    }
}