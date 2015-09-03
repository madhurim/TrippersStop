angular.module('TrippismUIApp').directive('googleattractionInfo',
                                            ['$compile',
                                                '$q',
                                                '$rootScope',
                                                'GoogleAttractionFactory',
                                                '$timeout',
                                                '$modal',
                                                     '$sce',
                                                'TrippismConstants',
    function ($compile, $q, $rootScope, GoogleAttractionFactory, $timeout, $modal, $sce, TrippismConstants) {
        return {
            restrict: 'E',
            scope: { googleattractionParams: '=', isOpen: '=' },
            templateUrl: '/app/Views/Partials/GoogleAttractionPartial.html',
            controller: function ($scope) {

                $scope.DisplayattractionsInfo = false;

                $scope.googleMapId = "googleMapId_" + $scope.googleattractionParams.tabIndex;
                $scope.gMapId = "gMapId_" + $scope.googleattractionParams.tabIndex;

                $scope.RenderMap = RenderMap;
                $scope.googleattractionsMap = undefined;
                $scope.AttractionMarkers = [];
                $scope.bounds = new google.maps.LatLngBounds();
                $scope.MapLoaded = false;

                $scope.$on('ontabClicked', function (event, args) {
                    if (args == $scope.googleattractionParams.tabIndex) {
                        if ($scope.MapLoaded) {
                            $timeout(function () {
                                if ($scope.InfoWindow) $scope.InfoWindow.close();
                                $scope.FittoScreen();
                            }, 100, false);
                        }
                        else
                            $scope.loadgoogleattractionInfo();
                    }
                });


                $scope.$on('onMarkerPopup', function (event, args) {
                    if (args.tabIndex == $scope.googleattractionParams.tabIndex)
                        SetMarkerSlider(args.place)
                });


                function SetMarkerSlider(MapDet) {
                    $scope.slides = [];
                    $scope.IsMarkerSelected = false;
                    $scope.IsMapPopupLoading = true;
                    $scope.PhoneNo = "";
                    $scope.raitingToAppend = "";
                    $scope.PlaceName = "";
                    $scope.Placeaddress = "";
                    var service = new google.maps.places.PlacesService($scope.googleattractionsMap);
                    var request = { placeId: MapDet.place_id };
                    $scope.SelectedPlaceId = MapDet.place_id;
                    service.getDetails(request, function (place, status) {
                        $scope.IsMapPopupLoading = false;
                        if (status == google.maps.places.PlacesServiceStatus.OK) {

                            // Multi photo
                            if (place.photos != null && place.photos.length > 0) {
                                var photos = [];
                                for (var photoidx = 0; photoidx < place.photos.length; photoidx++) {
                                    //var Imgsrc = place.photos[photoidx].getUrl({ 'maxWidth': 440, 'maxHeight': 270 });
                                    var Imgsrc = place.photos[photoidx].getUrl({ 'maxWidth': 570, 'maxHeight': 400 });
                                    var objtopush = { image: Imgsrc, text: "" };
                                    photos.push(objtopush);
                                }
                                $scope.addSlides(photos);
                            }
                            $scope.PlaceName = place.name;
                            $scope.Placeaddress = $sce.trustAsHtml(place.adr_address);
                            if (place.formatted_phone_number != undefined)
                                $scope.PhoneNo = place.formatted_phone_number;

                            $scope.raitingToAppend = "";
                            if (place.rating != undefined)
                                $scope.raitingToAppend = $sce.trustAsHtml(getRatings(place.rating));

                            $scope.IsMapPopupLoading = false;
                        }

                    });

                    var mapheight = $('#' + $scope.gMapId).height() - 400;
                    var mapWidth = $('#' + $scope.gMapId).width() - 600;

                    $("#" + $scope.googleMapId).css('top', '-25px');
                    $("#" + $scope.googleMapId).css('left', mapWidth / 2);

                    $scope.IsMarkerSelected = true;

                }


                $scope.FittoScreen = function () {
                    google.maps.event.trigger($scope.googleattractionsMap, 'resize');
                    var latlng = new google.maps.LatLng($scope.googleattractionParams.DestinationairportName.airport_Lat, $scope.googleattractionParams.DestinationairportName.airport_Lng);
                    $scope.googleattractionsMap.setCenter(latlng);
                    $scope.googleattractionsMap.fitBounds($scope.bounds);
                };

                var mapStyle = TrippismConstants.attractionTabMapStyle;

                $scope.attractionmapOptions = {
                    center: new google.maps.LatLng(0, 0),
                    zoom: 2,
                    minZoom: 2,
                    styles: mapStyle,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };

                var mapid = angular.element(document.querySelector('.map-canvas'));

                if ($rootScope.mapHeight == undefined) {
                    $rootScope.mapHeight = $(window).height() - 350;
                    $(".map-canvas").css("height", $rootScope.mapHeight + 'px');
                } else {
                    $('.map-canvas').each(function (i, obj) {
                        $(this).css('height', $rootScope.mapHeight);
                    });
                }



                $timeout(function () {
                    google.maps.event.trigger($scope.googleattractionsMap, 'resize');
                }, 1000, false);

                $scope.GoogleAttractionDisplay = function () {
                    $scope.quantity = 20;
                };
                $scope.$watchGroup(['googleattractionParams', 'isOpen'], function (newValue, oldValue, $scope) {
                    $scope.loadgoogleattractionInfo();
                    if ($scope.googleattractionsMap != undefined) {
                        $timeout(function () {
                            $scope.FittoScreen();
                        }, 1000, false);
                    }
                });

                $scope.loadgoogleattractionInfo = function () {
                    if (!$scope.MapLoaded) {
                        $scope.googleattractionInfoLoaded = false;
                        $scope.googleattractionInfoNoDataFound = false;
                        $scope.googleattractionData = "";
                        $scope.attractionsplaces = [];
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
                                        $scope.MapLoaded = true;
                                        $scope.attractionsplaces = data.results;
                                        $scope.googleattractionData = data;
                                        $scope.quantity = 5;
                                        $scope.googleattractionInfoLoaded = true;
                                    });
                                }
                            }

                        }
                    }
                };


                $scope.cancel = function () {
                    $scope.IsMarkerSelected = false;
                };


                $scope.MaxRating = 5;
                function getRatings(num) {
                    var stars = [];
                    for (var i = 0; i < $scope.MaxRating; i++) {
                        stars.push({});
                    }
                    var starContainerMaxWidth = 100; //%
                    var filledInStarsContainerWidth = num / $scope.MaxRating * starContainerMaxWidth;

                    var ratingDiv = "<div class='average-rating-container'>";
                    if (stars.length > 0) {

                        ratingDiv += "<ul class='rating background' class='readonly'>";

                        for (var starIdx = 0; starIdx < stars.length; starIdx++)
                            ratingDiv += "<li class='star'><i class='fa fa-star'></i></li>";

                        ratingDiv += "</ul>";
                        ratingDiv += "<ul class='rating foreground readonly'  style='width:" + filledInStarsContainerWidth + "%'>";

                        for (var starIdx = 0; starIdx < stars.length; starIdx++)
                            ratingDiv += "<li class='star filled'><i class='fa fa-star'></i></li>";
                    }
                    ratingDiv += "  </ul></div>";
                    return ratingDiv;
                }

                $scope.IsMapPopupLoading = false;
                $scope.noWrapSlides = false;

                var slides = [];
                $scope.slides = [];

                $scope.addSlides = function (photos) {
                    for (var photoidex = 0; photoidex < photos.length; photoidex++)
                        $scope.slides.push(photos[photoidex]);
                    $scope.$apply();
                };
                $scope.SelectedPlaceId = "";
                function RenderMap(maps) {
                    if (maps != undefined && maps.length > 0) {

                        $scope.DisplayattractionsInfo = true;
                        $scope.InfoWindow;
                        selected = maps;
                        for (var x = 0; x < maps.length; x++) {
                            var icon = new google.maps.MarkerImage(
                                             maps[x].icon,
                                            new google.maps.Size(71, 71),
                                            new google.maps.Point(0, 0),
                                            new google.maps.Point(17, 34),
                                            new google.maps.Size(25, 25)
                                        );

                            var iconlatlng = new google.maps.LatLng(maps[x].geometry.location.lat, maps[x].geometry.location.lng);
                            var marker = new MarkerWithLabel({
                                position: iconlatlng,
                                map: $scope.googleattractionsMap,
                                title: '' + maps[x].name + '',
                                labelAnchor: new google.maps.Point(12, 35),
                                labelInBackground: false,
                                animation: google.maps.Animation.DROP,
                                CustomMarkerInfo: maps[x],
                                labelStyle: { opacity: 0.75 },
                                icon: icon,//'app/Styles/images/mapicon.png'
                            });

                            $scope.bounds.extend(marker.position);
                            var contentString = "";
                            $scope.InfoWindow = new google.maps.InfoWindow();

                            var MapDet = maps[x];

                            //google.maps.event.addListener(marker, 'mouseover', (function (marker, MapDet, x, contentString, $compile, infowindow, $scope) {
                            google.maps.event.addListener(marker, 'mouseover', (function (MapDet) {
                                return function () {
                                    SetMarkerSlider(MapDet);
                                };
                            })(MapDet));
                            //})(marker, MapDet, x, contentString, $compile, $scope.InfoWindow, $scope));
                            $scope.AttractionMarkers.push(marker);
                        }

                        google.maps.event.addListenerOnce($scope.googleattractionsMap, 'idle', function () {
                            $scope.FittoScreen();
                        });
                        $timeout(function () {
                            $scope.googleattractionsMap.setCenter(new google.maps.LatLng($scope.googleattractionParams.DestinationairportName.airport_Lat, $scope.googleattractionParams.DestinationairportName.airport_Lng));
                        }, 1000, false);
                    }
                };
            },
            link: function (scope, elem, attrs) {

            }
        }
    }]);
