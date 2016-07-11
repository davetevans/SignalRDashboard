(function (signalrdashboardMilliman) {
    function updateModelArray(currentModelArray, newModelArray) {
        for (var i = 0; i < newModelArray.length; i++) {
            var foundProject = currentModelArray.find(function(project) { return project.projectId === newModelArray[i].projectId; });
            if (!foundProject) {
                foundProject = new signalrdashboardMilliman.TeamCityStatus();
                currentModelArray.push(foundProject);
            } 
            foundProject.updateFromData(newModelArray[i]);    
        }
    };

    signalrdashboardMilliman.TeamCityStatusComponent =
      ng.core.Component({
          selector: 'team-city-status',
          templateUrl: '/scripts/app/templates/milliman/TeamCityStatusComponent.html'
      })
      .Class({
          constructor: function() {
              this.model = [];
              this.componentName = 'TeamCityStatus';
              signalrdashboard.dashboard.registerComponent(this);
          },
          ngOnInit: function() {              
              signalrdashboard.dashboard.completeComponentRegistration();             
          },
          setupHub : function(connection) {
            var hub = connection.teamCityStatus;
            var model = this.model;
            
            // Add a client-side hub method that the server will call
            hub.client.updateTeamCityStatus = function(stats) {
                updateModelArray(model, stats);
            };
          },
          initialiseData: function(connection) {
              var model = this.model;
             
              connection.teamCityStatus.server.getModel().done(function(stats) {
                  updateModelArray(model, stats);
              });                    
          }
          });
})(window.signalrdashboard.milliman || (window.signalrdashboard.milliman = {}));