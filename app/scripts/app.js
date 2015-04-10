(function () {
	var app = angular.module("Ideas", ["ngRoute", "ngAnimate", 'angular-loading-bar', "ngDialog"])
        
        .config(['cfpLoadingBarProvider', function(cfpLoadingBarProvider) {
            cfpLoadingBarProvider.includeBar = true;
            cfpLoadingBarProvider.includeSpinner = false;
         }])
   
}());