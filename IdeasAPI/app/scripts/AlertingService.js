(function(common) {

    var alerting = function($timeout) {

        var currentAlerts = [];
        
        var addSuccess = function (message) {
            var alert = { type: "success", message: message };
            addAlert(alert);
        };

        var addDanger = function(message) {
            var alert = { type: "danger", message: message };
            addAlert(alert);
        };

        var addAlert = function (alert) {
	        console.log(alert);
        	currentAlerts.unshift(alert);
            $timeout(function() {
                for (var i = 0; i < currentAlerts.length; i++) {
                    if (currentAlerts[i] == alert) {
                        currentAlerts.splice(i, 1);
                        break;
                    }
                }
            }, 10000);
        };

        var errorHandler = function(description) {
            return function() {
                addDanger(description);
            };
        };


        return {
        	errorHandler: errorHandler,
        	addDanger: addDanger,
            addSuccess: addSuccess,
            currentAlerts: currentAlerts
        };

    };




    common.service("alertingService", ["$timeout", alerting]);

}(angular.module("Ideas")))