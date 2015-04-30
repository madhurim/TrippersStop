


(function () {
    'use strict';
    var controllerId = 'LandingPageController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', LandingPageController]);

    function LandingPageController($scope) {
        $scope.LandingStatement = 'Hi, Guest!';
    }
})();


