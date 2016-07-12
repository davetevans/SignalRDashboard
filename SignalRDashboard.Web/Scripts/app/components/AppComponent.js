(function (signalrdashboard) {
    signalrdashboard.AppComponent =
      ng.core.Component({
            selector: "dashboard-app",            
            templateUrl: "/scripts/app/templates/core/DashboardComponent.html",
            directives: [
                signalrdashboard.core.ControlsComponent,
                signalrdashboard.milliman.TeamCityStatusComponent,
                signalrdashboard.milliman.TwitterStatusComponent,
                signalrdashboard.demo.BuildMetricsComponent
            ]
          }    
      )
      .Class({
          constructor: function() {              
          }          
      });
})(window.signalrdashboard || (window.signalrdashboard = {}));

/*
                signalrdashboard.core.ConnectedUsersComponent,
                signalrdashboard.demo.SiteStatisticsComponent,
                signalrdashboard.demo.SiteStatusComponent
*/