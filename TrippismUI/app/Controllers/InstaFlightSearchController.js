(function () {
    'use strict';
    var controllerId = 'InstaFlightSearchController';
    angular.module('TrippismUIApp').controller(controllerId, ['$scope', '$filter', '$modal', 'InstaFlightSearchFactory', InstaFlightSearchController]);
    function InstaFlightSearchController($scope, $filter, $modal, InstaFlightSearchFactory) {
        $scope.isInstaFlightDataFound = null;
        $scope.instaFlightSearchData = $scope.$parent.attractionParams.instaFlightSearchData;
        function active() {
            if ($scope.instaFlightSearchData != undefined) {
                $scope.isSearchingFlights = true;
                var departureDate = $filter('date')(angular.isString($scope.instaFlightSearchData.FromDate) ? new Date($scope.instaFlightSearchData.FromDate.split('T')[0].replace(/-/g, "/")) : $scope.instaFlightSearchData.FromDate, 'yyyy-MM-dd');
                var returnDate = $filter('date')(angular.isString($scope.instaFlightSearchData.ToDate) ? new Date($scope.instaFlightSearchData.ToDate.split('T')[0].replace(/-/g, "/")) : $scope.instaFlightSearchData.ToDate, 'yyyy-MM-dd');

                $scope.instaFlightSearch = {
                    Origin: $scope.instaFlightSearchData.OriginAirportName,
                    Destination: $scope.instaFlightSearchData.DestinationaArportName,
                    DepartureDate: departureDate,
                    ReturnDate: returnDate,
                    Minfare: $scope.instaFlightSearchData.Minfare,
                    Maxfare: $scope.instaFlightSearchData.Maxfare,
                    IncludedCarriers: $scope.instaFlightSearchData.IncludedCarriers
                };
                if ($scope.instaFlightSearch.IncludedCarriers != '' && $scope.instaFlightSearch.IncludedCarriers.length > 0)
                    $scope.instaFlightSearch.IncludedCarriers = $scope.instaFlightSearch.IncludedCarriers.join(',');
                InstaFlightSearchFactory.GetData($scope.instaFlightSearch).then(function (data) {
                    if (data.status != 404 && data.status != 400 && data != "" && data.PricedItineraries.length > 0) {
                        $scope.isInstaFlightDataFound = true;
                        $scope.isSearchingFlights = false;
                        $scope.instaFlightSearchResult = data;
                        $scope.instaFlightSearchLimit = 10;
                        $scope.lowestFare = $scope.$parent.attractionParams.mapOptions.LowestFare.Fare;
                        $scope.currencyCode = $scope.$parent.attractionParams.mapOptions.CurrencyCode;
                    }
                    else {
                        $scope.isInstaFlightDataFound = false;
                    }
                })
            }
        }
        active();

        $scope.dismiss = function () {
            $scope.$dismiss('cancel');
        };
        $scope.increaseLimit = function () {
            $scope.instaFlightSearchLimit = $scope.instaFlightSearchLimit + 10;
        }
    }
})();