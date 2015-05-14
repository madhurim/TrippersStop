var TrippismUIApp = angular.module('TrippismUIApp',
                        [
                          //'ngRoute',
                          'ui.router',
                          'ui.bootstrap',
                          'blockUI',
                          'ui.map',
                          'ui.event'
                        ]);

TrippismUIApp.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/destination');
    $stateProvider
        // HOME STATES AND NESTED VIEWS ========================================
         .state('destination', {
             url: '/destination',
             templateUrl: '/app/Views/destination.html'
         })
        .state('fareforecast', {
            url: '/fareforecast',
            templateUrl: '/app/Views/fareforecast.html'
        })
     .state('/', {
         url: '/fareforecast',
         templateUrl: '/app/Views/fareforecast.html'
     })

  
        
});

TrippismUIApp.config(function (blockUIConfig) {
    // Change the default overlay message
    //blockUIConfig.message = 'Please stop clicking!';
    // Change the default delay to 100ms before the blocking is visible
    //blockUIConfig.delay = 100;
});



