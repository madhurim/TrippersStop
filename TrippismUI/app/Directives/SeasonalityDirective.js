angular.module('TrippismUIApp').directive('seasonalityInfo', ['$compile', '$rootScope', '$timeout', '$filter', 'SeasonalityFactory',
    function ($compile, $rootScope, $timeout,$filter, SeasonalityFactory) {
    return {
        restrict: 'E',

        scope: {
            seasonalityParams: '=',
            isOpen: '=',
           // mailmarkereasonalityInfo: '=',
            showChart: '='
        },
        templateUrl: '/app/Views/Partials/SeasonalityPartial.html',
        link: function (scope, elem, attrs) {
            scope.formats = Dateformat();
            scope.format = scope.formats[5];
            scope.Isviewmoredisplayed = false;

            scope.SeasonalityDisplay = function () {
                scope.MarkerSeasonalityInfo.Seasonality = scope.SeasonalityData;
                 scope.mailmarkereasonalityInfo.Seasonality = scope.SeasonalityData;

                // Setting up fare data for email
                //scope.attractionParams.dataforEmail.SeasonalityDataForEmail = {};
                //scope.attractionParams.dataforEmail.SeasonalityDataForEmail = data;


                scope.Isviewmoredisplayed = true;
            };

            scope.$parent.divSeasonality = false;

            scope.loadingSeasonality = true;
            scope.$watch('loadSeasonalityInfoLoaded',
              function (newValue) {
                  scope.loadingSeasonality = angular.copy(!newValue);
                  scope.$parent.divSeasonality = newValue;
              }
            );
            
            scope.loadSeasonalityInfo = function () {
                scope.MarkerSeasonalityInfo = "";
                scope.loadSeasonalityInfoLoaded = false;
                scope.SeasonalityNoDataFound = false;
                if (scope.seasonalityParams != undefined) {
                    if (scope.loadSeasonalityInfoLoaded == false) {
                        if (scope.MarkerSeasonalityInfo == "") {
                            var Seasonalitydata = {
                                "Destination": scope.seasonalityParams.Destinatrion, // JFK
                            };

                            $timeout(function () {
                                scope.inProgressSeasonalityinfo = true;
                                scope.seasonalitypromise = SeasonalityFactory.Seasonality(Seasonalitydata).then(function (data) {

                                    if (data.status == 404){
                                        scope.SeasonalityNoDataFound = true;
                                        $rootScope.$broadcast('divSeasonalityEvent', false);
                                        return;
                                    }   
                                    debugger;
                                    scope.SeasonalityData = data.Seasonality;
                                    $rootScope.$broadcast('divSeasonalityEvent', true);

                                    var defaultSeasonality = data.Seasonality;
                                    var now = new Date();
                                    var NextDate = common.addDays(now, 30);
                                    
                                    var filteredSeasonalityData = [];
                                    for (var i = 0; i < defaultSeasonality.length; i++) {
                                        scope.MarkerSeasonalityInfo.Seasonality = [];
                                        var datetocheck = new Date(defaultSeasonality[i].WeekStartDate);
                                        if (datetocheck > now && datetocheck < NextDate)
                                            filteredSeasonalityData.push(defaultSeasonality[i]);
                                    }
                                    if (filteredSeasonalityData.length == 0) {
                                        for (var i = 0; i < 5; i++) 
                                            filteredSeasonalityData.push(defaultSeasonality[i]);
                                    }
                                    data.Seasonality = filteredSeasonalityData;
                                    scope.MarkerSeasonalityInfo = data;
                                    scope.mailmarkereasonalityInfo = data;


                                    // Setting up fare data for email
                                    //scope.attractionParams.dataforEmail.SeasonalityDataForEmail = {};
                                    //scope.attractionParams.dataforEmail.SeasonalityDataForEmail = data;

                                    
                                    scope.inProgressSeasonalityinfo = false;
                                    scope.loadSeasonalityInfoLoaded = true;
                                    
                                });
                            }, 0, false);
                        }
                    }
  
                }
            };
            scope.$watchGroup(['seasonalityParams', 'isOpen', 'showChart'], function (newValue, oldValue, scope) {
                //Add Scope For Chart

                if (scope.seasonalityParams != undefined) {
                    scope.DepartDate = $filter('date')(scope.seasonalityParams.Fareforecastdata.DepartureDate, scope.format, null);
                    scope.ReturnDate = $filter('date')(scope.seasonalityParams.Fareforecastdata.ReturnDate, scope.format, null);
                    scope.chartHeight = 300;
                    scope.TabIndex = "seasonality" + scope.seasonalityParams.tabIndex;
                    var mapHTML = "";
                    if (scope.seasonalityParams.tabIndex == 999)
                        mapHTML = "<div id='" + scope.TabIndex + "' class='seasonalityinmaindiv' ></div>";
                    else
                        mapHTML = "<div id='" + scope.TabIndex + "'></div>";
                    elem.append($compile(mapHTML)(scope));
                    if (scope.seasonalityParams.tabIndex == 999)
                        document.getElementById(scope.TabIndex).innerHTML = "";
                }
                //Add Scope For Chart
                if (scope.isOpen == true) {
                   // if (newValue != oldValue)
                        scope.loadSeasonalityInfo();
                }
                else {
                    scope.MarkerSeasonalityInfo = "";
                }
            });
            scope.$watch('SeasonalityData', function (newValue, oldValue) {
                //if (newValue != oldValue) {
                    if (scope.showChart == true) {
                        DisplayChart();
                    }
               // }
            })
            scope.Chart = [];
            function DisplayChart() {
                var chartData = [];
                var startdate;
                if (scope.SeasonalityData != undefined && scope.SeasonalityData != "") {
                    for (i = 0; i < scope.SeasonalityData.length; i++) 
                    {
                        var WeekStartDate = new Date(scope.SeasonalityData[i].WeekStartDate);
                        if (i == 0)
                        { startdate = Date.UTC(WeekStartDate.getFullYear(), WeekStartDate.getMonth(), WeekStartDate.getDate()); }

                        var utcdate = Date.UTC(WeekStartDate.getFullYear(), WeekStartDate.getMonth(), WeekStartDate.getDate());
                        var SeasonalityIndicator = 1;
                        var NumberOfObervations = 0;
                        if (scope.SeasonalityData[i].SeasonalityIndicator == "Low") {
                            SeasonalityIndicator = 1;
                        }
                        if (scope.SeasonalityData[i].SeasonalityIndicator == "Medium") {
                            SeasonalityIndicator = 2;
                        }
                        if (scope.SeasonalityData[i].SeasonalityIndicator == "High") {
                            SeasonalityIndicator = 3;
                        }
                        if (scope.SeasonalityData[i].NumberOfObervations == "GreaterThen10000")
                            NumberOfObervations = 12000;
                        if (scope.SeasonalityData[i].NumberOfObervations == "LessThan10000")
                            NumberOfObervations = 8000;
                        if(scope.SeasonalityData[i].NumberOfObervations == "LessThan1000")
                            NumberOfObervations = 00;
                        
                        var serise = {
                            x: utcdate,
                            y: SeasonalityIndicator,
                            z: NumberOfObervations
                        };
                       
                        chartData.push(serise);
                       
                    }
                    //   }


                    var options = {
                        chart: {
                            height: scope.chartHeight,
                            type: 'bubble',
                            renderTo: scope.TabIndex,
                        },
                        title: {
                            text: ''
                        },
                        xAxis: {
                            type: 'datetime',
                            labels: {
                                rotation: -45
                            },
                            //tickInterval: 14,
                            dateTimeLabelFormats: {
                                day: '%Y-%m-%e',
                                month: '%Y-%m-%e',
                                year: '%Y'
                            },
                            title: {
                                text: 'Traffic patterns for next 52 Weeks'
                            }
                        },
                        yAxis: {
                            min:0,
                            max: 4,
                            tickInterval: 1,
                            title: {
                                text: 'Traffic Category'
                            },
                            labels: {
                                formatter: function () {

                                    var result = '';
                                    if (this.value == 1) {
                                        result = '<span> ' + 'Low' + ' </span>';
                                    }
                                    else if (this.value == 2) {
                                        result = '<span> ' + 'Medium' + ' </span>';
                                    }
                                    else if( this.value == 3){
                                        result = '<span> ' + 'High' + ' </span>';
                                    }
                                    else{
                                        result = '<span> ' + '' + ' </span>';
                                    }
                                    return result;
                                }
                            }
                        },
                        legend: {
                            enabled: false
                        },
                        tooltip: {
                            headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                            pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}$</b><br/>',
                            formatter: function () {
                                return '<span style="font-size:11px">' + this.series.name + '</span><br><b>' +
                                    Highcharts.dateFormat('%Y-%m-%e', new Date(this.x)) + ',' + this.y  ;
                            }
                        },
                        series: [{
                            name: "Seasonality",
                            data: chartData,
                            pointStart: startdate,
                            colorByPoint: false,
                            pointInterval: 336 * 3600 * 1000 // one day
                        }]
                    };

                    $timeout(function () {
                        scope.Chart = new Highcharts.Chart(options);
                    }, 0, false);
                }
            }
            //scope.$watch('seasonalityParams',
            //  function (newValue, oldValue) {
            //      if (newValue != oldValue)
            //          scope.loadSeasonalityInfo();
            //  }
            //);
        }
    }
}]);
