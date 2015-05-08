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
                if(data.Origin == undefined) 
                    data.Origin ="";

                if(data.DepartureDate == undefined) 
                    data.DepartureDate ="";
               
                if(data.ReturnDate == undefined) 
                    data.ReturnDate ="";

                if(data.Lengthofstay == undefined) 
                    data.Lengthofstay ="";
               
                if(data.Earliestdeparturedate == undefined) 
                    data.Earliestdeparturedate ="";
               
                if(data.Latestdeparturedate == undefined) 
                    data.Latestdeparturedate ="";

                if(data.Theme == undefined) 
                    data.Theme ="";

                if(data.Location == undefined) 
                    data.Location ="";

                if(data.Minfare == undefined) 
                    data.Minfare ="";
               
                if(data.Maxfare == undefined) 
                    data.Maxfare ="";
               
                if(data.PointOfSaleCountry == undefined) 
                    data.PointOfSaleCountry ="";
   
                if(data.Region == undefined) 
                    data.Region ="";
               
                if(data.TopDestinations == undefined) 
                    data.TopDestinations ="";
   
                var salescountry = 'US';
                var url = $rootScope.apiURL + '/api/Destinations?' +
                 'Origin=' + data.Origin +
                 //'&Destination=' + data.Destination +
                 '&departuredate=' + data.DepartureDate +
                 '&ReturnDate=' + data.ReturnDate +
                 '&Lengthofstay=' + data.Lengthofstay +
                 '&Latestdeparturedate=' + data.Latestdeparturedate +
                 '&Theme=' + data.Theme +
                 '&Location=' + data.Location +
                 '&Minfare=' + data.Minfare +
                 '&Maxfare=' + data.Maxfare +
                 
                 '&Region=' + data.Region +
                 '&TopDestinations=' + data.TopDestinations +
                 '&Earliestdeparturedate=' + data.Earliestdeparturedate + '&PointOfSaleCountry=US&ac2lonlat=1';
                //'&PointOfSaleCountry=' + data.PointOfSaleCountry +
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