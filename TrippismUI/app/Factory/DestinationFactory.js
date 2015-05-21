(function () {
    'use strict';
    var serviceId = 'DestinationFactory';
    angular.module('TrippismUIApp').factory(serviceId, ['$http', '$rootScope', DestinationFactory]);

    function DestinationFactory($http, $rootScope) {

        // Define the functions and properties to reveal.
        var service = {
            findDestinations: findDestinations,
            fareforecast: fareforecast,
            SeasonalityHistorySearch: SeasonalityHistorySearch,
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
        function findDestinations(data) {
            var testURL = 'Destinations?' + serialize(data);

            var RequestedURL = $rootScope.apiURL + testURL;
            return $http.get(RequestedURL)
            .then(function (data) {
                return data.data;
            }, function (e) {
                return e;
            });
        }
        function SeasonalityHistorySearch(searchdata) {


            var url = $rootScope.apiURL + 'Seasonality?' +
             'Destination=' + searchdata.Destination;
            return $http.get(url)
                .then(function (data) {
                    return data.data;
                }, function (e) {
                    return e;
                });
        }

        function fareforecast(data) {
            //var url = $rootScope.apiURL + '/api/FareForecast/Get?Origin=' + data.Origin + '&EarliestDepartureDate='
            // + data.EarliestDepartureDate + '&LatestDepartureDate=' + data.LatestDepartureDate + '&Destination=' + data.Destination + '&lengthofstay=' + data.LengthOfStay ;
            var url = $rootScope.apiURL + 'FareForecast/Get?Origin=' + data.Origin + '&DepartureDate='
             + data.EarliestDepartureDate + '&ReturnDate=' + data.LatestDepartureDate + '&Destination=' + data.Destination;
            return $http.get(url)
                .then(function (data) {
                    return data.data;
                }, function (e) {
                    return e;
                });
        }
    }
})();