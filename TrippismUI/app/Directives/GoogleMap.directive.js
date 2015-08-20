var selected;
'use strict';
angular.module('TrippismUIApp')
  .directive('googleMap', function ($compile) {
      var directive = {};
      directive.templateUrl = '/app/Views/GoogleMap.html',
      directive.scope = {
          btntext: "=btntext",
          destinations: "=destinations",
          airportlist: "=airportlist",
          defaultlat: "@",
          defaultlng: "@"
      }

      directive.controller = function ($scope, $q, $compile, $filter, $timeout, $rootScope, $http) {
          $scope.destinationMap = undefined;
          $scope.faresList = [];
          $scope.myMarkers = [];
          $scope.bounds;
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
              center: new google.maps.LatLng($scope.defaultlat, $scope.defaultlng),
              zoom: 2,
              minZoom: 2,
              backgroundColor: "#BCCFDE",
              styles: styleArray,
              mapTypeId: google.maps.MapTypeId.ROADMAP
          };

          $scope.showPosition = function (destinations) {

              $scope.destinationslist = destinations;
              var promises = [];
              for (var i = 0; i < $scope.destinationslist.length; i++) {
                  promises.push(getMapUrlData($scope.destinationslist[i]));
              }
              $q.all(promises).then(function (maps) {

                  if (maps.length > 0)
                      maps = _.compact(maps);
                  RenderMap(maps);
              }.bind(this));
          }

          $scope.displayDestinations = function (buttnText, destinations) {
              $scope.faresList = [];
              $scope.faresList = angular.copy(destinations);
              $scope.showPosition(_.uniq($scope.faresList, function (destination) { return destination.DestinationLocation; }))
          }



          var RenderMap = function (maps) {
              $scope.InfoWindow;
              var bounds = new google.maps.LatLngBounds();
              $scope.bounds = bounds;
              selected = maps;
              
              for (var x = 0; x < maps.length; x++) {
                  var latlng1 = new google.maps.LatLng(maps[x].lat, maps[x].lng);
                  var LowestFarePrice = "N/A";

                  if (maps[x].LowestFare != "N/A") {
                      LowestFarePrice = maps[x].LowestFare.toFixed(2);
                      if (LowestFarePrice == 0) 
                          LowestFarePrice = "N/A";
                      LowestFarePrice = $filter('currency')(LowestFarePrice, maps[x].CurrencyCode +' ', 0)
                  }
                  var airportName = _.find($scope.airportlist, function (airport) {
                      return airport.airport_Code == maps[x].DestinationLocation
                  });
                  var marker = new MarkerWithLabel({
                      position: latlng1,
                      map: $scope.destinationMap,
                      title: airportName.airport_FullName,
                      labelContent: LowestFarePrice + ' <br/>' + maps[x].DestinationLocation + ' [ ' + airportName.airport_CityName + ' ]',
                      labelAnchor: new google.maps.Point(12, 35),
                      labelClass: "labelscolor", // the CSS class for the label
                      labelInBackground: false,
                      labelanimation: google.maps.Animation.DROP,
                      animation: google.maps.Animation.DROP,
                      CustomMarkerInfo: maps[x],
                      labelStyle: { opacity: 1 },
                      //icon: 'http://demo.crackerworld.in/images/map-blues.png',
                      icon: 'app/Styles/images/mapicon.png'
                  });
                  bounds.extend(marker.position);
                  $scope.bounds.extend(marker.position);
                 

                  var contentString = '<div style="min-width:100px;padding-top:5px;" id="content">' +
                                          '<div class="col-sm-12 padleft0"><strong>'
                                            + "(" + maps[x].DestinationLocation + ") " + airportName.airport_FullName + ', ' + airportName.airport_CityName + '</strong></div>' +
                                            '<br /><div class="col-sm-12 padleft0">' +
                                            '<label>Lowest fare: </label><strong class="text-success"> ' + maps[x].CurrencyCode + ' ' + maps[x].LowestFare + '</strong>' +
                                    '</div> ';

                  $scope.InfoWindow = new google.maps.InfoWindow();
                  var mapsdetails = maps[x];

                  google.maps.event.addListener(marker, 'click', (function (marker, contentString, infowindow) {
                      return function () {
                          var OriginairportName = _.find($scope.airportlist, function (airport) {
                              return airport.airport_Code == $scope.$parent.Origin.toUpperCase()
                          });
                          var DestinationairportName = _.find($scope.airportlist, function (airport) {
                              return airport.airport_Code == marker.CustomMarkerInfo.DestinationLocation
                          });

                          var dataForecast = {
                              "Origin": $scope.$parent.Origin,
                              "DepartureDate": $filter('date')(marker.CustomMarkerInfo.DepartureDateTime, 'yyyy-MM-dd'),
                              "ReturnDate": $filter('date')(marker.CustomMarkerInfo.ReturnDateTime, 'yyyy-MM-dd'),
                              "Destination": marker.CustomMarkerInfo.DestinationLocation
                          };
                          
                          $rootScope.$broadcast('EmptyFareForcastInfo', {
                              Origin: OriginairportName.airport_CityName,
                              Destinatrion: DestinationairportName.airport_Code,
                              Fareforecastdata: dataForecast,
                              mapOptions: marker.CustomMarkerInfo, //mapsdetails,
                              OriginairportName: OriginairportName,
                              DestinationairportName: DestinationairportName,
                              DestinationList: $scope.destinations,
                              AvailableAirports: $scope.airportlist,
                              //tabIndex : 999  // used for popup
                          });
                      };
                  })(marker, contentString, $scope.InfoWindow));
                  $scope.myMarkers.push(marker);
              }
          };

          var getMapUrlData = function (airportCode) {
              debugger;
              var d = $q.defer();
              var originairport = _.find($scope.airportlist, function (airport) {
                  return airport.airport_Code == airportCode.DestinationLocation
              });

              if (originairport != undefined) {
                  airportCode.lat = originairport.airport_Lat;
                  airportCode.lng = originairport.airport_Lng;
                  d.resolve(airportCode); // return the original object, so you can access it's other properties
              } else { d.resolve(); }
              return d.promise;
          }

          $scope.$on('eventDestinationMapresize', function (event, args) {
              google.maps.event.trigger($scope.destinationMap, 'resize');
          });

          $scope.resetMarker = function () {
              $timeout(function () {
                  $scope.destinationMap.setZoom(2);
                  var latlng = new google.maps.LatLng($scope.defaultlat, $scope.defaultlng);
                  $scope.destinationMap.setCenter(latlng);
              }, 0, false);
              if ($scope.myMarkers.length > 0) {
                  for (var i = 0; i < $scope.myMarkers.length; i++)
                      $scope.myMarkers[i].setMap(null);
                  $scope.myMarkers.length = 0;
              }
              $scope.myMarkers = [];
          }

          function serialize(obj) {
              var str = [];
              for (var p in obj)
                  if (obj.hasOwnProperty(p)) {
                      var propval = encodeURIComponent(obj[p]);
                      if (propval != "undefined" && propval != "null" && propval != '')
                          str.push(encodeURIComponent(p) + "=" + propval);
                  }
              return str.join("&");
          }

      }

      directive.link = function (scope, elm, attrs) {
          scope.$watchGroup(['destinations', 'airportlist'], function (newValues, oldValues, scope) {
              scope.resetMarker();
              if (scope.destinations != undefined && scope.destinations.length > 0) {
                  scope.displayDestinations(scope.btntext, scope.destinations);
                  scope.airportlist;
              }
          });
      }
      return directive;
  });
