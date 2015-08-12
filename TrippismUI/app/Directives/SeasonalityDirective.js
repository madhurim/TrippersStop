angular.module('TrippismUIApp').directive('seasonalityInfo', ['$compile', '$rootScope', '$timeout', 'SeasonalityFactory',
    function ($compile, $rootScope, $timeout, SeasonalityFactory) {
    return {
        restrict: 'E',

        scope: {
            seasonalityParams: '=',
            isOpen: '=',
            mailmarkereasonalityInfo: '=',
        },
        templateUrl: '/app/Views/Partials/SeasonalityPartial.html',
        link: function (scope, elem, attrs) {

            scope.Isviewmoredisplayed = false;

            scope.SeasonalityDisplay = function () {
                scope.MarkerSeasonalityInfo.Seasonality = scope.SeasonalityData;
                 scope.mailmarkereasonalityInfo.Seasonality = scope.SeasonalityData;

                // Setting up fare data for email
                //scope.attractionParams.dataforEmail.SeasonalityDataForEmail = {};
                //scope.attractionParams.dataforEmail.SeasonalityDataForEmail = data;


                scope.Isviewmoredisplayed = true;
            };

            scope.$parent.divSeasonality = false;

            scope.loadingSeasonality = true;
            scope.$watch('loadSeasonalityInfoLoaded',
              function (newValue) {
                  scope.loadingSeasonality = angular.copy(!newValue);
                  scope.$parent.divSeasonality = newValue;
              }
            );
            
            scope.loadSeasonalityInfo = function () {
                scope.MarkerSeasonalityInfo = "";
                scope.loadSeasonalityInfoLoaded = false;
                scope.SeasonalityNoDataFound = false;
                if (scope.seasonalityParams != undefined) {
                    if (scope.loadSeasonalityInfoLoaded == false) {
                        if (scope.MarkerSeasonalityInfo == "") {
                            var Seasonalitydata = {
                                "Destination": scope.seasonalityParams.Destinatrion, // JFK
                            };

                            $timeout(function () {
                                scope.inProgressSeasonalityinfo = true;
                                scope.seasonalitypromise = SeasonalityFactory.Seasonality(Seasonalitydata).then(function (data) {

                                    if (data.status == 404){
                                        scope.SeasonalityNoDataFound = true;
                                        $rootScope.$broadcast('divSeasonalityEvent', false);
                                        return;
                                    }   
                                    
                                    scope.SeasonalityData = data.Seasonality;
                                    $rootScope.$broadcast('divSeasonalityEvent', true);

                                    var defaultSeasonality = data.Seasonality;
                                    var now = new Date();
                                    var NextDate = common.addDays(now, 30);
                                    
                                    var filteredSeasonalityData = [];
                                    for (var i = 0; i < defaultSeasonality.length; i++) {
                                        scope.MarkerSeasonalityInfo.Seasonality = [];
                                        var datetocheck = new Date(defaultSeasonality[i].WeekStartDate);
                                        if (datetocheck > now && datetocheck < NextDate)
                                            filteredSeasonalityData.push(defaultSeasonality[i]);
                                    }
                                    if (filteredSeasonalityData.length == 0) {
                                        for (var i = 0; i < 5; i++) 
                                            filteredSeasonalityData.push(defaultSeasonality[i]);
                                    }
                                    data.Seasonality = filteredSeasonalityData;
                                    scope.MarkerSeasonalityInfo = data;
                                    scope.mailmarkereasonalityInfo = data;


                                    // Setting up fare data for email
                                    //scope.attractionParams.dataforEmail.SeasonalityDataForEmail = {};
                                    //scope.attractionParams.dataforEmail.SeasonalityDataForEmail = data;

                                    
                                    scope.inProgressSeasonalityinfo = false;
                                    scope.loadSeasonalityInfoLoaded = true;
                                    
                                });
                            }, 0, false);
                        }
                    }
  
                }
            };
            scope.$watchGroup(['seasonalityParams', 'isOpen'], function (newValue, oldValue, scope) {
                if (scope.isOpen == true) {
                    if (newValue != oldValue)
                        scope.loadSeasonalityInfo();
                }
                else {
                    scope.MarkerSeasonalityInfo = "";
                }
            });
            //scope.$watch('seasonalityParams',
            //  function (newValue, oldValue) {
            //      if (newValue != oldValue)
            //          scope.loadSeasonalityInfo();
            //  }
            //);
        }
    }
}]);
