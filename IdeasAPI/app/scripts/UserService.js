(function (app) {
	var userService = function ($http, $q) {
		
		var getUser = function() {

			return $q(function(resolve, reject) {
				setTimeout(function() {
					var object = {
						name: "Jonas",
						avatar: "http://t0.gstatic.com/images?q=tbn:ANd9GcQyCEyETo6F5StYkcs2lyapx_uyZK40Lw7oIktFTSWbDcR5JusjqA"
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