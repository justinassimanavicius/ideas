(function (app) {
	var userController = function ($scope, userService) {

		userService
			.getUser()
			.then(function (user) {
			$scope.user = user;
		});

		
	}

	app.controller("userController", ["$scope", "userService", userController]);
}(angular.module("Ideas")));