using SignalRDashboard.Data.Core.Hubs.Models;

namespace SignalRDashboard.Data.Core.Subscribers
{
    public interface IDatasourceSubscriber<out TModel> where TModel:DashboardHubModel
    {
        TModel Model { get; }
    }
}