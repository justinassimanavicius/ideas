(function () {
	angular.module("Ideas", ["ngRoute", "ngAnimate", 'angular-loading-bar', 'mgcrea.ngStrap', 'mgcrea.ngStrap.collapse'])
        .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
            $locationProvider.html5Mode(true).hashPrefix('!');
            $routeProvider
            .when("/", {
            	templateUrl: "app/templates/home.html",
            	controller: "homeController"
            })
            .when("/item/:id", {
            	templateUrl: "app/templates/item.html",
                controller: "itemController"
            })
            .otherwise({ redirectTo: "/" });

        }])
        .config(['cfpLoadingBarProvider', function(cfpLoadingBarProvider) {
            cfpLoadingBarProvider.includeBar = true;
            cfpLoadingBarProvider.includeSpinner = false;
        }]);
   
}());