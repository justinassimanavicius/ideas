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
		
		return {
			getItems : getItems,
			getItem: getItem,
			saveItem: saveItem
		}
	};

	app.service("itemService", ["$http", "$q", itemService]);
}(angular.module("Ideas")));