


(function () {
    'use strict';
    var controllerId = 'LandingPageController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', '$location', LandingPageController]);

    function LandingPageController($scope, $location) {
        
        $scope.getClass = function (path) {
            if ($location.path().substr(0, path.length) == path) {
                return "active"
            } else {
                return ""
            }
        }

    }
})();


