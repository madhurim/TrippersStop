/// <reference path="../Views/partials/aside.html" />
/// <reference path="../Views/partials/aside.html" />

(function () {
    'use strict';
    var controllerId = 'DestinationController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope',
            '$location',
            '$modal',
            '$rootScope',
            '$timeout',
            'DestinationFactory',
            'UtilFactory',
            'FareforecastFactory',
            'SeasonalityFactory',
             DestinationController]);

    function DestinationController(
        $scope,
        $location,
        $modal,
        $rootScope,
        $timeout,
        DestinationFactory,
        UtilFactory,
        FareforecastFactory,
        SeasonalityFactory
        ) {

        $scope.ShowDestinationView = true;
        $scope.TabcontentView = true;
        $scope.TabCreatedCount = 0;
        $scope.tabManager = {};
        $scope.tabManager.tabItems = [];
        $scope.tabManager.checkIfMaxTabs = function () {
            var max = 4;
            var i = $scope.tabManager.tabItems.length;
            if (i > max) {
                return true;
            }
            return false;
        };

        $scope.tabManager.getTitle = function (tabInfo) {
            tabInfo.title.substr(0, 10);
        };

        $scope.tabManager.resetSelected = function () {
            angular.forEach($scope.tabManager.tabItems, function (pane) {
                pane.TabcontentView = false; // Custom
                pane.selected = false;
            });
        };

        //$scope.tabManager.addTab = function () {
        //    $scope.tabManager.resetSelected();
        //    var i = ($scope.tabManager.tabItems.length + 1);
        //    $scope.tabManager.tabItems.push({
        //        title: "Tab No: " + i,
        //        content: "Lores sum ep sum news test [" + i + "]",
        //        selected: true
        //    });
        //};

        $scope.$on('CreateTabForDestination', function () {
            $scope.tabManager.resetSelected();
            var i = ($scope.tabManager.tabItems.length + 1);
            $scope.TabCreatedCount = $scope.TabCreatedCount + 1;
            var _paramsdata = $scope.seasonalitydirectiveData;
            _paramsdata.tabIndex = $scope.TabCreatedCount;

            $scope.tabManager.tabItems.push({
                //youtubeData: _youtubeparams,
                parametersData: _paramsdata,
                title: $scope.seasonalitydirectiveData.OriginairportName.airport_Code + ' - ' + $scope.seasonalitydirectiveData.DestinationairportName.airport_Code,
                content: "",
                selected: true,
                TabcontentView :true
            });

            $scope.ShowDestinationView = false;
        });

        $scope.tabManager.selectPreviousTab = function (i, $event) {
            if (typeof $event != 'undefined') {
                $event.stopPropagation();
            }
            if (i > 0) {
                angular.forEach($scope.tabManager.tabItems, function (tabInfo) {
                    tabInfo.selected = false;
                });
                $scope.tabManager.tabItems[i - 1].selected = true;
            }
        }

        $scope.tabManager.removeTab = function (i, $event) {

            if (typeof $event != 'undefined') {
                $event.stopPropagation();
            }
            if ($scope.tabManager.tabItems.length > 0)
                $scope.tabManager.selectPreviousTab(i);
            // remove from array
            $scope.tabManager.tabItems.splice(i, 1);
            $scope.ViewDestination();
        }


        $scope.tabManager.select = function (i) {
            $scope.ShowDestinationView = false;
            angular.forEach($scope.tabManager.tabItems, function (tabInfo) {
                tabInfo.selected = false;
                tabInfo.TabcontentView = false;
            });
            $scope.tabManager.tabItems[i].selected = true;
            $scope.tabManager.tabItems[i].TabcontentView = true;
            
            //$rootScope.$broadcast('ViewTab');
        }

        //add few tabs
        //$scope.tabManager.tabItems.push({ title: "Tab No Home"  });
        //for (var i = 1; i < 3; i++) {
        //    $scope.tabManager.tabItems.push({
        //        title: "Tab No: " + i,
        //        content: "Lores sum ep sum news test [" + i + "]",
        //        selected: false
        //    });
        //}

        // init the first active tab
        //$scope.tabManager.select(0);


        $scope.isSearching = true;
        $scope.MailMarkerSeasonalityInfo = {};
        $scope.MailFareRangeData = {};

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
        $scope.MaximumFromDate = ConvertToRequiredDate(common.addDays(new Date(), 192), 'UI');
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
        $scope.btnSearchClick = btnSearchClick;
        var dt = new Date();

        dt.setHours(0, 0, 0, 0);

        var Todt = new Date();
        Todt.setDate(Todt.getDate() + 5); // add default from 5 days
        Todt.setHours(0, 0, 0, 0)

        $scope.ToDate = ConvertToRequiredDate(Todt, 'UI');
        $scope.FromDate = ConvertToRequiredDate(dt, 'UI');



        $scope.minTodayDate = new Date();
        $scope.minFromDate = new Date();
        $scope.minFromDate = $scope.minFromDate.setDate($scope.minFromDate.getDate() + 1);
        $scope.Destinationfortab = "";

        // $scope.Earliestdeparturedate = ConvertToRequiredDate($scope.FromDate, 'UI');

        $scope.ViewDestination = function () {
            $scope.ShowDestinationView = true;
            $scope.tabManager.resetSelected();
            $scope.TabcontentView = false;
            //$rootScope.$broadcast('eventDestinationMapresize');
        };

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
                   $scope.Latestdeparturedate = ConvertToRequiredDate(newDt.setDate(newDt.getDate() + 1), 'UI')
               }
               /**/

               // Calculate datediff
               var diff = daydiff(new Date(newValue).setHours(0, 0, 0, 0), new Date($scope.Latestdeparturedate).setHours(0, 0, 0, 0));
               if (diff > 30)
                   $scope.Latestdeparturedate = ConvertToRequiredDate(common.addDays(newDt, 30), 'UI');

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
                      $scope.ToDate = ConvertToRequiredDate(newDt.setDate(newDt.getDate() + 1), 'UI')
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

        //$scope.$watch('IsAdvancedSearch', function (scope) {
        //    if (scope == false || scope == undefined) {
        //        $scope.Theme = "";
        //        $scope.Region = "";
        //        $scope.Minfare = "";
        //        $scope.Maxfare = "";
        //        $scope.TopDestinations = "";
        //        $scope.Earliestdeparturedate = "";
        //        $scope.Latestdeparturedate = "";
        //        $scope.LenghtOfStay = "";
        //    }
        //});

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
            $scope.fareforecastdirective = $scope.fareData;
            //$scope.seasonalitydirectiveData = args.Destinatrion;
            $scope.seasonalitydirectiveData = args;
            $scope.fareforecastdirectiveDisplay = true;
            //$scope.open('lg', args);
        });

        function btnSearchClick() {
            $scope.fareforecastdirectiveDisplay = false;
            if ($scope.isSearching == true) {
                $scope.isSearching = false;

            }
            else {
                $scope.isSearching = true;
            }
        }
        function loadFareForecastInfo() {
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
            if ($scope.MarkerSeasonalityInfo == "") {
                var Seasonalitydata = {
                    "Destination": $scope.Destinationfortab
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
            var org = search.Origin;
            var _qFromDate = search.DepartureDate;
            var _qToDate = search.ReturnDate;

            $scope.IsairportJSONLoading = true;
            UtilFactory.ReadAirportJson().then(function (data) {
                $scope.IsairportJSONLoading = false;
                $scope.CalledOnPageLoad = false;
                $scope.AvailableAirports = data;
                
                $scope.AvailableCodes = angular.copy($scope.AvailableAirports);

                // Static Block
                $scope.Origin = 'ATL';
                $scope.CalledOnPageLoad = true;
                $scope.findDestinations('Cheapest');
                return;
                // Static Block Ends

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
                    $scope.CalledOnPageLoad = true;
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

            $scope.isSearching = true;
            if (buttnText == 'All') { $scope.SearchbuttonIsLoading = true; $scope.SearchbuttonText = $scope.LoadingText; }
            else if (buttnText == 'Cheapest') { $scope.SearchbuttonChepestIsLoading = true; $scope.SearchbuttonCheapestText = $scope.LoadingText; }


            var originairport = _.find($scope.AvailableAirports, function (airport) { return airport.airport_Code == $scope.Origin });

            var PointOfsalesCountry;
            if (originairport != undefined)
                PointOfsalesCountry = originairport.airport_CountryCode;

            var data = {
                "Origin": $scope.Origin,
                "DepartureDate": ($scope.FromDate == '' || $scope.FromDate == undefined) ? null : ConvertToRequiredDate($scope.FromDate, 'API'),
                "ReturnDate": ($scope.ToDate == '' || $scope.ToDate == undefined) ? null : ConvertToRequiredDate($scope.ToDate, 'API'),
                "Lengthofstay": $scope.LenghtOfStay,
                "Earliestdeparturedate": ($scope.Earliestdeparturedate == '' || $scope.Earliestdeparturedate == undefined) ? null : ConvertToRequiredDate($scope.Earliestdeparturedate, 'API'),
                "Latestdeparturedate": ($scope.Latestdeparturedate == '' || $scope.Latestdeparturedate == undefined) ? null : ConvertToRequiredDate($scope.Latestdeparturedate, 'API'),
                "Theme": ($scope.Theme != undefined) ? $scope.Theme.id : "",
                "Location": $scope.Location,
                "Minfare": $scope.Minfare,
                "Maxfare": $scope.Maxfare,
                "PointOfSaleCountry": PointOfsalesCountry, //$scope.PointOfSaleCountry,
                "Region": ($scope.Region != undefined) ? $scope.Region.id : "",
                //"TopDestinations": 50, //$scope.TopDestinations,
                "Destination": $scope.Destination
            };
            $scope.inProgress = true;
            
            $scope.mappromise = DestinationFactory.findDestinations(data).then(function (data) {
                $scope.isSearching = false;
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
                $scope.inProgress = false;

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
            modalInstance.result.then(function (selectedItem) { },
            function () {

            });
        };
    }
})();


var controllerId = 'DestinationDetailsCtrl'; //DestinationDetailsCtrl
angular.module('TrippismUIApp').controller(controllerId,
    ['$scope', '$timeout', '$modalInstance', 'mapDetails', 'OriginairportName', 'DestinationairportName', 'Fareforecastdata', 'Destination', 'destinationScope', 'FareforecastFactory', 'SeasonalityFactory', 'FareRangeFactory', DestinationDetailsCtrl]);

function DestinationDetailsCtrl($scope, $timeout, $modal, $modalInstance, mapDetails, OriginairportName, DestinationairportName, Fareforecastdata, Destination, destinationScope, FareforecastFactory, SeasonalityFactory, FareRangeFactory) {
    //$scope.loadFareForecastInfo = loadFareForecastInfo;

    $scope.mapDetails = mapDetails;

    $scope.delay = 0;
    $scope.minDuration = 0;
    $scope.message = 'Please Wait...';
    $scope.backdrop = true;
    $scope.promise = "myPromise";


    $scope.OriginairportName = OriginairportName;
    $scope.DestinationairportName = DestinationairportName;
    $scope.fareData = Fareforecastdata;
    $scope.Destinationfortab = Destination;

    $scope.destinationScope = destinationScope;

    $scope.oneAtATime = true;

    $scope.DestinationName = ($scope.OriginairportName.airport_FullName.indexOf("All") > -1) ? $scope.OriginairportName.airport_CityName : $scope.OriginairportName.airport_FullName + ', ' + $scope.OriginairportName.airport_CityName;

    $scope.ok = function () { $modalInstance.close($scope.selected.item); };

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

            $scope.inProgressFareinfo = true;
            $scope.fareinfopromise = FareforecastFactory.fareforecast($scope.fareData).then(function (data) {
                $scope.inProgressFareinfo = false;
                if (data.status == 404)
                    $scope.FareNoDataFound = true;
                $scope.FareforecastData = data;
            });
        }
        $scope.FareForecastInfoLoaded = true;
    };
    $scope.loadFareForecastInfo();
    $scope.MarkerSeasonalityInfo = "";

    $scope.loadSeasonalityInfoLoaded = false;
    $scope.SeasonalityNoDataFound = false;


    $scope.SeasonalityDisplay = function () {
        $scope.MarkerSeasonalityInfo.Seasonality = $scope.SeasonalityData;
    };

    $scope.loadSeasonalityInfo = function ($event) {
        if ($scope.loadSeasonalityInfoLoaded == false) {
            if ($scope.MarkerSeasonalityInfo == "") {
                var Seasonalitydata = {
                    "Destination": $scope.Destinationfortab, // JFK
                };

                $timeout(function () {
                    $scope.inProgressSeasonalityinfo = true;
                    $scope.FareRangeLoading = true;
                    $scope.seasonalitypromise = SeasonalityFactory.Seasonality(Seasonalitydata).then(function (data) {

                        if (data.status == 404)
                            $scope.SeasonalityNoDataFound = true;
                        else {
                            $scope.SeasonalityData = data.Seasonality;
                        }

                        var defaultSeasonality = data.Seasonality;
                        var now = new Date();
                        var NextDate = common.addDays(now, 30);


                        var filteredSeasonalityData = [];
                        for (var i = 0; i < defaultSeasonality.length; i++) {
                            $scope.MarkerSeasonalityInfo.Seasonality = [];
                            var datetocheck = new Date(defaultSeasonality[i].WeekStartDate);
                            if (datetocheck > now && datetocheck < NextDate)
                                filteredSeasonalityData.push(defaultSeasonality[i]);
                        }
                        data.Seasonality = filteredSeasonalityData;
                        $scope.MarkerSeasonalityInfo = data;
                        $scope.inProgressSeasonalityinfo = false;
                    });
                    $scope.loadfareRangeInfo();
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
            "Origin": $scope.fareData.Origin,
            "Destination": $scope.fareData.Destination,
            "EarliestDepartureDate": $scope.fareData.DepartureDate,
            "LatestDepartureDate": $scope.fareData.ReturnDate,
            "Lengthofstay": 4
        };
        if ($scope.fareRangeInfoLoaded == false) {
            if ($scope.fareRangeData == "") {
                $scope.farerangepromise = FareRangeFactory.fareRange(data).then(function (data) {
                    $scope.FareRangeLoading = false;

                    if (data.status == 404)
                        $scope.fareRangeInfoNoDataFound = true;
                    $scope.fareRangeData = data;
                });
            }
        }
        $scope.fareRangeInfoLoaded = true;
    };


};