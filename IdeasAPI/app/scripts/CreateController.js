(function (app) {
	var createController = function ($scope, itemService, ngDialog) {

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

	app.controller("createController", ["$scope", "itemService", "ngDialog", createController]);
}(angular.module("Ideas")));