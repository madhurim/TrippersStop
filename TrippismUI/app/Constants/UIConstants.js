(function () {
    'use strict';
    var constants = {
        googlePlacesApiKey: "AIzaSyC0CVNlXkejEzLzGCMVMj8PZ7gBzj8ewuQ",
        DefaultLenghtOfStay: 4,
        YouTubeEmbedUrl: "//www.youtube.com/embed/",
        HighChartDateFormat: '%m-%e-%Y',
        HighChartTwoDecimalCurrencyFormat: '{point.y:.2f}',


        attractionTabMapStyle: [
            { "featureType": "all", "elementType": "geometry", "stylers": [{ "visibility": "on" }] },
            { "featureType": "administrative", "elementType": "labels.text.fill", "stylers": [{ "color": "#444444" }] },
            { "featureType": "landscape", "elementType": "all", "stylers": [{ "color": "#f2f2f2" }, { "visibility": "on" }] },
            { "featureType": "landscape", "elementType": "geometry.fill", "stylers": [{ "visibility": "on" }, { "color": "#fbd09d" }] },
            { "featureType": "landscape.natural.landcover", "elementType": "geometry", "stylers": [{ "visibility": "on" }] },
            { "featureType": "landscape.natural.landcover", "elementType": "geometry.fill", "stylers": [{ "visibility": "on" }] },
            { "featureType": "landscape.natural.landcover", "elementType": "geometry.stroke", "stylers": [{ "visibility": "on" }] },
            { "featureType": "landscape.natural.terrain", "elementType": "geometry.fill", "stylers": [{ "color": "#ff0000" }, { "visibility": "off" }] },
            { "featureType": "poi", "elementType": "all", "stylers": [{ "visibility": "on" }] },
            { "featureType": "road", "elementType": "all", "stylers": [{ "saturation": -100 }, { "lightness": 45 }] },
            { "featureType": "road", "elementType": "labels.text.fill", "stylers": [{ "lightness": "-75" }] },
            { "featureType": "road", "elementType": "labels.text.stroke", "stylers": [{ "visibility": "on" }] },
            { "featureType": "road.highway", "elementType": "all", "stylers": [{ "visibility": "simplified" }] },
            { "featureType": "road.highway", "elementType": "geometry.fill", "stylers": [{ "visibility": "on" }, { "color": "#f7931e" }, { "weight": "2" }] },
            { "featureType": "road.highway", "elementType": "geometry.stroke", "stylers": [{ "visibility": "on" }] },
            { "featureType": "road.highway", "elementType": "labels.text.fill", "stylers": [{ "visibility": "on" }] },
            { "featureType": "road.highway", "elementType": "labels.text.stroke", "stylers": [{ "visibility": "off" }, { "color": "#f7931e" }, { "weight": "2.29" }] },
            { "featureType": "road.arterial", "elementType": "all", "stylers": [{ "visibility": "on" }] },
            { "featureType": "road.arterial", "elementType": "geometry.fill", "stylers": [{ "visibility": "on" }, { "lightness": "-3" }] },
            { "featureType": "road.arterial", "elementType": "geometry.stroke", "stylers": [{ "visibility": "on" }] },
            { "featureType": "road.arterial", "elementType": "labels.icon", "stylers": [{ "visibility": "on" }] },
            { "featureType": "road.local", "elementType": "geometry.stroke", "stylers": [{ "visibility": "on" }] },
            { "featureType": "transit", "elementType": "all", "stylers": [{ "visibility": "on" }] },
            { "featureType": "water", "elementType": "all", "stylers": [{ "color": "#81cfce" }, { "visibility": "on" }] },
            { "featureType": "water", "elementType": "geometry", "stylers": [{ "visibility": "on" }, { "color": "#ff0000" }] },
            { "featureType": "water", "elementType": "geometry.fill", "stylers": [{ "color": "#4cc2c1" }, { "lightness": "13" }] },
            { "featureType": "water", "elementType": "labels.text.fill", "stylers": [{ "color": "#020000" }] },
            { "featureType": "water", "elementType": "labels.text.stroke", "stylers": [{ "visibility": "off" }] }],
        destinationSearchMapSyle: [
            { "featureType": "all", "elementType": "geometry", "stylers": [{ "visibility": "off" }] },
            { "featureType": "administrative", "elementType": "labels.text.fill", "stylers": [{ "color": "#444444" }] },
            { "featureType": "landscape", "elementType": "all", "stylers": [{ "color": "#f2f2f2" }, { "visibility": "on" }] },
            { "featureType": "landscape", "elementType": "geometry.fill", "stylers": [{ "visibility": "on" }, { "color": "#fbd09d" }] },
            { "featureType": "landscape.natural.landcover", "elementType": "geometry", "stylers": [{ "visibility": "on" }] },
            { "featureType": "landscape.natural.landcover", "elementType": "geometry.fill", "stylers": [{ "visibility": "on" }] },
            { "featureType": "landscape.natural.landcover", "elementType": "geometry.stroke", "stylers": [{ "visibility": "on" }] },
            { "featureType": "landscape.natural.terrain", "elementType": "geometry.fill", "stylers": [{ "color": "#ff0000" }, { "visibility": "off" }] },
            { "featureType": "poi", "elementType": "all", "stylers": [{ "visibility": "off" }] },
            { "featureType": "road", "elementType": "all", "stylers": [{ "saturation": -100 }, { "lightness": 45 }] },
            { "featureType": "road", "elementType": "labels.text.fill", "stylers": [{ "lightness": "-75" }] },
            { "featureType": "road", "elementType": "labels.text.stroke", "stylers": [{ "visibility": "off" }] },
            { "featureType": "road.highway", "elementType": "all", "stylers": [{ "visibility": "simplified" }] },
            { "featureType": "road.highway", "elementType": "geometry.fill", "stylers": [{ "visibility": "on" }, { "color": "#f7931e" }, { "weight": "2" }] },
            { "featureType": "road.highway", "elementType": "geometry.stroke", "stylers": [{ "visibility": "off" }] },
            { "featureType": "road.highway", "elementType": "labels.text.fill", "stylers": [{ "visibility": "on" }] },
            { "featureType": "road.highway", "elementType": "labels.text.stroke", "stylers": [{ "visibility": "off" }, { "color": "#f7931e" }, { "weight": "2.29" }] },
            { "featureType": "road.arterial", "elementType": "all", "stylers": [{ "visibility": "on" }] },
            { "featureType": "road.arterial", "elementType": "geometry.fill", "stylers": [{ "visibility": "on" }, { "lightness": "-3" }] },
            { "featureType": "road.arterial", "elementType": "geometry.stroke", "stylers": [{ "visibility": "off" }] },
            { "featureType": "road.arterial", "elementType": "labels.icon", "stylers": [{ "visibility": "off" }] },
            { "featureType": "road.local", "elementType": "geometry.stroke", "stylers": [{ "visibility": "off" }] },
            { "featureType": "transit", "elementType": "all", "stylers": [{ "visibility": "off" }] },
            { "featureType": "water", "elementType": "all", "stylers": [{ "color": "#81cfce" }, { "visibility": "on" }] },
            { "featureType": "water", "elementType": "geometry", "stylers": [{ "visibility": "on" }, { "color": "#ff0000" }] },
            { "featureType": "water", "elementType": "geometry.fill", "stylers": [{ "color": "#4cc2c1" }, { "lightness": "13" }] },
            { "featureType": "water", "elementType": "labels.text.fill", "stylers": [{ "color": "#020000" }] },
            { "featureType": "water", "elementType": "labels.text.stroke", "stylers": [{ "visibility": "off" }] }]
    };

    //to use urls, inject in controller
    angular
        .module('TrippismUIApp')
        .constant('TrippismConstants', constants);

})();
