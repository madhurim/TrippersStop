
(function () {
    'use strict';
    var controllerId = 'DestinationController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', '$rootScope', '$http', '$q', 'blockUIConfig', '$location', '$anchorScroll','DestinationFactory', DestinationController]);

    function DestinationController($scope, $rootScope, $http, $q, blockUIConfig, $location,$anchorScroll, DestinationFactory) {
       
        function MapscrollTo(id) {
            var old = $location.hash();
            $location.hash(id);
            $anchorScroll();
            $location.hash(old);
        }
        $scope.hasError = false;
        $scope.Location = "";
        $scope.AvailableCodes = [];
        $scope.SeasonalityHistorySearch = SeasonalityHistorySearch;
        $scope.formats = ['yyyy-MM-dd', 'dd-MM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate','MM/dd/yyyy'];
        $scope.format = $scope.formats[5];       
        $scope.accuracy = "0";

        function ConvertToRequiredDate(dt) {
            dt = new Date(dt);
            var curr_date = ('0' + dt.getDate()).slice(-2);
            var curr_month = ('0' + (dt.getMonth() + 1)).slice(-2);
            var curr_year = dt.getFullYear();
            var _date = curr_year + "-" + curr_month + "-" + curr_date;
            return _date;
        }

        
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


        function daydiff(first, second) {
            return Math.round((second - first) / (1000 * 60 * 60 * 24));
        }

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

        $scope.Origin = '';
        $scope.Destination = '';
        $scope.AvailableThemes = [
                                    { id: "BEACH", value: "BEACH" },
                                    { id: "CARIBBEAN", value: "CARIBBEAN" },
                                    { id: "DISNEY", value: "DISNEY" },
                                    { id: "GAMBLING", value: "GAMBLING" },
                                    { id: "HISTORIC", value: "HISTORIC" },
                                    { id: "MOUNTAINS", value: "MOUNTAINS" },
                                    { id: "NATIONAL-PARKS", value: "NATIONAL-PARKS" },
                                    { id: "OUTDOORS", value: "OUTDOORS" },
                                    { id: "ROMANTIC", value: "ROMANTIC" },
                                    { id: "SHOPPING", value: "SHOPPING" },
                                    { id: "SKIING", value: "SKIING" },
                                    { id: "THEME-PARK", value: "THEME-PARK" }
        ];
        $scope.AvailableRegions = [
                                    { id: 'Africa', value: 'Africa' },
                                    { id: 'Asia Pacific', value: 'Asia Pacific' },
                                    { id: 'Europe', value: 'Europe' },
                                    { id: 'Latin America', value: 'Latin America' },
                                    { id: 'Middle East', value: 'Middle East' },
                                    { id: 'North America', value: 'North America' },
        ];

        $scope.MaximumFromDate = ConvertToRequiredDate(common.addDays(new Date(), 192));

        function SeasonalityHistorySearch(dest) {
            var data = { "Destination": dest };
            DestinationFactory.SeasonalityHistorySearch(data).then(function (data) {
                if (data != undefined) {
                    var result = JSON.parse(data);
                    var objects = JSON.parse(result.Response);
                }
            });
        };

        $scope.CustomAirportsData = [];

        $scope.activate = activate;
        $scope.AvailableAirports = [];
        function activate() {         
            blockUIConfig.autoBlock = true;
            $http.get('../app/Constants/airports.json').success(function (_arrairports) {
                for (var i = 0; i < _arrairports.length; i++) {
                    if (_arrairports[i].Airports != undefined) {
                        for (var cntAirport = 0; cntAirport < _arrairports[i].Airports.length ; cntAirport++) {
                            var objtopush = _.omit(_arrairports[i], "Airports");
                            objtopush['airport_Code'] = _arrairports[i].Airports[cntAirport].airport_Code;
                            objtopush['airport_FullName'] = _arrairports[i].Airports[cntAirport].airport_FullName;
                            objtopush['airport_Lat'] = _arrairports[i].Airports[cntAirport].airport_Lat;
                            objtopush['airport_Lng'] = _arrairports[i].Airports[cntAirport].airport_Lng;
                            objtopush['airport_IsMAC'] = false;
                            $scope.AvailableAirports.push(objtopush);
                        }
                    }
                    if (_arrairports[i].Airports != undefined && _arrairports[i].Airports.length > 1) {
                        var objtopush = _.omit(_arrairports[i], "Airports");
                        objtopush['airport_Code'] = _arrairports[i].airport_CityCode;
                        objtopush['airport_FullName'] = _arrairports[i].airport_CityName + ", All Airports";
                        objtopush['airport_Lat'] = null;
                        objtopush['airport_Lng'] = null;
                        objtopush['airport_IsMAC'] = true;
                        $scope.AvailableAirports.push(objtopush);
                    }
                }

                $scope.AvailableCodes = angular.copy($scope.AvailableAirports);
                getIpinfo();
                MapscrollTo('map_canvas');
            });

        }
        activate();
       
        $scope.buttontext = "All";
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

        function getIpinfo() {
            blockUIConfig.autoBlock = true;
            var url = "http://ipinfo.io?callback=JSON_CALLBACK";
            $http.jsonp(url)
           .success(function (data) {
               var originairport = _.find($scope.AvailableAirports, function (airport) { return airport.airport_CityName == data.city && airport.airport_CountryCode == data.country });
               if (originairport != null) {
                   $scope.Origin = originairport.airport_Code;
                   $scope.CalledOnPageLoad = true;
                   $scope.findDestinations('Cheapest');
               }
           });

        }

        $rootScope.apiURL = 'http://localhost:14606/sabre/api/';

        $scope.findDestinations = findDestinations;

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

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        $scope.LoadingText = "Loading..";
        $scope.SearchbuttonText = "Get Destinations";
        $scope.SearchbuttonTo10Text = "Top 10";
        $scope.SearchbuttonCheapestText = "Top 10 Cheapest";
        $scope.SearchbuttonIsLoading = false;
        $scope.SearchbuttonTop10IsLoading = false;
        $scope.SearchbuttonChepestIsLoading = false;

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
            debugger;
            DestinationFactory.findDestinations(data).then(function (data) {
                $scope.SearchbuttonText = "Get Destinations";               
                $scope.SearchbuttonCheapestText = "Top 10 Cheapest";
                $scope.SearchbuttonIsLoading = false;              
                $scope.SearchbuttonChepestIsLoading = false;

                if (data.FareInfo != null) {                   
                        $scope.destinationlist = data.FareInfo;
                        $scope.buttontext = "Cheapest";                        
                        MapscrollTo('wrapper')
                }
                else {
                    alertify.alert("Destination Finder", "");
                    alertify.alert('Sorry! There are no destinations, match your search request!').set('onok', function (closeEvent) { });
                }
            });
            if ($scope.CalledOnPageLoad)
                $scope.CalledOnPageLoad = false;
        }
    }

})();