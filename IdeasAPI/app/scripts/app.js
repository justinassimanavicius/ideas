(function () {
	angular.module("Ideas", ["ngRoute", "ngAnimate", 'angular-loading-bar', "ngDialog"])
        .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
            $locationProvider.html5Mode(true).hashPrefix('!');
            $routeProvider
            .when("/", {
            	templateUrl: "app/templates/home.html",
            	controller: "homeController"
            }).when("/create", {
            	templateUrl: "app/templates/create.html",
            	controller: "createController"
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
         }])
   
}());