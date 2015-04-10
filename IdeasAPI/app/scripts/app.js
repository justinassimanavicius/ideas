(function () {
	var app = angular.module("Ideas", ["ngRoute", "ngAnimate", 'angular-loading-bar', "ngDialog"])
        .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
            $locationProvider.html5Mode(true).hashPrefix('!');
            $routeProvider
            .when("/", {
                templateUrl: "templates/home.html",
                controller: "homeController"
            })
            .when("/item/:id", {
                templateUrl: "templates/item.html",
                controller: "itemController"
            })
            .otherwise({ redirectTo: "/" });

        }])
        .config(['cfpLoadingBarProvider', function(cfpLoadingBarProvider) {
            cfpLoadingBarProvider.includeBar = true;
            cfpLoadingBarProvider.includeSpinner = false;
         }])
   
}());