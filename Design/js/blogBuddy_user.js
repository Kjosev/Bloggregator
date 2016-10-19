/* js for users pages */

$(function() {

    /* from login/register with social change to email */
    $("#hide-social").click(function (){
        $("#social").addClass("hidden");
        $("#email").removeClass("hidden");
    });
    $("#hide-email").click(function (){
        $("#email").addClass("hidden");
        $("#social").removeClass("hidden");
    });

    /* chanbe ribbon star on click */
    $(".ribbon").click(function (){
        var ribbon = $( $( $(this).children()[0]).children()[0]);
       if (ribbon.hasClass("glyphicon-star-empty")) {
           ribbon.removeClass("glyphicon-star-empty").addClass("glyphicon-star");
       } else {
           ribbon.removeClass("glyphicon-star").addClass("glyphicon-star-empty");
       }
    });


    /* account */
    $("#sidebar a").on("click", function(){
        if (!$(this).hasClass("active") && !$(this).hasClass("disabled")) {
            $("#deactivate , #email , #third-party").hide();
            $("#" + $(this).data("id")).show();


            $("#sidebar a").removeClass("active");
            $(this).addClass("active");
        }
    });

    /* to check or uncheck all */
    $(".toggle-settings").on("click", function(){
        var isChecked = this.checked;
        $(this).closest(".panel-default").find(".toggle-settings-sources").prop("checked", isChecked);

        /* to make active all table row */
        if (isChecked) {
            $(this).closest(".panel-default").find(".toggle-settings-sources").parent().parent().removeClass("active");
            $(this).closest(".panel-default").find(".toggle-settings-sources").parent().parent().addClass("active");
        } else {
            $(this).closest(".panel-default").find(".toggle-settings-sources").parent().parent().removeClass("active");
        }
    });

    /* on change checkbox */
    $('.toggle-settings-sources').change(function() {
        var isChecked = this.checked;

        if (isChecked) {
            $(this).parent().parent().addClass("active");
        } else {
            $(this).parent().parent().removeClass("active");
        }
    });

    /* to check title checkbox */
    $(".toggle-settings-sources").on("click", function(){
        if ($(this).closest(".panel-default").find(".toggle-settings-sources:checked").length > 0){
            $(this).closest(".panel-default").find(".toggle-settings").prop("checked", true);
        }
        else{
            $(this).closest(".panel-default").find(".toggle-settings").prop("checked", false);
        }
    });


});