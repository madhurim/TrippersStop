angular.module('TrippismUIApp').directive('weatherInfo', ['$compile', '$filter', '$timeout', '$rootScope', 'WeatherFactory', 'UtilFactory', 'TrippismConstants',
    function ($compile, $filter, $timeout, $rootScope, WeatherFactory, UtilFactory, TrippismConstants) {
        return {
            restrict: 'E',
            scope: {
                weatherParams: '=',
            },
            templateUrl: '/app/Views/Partials/WeatherPartial.html',
            link: function (scope, elem, attrs) {

                scope.formats = Dateformat();
                scope.format = scope.formats[5];
                scope.WeatherInfoNoDataFound = true;
                scope.weatherParams.WeatherInfoNoDataFound = true;
                scope.chartHeight = 300;
                scope.StateList = [];
                UtilFactory.ReadStateJson().then(function (data) {
                    scope.StateList = data;
                });

                scope.$watchGroup(['weatherParams'], function (newValue, oldValue, scope) {

                    UtilFactory.ReadStateJson().then(function (data) {
                        scope.StateList = data;
                        if (scope.weatherParams != undefined && scope.weatherParams.tabIndex != undefined) {
                            scope.DepartDate = $filter('date')(scope.weatherParams.Fareforecastdata.DepartureDate, scope.format, null);
                            scope.ReturnDate = $filter('date')(scope.weatherParams.Fareforecastdata.ReturnDate, scope.format, null);
                            scope.TabIndex = "weather" + scope.weatherParams.tabIndex;
                            var mapHTML = "<div id='" + scope.TabIndex + "'></div>";
                            elem.append($compile(mapHTML)(scope));
                            scope.WeatherRangeInfo();

                        } else {
                            scope.WeatherData = "";
                        }
                    });
                });
                function removeElement(element) {
                    element && element.parentNode && element.parentNode.removeChild(element);
                }

                scope.WeatherRangeInfo = function () {
                    scope.WeatherInfoLoaded = false;

                    scope.HighTempratureC = "0";
                    scope.HighTempratureF = "0";
                    scope.LowTempratureC = "0";
                    scope.LowTempratureF = "0";
                    if (scope.weatherParams != undefined) {

                        var statedata = _.find(scope.StateList, function (state) { return state.CityName == scope.weatherParams.DestinationairportName.airport_CityName });
                        if (statedata == undefined) {
                            scope.WeatherData = "";
                            scope.WeatherInfoNoDataFound = true;
                            scope.weatherParams.WeatherInfoNoDataFound = true;
                        }
                        else {

                            scope.WeatherData = "";
                            var data = {
                                "CountryCode": scope.weatherParams.DestinationairportName.airport_CountryCode,
                                "AirportCode": scope.weatherParams.DestinationairportName.airport_Code,//scope.weatherParams.DestinationairportName.airport_CityName,
                                "DepartDate": $filter('date')(scope.weatherParams.Fareforecastdata.DepartureDate, scope.format, null),
                                "ReturnDate": $filter('date')(scope.weatherParams.Fareforecastdata.ReturnDate, scope.format, null)
                            };

                            if (scope.WeatherInfoLoaded == false) {
                                if (scope.WeatherData == "") {
                                    scope.Weatherpromise = WeatherFactory.GetData(data).then(function (data) {
                                        scope.WeatherInfoLoaded = false;
                                        if (data == "" || data.status == 404) {
                                            scope.WeatherInfoNoDataFound = true;
                                            scope.weatherParams.WeatherInfoNoDataFound = false;
                                            return;
                                        }
                                        scope.WeatherInfoNoDataFound = false;
                                        scope.weatherParams.WeatherInfoNoDataFound = false;
                                        scope.WeatherData = data;
                                    });
                                }
                            }
                        }
                        scope.WeatherInfoLoaded = true;
                    }
                };

                scope.$watch('WeatherData', function (newValue, oldValue) {
                    if (newValue != oldValue)
                        DisplayChart();
                })

                scope.Chart = [];
                function DisplayChart() {
                    var chartData = [];
                    if (scope.WeatherData != undefined && scope.WeatherData != "") {

                        if (scope.WeatherData.TempHighAvg != undefined) {
                            scope.HighTempratureF = scope.WeatherData.TempHighAvg.Avg.F;
                            scope.HighTempratureC = scope.WeatherData.TempHighAvg.Avg.C;
                        }

                        if (scope.WeatherData.TempLowAvg != undefined) {
                            scope.LowTempratureC = scope.WeatherData.TempLowAvg.Avg.C;
                            scope.LowTempratureF = scope.WeatherData.TempLowAvg.Avg.F;
                        }

                        // if (scope.WeatherData.WeatherChances != undefined && scope.WeatherData.WeatherChances.length > 0) {
                        for (i = 0; i < scope.WeatherData.WeatherChances.length; i++) {
                            var datas = {
                                name: (scope.WeatherData.WeatherChances[i].Name == 'Sweltering') ? 'Very Hot' : scope.WeatherData.WeatherChances[i].Name,
                                y: scope.WeatherData.WeatherChances[i].Percentage
                            };
                            chartData.push(datas);
                        }
                        //   }

                        //$('#weatherChart').highcharts({
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
                                type: 'category',
                                title: {
                                    text: 'Weather Type'
                                }
                            },
                            yAxis: {
                                tickInterval: 25,
                                min: 0,
                                max: 100,
                                title: {
                                    text: 'Percentage'
                                }
                            },
                            legend: {
                                enabled: false
                            },
                            plotOptions: {
                                series: {
                                    borderWidth: 0,
                                    dataLabels: {
                                        enabled: true,
                                        format: '{point.y:.1f}%'
                                    }
                                }
                            },
                            tooltip: {
                                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>' + TrippismConstants.HighChartTwoDecimalCurrencyFormat + '%</b><br/>'
                            },
                            series: [{
                                name: "Temprature",
                                colorByPoint: false,
                                data: chartData
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