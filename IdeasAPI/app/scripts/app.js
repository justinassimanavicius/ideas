(function () {
	angular.module("Ideas", ["ngRoute", "ngAnimate", "ui.bootstrap"])
		.config([
			'$routeProvider', '$locationProvider', function($routeProvider, $locationProvider) {
				$locationProvider.html5Mode(true).hashPrefix('!');
				$routeProvider
					.when("/", {
						templateUrl: "app/templates/home.html",
						controller: "homeController"
					}).when("/create", {
						templateUrl: "app/templates/createItem.html",
						controller: "createItemController"
					})
					.when("/item/:id", {
						templateUrl: "app/templates/item.html",
						controller: "itemController"
					})
					.when('/logout', { redirectTo: redirect })
					.otherwise({ redirectTo: "/" });

			}
		]);

	function redirect(skip, url) {
		window.location.href = "/"+url;
	};
   
}());