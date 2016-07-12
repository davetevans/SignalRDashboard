(function(signalrdashboardMilliman) {
    signalrdashboardMilliman.TeamCityStatus = TeamCityStatus;

    function TeamCityStatus() {
        // Properties matching data model received via SignalR
        this.projectId = "";
        this.projectName = "";
        this.buildConfigs = [];

        // Error check conditions
        var model = this;
        var errorChecks = [];

        this.updateFromData = function(data) {            
            signalrdashboard.mapping.map(data, this);
            signalrdashboard.alertHandler.performErrorChecks('TeamCityStatus', errorChecks);
        };
    }
    
})(window.signalrdashboard.milliman || (window.signalrdashboard.milliman = {}));