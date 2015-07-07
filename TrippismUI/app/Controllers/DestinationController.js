
(function () {
    'use strict';
    var controllerId = 'DestinationController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', '$location', 'blockUIConfig', '$modal', '$rootScope', '$timeout', 'DestinationFactory', 'UtilFactory', 'FareforecastFactory', 'SeasonalityFactory', DestinationController]);

    function DestinationController($scope,$location, blockUIConfig, $modal, $rootScope, $timeout, DestinationFactory, UtilFactory, FareforecastFactory, SeasonalityFactory) {
        $scope.hasError = false;
        $scope.Location = "";
        $scope.AvailableCodes = [];
        $scope.formats = Dateformat();
        $scope.format = $scope.formats[5];
        $scope.activate = activate;      
        $scope.findDestinations = findDestinations;
        $scope.Origin = '';
        $scope.Destination = '';
        $scope.buttontext = "All";
        $scope.AvailableAirports = [];
        $scope.destinationlist = "";
        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.AvailableThemes = AvailableTheme();
        $scope.AvailableRegions = AvailableRegions();
        $scope.IsHistoricalInfo = false;
        $scope.MaximumFromDate = ConvertToRequiredDate(common.addDays(new Date(), 192),'UI');
        $scope.LoadingText = "Loading..";
        $scope.SearchbuttonText = "Get Destinations";       
        $scope.SearchbuttonCheapestText = "Top 10 Cheapest";
        $scope.SearchbuttonIsLoading = false;       
        $scope.SearchbuttonChepestIsLoading = false;
        $scope.oneAtATime = true;
        $scope.status = {
            isFirstOpen: true,        
            Seasonalitystatus: false          
        };
        $scope.loadFareForecastInfo = loadFareForecastInfo;
        $scope.ISloader = false;
        var dt = new Date();
        
        dt.setHours(0, 0, 0, 0);
        
        var Todt = new Date();
        Todt.setDate(Todt.getDate() + 5); // add default from 5 days
        Todt.setHours(0, 0, 0, 0)

        $scope.ToDate = ConvertToRequiredDate(Todt,'UI');
        $scope.FromDate = ConvertToRequiredDate(dt,'UI');
        
        $scope.minTodayDate = new Date();
        $scope.minFromDate = new Date();
        $scope.minFromDate = $scope.minFromDate.setDate($scope.minFromDate.getDate() + 1);
        $scope.Destinationfortab = "";
        

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
                   $scope.Latestdeparturedate = ConvertToRequiredDate(newDt.setDate(newDt.getDate() + 1),'UI')
               }
               /**/

               // Calculate datediff
               var diff = daydiff(new Date(newValue).setHours(0, 0, 0, 0), new Date($scope.Latestdeparturedate).setHours(0, 0, 0, 0));
               if (diff > 30)
                   $scope.Latestdeparturedate = ConvertToRequiredDate(common.addDays(newDt, 30),'UI');

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
                      $scope.ToDate = ConvertToRequiredDate(newDt.setDate(newDt.getDate() + 1),'UI')
                  }
                  /**/

                  //SET MINIMUN SELECTED DATE for TODATE
                  $scope.minFromDate = new Date(newValue);
                  $scope.minFromDate = $scope.minFromDate.setDate($scope.minFromDate.getDate() + 1);
                  $scope.MaximumToDate = common.addDays($scope.minFromDate, 16);
              }
       );

        $scope.getMatchingStuffs = function ($viewValue) {
            var matchingStuffs = [];
            for (var i = 0; i < $scope.AvailableCodes.length; i++) {
                if ($scope.AvailableCodes[i].airport_CityName.toLowerCase().indexOf($viewValue.toLowerCase()) != -1 || 
                    $scope.AvailableCodes[i].airport_Code.toLowerCase().indexOf($viewValue.toLowerCase()) != -1) 
                    matchingStuffs.push($scope.AvailableCodes[i]);
                
            }
            return matchingStuffs;
        }

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

        $scope.$on('CloseFareForcastInfo', function (event, args) {
            $scope.IsHistoricalInfo = false;
            $scope.ISloader = false;
            $scope.status = {
                isFirstOpen: true,              
                Seasonalitystatus: false               
            };
        });

        $scope.$on('EmptyFareForcastInfo', function (event, args) {
            $scope.IsHistoricalInfo = false;
            $scope.MarkerInfo = "";
            $scope.MarkerSeasonalityInfo = "";
            $scope.OriginFullName = args.Origin;
            $scope.DestinationFullName = args.Destinatrion;
            $scope.fareData = args.Fareforecastdata;
            $scope.Destinationfortab = args.Destinatrion;
            $scope.open('lg',args);
        });

        function loadFareForecastInfo()
        {
            $scope.MarkerInfo = "";
            $scope.status = {
                isFirstOpen: true,            
                Seasonalitystatus: false             
            };
            FareforecastFactory.fareforecast($scope.fareData).then(function (data) {              
             $scope.MarkerInfo = data;                
            });          
        }

        $scope.loadSeasonalityInfo = function ($event) {           
            if ($scope.MarkerSeasonalityInfo == "")
            {
                var Seasonalitydata = {
                    "Destination":  $scope.Destinationfortab                    
                };
                $timeout(function () {                 
                    $scope.status = {
                        isFirstOpen: false,                      
                        Seasonalitystatus: true                        
                    };
                    SeasonalityFactory.Seasonality(Seasonalitydata).then(function (data) {
                        $scope.MarkerSeasonalityInfo = data;                      
                    });
                }, 0, false);
            }
        };

        function activate() {
            var search = $location.search();
            var org = search.org;
            var _qFromDate = search.fromdate;
            var _qToDate = search.todate;

            
            blockUIConfig.autoBlock = false;
            UtilFactory.ReadAirportJson().then(function (data) {
                blockUIConfig.autoBlock = true;
                $scope.AvailableAirports = data;
                $scope.CalledOnPageLoad = true;
                $scope.AvailableCodes = angular.copy($scope.AvailableAirports);
                
                if (org == undefined || org == '') {
                    UtilFactory.getIpinfo($scope.AvailableAirports).then(function (data) {
                        if (data == undefined) {
                            //  alertify.alert('Trippism', 'Oops! Sorry, we are unable to detect your home location automatically.');
                            return;
                        }
                        else {
                             $scope.Origin = data.airport_Code;
                           $scope.findDestinations('Cheapest');
                        }
                    });
                }
                else {
                    if (_qFromDate != undefined && _qFromDate != '')
                        $scope.FromDate = _qFromDate;

                    if (_qToDate != undefined && _qToDate != '')
                        $scope.ToDate = _qToDate;
                    
                    $scope.Origin = org;
                    $scope.findDestinations('Cheapest');
                }
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

        $scope.openToDate = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.opened = true;
        };

        $scope.CloseDetailInfo = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.IsHistoricalInfo = false;
            $scope.ISloader = false;
            $scope.status = {
                isFirstOpen: true,               
                Seasonalitystatus: false             
            };
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
            if ($scope.CalledOnPageLoad == false) {
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
                "DepartureDate": ($scope.FromDate == '' || $scope.FromDate == undefined) ? null : ConvertToRequiredDate($scope.FromDate ,'API'),
                "ReturnDate": ($scope.ToDate == '' || $scope.ToDate == undefined) ? null : ConvertToRequiredDate($scope.ToDate,'API'),
                "Lengthofstay": $scope.LenghtOfStay,
                "Earliestdeparturedate": ($scope.Earliestdeparturedate == '' || $scope.Earliestdeparturedate == undefined) ? null : ConvertToRequiredDate($scope.Earliestdeparturedate,'API'),
                "Latestdeparturedate": ($scope.Latestdeparturedate == '' || $scope.Latestdeparturedate == undefined) ? null : ConvertToRequiredDate($scope.Latestdeparturedate,'API'),
                "Theme": ($scope.Theme != undefined) ? $scope.Theme.id : "",
                "Location": $scope.Location,
                "Minfare": $scope.Minfare,
                "Maxfare": $scope.Maxfare,
                "PointOfSaleCountry": $scope.PointOfSaleCountry,
                "Region": ($scope.Region != undefined) ? $scope.Region.id : "",
                "TopDestinations": $scope.TopDestinations,
                "Destination": $scope.Destination
            };

            blockUIConfig.message = "Loading destinations..."
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
                    alertify.alert('Opps! Sorry, no suggestions are available from your origin on various destinations!').set('onok', function (closeEvent) { });
                }
                blockUIConfig.message = "Loading..."
            });
            
            if ($scope.CalledOnPageLoad)
                $scope.CalledOnPageLoad = false;
        }
        
        $scope.open = function (size, obj) {
            $scope.mapOptions = obj.mapOptions;
            var modalInstance = $modal.open({
                animation: true,
                templateUrl: 'DestinationDetails.html',
                controller: 'DestinationDetailsCtrl',
                size: size,
                resolve: {
                    mapDetails: function () {
                        return $scope.mapOptions;
                    },
                    OriginairportName: function () {
                        return obj.OriginairportName;
                    },
                    DestinationairportName: function () { return obj.DestinationairportName; },
                    Fareforecastdata: function () { return obj.Fareforecastdata; },
                    Destination: function () { return obj.Destinatrion; },
                    destinationScope: function () { return $scope; }
                }
            });
            modalInstance.result.then(function (selectedItem) {},
            function () {

            });
        };
    }
})();


var controllerId = 'DestinationDetailsCtrl'; //DestinationDetailsCtrl
angular.module('TrippismUIApp').controller(controllerId,
    ['$scope', 'blockUIConfig', '$timeout', '$modalInstance', 'mapDetails', 'OriginairportName', 'DestinationairportName', 'Fareforecastdata', 'Destination', 'destinationScope', 'FareforecastFactory', 'SeasonalityFactory', 'FareRangeFactory', DestinationDetailsCtrl]);

function DestinationDetailsCtrl($scope, blockUIConfig, $timeout, $modalInstance, mapDetails, OriginairportName, DestinationairportName, Fareforecastdata, Destination, destinationScope, FareforecastFactory, SeasonalityFactory, FareRangeFactory) {
    //$scope.loadFareForecastInfo = loadFareForecastInfo;
    $scope.mapDetails = mapDetails;
    
    $scope.OriginairportName = OriginairportName;
    $scope.DestinationairportName = DestinationairportName;
    $scope.fareData = Fareforecastdata;
    $scope.Destinationfortab = Destination;
    
    $scope.destinationScope = destinationScope;
    
    $scope.oneAtATime = true;
    
    $scope.DestinationName = ($scope.OriginairportName.airport_FullName.indexOf("All") > -1) ? $scope.OriginairportName.airport_CityName : $scope.OriginairportName.airport_FullName + ', ' + $scope.OriginairportName.airport_CityName;

    $scope.ok = function () { $modalInstance.close($scope.selected.item);};

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.oneAtATime = true;
    $scope.status = {
        isFirstOpen: true,
        isFirstDisabled: false
    };
    
    $scope.FareForecastInfoLoaded = false;
    $scope.FareNoDataFound = false;
    $scope.loadFareForecastInfo = function ($event) {
        
        if ($scope.FareForecastInfoLoaded == false) {
            //$scope.FareforecastData = ;
            $scope.status = {
                isFirstOpen: true,
                Seasonalitystatus: false
            };
            blockUIConfig.message = 'Please wait! Fare Details are loading...!';
            FareforecastFactory.fareforecast($scope.fareData).then(function (data) {
                if (data.status == 404)
                    $scope.FareNoDataFound = true;
                $scope.FareforecastData = data;
                blockUIConfig.message = 'Loading...';
            });
        }
        $scope.FareForecastInfoLoaded = true;
    };
    $scope.loadFareForecastInfo();
    $scope.MarkerSeasonalityInfo = "";

    $scope.loadSeasonalityInfoLoaded = false;
    $scope.SeasonalityNoDataFound = false;
    $scope.loadSeasonalityInfo = function ($event) {
        if ($scope.loadSeasonalityInfoLoaded == false) {
            if ($scope.MarkerSeasonalityInfo == "") {
                var Seasonalitydata = {
                    "Destination": $scope.Destinationfortab, // JFK
                };

                $timeout(function () {
                    //blockUIConfig.message = 'Please wait! Seasonality info are loading...!';
                    blockUIConfig.message = 'Please wait! Insights details are loading...!';
                    SeasonalityFactory.Seasonality(Seasonalitydata).then(function (data) {

                        blockUIConfig.autoBlock = false;
                        $scope.loadfareRangeInfo();
                        
                        if (data.status == 404)
                            $scope.SeasonalityNoDataFound = true;
                       
                        //var defaultSeasonality = data.Seasonality;
                        //var now = new Date();
                        //var Nextday;
                        //Nextday =  common.addDays(now,30);
                        
                        

                        //for (var i = 0; i < defaultSeasonality.length; i++) {
                        //    var datetocheck = defaultSeasonality[i].WeekStartDate;
                        //    console.log(datetocheck > from && datetocheck < to)
                        //}

                        $scope.MarkerSeasonalityInfo = data;
                        blockUIConfig.message = 'Loading...';
                    });

                }, 0, false);
            }
        }
        $scope.loadSeasonalityInfoLoaded = true;
    };

    $scope.fareRangeInfoLoaded = false;
    $scope.fareRangeInfoNoDataFound = false;
    $scope.fareRangeData = "";

    $scope.loadfareRangeInfo = function ($event) {
        
        var data = {
            "Origin" :  $scope.fareData.Origin,
            "Destination": $scope.fareData.Destination,
            "EarliestDepartureDate": $scope.fareData.DepartureDate,
            "LatestDepartureDate": $scope.fareData.ReturnDate,
            "Lengthofstay" : 4
        }; 
        if ($scope.fareRangeInfoLoaded == false) {
            if ($scope.fareRangeData == "") {
                    $scope.FareRangeLoading = true;
                    //blockUIConfig.message = 'Please wait! fare range info are loading...!';
                    FareRangeFactory.fareRange(data).then(function (data) {
                        $scope.FareRangeLoading = false;
                        blockUIConfig.autoBlock = true;
                        if (data.status == 404)
                            $scope.fareRangeInfoNoDataFound = true;
                        $scope.fareRangeData = data;
                      //  blockUIConfig.message = 'Loading...';
                    });
            }
        }
        $scope.fareRangeInfoLoaded = true;
    };


  
};