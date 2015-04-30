var TrippismUIApp = angular.module('TrippismUIApp',
                        [
                          //'ngRoute',
                          'ui.router',
                          'ui.bootstrap'

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

//var configFunction = function ($routeProvider) {
//    $routeProvider.
//        when('/page1', {
//            templateUrl: 'Scripts/app/Views/page1.html'
//        })
//        .when('/page2', {
//            templateUrl: 'Scripts/app/Views/page2.html'
//        })
//        .when('/404', {
//            templateUrl: 'Scripts/app/Views/404.html'
//        })
//        .otherwise({
//            templateUrl: 'Scripts/app/Views/default.html'
//        })
//}
//configFunction.$inject = ['$routeProvider'];

//TrippismUIApp.config(configFunction);