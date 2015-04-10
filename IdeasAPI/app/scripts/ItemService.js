(function (app) {
	var itemService = function ($http ) {
		
		getItems = function(){
			return $http.get('app/mock/items.js');			
		}
		
		getItem = function(id){
			return $http.get('app/mock/item.js');
		}
		
		return {
			getItems : getItems,
			getItem : getItem
		}
	};

	app.service("itemService", ["$http", "$q", itemService]);
}(angular.module("Ideas")));