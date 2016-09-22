(function(signalrdashboardMilliman) {
    signalrdashboardMilliman.ConfidenceStatus = ConfidenceStatus;

    function ConfidenceStatus() {
        // Properties matching data model received via SignalR
        this.teamCityConfidence = '';

        // Error check conditions
        var model = this;
        var errorChecks = [];

        this.updateFromData = function(data) {            
            signalrdashboard.mapping.map(data, this);
            signalrdashboard.alertHandler.performErrorChecks('ConfidenceStatus', errorChecks);
        };
    }
    
})(window.signalrdashboard.milliman || (window.signalrdashboard.milliman = {}));