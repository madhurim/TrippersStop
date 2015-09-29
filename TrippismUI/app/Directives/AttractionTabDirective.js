angular.module('TrippismUIApp').directive('attractionTab', ['$compile', function ($compile) {
    return {
        restrict: 'E',
        scope: {
            attractionParams: '=',
            homeFn:'&'
        },
        templateUrl: '/app/Views/Partials/AttractionTabPartial.html',
        link: function (scope, elem, attrs) {
            scope.attractionParams.WeatherInfoNoDataFound = false;
        }
    }
}]);
