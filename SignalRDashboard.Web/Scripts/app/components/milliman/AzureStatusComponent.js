(function (signalrdashboardMilliman) {
    signalrdashboardMilliman.AzureStatusComponent =
      ng.core.Component({
          selector: 'azure-status',
          templateUrl: '/scripts/app/templates/milliman/AzureStatusComponent.html'
      })
      .Class({
          constructor: function() {
              this.model = new signalrdashboardMilliman.AzureStatus();
              this.componentName = 'AzureStatus';
              signalrdashboard.dashboard.registerComponent(this);
          },
          ngOnInit: function() {              
              signalrdashboard.dashboard.completeComponentRegistration();             
          },
          setupHub : function(connection) {
            var hub = connection.azureStatus;
            var model = this.model;
            
            // Add a client-side hub method that the server will call
            hub.client.updateAzureStatus = function(stats) {
                model.updateFromData(stats);
            };
          },
          initialiseData: function(connection) {
              var model = this.model;
             
              connection.azureStatus.server.getModel().done(function(stats) {
                  model.updateFromData(stats);
              });                    
          }
          });
})(window.signalrdashboard.milliman || (window.signalrdashboard.milliman = {}));