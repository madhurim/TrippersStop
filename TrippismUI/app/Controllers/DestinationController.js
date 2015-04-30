
(function () {
    'use strict';
    var controllerId = 'DestinationController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', '$rootScope', 'DestinationFactory', DestinationController]);

    function DestinationController($scope, $rootScope, DestinationFactory) {

        $scope.formats = ['yyyy-MM-dd', 'dd-MM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
        $scope.format = $scope.formats[0];


        var d = new Date();
        var curr_date = ('0' + d.getDate()).slice(-2);
        var curr_month = ('0' + (d.getMonth() + 1)).slice(-2);
        var curr_year = d.getFullYear();

        $scope.ToDate = curr_year + "-" + curr_month + "-" + curr_date;
        $scope.FromDate = curr_year + "-" + curr_month + "-" + curr_date;
        $scope.Destination = 'bos';
        
        $scope.faresList = [];
        $rootScope.apiURL = 'http://localhost:14606';
        $scope.getDestinations = findDestinations;

        $scope.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.opened = true;
        };

        $scope.openFromDate = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.openedFromDate = true;
        };

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.SearchbuttonText = "Get Destinations";
        $scope.SearchbuttonIsLoading = false;

        function ConvertToRequiredDate(dt) {
            dt = new Date(dt);
            var curr_date = ('0' + dt.getDate()).slice(-2);
            var curr_month = ('0' + (dt.getMonth() + 1)).slice(-2);
            var curr_year = dt.getFullYear();
            var _date = curr_year + "-" + curr_month + "-" + curr_date;
            return _date;
        }

        function findDestinations() {
            $scope.faresList = [];
            $scope.SearchbuttonText = "Loading Destinations...";
            $scope.SearchbuttonIsLoading = true;
            var data = {
                "Origin": $scope.Destination,
                "DepartureDate": ConvertToRequiredDate($scope.FromDate),
                "ReturnDate": ConvertToRequiredDate($scope.ToDate),
                "Lengthofstay": '4'
            };

            DestinationFactory.findDestinations(data).then(function (data) {
                $scope.SearchbuttonText = "Get Destinations";
                $scope.SearchbuttonIsLoading = false;
                $scope.faresList = angular.copy(data.FareInfo);
            });
        }




    }







})();


