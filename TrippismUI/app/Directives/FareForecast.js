angular.module('TrippismUIApp').directive('fareForecast', ['$compile', 'FareforecastFactory', '$rootScope', '$modal',
    function ($compile, FareforecastFactory, $rootScope,$modal) {
    return {
        restrict: 'E',

        scope: {
            destinationData: '=',
            seasonalityData: '='
        },

        templateUrl: '/app/Views/Partials/FareForecastPartial.html',
        link: function (scope, elem, attrs) {
            scope.IsRequestCompleted = false;
            scope.activate = activate;
            scope.closePanel = closePanel;
            scope.visibleDiv = false;
            scope.ShowMoreDestinationsInfo = ShowMoreDestinationsInfo;

            scope.$watch('FareNoDataFound',
              function (newValue) {
                  scope.FareNoDataFound = angular.copy(newValue);
              }
            );

            function ShowMoreDestinationsInfo(seasonalitydirectiveData) {
                scope.visibleDiv = false;
                $rootScope.$broadcast('CreateTabForDestination');
                closePanel();
            }

            function closePanel() {
                scope.$parent.fareforecastdirectiveDisplay = false;
                scope.isOpens = false;
                scope.visibleDiv = false;
            }

            function activate() {
                scope.visibleDiv = false;
                scope.FareNoDataFound = false;
                scope.FareforecastData = "";
                scope.IsRequestCompleted = false;
                scope.fareinfopromise = FareforecastFactory.fareforecast(scope.destinationData).then(function (data) {

                    scope.IsRequestCompleted = true;
                    scope.inProgressFareinfo = false;
                    if (data.status == 404) {
                        scope.FareNoDataFound = true;
                        return;
                    }
                    scope.FareforecastData = data;
                });
            };


            scope.$watch('destinationData',
              function (newValue, oldValue) {
                  if (newValue != oldValue) {
                      activate();
                      scope.isOpens = false;
                  }
              }
            );

            scope.$watch('seasonalityData',
              function (newValue, oldValue) {
                  if (newValue != oldValue) {
                     // scope.visibleDiv = true;
                      scope.mapDetails = scope.seasonalityData.mapOptions;
                      scope.googleattractionData = {
                          airport_Lat: scope.seasonalityData.DestinationairportName.airport_Lat,
                          airport_Lng: scope.seasonalityData.DestinationairportName.airport_Lng
                      }
                      scope.weatherData = undefined;
                      scope.isOpens = false;
                      scope.openaccordiondata = false;
                  }
              }
            );

            scope.SendEmailToUser = function (destinationdet) {
                var GetEmailDetPopupInstance = $modal.open({
                    templateUrl: 'EmailDetForm.html',
                    controller: 'EmailForDestinationDet',
                    scope: scope,
                    resolve: {
                        seasonalityData: function () {
                            return scope.seasonalityData;
                        }
                    }
                });
            }

            scope.openaccordion = function () {
                scope.openaccordiondata = true;

                //scope.isOpens = !scope.isOpens;
                //if (scope.openaccordiondata == false) {
                //    scope.openaccordiondata = true;

                //}
            };

            scope.openaccordion();

            //scope.visibleDiv = true;
            

            //scope.FareRangeData = true;
            //scope.SeasonalityData = true;

            scope.FareRangeDivDisplay = true;
            scope.SeasonalityDivDisplay = true;
            scope.WeatherDivDisplay = true;
            
            scope.$on('divFareRangeEvent', function (event, args) {
                scope.FareRangeDivDisplay = args;
                if (scope.FareRangeDivDisplay == true || scope.SeasonalityDivDisplay == true || scope.WeatherDivDisplay == true)
                    scope.visibleDiv = true;
                else
                    scope.visibleDiv = false;
            });
            scope.$on('divSeasonalityEvent', function (event, args) {
                scope.SeasonalityDivDisplay = args;
                if (scope.FareRangeDivDisplay == true || scope.SeasonalityDivDisplay == true || scope.WeatherDivDisplay == true)
                    scope.visibleDiv = true;
                else 
                    scope.visibleDiv = false;
            });

            scope.$on('divWeatherEvent', function (event, args) {
                scope.WeatherDivDisplay = args;
                if (scope.FareRangeDivDisplay == true || scope.SeasonalityDivDisplay == true || scope.WeatherDivDisplay == true)
                    scope.visibleDiv = true;
                else
                    scope.visibleDiv = false;
            });

        }
    }
}]);
