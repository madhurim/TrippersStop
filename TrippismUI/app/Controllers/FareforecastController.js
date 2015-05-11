(function () {
    'use strict';
    var controllerId = 'FareforecastController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', '$rootScope', '$modal', '$http', 'FareforecastFactory', FareforecastController]);

    function FareforecastController($scope, $rootScope, $modal, $http, FareforecastFactory) {

        $scope.hasError = false;
        
        
        //$scope.closeAlert = closeAlert;
        $scope.lat = "0";
        $scope.lng = "0";
        $scope.accuracy = "0";
        $scope.error = "";
        $scope.model = { destinationMap: undefined };
        $scope.myMarkers = [];
        
        $scope.mapOptions = {
            center: new google.maps.LatLng($scope.lat, $scope.lng),
            zoom: 3,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        $scope.showPosition = function (position, destinations) {
            //$scope.lat = position.coords.latitude;
            //$scope.lng = position.coords.longitude;
            //$scope.accuracy = position.coords.accuracy;
            //$scope.$apply();
            var latlng = new google.maps.LatLng($scope.lat, $scope.lng);
            $scope.model.destinationMap.setCenter(latlng);
            for (var x = 0; x < destinations.length; x++) {
                $http({ method: 'GET', url: 'http://maps.googleapis.com/maps/api/geocode/json?address=' + destinations[x] + '&sensor=false' }).
                    success(function (data, status) {
                        if (data.results[0] != undefined) {
                            var p = data.results[0].geometry.location
                            var latlng = new google.maps.LatLng(p.lat, p.lng);
                            var marker = new google.maps.Marker({
                                position: latlng,
                                map: $scope.model.destinationMap,
                                title: data.results[0].address_components[0].short_name,
                                animation: google.maps.Animation.DROP,
                            });
                            $scope.myMarkers.push(new google.maps.Marker(marker));
                        }
                    });

            }
        }

        function ConvertToRequiredDate(dt) {
            dt = new Date(dt);
            var curr_date = ('0' + dt.getDate()).slice(-2);
            var curr_month = ('0' + (dt.getMonth() + 1)).slice(-2);
            var curr_year = dt.getFullYear();
            var _date = curr_year + "-" + curr_month + "-" + curr_date;
            return _date;
        }

        $scope.formats = ['yyyy-MM-dd', 'dd-MM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
        $scope.format = $scope.formats[0];
        
        var dt = new Date();
        dt.setHours(0, 0, 0, 0)
        var Todt = new Date();
        Todt.setDate(Todt.getDate() + 5); // add default from 5 days
        Todt.setHours(0, 0, 0, 0)

        $scope.ToDate = ConvertToRequiredDate(Todt);
        $scope.FromDate = ConvertToRequiredDate(dt);

        $scope.minTodayDate = new Date();
        $scope.minFromDate = new Date();
        $scope.minFromDate = $scope.minFromDate.setDate($scope.minFromDate.getDate() + 1);


        function daydiff(first, second) {
            return Math.round((second - first) / (1000 * 60 * 60 * 24));
        }

        $scope.datedifff = daydiff(dt, Todt);

        $scope.ToDate = ConvertToRequiredDate(Todt);
        $scope.FromDate = ConvertToRequiredDate(dt);

        $scope.$watch(function (scope) { return scope.FromDate },
              function (newValue, oldValue) {

                  /* If from date is greater than to date */
                  var newDt = new Date(newValue);
                  newDt.setHours(0, 0, 0, 0);
                  var todate = new Date($scope.ToDate);
                  todate.setHours(0, 0, 0, 0);

                  if (newDt >= todate) {
                      $scope.ToDate = ConvertToRequiredDate(newDt.setDate(newDt.getDate() + 1))
                  }
                  /**/

                  //SET MINIMUN SELECTED DATE for TODATE
                  $scope.minFromDate = new Date(newValue);
                  $scope.minFromDate = $scope.minFromDate.setDate($scope.minFromDate.getDate() + 1);

                  // Calculate datediff
                  $scope.datedifff = daydiff(new Date(newValue).setHours(0, 0, 0, 0), new Date($scope.ToDate).setHours(0, 0, 0, 0));

              }
       );
        $scope.$watch(function (scope) { return scope.ToDate },
              function (newValue, oldValue) {
                  $scope.datedifff = daydiff(new Date($scope.FromDate).setHours(0, 0, 0, 0), new Date(newValue).setHours(0, 0, 0, 0));
              }
       );

        $scope.Origin = '';
        $scope.Destination = '';
        $scope.IsSearched = false;
        $scope.faresList = [];
        $scope.forecastfareList = [];
        $rootScope.apiURL = 'http://localhost:14606/sabre/api/';

        $scope.findfares  = findfares ;
        
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

        $scope.LoadingText = "Loading..";
        $scope.SearchbuttonText = "Get Fares";
        $scope.SearchbuttonIsLoading = false;
        
      
        
        $scope.isUndefined = function (thing) {
            return (typeof thing === "undefined");
        }

        function findfares() {
           
            if ($scope.frmdestfinder.$invalid) {
                $scope.hasError = true;
                return;
            }
            $scope.SearchedfareInfo = undefined;
            $scope.IsSearched = true;
            $scope.faresList = [];
            $scope.SearchbuttonIsLoading = true; $scope.SearchbuttonText = $scope.LoadingText; 
            var data = {
                "Origin": $scope.Origin,
                "DepartureDate": ConvertToRequiredDate($scope.FromDate),
                "ReturnDate": ConvertToRequiredDate($scope.ToDate),
                "Destination": $scope.Destination
            };
            FareforecastFactory.fareforecast(data).then(function (data) {
                    $scope.SearchbuttonText = "Get Fares";
                    $scope.SearchbuttonIsLoading = false;
                    console.log(data);
                    if (data != null || data != undefined) {
                        $scope.SearchedfareInfo = data;
                        displayDestinations(data);
                    }
                });

        }
        
        function DrawMaps(pdestinations) {
            $scope.showPosition('', pdestinations);
        }

        function displayDestinations(destination) {
            $scope.faresList = angular.copy(destinations);
            var destinations = new Array(destination.OriginLocation, destination.DestinationLocation);
            DrawMaps(destinations);
        }
    }


})();