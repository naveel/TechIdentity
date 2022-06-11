$(document).ready(function () {
    //iCheck for checkbox and radio inputs
    $('input[type="checkbox"].minimal').iCheck({
        checkboxClass: 'icheckbox_minimal-blue'
    });
});

// Validate Form
function ValidateForm(sender) {
    if ($("form").valid()) {
        if (!ValidateText($("#role_desc"))) {
            return;
        }

        $("#sender").val(sender);
        $("form").submit();
    }
}

function InitMaxLength(object) {
    $(object).maxlength({
        alwaysShow: true,
        threshold: 10,
        warningClass: "label label-info",
        limitReachedClass: "label label-warning",
        placement: 'bottom',
        preText: 'used ',
        separator: ' of ',
        postText: ' chars.'
    });
}

$("#selectAll,#createAll,#updateAll,#deleteAll").on("ifChecked", function () {
    $($(this).attr("data-ref-check")).iCheck("check").trigger("ifToggled");
});

$("#selectAll,#createAll,#updateAll,#deleteAll").on("ifUnchecked", function () {
    $($(this).attr("data-ref-check")).iCheck("uncheck");
});

$(".selectCheck,.createCheck,.updateCheck,.deleteCheck").on("ifUnchecked", function () {
    $($(this).attr("data-ref-check")).iCheck("uncheck");
});
function ValidateNumber(object) {
    var numeric = /^[0-9]\d*$/;

    if (!numeric.test($(object).val())) {
        $(object).next("span").text("Value contains invalid characters, only numbers are allowed");
        ScrollView($(object));
        return false;
    }
    else {
        $(object).next("span").text("");
        return true;
    }
}
function ValidateText(object) {
    var text = /^[A-Z a-z]*$/;

    if (!text.test($(object).val())) {
        $(object).next("span").text("Name contains invalid characters, only alphabets are allowed");
        ScrollView($(object));
        return false;
    }
    else {
        $(object).next("span").text("");
        return true;
    }
}
function ValidateText_Custom(object) {
    if ($(object).val() != "") {
        //var text = /^[A-Z a-z]*$/;
        var text = /^[A-Za-z ._\\,()-]+$/;
        debugger;
        if (!text.test($(object).val())) {
            $(object).parent("div").next("span").text("Name contains invalid characters, only alphabets and special character ._,()- are allowed");
            ScrollView($(object));
            return false;
        }
        else {
            $(object).parent("div").next("span").text("");
            return true;
        }
    }
}
function ValidateTextChar(object) {
    if ($(object).val() != "") {
        var text = /^[A-Za-z0-9/_-]*$/;
        debugger;
        if (!text.test($(object).val())) {
            $(object).next("span").text("Value contains invalid characters, only alphabets, numbers and special characters (/,_,-) are allowed");
            ScrollView($(object));
            return false;
        }
        else {
            $(object).next("span").text("");
            return true;
        }
    }
}
function ValidateTextChar_Custom(object) {
    var text = /^[A-Za-z 0-9/_-]*$/;
    debugger;
    if (!text.test($(object).val())) {
        $(object).next("span").text("Value contains invalid characters, only alphabets, numbers and special characters (/,_,-) are allowed");
        ScrollView($(object));
        return false;
    }
    else {
        $(object).next("span").text("");
        return true;
    }
}
function ValidateTextChar_WSpace(object) {
    var text = /^[A-Za-z 0-9/._-]*$/;
    debugger;
    if (!text.test($(object).val())) {
        $(object).next("span").text("Value contains invalid characters, only alphabets, numbers and special characters (/,.,_,-) are allowed");
        ScrollView($(object));
        return false;
    }
    else {
        $(object).next("span").text("");
        return true;
    }
}