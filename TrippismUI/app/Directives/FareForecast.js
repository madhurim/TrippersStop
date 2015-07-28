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
            
            scope.ShowMoreDestinationsInfo = ShowMoreDestinationsInfo;

            function ShowMoreDestinationsInfo(seasonalitydirectiveData) {
                $rootScope.$broadcast('CreateTabForDestination');
                closePanel();
            }

            function closePanel() {
                scope.$parent.fareforecastdirectiveDisplay = false;
                scope.isOpens = false;
            }

            function activate() {
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
                      scope.mapDetails = scope.seasonalityData.mapOptions;
                      scope.googleattractionData = {
                          airport_Lat: scope.seasonalityData.DestinationairportName.airport_Lat,
                          airport_Lng: scope.seasonalityData.DestinationairportName.airport_Lng
                      }
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
                scope.isOpens = !scope.isOpens;
                if (scope.openaccordiondata == false) {
                    scope.openaccordiondata = true;
                }
            };

            // Events
            //elem.bind('click', function () {
            //    elem.css('background-color', 'white');
            //    scope.$apply(function () {
            //        scope.color = "white";
            //    });
            //});
            //elem.bind('mouseover', function () {
            //    elem.css('cursor', 'pointer');
            //});
        }
    }
}]);
