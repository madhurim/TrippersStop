angular.module('TrippismUIApp').directive('googleattractionInfo', ['$compile', 'GoogleAttractionFactory', function ($compile, GoogleAttractionFactory) {
    return {
        restrict: 'E',
        scope: { googleattractionParams: '=', isOpen: '=' },
        templateUrl: '/app/Views/Partials/GoogleAttractionPartial.html',
        link: function (scope, elem, attrs) {

            scope.GoogleAttractionDisplay = function () {
                scope.quantity = 20;
            };
            scope.$watchGroup(['googleattractionParams', 'isOpen'], function (newValue, oldValue, scope) {
                if (scope.isOpen == true) {
                    if (newValue != oldValue)
                        scope.loadgoogleattractionInfo();
                }
                else {
                    scope.googleattractionData = "";
                }
            });
            // scope.$watch('googleattractionParams',
            //  function (newValue, oldValue) {
            //      if (newValue != oldValue)
            //          scope.loadgoogleattractionInfo();
            //  }
            //);



            scope.loadgoogleattractionInfo = function () {
                scope.googleattractionInfoLoaded = false;
                scope.googleattractionInfoNoDataFound = false;
                scope.googleattractionData = "";
                if (scope.googleattractionParams != undefined) {
                    var data = {
                        "Latitude": scope.googleattractionParams.airport_Lat,
                        "Longitude": scope.googleattractionParams.airport_Lng
                    };
                    if (scope.googleattractionInfoLoaded == false) {
                        if (scope.googleattractionData == "") {
                            scope.googleattractionpromise = GoogleAttractionFactory.googleAttraction(data).then(function (data) {
                                if (data.status == 404)
                                    scope.googleattractionInfoNoDataFound = true;
                                scope.googleattractionData = data;
                                scope.quantity = 5;

                            });
                        }
                    }
                    scope.googleattractionInfoLoaded = true;
                }
            };
        }
    }
}]);


angular.module('TrippismUIApp').directive("averageStarRating", function () {
    return {
        restrict: "EA",
        template: "<div class='average-rating-container'>" +
                   "  <ul class='rating background' class='readonly'>" +
                   "    <li ng-repeat='star in stars' class='star'>" +
                   "      <i class='fa fa-star'></i>" + //&#9733
                   "    </li>" +
                   "  </ul>" +
                   "  <ul class='rating foreground' class='readonly' style='width:{{filledInStarsContainerWidth}}%'>" +
                   "    <li ng-repeat='star in stars' class='star filled'>" +
                   "      <i class='fa fa-star'></i>" + //&#9733
                   "    </li>" +
                   "  </ul>" +
                   "</div>",
        scope: {
            averageRatingValue: "=ngModel",
            max: "=?", //optional: default is 5
        },
        link: function (scope, elem, attrs) {
            if (scope.max == undefined) { scope.max = 5; }
            function updateStars() {
                scope.stars = [];
                for (var i = 0; i < scope.max; i++) {
                    scope.stars.push({});
                }
                var starContainerMaxWidth = 100; //%
                scope.filledInStarsContainerWidth = scope.averageRatingValue / scope.max * starContainerMaxWidth;
            };
            scope.$watch("averageRatingValue", function (oldVal, newVal) {
                if (newVal) { updateStars(); }
            });
        }
    };
});