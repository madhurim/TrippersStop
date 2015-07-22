angular.module('TrippismUIApp').directive('farerangeInfo', ['$compile', 'FareRangeFactory', function ($compile, FareRangeFactory) {
    return {
        restrict: 'E',
        scope: {
            farerangeParams: '=', isOpen: '=',
            mailfarerangeData : '='
        },
        templateUrl: '/app/Views/Partials/FareRangePartial.html',
        link: function (scope, elem, attrs) {
            scope.$watchGroup(['farerangeParams', 'isOpen'], function (newValue, oldValue, scope) {
                if (scope.isOpen == true) {
                    if (newValue != oldValue)
                        scope.loadfareRangeInfo();
                } else {
                    scope.fareRangeData = "";
                    scope.mailfarerangeData = "";
                }
            });
            // scope.$watch('farerangeParams',
            //  function (newValue, oldValue) {
            //      if (newValue != oldValue)
            //          scope.loadfareRangeInfo();
            //  }
            //);

            scope.loadfareRangeInfo = function () {
                scope.fareRangeInfoLoaded = false;
                scope.fareRangeInfoNoDataFound = false;
                scope.fareRangeData = "";
                scope.mailfarerangeData = "";
                if (scope.farerangeParams != undefined) {
                    var data = {
                        "Origin": scope.farerangeParams.Fareforecastdata.Origin,
                        "Destination": scope.farerangeParams.Fareforecastdata.Destination,
                        "EarliestDepartureDate": scope.farerangeParams.Fareforecastdata.DepartureDate,
                        "LatestDepartureDate": scope.farerangeParams.Fareforecastdata.ReturnDate,
                        "Lengthofstay": 4
                    };
                    if (scope.fareRangeInfoLoaded == false) {
                        if (scope.fareRangeData == "") {
                            scope.farerangepromise = FareRangeFactory.fareRange(data).then(function (data) {
                                scope.FareRangeLoading = false;
                                if (data.status == 404)
                                    scope.fareRangeInfoNoDataFound = true;
                                scope.fareRangeData = data;
                                scope.mailfarerangeData = data;
                            });
                        }
                    }
                    scope.fareRangeInfoLoaded = true;
                }
            };
        }
    }
}]);
