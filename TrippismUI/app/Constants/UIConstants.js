(function () {
    'use strict';
    var constants = {
        googlePlacesApiKey: "AIzaSyC0CVNlXkejEzLzGCMVMj8PZ7gBzj8ewuQ",
        DefaultLenghtOfStay: 4,
        YouTubeEmbedUrl :"//www.youtube.com/embed/",
    };

    //to use urls, inject in controller
    angular
        .module('TrippismUIApp')
        .constant('TrippismConstants', constants);

})();
