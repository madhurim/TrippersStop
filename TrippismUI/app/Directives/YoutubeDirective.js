﻿angular.module('TrippismUIApp').directive('youtubeInfo', ['$compile', 'YouTubeFactory', '$sce', function ($compile, YouTubeFactory,$sce) {
    return {
        restrict: 'E',
        scope: {
            youtubeParams : '=',
        },
        templateUrl: '/app/Views/Partials/YoutubePartial.html',
        
        link: function (scope, elem, attrs) {
            scope.youtubeInfoDataFound = false;
            scope.$watchGroup(['youtubeParams'], function (newValue, oldValue, scope) {
                scope.youtubeData = "";
                scope.loadyoutubeInfo();
                //if (scope.isOpen == true) {
                //    if (newValue != oldValue)
                //        scope.loadyoutubeInfo();
                //}
                //else {
                //    scope.youtubeData = "";
                //}
            });

            scope.loadyoutubeInfo = function () {
                
                scope.youtubeInfoLoaded = false;
                scope.youtubeInfoDataFound = false;
                scope.youtubeData = "";
                if (scope.youtubeParams != undefined) {
                    var data = {
                        "location": scope.youtubeParams.DestinationairportName.airport_Lat + "," + scope.youtubeParams.DestinationairportName.airport_Lng
                    };
                    if (scope.youtubeInfoLoaded == false) {
                        if (scope.youtubeData == "") {
                            scope.youtubepromise = YouTubeFactory.youTube(data).then(function (data) {
                                if (data.status == 404) {
                                    scope.youtubeInfoDataFound = false;
                                    return;
                                }
                                scope.youtubeData = data;
                                scope.getVideoUrl = $sce.trustAsResourceUrl("//www.youtube.com/embed/" + data[0].VideoId);
                                scope.youtubeInfoDataFound = true;
                            });
                        }
                    }
                    scope.youtubeInfoLoaded = true;
                }
            };


            scope.setVideoUrl = function (url) {
                scope.getVideoUrl = $sce.trustAsResourceUrl("//www.youtube.com/embed/" + url);
            }

        }
    }
}]);
