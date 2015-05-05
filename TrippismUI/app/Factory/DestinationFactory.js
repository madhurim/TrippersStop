(function () {
    'use strict';
    var serviceId = 'DestinationFactory';
    angular.module('TrippismUIApp').factory(serviceId, ['$http', '$rootScope', DestinationFactory]);

    function DestinationFactory($http, $rootScope) {
        
        // Define the functions and properties to reveal.
        var service = {
            findDestinations: findDestinations,
            fareforecast: fareforecast,
        };
        return service;

        function findDestinations(data) {
            
            var url = $rootScope.apiURL + '/api/Destinations/Get?Origin=' + data.Origin + '&DepartureDate='
                + data.DepartureDate + '&ReturnDate=' + data.ReturnDate + '&Lengthofstay=' + data.Lengthofstay;
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
            var url = $rootScope.apiURL + '/api/FareForecast/Get?Origin=' + data.Origin + '&DepartureDate='
             + data.EarliestDepartureDate + '&ReturnDate=' + data.LatestDepartureDate + '&Destination=' + data.Destination ;
            return $http.get(url)
                .then(function (data) {
                    return data.data;
                }, function (e) {
                    return e;
                });
        }
    }
})();