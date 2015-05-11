(function () {
    'use strict';
    var serviceId = 'FareforecastFactory';
    angular.module('TrippismUIApp').factory(serviceId, ['$http', '$rootScope', FareforecastFactory]);

    function FareforecastFactory($http, $rootScope) {
        // Define the functions and properties to reveal.
        var service = {
            fareforecast: fareforecast,
        };
        return service;

        function fareforecast(data) {
            var url = $rootScope.apiURL + 'FareForecast?Origin=' + data.Origin + '&DepartureDate='
             + data.DepartureDate + '&ReturnDate=' + data.ReturnDate + '&Destination=' + data.Destination;
            return $http.get(url)
                .then(function (data) {
                    return data.data;
                }, function (e) {
                    return e;
                });
        }
    }
})();