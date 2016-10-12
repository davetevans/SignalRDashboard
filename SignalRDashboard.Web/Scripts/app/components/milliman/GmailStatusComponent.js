(function (signalrdashboardMilliman) {
    signalrdashboardMilliman.GmailStatusComponent =
      ng.core.Component({
          selector: 'gmail-status',
          templateUrl: '/scripts/app/templates/milliman/GmailStatusComponent.html'
      })
      .Class({
          constructor: function() {
              this.model = new signalrdashboardMilliman.GmailStatus();
              this.componentName = 'GmailStatus';
              signalrdashboard.dashboard.registerComponent(this);
          },
          ngOnInit: function() {              
              signalrdashboard.dashboard.completeComponentRegistration();             
          },
          setupHub : function(connection) {
            var hub = connection.gmailStatus;
            var model = this.model;
            
            // Add a client-side hub method that the server will call
            hub.client.updateGmailStatus = function(stats) {
                Cookies.set("lastMail", stats.lastMail); 
                Cookies.set("lastMailTime", stats.lastMailTime); 
                model.updateFromData(stats);
            };
          },
          initialiseData: function() {
              var model = this.model;
              var defaultMail = Cookies.get("lastMail") === "null" ? null : Cookies.get("lastMail"); 
              var defaultLastMailTime = Cookies.get("lastMailTime") === "null" ? null : Cookies.get("lastMailTime"); 
              var gmailStatus = {
                lastMail: defaultMail || "where's the f'ing coffee van!",
                lastMailTime: defaultLastMailTime || "09:00",
                mailIsNew: false
              };
              model.updateFromData(gmailStatus);                   
          }
    });
})(window.signalrdashboard.milliman || (window.signalrdashboard.milliman = {}));