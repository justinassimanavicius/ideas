(function(module) {

	var requestInterceptor = function ($q, alertingService, $location) {

        
        var request = function(config) {
            return config;
        };

        var requestError = function(error) {
        	alertingService.addDanger();
            return $q.reject(error);
        };

        var response = function(response) {
            var type = response.data.type || response.data.Type
        	if (type === "Validation" || type === "Error") {
		        if (!response.config.silent) {
		        	alertingService.addDanger();
		        }
                return $q.reject(response);
            }
            return $q.when(response);
        };

        var responseError = function (error) {
        	if (error.status == 401) {
        		$location.path('/logout');
		        return;
	        }
        	alertingService.addDanger();
            return $q.reject(error);
        };

        
        return {
            request: request,
            response: response,
            requestError: requestError,
            responseError: responseError,
        };

    };

	module.factory("requestinterceptor", ["$q", "alertingService", "$location", requestInterceptor]);

    module.config(["$httpProvider",function($httpProvider) {
        $httpProvider.interceptors.push("requestinterceptor");
    }]);

}(angular.module("Ideas")));