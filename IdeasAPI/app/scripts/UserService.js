(function (app) {
	var userService = function ($http, $q) {
		
		var getUser = function() {

			return $q(function(resolve, reject) {
				setTimeout(function() {
					var object = {
						name: "Jonas Jankauskas",
						avatar: "home/dicpic"
					};
					resolve(object);

				}, 1000);
			});
		}


		return {
			getUser: getUser
		}
	};

	app.service("userService", ["$http", "$q", userService]);
}(angular.module("Ideas")));