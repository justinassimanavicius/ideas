(function (app) {
	var userService = function ($http, $q) {

		var user = null;


		var getUser = function() {
			if (user) {
				return $q(function(resolve, reject) {
						resolve(user);
					}
				);
			}
			return $http.get("api/user").then(function (result) {
				user = result.data;
				return user;
			});

		}


		return {
			getUser: getUser
		}
	};

	app.service("userService", ["$http", "$q", userService]);
}(angular.module("Ideas")));