


(function () {
    'use strict';
    var controllerId = 'LandingPageController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', '$rootScope', '$location', '$anchorScroll', LandingPageController]);

    function LandingPageController($scope, $rootScope, $location, $anchorScroll) {
        if (angular.lowercase($location.host()) == "localhost") {
            //devlopment url
            $rootScope.apiURL = 'http://localhost:14606/api/Sabre/';
            $rootScope.apiURLForEmail = 'http://localhost:14606/api/Email/SendEmailtoUser';
            $rootScope.apiURLForGoogleAttraction = 'http://localhost:14606/api/googleplace/';
            $rootScope.apiURLForYouTube = 'http://localhost:14606/api/youtube/';
            $rootScope.apiURLForWeather = 'http://localhost:14606/api/weather/international/history';
            $rootScope.apiURLForUSWeather = 'http://localhost:14606/api/weather/history';
            $rootScope.apiURLForGoogleGeoReverseLookup = 'http://localhost:14606/api/googlegeocode/reverselookup/';
            $rootScope.apiURLForFeedback = 'http://localhost:14606/api/Email/SendFeedback';
        }
        else {
            //live url
            $rootScope.apiURL = 'http://' + $location.host() + '/api/';
            $rootScope.apiURLForEmail = 'http://' + $location.host() + '/api/Email/SendEmailtoUser';
            $rootScope.apiURLForWeather = 'http://' + $location.host() + '/api/weather/international/history';
            $rootScope.apiURLForGoogleAttraction = 'http://' + $location.host() + '/api/googleplace/';
            $rootScope.apiURLForYouTube = 'http://' + $location.host() + '/api/youtube/';
            $rootScope.apiURLForUSWeather = 'http://' + $location.host() + '/api/weather/history';
            $rootScope.apiURLForGoogleGeoReverseLookup = 'http://' + $location.host() + '/api/googlegeocode/reverselookup/';
            $rootScope.apiURLForFeedback = 'http://' + $location.host() + '/api/Email/SendFeedback';
        }
        $scope.getClass = function (path) {
            if ($location.path().substr(0, path.length) == path) {
                return "active"
            } else {
                return ""
            }
        }
        $scope.GoToTop = function () {
            var old = $location.hash();
            $location.hash('top');
            $anchorScroll();
            $location.hash(old);
        }
    }
})();


