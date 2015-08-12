angular.module('TrippismUIApp').directive('attractionTab', ['$compile', function ($compile) {
    return {
        restrict: 'E',
        scope: {
            attractionParams: '=',
        },
        templateUrl: '/app/Views/Partials/attractionTabPartial.html',
        link: function (scope, elem, attrs) {
        }
    }
}]);
