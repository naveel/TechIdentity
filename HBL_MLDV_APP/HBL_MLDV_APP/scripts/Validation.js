function validate(mst_Id, doc_typ_sk) {
    debugger;
    if (mst_Id != "") {
        $.ajax({
            url: '/Home/GetValidationMsgs/',
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { doc_mst_sk: mst_Id, doc_typ_sk: doc_typ_sk },
            success: function (data) {
                debugger;
                var html = "";
                //for (i = 0; i < data.length; i++) {
                //    if (data[i].valid_typ == "1") { //Valid
                //        html += "<div class='bg-green color-palette' style='border-radius: 5px;margin: 0 0 8px 0; padding: 5px 30px 5px 15px;'><span style='padding-right:5px;'><i class='fa fa-check'></i></span>" + data[i].description + "</div>";
                //    }
                //    else if (data[i].valid_typ == "2") { //Alert
                //        html += "<div class='bg-orange color-palette' style='border-radius: 5px;margin: 0 0 8px 0; padding: 5px 30px 5px 15px;'><span style='padding-right:5px;'><i class='fa fa-warning'></i></span>" + data[i].description + "</div>";
                //    }
                //    else if (data[i].valid_typ == "3") { //Invalid
                //        html += "<div class='bg-red color-palette' style='border-radius: 5px;margin: 0 0 8px 0; padding: 5px 30px 5px 15px;'><span style='padding-right:5px;'><i class='fa fa-ban'></i></span>" + data[i].description + "</div>";
                //    }
                //    else {
                //        html += "";
                //    }
                //}

                for (i = 0; i < data.length; i++) {
                    if (data[i].valid_typ == "1") { //Valid
                        html += "<li style='padding: 5px 30px 5px 15px;'><span style='padding-right:5px;' class='text-success'><i class='fa fa-check'></i></span>" + data[i].description + "</li>";
                    }
                    else if (data[i].valid_typ == "2") { //Alert
                        html += "<li style='padding: 5px 30px 5px 15px;'><span style='padding-right:5px;' class='text-warning'><i class='fa fa-warning'></i></span>" + data[i].description + "</li>";
                    }
                    else if (data[i].valid_typ == "3") { //Invalid
                        html += "<div class='bg-red disabled color-palette' style='border-radius: 5px;margin: 0 0 8px 0; padding: 5px 30px 5px 15px;'><span style='padding-right:5px;'><i class='fa fa-remove'></i></span>" + data[i].description + "</div>";
                    }
                    else if (data[i].valid_typ == "4") { //High Priorty Alert
                        html += "<div class='bg-warning' style='border-radius: 5px;margin: 0 0 8px 0; padding: 5px 30px 5px 15px;'><span style='padding-right:5px;' class='text-warning'><i class='fa fa-warning'></i></span>" + data[i].description + "</div>";
                    }
                    else {
                        html += "";
                    }
                }

                $("#validationlst").html(html);
                //$('#livalidate').click();
            },
            error: function (data) {
                Notify("error", "Error getting list");
            },
            failure: function (data) {
                Notify("error", "Failure getting list");
            }
        });
    }
}
function SubmitForm(btn) {
    
    debugger;
    var val = "";
    if ($(btn).attr("data-status") == undefined) {
        val = btn;
    }
    else {
        val = $(btn).attr("data-status");
    }
    $('#issubmitform').val(val);
  //  $('#msg_id').text();
    $("form").submit();
    //if ($("form").valid()) {
    //    var validate = true;
    //    if (validate) {
    //        $("form").submit();
    //    }
    //    else {
    //        Notify("error", "Please fill all required fields");
    //        e.preventDefault();
    //        return;
    //    }
    //}
}


function dateconvert(dateAsFromServerSide)
{
    if (dateAsFromServerSide == null) { return ""; }
    var parsedDate = new Date(parseInt(dateAsFromServerSide.substr(6)));
    //alert(parsedDate);
    var jsDate = new Date(parsedDate)
    return setZeroindate(jsDate.getDate()) + "-" + setZeroindate(jsDate.getMonth() + 1) + "-" + jsDate.getFullYear();
    //return jsDate;
}
function dateconvertwithtime(dateAsFromServerSide) {
if (dateAsFromServerSide == null) { return "";}
    var parsedDate = new Date(parseInt(dateAsFromServerSide.substr(6)));
    //alert(parsedDate);
    var jsDate = new Date(parsedDate)
    return setZeroindate(jsDate.getDate()) + "-" + setZeroindate(jsDate.getMonth() + 1) + "-" + jsDate.getFullYear() + " " + jsDate.getHours() + ":" + jsDate.getMinutes();
    //return jsDate;
}
function setZeroindate(res) {
    if (res < 10) {
        return "0" + res;
    }
    else {
        return res;
    }
}
