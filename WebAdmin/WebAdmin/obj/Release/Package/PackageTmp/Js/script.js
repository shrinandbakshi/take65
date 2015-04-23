$(function(){
	//	Initialize Superfish
	$('.sf-menu').superfish({
		autoArrows: true
	});
	
	//	Initialize UItoTop
	$().UItoTop({ easingType: 'easeOutQuart' });
})

$(function () {
    $("#accordion dt").next("#accordion dd").css({ display: 'none' });
    $("#accordion dt").click(function () { $(this).next("#accordion dd").slideToggle("slow").siblings("#accordion dd:visible").slideUp("slow"); $(this).toggleClass("active"); $(this).siblings("#accordion dt").removeClass("active"); return false });
    //  Initialize Contact Form
    $('#form1').forms({
        ownerEmail: '#'
    });
})
$(window).load(function () {
    $('.heading-wrapper-2 .heading-after-2').each(function () {
        var thiswidth = ($(this).parent().width() - $(this).prev().width()) / 2 - 22;
        $(this).css({ width: thiswidth })
    })
    $('.heading-wrapper-2 .heading-before-2').each(function () {
        var thiswidth = ($(this).parent().width() - $(this).next().width()) / 2 - 22;
        $(this).css({ width: thiswidth })
    })
});
$(window).load(function () {
    $('.heading-wrapper .heading-after').each(function () {
        var thiswidth = ($(this).parent().width() - $(this).prev().width()) / 2 - 28;
        $(this).css({ width: thiswidth })
    })
    $('.heading-wrapper .heading-before').each(function () {
        var thiswidth = ($(this).parent().width() - $(this).next().width()) / 2 - 28;
        $(this).css({ width: thiswidth })
    })
});