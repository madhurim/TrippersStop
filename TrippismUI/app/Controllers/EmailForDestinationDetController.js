﻿(function () {
    'use strict';
    var controllerId = 'EmailForDestinationDet';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', '$filter', '$modal',
         'EmailForDestinationDetFactory', 'SeasonalityFactory',
         'FareRangeFactory',
         'eMailData',
         'eMailDataFareForeCast',
         '$timeout',
         'TrippismConstants', EmailForDestinationDet]);

    function EmailForDestinationDet($scope, $filter, $modal,
        EmailForDestinationDetFactory, SeasonalityFactory, FareRangeFactory,
        eMailData,
        eMailDataFareForeCast,
        $timeout,
        TrippismConstants) {
        $scope.SharedbuttonText = "Share";
        $scope.Toemailaddress = "";
        $scope.subject = "";
        $scope.hasError = false;
        $scope.email = "";
        $scope.fromemail = "";
        $scope.Defaultsubject = eMailData.OriginairportName.airport_FullName;
        $scope.Subject = "Destination Locations from Origin " + $scope.Defaultsubject + " via [Trippism]";
        $scope.emailvalidate = false;
        $scope.eMailData = eMailData;

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
            var basicDetinationDetlist = $scope.eMailData.DestinationList;

            var basicDetinationDetlist = _.filter(basicDetinationDetlist, function (item) {
                return item.LowestNonStopFare.Fare !== 'N/A';
            });            
            var basicDetinationDetlist = basicDetinationDetlist.sort(function (a, b) { return (parseFloat(a.LowestNonStopFare.Fare) < parseFloat(b.LowestNonStopFare.Fare)) ? 1 : -1; }).reverse().slice(0, 20);

            var airportlist = $scope.eMailData.AvailableAirports;

            var Origin = $scope.eMailData.SearchCriteria.Origin.toUpperCase();

            var OriginairportName = _.find(airportlist, function (airport) {
                return airport.airport_Code == Origin
            });
            var destinationfareinfo = _.find($scope.eMailData.DestinationList, function (Destination) {
                return Destination.DestinationLocation == $scope.eMailData.DestinationairportName.airport_Code.toUpperCase()
            });

            var FareData = eMailDataFareForeCast;

            var sortedObjs = _.filter(basicDetinationDetlist, function (item) {
                return item.LowestFare !== 'N/A';
            });
            sortedObjs = _(sortedObjs).sortBy(function (obj) { return parseInt(obj.LowestFare, 10) });

            var FromDate = ConvertToRequiredDate($scope.eMailData.SearchCriteria.FromDate, 'API');
            var ToDate = ConvertToRequiredDate($scope.eMailData.SearchCriteria.ToDate, 'API');
            var OriginName = OriginairportName.airport_CityCode.toUpperCase();


            var url = 'http://' + window.document.location.host;
            var tripissm_rdrURL = '<a href="' + url + '/#/destination?Origin=' + OriginName + '&DepartureDate=' + FromDate + '&ReturnDate=' + ToDate + '">www.trippism.com</a>';


            var contentString = '<div style="font-family: arial,sans-serif;color: black;">' +
                           '<p>Hi,</p><p>I got following from ' + tripissm_rdrURL + '</p><p>From our orgin <strong>' + OriginairportName.airport_CityName + '</strong> during ' + ConvertToRequiredDate(sortedObjs[0].DepartureDateTime, 'UI') + ' to ' + ConvertToRequiredDate(sortedObjs[0].ReturnDateTime, 'UI') + ' , we have following options to fly.</p>' +
                          '<table class="table" style="color: #333;font-family: Helvetica, Arial, sans-serif;width:90%%; border-collapse:collapse; border-spacing: 0;"><tr><th style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #DFDFDF;">Destination</th><th style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #DFDFDF;">Lowest Fare</th><th style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #DFDFDF;">Lowest Non Stop Fare</th></tr>';

            var MarkersString = '';
            for (var x = 0; x < sortedObjs.length; x++) {
                var airportName = _.find(airportlist, function (airport) {
                    return airport.airport_Code == sortedObjs[x].DestinationLocation
                });

                var lowestnonfare = "";
                var lowestfare = "";
                if (sortedObjs[x].LowestNonStopFare != undefined && sortedObjs[x].LowestNonStopFare.Fare != "N/A") { lowestnonfare = sortedObjs[x].CurrencyCode + " " + $filter('number')(sortedObjs[x].LowestNonStopFare.Fare, '2'); } else { lowestnonfare = "N/A"; }
                if (sortedObjs[x].LowestFare != undefined && sortedObjs[x].LowestFare.Fare != "N/A") { lowestfare = sortedObjs[x].CurrencyCode + " " + $filter('number')(sortedObjs[x].LowestFare.Fare, '2'); } else { lowestfare = "N/A"; }

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
            if ((LowestFare == "N/A" || LowestFare == 0) && (destinationfareinfo.LowestFare != undefined && destinationfareinfo.LowestFare.Fare != "N/A")) {
                LowestFare = destinationfareinfo.LowestFare.Fare.toFixed(2)
                CurrencyCode = destinationfareinfo.CurrencyCode;

            }

            if ($scope.eMailData.mapOptions != undefined) {

                var LowestNonStopFare = (($scope.eMailData.mapOptions.LowestNonStopFare == undefined || $scope.eMailData.mapOptions.LowestNonStopFare.Fare == 'N/A')) ? 'N/A' : Number($scope.eMailData.mapOptions.LowestNonStopFare.Fare).toFixed(2);

                var DepartureDate = ConvertToRequiredDate($scope.eMailData.mapOptions.DepartureDateTime, 'UI');
                var ReturnDate = ConvertToRequiredDate($scope.eMailData.mapOptions.ReturnDateTime, 'UI');


                contentString += '<br/><div style="font-size : 13px;"><b style=" text-decoration: underline;">Destination Details</b></div><br/> <div style="width:100%;" >' +
                              '<div style="width:25%;float:left;" >' +
                                  '<span>Destination : </span><br><strong >'
                                    + $scope.eMailData.DestinationairportName.airport_FullName + ', '
                                    + $scope.eMailData.DestinationairportName.airport_CityName +
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

            contentString += ' <p style="clear:both;padding-top:20px;">Please explore ' + tripissm_rdrURL + ' for more details to plan vacation, trip.</p><p>Thanks,</p><p>via Trippism - new generation trip planner!</p></div>';

            var rdrURL = '<a href="' + url + '/#/destination?Origin=' + OriginName + '&DepartureDate=' + FromDate + '&ReturnDate=' + ToDate + '">';
            contentString += rdrURL + '<img src="https://maps.googleapis.com/maps/api/staticmap?zoom=2&size=800x500&maptype=roadmap&' + MarkersString + '" /></a>';

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
    }
})();


