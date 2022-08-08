


$(".panel-heading").click(function () {
    var anim = $(this).parent().data("hideanim");
    if (anim == "slide") {
        $(this).next().slideToggle(500);
    }
    else {
        if (anim == "fade") {
            $(this).next().fadeToggle(500);
        }
    }
});


