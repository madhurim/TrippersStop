
(function () {
    'use strict';
    var controllerId = 'DestinationController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', '$rootScope', '$modal', '$http', '$q', '$compile', 'blockUIConfig', '$location', '$anchorScroll','DestinationFactory','$filter', DestinationController]);

    function DestinationController($scope, $rootScope, $modal, $http, $q, $compile, blockUIConfig, $location,$anchorScroll, DestinationFactory,$filter) {

        var getMapUrlData = function (airportCode) {
            var d = $q.defer();
            var originairport = _.find($scope.AvailableAirports, function (airport) {
                return airport.airport_Code == airportCode.DestinationLocation
            });

            if (originairport != undefined) {
                airportCode.lat = originairport.airport_Lat;
                airportCode.lng = originairport.airport_Lng;
                d.resolve(airportCode); // return the original object, so you can access it's other properties

            } else {
                d.resolve();
            }

            return d.promise;
        }
        function MapscrollTo(id) {
            var old = $location.hash();
            $location.hash(id);
            $anchorScroll();
            $location.hash(old);
        }
        $scope.hasError = false;
        $scope.Location = "";
        $scope.AvailableCodes = [];
        $scope.SeasonalityHistorySearch = SeasonalityHistorySearch;
        $scope.formats = ['yyyy-MM-dd', 'dd-MM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate','MM/dd/yyyy'];
        $scope.format = $scope.formats[5];
        //$scope.lat = "43.09024";
        //$scope.lng = "-30.712891";
        $scope.lat = "20";
        $scope.lng = "0";
        $scope.accuracy = "0";

        $scope.destinationMap = undefined;
        $scope.myMarkers = [];


        //var styleArray = [{ featureType: "landscape.natural", elementType: "geometry.fill", stylers: [{ visibility: "on" }, { color: "#e0efef" }] }, { featureType: "poi", elementType: "geometry.fill", stylers: [{ visibility: "on" }, { hue: "#1900ff" }, { color: "#c0e8e8" }] }, { featureType: "poi", elementType: "labels.text", stylers: [{ visibility: "off" }] }, { featureType: "poi", elementType: "labels.icon", stylers: [{ visibility: "off" }] }, { featureType: "road", elementType: "geometry", stylers: [{ lightness: 100 }, { visibility: "simplified" }] }, { featureType: "road", elementType: "labels", stylers: [{ visibility: "off" }] }, { featureType: "transit.line", elementType: "geometry", stylers: [{ visibility: "on" }, { lightness: 700 }] }, { featureType: "transit.line", elementType: "labels.text", stylers: [{ visibility: "off" }] }, { featureType: "transit.line", elementType: "labels.text.fill", stylers: [{ visibility: "off" }] }, { featureType: "transit.line", elementType: "labels.text.stroke", stylers: [{ visibility: "off" }] }, { featureType: "transit.line", elementType: "labels.icon", stylers: [{ visibility: "off" }] }, { featureType: "transit.station", elementType: "labels.text", stylers: [{ visibility: "off" }] }, { featureType: "transit.station", elementType: "labels.icon", stylers: [{ visibility: "off" }] }, { featureType: "water", elementType: "all", stylers: [{ color: "#9999FF" }] }];
        var styleArray = [{ featureType: "landscape.natural", elementType: "geometry.fill", stylers: [{ visibility: "on" }, { color: "#FFFFFF" }] },
                          { featureType: "poi", elementType: "geometry.fill", stylers: [{ visibility: "on" }, { hue: "#1900ff" }, { color: "#c0e8e8" }] },
                          { featureType: "poi", elementType: "labels.text", stylers: [{ visibility: "off" }] },
                          { featureType: "poi", elementType: "labels.icon", stylers: [{ visibility: "off" }] },
                          { featureType: "road", elementType: "geometry", stylers: [{ lightness: 100 }, { color: '#000000' }, { visibility: "simplified" }] },
                          { featureType: "road", elementType: "labels", stylers: [{ visibility: "off" }] },
                          { featureType: "transit.line", elementType: "geometry", stylers: [{ visibility: "on" }, { lightness: 700 }] },
                          { featureType: "transit.line", elementType: "labels.text", stylers: [{ visibility: "off" }] },
                          { featureType: "transit.line", elementType: "labels.text.fill", stylers: [{ visibility: "off" }] },
                          { featureType: "transit.line", elementType: "labels.text.stroke", stylers: [{ visibility: "off" }] },
                          { featureType: "transit.line", elementType: "labels.icon", stylers: [{ visibility: "off" }] },
                          { featureType: "transit.station", elementType: "labels.text", stylers: [{ visibility: "off" }] },
                          { featureType: "transit.station", elementType: "labels.icon", stylers: [{ visibility: "off" }] },
                          { featureType: "water", elementType: "all", stylers: [{ color: "#BCCFDE" }] }];
        $scope.mapOptions = {
            center: new google.maps.LatLng($scope.lat, $scope.lng),
            zoom: 0,
            minZoom: 2,
            backgroundColor: "#BCCFDE",
            styles: styleArray,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        $scope.markerClicked = function (marker) {
            $scope.destinationMap.panTo(marker.getPosition());
            $scope.IsHistoricalInfo = false;
            $scope.MarkerInfo = marker.CustomMarkerInfo;
        };

        //Set Map Location to Origin
        var SetMaptoCurrentLocation = function () {
            var originairport = _.find($scope.AvailableAirports, function (airport) { return airport.airport_Code == $scope.Origin });
            if (originairport != undefined) {
                var latlng = new google.maps.LatLng(originairport.airport_Lat, originairport.airport_Lng);
                $scope.destinationMap.setCenter(latlng);
                $scope.destinationMap.panTo(latlng);
                $scope.destinationMap.setZoom(5);
            }
        }

        var RenderMap = function (maps) {
            $scope.InfoWindow;

            var bounds = new google.maps.LatLngBounds();

            for (var x = 0; x < maps.length; x++) {
                var latlng1 = new google.maps.LatLng(maps[x].lat, maps[x].lng);
                var marker = new google.maps.Marker({
                    position: latlng1,
                    map: $scope.destinationMap,
                    title: maps[x].DestinationLocation,
                    icon: 'http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=' + (x+ 1) + '|ff776b|000000',
                    animation: google.maps.Animation.DROP,
                    CustomMarkerInfo: maps[x]
                });
                bounds.extend(marker.position);
                //var contentString = '<div ng-controller="FareforecastController" style="min-width:200px;padding-top:5px;" id="content">' +
                //                        '<div class="col-sm-6 padleft0"><span>Airport Code:</span><br /><strong>'
                //                            + maps[x].DestinationLocation + '</strong></div>' +
                //                        '<div class="col-sm-6 padlr0 text-right">' + maps[x].LowestFare + ' ' + maps[x].CurrencyCode + '</div>' +
                //                     '</div>';
                var contentString = '<div ng-controller="FareforecastController" style="min-width:200px;padding-top:5px;" id="content">' +
                                        '<div class="col-sm-6 padleft0"><span>Destination: </span><br /><strong>'
                                          + maps[x].DestinationLocation + '</strong></div>' +
                                          '<div class="col-sm-6 padleft0"><span>Currency: </span><br /><strong>'
                                          + maps[x].CurrencyCode + '</strong></div>' +
                                          '<div class="col-sm-6 padleft0"><span>Lowest Fare: </span><br /><strong>'
                                          + maps[x].LowestFare + '</strong></div>' +
                                          '<div class="col-sm-6 padleft0"><span>Lowest Non Stop Fare: </span><br /><strong>'
                                          + maps[x].LowestNonStopFare + '</strong></div>' +
                                          '<div class="col-sm-6 padleft0"><span>Departure Date: </span><br /><strong>'
                                          + $filter('date')(maps[x].DepartureDateTime, $scope.format, null) + '</strong></div>' +
                                          '<div class="col-sm-6 padleft0"><span>Return Date: </span><br /><strong>'
                                          + $filter('date')(maps[x].ReturnDateTime, $scope.format, null) + '</strong></div>' +
                                   '</div>';

                $scope.InfoWindow = new google.maps.InfoWindow()
                google.maps.event.addListener(marker, 'click', (function (marker, contentString, infowindow) {
                    return function () {
                        if ($scope.InfoWindow) $scope.InfoWindow.close();
                        $scope.InfoWindow = new google.maps.InfoWindow({ content: contentString, maxWidth: 500 });
                        $scope.InfoWindow.open($scope.destinationMap, marker);
                    };
                })(marker, contentString, $scope.InfoWindow));

                $scope.myMarkers.push(marker);
            }
            //now fit the map to the newly inclusive bounds
            //if ($scope.myMarkers.length > 0) {
            //    //$scope.destinationMap.fitBounds(bounds);
            //    //$scope.markerCluster = new MarkerClusterer($scope.destinationMap, $scope.myMarkers);
            //}
        };

        $scope.showPosition = function (destinations) {
            $scope.destinations = destinations;
            var promises = [];
            for (var i = 0; i < $scope.destinations.length; i++) {
                promises.push(getMapUrlData($scope.destinations[i]));
            }
            $q.all(promises).then(function (maps) {

                if (maps.length > 0)
                    maps = _.compact(maps);
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
               var diff = daydiff(new Date(newValue).setHours(0, 0, 0, 0), new Date($scope.Latestdeparturedate).setHours(0, 0, 0, 0));
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
                  $scope.MaximumToDate = common.addDays($scope.minFromDate, 16);

              }
       );

        $scope.Origin = '';
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

        function SeasonalityHistorySearch(dest) {
            var data = { "Destination": dest };
            DestinationFactory.SeasonalityHistorySearch(data).then(function (data) {
                if (data != undefined) {
                    var result = JSON.parse(data);
                    var objects = JSON.parse(result.Response);
                }
            });
        };

        $scope.CustomAirportsData = [];

        $scope.activate = activate;
        $scope.AvailableAirports = [];
        function activate() {
            blockUIConfig.autoBlock = true;
            $http.get('../app/Constants/airports.json').success(function (_arrairports) {
                for (var i = 0; i < _arrairports.length; i++) {
                    if (_arrairports[i].Airports != undefined) {
                        for (var cntAirport = 0; cntAirport < _arrairports[i].Airports.length ; cntAirport++) {
                            var objtopush = _.omit(_arrairports[i], "Airports");
                            objtopush['airport_Code'] = _arrairports[i].Airports[cntAirport].airport_Code;
                            objtopush['airport_FullName'] = _arrairports[i].Airports[cntAirport].airport_FullName;
                            objtopush['airport_Lat'] = _arrairports[i].Airports[cntAirport].airport_Lat;
                            objtopush['airport_Lng'] = _arrairports[i].Airports[cntAirport].airport_Lng;
                            objtopush['airport_IsMAC'] = false;
                            $scope.AvailableAirports.push(objtopush);
                        }
                    }
                    if (_arrairports[i].Airports != undefined && _arrairports[i].Airports.length > 1) {
                        var objtopush = _.omit(_arrairports[i], "Airports");
                        objtopush['airport_Code'] = _arrairports[i].airport_CityCode;
                        objtopush['airport_FullName'] = _arrairports[i].airport_CityName + ", All Airports";
                        objtopush['airport_Lat'] = null;
                        objtopush['airport_Lng'] = null;
                        objtopush['airport_IsMAC'] = true;
                        $scope.AvailableAirports.push(objtopush);
                    }
                }

                $scope.AvailableCodes = angular.copy($scope.AvailableAirports);
                getIpinfo();
                MapscrollTo('map_canvas');
            });

        }
        activate();

        $scope.onSelect = function ($item, $model, $label) {
            $scope.Origin = $item.airport_Code;
        };

        $scope.formatInput = function ($model) {
            if ($model == "" || $model == undefined) return "";

            var originairport = _.find($scope.AvailableAirports, function (airport) { return airport.airport_Code == $model });
            var airportname = (originairport.airport_FullName.toLowerCase().indexOf("airport") > 0) ? originairport.airport_FullName : originairport.airport_FullName + " Airport";
            var CountryName = (originairport.airport_CountryName != undefined) ? originairport.airport_CountryName : "";
            return originairport.airport_Code + ", " + airportname + ", " + originairport.airport_CityName + ", " + CountryName;
        }

        function getnearByAirport() {
            $http({
                method: 'GET',
                url: 'https://airport.api.aero/airport/ORK?user_key=80d9c3bb86b16e397e4bd94875a34552?callback=JSON_CALLBACK',
                headers: { 'Content-type': 'application/xml' }
            }).success(function (d) {

            });
        }

        function getIpinfo() {
            blockUIConfig.autoBlock = true;
            var url = "http://ipinfo.io?callback=JSON_CALLBACK";
            $http.jsonp(url)
           .success(function (data) {
               var originairport = _.find($scope.AvailableAirports, function (airport) { return airport.airport_CityName == data.city && airport.airport_CountryCode == data.country });
               if (originairport != null) {
                   $scope.Origin = originairport.airport_Code;
                   $scope.CalledOnPageLoad = true;
                   $scope.findDestinations('Cheapest');
               }
           });

        }

        $scope.faresList = [];

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
        $scope.SearchbuttonText = "Get Destinations";
        $scope.SearchbuttonTo10Text = "Top 10";
        $scope.SearchbuttonCheapestText = "Top 10 Cheapest";
        $scope.SearchbuttonIsLoading = false;
        $scope.SearchbuttonTop10IsLoading = false;
        $scope.SearchbuttonChepestIsLoading = false;


        function RemoveMarkers() {
            $scope.destinationMap.setZoom(2);
            var latlng = new google.maps.LatLng($scope.lat, $scope.lng);
            $scope.destinationMap.setCenter(latlng);
            if ($scope.myMarkers.length > 0) {
                for (var i = 0; i < $scope.myMarkers.length; i++)
                    $scope.myMarkers[i].setMap(null);
                $scope.myMarkers.length = 0;
               // $scope.markerCluster.clearMarkers();
            }

            $scope.myMarkers = [];
        }

        function findDestinations(buttnText) {
            if ($scope.CalledOnPageLoad != true) {
                if ($scope.frmdestfinder.$invalid) {
                    $scope.hasError = true;
                    return;
                }
            }
            RemoveMarkers();
            $scope.faresList = [];
            $scope.IsHistoricalInfo = false;
            if (buttnText == 'All') { $scope.SearchbuttonIsLoading = true; $scope.SearchbuttonText = $scope.LoadingText; }
            else if (buttnText == 'Top10') { $scope.SearchbuttonTop10IsLoading = true; $scope.SearchbuttonTo10Text = $scope.LoadingText; }
            else if (buttnText == 'Cheapest') { $scope.SearchbuttonChepestIsLoading = true; $scope.SearchbuttonCheapestText = $scope.LoadingText; }
            var data = {
                "Origin": $scope.Origin,
                "DepartureDate": ($scope.FromDate == '' || $scope.FromDate == undefined) ? null : ConvertToRequiredDate($scope.FromDate),
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
                $scope.SearchbuttonText = "Get Destinations";
                $scope.SearchbuttonTo10Text = "Top 10";
                $scope.SearchbuttonCheapestText = "Top 10 Cheapest";
                $scope.SearchbuttonIsLoading = false;
                $scope.SearchbuttonTop10IsLoading = false;
                $scope.SearchbuttonChepestIsLoading = false;

                if (data != null) {
                    if (data.FareInfo != null) {
                        buttnText = "Cheapest";
                        displayDestinations(buttnText, data.FareInfo);
                        MapscrollTo('wrapper')
                    }
                }
                else {
                    alertify.alert("Destination Finder", "");
                    alertify.alert('Sorry! There are no destinations, match your search request!').set('onok', function (closeEvent) { });
                }
            });
            if ($scope.CalledOnPageLoad)
                $scope.CalledOnPageLoad = false;
        }

        var GetUniqueDestinations = function (destinations) {

            var result = _.map(destinations, function (currentObject) {
                return _.omit(currentObject, "Links");
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