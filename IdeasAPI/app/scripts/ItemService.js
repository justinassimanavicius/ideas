(function (app) {
	var itemService = function ($http ) {
		
		var getItems = function(){
			return $http.get('api/entry');
		}
		
		var getItem = function(id){
			return $http.get('api/entry/' + id);
		}

		var saveItem = function (item) {
			return $http.post('api/entry', item);
		}

		var vote = function (itemId, voteValue) {
			return $http.post('api/entry/' + itemId + '/vote', { isPositive: voteValue });
		}
		

		var getComments = function(itemId) {
			return $http.get('api/entry/' + itemId + '/comment');
		}

		var deleteItem = function(itemId) {
			return $http.delete('api/entry/' + itemId);
		}

		var approveItem = function (itemId) {
			return $http.post('api/entry/' + itemId+"/approve");
		}

		var saveComment = function (itemId, comment) {
			return $http.post('api/entry/' + itemId + '/comment', comment);
		}

		return {
			getItems: getItems,
			getItem: getItem,
			saveItem: saveItem,
			vote: vote,
			getComments: getComments,
			saveComment: saveComment,
			deleteItem: deleteItem,
			approveItem: approveItem
	}
	};

	app.service("itemService", ["$http", "$q", itemService]);
}(angular.module("Ideas")));