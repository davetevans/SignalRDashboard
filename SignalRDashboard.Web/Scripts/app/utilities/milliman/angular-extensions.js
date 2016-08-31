(function(signalrdashboardMilliman) {

    signalrdashboardMilliman.filter('percentage', ['$filter', function ($filter) {
      return function (input, decimals) {
        return $filter('number')(input * 100, decimals) + '%';
      };
    }]);
    
})(window.signalrdashboard.milliman || (window.signalrdashboard.milliman = {}));