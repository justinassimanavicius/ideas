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
			$scope.selectedFilter = $scope.allFilter;
		}

		$scope.showMy = function () {
			$scope.selectedFilter = $scope.myFilter;
		}

		$scope.showUnaproved = function () {
			$scope.selectedFilter = $scope.unaprovedFilter;
		}

		$scope.allFilter = null;

		$scope.myFilter = function (actual, expected) {
			return actual.author == $scope.userName;
		}

		$scope.unaprovedFilter = function (actual, expected) {
			return actual.status == "Pending";
		}

		$scope.toggleDropdown = function ($event) {
			$event.preventDefault();
			$event.stopPropagation();
			$scope.status.isopen = !$scope.status.isopen;
		};

		$scope.toggled = function (open) {
			console.log('Dropdown is now: ', open);
		};


	};

	app.controller("homeController", ["$scope", "itemService", "userService", homeController]);
}(angular.module("Ideas")));