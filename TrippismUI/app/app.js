var TrippismUIApp = angular.module('TrippismUIApp',
                        [
                            //'ngRoute',
                            'ui.router',
                          'ui.bootstrap',
                          'ui.map',
                          'ui.event',
                          'cgBusy',
                          //'ngAside' ,
                          //'google-maps',
                          
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

        .state('destination1', {
            url: '/destination1',
            templateUrl: '/app/Views/destination1.html',
            views: {
                "": {
                    templateUrl: '/app/Views/destination1.html',
                }

            }
        })
        .state('destinationtesting', {
            url: '/destinationtesting',
            templateUrl: '/app/Views/DestinationTesting.html'
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




TrippismUIApp.filter('limit', function () {
    return function (input, value) {
        return input.substr(0, value);
    }
});

TrippismUIApp.directive('tabs', function () {
    return {
        restrict: 'A',
        transclude: true,
        scope: {},
        controller: function ($scope, $element) {
            var panes = $scope.panes = [];

            $scope.select = function (pane) {
                angular.forEach(panes, function (pane) {
                    pane.selected = false;
                });
                pane.selected = true;
            };

            this.addPane = function (pane) {
                if (panes.length === 0) $scope.select(pane);
                panes.push(pane);
            };
        },
        template:
            '<div class="tabbable">' +
                '<ul class="nav nav-tabs my-tab">' +
                '<li ng-repeat="pane in panes" ng-class="{active:pane.$parent.tabInfo.selected}">' +
        '<a href="" ng-click="pane.$parent.tabManager.select($index)">{{pane.title}}</a><i class="fa fa-times-circle close my-close"></i>' +
                '</li>' +
                '</ul>' +
                '<div class="tab-content padtop5" ng-transclude></div>' +
                '</div>',
        replace: true
    };
});
TrippismUIApp.directive('pane', function () {
    return {
        require: '^tabs',
        restrict: 'A',
        transclude: true,
        scope: { title: '@' },
        link: function (scope, element, attrs, tabsCtrl) {
            tabsCtrl.addPane(scope);
        },
        template:
            '<div class="tab-pane" ng-class="{active: $parent.tabInfo.selected}" ng-transclude>' +
                '</div>',
        replace: true
    };
});



//TrippismUIApp.config(function (blockUIConfig) {
//    blockUIConfig.autoInjectBodyBlock = false;
//    blockUIConfig.delay = 10;
//});




