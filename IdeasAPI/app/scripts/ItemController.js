(function (app) {
	var itemController = function ($scope, $routeParams, itemService, alertingService) {
		var id = $routeParams.id;


		$scope.loading = true;

		var updateItem = function () {
			$scope.loading = true;
			itemService
				.getItem(id)
				.success(function (result) {
					$scope.loading = false;
					$scope.item = result;
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

			if ($scope.item.voteResult !== undefined) {
				itemService
					.vote($scope.item.id, value)
					.success(function() {
						updateItem();
					});
			}
		}

		updateItem();
		

	};

	app.controller("itemController", ["$scope", "$routeParams", "itemService", "alertingService", itemController]);
}(angular.module("Ideas")));