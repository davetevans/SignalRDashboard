﻿(function(signalrdashboardMilliman) {
    signalrdashboardMilliman.AzureStatus = AzureStatus;

    function AzureStatus() {
        // Properties matching data model received via SignalR
        this.id = '';
        this.name = '';

        // Error check conditions
        var model = this;
        var errorChecks = [];

        this.updateFromData = function(data) {            
            signalrdashboard.mapping.map(data, this);
            signalrdashboard.alertHandler.performErrorChecks('AzureStatus', errorChecks);
        };
    }
    
})(window.signalrdashboard.milliman || (window.signalrdashboard.milliman = {}));