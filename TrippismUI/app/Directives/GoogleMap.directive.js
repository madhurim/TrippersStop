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
              if (buttnText == 'All') {
                  $scope.faresList = angular.copy(destinations);
                  $scope.showPosition(_.uniq($scope.faresList, function (destination) { return destination.DestinationLocation; }))
              }

              if (buttnText == 'Cheapest') {
                  if (destinations.length > 0) {
                      var sortedObjs = _.filter(destinations, function (item) {
                          return item.LowestFare !== 'N/A';
                      });
                      sortedObjs = _(sortedObjs).sortBy(function (obj) { return parseInt(obj.LowestFare, 10) })
                      for (var i = 0; i < 10; i++)
                          if (sortedObjs[i] != undefined)
                              $scope.faresList.push(sortedObjs[i]);
                    
                  }
                  $scope.showPosition(_.uniq($scope.faresList, function (destination) { return destination.DestinationLocation; }))
              }
          }

        

          var RenderMap = function (maps) {
              $scope.InfoWindow;
            
              var bounds = new google.maps.LatLngBounds();
              selected = maps;
              for (var x = 0; x < maps.length; x++) {
                  var latlng1 = new google.maps.LatLng(maps[x].lat, maps[x].lng);


                  //var marker = new google.maps.Marker({
                  //    position: latlng1,
                  //    map: $scope.destinationMap,
                  //    title: maps[x].DestinationLocation,
                  //    icon: 'http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=' + (x + 1) + '|ff776b|000000',
                  //    animation: google.maps.Animation.DROP,
                  //    CustomMarkerInfo: maps[x],
                  //});
                  //createTooltip(marker);
                  
                  var marker = new MarkerWithLabel({
                      position: latlng1,
                      map: $scope.destinationMap,
                      title: maps[x].DestinationLocation,
                      //labelContent: maps[x].DestinationLocation,
                      labelAnchor: new google.maps.Point(12, 35),
                      labelClass: "labels", // the CSS class for the label
                      labelInBackground: false,
                      animation: google.maps.Animation.DROP,
                      CustomMarkerInfo: maps[x],
                      //labelStyle: { opacity: 0.75 },
                      //icon: 'https://mts.googleapis.com/vt/icon/name=icons/spotlight/spotlight-waypoint-a.png&text=' + maps[x].DestinationLocation + '&psize=11&font=fonts/Roboto-Regular.ttf&color=ff333333&ax=44&ay=48&scale=1',
                      icon: 'https://mts.googleapis.com/vt/icon/name=icons/spotlight/spotlight-waypoint-b.png&text=' + maps[x].DestinationLocation + '&psize=11&font=fonts/Roboto-Regular.ttf&color=ff000033&ax=44&ay=48&scale=1',
                  });
                  
                  
                  bounds.extend(marker.position);
                  
                  var airportName = _.find($scope.airportlist, function (airport) {
                      return airport.airport_Code == maps[x].DestinationLocation
                  });
               
                  var contentString = '<div style="min-width:100px;padding-top:5px;" id="content">' +
                                          '<div class="col-sm-12 padleft0"><strong>'
                                            + airportName.airport_FullName + ', ' + airportName.airport_CityName + '</strong></div>' +
                                            '<br /><div class="col-sm-6 padleft0">'+
                                            '<strong>'
                                            + maps[x].CurrencyCode  +' '+ maps[x].LowestFare + '</strong>' +
                                            
                 '</div> ';

                  $scope.InfoWindow = new google.maps.InfoWindow();
                  var mapsdetails = maps[x];

                  google.maps.event.addListener(marker, 'mouseover', (function (marker, contentString, infowindow) {
                      return function () {
                          if ($scope.InfoWindow) $scope.InfoWindow.close();
                          $scope.InfoWindow = new google.maps.InfoWindow({ content: contentString, maxWidth: 500 });
                          $scope.InfoWindow.open($scope.destinationMap, marker);
                      };
                  })(marker, contentString, $scope.InfoWindow));
                  

                  google.maps.event.addListener(marker, 'mouseout', function () {
                      
                  });

                  google.maps.event.addListener(marker, 'click', (function (marker, contentString, infowindow) {
                      
                      return function () {
                          // Play Video on MArker click
                         // $scope.$parent.YouTubePlayer.playVideo();

                          if ($scope.InfoWindow) $scope.InfoWindow.close(); 
                          $scope.InfoWindow = new google.maps.InfoWindow({ content: contentString, maxWidth: 500 });

                          //google.maps.event.addListener($scope.InfoWindow, 'closeclick', function () {                           
                          //    $rootScope.$broadcast('CloseFareForcastInfo');
                          //});

                          $scope.InfoWindow.open($scope.destinationMap, marker);

                          var OriginairportName = _.find($scope.airportlist, function (airport) {
                              return airport.airport_Code == $scope.$parent.Origin
                          });

                          var DestinationairportName = _.find($scope.airportlist, function (airport) {
                              return airport.airport_Code == marker.CustomMarkerInfo.DestinationLocation
                          });
                          
                          var dataForecast = {
                              "Origin": $scope.$parent.Origin,
                              "DepartureDate": ConvertToRequiredDate(marker.CustomMarkerInfo.DepartureDateTime),
                              "ReturnDate": ConvertToRequiredDate(marker.CustomMarkerInfo.ReturnDateTime),
                              "Destination": marker.CustomMarkerInfo.DestinationLocation
                          };

                          $rootScope.$broadcast('EmptyFareForcastInfo', {
                              Origin: OriginairportName.airport_CityName,
                              Destinatrion: DestinationairportName.airport_CityName,
                              Fareforecastdata: dataForecast,
                              mapOptions: marker.CustomMarkerInfo, //mapsdetails,
                              OriginairportName : OriginairportName,
                              DestinationairportName: DestinationairportName,

                          });
                     };
                  })(marker, contentString, $scope.InfoWindow));

                  $scope.myMarkers.push(marker);
              }         
          };
          
          var getMapUrlData = function (airportCode) {             
              var d = $q.defer();
              var originairport = _.find($scope.airportlist, function (airport) {
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

          $scope.resetMarker = function () {            
              $timeout(function () {              
                  $scope.destinationMap.setZoom(2);
                  var latlng = new google.maps.LatLng($scope.defaultlat, $scope.defaultlng);
                  $scope.destinationMap.setCenter(latlng);
              }, 0,false);             
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
          scope.$watchGroup(['btntext', 'destinations', 'airportlist'], function (newValues, oldValues, scope) {
              scope.resetMarker();
              if (scope.destinations!=undefined && scope.destinations.length > 0) {
                  scope.displayDestinations(scope.btntext, scope.destinations);
                  scope.airportlist;
              }
              
          });
      }

      return directive;
  });
