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
        
});

//TrippismUIApp.directive("otcDynamic", function ($compile) {
//    return {
//        link: function (scope, element) {
//            var template = "<button ng-click='doSomething()'>{{label}}</button>";
//            var linkFn = $compile(template);
//            var content = linkFn(scope);
//            element.append(content);
//        }
//    }
//});
