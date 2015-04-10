(function (app) {
	var itemController = function ($scope, $routeParams) {
		$scope.id = $routeParams.id;
	};

	app.controller("itemController", ["$scope", "$routeParams", itemController]);
}(angular.module("Ideas")));