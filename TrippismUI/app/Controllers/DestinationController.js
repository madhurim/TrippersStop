
(function () {
    'use strict';
    var controllerId = 'DestinationController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', '$rootScope', '$modal', 'DestinationFactory', DestinationController]);

    function DestinationController($scope, $rootScope, $modal, DestinationFactory) {

        $scope.formats = ['yyyy-MM-dd', 'dd-MM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
        $scope.format = $scope.formats[0];

        var dt = new Date();
        var curr_date = ('0' + dt.getDate()).slice(-2);
        var curr_month = ('0' + (dt.getMonth() + 1)).slice(-2);
        var curr_year = dt.getFullYear();
        $scope.ToDate = curr_year + "-" + curr_month + "-" + curr_date;
        $scope.FromDate = curr_year + "-" + curr_month + "-" + curr_date;
        $scope.Origin = 'bos';
        
        $scope.faresList = [];
        $scope.forecastfareList = [];
        $rootScope.apiURL = 'http://localhost:14606';

        $scope.getDestinations = findDestinations;
        $scope.fareforecast = fareforecast;
        //$scope.lowfareforecast = lowfareforecast;

        function fareforecast(rate) {
            rate.Origin = $scope.Origin;
            var datatopost = {
                Origin: rate.Origin,
                EarliestDepartureDate :ConvertToRequiredDate(rate.DepartureDateTime),
                LatestDepartureDate: ConvertToRequiredDate(rate.ReturnDateTime),
                Destination : rate.DestinationLocation,
                LengthOfStay : "4"
            };
            DestinationFactory.fareforecast(datatopost).then(function (data) {
                $scope.forecastfareList = angular.copy(data.FareData);
                var modalInstance = $modal.open({
                    templateUrl: 'myModalContent.html',
                    controller: 'ModalInstanceCtrl',
                    resolve: {
                        items: function () {return $scope.forecastfareList;},
                        Origin: function () { return $scope.Origin;},
                        Destination: function () {return datatopost.Destination;}
                    }
                });
                modalInstance.result.then(function (selectedItem) {
                    $scope.selected = selectedItem;
                });
            });
        }

        //function lowfareforecast(rate) {
        //    rate.Origin = $scope.Destination;
        //    var data = {
        //        Origin: rate.Origin,
        //        DepartureDate: ConvertToRequiredDate(rate.DepartureDateTime),
        //        ReturnDate: ConvertToRequiredDate(rate.ReturnDateTime),
        //        Destination: rate.DestinationLocation
        //    };
        //    DestinationFactory.lowfareforecast(data).then(function (data) {
        //        console.log(data);
        //        $scope.lowestforecastfareList = angular.copy(data.FareData);
        //    });
        //}

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

        $scope.LoadingText = "Loading...";

        $scope.SearchbuttonText = "Get Destinations (All)";
        $scope.SearchbuttonTo10Text = "Top 10";
        $scope.SearchbuttonCheapestText = "Top 10 Cheapest";

        $scope.SearchbuttonIsLoading = false;
        $scope.SearchbuttonTop10IsLoading = false;
        $scope.SearchbuttonChepestIsLoading = false;
        
        function ConvertToRequiredDate(dt) {
            dt = new Date(dt);
            var curr_date = ('0' + dt.getDate()).slice(-2);
            var curr_month = ('0' + (dt.getMonth() + 1)).slice(-2);
            var curr_year = dt.getFullYear();
            var _date = curr_year + "-" + curr_month + "-" + curr_date;
            return _date;
        }

        function findDestinations(buttnText) {
            $scope.faresList = [];
            if (buttnText == 'All') { $scope.SearchbuttonIsLoading = true; $scope.SearchbuttonText = $scope.LoadingText; }
            else if (buttnText == 'Top10') { $scope.SearchbuttonTop10IsLoading = true; $scope.SearchbuttonTo10Text = $scope.LoadingText; }
            else if (buttnText == 'Cheapest') { $scope.SearchbuttonChepestIsLoading = true; $scope.SearchbuttonCheapestText = $scope.LoadingText; }
            var data = {
                "Origin": $scope.Origin,
                "DepartureDate": ConvertToRequiredDate($scope.FromDate),
                "ReturnDate": ConvertToRequiredDate($scope.ToDate),
                "Lengthofstay": '4'
            };
            DestinationFactory.findDestinations(data).then(function (data) {
                $scope.SearchbuttonText = "Get Destinations (All)";
                $scope.SearchbuttonTo10Text = "Top 10";
                $scope.SearchbuttonCheapestText = "Top 10 Cheapest";
                $scope.SearchbuttonIsLoading = false;
                $scope.SearchbuttonTop10IsLoading = false;
                $scope.SearchbuttonChepestIsLoading = false;
                displayDestinations(buttnText, data.FareInfo);
            });
        }

        function displayDestinations(buttnText, destinations) {
            if (buttnText == 'All') 
                $scope.faresList = angular.copy(destinations);
            
            else if (buttnText == 'Top10') {
                if (destinations.length > 0) {
                    for (var i = 0; i < 10; i++) 
                        if (destinations[i] != undefined) { $scope.faresList.push(destinations[i]); }
                }
                
            }
            else if (buttnText == 'Cheapest') {
                if (destinations.length > 0) {
                    var sortedObjs = _.filter(destinations, function (item) {
                        return item.LowestFare !== 'N/A';
                    });
                    sortedObjs = _(sortedObjs).sortBy(function (obj) { return parseInt(obj.LowestFare, 10) })
                    for (var i = 0; i < 10; i++)
                        if (sortedObjs[i] != undefined) 
                            $scope.faresList.push(sortedObjs[i]);
                }
            }
        }
    }

    angular.module('TrippismUIApp').controller('ModalInstanceCtrl', function ($scope, $modalInstance, items, Origin, Destination) {
        $scope.forecastfareList = items;
        $scope.Origin = Origin;
        $scope.Destination = Destination;
        $scope.ok = function () {
            $modalInstance.close();
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    });



})();