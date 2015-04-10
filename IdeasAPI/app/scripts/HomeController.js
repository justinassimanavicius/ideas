(function (app) {
	var homeController = function ($scope, itemService) {

		$scope.sortOrder = "date";

		itemService
			.getItems()
			.success(function (result) {
                        $scope.items = result;
                    });
	};

	app.controller("homeController", ["$scope", "itemService", homeController]);
}(angular.module("Ideas")));