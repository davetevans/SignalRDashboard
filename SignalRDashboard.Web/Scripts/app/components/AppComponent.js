(function (signalrdashboard) {
    signalrdashboard.AppComponent =
      ng.core.Component({
            selector: "dashboard-app",            
            templateUrl: "/scripts/app/templates/core/DashboardComponent.html",
            directives: [
                signalrdashboard.core.ControlsComponent,
                signalrdashboard.milliman.TwitterStatusComponent
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
                signalrdashboard.demo.SiteStatusComponent,
                signalrdashboard.demo.SiteStatisticsComponent,
                signalrdashboard.demo.BuildMetricsComponent,
                signalrdashboard.milliman.TeamCityStatusComponent
*/