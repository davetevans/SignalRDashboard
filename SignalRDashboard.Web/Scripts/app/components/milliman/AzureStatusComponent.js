(function (signalrdashboardMilliman) {
    function updateModelArray(currentModelArray, newModelArray) {
        for (var i = 0; i < newModelArray.length; i++) {
            var foundGroup = currentModelArray.find(function(group) { return group.groupId === newModelArray[i].groupId; });
            if (!foundGroup) {
                foundGroup = new signalrdashboardMilliman.AzureStatus();
                currentModelArray.push(foundGroup);
            } 
            foundGroup.updateFromData(newModelArray[i]);    
        }
    };

    signalrdashboardMilliman.AzureStatusComponent =
      ng.core.Component({
          selector: 'azure-status',
          templateUrl: '/scripts/app/templates/milliman/AzureStatusComponent.html'
      })
      .Class({
          constructor: function() {
              this.model = [];
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
                updateModelArray(model, stats);
            };
          },
          initialiseData: function(connection) {
              var model = this.model;
             
              connection.azureStatus.server.getModel().done(function(stats) {
                  updateModelArray(model, stats);
              });                    
          }
          });
})(window.signalrdashboard.milliman || (window.signalrdashboard.milliman = {}));