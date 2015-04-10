(function (app) {
	var userService = function ($http) {
		
		var getUser = function() {

			return $http.get("api/user").then(function (result){return result.data});

		}


		return {
			getUser: getUser
		}
	};

	app.service("userService", ["$http", userService]);
}(angular.module("Ideas")));