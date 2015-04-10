(function (app) {
	var itemService = function ($http ) {
		
		getItems = function(){
			return $http.get('api/entry');
		}
		
		getItem = function(id){
			return $http.get('api/entry/' + id);
		}

		saveItem = function (item) {
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