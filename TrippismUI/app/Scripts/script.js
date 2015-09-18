// JavaScript Document


	$(document).ready(function(){
		$(".search-btn").click(function(){
			$(this).toggleClass('active');	
			$(".destination-wp").slideToggle("slow");
		});
		$(".destinations-btn").click(function(){
			$(".popular-destinations").toggleClass("active");
		});
		  $('.your-search-btn').click(function(e) {
			  $(this).toggleClass('active');	
			$('.advanced-search').slideToggle("slow");
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

	(function($){
		$(window).load(function(){
			
			$("a[rel='load-content']").click(function(e){
				e.preventDefault();
				var url=$(this).attr("href");
				$.get(url,function(data){
					$(".content .mCSB_container").append(data); //load new content inside .mCSB_container
					//scroll-to appended content 
					$(".content").mCustomScrollbar("scrollTo","h2:last");
				});
			});
			
			$(".content").delegate("a[href='top']","click",function(e){
				e.preventDefault();
				$(".content").mCustomScrollbar("scrollTo",$(this).attr("href"));
			});
			
		});
	})(jQuery);