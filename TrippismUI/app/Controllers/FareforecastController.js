(function () {
    'use strict';
    var controllerId = 'FareforecastController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope','$modal', 'FareforecastFactory', 'UtilFactory', FareforecastController]);

    function FareforecastController($scope, $modal, FareforecastFactory, UtilFactory) {

        $scope.hasError = false;                                
        $scope.activate = activate;
        $scope.Origin = '';
        $scope.Destination = '';
        $scope.IsSearched = false;
        $scope.AvailableAirports = [];
        $scope.forecastfareList = [];
        $scope.findfares = findfares;
        $scope.formats = Dateformat();
        $scope.format = $scope.formats[0];
        $scope.LoadingText = "Loading..";
        $scope.SearchbuttonText = "Get Fares";
        $scope.SearchbuttonIsLoading = false;      

        var dt = new Date();
        dt.setHours(0, 0, 0, 0)
        var Todt = new Date();
        Todt.setDate(Todt.getDate() + 5); // add default from 5 days
        Todt.setHours(0, 0, 0, 0)

        $scope.ToDate = ConvertToRequiredDate(Todt,'UI');
        $scope.FromDate = ConvertToRequiredDate(dt,'UI');

        $scope.minTodayDate = new Date();
        $scope.minFromDate = new Date();
        $scope.minFromDate = $scope.minFromDate.setDate($scope.minFromDate.getDate() + 1);

        $scope.MaximumFromDate = ConvertToRequiredDate(common.addDays(new Date(), 60),'UI');
      
        $scope.$watch(function (scope) { return scope.FromDate },
              function (newValue, oldValue) {

                  if (newValue == null)
                      return;

                  /* If from date is greater than to date */
                  var newDt = new Date(newValue);
                  newDt.setHours(0, 0, 0, 0);
                  var todate = new Date($scope.ToDate);
                  todate.setHours(0, 0, 0, 0);

                  if (newDt >= todate) {
                      $scope.ToDate = ConvertToRequiredDate(newDt.setDate(newDt.getDate() + 1),'UI')
                  }
                  /**/

                  //SET MINIMUN SELECTED DATE for TODATE
                  $scope.minFromDate = new Date(newValue);
                  $scope.minFromDate = $scope.minFromDate.setDate($scope.minFromDate.getDate() + 1);
                  $scope.MaximumToDate = common.addDays(new Date(), 60);

              }
       );
     
        $scope.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            if ($scope.openedFromDate) {
                $scope.openedFromDate = false;
            }
            $scope.opened = true;
        };

        $scope.openFromDate = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            if ($scope.opened) {
                $scope.opened = false;
            }
            $scope.openedFromDate = true;
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
                    $scope.findfares();
                });             
            });
        }

        activate();

        $scope.onSelect = function ($item, $model, $label) {
            $scope.Origin = $item.airport_Code;         
        };

        $scope.onDestinationSelect=function ($item, $model, $label)
        {
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

        function findfares() {

                if ($scope.frmforecastfinder.$invalid) {
                    $scope.hasError = true;
                    return;
                }
           
            $scope.SearchedfareInfo = undefined;
            $scope.IsSearched = true;          
            $scope.SearchbuttonIsLoading = true; $scope.SearchbuttonText = $scope.LoadingText; 
            var data = {
                "Origin": $scope.Origin,
                "DepartureDate": ConvertToRequiredDate($scope.FromDate,'API'),
                "ReturnDate": ConvertToRequiredDate($scope.ToDate,'API'),
                "Destination": $scope.Destination
            };
            FareforecastFactory.fareforecast(data).then(function (data) {
                    $scope.SearchbuttonText = "Get Fares";
                    $scope.SearchbuttonIsLoading = false;                  
                    if (data != null || data != undefined) {                    
                        $scope.SearchedfareInfo = data;                                             
                    }
            });
        
        }      
    }

})();