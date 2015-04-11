(function (app) {
	var createItemController = function ($scope, itemService, $location) {

		$scope.item = {};

		$scope.saveItem = function() {
			itemService
				.saveItem($scope.item)
				.success(function() {

					$location.path('/home');
			});
		}



	};

	app.controller("createItemController", ["$scope", "itemService", "$location", createItemController]);
}(angular.module("Ideas")));