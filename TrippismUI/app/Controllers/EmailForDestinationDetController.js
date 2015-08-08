(function () {
    'use strict';
    var controllerId = 'EmailForDestinationDet';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', '$filter', '$modal', 'EmailForDestinationDetFactory', 'SeasonalityFactory', 'FareRangeFactory', 'seasonalityData', '$timeout', EmailForDestinationDet]);

    function EmailForDestinationDet($scope, $filter, $modal, EmailForDestinationDetFactory, SeasonalityFactory, FareRangeFactory, seasonalityData, $timeout) {

        $scope.SharedbuttonText = "Share";
        $scope.SendEmailToUser = SendEmailToUser;
        $scope.Toemailaddress = "";
        $scope.subject = "";
        $scope.hasError = false;
        $scope.email = "";
        $scope.fromemail = "";
        $scope.Defaultsubject = seasonalityData.OriginairportName.airport_FullName;
        $scope.Subject = "Destination Locations from Origin " + $scope.Defaultsubject + " via [Trippism]";
        $scope.emailvalidate = false;
        $scope.seasonalityData = seasonalityData;



        $scope.submitModal = function () {
            if ($scope.FormGetEmailDet.$invalid) {
                $scope.hasError = true;
                return;
            }
            validateEmail();
            if ($scope.FormGetEmailDet.$valid) {
                var data = { From: $scope.fromemail, To: $scope.email, subject: $scope.Subject }
                $scope.Toemailaddress = $scope.email;
                $scope.subject = $scope.Subject;
                $scope.FromEmail = $scope.fromemail;
                activate();
            }
        }
        function loadSeasonalityInfo(seasonalityData) {

            var Seasonalitydata = {
                "Destination": seasonalityData.Destinatrion
            };
            //$timeout(function () {
            SeasonalityFactory.Seasonality(Seasonalitydata).then(function (data) {
                $scope.MarkerSeasonalityInfo = data;
            });
            //}, 0, false);

        };
        loadSeasonalityInfo($scope.seasonalityData);
        function loadfareRangeInfo(seasonalityData) {
            if (seasonalityData != undefined) {
                var data = {
                    "Origin": seasonalityData.Fareforecastdata.Origin,
                    "Destination": seasonalityData.Fareforecastdata.Destination,
                    "EarliestDepartureDate": seasonalityData.Fareforecastdata.DepartureDate,
                    "LatestDepartureDate": seasonalityData.Fareforecastdata.ReturnDate,
                    "Lengthofstay": 4
                };

                $scope.farerangepromise = FareRangeFactory.fareRange(data).then(function (data) {
                    if (data.status == 404)
                        $scope.fareRangeInfoNoDataFound = true;
                    $scope.fareRangeData = data;
                });
            }
        };
        loadfareRangeInfo($scope.seasonalityData);
        $scope.dismiss = function () {
            $scope.$dismiss('cancel')
        };
        function validateEmail() {
            if ($scope.email.length < 1) {
                $scope.hasError = true;
                return;
            }
            var emails = $scope.email.split(',');

            var isValid = true;
            for (var i = 0; isValid && i < emails.length; i++) {
                if (!checkEmail(emails[i])) {
                    $scope.emailvalidate = true;
                    $scope.hasError = true;
                    $scope.FormGetEmailDet.$invalid = true;
                }
            }
        }

        function activate() {
            $scope.formats = Dateformat();
            $scope.format = $scope.formats[5];
            var basicDetinationDetlist = $scope.seasonalityData.DestinationList;
            var airportlist = $scope.seasonalityData.AvailableAirports;
            var OriginairportName = _.find(airportlist, function (airport) {
                return airport.airport_Code == $scope.seasonalityData.OriginairportName.airport_Code.toUpperCase()
            });
            var FareData = $scope.$parent.FareforecastData;

            var sortedObjs = _.filter(basicDetinationDetlist, function (item) {
                return item.LowestFare !== 'N/A';
            });
            sortedObjs = _(sortedObjs).sortBy(function (obj) { return parseInt(obj.LowestFare, 10) })

            var contentString = '<div style="font-family: arial,sans-serif;color: black;">' +
                           '<p>Hi,</p><p>I got following from <a href="www.trippism.com">www.trippism.com</a></p><p>From our orgin <strong>' + OriginairportName.airport_CityName + '</strong> during ' + $filter('date')(sortedObjs[0].DepartureDateTime, $scope.format, null) + ' to ' + $filter('date')(sortedObjs[0].ReturnDateTime, $scope.format, null) + ' , we have following options to fly.</p>' +
                          '<table class="table" style="color: #333;font-family: Helvetica, Arial, sans-serif;width:90%%; border-collapse:collapse; border-spacing: 0;"><tr><th style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #DFDFDF;">Destination</th><th style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #DFDFDF;">Lowest Fare</th><th style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #DFDFDF;">Lowest Non Stop Fare</th></tr>';

            var MarkersString = '';
            for (var x = 0; x < sortedObjs.length; x++) {
                var airportName = _.find(airportlist, function (airport) {
                    return airport.airport_Code == sortedObjs[x].DestinationLocation
                });

                var lowestnonfare = "";
                var lowestfare = "";
                if (sortedObjs[x].LowestNonStopFare != "N/A") { lowestnonfare = sortedObjs[x].CurrencyCode + " " + $filter('number')(sortedObjs[x].LowestNonStopFare, '2'); } else { lowestnonfare = "N/A"; }
                if (sortedObjs[x].LowestFare != "N/A") { lowestfare = sortedObjs[x].CurrencyCode + " " + $filter('number')(sortedObjs[x].LowestFare, '2'); } else { lowestfare = "N/A"; }

                contentString += '<tr><td style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #FAFAFA;text-align: left;word-wrap: break-word;">' + airportName.airport_CityName + '</td>' +
                                    '<td style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #FAFAFA;text-align: right;word-wrap: break-word;">' + lowestfare + '</td>' +
                                    '<td style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #FAFAFA;text-align: right;word-wrap: break-word;">' + lowestnonfare + '</td></tr>';

                MarkersString += "markers=color:blue|label:" + airportName.airport_Code + "|" + airportName.airport_Lat + "," + airportName.airport_Lng + "&";
            }

            contentString += '</table>';

            var LowestFare = 'N/A';
            var CurrencyCode = "N/A";

            if (FareData != undefined && FareData != "") {
                if (FareData.LowestFare != undefined)
                    LowestFare = FareData.LowestFare;
                if (FareData.CurrencyCode != undefined)
                    CurrencyCode = FareData.CurrencyCode;
            }

            if ($scope.seasonalityData.mapOptions != undefined) {

                var LowestNonStopFare = ($scope.seasonalityData.mapOptions.LowestNonStopFare == 'N/A') ? 'N/A' : Number($scope.seasonalityData.mapOptions.LowestNonStopFare).toFixed(2);

                var DepartureDate = $filter('date')($scope.seasonalityData.mapOptions.DepartureDateTime, $scope.format)
                var ReturnDate = $filter('date')($scope.seasonalityData.mapOptions.ReturnDateTime, $scope.format)

                contentString += '<br/><div style="font-size : 13px;"><b style=" text-decoration: underline;">Destination Details</b></div><br/> <div style="width:100%;" >' +
                              '<div style="width:25%;float:left;" >' +
                                  '<span>Destination : </span><br><strong >'
                                    + $scope.seasonalityData.DestinationairportName.airport_FullName + ', '
                                    + $scope.seasonalityData.DestinationairportName.airport_CityName +
                                  '</strong>' +
                              '</div>' +
                              '<div  style="width:13%;float:left;">' +
                                  '<span>Lowest Fare: </span><br><strong >'
                                    + CurrencyCode + ' '
                                    + LowestFare +
                                    '</strong><br>' +
                              '</div>' +
                              '<div  style="width:20%;float:left;">' +
                                  '<span>Lowest Non Stop Fare: </span><br><strong > '
                                  + CurrencyCode + ' '
                                  + LowestNonStopFare + '</strong>' +
                              '</div>' +
                              '<div  style="width:15%;float:left;">' +
                              '<span>Departure Date: </span><br><strong >' + DepartureDate + '</strong>' +
                              '</div>' +
                              '<div style="width:10%;padding:0px;float:left;">' +
                                  '<span>Return Date: </span><br><strong >' + ReturnDate + '</strong>' +
                              '</div>' +
                              '</div>';
            }

            var FareForeCastHTML = "";


            debugger;
            //if ($scope.seasonalityData.FareforecastData != undefined) {
            if (FareData != undefined && FareData != "") {
                var Recommendation = FareData.Recommendation;

                FareForeCastHTML += "<div style='clear:both;padding-top:15px;' ><b style='text-decoration: underline;'>Fareforecast Info</b></div>";

                FareForeCastHTML += '<div style="clear:both;" > </div><div style="font-size : 13px;clear:both;"><b>Recommendation:</b> ';

                if (Recommendation != undefined && Recommendation != '') {
                    if (Recommendation.toUpperCase() == 'BUY')
                        FareForeCastHTML += '<img style="margin-top:-10px;" title="Buy" src="~/Styles/images/thumbs_up.png" /> ';

                    if (Recommendation.toUpperCase() == 'WAIT')
                        FareForeCastHTML += '<img style="margin-top:-10px;" title="Buy" src="~/Styles/images/time.png" /> ';

                    if (Recommendation.toUpperCase() == 'WATCH')
                        FareForeCastHTML += '<img style="margin-top:-10px;" title="Buy" src="~/Styles/images/circle-info.png" /> ';

                    if (Recommendation.toUpperCase() == 'UNKNOWN')
                        FareForeCastHTML += '<img style="margin-top:-10px;" title="Buy" src="~/Styles/images/hor-loading.png" /> ';
                }
                var HighestPredictedFare;
                var LowestPredictedFare;
                if (FareData.Forecast != undefined) {
                    HighestPredictedFare = (FareData.Forecast.HighestPredictedFare == 'N/A') ? 'N/A' : Number(FareData.Forecast.HighestPredictedFare).toFixed(2);
                    LowestPredictedFare = (FareData.Forecast.LowestPredictedFare == 'N/A') ? 'N/A' : Number(FareData.Forecast.LowestPredictedFare).toFixed(2);

                    FareForeCastHTML += '<div  style="clear:both;margin-top:10px;width:100%;">' +
                                            '<div style="padding:0px;width:30%;float:left;">' +
                                                '<span>Highest Predicted Fare: </span><br />'
                                                    + '<strong>' + CurrencyCode + ' ' + HighestPredictedFare
                                                    + '</strong>' +
                                            '</div>' +
                                            '<div style="padding:0px;width:30%;float:left;">' +
                                                '<span>Lowest Predicted Fare: </span><br /><strong>' +
                                                    FareData.CurrencyCode + ' ' + LowestPredictedFare +
                                            '</strong>' +
                                       '</div>' +
                                       '</div>';

                    FareForeCastHTML += '</div>';
                }
            }
            contentString += FareForeCastHTML;


            var SeasonalityHTML = "";
            //var SeasonalityData = $scope.MarkerSeasonalityInfo;
            var SeasonalityData = $scope.$parent.MailMarkerSeasonalityInfo;


            if (SeasonalityData != undefined && SeasonalityData != "" && SeasonalityData.Seasonality.length > 0) {
                var SeasonText = 'Traffic volume booked to the requested destination airport for each of the previous 52 weeks. It’s the booked traffic for each week to each of the other previous 51 weeks, and rated accordingly.';
                contentString += "<div style='clear:both;padding-top:15px;margin-bottom:5px;' ><b style='text-decoration: underline;'>Traffic patterns</b><br/>" + SeasonText + "</div>";
                SeasonalityHTML += '<table >' +
                                        '<tr>' +
                                            '<th style="border:1px solid transparent;height:30px;background:#dfdfdf">Week#</th>' +
                                            '<th style="border:1px solid transparent;height:30px;background:#dfdfdf">Start Date</th>' +
                                            '<th style="border:1px solid transparent;height:30px;background:#dfdfdf">End Date</th>' +
                                            '<th style="border:1px solid transparent;height:30px;background:#dfdfdf">Traffic Volume</th>' +
                                            '<th style="border:1px solid transparent;height:30px;background:#dfdfdf">Booking Quantities</th>' +
                                        '</tr>';


                angular.forEach(SeasonalityData.Seasonality, function (value, key) {
                    SeasonalityHTML += '<tr>' +
                                            '<td style="text-align:center;  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;" > ' + value.YearWeekNumber + '</td>' +
                                            '<td style="  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;" >' + $filter('date')(value.WeekStartDate, $scope.format) + '</td>' +
                                            '<td style="  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;text-align:center;" > ' + $filter('date')(value.WeekEndDate, $scope.format) + ' </td>' +
                                            '<td style="text-align:center;  border: 1px solid transparent;height: 30px;background: #fafafa;word-wrap: break-word;" >';
                    var Indicator = value.SeasonalityIndicator;
                    if (Indicator == 'High')
                        SeasonalityHTML += '<span style="background-color: #468847;padding: 5px 15px;color: #FFF;text-align: center;">' + Indicator + '</span>';
                    else if (Indicator == 'Medium')
                        SeasonalityHTML += '<span style="background-color: #b6ff00;padding: 5px;text-align: center;">' + Indicator + '</span>';
                    else if (Indicator == 'Low')
                        SeasonalityHTML += '<span style="background-color: #dff0d8;padding: 5px 18px;text-align: center;">' + Indicator + '</span>';

                    SeasonalityHTML += '</td>';
                    SeasonalityHTML += '<td style="  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;">';

                    var Observations = value.NumberOfObservations;
                    if (Observations == 'GreaterThan10000')
                        SeasonalityHTML += '<span >Over crowded</span>';

                    if (Observations == 'LessThan10000')
                        SeasonalityHTML += '<span >crowded</span>';

                    if (Observations == 'LessThan1000')
                        SeasonalityHTML += '<span >not crowded</span>';

                    SeasonalityHTML += '</td>' +
                 '</tr>';
                });

                SeasonalityHTML += '</table>'
            }

            contentString += SeasonalityHTML;
            var FareRangeHTML = "";
            //var fareRangeData = $scope.fareRangeData;
            var fareRangeData = $scope.$parent.MailFareRangeData;

            if (fareRangeData != "") {
                contentString += "<div style='clear:both;padding-top:15px;margin-bottom:5px;' ><b style='text-decoration: underline;'>Fare Range</b><br />Median, highest, and lowest published fares during the previous 4 weeks for each of the future departure dates in a range, using the specific origin, destination, and length of stay requested.</div>";
                FareRangeHTML += '<table >' +
                                       '<tr>' +
                                           '<th style="border:1px solid transparent;height:30px;background:#dfdfdf">Maximum Fare</th>' +
                                           '<th style="border:1px solid transparent;height:30px;background:#dfdfdf">Minimum Fare</th>' +
                                           '<th style="border:1px solid transparent;height:30px;background:#dfdfdf">Median Fare</th>' +
                                           '<th style="border:1px solid transparent;height:30px;background:#dfdfdf">Count</th>' +
                                           '<th style="border:1px solid transparent;height:30px;background:#dfdfdf">Departure Date</th>' +
                                           '<th style="border:1px solid transparent;height:30px;background:#dfdfdf">Return Date</th>' +
                                       '</tr>';
                angular.forEach(fareRangeData.FareData, function (value, key) {
                    FareRangeHTML += '<tr>' +
                                           '<td style="text-align:center;  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;" > ' + value.CurrencyCode + ' ' + Number(value.MaximumFare).toFixed(2) + '</td>' +
                                           '<td style="text-align:center;  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;" > ' + value.CurrencyCode + ' ' + Number(value.MinimumFare).toFixed(2) + '</td>' +
                                           '<td style="text-align:center;  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;" > ' + value.CurrencyCode + ' ' + Number(value.MedianFare).toFixed(2) + '</td>' +
                                           '<td style="text-align:center;  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;" > ' + value.Count + '</td>' +
                                           '<td style="text-align:center;  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;" > ' + $filter('date')(value.DepartureDateTime, $scope.format) + '</td>' +
                                           '<td style="text-align:center;  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;" > ' + $filter('date')(value.ReturnDateTime, $scope.format) + '</td>' +
                                        '</tr>';
                });
                FareRangeHTML += '</table>'

            }

            contentString += FareRangeHTML;

            contentString += ' <p style="clear:both;padding-top:20px;">Please explore <a href="www.trippism.com">www.trippism.com</a> for more details to plan vacation, trip.</p><p>Thanks,</p><p>via Trippism - new generation trip planner!</p></div>';

            var FromDate = ConvertToRequiredDate($scope.seasonalityData.Fareforecastdata.DepartureDate, 'API');
            var ToDate = ConvertToRequiredDate($scope.seasonalityData.Fareforecastdata.ReturnDate, 'API');
            var OriginName = $scope.seasonalityData.OriginairportName.airport_CityCode.toUpperCase();
            var url = 'http://' + window.document.location.host;

            //var rdrURL = '<a href="http://localhost:1299/#/destination?org=' + OriginName + '&fromdate=' + FromDate + '&todate=' + ToDate + '">';
            //var rdrURL = '<a href="http://www.trippism.com/#/destination?org=' + OriginName + '&fromdate=' + FromDate + '&todate=' + ToDate + '">';
            var rdrURL = '<a href="' + url + '/#/destination?Origin=' + OriginName + '&DepartureDate=' + FromDate + '&ReturnDate=' + ToDate + '">';

            contentString += rdrURL + '<img src="https://maps.googleapis.com/maps/api/staticmap?zoom=2&size=800x500&maptype=roadmap&' + MarkersString + '" /></a>';
            //'<img src="https://maps.googleapis.com/maps/api/staticmap?zoom=13&size=800x500&maptype=roadmap&markers=color:blue%7SSlabel:S%7C40.702147,-74.015794&markers=color:green%7Clabel:G%7C40.711614,-74.012318&markers=color:red%7Clabel:A%7C40.718217,-73.998284" /></a>';



            var email = {
                From: $scope.FromEmail,
                To: $scope.Toemailaddress,
                subject: $scope.subject,
                body: contentString
            };


            $scope.sendmailPromise = EmailForDestinationDetFactory.SendEmail(email).then(function (data) {
                if (data.Data.status == "ok") {
                    $scope.dismiss();
                    alertify.alert("Sucess", "");
                    alertify.alert('Shared via Email sucessfully.').set('onok', function (closeEvent) { });
                }
                else {
                    alertify.alert("Error", "");
                    alertify.alert(data.Data.status).set('onok', function (closeEvent) { });
                }

            });
        }

        function SendEmailToUser(destinationdet) {
            //var dest = destinationdet.$parent.$parent.$parent;

            var dest = destinationdet;
            $scope.Defaultsubject = $scope.seasonalityData.OriginairportName.airport_FullName;
            $scope.destinationinfo = dest;

            var GetEmailDetPopupInstance = $modal.open({
                templateUrl: 'EmailDetForm.html',
                scope: $scope,
            });
            //GetEmailDetPopupInstance.result.then(function (result) {
            //    $scope.Toemailaddress = result.To;
            //    $scope.subject = result.subject;
            //    $scope.FromEmail = result.From;
            //    activate();
            //});
        }
    }
})();


