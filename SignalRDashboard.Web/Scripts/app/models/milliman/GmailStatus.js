(function(signalrdashboardMilliman) {
    signalrdashboardMilliman.GmailStatus = GmailStatus;

    function GmailStatus() {
        // Properties matching data model received via SignalR
        this.lastMail = '';
        this.lastMailTime = '';
        this.mailIsNew = false;

        // Error tracking properties
        this.hasNewMail = false;

        // Error check conditions
        var model = this;
        var errorChecks = [];
        errorChecks.push({ model: model, condition: function() { return model.mailIsNew; }, targetProperty: 'hasNewMail' });

        this.updateFromData = function(data) {            
            signalrdashboard.mapping.map(data, this);
            signalrdashboard.alertHandler.performErrorChecks('GmailStatus', errorChecks);
        };
    }
    
})(window.signalrdashboard.milliman || (window.signalrdashboard.milliman = {}));