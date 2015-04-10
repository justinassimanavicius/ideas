(function (app) {
	var createController = function ($scope, itemService) {

		$scope.item = {};

		$scope.saveItem = function() {
			itemService
				.saveItem($scope.item)
				.success(function() {
					ngDialog.open({
						template: '<p>my template</p>',
						plain: true
					});
			});
		}

	};

	app.controller("createController", ["$scope", "itemService", createController]);
}(angular.module("Ideas")));