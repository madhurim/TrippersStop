(function () {
    'use strict';
    var controllerId = 'FareRangeController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', '$modal', 'FareRangeFactory', 'UtilFactory', FareRangeController]);

    function FareRangeController($scope, $modal, FareRangeFactory, UtilFactory) {

        $scope.hasError = false;
        $scope.activate = activate;
        $scope.Origin = '';
        $scope.Destination = '';
        $scope.IsSearched = false;
        $scope.AvailableAirports = [];   
        $scope.findfarerRange = findfarerRange;
        $scope.formats = Dateformat();
        $scope.format = $scope.formats[0];
        $scope.LoadingText = "Loading..";
        $scope.SearchbuttonText = "Get Fare Range";
        $scope.SearchbuttonIsLoading = false;
        
        var dt = new Date();
        dt.setHours(0, 0, 0, 0)
        var Todt = new Date();
        Todt.setDate(Todt.getDate() + 5); // add default from 5 days
        Todt.setHours(0, 0, 0, 0)

        $scope.LatestDepartureDate = ConvertToRequiredDate(Todt);
        $scope.EarliestDepartureDate = ConvertToRequiredDate(dt);

        $scope.minTodayDate = new Date();
        $scope.minEarliestDepartureDate = new Date();
        $scope.minEarliestDepartureDate = $scope.minEarliestDepartureDate.setDate($scope.minEarliestDepartureDate.getDate() + 1);

        $scope.MaximumEarliestDepartureDate = ConvertToRequiredDate(common.addDays(new Date(), 90));
      
        $scope.$watch(function (scope) { return scope.EarliestDepartureDate },
              function (newValue, oldValue) {

                  if (newValue == null)
                      return;

                  /* If from date is greater than to date */
                  var newDt = new Date(newValue);
                  newDt.setHours(0, 0, 0, 0);
                  var todate = new Date($scope.LatestDepartureDate);
                  todate.setHours(0, 0, 0, 0);

                  if (newDt >= todate) {
                      $scope.LatestDepartureDate = ConvertToRequiredDate(newDt.setDate(newDt.getDate() + 1))
                  }
                  /**/

                  //SET MINIMUN SELECTED DATE for TODATE
                  $scope.minEarliestDepartureDate = new Date(newValue);
                  $scope.minEarliestDepartureDate = $scope.minEarliestDepartureDate.setDate($scope.minEarliestDepartureDate.getDate() + 1);
                  $scope.MaximumEarliestDepartureDate = common.addDays(new Date(), 90);
             
              }
       );

        $scope.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            if ($scope.openedEarliestDepartureDate)
            {
                $scope.openedEarliestDepartureDate = false;
            }
            $scope.opened = true;
        };

        $scope.openEarliestDepartureDate = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            if ($scope.opened) {
                $scope.opened = false;
            }
            $scope.openedEarliestDepartureDate = true;
        };

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        function activate() {
            UtilFactory.ReadAirportJson().then(function (data) {
                $scope.AvailableAirports = data;
                $scope.AvailableCodes = angular.copy($scope.AvailableAirports);
                UtilFactory.getIpinfo($scope.AvailableAirports).then(function (data) {
                    $scope.Origin = data.airport_Code;
                    $scope.findfarerRange();
                });             
            });
        }

        activate();

        $scope.onSelect = function ($item, $model, $label) {
            $scope.Origin = $item.airport_Code;
        };

        $scope.onDestinationSelect = function ($item, $model, $label) {
            $scope.Destination = $item.airport_Code;
        }

        $scope.formatInput = function ($model) {
            if ($model == "" || $model == undefined) return "";

            var originairport = _.find($scope.AvailableAirports, function (airport) { return airport.airport_Code == $model });
            var airportname = (originairport.airport_FullName.toLowerCase().indexOf("airport") > 0) ? originairport.airport_FullName : originairport.airport_FullName + " Airport";
            var CountryName = (originairport.airport_CountryName != undefined) ? originairport.airport_CountryName : "";
            return originairport.airport_Code + ", " + airportname + ", " + originairport.airport_CityName + ", " + CountryName;
        }

        $scope.isUndefined = function (thing) {
            return (typeof thing === "undefined");
        }

        function findfarerRange() {

            if ($scope.frmrangefinder.$invalid) {
                $scope.hasError = true;
                return;
            }
            
            $scope.SearchedfarerangeInfo = undefined;
            $scope.IsSearched = true;
            $scope.SearchbuttonIsLoading = true; $scope.SearchbuttonText = $scope.LoadingText;
            var data = {
                "Origin": $scope.Origin,
                "EarliestDepartureDate": ConvertToRequiredDate($scope.EarliestDepartureDate),
                "LatestDepartureDate": ConvertToRequiredDate($scope.LatestDepartureDate),
                "Destination": $scope.Destination,
                "Lengthofstay": $scope.LenghtOfStay,
            };
            FareRangeFactory.fareRange(data).then(function (data) {
                $scope.SearchbuttonText = "Get Fare Range";
                $scope.SearchbuttonIsLoading = false;
                if (data != null || data != undefined) {      
                    $scope.SearchedfarerangeInfo = data;                  
                }
            });

        }
    }

})();