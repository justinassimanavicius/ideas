(function (app) {
	var createItemController = function ($scope, itemService) {

		$scope.item = {};

		$scope.saveItem = function() {
			itemService
				.saveItem($scope.item)
				.success(function() {

					console.log("yay");
			});
		}

	};

	app.controller("createItemController", ["$scope", "itemService", createItemController]);
}(angular.module("Ideas")));