
(function () {
    'use strict';
    var controllerId = 'DestinationController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', '$rootScope', '$http', '$q', 'blockUIConfig', '$location', '$anchorScroll', 'DestinationFactory', 'UtilFactory', DestinationController]);

    function DestinationController($scope, $rootScope, $http, $q, blockUIConfig, $location, $anchorScroll, DestinationFactory, UtilFactory) {

        $scope.hasError = false;
        $scope.Location = "";
        $scope.AvailableCodes = [];
        $scope.formats = Dateformat();
        $scope.format = $scope.formats[5];
        $scope.activate = activate;
        $rootScope.apiURL = 'http://localhost:14606/sabre/api/';
        $scope.findDestinations = findDestinations;
        $scope.Origin = '';
        $scope.Destination = '';
        $scope.buttontext = "All";
        $scope.AvailableAirports = [];
        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.AvailableThemes = AvailableTheme();
        $scope.AvailableRegions = AvailableRegions();

        $scope.MaximumFromDate = ConvertToRequiredDate(common.addDays(new Date(), 192));
        $scope.LoadingText = "Loading..";
        $scope.SearchbuttonText = "Get Destinations";
       // $scope.SearchbuttonTo10Text = "Top 10";
        $scope.SearchbuttonCheapestText = "Top 10 Cheapest";
        $scope.SearchbuttonIsLoading = false;
       // $scope.SearchbuttonTop10IsLoading = false;
        $scope.SearchbuttonChepestIsLoading = false;

        var dt = new Date();
        dt.setHours(0, 0, 0, 0)
        var Todt = new Date();
        Todt.setDate(Todt.getDate() + 5); // add default from 5 days
        Todt.setHours(0, 0, 0, 0)

        $scope.ToDate = ConvertToRequiredDate(Todt);
        $scope.FromDate = ConvertToRequiredDate(dt);

        $scope.minTodayDate = new Date();
        $scope.minFromDate = new Date();
        $scope.minFromDate = $scope.minFromDate.setDate($scope.minFromDate.getDate() + 1);

        $scope.ToDate = ConvertToRequiredDate(Todt);
        $scope.FromDate = ConvertToRequiredDate(dt);


        $scope.$watch(function (scope) { return scope.Earliestdeparturedate },
           function (newValue, oldValue) {
               if (newValue == null)
                   return;

               /* If from date is greater than to date */
               var newDt = new Date(newValue);
               newDt.setHours(0, 0, 0, 0);
               var todate = ($scope.Latestdeparturedate == undefined || $scope.Latestdeparturedate == '') ? new Date() : new Date($scope.Latestdeparturedate);
               todate.setHours(0, 0, 0, 0);

               if (newDt >= todate) {
                   $scope.Latestdeparturedate = ConvertToRequiredDate(newDt.setDate(newDt.getDate() + 1))
               }
               /**/

               // Calculate datediff
               var diff = daydiff(new Date(newValue).setHours(0, 0, 0, 0), new Date($scope.Latestdeparturedate).setHours(0, 0, 0, 0));
               if (diff > 30)
                   $scope.Latestdeparturedate = ConvertToRequiredDate(common.addDays(newDt, 30));

               $scope.MaximumLatestdeparturedate = common.addDays(newDt, 30);
           }
        );


        $scope.$watch(function (scope) { return scope.FromDate },
              function (newValue, oldValue) {

                  if (newValue == null)
                      return;

                  /* If from date is greater than to date */
                  var newDt = new Date(newValue);
                  newDt.setHours(0, 0, 0, 0);
                  var todate = new Date($scope.ToDate);
                  todate.setHours(0, 0, 0, 0);

                  if (newDt >= todate) {
                      $scope.ToDate = ConvertToRequiredDate(newDt.setDate(newDt.getDate() + 1))
                  }
                  /**/

                  //SET MINIMUN SELECTED DATE for TODATE
                  $scope.minFromDate = new Date(newValue);
                  $scope.minFromDate = $scope.minFromDate.setDate($scope.minFromDate.getDate() + 1);
                  $scope.MaximumToDate = common.addDays($scope.minFromDate, 16);

              }
       );

        $scope.$watch('IsAdvancedSearch', function (scope) {
            if (scope == false || scope == undefined) {
                $scope.Theme = "";
                $scope.Region = "";
                $scope.Minfare = "";
                $scope.Maxfare = "";
                $scope.TopDestinations = "";
                $scope.Earliestdeparturedate = "";
                $scope.Latestdeparturedate = "";
                $scope.LenghtOfStay = "";
            }
        });

        function activate() {
            UtilFactory.ReadAirportJson().then(function (data) {
                $scope.AvailableAirports = data;
                $scope.AvailableCodes = angular.copy($scope.AvailableAirports);
                UtilFactory.getIpinfo($scope.AvailableAirports).then(function (data) {
                    $scope.Origin = data.airport_Code;
                    $scope.CalledOnPageLoad = true;
                    $scope.findDestinations('Cheapest');
                });
                UtilFactory.MapscrollTo('wrapper');
            });
        }

        activate();

        $scope.onSelect = function ($item, $model, $label) {
            $scope.Origin = $item.airport_Code;
        };

        $scope.formatInput = function ($model) {
            if ($model == "" || $model == undefined) return "";

            var originairport = _.find($scope.AvailableAirports, function (airport) { return airport.airport_Code == $model });
            var airportname = (originairport.airport_FullName.toLowerCase().indexOf("airport") > 0) ? originairport.airport_FullName : originairport.airport_FullName + " Airport";
            var CountryName = (originairport.airport_CountryName != undefined) ? originairport.airport_CountryName : "";
            return originairport.airport_Code + ", " + airportname + ", " + originairport.airport_CityName + ", " + CountryName;
        }

        $scope.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.opened = true;
        };

        $scope.openEarliestdeparturedate = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.openedEarliestdeparturedate = true;
        };

        $scope.openLatestdeparturedate = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.openedLatestdeparturedate = true;
        };

        $scope.openFromDate = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.openedFromDate = true;
        };

        function findDestinations(buttnText) {
            if ($scope.CalledOnPageLoad != true) {
                if ($scope.frmdestfinder.$invalid) {
                    $scope.hasError = true;
                    return;
                }
            }
            $scope.destinationlist = "";
            $scope.faresList = [];
            $scope.IsHistoricalInfo = false;
            if (buttnText == 'All') { $scope.SearchbuttonIsLoading = true; $scope.SearchbuttonText = $scope.LoadingText; }
            else if (buttnText == 'Cheapest') { $scope.SearchbuttonChepestIsLoading = true; $scope.SearchbuttonCheapestText = $scope.LoadingText; }
            var data = {
                "Origin": $scope.Origin,
                "DepartureDate": ($scope.FromDate == '' || $scope.FromDate == undefined) ? null : ConvertToRequiredDate($scope.FromDate),
                "ReturnDate": ($scope.ToDate == '' || $scope.ToDate == undefined) ? null : ConvertToRequiredDate($scope.ToDate),
                "Lengthofstay": $scope.LenghtOfStay,
                "Earliestdeparturedate": ($scope.Earliestdeparturedate == '' || $scope.Earliestdeparturedate == undefined) ? null : ConvertToRequiredDate($scope.Earliestdeparturedate),
                "Latestdeparturedate": ($scope.Latestdeparturedate == '' || $scope.Latestdeparturedate == undefined) ? null : ConvertToRequiredDate($scope.Latestdeparturedate),
                "Theme": ($scope.Theme != undefined) ? $scope.Theme.id : "",
                "Location": $scope.Location,
                "Minfare": $scope.Minfare,
                "Maxfare": $scope.Maxfare,
                "PointOfSaleCountry": $scope.PointOfSaleCountry,
                "Region": ($scope.Region != undefined) ? $scope.Region.id : "",
                "TopDestinations": $scope.TopDestinations,
                "Destination": $scope.Destination

            };

            DestinationFactory.findDestinations(data).then(function (data) {
                $scope.SearchbuttonText = "Get Destinations";
                $scope.SearchbuttonCheapestText = "Top 10 Cheapest";
                $scope.SearchbuttonIsLoading = false;
                $scope.SearchbuttonChepestIsLoading = false;

                if (data.FareInfo != null) {
                    $scope.destinationlist = data.FareInfo;
                    $scope.buttontext = "Cheapest";
                    UtilFactory.MapscrollTo('wrapper');
                }
                else {
                    $scope.buttontext = $scope.buttontext == "All" ? "Cheapest" : "All";
                    alertify.alert("Destination Finder", "");
                    alertify.alert('Sorry! There are no destinations, match your search request!').set('onok', function (closeEvent) { });
                }
            });
            if ($scope.CalledOnPageLoad)
                $scope.CalledOnPageLoad = false;
        }
    }

})();