(function () {
    'use strict';
    var controllerId = 'DestinationController';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope',
            '$location',
            '$modal',
            '$rootScope',
            '$timeout',
            '$filter',
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
        $filter,
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
            $rootScope.$broadcast('ontabClicked', $scope.tabManager.tabItems[i].parametersData.tabIndex);
        }

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
        $scope.OriginCityName = '';
        $scope.Destination = '';
        $scope.buttontext = "All";
        $scope.AvailableAirports = [];
        $scope.destinationlist = "";
        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.topdestinationflg = true;
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
        $scope.CreateTab = CreateTab;
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
        $scope.top5destinationclick = function (item) {
            debugger;
            var OriginairportName = _.find($scope.AvailableAirports, function (airport) {
                return airport.airport_Code == $scope.Origin.toUpperCase()
            });
            var DestinationairportName = _.find($scope.AvailableAirports, function (airport) {
                return airport.airport_Code == item.topdestinationFareInfo.DestinationLocation
            });

            var dataForecast = {
                "Origin": $scope.Origin,
                "DepartureDate": $filter('date')(item.topdestinationFareInfo.DepartureDateTime, 'yyyy-MM-dd'),
                "ReturnDate": $filter('date')(item.topdestinationFareInfo.ReturnDateTime, 'yyyy-MM-dd'),
                "Destination": item.topdestinationFareInfo.DestinationLocation
            };

            $rootScope.$broadcast('EmptyFareForcastInfo', {
                Origin: OriginairportName.airport_CityName,
                Destinatrion: DestinationairportName.airport_Code,
                Fareforecastdata: dataForecast,
                mapOptions: item.topdestinationFareInfo,
                OriginairportName: OriginairportName,
                DestinationairportName: DestinationairportName,
                DestinationList: $scope.destinationlist,
                AvailableAirports: $scope.AvailableAirports,
                //tabIndex : 999  // used for popup
            });
       
      

        };

        $scope.ViewDestination = function () {
            $scope.ShowDestinationView = true;
            $scope.tabManager.resetSelected();
            $scope.TabcontentView = false;
            $timeout(function () {
                $rootScope.$broadcast('eventDestinationMapresize');
            }, 500, false);

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
            $scope.seasonalitydirectiveData = args;
            CreateTab();
        });

        function CreateTab() {
            $scope.tabManager.resetSelected();
            var i = ($scope.tabManager.tabItems.length + 1);
            $scope.TabCreatedCount = $scope.TabCreatedCount + 1;
            var _paramsdata = $scope.seasonalitydirectiveData;
            _paramsdata.tabIndex = $scope.TabCreatedCount;
            var searchcriteria = {
                FromDate :$scope.FromDate,
                ToDate: $scope.ToDate,
                Lengthofstay: $scope.LenghtOfStay
                };

            _paramsdata.dataforEmail = {};
            _paramsdata.searchcriteria = searchcriteria;
            $scope.tabManager.tabItems.push({
                parametersData: _paramsdata,
                title: $scope.seasonalitydirectiveData.OriginairportName.airport_Code + ' - ' + $scope.seasonalitydirectiveData.DestinationairportName.airport_Code,
                content: "",
                selected: true,
                TabcontentView: true
            });

            $scope.ShowDestinationView = false;
        }

        $scope.$on('CreateTabForDestination', function () {
            $scope.tabManager.resetSelected();
            var i = ($scope.tabManager.tabItems.length + 1);
            $scope.TabCreatedCount = $scope.TabCreatedCount + 1;
            var _paramsdata = $scope.seasonalitydirectiveData;
            _paramsdata.tabIndex = $scope.TabCreatedCount;
            _paramsdata.dataforEmail = [];
            $scope.tabManager.tabItems.push({
                parametersData: _paramsdata,
                title: $scope.seasonalitydirectiveData.OriginairportName.airport_Code + ' - ' + $scope.seasonalitydirectiveData.DestinationairportName.airport_Code,
                content: "",
                selected: true,
                TabcontentView: true
            });

            $scope.ShowDestinationView = false;
        });

        function btnSearchClick() {
            $scope.fareforecastdirectiveDisplay = false;
            if ($scope.isSearching == true)
                $scope.isSearching = false;
            else
                $scope.isSearching = true;
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

        $scope.$watch('Origin',
                function (oldval, newValue) {
                    if (newValue != undefined && newValue != "") {
                        $scope.OrigintoDisp = $scope.Origin.toUpperCase();
                        var originairportdata = _.find($scope.AvailableAirports, function (airport) { return airport.airport_Code == $scope.Origin.toUpperCase() });
                        if (originairportdata != undefined)
                            $scope.OriginCityName = originairportdata.airport_CityName;
                        else
                            $scope.OriginCityName = '';
                    }
                }
              );

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
               
                if (org == undefined || org == '') {
                    UtilFactory.getIpinfo($scope.AvailableAirports).then(function (data) {
                        if (data == undefined)
                            return;
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
            $scope.OriginCityName = $item.airport_CityName;
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

            var originairport = _.find($scope.AvailableAirports, function (airport) { return airport.airport_Code == $scope.Origin.toUpperCase() });
           
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
                "PointOfSaleCountry": PointOfsalesCountry,
                "Region": ($scope.Region != undefined) ? $scope.Region.id : "",
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
           
            //Get Top Destination
            data.TopDestinations = 5;
            $scope.topdestination = DestinationFactory.findDestinations(data).then(function (data) {
                if (data.FareInfo != null) {
                    $scope.topdestinationlist = [];
                    for (var x = 0; x < data.FareInfo.length; x++) {
                        var airportdata = _.find($scope.AvailableAirports, function (airport) {
                            return airport.airport_Code == data.FareInfo[x].DestinationLocation
                        });
                        var topdestination = {
                            "AirportCode" : airportdata.airport_Code,
                            "Cityname": airportdata.airport_CityName,
                            "LowestNonStopFare": data.FareInfo[x].LowestNonStopFare,
                            "LowestFare": data.FareInfo[x].LowestFare,
                            "topdestinationFareInfo": data.FareInfo[x]
                        };
                        $scope.topdestinationlist.push(topdestination);
                    }
                }
                $scope.inProgress = false;
            });


            if ($scope.CalledOnPageLoad)
                $scope.CalledOnPageLoad = false;
        }
    }
})();


