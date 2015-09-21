

var TrippismUIApp = angular.module('TrippismUIApp',
                        [
                            
                          'ui.router',
                          'ui.bootstrap',
                          'ui.map',
                          'ui.event',
                          'cgBusy',
                        
                        ]);

TrippismUIApp.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.when('', '/destination').otherwise('/destination');


    $stateProvider
        // HOME STATES AND NESTED VIEWS ========================================
         .state('destination', {
             url: '/destination',
             templateUrl: '/app/Views/destination1.html'
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
                '<div class="tab-content padtop5" style="margin-top:15px;" ng-transclude></div>' +
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


var constants = {
    googlePlacesApiKey: "AIzaSyC0CVNlXkejEzLzGCMVMj8PZ7gBzj8ewuQ",
    DefaultLenghtOfStay: 4,
    YouTubeEmbedUrl: "//www.youtube.com/embed/",
    HighChartDateFormat: '%m-%e-%Y',
    HighChartTwoDecimalCurrencyFormat: '{point.y:.2f}',


    attractionTabMapStyle: [{ "featureType": "landscape", "stylers": [{ "hue": "#FFBB00" }, { "saturation": 43.400000000000006 }, { "lightness": 37.599999999999994 }, { "gamma": 1 }] }, { "featureType": "road.highway", "stylers": [{ "hue": "#FFC200" }, { "saturation": -61.8 }, { "lightness": 45.599999999999994 }, { "gamma": 1 }] }, { "featureType": "road.arterial", "stylers": [{ "hue": "#FF0300" }, { "saturation": -100 }, { "lightness": 51.19999999999999 }, { "gamma": 1 }] }, { "featureType": "road.local", "stylers": [{ "hue": "#FF0300" }, { "saturation": -100 }, { "lightness": 52 }, { "gamma": 1 }] }, { "featureType": "water", "stylers": [{ "hue": "#0078FF" }, { "saturation": -13.200000000000003 }, { "lightness": 2.4000000000000057 }, { "gamma": 1 }] }, { "featureType": "poi", "stylers": [{ "hue": "#00FF6A" }, { "saturation": -1.0989010989011234 }, { "lightness": 11.200000000000017 }, { "gamma": 1 }] }],
    destinationSearchMapSyle: [{ "featureType": "landscape", "stylers": [{ "hue": "#FFBB00" }, { "saturation": 43.400000000000006 }, { "lightness": 37.599999999999994 }, { "gamma": 1 }] }, { "featureType": "road.highway", "stylers": [{ "hue": "#FFC200" }, { "saturation": -61.8 }, { "lightness": 45.599999999999994 }, { "gamma": 1 }] }, { "featureType": "road.arterial", "stylers": [{ "hue": "#FF0300" }, { "saturation": -100 }, { "lightness": 51.19999999999999 }, { "gamma": 1 }] }, { "featureType": "road.local", "stylers": [{ "hue": "#FF0300" }, { "saturation": -100 }, { "lightness": 52 }, { "gamma": 1 }] }, { "featureType": "water", "stylers": [{ "hue": "#0078FF" }, { "saturation": -13.200000000000003 }, { "lightness": 2.4000000000000057 }, { "gamma": 1 }] }, { "featureType": "poi", "stylers": [{ "hue": "#00FF6A" }, { "saturation": -1.0989010989011234 }, { "lightness": 11.200000000000017 }, { "gamma": 1 }] }]
};

TrippismUIApp.constant('TrippismConstants', constants);