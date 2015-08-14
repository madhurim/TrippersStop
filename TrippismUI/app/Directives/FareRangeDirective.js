angular.module('TrippismUIApp').directive('farerangeInfo', ['$compile', '$timeout', '$filter', '$rootScope', 'FareRangeFactory',
    function ($compile, $timeout, $filter, $rootScope, FareRangeFactory) {
    return {
        restrict: 'E',
        scope: {
            farerangeParams: '=', isOpen: '=', 
           // mailfarerangeData: '=',
            showChart: '='
        },
        templateUrl: '/app/Views/Partials/FareRangePartial.html',
        link: function (scope, elem, attrs) {
            scope.formats = Dateformat();
            scope.format = scope.formats[5];
            scope.$watchGroup(['farerangeParams', 'isOpen','showChart'], function (newValue, oldValue, scope) {

                //Add Scope For Chart
                if (scope.farerangeParams != undefined) {
                    scope.DepartDate = $filter('date')(scope.farerangeParams.Fareforecastdata.DepartureDate, scope.format, null);
                    scope.ReturnDate = $filter('date')(scope.farerangeParams.Fareforecastdata.ReturnDate, scope.format, null);
                    scope.chartHeight = 400;
                    scope.TabIndex = "farerange" + scope.farerangeParams.tabIndex;
                    var mapHTML = "<div id='" + scope.TabIndex + "'></div>";
                    elem.append($compile(mapHTML)(scope));
                }

                if (scope.isOpen == true) {
                  //  if (newValue != oldValue || (newValue == null && newValue == null))
                        scope.loadfareRangeInfo();
                } else {
                    scope.fareRangeData = "";
                    scope.mailfarerangeData = "";
                }
            });
            scope.loadingFareRange = true;
            scope.$parent.divFareRange = false;
            scope.$watch('fareRangeInfoLoaded',
              function (newValue) {
                  scope.loadingFareRange = angular.copy(!newValue);
                  scope.$parent.divFareRange = newValue;
              }
            );

          
            scope.loadfareRangeInfo = function () {
                scope.fareRangeInfoLoaded = false;
                scope.fareRangeInfoNoDataFound = false;
                scope.fareRangeData = "";
                scope.mailfarerangeData = "";
                if (scope.farerangeParams != undefined) {
                    var data = {
                        "Origin": scope.farerangeParams.Fareforecastdata.Origin,
                        "Destination": scope.farerangeParams.Fareforecastdata.Destination,
                        "EarliestDepartureDate": scope.farerangeParams.Fareforecastdata.DepartureDate,
                        "LatestDepartureDate": scope.farerangeParams.Fareforecastdata.ReturnDate,
                        "Lengthofstay": 4
                    };
                    if (scope.fareRangeInfoLoaded == false) {
                        if (scope.fareRangeData == "") {
                            scope.farerangepromise = FareRangeFactory.fareRange(data).then(function (data) {
                                scope.FareRangeLoading = false;
                                if (data.status == 404) {
                                    
                                    scope.fareRangeInfoNoDataFound = true;
                                    $rootScope.$broadcast('divFareRangeEvent', false, scope.Seasonalityresult);
                                    return;
                                }
                                scope.fareRangeData = data;
                                scope.mailfarerangeData = data;
                                scope.fareRangeInfoLoaded = true;
                                $rootScope.$broadcast('divFareRangeEvent', true, scope.Seasonalityresult);
                            });
                        }
                    }
                }
            };

            scope.$watch('fareRangeData', function (newValue, oldValue) {
                if (newValue != oldValue) {
                    if (scope.showChart == true) {
                        DisplayChart();
                    }
                }
            })

            scope.Chart = [];
            function DisplayChart() {
                var chartData1 = [];
                var chartData2 = [];
                var chartData3 = [];
                var startdate;
                if (scope.fareRangeData != undefined && scope.fareRangeData != "") {
                    for (i = 0; i < scope.fareRangeData.FareData.length; i++) {
                        var DepartureDate = new Date(scope.fareRangeData.FareData[i].DepartureDateTime);
                        var returnDate = new Date(scope.fareRangeData.FareData[i].ReturnDateTime);

                        if (i == 0)
                        {
                            startdate = Date.UTC(DepartureDate.getFullYear(), DepartureDate.getMonth(), DepartureDate.getDate());
                            firstCurrencyCode = scope.fareRangeData.FareData[i].CurrencyCode;
                        }
                       
                        var utcdate = Date.UTC(DepartureDate.getFullYear(), DepartureDate.getMonth(), DepartureDate.getDate());
                        var retutcdate = Date.UTC(returnDate.getFullYear(), returnDate.getMonth(), returnDate.getDate());
                        
                        var serise1 = {
                            x: utcdate,
                            y: scope.fareRangeData.FareData[i].MaximumFare,
                            z: scope.fareRangeData.FareData[i].MaximumFare,
                            returndate: retutcdate,
                            CurrencyCode: scope.fareRangeData.FareData[i].CurrencyCode
                        };
                        var serise2 = {
                            x: utcdate,
                            y: scope.fareRangeData.FareData[i].MinimumFare,
                            z: scope.fareRangeData.FareData[i].MinimumFare,
                            returndate: retutcdate,
                            CurrencyCode: scope.fareRangeData.FareData[i].CurrencyCode
                        };
                        var serise3 = {
                            x: utcdate,
                            y: scope.fareRangeData.FareData[i].MedianFare,
                            z: scope.fareRangeData.FareData[i].MedianFare,
                            returndate: retutcdate,
                            CurrencyCode: scope.fareRangeData.FareData[i].CurrencyCode
                        };
                        chartData1.push(serise1);
                        chartData2.push(serise2);
                        chartData3.push(serise3);
                    }

                    var options = {
                        chart: {
                            height: scope.chartHeight,
                            type: 'column',
                            renderTo: scope.TabIndex,
                        },
                        title: {
                            text: ''
                        },
                        xAxis: {
                            type: 'datetime',
                            labels:{ rotation : -45
                            },
                            dateTimeLabelFormats: {
                                day: '%m-%e-%Y',
                                month: '%e. %b',
                                year: '%b'
                            },
                            title: {
                                text:'Historical Fare Rate for date of [ '+Highcharts.dateFormat('%m-%e-%Y',new Date(scope.farerangeParams.Fareforecastdata.DepartureDate)) + ' To ' + Highcharts.dateFormat('%m-%e-%Y',new Date(scope.farerangeParams.Fareforecastdata.ReturnDate)) +' ]'
                            }
                        },
                        yAxis: {
                            title: {
                                text: 'Fare Rate in ' + firstCurrencyCode
                            }
                        },
                        legend: {
                            enabled: true
                        },
                        plotOptions: {
                            series: {
                                borderWidth: 0,
                                dataLabels: {
                                    enabled: true,
                                    format: firstCurrencyCode +' {point.y:.0f}'
                                }
                            }
                        },
                        tooltip: {
                            headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                            pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}$</b><br/>',
                            formatter: function () {
                                return '<span style="font-size:11px;color:#87ceeb"> Fare Detail </span><br>' +
                                    '<span style="color:#87ceeb"> Date : </span><b> [ ' + Highcharts.dateFormat('%m-%e-%Y', new Date(this.x)) + ' - ' + Highcharts.dateFormat('%m-%e-%Y', new Date(this.point.returndate)) + ' ] </b><br>' +
                                    '<span style="color:#87ceeb">' + this.series.name + ' : </span><b>' + this.point.CurrencyCode + ' ' + Highcharts.numberFormat(this.point.y, 0) + '</b>';
                            }
                        },
                        series: [{
                            name :"Minimum Fare",
                            data: chartData2,
                            pointStart: startdate,
                            color: '#adff2f',
                            pointInterval: 24 * 3600 * 1000 // one day
                        },
                        {
                            name :"Median Fare",
                            data: chartData3,
                            pointStart: startdate,
                            color: '#2e8b57',
                            pointInterval: 24 * 3600 * 1000 // one day
                        },{
                        name:"Maximum Fare",
                        data: chartData1,
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
