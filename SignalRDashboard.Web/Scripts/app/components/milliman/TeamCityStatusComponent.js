(function (signalrdashboardMilliman) {
    signalrdashboardMilliman.TeamCityStatusComponent =
      ng.core.Component({
          selector: 'team-city-status',
          templateUrl: '/scripts/app/templates/milliman/TeamCityStatusComponent.html'
      })
      .Class({
          constructor: function() {
              this.model = new signalrdashboardMilliman.TeamCityStatus();
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
                model.updateFromData(stats);
            };
          },
          initialiseData: function(connection) {
              var model = this.model;
             
              connection.teamCityStatus.server.getModel().done(function(stats) {
                  model.updateFromData(stats);
              });                    
          }
          });
})(window.signalrdashboard.milliman || (window.signalrdashboard.milliman = {}));