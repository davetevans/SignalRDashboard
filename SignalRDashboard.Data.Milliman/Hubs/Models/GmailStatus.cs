using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Milliman.Hubs.Models
{
    public class GmailStatus : DashboardHubModel
    {
        private string _lastMail;
        public string LastMail
        {
            get { return _lastMail; }
            set { SetProperty(ref _lastMail, value); }
        }
        private string _lastMailId;
        public string LastMailId
        {
            get { return _lastMailId; }
            set { SetProperty(ref _lastMailId, value); }
        }
        private bool _mailIsNew;
        public bool MailIsNew
        {
            get { return _mailIsNew; }
            set { SetProperty(ref _mailIsNew, value); }
        }
        private string _lastMailTime;
        public string LastMailTime
        {
            get { return _lastMailTime; }
            set { SetProperty(ref _lastMailTime, value); }
        }
    }
}