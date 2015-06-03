


(function () {
    'use strict';
    var controllerId = 'LandingPageController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', '$rootScope', '$location', '$anchorScroll', LandingPageController]);
   
    function LandingPageController($scope, $rootScope, $location, $anchorScroll) {

        if (angular.lowercase($location.host()) == "localhost")
        {
            //devlopment url
            $rootScope.apiURL = 'http://localhost:14606/sabre/api/';
        }
        else
        {
            //live url
            $rootScope.apiURL = 'http://' + $location.host() + '/sabre/api/';
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


