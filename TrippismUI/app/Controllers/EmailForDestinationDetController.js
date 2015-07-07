(function () {
    'use strict';
    var controllerId = 'EmailForDestinationDet';
    angular.module('TrippismUIApp').controller(controllerId,
        ['$scope', 'blockUIConfig', '$filter', '$modal', 'EmailForDestinationDetFactory', EmailForDestinationDet]);

    function EmailForDestinationDet($scope,blockUIConfig, $filter, $modal, EmailForDestinationDetFactory) {
        $scope.SharedbuttonText = "Share";
        $scope.SendEmailToUser = SendEmailToUser;   
        $scope.Toemailaddress = "";
        $scope.subject = "";
        $scope.hasError = false;
        $scope.email = "";
        $scope.fromemail = "";
        $scope.Subject = "Destination Locations from Origin " + $scope.Defaultsubject + " via [Trippism]";
        $scope.emailvalidate = false;
   
        $scope.submitModal = function () {
            if ($scope.FormGetEmailDet.$invalid) {
                $scope.hasError = true;
                return;
            }
            validateEmail();
            if ($scope.FormGetEmailDet.$valid) {
                var data = {From : $scope.fromemail, To: $scope.email, subject: $scope.Subject }
                $scope.$close(data);
            }
        }

        $scope.dismiss = function () {
            $scope.$dismiss('cancel')
        };

        function validateEmail()
        {
            if ($scope.email.length < 1) {
                $scope.hasError = true;
                return;
            }
            var emails = $scope.email.split(',');
           
            var isValid = true;
            for (var i = 0; isValid && i < emails.length; i++) {
                if(!checkEmail(emails[i]))
                {
                    $scope.emailvalidate = true;
                    $scope.hasError = true;
                    $scope.FormGetEmailDet.$invalid = true;
                }               
            }
        }

        function activate()
        {
            var basicDetinationDetlist = $scope.destinationScope.destinationlist;
            var airportlist = $scope.destinationScope.AvailableAirports;
            var OriginairportName = _.find(airportlist, function (airport) {
                return airport.airport_Code == $scope.destinationScope.Origin.toUpperCase()
            });
            
            var sortedObjs = _.filter(basicDetinationDetlist, function (item) {
                return item.LowestFare !== 'N/A';
            });
            sortedObjs = _(sortedObjs).sortBy(function (obj) { return parseInt(obj.LowestFare, 10) })

            var contentString = '<div style="font-family: arial,sans-serif;color: black;">' +
                           '<p>Hi,</p><p>I got following from <a href="www.trippism.com">www.trippism.com</a></p><p>From our orgin <strong>' + OriginairportName.airport_CityName + '</strong> during ' + $filter('date')(sortedObjs[0].DepartureDateTime, $scope.destinationScope.format, null) + ' to ' + $filter('date')(sortedObjs[0].ReturnDateTime, $scope.destinationScope.format, null) + ' , we have following options to fly.</p>' +
                          '<table class="table" style="color: #333;font-family: Helvetica, Arial, sans-serif;width:90%%; border-collapse:collapse; border-spacing: 0;"><tr><th style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #DFDFDF;">Destination</th><th style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #DFDFDF;">Lowest Fare</th><th style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #DFDFDF;">Lowest Non Stop Fare</th></tr>';

            var MarkersString = '';
            for (var x = 0; x < 10; x++) {
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

                MarkersString += "markers=color:blue|label:" + airportName.airport_Code + "|" + airportName.airport_Lat + "," + airportName.airport_Lng+"&";
            }

            contentString += '</table>';
            if ($scope.destinationinfo.mapDetails != undefined) {
                console.log($scope.destinationinfo.mapDetails);

                var LowestNonStopFare = ($scope.destinationinfo.mapDetails.LowestNonStopFare == 'N/A') ? 'N/A' : Number($scope.destinationinfo.mapDetails.LowestNonStopFare).toFixed(2);
                var LowestFare = ($scope.destinationinfo.mapDetails.LowestFare == 'N/A') ? 'N/A' : Number($scope.destinationinfo.mapDetails.LowestFare).toFixed(2);
                var DepartureDate = $filter('date')($scope.destinationinfo.mapDetails.DepartureDateTime, $scope.destinationScope.format)
                var ReturnDate = $filter('date')($scope.destinationinfo.mapDetails.ReturnDateTime, $scope.destinationScope.format)

                contentString += '<br/><div style="font-size : 13px;"><b style=" text-decoration: underline;">Destination Details</b></div><br/> <div style="width:100%;" >' +
                              '<div style="width:25%;float:left;" >' +
                                  '<span>Destination : </span><br><strong >'
                                    + $scope.destinationinfo.DestinationairportName.airport_FullName + ', '
                                    + $scope.destinationinfo.DestinationairportName.airport_CityName +
                                  '</strong>' +
                              '</div>' +
                              '<div  style="width:13%;float:left;">' +
                                  '<span>Lowest Fare: </span><br><strong >'
                                    + $scope.destinationinfo.mapDetails.CurrencyCode + ' '
                                    + LowestFare +
                                    '</strong><br>' +
                              '</div>' +
                              '<div  style="width:20%;float:left;">' +
                                  '<span>Lowest Non Stop Fare: </span><br><strong > '
                                  + $scope.destinationinfo.mapDetails.CurrencyCode + ' '
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

            if ($scope.destinationinfo.FareforecastData != undefined)
            {   
                var Recommendation = $scope.destinationinfo.FareforecastData.Recommendation;
                
                FareForeCastHTML += "<div style='clear:both;padding-top:15px;' ><b style='text-decoration: underline;'>Fareforecast Info</b></div>";

                FareForeCastHTML += '<div style="clear:both;" > </div><div style="font-size : 13px;clear:both;"><b>Recommendation:</b> ';

                if (Recommendation != undefined || Recommendation != '') {
                    if (Recommendation.toUpperCase() == 'BUY')
                        FareForeCastHTML += '<img style="margin-top:-10px;" title="Buy" src="~/Styles/images/thumbs_up.png" /> ';

                    if (Recommendation.toUpperCase() == 'WAIT')
                        FareForeCastHTML += '<img style="margin-top:-10px;" title="Buy" src="~/Styles/images/time.png" /> ';

                    if (Recommendation.toUpperCase() == 'WATCH')
                        FareForeCastHTML += '<img style="margin-top:-10px;" title="Buy" src="~/Styles/images/circle-info.png" /> ';

                    if (Recommendation.toUpperCase() == 'UNKNOWN')
                        FareForeCastHTML += '<img style="margin-top:-10px;" title="Buy" src="~/Styles/images/hor-loading.png" /> ';
                }

                var HighestPredictedFare = ($scope.destinationinfo.FareforecastData.Forecast.HighestPredictedFare == 'N/A') ? 'N/A' : Number($scope.destinationinfo.FareforecastData.Forecast.HighestPredictedFare).toFixed(2);
                var LowestPredictedFare = ($scope.destinationinfo.FareforecastData.Forecast.LowestPredictedFare == 'N/A') ? 'N/A' : Number($scope.destinationinfo.FareforecastData.Forecast.LowestPredictedFare).toFixed(2);
                
                FareForeCastHTML += '<div  style="clear:both;margin-top:10px;width:100%;">'+
                                        '<div style="padding:0px;width:30%;float:left;">'+
                                            '<span>Highest Predicted Fare: </span><br />' 
                                                +'<strong>' + $scope.destinationinfo.FareforecastData.CurrencyCode + ' ' + HighestPredictedFare
                                                +'</strong>' +
                                        '</div>'+
                                        '<div style="padding:0px;width:30%;float:left;">' +
                                            '<span>Lowest Predicted Fare: </span><br /><strong>' +
                                                $scope.destinationinfo.FareforecastData.CurrencyCode + ' ' + LowestPredictedFare +
                                        '</strong>' +
                                   '</div>'+
                                   '</div>';

                FareForeCastHTML += '</div>';
            }
            contentString += FareForeCastHTML;


            var SeasonalityHTML = "";
            
            var SeasonalityData = $scope.destinationinfo.MarkerSeasonalityInfo;
            if (SeasonalityData != "") {
                contentString += "<div style='clear:both;padding-top:15px;margin-bottom:5px;' ><b style='text-decoration: underline;'>Seasonality Info</b></div>";
                SeasonalityHTML += '<table >'+
                                        '<tr>'+
                                            '<th style="border:1px solid transparent;height:30px;background:#dfdfdf">Week#</th>' +
                                            '<th style="border:1px solid transparent;height:30px;background:#dfdfdf">Start Date</th>' +
                                            '<th style="border:1px solid transparent;height:30px;background:#dfdfdf">End Date</th>' +
                                            '<th style="border:1px solid transparent;height:30px;background:#dfdfdf">Traffic Volume</th>' +
                                            '<th style="border:1px solid transparent;height:30px;background:#dfdfdf">Booking Quantities</th>' +
                                        '</tr>';
                                    

                angular.forEach(SeasonalityData.Seasonality, function (value, key) {
                    SeasonalityHTML += '<tr>'+
                                            '<td style="text-align:center;  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;" > ' + value.YearWeekNumber + '</td>' +
                                            '<td style="  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;" >' + $filter('date')(value.WeekStartDate, $scope.destinationScope.format) + '</td>' +
                                            '<td style="  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;text-align:center;" > ' + $filter('date')(value.WeekEndDate, $scope.destinationScope.format) + ' </td>' +
                                            '<td style="text-align:center;  border: 1px solid transparent;height: 30px;background: #fafafa;word-wrap: break-word;" >';
                                            var Indicator = value.SeasonalityIndicator;
                                            if(Indicator == 'High')
                                                SeasonalityHTML += '<span style="background-color: #468847;padding: 5px 15px;color: #FFF;text-align: center;">' + Indicator + '</span>';
                                            else if(Indicator == 'Medium')
                                                SeasonalityHTML += '<span style="background-color: #b6ff00;padding: 5px;text-align: center;">' + Indicator + '</span>';
                                            else if(Indicator == 'Low')
                                                SeasonalityHTML += '<span style="background-color: #dff0d8;padding: 5px 18px;text-align: center;">' + Indicator + '</span>';

                                            SeasonalityHTML += '</td>';
                                            SeasonalityHTML += '<td style="  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;">';
                                            
                                            var Observations = value.NumberOfObservations;
                                            if( Observations == 'GreaterThan10000')
                                                SeasonalityHTML += '<span >Over crowded</span>';

                                            if( Observations == 'LessThan10000')
                                                SeasonalityHTML += '<span >crowded</span>';

                                            if( Observations == 'LessThan1000')
                                                SeasonalityHTML += '<span >not crowded</span>';
                                            
                                           SeasonalityHTML += '</td>'+
                                        '</tr>';
                });
                 
                SeasonalityHTML +='</table>'
            }
            
            contentString += SeasonalityHTML;

            var FareRangeHTML = "";
            var fareRangeData = $scope.destinationinfo.fareRangeData;
            if (fareRangeData != "") {
                contentString += "<div style='clear:both;padding-top:15px;margin-bottom:5px;' ><b style='text-decoration: underline;'>Fare Range Info</b></div>";
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
                                           '<td style="text-align:center;  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;" > ' + value.CurrencyCode + ' ' + value.MaximumFare + '</td>' +
                                           '<td style="text-align:center;  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;" > ' + value.CurrencyCode + ' ' + value.MinimumFare + '</td>' +
                                           '<td style="text-align:center;  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;" > ' + value.CurrencyCode + ' ' + value.MedianFare + '</td>' +
                                           '<td style="text-align:center;  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;" > ' + value.Count + '</td>' +
                                           '<td style="text-align:center;  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;" > ' + $filter('date')(value.DepartureDateTime, $scope.destinationScope.format) + '</td>' +
                                           '<td style="text-align:center;  border: 1px solid transparent;height: 30px;background: #fafafa;text-align: center;word-wrap: break-word;" > ' +  $filter('date')(value.ReturnDateTime, $scope.destinationScope.format) + '</td>' +
                                        '</tr>';
                });
                FareRangeHTML += '</table>'
                
            }

            contentString += FareRangeHTML;

            contentString += ' <p style="clear:both;padding-top:20px;">Please explore <a href="www.trippism.com">www.trippism.com</a> for more details to plan vacation, trip.</p><p>Thanks,</p><p>via Trippism - new generation trip planner!</p></div>';

            var FromDate = ConvertToRequiredDate($scope.destinationScope.FromDate,'API');
            var ToDate = ConvertToRequiredDate($scope.destinationScope.ToDate,'API');
            var OriginName = $scope.destinationScope.Origin.toUpperCase();
            var url = 'http://' + window.document.location.host;
            
            //var rdrURL = '<a href="http://localhost:1299/#/destination?org=' + OriginName + '&fromdate=' + FromDate + '&todate=' + ToDate + '">';
            //var rdrURL = '<a href="http://www.trippism.com/#/destination?org=' + OriginName + '&fromdate=' + FromDate + '&todate=' + ToDate + '">';
            var rdrURL = '<a href="'+ url +'/#/destination?org=' + OriginName + '&fromdate=' + FromDate + '&todate=' + ToDate + '">';
            
            contentString += rdrURL + '<img src="https://maps.googleapis.com/maps/api/staticmap?zoom=2&size=800x500&maptype=roadmap&'+ MarkersString +'" /></a>';
            //'<img src="https://maps.googleapis.com/maps/api/staticmap?zoom=13&size=800x500&maptype=roadmap&markers=color:blue%7SSlabel:S%7C40.702147,-74.015794&markers=color:green%7Clabel:G%7C40.711614,-74.012318&markers=color:red%7Clabel:A%7C40.718217,-73.998284" /></a>';
            
            
            
            var email = {
                From: $scope.FromEmail,
                To: $scope.Toemailaddress,
                subject: $scope.subject,
                body: contentString
            };
            
            blockUIConfig.message = "Please Wait! Destination Information Sharing...";
            EmailForDestinationDetFactory.SendEmail(email).then(function (data) {
                if (data.Data.status == "ok") {
                    alertify.alert("Sucess", "");
                    alertify.alert('Shared via Email sucessfully.').set('onok', function (closeEvent) { });
                }
                else {
                    alertify.alert("Error", "");
                    alertify.alert(data.Data.status).set('onok', function (closeEvent) { });
                }
                blockUIConfig.message = "Loading...";
            });
        }

        function SendEmailToUser(destinationdet) {
            
            var dest = destinationdet.$parent.$parent.$parent;
            $scope.Defaultsubject = $scope.destinationScope.OriginFullName;
            $scope.destinationinfo = dest;
            var GetEmailDetPopupInstance = $modal.open({
                templateUrl: 'EmailDetForm.html',
                scope: $scope,
            });
            GetEmailDetPopupInstance.result.then(function (result) {
                $scope.Toemailaddress = result.To;
                $scope.subject = result.subject;
                $scope.FromEmail = result.From;
                activate();
            });
        }
    }
})();


