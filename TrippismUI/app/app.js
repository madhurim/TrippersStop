

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
                '<a ng-click="pane.$parent.tabManager.select($index)">{{pane.title}}&nbsp;<span ng-click="pane.$parent.tabManager.removeTab($index);panes.splice($index, 1);" class="glyphicon glyphicon-remove"></span></a>' +

                '</li>' +
                '</ul>' +
                '<div class="tab-content padtop5" style="margin-top:15px;" ng-transclude></div>' +
                '</div>',

        //template:
        //    '<div class="tabbable">' +
        //        '<ul class="nav nav-tabs my-tab">' +
        //        '<li ng-repeat="pane in panes" ng-class="{active:pane.$parent.tabInfo.selected}">' +
        //        '<a ng-click="pane.$parent.tabManager.select($index)">{{pane.title}}&nbsp;<span ng-click="pane.$parent.tabManager.removeTab($index);panes.splice($index, 1);" class="glyphicon glyphicon-remove"></span></a>' +
        
        //        '</li>' +
        //        '</ul>' +
        //        '<div class="tab-content padtop5" style="margin-top:15px;" ng-transclude></div>' +
        //        '</div>',
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

    aboutUsText : "<p class=p-text-align>Trippism is a unique and innovative solution to the problem of destination discovery in leisure travel. Around the world data and statistics show that leisure travelers want to go somewhere , however don't know where. Most online travel businesses offer good prices and booking mechanisms , however no means to figure out where you can go. Here is where Trippism comes. Our state of the art workflow suggests you destinations given your origin , dates , budget and preferences like themes , regions etc.. "+
    "A new tab opens when you pick a destination giving an overview of that destination which includes key decision making information like lowest current fares, fare intelligence , weather history , traffic seasonality , attractions , attraction related videos. It's all about the art of destination discovery , making it easier by bringing together relevant information based on historical data and user generated content across the web. </p>"+
            " <p class=p-text-align>We are not a travel booking engine nor do we intend to give you the best deal for that travel search. We provide the current lowest published fares by airlines combined with fare intelligence that enables you to understand the fare trends and see if the current fares are in your budget. Our success lies in you being able to discover that destination based on your preferences and our information.</p> " +
            " <p class=p-text-align>Team Trippism is made of dedicated , smart and passionate professionals who are striving everyday to put the best solution out there. Our business know how and technical strengths combined , enable us to bring this innovative travel offering.</p>" +
            " <p class=p-text-align>This is just a soft beta launch and there is much more to come , which will help you find your vacation destination of choice.</p>",

    attractionTabMapStyle: [{ "featureType": "landscape", "stylers": [{ "hue": "#FFBB00" }, { "saturation": 43.400000000000006 }, { "lightness": 37.599999999999994 }, { "gamma": 1 }] }, { "featureType": "road.highway", "stylers": [{ "hue": "#FFC200" }, { "saturation": -61.8 }, { "lightness": 45.599999999999994 }, { "gamma": 1 }] }, { "featureType": "road.arterial", "stylers": [{ "hue": "#FF0300" }, { "saturation": -100 }, { "lightness": 51.19999999999999 }, { "gamma": 1 }] }, { "featureType": "road.local", "stylers": [{ "hue": "#FF0300" }, { "saturation": -100 }, { "lightness": 52 }, { "gamma": 1 }] }, { "featureType": "water", "stylers": [{ "hue": "#0078FF" }, { "saturation": -13.200000000000003 }, { "lightness": 2.4000000000000057 }, { "gamma": 1 }] }, { "featureType": "poi", "stylers": [{ "hue": "#00FF6A" }, { "saturation": -1.0989010989011234 }, { "lightness": 11.200000000000017 }, { "gamma": 1 }] }],
    destinationSearchMapSyle: [{ "featureType": "landscape", "stylers": [{ "hue": "#FFBB00" }, { "saturation": 43.400000000000006 }, { "lightness": 37.599999999999994 }, { "gamma": 1 }] }, { "featureType": "road.highway", "stylers": [{ "hue": "#FFC200" }, { "saturation": -61.8 }, { "lightness": 45.599999999999994 }, { "gamma": 1 }] }, { "featureType": "road.arterial", "stylers": [{ "hue": "#FF0300" }, { "saturation": -100 }, { "lightness": 51.19999999999999 }, { "gamma": 1 }] }, { "featureType": "road.local", "stylers": [{ "hue": "#FF0300" }, { "saturation": -100 }, { "lightness": 52 }, { "gamma": 1 }] }, { "featureType": "water", "stylers": [{ "hue": "#0078FF" }, { "saturation": -13.200000000000003 }, { "lightness": 2.4000000000000057 }, { "gamma": 1 }] }, { "featureType": "poi", "stylers": [{ "hue": "#00FF6A" }, { "saturation": -1.0989010989011234 }, { "lightness": 11.200000000000017 }, { "gamma": 1 }] }]
};

TrippismUIApp.constant('TrippismConstants', constants);