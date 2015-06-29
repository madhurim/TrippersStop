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
        $scope.email = "anand@trivenitechnologies.in";
        $scope.Subject = "Destination Locations from Origin " + $scope.Defaultsubject + " via [Trippism]";
        $scope.emailvalidate = false;
   
        $scope.submitModal = function () {           
             validateEmail();
            if ($scope.FormGetEmailDet.$invalid) {
                $scope.hasError = true;
                return;
            }

            if ($scope.FormGetEmailDet.$valid) {
                var data = { To: $scope.email, subject: $scope.Subject }
                $scope.$close(data);
            }
        }

        $scope.dismiss = function () {
            $scope.$dismiss('cancel')
        };

        function validateEmail()
        {           
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
            //var airportlist = $scope.$parent.AvailableAirports;
            var airportlist = $scope.destinationScope.AvailableAirports;
            
            var OriginairportName = _.find(airportlist, function (airport) {
                return airport.airport_Code == $scope.destinationScope.Origin
            });
            
            var sortedObjs = _.filter(basicDetinationDetlist, function (item) {
                return item.LowestFare !== 'N/A';
            });
            sortedObjs = _(sortedObjs).sortBy(function (obj) { return parseInt(obj.LowestFare, 10) })

            var contentString = '<div style="font-family: arial,sans-serif;color: black;">' +
                           '<p>Hi,</p><p>I got following from <a href="www.trippism.com">www.trippism.com</a></p><p>From our orgin <strong>' + OriginairportName.airport_CityName + '</strong> during ' + $filter('date')(sortedObjs[0].DepartureDateTime, $scope.destinationScope.format, null) + ' to ' + $filter('date')(sortedObjs[0].ReturnDateTime, $scope.destinationScope.format, null) + ' , we have following option to fly.</p>' +
                          '<table class="table" style="color: #333;font-family: Helvetica, Arial, sans-serif;width:90%%; border-collapse:collapse; border-spacing: 0;"><tr><th style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #DFDFDF;">Destination</th><th style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #DFDFDF;">Lowest Fare (' + sortedObjs[0].CurrencyCode + ')</th><th style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #DFDFDF;">Lowest Non Stop Fare (' + sortedObjs[0].CurrencyCode + ')</th></tr>';

            var MarkersString = '';
            for (var x = 0; x < 10; x++) {
                var airportName = _.find(airportlist, function (airport) {
                    return airport.airport_Code == sortedObjs[x].DestinationLocation
                });
                
                var lowestnonfare = "";
                var lowestfare = "";
                if (sortedObjs[x].LowestNonStopFare != "N/A") { lowestnonfare = $filter('number')(sortedObjs[x].LowestNonStopFare, '2'); } else { lowestnonfare = "N/A"; }
                if (sortedObjs[x].LowestFare != "N/A") { lowestfare = $filter('number')(sortedObjs[x].LowestFare, '2'); } else { lowestfare = "N/A"; }

                contentString += '<tr><td style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #FAFAFA;text-align: center;word-wrap: break-word;">' + airportName.airport_CityName + '</td>' +                                   
                                    '<td style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #FAFAFA;text-align: right;word-wrap: break-word;">' + lowestfare + '</td>' +
                                    '<td style="border: 1px solid transparent;height: 30px;transition: all 0.3s;background: #FAFAFA;text-align: right;word-wrap: break-word;">' + lowestnonfare + '</td></tr>';

                        

                
                //MarkersString += "markers=color:blue|label:" + airportName.airport_Code + "|" + airportName.airport_Lat + "," + airportName.airport_Lng;
            }

            contentString += '</table><p>Please explore <a href="www.trippism.com">www.trippism.com</a> for more details to plan vacation, trip.</p><p>Thanks,</p><p>via Trippism - new generation trip planner!</p></div>';

            //console.log("https://maps.googleapis.com/maps/api/staticmap?zoom=1&size=800x500&maptype=roadmap&"+ MarkersString );

            //contentString += '<a href="http://www.trippism.com">' +
            //                '<img src="https://maps.googleapis.com/maps/api/staticmap?zoom=3&size=800x500&maptype=roadmap&"'+ MarkersString +'" /></a>';
                                //'<img src="https://maps.googleapis.com/maps/api/staticmap?zoom=13&size=800x500&maptype=roadmap&markers=color:blue%7SSlabel:S%7C40.702147,-74.015794&markers=color:green%7Clabel:G%7C40.711614,-74.012318&markers=color:red%7Clabel:A%7C40.718217,-73.998284" /></a>';
            debugger;
            var emaildet = {
                From: "test@gmail.com",
                To: $scope.Toemailaddress,
                subject: $scope.subject,
                body: contentString,
                //mapImage: '';
            };

            blockUIConfig.message = "Please Wait! Destination Information Sharing...";
            EmailForDestinationDetFactory.SendEmail(emaildet).then(function (data) {
                if (data.Data.status == "ok") {
                    alertify.alert("Sucess", "");
                    alertify.alert('Destination Information send sucessfully.').set('onok', function (closeEvent) { });
                }
                else {
                    alertify.alert("Error", "");
                    alertify.alert(data.Data.status).set('onok', function (closeEvent) { });
                }
                blockUIConfig.message = "Loading...";
            });
        }

        function SendEmailToUser() {
            
            $scope.Defaultsubject = $scope.destinationScope.OriginFullName;
            var GetEmailDetPopupInstance = $modal.open({
                templateUrl: 'EmailDetForm.html',
                scope: $scope
            });
            GetEmailDetPopupInstance.result.then(function (result) {             
                $scope.Toemailaddress = result.To;
                $scope.subject = result.subject;
                activate();
            });
        }
    }
})();


