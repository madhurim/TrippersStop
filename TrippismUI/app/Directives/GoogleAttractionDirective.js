﻿angular.module('TrippismUIApp').directive('googleattractionInfo',
                                            [   '$compile',
                                                '$q',
                                                'GoogleAttractionFactory','$timeout',
    function ($compile, $q, GoogleAttractionFactory, $timeout) {
        return {
            restrict: 'E',
            scope: { googleattractionParams: '=', isOpen: '=' },
            templateUrl: '/app/Views/Partials/GoogleAttractionPartial.html',
            controller: function ($scope) {
                $scope.RenderMap = RenderMap;
                $scope.googleattractionsMap = undefined;
                $scope.AttractionMarkers = [];
                $scope.bounds = new google.maps.LatLngBounds();

                $scope.FittoScreen = function () {
                    google.maps.event.trigger($scope.googleattractionsMap, 'resize');
                    var latlng = google.maps.LatLng($scope.googleattractionParams.DestinationairportName.airport_Lat, $scope.googleattractionParams.DestinationairportName.airport_Lng);
                    $scope.googleattractionsMap.setCenter(latlng);
                    $scope.googleattractionsMap.fitBounds($scope.bounds);
                };

                $scope.attractionmapOptions = {
                    center: new google.maps.LatLng(0, 0),
                    zoom: 2,
                    minZoom: 2,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };
                $scope.GoogleAttractionDisplay = function () {
                    $scope.quantity = 20;
                };
                $scope.$watchGroup(['googleattractionParams', 'isOpen'], function (newValue, oldValue, $scope) {
                    $scope.loadgoogleattractionInfo();
                    if ($scope.googleattractionsMap != undefined) 
                        $scope.FittoScreen();
                });

                $scope.loadgoogleattractionInfo = function () {
                    $scope.googleattractionInfoLoaded = false;
                    $scope.googleattractionInfoNoDataFound = false;
                    $scope.googleattractionData = "";
                    if ($scope.googleattractionParams != undefined) {
                        var data = {
                            "Latitude": $scope.googleattractionParams.DestinationairportName.airport_Lat,//$scope.googleattractionParams.airport_Lat,
                            "Longitude": $scope.googleattractionParams.DestinationairportName.airport_Lng //$scope.googleattractionParams.airport_Lng
                        };
                        if ($scope.googleattractionInfoLoaded == false) {

                            $scope.attractionmapOptions = {
                                center: new google.maps.LatLng($scope.googleattractionParams.DestinationairportName.airport_Lat, $scope.googleattractionParams.DestinationairportName.airport_Lng),
                                zoom: 1,
                                minZoom: 10,
                                backgroundColor: "#BCCFDE",
                                mapTypeId: google.maps.MapTypeId.ROADMAP
                            };

                            if ($scope.googleattractionData == "") {
                                $scope.googleattractionpromise = GoogleAttractionFactory.googleAttraction(data).then(function (data) {
                                    if (data.status == 404) {
                                        $scope.googleattractionInfoNoDataFound = true;
                                        return;
                                    }
                                    RenderMap(data.results);
                                    $scope.googleattractionData = data;
                                    $scope.quantity = 5;
                                    $scope.googleattractionInfoLoaded = true;
                                });
                            }
                        }

                    }
                };
                
                function RenderMap(maps) {
                    console.log($scope.AttractionMarkers);
                    if (maps != undefined && maps.length > 0) {
                        $scope.InfoWindow;
                        selected = maps;
                        for (var x = 0; x < maps.length; x++) {
                            var latlng1 = new google.maps.LatLng(maps[x].geometry.location.lat, maps[x].geometry.location.lng);
                            var marker = new MarkerWithLabel({
                                position: latlng1,
                                map: $scope.googleattractionsMap,
                                title: '' + maps[x].name + '',
                                labelAnchor: new google.maps.Point(12, 35),
                                labelInBackground: false,
                                animation: google.maps.Animation.DROP,
                                CustomMarkerInfo: maps[x],
                                labelStyle: { opacity: 0.75 },
                                //icon: 'https://mts.googleapis.com/vt/icon/name=icons/spotlight/spotlight-waypoint-b.png&text=' + maps[x].name + '&psize=8&font=fonts/Roboto-Regular.ttf&color=ff000033&ax=44&ay=48&scale=1',
                            });

                            $scope.bounds.extend(marker.position);

                            var contentString = '<div style="min-width:100px;padding-top:5px;" id="content">' +
                                             '<div class="col-sm-12 padleft0"><strong>'
                                               + maps[x].name + '</strong></div>' +
                                               '<br /><div class="col-sm-12 padleft0 word-wrap">' + maps[x].vicinity + '</div>' +
                                       '</div> ';

                            $scope.InfoWindow = new google.maps.InfoWindow();

                            google.maps.event.addListener(marker, 'mouseover', (function (marker, contentString, infowindow) {
                                return function () {
                                    if ($scope.InfoWindow) $scope.InfoWindow.close();
                                    $scope.InfoWindow = new google.maps.InfoWindow({ content: contentString, maxWidth: 500 });
                                    $scope.InfoWindow.open($scope.googleattractionsMap, marker);
                                };
                            })(marker, contentString, $scope.InfoWindow));
                            $scope.AttractionMarkers.push(marker);
                        }
                        $timeout(function () {
                            $scope.FittoScreen();
                        }, 1000, false);
                        
                    }
                };


            },
            link: function (scope, elem, attrs) {

            }
        }
    }]);


