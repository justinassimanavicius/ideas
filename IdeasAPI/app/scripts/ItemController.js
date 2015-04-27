(function (app) {
	var itemController = function ($scope, $routeParams, itemService, alertingService, $location, userService) {
		var id = $routeParams.id;


		$scope.loading = true;

		var updateItem = function () {
			itemService
				.getItem(id)
				.success(function (result) {
					$scope.loading = false;
					$scope.item = result;
					userService.getUser()
					.then(function(user) {
						//$scope.isModerator = user.isModerator;
						$scope.userName = user.name;
						$scope.canDelete = result.author == user.name;
						$scope.canApprove = user.isModerator && result.status == "Awaiting analysis";
					});
					
					updateComments();
				});
		}


		var updateComments = function () {
			itemService
				.getComments(id)
				.success(function (result) {
					$scope.comments = result;
				});
		}

		$scope.upvote = function () {
			vote(true);
		}


		$scope.downvote = function () {
			vote(false);
		}


		$scope.deleteItem = function () {
			itemService
			.approveItem($scope.item.id)
			.success(function () {
				$location.path('/home');
			}).error(function () {
				alertingService.addDanger(true);

			});
		}

		$scope.approveItem = function () {
			itemService
			.aproveItem($scope.item.id)
			.success(function () {
				alertingService.addSuccess("You approved this item!");
				updateItem();
			}).error(function () {
				alertingService.addDanger(true);

			});
		}

		$scope.addComment = function () {


			itemService
				.saveComment($scope.item.id, {message: $scope.comment})
				.success(function () {
					updateItem();
					$scope.comment = null;
					alertingService.addSuccess(true);
			}).error(function() {
				alertingService.addDanger(true);

			});
		}

		function vote(value) {
			if ($scope.item.voteResult == undefined) {
				itemService
					.vote($scope.item.id, value)
					.success(function() {
						updateItem();
					});
			}
		}

		updateItem();
		

	};

	app.controller("itemController", ["$scope", "$routeParams", "itemService", "alertingService", "$location", "userService", itemController]);
}(angular.module("Ideas")));