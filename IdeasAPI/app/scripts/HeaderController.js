(function (app) {
	var headerController = function ($scope, $location) {

		$scope.isActive = function(path) {
			return path === $location.path();
		}
	};

	app.controller("headerController", ["$scope", "$location", headerController]);
}(angular.module("Ideas")));