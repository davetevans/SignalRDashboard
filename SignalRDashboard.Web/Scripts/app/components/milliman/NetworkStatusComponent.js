(function (signalrdashboardMilliman) {
    signalrdashboardMilliman.NetworkStatusComponent =
      ng.core.Component({
          selector: 'network-status',
          templateUrl: '/scripts/app/templates/milliman/NetworkStatusComponent.html'
      })
      .Class({
          constructor: function() {
              this.model = new signalrdashboardMilliman.NetworkStatus();
              this.componentName = 'NetworkStatus';
              signalrdashboard.dashboard.registerComponent(this);
          },
          ngOnInit: function() {              
              signalrdashboard.dashboard.completeComponentRegistration();             
          },
          setupHub : function(connection) {
            var hub = connection.networkStatus;
            var model = this.model;
            
            // Add a client-side hub method that the server will call
            hub.client.updateNetworkStatus = function(stats) {
                model.updateFromData(stats);
            };
          },
          initialiseData: function(connection) {
              var model = this.model;
             
              connection.networkStatus.server.getModel().done(function(stats) {
                  model.updateFromData(stats);
              });                    
          }
          });
})(window.signalrdashboard.milliman || (window.signalrdashboard.milliman = {}));