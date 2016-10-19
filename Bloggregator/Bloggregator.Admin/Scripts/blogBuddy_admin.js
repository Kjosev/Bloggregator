/* js for administration pages */

$(function () {

    /* change font color in select category */
    $('#selectCategory').change(function () {
        $(this).removeClass("blogBuddy-gray");
    });
});

var fillSelectCategory = function (url) {

    var selectCategoryElem = $("#selectCategory");

    $("<option disabled selected hidden class=\"blogBuddy-black\">Category</option>")
        .appendTo(selectCategoryElem);

    $.ajax({
        url: url,
        success: function (data, textStatus, jqXHR) {

            $.each(data,
                function (index, value) {
                    var element = "<option>" + value.Name + "</option>";
                    $(element)
                        .addClass("blogBuddy-black")
                        .attr("value", value.Id)
                        .appendTo(selectCategoryElem);
                });
        }
    });
}