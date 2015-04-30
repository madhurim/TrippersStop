
(function () {
    'use strict';
    var controllerId = 'DestinationController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope','$rootScope','DestinationFactory', DestinationController]);
    
    function DestinationController($scope, $rootScope, DestinationFactory) {
        
        $scope.Destination = 'bos';
        $scope.FromDate = '2015-04-01';
        $scope.ToDate = '2015-04-29';
        $scope.faresList = [];
        $rootScope.apiURL = 'http://localhost:14606';
        $scope.getDestinations = findDestinations;

        function findDestinations() {
            var data = {
                "Origin": $scope.Destination,
                "DepartureDate": $scope.FromDate,
                "ReturnDate": $scope.ToDate,
                "Lengthofstay" : '4'
            };
            
            DestinationFactory.findDestinations(data).then(function (data) {

                $scope.faresList = angular.copy(data.FareInfo);
            
            });
        }

    
    }
    
 
    



  
})();


