using System;
using System.Linq;
using System.Threading;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRDashboard.Data.Core.Hubs;
using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Core.Subscribers
{
    public abstract class DatasourceSubscriber<TModel, THub> : IDisposable, IDatasourceSubscriber<TModel> 
        where TModel : DashboardHubModel, new()
        where THub : DashboardHub
    {
        private readonly string _hubName;
        private readonly Timer _timer;
        private bool _stopping;
        private readonly object _lockObject = new object();

        protected DatasourceSubscriber(
            IHubConnectionContext<dynamic> clients)
        {
            var hubNameAttribute = typeof(THub).GetCustomAttributes(typeof(HubNameAttribute), true).FirstOrDefault() as HubNameAttribute;
            if (hubNameAttribute == null) throw new ArgumentNullException("HubName attribute not found on THub type: " + nameof(THub));
            _hubName = hubNameAttribute.HubName;
           
            Clients = clients;
            Model = new TModel();
            _timer = new Timer(RefreshData, null, TimeSpan.FromSeconds(0), Timeout.InfiniteTimeSpan);
        }

        public TModel Model { get; }

        private void RefreshData(object state)
        {
            lock (_lockObject)
            {
                if (_stopping) return;

                RefreshData(Model);

                if (Model.HasChanged)
                {
                    BroadcastData(Model);
                }
          
                Model.ResetChangedState();
            }
        }

        protected abstract void RefreshData(TModel model);

        protected abstract void BroadcastData(TModel model);

        protected IHubConnectionContext<dynamic> Clients { get; private set; }

        public void Dispose()
        {
            lock (_lockObject)
            {
                if (!_stopping)
                {
                    _stopping = true;
                    _timer.Dispose();
                }
            }
        }
    }
}
