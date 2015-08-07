(function () {
    'use strict';
    var serviceId = 'GoogleAttractionFactory';
    angular.module('TrippismUIApp').factory(serviceId, ['$http', '$rootScope', GoogleAttractionFactory]);

    function GoogleAttractionFactory($http, $rootScope) {
        // Define the functions and properties to reveal.
        var service = {
            googleAttraction: googleAttraction,
            getPlaceDetails: getPlaceDetails
        };
        return service;

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

        function getPlaceDetails(placeid, map) {
            var service = new google.maps.places.PlacesService(map);
            var request = { placeId: placeid };
            var sss=  service.getDetails(request, function (place, status) {});
            debugger;
            //var url = 'https://maps.googleapis.com/maps/api/place/details/json?placeid="'+ placeid +'"&key=AIzaSyAQUUoKix1RYuUSlnQHdCG0mFGOSC29vGk';
            //return $http.get(url)
            //   .then(function (data) {
            //       return data.data;
            //   }, function (e) {
            //       return e;
            //   });
        }
        function googleAttraction(data) {
            var testURL = 'locationsearch?' + serialize(data);
            var url = $rootScope.apiURLForGoogleAttraction + testURL;

            return $http.get(url)
                .then(function (data) {
                    return data.data;
                }, function (e) {
                    return e;
                });
        }
    }
})();