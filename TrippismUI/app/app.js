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
        .state('default', {
            url: '/default',
            templateUrl: '/app/Views/default.html'
        })
});

