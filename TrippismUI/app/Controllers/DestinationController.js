
(function () {
    'use strict';
    var controllerId = 'DestinationController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', '$rootScope', '$modal', '$http', 'DestinationFactory', DestinationController]);

    function DestinationController($scope, $rootScope, $modal, $http, DestinationFactory) {

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
        dt.setHours(0,0,0,0)
        var Todt = new Date();
        Todt.setDate(Todt.getDate() + 5); // add default from 5 days
        Todt.setHours(0,0,0,0)

        $scope.ToDate = ConvertToRequiredDate(Todt);
        $scope.FromDate = ConvertToRequiredDate(dt);

        $scope.minTodayDate = new Date();
        $scope.minFromDate = new Date();
        $scope.minFromDate = $scope.minFromDate.setDate($scope.minFromDate.getDate() + 1);


        function daydiff(first, second) {
            //var oneDay = 24 * 60 * 60 * 1000;
            //return Math.round(Math.abs((second.getTime() - first.getTime()) / (oneDay)));
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
                      $scope.ToDate = newDt.setDate(newDt.getDate() + 1)
                  }
                  /**/

                  //SET MINIMUN SELECTED DATE for TODATE
                  $scope.minFromDate = new Date(newValue);
                  $scope.minFromDate = $scope.minFromDate.setDate($scope.minFromDate.getDate() + 1);

                  // Calculate datediff
                  $scope.datedifff = daydiff(new Date(newValue).setHours(0,0,0,0), new Date($scope.ToDate).setHours(0,0,0,0));

              }
       );
        $scope.$watch(function (scope) { return scope.ToDate },
              function (newValue, oldValue) {
                  $scope.datedifff = daydiff(new Date($scope.FromDate).setHours(0,0,0,0), new Date(newValue).setHours(0,0,0,0));
              }
       );

        $scope.Origin = '';

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
                EarliestDepartureDate: ConvertToRequiredDate(rate.DepartureDateTime),
                LatestDepartureDate: ConvertToRequiredDate(rate.ReturnDateTime),
                Destination: rate.DestinationLocation,
                LengthOfStay: "4"
            };
            DestinationFactory.fareforecast(datatopost).then(function (data) {
                $scope.forecastfareList = angular.copy(data.FareData);
                var modalInstance = $modal.open({
                    templateUrl: 'myModalContent.html',
                    controller: 'ModalInstanceCtrl',
                    resolve: {
                        items: function () { return $scope.forecastfareList; },
                        Origin: function () { return $scope.Origin; },
                        Destination: function () { return datatopost.Destination; }
                    }
                });
                modalInstance.result.then(function (selectedItem) {
                    $scope.selected = selectedItem;
                });
            });
        }

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
        $scope.SearchbuttonText = "Get Destinations (All)";
        $scope.SearchbuttonTo10Text = "Top 10";
        $scope.SearchbuttonCheapestText = "Top 10 Cheapest";
        $scope.SearchbuttonIsLoading = false;
        $scope.SearchbuttonTop10IsLoading = false;
        $scope.SearchbuttonChepestIsLoading = false;

        $scope.closeAlert = function (index) {
            $scope.errors.splice(index, 1);
        };

        //var closeAlert = function (index) {
        //    debugger;
        //    $scope.errors.splice(index, 1);
        //};

        function findDestinations(buttnText) {
            
            if ($scope.Origin == undefined || $scope.Origin == "") {
                $scope.hasError = true;
                $scope.errors = [
                   { type: 'warning', msg: 'Please enter origin.' } 
                ];
                return;
            }
            
            $scope.faresList = [];
            if (buttnText == 'All') { $scope.SearchbuttonIsLoading = true; $scope.SearchbuttonText = $scope.LoadingText; }
            else if (buttnText == 'Top10') { $scope.SearchbuttonTop10IsLoading = true; $scope.SearchbuttonTo10Text = $scope.LoadingText; }
            else if (buttnText == 'Cheapest') { $scope.SearchbuttonChepestIsLoading = true; $scope.SearchbuttonCheapestText = $scope.LoadingText; }
            var data = {
                "Origin": $scope.Origin,
                "DepartureDate": ConvertToRequiredDate($scope.FromDate),
                "ReturnDate": ConvertToRequiredDate($scope.ToDate),
                "Lengthofstay": $scope.datedifff
            };
            DestinationFactory.findDestinations(data).then(function (data) {
                $scope.SearchbuttonText = "Get Destinations (All)";
                $scope.SearchbuttonTo10Text = "Top 10";
                $scope.SearchbuttonCheapestText = "Top 10 Cheapest";
                $scope.SearchbuttonIsLoading = false;
                $scope.SearchbuttonTop10IsLoading = false;
                $scope.SearchbuttonChepestIsLoading = false;
                displayDestinations(buttnText, data.FareInfo);
                console.log(data.FareInfo)
            });


        }
        var GetUniqueDestinations = function (destinations) {
            return _.pluck(destinations, "DestinationLocation");
        }

        function DrawMaps(pdestinations) {
            var destinations = GetUniqueDestinations(pdestinations);
            $scope.showPosition('', destinations);
        }

        function displayDestinations(buttnText, destinations) {
            if (buttnText == 'All') {
                $scope.faresList = angular.copy(destinations);
                DrawMaps(_.uniq($scope.faresList, function (destination) { return destination.DestinationLocation; }))
            }

            else if (buttnText == 'Top10') {
                if (destinations.length > 0) {
                    for (var i = 0; i < 10; i++)
                        if (destinations[i] != undefined) { $scope.faresList.push(destinations[i]); }
                }
                DrawMaps(_.uniq($scope.faresList, function (destination) { return destination.DestinationLocation; }))
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
                DrawMaps(_.uniq($scope.faresList, function (destination) { return destination.DestinationLocation; }))
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