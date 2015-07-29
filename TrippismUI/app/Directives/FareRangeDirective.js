angular.module('TrippismUIApp').directive('farerangeInfo', ['$compile', '$rootScope', 'FareRangeFactory',
    function ($compile, $rootScope,FareRangeFactory) {
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

            scope.loadingFareRange = true;
            scope.$parent.divFareRange = false;
            scope.$watch('fareRangeInfoLoaded',
              function (newValue) {
                  scope.loadingFareRange = angular.copy(!newValue);
                  scope.$parent.divFareRange = newValue;
              }
            );

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
                                if (data.status == 404) {
                                    
                                    scope.fareRangeInfoNoDataFound = true;
                                    $rootScope.$broadcast('divFareRangeEvent', false, scope.Seasonalityresult);
                                    return;
                                }
                                scope.fareRangeData = data;
                                scope.mailfarerangeData = data;
                                scope.fareRangeInfoLoaded = true;
                                $rootScope.$broadcast('divFareRangeEvent', true, scope.Seasonalityresult);
                            });
                        }
                    }
                    //scope.fareRangeInfoLoaded = true;
                }
            };
        }
    }
}]);
