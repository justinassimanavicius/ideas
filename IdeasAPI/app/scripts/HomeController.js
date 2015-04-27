(function (app) {
	var homeController = function ($scope, itemService, userService) {

		$scope.sortOrder = "createDate";
		$scope.loadingItems = true;

		itemService
			.getItems()
			.success(function(result) {
				$scope.items = result;
				$scope.loadingItems = false;
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

		$scope.showUnapproved = function () {
			$scope.selectedFilter = $scope.unapprovedFilter;
		}

		$scope.allFilter = null;

		$scope.myFilter = function (actual, expected) {
			return actual.author == $scope.userName;
		}

		$scope.unapprovedFilter = function (actual, expected) {
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

		$scope.status = {
			isopen: false
		};


		$scope.sortBy = function (order) {
			$scope.sortOrder = order;
		}
	};

	app.controller("homeController", ["$scope", "itemService", "userService", homeController]);
}(angular.module("Ideas")));