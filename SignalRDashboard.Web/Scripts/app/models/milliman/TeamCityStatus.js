(function(signalrdashboardMilliman) {
    signalrdashboardMilliman.TeamCityStatus = TeamCityStatus;

    function TeamCityStatus() {
        // Properties matching data model received via SignalR
        this.projectId = "";
        this.projectName = "";

        // Error tracking properties
        //this.hasNewTweet = false;

        // Error check conditions
        var model = this;
        var errorChecks = [];
        //errorChecks.push({ model: model, condition: function() { return model.tweetIsNew; }, targetProperty: 'hasNewTweet' });

        this.updateFromData = function(data) {            
            signalrdashboard.mapping.map(data, this);
            signalrdashboard.alertHandler.performErrorChecks('TeamCityStatus', errorChecks);
        };
    }
    
})(window.signalrdashboard.milliman || (window.signalrdashboard.milliman = {}));