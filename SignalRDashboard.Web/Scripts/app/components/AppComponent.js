﻿(function (signalrdashboard) {
    signalrdashboard.AppComponent =
      ng.core.Component({
            selector: "dashboard-app",            
            templateUrl: "/scripts/app/templates/core/DashboardComponent.html",
            directives: [
                signalrdashboard.core.ControlsComponent,
                signalrdashboard.milliman.TeamCityStatusComponent,
                signalrdashboard.milliman.AzureStatusComponent,
                signalrdashboard.milliman.NetworkStatusComponent,
                signalrdashboard.milliman.ConfidenceStatusComponent,
                signalrdashboard.milliman.GmailStatusComponent
            ]
          }    
      )
      .Class({
          constructor: function() {              
          }          
      });
})(window.signalrdashboard || (window.signalrdashboard = {}));