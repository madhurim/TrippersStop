var TrippismUIApp = angular.module('TrippismUIApp',
                        [
                          //'ngRoute',
                          'ui.router',
                          'ui.bootstrap',
                          'blockUI',
                          'ui.map',
                          'ui.event',
                          
                        ]);


TrippismUIApp.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.when('', '/destination').otherwise('/destination');

  
    $stateProvider
        // HOME STATES AND NESTED VIEWS ========================================
         .state('destination', {
             url: '/destination',
             templateUrl: '/app/Views/destination.html',
             views: {
                 "": {
                     templateUrl: '/app/Views/destination.html',
                 }
                 
             }
         })
        .state('fareforecast', {
            url: '/fareforecast',
            templateUrl: '/app/Views/fareforecast.html'
        })
         .state('farerange', {
             url: '/farerange',
             templateUrl: '/app/Views/farerange.html'
         })
         .state('seasonality', {
             url: '/seasonality',
             templateUrl: '/app/Views/seasonality.html'
         })         
     .state('/', {
         url: '/destination',
         templateUrl: '/app/Views/destination.html'
        
     })
});

TrippismUIApp.directive("scroll", function ($window) {
    return function (scope, element, attrs) {
        angular.element($window).bind("scroll", function () {
            if (this.pageYOffset >= 5) {
                scope.EnableScroll = true;
            } else {
                scope.EnableScroll = false;
            }
            scope.$apply();
        });
    };
});


//TrippismUIApp.config(function (blockUIConfig) {
//    // Change the default overlay message
//    //blockUIConfig.message = 'Please stop clicking!';
//    // Change the default delay to 100ms before the blocking is visible
//   // blockUIConfig.delay = 0;
//});




