(function (module) {

	var createItem = function () {
		return {
			restrict: "AE",
			templateUrl: "app/templates/createItem.html",
			replace: true,
		};
	};

	module.directive("createItem", [ createItem]);

}(angular.module("Ideas")));