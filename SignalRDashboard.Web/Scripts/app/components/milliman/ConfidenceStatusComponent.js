(function (signalrdashboardMilliman) {
    signalrdashboardMilliman.ConfidenceStatusComponent =
      ng.core.Component({
          selector: 'confidence-status',
          templateUrl: '/scripts/app/templates/milliman/ConfidenceStatusComponent.html'
      })
      .Class({
          constructor: function() {
              this.model = new signalrdashboardMilliman.ConfidenceStatus();
              this.componentName = 'ConfidenceStatus';
              signalrdashboard.dashboard.registerComponent(this);
          },
          ngOnInit: function() {              
              signalrdashboard.dashboard.completeComponentRegistration();             
          },
          setupHub : function(connection) {
            var hub = connection.confidenceStatus;
            var model = this.model;
            
            // Add a client-side hub method that the server will call
            hub.client.updateConfidenceStatus = function(stats) {
                model.updateFromData(stats);
            };
          },
          initialiseData: function(connection) {
              var model = this.model;
             
              connection.confidenceStatus.server.getModel().done(function(stats) {
                  model.updateFromData(stats);
              });                    
          }
          });
})(window.signalrdashboard.milliman || (window.signalrdashboard.milliman = {}));