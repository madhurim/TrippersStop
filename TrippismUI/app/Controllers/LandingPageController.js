


(function () {
    'use strict';
    var controllerId = 'LandingPageController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', '$location','$anchorScroll', LandingPageController]);

    function LandingPageController($scope, $location, $anchorScroll) {
        
        $scope.getClass = function (path) {
            if ($location.path().substr(0, path.length) == path) {
                return "active"
            } else {
                return ""
            }
        }

        $scope.GoToTop = function () {
            $location.hash('top');
            $anchorScroll();
        }
    }
})();


