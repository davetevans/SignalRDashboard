using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public interface ITwitterStatusProvider
    {
        TwitterStatusData GetTwitterStatus();
    }
}
