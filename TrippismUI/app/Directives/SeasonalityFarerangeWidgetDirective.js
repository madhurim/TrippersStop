angular.module('TrippismUIApp').directive('seasonalityfarerangewidgetInfo',
                ['$compile', 'SeasonalityFactory', 'FareRangeFactory', '$filter', '$timeout', '$rootScope', 'UtilFactory', 'TrippismConstants',
    function ($compile, SeasonalityFactory, FareRangeFactory, $filter, $timeout, $rootScope, UtilFactory, TrippismConstants) {
        return {
            restrict: 'E',
            scope: {
                widgetParams: '=',
            },
            templateUrl: '/app/Views/Partials/SeasonalityFarerangeWidget.html',
            link: function (scope, elem, attrs) {
                scope.IsWidgetClosed = true;
                scope.formats = Dateformat();
                scope.format = scope.formats[5];
                scope.closeWidget = function () {
                    scope.IsWidgetClosed = false;
                }

                scope.SeasonalityWidgetData = undefined;
                scope.SeasonalityWidgetDataFound = false;

                scope.loadSeasonalityInfo = function () {
                    scope.SeasonalityWidgetDataFound = false;
                    if (scope.widgetParams != undefined) {
                        scope.SeasonalityData = scope.widgetParams.SeasonalityData;
                        if (scope.SeasonalityData != undefined && scope.SeasonalityData != "")
                            setSeasonalityData();
                    }
                };

                function setSeasonalityData() {

                    var chartrec = _.sortBy(scope.SeasonalityData, 'WeekStartDate');

                    var FrmDate = new Date(scope.widgetParams.Fareforecastdata.DepartureDate);
                    var Todate = new Date(scope.widgetParams.Fareforecastdata.ReturnDate)
                    for (i = 0; i < chartrec.length; i++) {

                        var WeekStartDate = new Date(chartrec[i].WeekStartDate);
                        if (WeekStartDate >= FrmDate && WeekStartDate <= Todate) {
                            var SeasonalityIndicator = "";
                            if (chartrec[i].SeasonalityIndicator == "High")
                                NumberOfObervations = 3;
                            if (chartrec[i].SeasonalityIndicator == "Medium")
                                NumberOfObervations = 2;
                            if (chartrec[i].SeasonalityIndicator == "Low")
                                NumberOfObervations = 1;
                            scope.SeasonalityWidgetData = {
                                NoofIcons: NumberOfObervations
                            };
                            scope.SeasonalityWidgetDataFound = true;
                            return;
                        }
                        else if (WeekStartDate > FrmDate) {
                            var NumberOfObervations = "";
                            if (chartrec[i].SeasonalityIndicator == "High")
                                NumberOfObervations = 3;
                            if (chartrec[i].SeasonalityIndicator == "Medium")
                                NumberOfObervations = 2;
                            if (chartrec[i].SeasonalityIndicator == "Low")
                                NumberOfObervations = 1;
                            scope.SeasonalityWidgetData = {
                                NoofIcons: NumberOfObervations
                            };
                            scope.SeasonalityWidgetDataFound = true;
                        }
                        //scope.$apply();
                    }
                }

                scope.FareRangeWidgetDataFound = false;
                scope.loadfareRangeInfo = function () {
                    scope.FareRangeWidgetDataFound = false;
                    if (scope.widgetParams != undefined) {
                        scope.fareRangeData = scope.widgetParams.FareRangeData;
                        
                        if (scope.fareRangeData != undefined && scope.fareRangeData != "") {

                            var FrmDate = new Date(scope.widgetParams.Fareforecastdata.DepartureDate);
                            var Todate = new Date(scope.widgetParams.Fareforecastdata.ReturnDate);
                            for (i = 0; i < scope.fareRangeData.FareData.length; i++) {
                                var WeekStartDate = new Date(scope.fareRangeData.FareData[i].DepartureDateTime);
                                if (WeekStartDate >= FrmDate && WeekStartDate <= Todate) {
                                    scope.FareRangeWidgetData = {
                                        MinimumFare: scope.fareRangeData.FareData[i].MinimumFare,
                                        MaximumFare: scope.fareRangeData.FareData[i].MaximumFare,
                                        MedianFare: scope.fareRangeData.FareData[i].MedianFare,
                                        CurrencyCode: scope.fareRangeData.FareData[i].CurrencyCode
                                    };
                                    scope.FareRangeWidgetDataFound = true;
                                    break;
                                }
                                scope.$apply();
                            }
                        }
                    }
                };

                scope.$watch('widgetParams.FareRangeData', function (newValue, oldval) {
                    if (newValue != undefined) {
                        scope.loadfareRangeInfo();
                    }
                });

                scope.$watch('widgetParams.SeasonalityData', function (newValue, oldval) {
                    if (newValue != undefined)
                        scope.loadSeasonalityInfo();
                });
                scope.$watchGroup(['widgetParams'], function (newValue, oldValue, scope) {
                    if (scope.widgetParams != undefined) {
                        scope.DepartDate = $filter('date')(scope.widgetParams.Fareforecastdata.DepartureDate, scope.format, null);
                        scope.ReturnDate = $filter('date')(scope.widgetParams.Fareforecastdata.ReturnDate, scope.format, null);
                    }
                });
            }
        }
    }]);