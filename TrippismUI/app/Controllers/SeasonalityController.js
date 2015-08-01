(function () {
    'use strict';
    var controllerId = 'SeasonalityController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', '$modal', 'SeasonalityFactory', 'UtilFactory', SeasonalityController]);

    function SeasonalityController($scope, $modal, SeasonalityFactory, UtilFactory) {

        $scope.hasError = false;
        $scope.activate = activate;     
        $scope.Destination = '';
        $scope.IsSearched = false;
        $scope.AvailableAirports = [];
        $scope.findseasonality = findseasonality;     
        $scope.LoadingText = "Loading..";
        $scope.SearchbuttonText = "Get Seasonality";
        $scope.SearchbuttonIsLoading = false;
     
        function activate() {
            UtilFactory.ReadAirportJson().then(function (data) {
                $scope.AvailableAirports = data;
                $scope.AvailableCodes = angular.copy($scope.AvailableAirports);
                UtilFactory.getIpinfo($scope.AvailableAirports).then(function (data) {
                    $scope.Destination = data.airport_Code;
                    $scope.findseasonality();
                });               
            });
        }

        activate();

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

        function findseasonality() {
            if ($scope.frmrangefinder.$invalid) {
                $scope.hasError = true;
                return;
            }
            $scope.SearchedseasonalityInfo = undefined;
            $scope.IsSearched = true;
            $scope.SearchbuttonIsLoading = true; $scope.SearchbuttonText = $scope.LoadingText;
            var data = {             
                "Destination": $scope.Destination                
            };
            SeasonalityFactory.Seasonality(data).then(function (data) {
                $scope.SearchbuttonText = "Get Seasonality";
                $scope.SearchbuttonIsLoading = false;
                if (data != null || data != undefined) {                  
                    $scope.SearchedseasonalityInfo = data;                                    
                }
            });

        }
    }

})();