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
TrippismUIApp.directive('infowindow', function () {
    return {
        restrict: 'E',
        templateUrl: '/app/Views/Partials/infowindowpartial.html',
        scope: {
            markerData: '='
        },
        link: function (scope) {
        }
    }
});

TrippismUIApp.directive('onCarouselChange', function ($parse) {
    return {
        require: 'carousel',
        link: function (scope, element, attrs, carouselCtrl) {
            var fn = $parse(attrs.onCarouselChange);
            var origSelect = carouselCtrl.select;
            carouselCtrl.select = function (nextSlide, direction) {
                if (nextSlide !== this.currentSlide) {
                    fn(scope, {
                        nextSlide: nextSlide,
                        direction: direction,
                    });
                }
                return origSelect.apply(this, arguments);
            };
        }
    }});

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
        '<a href="" ng-click="pane.$parent.tabManager.select($index)">{{pane.title}}</a><i ng-click="pane.$parent.tabManager.removeTab($index);panes.splice($index, 1);" class="fa fa-times-circle close my-close"></i>' +
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