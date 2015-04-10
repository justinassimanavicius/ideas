(function (app) {
	var itemService = function ($http ) {
		
		getItems = function(){
			return $http.get('/mock/items.js');			
		}
		
		return {
			getItems : getItems
		}
	};

	app.service("itemService", ["$http", "$q", itemService]);
}(angular.module("Ideas")));