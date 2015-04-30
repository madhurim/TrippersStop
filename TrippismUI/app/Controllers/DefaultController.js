
(function () {
    'use strict';
    var controllerId = 'DefaultController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', DefaultController]);

    function DefaultController($scope) {
        $scope.Statement = 'Hi, Guest!';
    }
})();


