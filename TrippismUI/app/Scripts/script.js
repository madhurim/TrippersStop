// JavaScript Document


$(document).ready(function () {
    
	    $(".search-btn").click(function () {
			$(this).toggleClass('active');	
			$(".slide-content-top").slideToggle("slow");
		});
		$(".destinations-btn").click(function(){
			$(".popular-destinations").toggleClass("active");
		});
		  $('.your-search-btn').click(function(e) {
			  $(this).toggleClass('active');	
			$('.slide-content-bottom').slideToggle("slow");
		  });
		$('.total-attractions-btn').click(function(e) {
			$(this).toggleClass('active');	
			$('.slide-content-bottom').slideToggle("slow");
		});

		$('.form-control').click(function(){
			$('.control-label').removeClass('active');
			$(this).prev().addClass('active');
	    });
		$('.form-control').click(function(){
			$('.icon-box').removeClass('active');
			$(this).next().addClass('active');
	    });

	});

//*************************
// Scrollbar
//*************************
(function ($) {


    $(window).load(function () {
        
        $("a[rel='load-content']").click(function (e) {
            e.preventDefault();
            var url = $(this).attr("href");
            $.get(url, function (data) {
                $(".content .mCSB_container").append(data); //load new content inside .mCSB_container
                //scroll-to appended content 
                $(".content").mCustomScrollbar("scrollTo", "h2:last");
            });
        });

        $(".content").delegate("a[href='top']", "click", function (e) {
            
            e.preventDefault();
            $(".content").mCustomScrollbar("scrollTo", $(this).attr("href"));
        });

        $("#content-1").mCustomScrollbar({
            axis: "x",
            advanced: {
                autoExpandHorizontalScroll: true
            }
        });
    

    });
})(jQuery);


	
	
	  //    var map;
      //function initialize() {
      //  var mapOptions = {
      //    zoom: 8,
      //    center: new google.maps.LatLng(-34.397, 150.644),
      //    mapTypeId: google.maps.MapTypeId.ROADMAP
      //  };
      //  map = new google.maps.Map(document.getElementById('map_canvas'),
      //      mapOptions);
      //}

      //google.maps.event.addDomListener(window, 'load', initialize);
