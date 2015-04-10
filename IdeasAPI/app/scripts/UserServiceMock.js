(function (app) {
	var userService = function ($q) {
		
		var getUser = function() {

			return $q(function (resolve, reject) {
				
					var object = {
						name: "Jonas Jankauskas",
						avatar: "https://s3.amazonaws.com/uifaces/faces/twitter/jsa/128.jpg",
						isModerator: true
					};
					resolve(object);

			});
		}


		return {
			getUser: getUser
		}
	};

	app.service("userService", ["$q", userService]);
}(angular.module("Ideas")));