(function (app) {
	var homeController = function ($scope, itemService, userService) {

		$scope.sortOrder = "date";

		itemService
			.getItems()
			.success(function (result) {
                        $scope.items = result;
			});

		userService.getUser()
			.then(function(user) {
				$scope.isModerator = user.isModerator;
				$scope.userName = user.name;
			});

		$scope.selectedFilter = null;

		$scope.showAll = function() {
			$scope.selectedFilter = null;
		}

		$scope.showMy = function () {
			$scope.selectedFilter = function(actual, expected) {
				return actual.author == $scope.userName;
			}
		}

		$scope.showUnaproved = function () {
			$scope.selectedFilter = function (actual, expected) {
				return actual.status == "Pending";
			}
		}
	};

	app.controller("homeController", ["$scope", "itemService", "userService", homeController]);
}(angular.module("Ideas")));