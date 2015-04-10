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
		
		return {
			getItems : getItems,
			getItem: getItem,
			saveItem: saveItem
		}
	};

	app.service("itemService", ["$http", "$q", itemMockService]);
}(angular.module("Ideas")));