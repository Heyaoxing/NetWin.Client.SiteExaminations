// JavaScript Document
/*Ê×Ò³*/
$(document).ready(function () {
         var random_bg=Math.floor(Math.random()*6+1);
         var bg = 'url(../Content/images/bg_0' + random_bg + '.jpg)';
         $(".index_body").css("background-image",bg);
});


/*»ù´¡*/

$(function() {
        var speed = 600;

        var bk_wh = $(".xn_right_topl").outerWidth(true);
        var _index = $(this).index();

        var bk_hightli = $('.xn_web_basics_xq').children().outerHeight(true);

        $(".xn_web_basics_span3").toggle(
            function() {

                var num = $(this).parent().parent().parent().find('.xn_web_basics_xq').children().length;
                var bk_hight = bk_hightli * num;

                $(this).parent().parent().parent().find(".xn_web_basics_xq").show().stop().animate({ height: bk_hight }, speed);

                $(this).css('background', 'url(../Content/images/xn_top.png) no-repeat center');
            },
            function() {

                $(this).parent().parent().parent().find(".xn_web_basics_xq").stop().animate({ height: "0" }, speed, function() {
                    $(this).hide();
                });
                $(this).css('background', 'url(../Content/images/xn_bottom.png) no-repeat center');
            }
        );
    }
);
 
 
 
 $(function(){	 	 
		
		$(".xn_hover dd").click(function(){ 
			$(this).parent().find(".xn_basics_ts").css("display","block");	
		});
		$(".xn_basics_close").click(function(){ 
			$(this).parent().parent().find(".xn_basics_ts").css("display","none");	
		});
 }
 )