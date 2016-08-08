(function(signalrdashboardMilliman) {
    signalrdashboardMilliman.NetworkStatus = NetworkStatus;

    function NetworkStatus() {
        // Properties matching data model received via SignalR
        this.canAccessInternet = false;
        this.canAccessAzure = false;
        this.canAccessTeamCity = false;

        // Error tracking properties
        this.somethingWrong = false;

        // Error check conditions
        var model = this;
        var errorChecks = [];
        errorChecks.push({ model: model, condition: function() { return !model.canAccessInternet || !model.canAccessAzure || !model.canAccessTeamCity; }, targetProperty: 'somethingWrong' });

        this.updateFromData = function(data) {            
            signalrdashboard.mapping.map(data, this);
            signalrdashboard.alertHandler.performErrorChecks('NetworkStatus', errorChecks);
        };
    }
    
})(window.signalrdashboard.milliman || (window.signalrdashboard.milliman = {}));