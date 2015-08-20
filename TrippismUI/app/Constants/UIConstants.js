(function () {
    'use strict';
    var constants = {
        googlePlacesApiKey: "AIzaSyC0CVNlXkejEzLzGCMVMj8PZ7gBzj8ewuQ",
        DefaultLenghtOfStay: 4,
        YouTubeEmbedUrl: "//www.youtube.com/embed/",
        HighChartDateFormat: '%m-%e-%Y',
        HighChartTwoDecimalCurrencyFormat: '{point.y:.2f}',
    };

    //to use urls, inject in controller
    angular
        .module('TrippismUIApp')
        .constant('TrippismConstants', constants);

})();
