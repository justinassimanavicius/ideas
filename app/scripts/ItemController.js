(function (app) {
	var itemController = function ($scope, $routeParams, itemService) {
		var id = $routeParams.id;
		
		itemService
			.getItem(id)
			.success(function (result) {
                        $scope.item = result;
                    });
	};

	app.controller("itemController", ["$scope", "$routeParams", "itemService", itemController]);
}(angular.module("Ideas")));