(function () {
    'use strict';
    var controllerId = 'InstaFlightSearchController';
    angular.module('TrippismUIApp').controller(controllerId, ['$scope', '$filter', '$modal', '$sce', 'InstaFlightSearchFactory', 'UtilFactory', 'instaFlightSearchData', InstaFlightSearchController]);
    function InstaFlightSearchController($scope, $filter, $modal, $sce, InstaFlightSearchFactory, UtilFactory, instaFlightSearchData) {
        $scope.isInstaFlightDataFound = null;
        $scope.instaFlightSearchData = instaFlightSearchData;
        $scope.airportNamesList = [];
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
                    IncludedCarriers: $scope.instaFlightSearchData.IncludedCarriers,
                    LowestFare: instaFlightSearchData.LowestFare
                };
                if ($scope.instaFlightSearch.IncludedCarriers != '' && $scope.instaFlightSearch.IncludedCarriers.length > 0)
                    $scope.instaFlightSearch.IncludedCarriers = $scope.instaFlightSearch.IncludedCarriers.join(',');
                InstaFlightSearchFactory.GetData($scope.instaFlightSearch).then(function (data) {
                    if (data.status != 404 && data.status != 400 && data != "" && data.PricedItineraries.length > 0) {
                        $scope.isInstaFlightDataFound = true;
                        $scope.isSearchingFlights = false;
                        $scope.instaFlightSearchResult = data;
                        $scope.instaFlightSearchLimit = 10;
                        $scope.currencyCode = $scope.$parent.attractionParams.mapOptions.CurrencyCode;
                        $scope.lowestFare = $scope.instaFlightSearch.LowestFare
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
        $scope.getStopsFromFlightSegment = function (flightSegment) {
            var arrivalAirportList = [];
            for (i = 0; i < flightSegment.length - 1; i++) {
                arrivalAirportList.push(flightSegment[i].ArrivalAirport.LocationCode);
            }
            return arrivalAirportList.join(', ');
        };
        $scope.airportConnectionData = function (flightSegmentList, flightSegment) {
            var result = { waitingTime: '', isAirportChanged: false, isConnectInAirport: false, isLongWait: false };
            var arrivalDate = new Date(flightSegment.ArrivalDateTime.replace('T', ' ').replace(/-/g, "/")).getTime();
            var arrivalAirport = flightSegment.ArrivalAirport.LocationCode;
            var index = flightSegmentList.indexOf(flightSegment);
            var nextFlightSegment = flightSegmentList[index + 1];
            if (nextFlightSegment != undefined) {
                //waitingTime
                var departureDate = new Date(nextFlightSegment.DepartureDateTime.replace('T', ' ').replace(/-/g, "/")).getTime();
                var dateDiffInMinute = (departureDate - arrivalDate) / 60000;
                var hours = $filter('floor')(dateDiffInMinute / 60);
                result.waitingTime = $filter('twoDigit')(hours) + ' h ' + $filter('twoDigit')(dateDiffInMinute % 60) + ' min';
                //isAirportChanged
                var departureAirport = nextFlightSegment.DepartureAirport.LocationCode;
                result.isAirportChanged = !(arrivalAirport == departureAirport);
                //isConnectInAirport
                result.isConnectInAirport = result.isAirportChanged == false && dateDiffInMinute > 0;
                //isLongWait
                result.isLongWait = hours > 2;
            }
            return result;
        }
        $scope.amountBifercation = function (TotalfareAmount)
        {
            var result = {
                BeforeDecimal: Math.floor(TotalfareAmount),
                AfterDecimal: "."+(TotalfareAmount + "").split(".")[1]
            };
            return result;
        }
        var getAirportNameFromCode = function (airportCode) {
            var airportName = airportCode;
            var airportData = $filter('filter')($scope.$parent.attractionParams.AvailableAirports, { airport_Code: airportCode });
            if (airportData) {
                if (angular.isArray(airportData))
                    airportData = airportData[0];
                //airportData = $filter('filter')(airportData, { airport_IsMAC: false });

                if (airportData && airportData.airport_FullName) {
                    airportName = airportData.airport_FullName;
                }
                return airportName;
            }
        }
        $scope.getHTMLTooltip = function (airportCode) {
            var airportName = getAirportNameFromCode(airportCode);
            //return $sce.trustAsHtml("<div><span class='glyphicon glyphicon-plane rotate-right plane-ico'></span><span style='margin-left:10px;'>" + airportName + "</span></div>");            
            return airportName;
        }
    }
})();