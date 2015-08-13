angular.module('TrippismUIApp').directive('attractiontabFareForecast', ['$compile', '$modal', 'FareforecastFactory',
    function ($compile,$modal, FareforecastFactory) {
    return {
        restrict: 'E',
        scope: {
            attractionParams: '=',
        },
        templateUrl: '/app/Views/Partials/attractiontabFareForecastPartial.html',
        link: function (scope, elem, attrs) {
            
            scope.SendEmailToUser = SendEmailToUser;
            function SendEmailToUser() {
                var GetEmailDetPopupInstance = $modal.open({
                    templateUrl: '/app/Views/Partials/EmailDetFormPartial.html', 
                    controller: 'EmailForDestinationDet',
                    scope: scope,
                    resolve: {
                        seasonalityData: function () { return scope.attractionParams; }
                    }
                });
            }

            function activate() {
                scope.FareNoDataFound = true;
                scope.FareforecastData = "";
                scope.IsRequestCompleted = false;
                scope.fareinfopromise = FareforecastFactory.fareforecast(scope.attractionParams.Fareforecastdata).then(function (data) {
                    scope.IsRequestCompleted = true;
                    scope.inProgressFareinfo = false;
                    if (data.status == 404) {
                        scope.FareNoDataFound = true;
                        return;
                    }
                    scope.FareNoDataFound = false;
                    scope.FareforecastData = data;
                    // Setting up fare data for email
                    scope.attractionParams.dataforEmail.FareForecastDataForEmail = {};
                    scope.attractionParams.dataforEmail.FareForecastDataForEmail =data;
                    
                });
            };

            scope.$watch('attractionParams',
             function (newValue, oldValue) {
                     activate();
             }
           );
        }
    }
}]);
