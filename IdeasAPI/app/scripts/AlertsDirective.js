(function(module) {

    var alerts = function(alertingService) {
        return {
        	restrict: "AE",
        	templateUrl: "app/templates/alert.html",
            replace: true,
            link: function(scope) {
            	scope.currentAlerts = alertingService.currentAlerts;
				scope.clear = function() {
					scope.currentAlerts.splice(0);
				},
				scope.isOfType = function (type) {
					var item = scope.currentAlerts[0] || {};
					return item.type == type;

				}
            }
        };
    };

    module.directive("alerts", ["alertingService",  alerts]);

}(angular.module("Ideas")));