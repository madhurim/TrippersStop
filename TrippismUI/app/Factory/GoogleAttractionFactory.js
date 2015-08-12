(function () {
    'use strict';
    var serviceId = 'GoogleAttractionFactory';
    angular.module('TrippismUIApp').factory(serviceId, ['$http', '$rootScope', GoogleAttractionFactory]);

    function GoogleAttractionFactory($http, $rootScope) {
        // Define the functions and properties to reveal.
        var service = {
            googleAttraction: googleAttraction,
           // getPlaceDetails: getPlaceDetails
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
