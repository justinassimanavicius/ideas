(function (app) {
	var createController = function ($scope, itemService, ngDialog) {

		$scope.item = {};

		$scope.saveItem = function() {
			itemService.saveItem($scope.item);
		}

	};

	app.controller("createController", ["$scope", "itemService", createController]);
}(angular.module("Ideas")));