
angular.module('TrippismUIApp').directive('attractionList', ['$compile', '$sce', '$rootScope', function ($compile, $sce, $rootScope) {
    return {
        restrict: 'E',
        scope: {
            attractions: '=',
            attractionmap: '=',
            attractiontabindex : '='
        },
        templateUrl: '/app/Views/Partials/attractionsList.html',
        link: function (scope, elem, attrs) {
            
            scope.showmore = true;
            scope.attractionstoDisp = [];
            scope.attractionsCnt = 0;

            function getAttractionsList(index) {
                if (scope.attractions != undefined && scope.attractions.length > 0) {
                    if (scope.attractionsCnt + 10 >= scope.attractions.length)
                        scope.showmore = false;

                    for (var i = index; i < index + 10 ; i++) {
                        var MapDet = scope.attractions[i];
                        if (MapDet != undefined) {
                            var request = { placeId: MapDet.place_id };
                            scope.service.getDetails(request, function (place, status) {
                                var PhoneNo = "";
                                if (place.formatted_phone_number != undefined)
                                    PhoneNo = place.formatted_phone_number;
                                var placedetails = {
                                    name: place.name,
                                    Placeaddress: $sce.trustAsHtml(place.adr_address),
                                    PhoneNo: PhoneNo,
                                    place_id: place.place_id
                                };
                                scope.attractionstoDisp.push(placedetails);
                            });
                        }
                    }
                }
            }

            scope.SelectPlace = function (place) {
                var sliderdata = {
                    tabIndex: scope.attractiontabindex,
                    place : place
                };
                $rootScope.$broadcast('onMarkerPopup', sliderdata);
            };

            

            scope.$watch('attractions', function (newValue, oldValue) {
                if (newValue != oldValue) {
                    scope.service = new google.maps.places.PlacesService(scope.attractionmap);
                    getAttractionsList(scope.attractionsCnt);
                }
            });

            scope.ShowMoreAttractions = function () {
                scope.attractionsCnt += 10;
                getAttractionsList(scope.attractionsCnt);
                if (scope.attractionsCnt + 10 >= scope.attractions.length)
                    scope.showmore = false;
            }
        }
    }
}]);
