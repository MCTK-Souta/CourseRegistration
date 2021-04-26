$(document).ready(function () {
    var education = $("#education").val();
    if (education == "3" || education == "4") {
        $("#schoolshow").css('display', 'block')
            ;}
    else {
        $("#schoolshow").css('display', 'none');
    }
})