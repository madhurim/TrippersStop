angular.module('TrippismUIApp').directive('weatherwidgetInfo', ['$compile', '$filter', '$timeout', '$rootScope', 'WeatherFactory', 'UtilFactory', 'TrippismConstants', 'GoogleGeoReverseLookupFactory',
    function ($compile, $filter, $timeout, $rootScope, WeatherFactory, UtilFactory, TrippismConstants, GoogleGeoReverseLookupFactory) {
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
                scope.IsWidgetClosed = true;
                scope.closeWidget = function () {
                    scope.IsWidgetClosed = false;
                }
                scope.$watchGroup(['weatherParams'], function (newValue, oldValue, scope) {

                    if (scope.weatherParams != undefined && scope.weatherParams.tabIndex != undefined) {
                        scope.DepartDate = $filter('date')(scope.weatherParams.Fareforecastdata.DepartureDate, scope.format, null);
                        scope.ReturnDate = $filter('date')(scope.weatherParams.Fareforecastdata.ReturnDate, scope.format, null);
                        scope.TabIndex = "weatherwidget" + scope.weatherParams.tabIndex;
                        var mapHTML = "<div id='" + scope.TabIndex + "'></div>";
                        elem.append($compile(mapHTML)(scope));


                    } else {
                        scope.WeatherwidgetData = "";
                    }
                     });
                scope.$watch('weatherParams.WeatherData', function (newValue, oldValue, scope) {                    
                    if (newValue == undefined) return;
                    scope.getWeatherInformation();
                });
                function removeElement(element) {
                    element && element.parentNode && element.parentNode.removeChild(element);
                }

                scope.getWeatherInformation = function () {
                    scope.HighTempratureC = "0";
                    scope.HighTempratureF = "0";
                    scope.LowTempratureC = "0";
                    scope.LowTempratureF = "0";
                    scope.WeatherInfoNoDataFound = true;
                    scope.WeatherwidgetData = scope.weatherParams.WeatherData;
                    if (scope.WeatherwidgetData != undefined && scope.WeatherwidgetData != "") {
                        scope.WeatherInfoNoDataFound = false;
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
                }
            }
        }
    }
]);
