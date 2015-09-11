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

                scope.loadSeasonalityInfo = function () {
                    scope.SeasonalityWidgetDataFound = false;
                    if (scope.widgetParams != undefined) {
                        var Seasonalitydata = {
                            "Destination": scope.widgetParams.Destinatrion, // JFK
                        };

                        $timeout(function () {
                            scope.seasonalitypromise = SeasonalityFactory.Seasonality(Seasonalitydata).then(function (data) {
                                if (data.status == 404) {
                                    scope.SeasonalityWidgetDataFound = false;
                                    return;
                                }
                                scope.widgetParams.NoDataFound = false;
                                scope.SeasonalityData = data.Seasonality;
                                if (scope.SeasonalityData != undefined && scope.SeasonalityData != "") {
                                    var chartrec = _.sortBy(scope.SeasonalityData, 'WeekStartDate');
                                    var FrmDate = new Date(scope.widgetParams.Fareforecastdata.DepartureDate);
                                    var Todate = new Date(scope.widgetParams.Fareforecastdata.ReturnDate)
                                    for (i = 0; i < chartrec.length; i++) {
                                        var WeekStartDate = new Date(chartrec[i].WeekStartDate);
                                        if (WeekStartDate >= FrmDate && WeekStartDate <= Todate) {
                                            var NumberOfObervations = "";
                                            if (chartrec[i].NumberOfObservations == "GreaterThan10000")
                                                NumberOfObervations = 3;
                                            if (chartrec[i].NumberOfObservations == "LessThan10000")
                                                NumberOfObervations = 2;
                                            if (chartrec[i].NumberOfObservations == "LessThan1000")
                                                NumberOfObervations = 1;
                                            scope.SeasonalityWidgetData = {
                                                NoofIcons: NumberOfObervations
                                            };
                                            scope.SeasonalityWidgetDataFound = true;
                                            break;
                                        }
                                        scope.$apply();
                                    }
                                }

                            });
                        }, 0, false);
                    }

                };

                scope.FareRangeWidgetDataFound = false;
                scope.loadfareRangeInfo = function () {
                    if (scope.widgetParams != undefined) {
                        scope.staydaylength = 0;
                        if (scope.widgetParams.SearchCriteria.FromDate != null && scope.widgetParams.SearchCriteria.ToDate != null) {
                            var frdt = new Date(scope.widgetParams.SearchCriteria.FromDate);
                            var todt = new Date(scope.widgetParams.SearchCriteria.ToDate);
                            var timeDiff = Math.abs(todt.getTime() - frdt.getTime());
                            scope.staydaylength = Math.ceil(timeDiff / (1000 * 3600 * 24));
                        }

                        var data = {
                            "Origin": scope.widgetParams.Fareforecastdata.Origin,
                            "Destination": scope.widgetParams.Fareforecastdata.Destination,
                            "EarliestDepartureDate": scope.widgetParams.Fareforecastdata.DepartureDate,
                            "LatestDepartureDate": scope.widgetParams.Fareforecastdata.ReturnDate,
                            "Lengthofstay": scope.staydaylength  //TrippismConstants.DefaultLenghtOfStay
                        };


                        scope.farerangepromise = FareRangeFactory.fareRange(data).then(function (data) {
                            scope.FareRangeLoading = false;
                            if (data.status == 404 || data.status == 400) {
                                scope.fareRangeInfoNoDataFound = true;
                                return;
                            }

                            scope.fareRangeData = data;
                            if (scope.fareRangeData != undefined && scope.fareRangeData != "") {

                                var FrmDate = new Date(scope.widgetParams.Fareforecastdata.DepartureDate);
                                var Todate = new Date(scope.widgetParams.Fareforecastdata.ReturnDate)

                                for (i = 0; i < scope.fareRangeData.FareData.length; i++) {

                                    var WeekStartDate = new Date(scope.fareRangeData.FareData[i].DepartureDateTime);
                                    if (WeekStartDate >= FrmDate && WeekStartDate <= Todate) {
                                        scope.FareRangeWidgetData = {
                                            MinimumFare: scope.fareRangeData.FareData[i].MinimumFare,
                                            MaximumFare: scope.fareRangeData.FareData[i].MaximumFare,
                                            MedianFare: scope.fareRangeData.FareData[i].MedianFare
                                        };
                                        scope.FareRangeWidgetDataFound = true;
                                        break;
                                    }
                                    scope.$apply();

                                }
                            }
                        });
                    }
                };

                scope.$watchGroup(['widgetParams'], function (newValue, oldValue, scope) {
                    if (scope.widgetParams != undefined) {
                        scope.DepartDate = $filter('date')(scope.widgetParams.Fareforecastdata.DepartureDate, scope.format, null);
                        scope.ReturnDate = $filter('date')(scope.widgetParams.Fareforecastdata.ReturnDate, scope.format, null);
                    }
                    scope.loadSeasonalityInfo();
                    scope.loadfareRangeInfo();
                });
            }
        }
    }]);