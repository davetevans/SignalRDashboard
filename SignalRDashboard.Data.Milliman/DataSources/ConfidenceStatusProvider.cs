using System;
using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class ConfidenceStatusProvider
    {
        private readonly bool _isInitialised = false;
        private ConfidenceStatusData _confidenceData = new ConfidenceStatusData();

        public ConfidenceStatusData GetConfidenceStatus()
        {
            if (!_isInitialised)
            {
                _confidenceData = new ConfidenceStatusData
                {
                    TeamCityConfidence = new Random().Next(0, 100)
                };
            }

            return _confidenceData;
        }
        
    }
}