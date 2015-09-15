angular.module('TrippismUIApp').directive('weatherwidgetInfo', ['$compile', '$filter', '$timeout', '$rootScope', 'WeatherFactory', 'UtilFactory', 'TrippismConstants',
    function ($compile, $filter, $timeout, $rootScope, WeatherFactory, UtilFactory, TrippismConstants) {
        return {
            restrict: 'E',
            scope: {
                weatherParams: '=',
            },
            templateUrl: '/app/Views/Partials/WeatherWidgetPartial.html',
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
                scope.IsWidgetClosed = true;
                scope.closeWidget = function () {
                    scope.IsWidgetClosed = false;
                }
                scope.$watchGroup(['weatherParams'], function (newValue, oldValue, scope) {

                    UtilFactory.ReadStateJson().then(function (data) {
                        scope.StateList = data;
                        if (scope.weatherParams != undefined && scope.weatherParams.tabIndex != undefined) {
                            scope.DepartDate = $filter('date')(scope.weatherParams.Fareforecastdata.DepartureDate, scope.format, null);
                            scope.ReturnDate = $filter('date')(scope.weatherParams.Fareforecastdata.ReturnDate, scope.format, null);
                            scope.TabIndex = "weatherwidget" + scope.weatherParams.tabIndex;
                            var mapHTML = "<div id='" + scope.TabIndex + "'></div>";
                            elem.append($compile(mapHTML)(scope));
                            scope.WeatherRangeInfo();

                        } else {
                            scope.WeatherwidgetData = "";
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
                            scope.WeatherwidgetData = "";
                            scope.WeatherInfoNoDataFound = true;
                            scope.weatherParams.WeatherInfoNoDataFound = true;
                        }
                        else {
                            scope.WeatherwidgetData = "";
                            var data = {
                                "CountryCode": scope.weatherParams.DestinationairportName.airport_CountryCode,
                                "AirportCode": scope.weatherParams.DestinationairportName.airport_Code,//scope.weatherParams.DestinationairportName.airport_CityName,
                                "DepartDate": $filter('date')(scope.weatherParams.Fareforecastdata.DepartureDate, scope.format, null),
                                "ReturnDate": $filter('date')(scope.weatherParams.Fareforecastdata.ReturnDate, scope.format, null)
                            };

                            if (scope.WeatherInfoLoaded == false) {
                                if (scope.WeatherwidgetData == "") {
                                    scope.Weatherpromise = WeatherFactory.GetData(data).then(function (data) {
                                        scope.WeatherInfoLoaded = false;
                                        if (data == "" || data.status == 404) {
                                            scope.WeatherInfoNoDataFound = true;
                                            scope.weatherParams.WeatherInfoNoDataFound = false;
                                            return;
                                        }
                                        scope.WeatherInfoNoDataFound = false;
                                        scope.weatherParams.WeatherInfoNoDataFound = false;
                                        scope.WeatherwidgetData = data;

                                        if (scope.WeatherwidgetData != undefined && scope.WeatherwidgetData != "") {
                                            
                                            var participation = _.find(scope.WeatherwidgetData.WeatherChances, function (chances) { return chances.Name == 'Precipitation' });
                                            var rain = _.find(scope.WeatherwidgetData.WeatherChances, function (chances) { return chances.Name == 'Rain' });
                                            if (participation != undefined && rain != undefined) {
                                                if (participation.Percentage >= 60 && rain.Percentage >= 60)
                                                    scope.IsparticipationDisplay = false;
                                                else
                                                    scope.IsparticipationDisplay = true;
                                            }
                                            else {
                                                scope.IsparticipationDisplay = true;
                                            }
                                            
                                            if (scope.WeatherwidgetData.TempHighAvg != undefined) {
                                                scope.HighTempratureF = scope.WeatherwidgetData.TempHighAvg.Avg.F;
                                                scope.HighTempratureC = scope.WeatherwidgetData.TempHighAvg.Avg.C;
                                            }

                                            if (scope.WeatherwidgetData.TempLowAvg != undefined) {
                                                scope.LowTempratureC = scope.WeatherwidgetData.TempLowAvg.Avg.C;
                                                scope.LowTempratureF = scope.WeatherwidgetData.TempLowAvg.Avg.F;
                                            }
                                        }
                                    });
                                }
                            }
                        }
                        scope.WeatherInfoLoaded = true;
                    }
                };

            }
        }
    }]);