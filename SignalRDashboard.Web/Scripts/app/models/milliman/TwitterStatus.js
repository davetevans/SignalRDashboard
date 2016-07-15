(function(signalrdashboardMilliman) {
    signalrdashboardMilliman.TwitterStatus = TwitterStatus;

    function TwitterStatus() {
        // Properties matching data model received via SignalR
        this.lastTweet = '';
        this.lastTweetTime = '';
        this.tweetIsNew = false;

        // Error tracking properties
        this.hasNewTweet = false;

        // Error check conditions
        var model = this;
        var errorChecks = [];
        errorChecks.push({ model: model, condition: function() { return model.tweetIsNew; }, targetProperty: 'hasNewTweet' });

        this.updateFromData = function(data) {            
            signalrdashboard.mapping.map(data, this);
            signalrdashboard.alertHandler.performErrorChecks('TwitterStatus', errorChecks);
        };
    }
    
})(window.signalrdashboard.milliman || (window.signalrdashboard.milliman = {}));