(function (signalrdashboardMilliman) {
    signalrdashboardMilliman.TwitterStatusComponent =
      ng.core.Component({
          selector: 'twitter-status',
          templateUrl: '/scripts/app/templates/milliman/TwitterStatusComponent.html'
      })
      .Class({
          constructor: function() {
              this.model = new signalrdashboardMilliman.TwitterStatus();
              this.componentName = 'TwitterStatus';
              signalrdashboard.dashboard.registerComponent(this);
          },
          ngOnInit: function() {              
              signalrdashboard.dashboard.completeComponentRegistration();             
          },
          setupHub : function(connection) {
            var hub = connection.twitterStatus;
            var model = this.model;
            
            // Add a client-side hub method that the server will call
            hub.client.updateTwitterStatus = function(stats) {
                model.updateFromData(stats);
            };
          },
          initialiseData: function(connection) {
              var model = this.model;
             
              connection.twitterStatus.server.getModel().done(function(stats) {
                  model.updateFromData(stats);
              });                    
          }
          });
})(window.signalrdashboard.milliman || (window.signalrdashboard.milliman = {}));