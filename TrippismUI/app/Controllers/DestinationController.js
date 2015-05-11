
(function () {
    'use strict';
    var controllerId = 'DestinationController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', '$rootScope', '$modal', '$http', '$q', '$compile', 'DestinationFactory', DestinationController]);

    function DestinationController($scope, $rootScope, $modal, $http, $q, $compile, DestinationFactory) {

        

        var getMapUrlData = function (airportCode) {
            var d = $q.defer();
            $http({ method: 'GET', url: 'http://maps.googleapis.com/maps/api/geocode/json?address=' + airportCode.DestinationLocation + ' airport&sensor=false' }).
                    success(function (data, status) {
                        if (data.results[0] != undefined) {
                            if (_.contains(data.results[0].types, "airport")) {
                                var latlong = data.results[0].geometry.location;
                                airportCode.lat = latlong.lat;
                                airportCode.lng = latlong.lng;
                                d.resolve(airportCode); // return the original object, so you can access it's other properties
                            }
                        } else {
                            d.resolve();
                        }
                    });
            return d.promise;
        }

        $scope.hasError = false;
        $scope.Location = "";
        
        $scope.SeasonalityHistorySearch = SeasonalityHistorySearch;
       
        function SeasonalityHistorySearch(dest) {
            var data = { "Destination" : dest};
            DestinationFactory.SeasonalityHistorySearch(data).then(function (data) {
                if (data != undefined) {
                    var result = JSON.parse(data);
                    var objects = JSON.parse(result.Response);
                }
            });
        };

        $scope.lat = "0";
        $scope.lng = "0";
        $scope.accuracy = "0";

        $scope.model = { destinationMap: undefined };
        $scope.myMarkers = [];

        $scope.mapOptions = {
            center: new google.maps.LatLng($scope.lat, $scope.lng),
            zoom: 3,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        $scope.markerClicked = function (marker) {
            $scope.model.destinationMap.panTo(marker.getPosition());
            $scope.IsHistoricalInfo = true;
            $scope.MarkerInfo = marker.CustomMarkerInfo;
            //SeasonalityHistorySearch(marker.title);
        };
        
        var RenderMap = function (maps) {
            $scope.InfoWindow;
            var latlng = new google.maps.LatLng($scope.lat, $scope.lng);
            $scope.model.destinationMap.setCenter(latlng);
            for (var x = 0; x < maps.length; x++) {
                var latlng1 = new google.maps.LatLng(maps[x].lat, maps[x].lng);
                var marker = new google.maps.Marker({
                    position: latlng1,
                    map: $scope.model.destinationMap,
                    title: maps[x].DestinationLocation,
                    animation: google.maps.Animation.DROP,
                    CustomMarkerInfo : maps[x]
                });
                var contentString = '<div ng-controller="FareforecastController" style="min-width:200px;padding-top:5px;" id="content">' +
                                        '<div class="col-sm-6 padleft0"><span>Airport Code:</span><br /><strong>'
                                            + maps[x].DestinationLocation+ '</strong></div>' +
                                        '<div class="col-sm-6 padlr0 text-right">' + maps[x].LowestFare + ' ' + maps[x].CurrencyCode + '</div>' +
                                     '</div>';

                //var contentString = ($compile("<input type='text' ng-model='Origin' />")($scope));

                $scope.InfoWindow = new google.maps.InfoWindow()
                google.maps.event.addListener(marker, 'click', (function (marker, contentString, infowindow) {
                    return function () {
                        if ($scope.InfoWindow) $scope.InfoWindow.close();
                        $scope.InfoWindow = new google.maps.InfoWindow({ content: contentString, maxWidth: 500 });
                        $scope.InfoWindow.open($scope.model.destinationMap, marker);
                    };
                })(marker, contentString, $scope.InfoWindow));
                $scope.myMarkers.push(new google.maps.Marker(marker));
            }
        };

        $scope.showPosition = function (destinations) {
            $scope.destinations = destinations;
            var promises = [];
            for (var i = 0; i < $scope.destinations.length; i++) {
                promises.push(getMapUrlData($scope.destinations[i]));
            }
            $q.all(promises).then(function (maps) {
                RenderMap(maps);
            }.bind(this));
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

        $scope.LenghtOfStay = daydiff(dt, Todt);

        $scope.ToDate = ConvertToRequiredDate(Todt);
        $scope.FromDate = ConvertToRequiredDate(dt);

        
        

        $scope.$watch(function (scope) { return scope.Earliestdeparturedate },
           function (newValue, oldValue) {
               if (newValue == null)
                   return;

               /* If from date is greater than to date */
               var newDt = new Date(newValue);
               newDt.setHours(0, 0, 0, 0);
               var todate = ($scope.Latestdeparturedate == undefined || $scope.Latestdeparturedate == '') ? new Date() : new Date($scope.Latestdeparturedate);
               todate.setHours(0, 0, 0, 0);

               if (newDt >= todate) {
                   $scope.Latestdeparturedate = ConvertToRequiredDate(newDt.setDate(newDt.getDate() + 1))
               }
               /**/

               // Calculate datediff
               var diff =  daydiff(new Date(newValue).setHours(0, 0, 0, 0), new Date($scope.Latestdeparturedate).setHours(0, 0, 0, 0));
               if (diff > 30) 
                   $scope.Latestdeparturedate = ConvertToRequiredDate(common.addDays(newDt, 30));

               $scope.MaximumLatestdeparturedate = common.addDays(newDt, 30);
           }
        );


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
                      $scope.ToDate = ConvertToRequiredDate(newDt.setDate(newDt.getDate() + 1))
                  }
                  /**/

                  //SET MINIMUN SELECTED DATE for TODATE
                  $scope.minFromDate = new Date(newValue);
                  $scope.minFromDate = $scope.minFromDate.setDate($scope.minFromDate.getDate() + 1);

                  // Calculate datediff
                  //$scope.LenghtOfStay = daydiff(new Date(newValue).setHours(0, 0, 0, 0), new Date($scope.ToDate).setHours(0, 0, 0, 0));
                  //if ($scope.LenghtOfStay > 16) {
                  //    $scope.ToDate = ConvertToRequiredDate(common.addDays(newDt, 16));
                  //}
                  $scope.MaximumToDate = common.addDays($scope.minFromDate, 16);

              }
       );
        $scope.$watch(function (scope) { return scope.ToDate },
              function (newValue, oldValue) {
                 // $scope.LenghtOfStay = daydiff(new Date($scope.FromDate).setHours(0, 0, 0, 0), new Date(newValue).setHours(0, 0, 0, 0));
              }
       );

        $scope.Origin = 'BOS';
        $scope.Destination = '';
        $scope.AvailableThemes = [
                                    { id: "BEACH", value: "BEACH" },
                                    { id: "CARIBBEAN", value: "CARIBBEAN" },
                                    { id: "DISNEY", value: "DISNEY" },
                                    { id: "GAMBLING", value: "GAMBLING" },
                                    { id: "HISTORIC", value: "HISTORIC" },
                                    { id: "MOUNTAINS", value: "MOUNTAINS" },
                                    { id: "NATIONAL-PARKS", value: "NATIONAL-PARKS" },
                                    { id: "OUTDOORS", value: "OUTDOORS" },
                                    { id: "ROMANTIC", value: "ROMANTIC" },
                                    { id: "SHOPPING", value: "SHOPPING" },
                                    { id: "SKIING", value: "SKIING" },
                                    { id: "THEME-PARK", value: "THEME-PARK" }
                                ];
        $scope.AvailableRegions = [
                                    { id: 'Africa', value: 'Africa' },
                                    { id: 'Asia Pacific', value: 'Asia Pacific' },
                                    { id: 'Europe', value: 'Europe' },
                                    { id: 'Latin America', value: 'Latin America' },
                                    { id: 'Middle East', value: 'Middle East' },
                                    { id: 'North America', value: 'North America' },
        ];

        $scope.MaximumFromDate = ConvertToRequiredDate(common.addDays(new Date(), 192));


        function getIpinfo() {
            var url = "http://ipinfo.io?callback=JSON_CALLBACK";
            $http.jsonp(url)
           .success(function (data) {
               $http.get('../app/Constants/airports.json').success(function (_arrairports) {
                   var originairport = _.find(_arrairports, function (airport) { return airport.iso == data.country && airport.status == 1 && airport.size == "medium"; });
                   $scope.Origin = originairport.iata;
                   $scope.CalledOnPageLoad = true;
                   $scope.findDestinations('Top10');
               });
           });
        }
        // getIpinfo();


        $scope.faresList = [];
        $scope.forecastfareList = [];
        $rootScope.apiURL = 'http://localhost:14606/sabre/api/';

        $scope.findDestinations = findDestinations;
        

        $scope.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.opened = true;
        };

        $scope.openEarliestdeparturedate = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.openedEarliestdeparturedate = true;
        };
        
        $scope.openLatestdeparturedate = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.openedLatestdeparturedate = true;
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

        function findDestinations(buttnText) {
            console.log($scope.frmdestfinder);  
            //  if ($scope.CalledOnPageLoad != true) {
            if ($scope.frmdestfinder.$invalid) {
                $scope.hasError = true;
                return;
            }
            //    }

            $scope.faresList = [];
            $scope.IsHistoricalInfo = false;
            if (buttnText == 'All') { $scope.SearchbuttonIsLoading = true; $scope.SearchbuttonText = $scope.LoadingText; }
            else if (buttnText == 'Top10') { $scope.SearchbuttonTop10IsLoading = true; $scope.SearchbuttonTo10Text = $scope.LoadingText; }
            else if (buttnText == 'Cheapest') { $scope.SearchbuttonChepestIsLoading = true; $scope.SearchbuttonCheapestText = $scope.LoadingText; }
            var data = {

                "Origin": $scope.Origin,
                "DepartureDate": ($scope.FromDate == '' || $scope.FromDate == undefined ) ? null : ConvertToRequiredDate($scope.FromDate),
                "ReturnDate": ($scope.ToDate == '' || $scope.ToDate == undefined) ? null : ConvertToRequiredDate($scope.ToDate),
                "Lengthofstay": $scope.LenghtOfStay,
                "Earliestdeparturedate": ($scope.Earliestdeparturedate == '' || $scope.Earliestdeparturedate == undefined) ? null : ConvertToRequiredDate($scope.Earliestdeparturedate),
                "Latestdeparturedate": ($scope.Latestdeparturedate == '' || $scope.Latestdeparturedate == undefined) ? null : ConvertToRequiredDate($scope.Latestdeparturedate),
                "Theme": ($scope.Theme != undefined) ? $scope.Theme.id : "",
                "Location": $scope.Location,
                "Minfare": $scope.Minfare,
                "Maxfare": $scope.Maxfare,
                "PointOfSaleCountry": $scope.PointOfSaleCountry,
                "Region": ($scope.Region != undefined) ? $scope.Region.id : "",
                "TopDestinations": $scope.TopDestinations,
                "Destination": $scope.Destination

            };
            DestinationFactory.findDestinations(data).then(function (data) {
                
                $scope.SearchbuttonText = "Get Destinations (All)";
                $scope.SearchbuttonTo10Text = "Top 10";
                $scope.SearchbuttonCheapestText = "Top 10 Cheapest";
                $scope.SearchbuttonIsLoading = false;
                $scope.SearchbuttonTop10IsLoading = false;
                $scope.SearchbuttonChepestIsLoading = false;

                if (data != null) {
                    if (data.FareInfo != null) 
                        displayDestinations(buttnText, data.FareInfo);
                    
                    //var result = JSON.parse(data);
                    //var objects = JSON.parse(result.Response);
                    //if (objects.FareInfo != undefined) 
                    //    displayDestinations(buttnText, objects.FareInfo);
                    
                }
            });
            if ($scope.CalledOnPageLoad)
                $scope.CalledOnPageLoad = false;


        }
        var GetUniqueDestinations = function (destinations) {
            //return _.pluck(destinations, "DestinationLocation");
            var result = _.map(destinations, function (currentObject) {
                
                return _.omit(currentObject, "Links");
                //return _.pick(currentObject, "DestinationLocation", "LowestFare", "CurrencyCode");
            });
            return result;
        }

        function DrawMaps(pdestinations) {
            var destinations = GetUniqueDestinations(pdestinations);
            $scope.showPosition(destinations);
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

   


})();