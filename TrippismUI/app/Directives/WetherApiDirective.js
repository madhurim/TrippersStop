angular.module('TrippismUIApp').directive('weatherInfo', ['$compile', '$filter', '$timeout', '$rootScope', 'WeatherFactory', 'UtilFactory',
    function ($compile, $filter, $timeout, $rootScope, WeatherFactory, UtilFactory) {
        return {
            restrict: 'E',
            scope: {
                weatherParams: '=',
                isOpen: '='
            },
            templateUrl: '/app/Views/Partials/WeatherPartial.html',
            link: function (scope, elem, attrs) {

                scope.formats = Dateformat();
                scope.format = scope.formats[5];

                //scope.TabIndex = "weather" + scope.weatherParams.tabIndex;
                //var mapHTML = "<div id='" + scope.TabIndex + "'></div>";
                //elem.append($compile(mapHTML)(scope));

                scope.chartHeight = 200;
                scope.StateList = [];
                UtilFactory.ReadStateJson().then(function (data) {
                    scope.StateList = data;
                });

                scope.$watchGroup(['weatherParams', 'isOpen'], function (newValue, oldValue, scope) {

                    UtilFactory.ReadStateJson().then(function (data) {
                        scope.StateList = data;
                        if (scope.weatherParams != undefined && scope.weatherParams.tabIndex != undefined) {

                            scope.DepartDate = $filter('date')(scope.weatherParams.Fareforecastdata.DepartureDate, scope.format, null);
                            scope.ReturnDate = $filter('date')(scope.weatherParams.Fareforecastdata.ReturnDate, scope.format, null);

                            if (scope.weatherParams.tabIndex == 999) {
                                var elementcls = document.getElementsByClassName("weatherinmaindiv");
                                if (elementcls.length > 0) {
                                    for (var i = 0; i < elementcls.length; i++)
                                        removeElement(elementcls[i]);
                                }
                            }
                            scope.TabIndex = "weather" + scope.weatherParams.tabIndex;
                            var mapHTML = "";

                            if (scope.weatherParams.tabIndex == 999)
                                mapHTML = "<div id='" + scope.TabIndex + "' class='weatherinmaindiv' ></div>";
                            else
                                mapHTML = "<div id='" + scope.TabIndex + "'></div>";
                            elem.append($compile(mapHTML)(scope));
                            if (scope.weatherParams.tabIndex == 999)
                                document.getElementById(scope.TabIndex).innerHTML = "";
                            scope.WeatherRangeInfo();

                        } else {
                            scope.WeatherData = "";
                        }
                    });

                    //scope.WeatherRangeInfo();
                    //if (scope.isOpen == true) {
                    //    if (newValue != oldValue)
                    //        scope.WeatherRangeInfo();
                    //}
                    //else {
                    //    scope.WeatherData = "";
                    //}
                });

                //UtilFactory.ReadStateJson().then(function (data) {
                //    scope.StateList = data;
                //});

                function removeElement(element) {
                    element && element.parentNode && element.parentNode.removeChild(element);
                }

                scope.WeatherRangeInfo = function () {
                    scope.WeatherInfoLoaded = false;
                    scope.WeatherInfoNoDataFound = false;
                    scope.HighTempratureC = "0";
                    scope.HighTempratureF = "0";
                    scope.LowTempratureC = "0";
                    scope.LowTempratureF = "0";
                    if (scope.weatherParams != undefined) {

                        var statedata = _.find(scope.StateList, function (state) { return state.CityName == scope.weatherParams.DestinationairportName.airport_CityName });
                        if (statedata == undefined) {
                            scope.WeatherData = "";
                            scope.WeatherInfoNoDataFound = true;
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
                                            return;
                                        }
                                        if (scope.weatherParams.tabIndex == 999) {
                                            scope.chartHeight = 200;
                                            $rootScope.$broadcast('divWeatherEvent', true);
                                        }
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
                                name: scope.WeatherData.WeatherChances[i].Name,
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
                                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b><br/>'
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