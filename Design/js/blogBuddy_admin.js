/* js for administration pages */

$(function() {

    /* change glyphicon icons on click */
    $(".blogBuddy-glyp").click(function () {
        if ($(this).hasClass("glyphicon-minus")) {
            $(this).removeClass("glyphicon-minus").addClass("glyphicon-plus");
            $(this).parent().parent().addClass("active");
        } else {
            $(this).removeClass("glyphicon-plus").addClass("glyphicon-minus");
            $(this).parent().parent().removeClass("active");
        }
    });

    /* change font color in select category */
    $('#selectCategory').change(function() {
        $(this).removeClass("blogBuddy-gray");
    });

});