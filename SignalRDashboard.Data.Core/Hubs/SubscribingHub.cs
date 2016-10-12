using System.Threading.Tasks;
using SignalRDashboard.Data.Core.Hubs.Models;
using SignalRDashboard.Data.Core.Subscribers;

namespace SignalRDashboard.Data.Core.Hubs
{
    public abstract class SubscribingHub<TModel> : DashboardHub where TModel:DashboardHubModel
    {
        private readonly IDatasourceSubscriber<TModel> _datasourceSubscriber;

        protected SubscribingHub(IHubUserConnectionTrackingStrategy connectionTrackingStrategy, IDatasourceSubscriber<TModel> datasourceSubscriber) : base(connectionTrackingStrategy)
        {
            _datasourceSubscriber = datasourceSubscriber;
        }

        protected TModel Model => _datasourceSubscriber.Model;

        public override Task OnConnected()
        {
            var task = base.OnConnected();
            return task;
        }

        public virtual TModel GetModel()
        {
            return Model;
        }
    }
}