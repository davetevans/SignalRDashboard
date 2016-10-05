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
                model.updateFromData(stats);
            };
          },
          initialiseData: function(connection) {
              var model = this.model;
             
              connection.gmailStatus.server.getModel().done(function(stats) {
                  model.updateFromData(stats);
              });                    
          }
          });
})(window.signalrdashboard.milliman || (window.signalrdashboard.milliman = {}));