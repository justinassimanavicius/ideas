(function (app) {
	var itemMockService = function ($http) {
		
		var getItems = function(){
			return $http.get('app/mock/items.js');
		}
		
		var getItem = function(id){
			return $http.get('app/mock/item'+id+'.js');
		}

		var saveItem = function (item) {
			var id = 1;
			item.id = id;
			return $http.get('app/mock/item' + id + '.js')
				.success(function() {
				return item;
			});
		}
		
		var noop = function() {
			
		}

		var getComments = function (itemId) {
			return $http.get('app/mock/comments.js');
		}

		var saveComment = function (itemId) {
			return $http.get('app/mock/comments.js');
		}


		return {
			getItems : getItems,
			getItem: getItem,
			saveItem: saveItem,
			vote: noop,
			getComments: getComments,
			saveComment: saveComment,
			deleteItem: noop,
			aproveItem: noop
		}
	};

	app.service("itemService", ["$http", "$q", itemMockService]);
}(angular.module("Ideas")));