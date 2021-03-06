﻿angular.module('TrippismUIApp').directive('farerangeInfo', ['$compile', '$timeout', '$filter', '$rootScope', 'FareRangeFactory', 'TrippismConstants',
    function ($compile, $timeout, $filter, $rootScope, FareRangeFactory, TrippismConstants) {
        return {
            restrict: 'E',
            scope: {
                farerangeParams: '=',
            },
            templateUrl: '/app/Views/Partials/FareRangePartial.html',
            link: function (scope, elem, attrs) {
                scope.formats = Dateformat();
                scope.format = scope.formats[5];
                scope.$watchGroup(['farerangeParams'], function (newValue, oldValue, scope) {

                    //Add Scope For Chart
                    if (scope.farerangeParams != undefined) {
                        scope.DepartDate = $filter('date')(scope.farerangeParams.Fareforecastdata.DepartureDate, scope.format, null);
                        scope.ReturnDate = $filter('date')(scope.farerangeParams.Fareforecastdata.ReturnDate, scope.format, null);
                        scope.chartHeight = 400;
                        scope.TabIndex = "farerange" + scope.farerangeParams.tabIndex;
                        var mapHTML = "<div style='float:left;width:100%;' id='" + scope.TabIndex + "'></div>";
                        elem.append($compile(mapHTML)(scope));
                    }
                    scope.loadfareRangeInfo();
                });
                scope.loadingFareRange = true;
                scope.$parent.divFareRange = false;
                scope.$watch('fareRangeInfoLoaded',
                  function (newValue) {
                      scope.loadingFareRange = angular.copy(!newValue);
                      scope.$parent.divFareRange = newValue;
                  }
                );
                scope.IsMacOrigin = false;

                scope.loadfareRangeInfo = function () {
                    scope.fareRangeInfoLoaded = false;
                    scope.fareRangeInfoNoDataFound = false;
                    scope.SelectedLocation = "";
                    scope.fareRangeData = "";
                    if (scope.farerangeParams != undefined) {
                        scope.staydaylength = 0;
                        if (scope.farerangeParams.SearchCriteria.FromDate != null && scope.farerangeParams.SearchCriteria.ToDate != null) {
                            // replace(/-/g, "/") used because of safari date convert problem                            
                            var frdt = new Date(scope.farerangeParams.SearchCriteria.FromDate);
                            var todt = new Date(scope.farerangeParams.SearchCriteria.ToDate);
                            var timeDiff = Math.abs(todt.getTime() - frdt.getTime());
                            scope.staydaylength = Math.ceil(timeDiff / (1000 * 3600 * 24));
                        }
                        var LatestDepartureDate = new Date(scope.farerangeParams.Fareforecastdata.ReturnDate.split('T')[0].replace(/-/g, "/"));
                        LatestDepartureDate.setDate(LatestDepartureDate.getDate() + 5);

                        var daydiff = getLengthOfStay(scope.farerangeParams.Fareforecastdata.DepartureDate, LatestDepartureDate);
                        if (daydiff > 15) {
                            LatestDepartureDate = new Date(scope.farerangeParams.Fareforecastdata.DepartureDate.split('T')[0].replace(/-/g, "/"));
                            LatestDepartureDate.setDate(LatestDepartureDate.getDate() + 14);
                        }
                        LatestDepartureDate = $filter('date')(LatestDepartureDate, 'yyyy-MM-dd')
                        var data = {
                            "Origin": scope.farerangeParams.Fareforecastdata.Origin.toUpperCase(),
                            "Destination": scope.farerangeParams.Fareforecastdata.Destination,
                            "EarliestDepartureDate": scope.farerangeParams.Fareforecastdata.DepartureDate,
                            "LatestDepartureDate": LatestDepartureDate,
                            "Lengthofstay": scope.staydaylength  //TrippismConstants.DefaultLenghtOfStay
                        };
                        if (scope.fareRangeInfoLoaded == false) {
                            if (scope.fareRangeData == "") {
                                scope.farerangepromise = FareRangeFactory.fareRange(data).then(function (data) {
                                    scope.FareRangeLoading = false;
                                    if (data.status == 404 || data.status == 400) {
                                        scope.fareRangeInfoNoDataFound = true;
                                        return;
                                    }
                                    var originairport = _.find(scope.farerangeParams.AvailableAirports, function (airport) { return airport.airport_Code == scope.farerangeParams.Fareforecastdata.Origin });
                                    var destinationairport = _.find(scope.farerangeParams.AvailableAirports, function (airport) { return airport.airport_Code == scope.farerangeParams.Fareforecastdata.Destination });

                                    // Done this code where citycode and airport code is same
                                    var originList = _.where(scope.farerangeParams.AvailableAirports, { airport_CityCode: scope.farerangeParams.Fareforecastdata.Origin });
                                    var originairportObj = _.find(originList, function (airport) { return airport.airport_IsMAC == true });
                                    if (originairportObj != undefined) originairport.airport_IsMAC = true;
                                    else originairport.airport_IsMAC = false;

                                    var desList = _.where(scope.farerangeParams.AvailableAirports, { airport_CityCode: scope.farerangeParams.Fareforecastdata.Destination });
                                    var destinationairportObj = _.find(desList, function (airport) { return airport.airport_IsMAC == true });
                                    if (destinationairportObj != undefined) destinationairport.airport_IsMAC = true;
                                    else destinationairport.airport_IsMAC = false;

                                    if (originairport != undefined && destinationairport != undefined) {
                                        // If both airports are not MAC
                                        if (!originairport.airport_IsMAC && !destinationairport.airport_IsMAC) {
                                            scope.IsMacOrigin = false;
                                            data.IsMacOrigin = false
                                            scope.fareRangeData = data;
                                        }
                                            // If Origin airport is MAC and Destination is not
                                        else if ((originairport.airport_IsMAC && !destinationairport.airport_IsMAC) || (originairport.airport_IsMAC && destinationairport.airport_IsMAC)) {

                                            scope.IsMacOrigin = true;
                                            var origins = _.groupBy(data.FareData, 'OriginLocation');
                                            if (origins != undefined) {
                                                var MinimumLocation = [];
                                                _.each(origins, function (org) {
                                                    MinimumLocation.push(_.min(org, function (loc) { return loc.MinimumFare; }));
                                                });
                                                var MinSelectedLocation = _.min(MinimumLocation, function (loc) { return loc.MinimumFare; });

                                                var locationairport = _.find(scope.farerangeParams.AvailableAirports, function (airport) { return airport.airport_Code == MinSelectedLocation.OriginLocation.toUpperCase() });
                                                if (locationairport != undefined)
                                                    scope.SelectedLocation = MinSelectedLocation.OriginLocation + ', ' + locationairport.airport_FullName + ", " + locationairport.airport_CityName;

                                                var faredata = {
                                                    DestinationLocation: MinSelectedLocation.DestinationLocation,
                                                    OriginLocation: MinSelectedLocation.OriginLocation,
                                                    IsMacOrigin: true,
                                                    SelectedLocation: scope.SelectedLocation,
                                                    FareData: origins[MinSelectedLocation.OriginLocation]
                                                };
                                                scope.fareRangeData = faredata;
                                            }
                                        }
                                            // If Destination airport is MAC and Origin  is not
                                        else if (!originairport.airport_IsMAC && destinationairport.airport_IsMAC) {
                                            scope.IsMacDestination = true;
                                            var destinations = _.groupBy(data.FareData, 'DestinationLocation');
                                            if (destinations != undefined) {
                                                var MinimumLocation = [];
                                                _.each(destinations, function (org) {
                                                    MinimumLocation.push(_.min(org, function (loc) { return loc.MinimumFare; }));
                                                });
                                                var MinSelectedLocation = _.min(MinimumLocation, function (loc) { return loc.MinimumFare; });

                                                var locationairport = _.find(scope.farerangeParams.AvailableAirports, function (airport) { return airport.airport_Code == MinSelectedLocation.DestinationLocation.toUpperCase() });
                                                if (locationairport != undefined)
                                                    scope.SelectedDestinationLocation = MinSelectedLocation.DestinationLocation + ', ' + locationairport.airport_FullName + ", " + locationairport.airport_CityName;

                                                var faredata = {
                                                    DestinationLocation: MinSelectedLocation.DestinationLocation,
                                                    OriginLocation: MinSelectedLocation.OriginLocation,
                                                    IsMacOrigin: false,
                                                    IsMacDestination: true,
                                                    SelectedLocation: '',
                                                    SelectedDestinationLocation: scope.SelectedDestinationLocation,
                                                    FareData: destinations[MinSelectedLocation.DestinationLocation]
                                                };
                                                scope.fareRangeData = faredata;
                                            }
                                        }
                                        else if (data.FareData != undefined && data.FareData[0].OriginLocation == undefined) {
                                            scope.fareRangeData = data;
                                        }

                                        scope.farerangeParams.FareRangeData = scope.fareRangeData;
                                    }
                                    scope.fareRangeInfoLoaded = true;
                                });
                            }
                        }
                    }
                };

                scope.$watch('fareRangeData', function (newValue, oldValue) {
                    if (newValue != oldValue)
                        DisplayChart();
                })

                scope.Chart = [];
                function DisplayChart() {
                    var chartDataMax = [];
                    var chartDataMin = [];
                    var chartDataMed = [];
                    var startdate;
                    if (scope.fareRangeData != undefined && scope.fareRangeData != "") {
                        for (i = 0; i < scope.fareRangeData.FareData.length; i++) {
                            // replace(/-/g, "/") used because of safari date convert problem                               
                            var DepartureDate = new Date(scope.fareRangeData.FareData[i].DepartureDateTime.split('T')[0].replace(/-/g, "/"));
                            var returnDate = new Date(scope.fareRangeData.FareData[i].ReturnDateTime.split('T')[0].replace(/-/g, "/"));
                            if (i == 0) {
                                startdate = Date.UTC(DepartureDate.getFullYear(), DepartureDate.getMonth(), DepartureDate.getDate());
                                firstCurrencyCode = scope.fareRangeData.FareData[i].CurrencyCode;
                            }

                            var utcdate = Date.UTC(DepartureDate.getFullYear(), DepartureDate.getMonth(), DepartureDate.getDate());
                            var retutcdate = Date.UTC(returnDate.getFullYear(), returnDate.getMonth(), returnDate.getDate());

                            var seriseMax = {
                                x: utcdate,
                                y: scope.fareRangeData.FareData[i].MaximumFare,
                                z: scope.fareRangeData.FareData[i].MaximumFare,
                                returndate: retutcdate,
                                CurrencyCode: scope.fareRangeData.FareData[i].CurrencyCode
                            };
                            var seriseMin = {
                                x: utcdate,
                                y: scope.fareRangeData.FareData[i].MinimumFare,
                                z: scope.fareRangeData.FareData[i].MinimumFare,
                                returndate: retutcdate,
                                CurrencyCode: scope.fareRangeData.FareData[i].CurrencyCode
                            };
                            var seriseMed = {
                                x: utcdate,
                                y: scope.fareRangeData.FareData[i].MedianFare,
                                z: scope.fareRangeData.FareData[i].MedianFare,
                                returndate: retutcdate,
                                CurrencyCode: scope.fareRangeData.FareData[i].CurrencyCode
                            };
                            chartDataMax.push(seriseMax);
                            chartDataMin.push(seriseMin);
                            chartDataMed.push(seriseMed);
                        }
                        var options = {
                            chart: {
                                height: scope.chartHeight,
                                type: 'column',
                                renderTo: scope.TabIndex,
                            },
                            title: {
                                text: (scope.IsMacOrigin) ? scope.SelectedLocation : (scope.IsMacDestination ? 'dest ' + scope.SelectedDestinationLocation : '')
                            },
                            xAxis: {
                                type: 'datetime',
                                labels: {
                                    formatter: function () {
                                        var returndate = new Date($filter('date')(this.value, scope.format, null));
                                        returndate.setDate(returndate.getDate() + scope.staydaylength + 1)
                                        return Highcharts.dateFormat(TrippismConstants.HighChartDateFormat, this.value) + ' -<br> ' + Highcharts.dateFormat(TrippismConstants.HighChartDateFormat, returndate);
                                    },
                                    rotation: -45
                                },
                                title: {
                                    text: 'Departure Date - Return Date'
                                }
                            },
                            yAxis: { title: { text: 'Fare Rate in ' + firstCurrencyCode } },
                            legend: {
                                enabled: true
                            },
                            plotOptions: {
                                series: {
                                    borderWidth: 0,
                                    dataLabels: {
                                        //allowOverlap:true,
                                        crop: false,
                                        overflow: 'none',
                                        enabled: true,
                                        align: 'right',
                                        format: TrippismConstants.HighChartTwoDecimalCurrencyFormat,
                                        style: {
                                            fontWeight: 'bold',
                                            fontSize: '10px'
                                        },
                                        rotation: -90
                                    }
                                }
                            },
                            tooltip: {
                                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>' + TrippismConstants.HighChartTwoDecimalCurrencyFormat + '$</b><br/>',
                                formatter: function () {
                                    return '<span style="font-size:11px;color:#87ceeb"> Fare Detail </span><br>' +
                                        '<span style="color:#87ceeb"> Date : </span><b> [ ' + Highcharts.dateFormat(TrippismConstants.HighChartDateFormat, new Date(this.x)) + ' - ' + Highcharts.dateFormat(TrippismConstants.HighChartDateFormat, new Date(this.point.returndate)) + ' ] </b><br>' +
                                        '<span style="color:#87ceeb">' + this.series.name + ' : </span><b>' + this.point.CurrencyCode + ' ' + Highcharts.numberFormat(this.point.y, 2) + '</b>';
                                }
                            },
                            series: [{
                                name: "Minimum Fare",
                                data: chartDataMin,
                                pointStart: startdate,
                                color: '#adff2f',
                                pointInterval: 24 * 3600 * 1000 // one day
                            },
                            {
                                name: "Median Fare",
                                data: chartDataMed,
                                pointStart: startdate,
                                color: '#2e8b57',
                                pointInterval: 24 * 3600 * 1000 // one day
                            }, {
                                name: "Maximum Fare",
                                data: chartDataMax,
                                pointStart: startdate,
                                color: '#87ceeb',
                                pointInterval: 24 * 3600 * 1000 // one day
                            }]
                        };

                        $timeout(function () {
                            scope.Chart = new Highcharts.Chart(options);
                        }, 0, false);
                    }
                }
            }
        }
    }]);
