$(document).ready(function () {

    // Initialize select2 library on select elements
    $("select").select2({
        templateResult: function (object) {
            if ($(object.element).attr("data-flag-url")) {
                if (!object.text.startsWith("--- ")) {
                    var span = $("<span><img src='" + $(object.element).attr("data-flag-url") + "' width='20px' style='margin-right:10px;'/>" + object.text + "</span>");
                    return span;
                }
                else {
                    var span = $("<span>" + object.text + "</span>");
                    return span;
                }
            }
            else {
                var span = $("<span>" + object.text + "</span>");
                return span;
            }
        },
        templateSelection: function (object) {
            if ($(object.element).attr("data-flag-url")) {
                if (!object.text.startsWith("--- ")) {
                    var span = $("<span><img src='" + $(object.element).attr("data-flag-url") + "' width='20px' style='margin-right:10px;'/>" + object.text.replace((' - ' + $(object.element).attr("data-country-name")), '') + "</span>");
                    return span;
                }
                else {
                    var span = $("<span>" + object.text + "</span>");
                    return span;
                }
            }
            else {
                var span = $("<span>" + object.text + "</span>");
                return span;
            }
        }
    });

    $(document).find("select").on("change", function () { $(this).trigger("blur"); });
    $("#status_sk").val($("#custom_status_sk option:selected").val());

    setTimeout(function () {
        $(document).find(".select2-selection--multiple").css("border-radius", "5px");
        $(document).find(".select2-selection--multiple").attr('style', 'min-height:50px;');
    }, 1000);

    $("#email").on("keyup", function () {
        $("#username").val($(this).val());
    });
    $("#email").on("focusout", function () {
        var page = document.location.href.match(/[^\/]+$/)[0];
        //var val = page == "Create" ? "c" : "e";
        if (!ValidateEmailRegistered($(this), page)) {
            return;
        }
    });
    //iCheck for checkbox and radio inputs
    $('input[type="checkbox"].minimal').iCheck({
        checkboxClass: 'icheckbox_minimal-blue'
    });

    RemoveSelectCaret();

    $('.disabled').removeClass("disabled");

    $("#status_bool").on("change", function () {
        debugger;
        if ($(this).prop("checked")) {
            $("#record_status").val('0');
        }
        else {
            $("#record_status").val('1');
        }
    });
});

function SetPhoneNumberLength(object) {
    var code = $(object).find(":selected").val();

    if (code != "") {
        var maxLength = $(object).find(":selected").attr("data-phone-len");

        var obj = $(object).parent("div").next("div").find("input[type='text']");
        obj.attr("minlength", 4);
        obj.attr("maxlength", maxLength);

        InitMaxLength(obj);
    }
}

//$("#mobile_cntry_cde").on("change", function () {
//    debugger;
//    var id = $(this).attr("id");
//    if ($(this).find(":selected").val() != "")
//        $("#select2-" + id + "-container").html('<span><img src=' + $(this).find(":selected").attr("data-flag-url") + ' width="20px" style="margin-right:10px;">' + $(this).find(":selected").val() + '</span>');
//});

//function ShowSummaryModal() {
//    $("#summary-modal").modal();
//}



// Currency Advance Search
function GetCurrency(object) {

    var country_id = $(object).find(":selected").val();

    if (country_id != "") {
        $.ajax({
            url: '/Transactions/Remittance/GetCurrencyByCountry/',
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { country_id: country_id },
            success: function (data) {
                if (data.length > 0) {
                    $("input[name='rmtnc_dtl.f_currency_sk']").val(data[0].currency_sk);
                    $("#f_currency_name").html(data[0].currency_name);
                    $("#f_currency_img").attr("src", data[0].img_url);
                    $("#f_currency_code").html(data[0].currency_code);

                    GetExchangeRateForCurrency(data[0].currency_sk);
                }
                else {
                    ResetForeignCurrency();
                    Notify("error", "Currency details not available");
                }
            },
            error: function (data) {
                ResetForeignCurrency();
                Notify("error", "Error getting currency details");
            },
            failure: function (data) {
                ResetForeignCurrency();
                Notify("error", "Failure getting currency details");
            }
        });
    }
    else {
        ResetForeignCurrency();
    }
}

// Currency Search for Edit
function GetCurrencyForEdit() {

    var country_id = $("#country_sk").find(":selected").val();

    $.ajax({
        url: '/Transactions/Remittance/GetCurrencyByCountry/',
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { country_id: country_id },
        success: function (data) {
            if (data != null) {
                $("input[name='rmtnc_dtl.f_currency_sk']").val(data[0].currency_sk);
                $("#f_currency_name").html(data[0].currency_name);
                $("#f_currency_img").attr("src", data[0].img_url);
                $("#f_currency_code").html(data[0].currency_code);
            }
            else {
                ResetForeignCurrency();
                Notify("error", "Currency details not available");
            }
        },
        error: function (data) {
            ResetForeignCurrency();
            Notify("error", "Error getting currency details");
        },
        failure: function (data) {
            ResetForeignCurrency();
            Notify("error", "Failure getting currency details");
        }
    });
}

function ResetForeignCurrency() {
    $("input[name='rmtnc_dtl.f_currency_sk']").val("");
    $("#f_currency_name").html("");
    $("#f_currency_img").attr("src", "");
    $("#f_currency_code").html("");
    $("input[name='rmtnc_dtl.exch_rate']").val("0.000000");
    $("input[name='rmtnc_dtl.exch_rate']").attr("data-min", "0.00");
    $("input[name='rmtnc_dtl.exch_rate']").attr("data-max", "0.00");
}

// Get Currency Exchange Rate
function GetExchangeRateForCurrency(currency_id) {
    $.ajax({
        url: '/Transactions/Remittance/GetExchangeRateForCurrency/',
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { currency_id: currency_id },
        success: function (data) {
            if (data != null) {
                debugger;
                var rate = 0;
                var txn_type = "S";

                if (txn_type == "B") {
                    rate = ((parseFloat(data.rate_min) + parseFloat(data.rate_max)) / 2).toFixed(6);
                }
                else if (txn_type == "S") {
                    rate = (((parseFloat(data.sale_min) + parseFloat(data.selling_margin)) + (parseFloat(data.sale_max) + parseFloat(data.selling_margin))) / 2).toFixed(6);
                }

                $("input[name='rmtnc_dtl.exch_rate']").val(addCommas(rate)).trigger("focusout");
                $("input[name='rmtnc_dtl.exch_rate']").attr("data-min", (parseFloat(data.sale_min) + parseFloat(data.selling_margin)).toFixed(6));
                $("input[name='rmtnc_dtl.exch_rate']").attr("data-max", (parseFloat(data.sale_max) + parseFloat(data.selling_margin)).toFixed(6));
            }
            else {
                $("input[name='rmtnc_dtl.exch_rate']").val("0.000000").trigger("focusout");
                $("input[name='rmtnc_dtl.exch_rate']").attr("data-min", "0.00");
                $("input[name='rmtnc_dtl.exch_rate']").attr("data-max", "0.00");
                Notify("error", "Exchange rate not available");
            }
        },
        error: function (data) {
            $("input[name='rmtnc_dtl.exch_rate']").val("0.000000").trigger("focusout");
            $("input[name='rmtnc_dtl.exch_rate']").attr("data-min", "0.00");
            $("input[name='rmtnc_dtl.exch_rate']").attr("data-max", "0.00");
            Notify("error", "Error getting exchange rate");
        },
        failure: function (data) {
            $("input[name='rmtnc_dtl.exch_rate']").val("0.000000").trigger("focusout");
            $("input[name='rmtnc_dtl.exch_rate']").attr("data-min", "0.00");
            $("input[name='rmtnc_dtl.exch_rate']").attr("data-max", "0.00");
            Notify("error", "Failure getting exchange rate");
        }
    });
}

var customerRequestSent = 0;
// Customer Advance Search
function AdvanceCustomerSearch(event) {
    if (customerRequestSent == 0) {
        customerRequestSent = 1;

        $.ajax({
            url: '/Transactions/ForeignCurrency/AdvanceCustomerSearch/',
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                // If already is DataTable then clear and destroy
                if ($.fn.dataTable.isDataTable("#tblSearchCustomer")) {
                    $("#tblSearchCustomer").DataTable().clear().destroy();
                }

                var table = $("#tblSearchCustomer").dataTable({
                    data: data,
                    "order": [],
                    columns: [
                        { "data": "remtr_sk" },
                        {
                            "data": "frst_nme", "render": function (data, type, row) {
                                return row.frst_nme + ((row.mid_nme == null) ? '' : (' ' + row.mid_nme)) + ((row.last_nme == null) ? '' : (' ' + row.last_nme))
                            }
                        },
                        { "data": "last_nme" },
                        { "data": "remtr_typ" },
                        { "data": "member_id" },
                        { "data": "doc_br" },
                        { "data": "addr" },
                        { "data": "mobile_cde" },
                        { "data": "mobile_nbr" },
                        { "data": "email_addr" },
                        { "data": "ref_cde" },
                        { "data": "country_desc" },
                        { "data": "city_desc" },
                        { "data": "img_url" },
                        { "data": "custmr_rmrks" }
                    ],
                    columnDefs:
                        [
                            { "targets": 0, "visible": false, "width": "0%" }
                            , { "targets": 1, "width": "20%" }
                            , { "targets": 2, "visible": false, "width": "0%" }
                            , { "targets": 3, "width": "10%" }
                            , { "targets": 4, "width": "15%" }
                            , { "targets": 5, "width": "25%" }
                            , { "targets": 6, "visible": false, "width": "0%" }
                            , { "targets": 7, "visible": false, "width": "0%" }
                            , { "targets": 8, "width": "15%" }
                            , { "targets": 9, "width": "15%" }
                            , { "targets": 10, "visible": false, "width": "0%" }
                            , { "targets": 11, "visible": false, "width": "0%" }
                            , { "targets": 12, "visible": false, "width": "0%" }
                            , { "targets": 13, "visible": false, "width": "0%" }
                            , { "targets": 14, "visible": false, "width": "0%" }
                        ]
                });

                $("#customer-modal").modal('show');
                customerRequestSent = 0;
            },
            error: function (data) {
                Notify("error", "Error getting customer list");
                customerRequestSent = 0;
            },
            failure: function (data) {
                Notify("error", "Failure getting customer list");
                customerRequestSent = 0;
            }
        });
    }
}

// Bind row click event
$(document).on("click", "#tblSearchCustomer tbody tr", function () {
    var data = $("#tblSearchCustomer").DataTable().row($(this)).data();
    $("#remtr_sk").val(data.remtr_sk).trigger("change");
    $("#remtr_name").val(data.frst_nme);
    var customerType = "Customer (" + ((data.remtr_typ == "Individual") ? "Indv." : "Corp.") + ")";
    $("#customer_type").html(customerType);
    $("#city").val(data.city_desc);
    $("#addr").val(data.addr);
    $("#email_addr").val(data.email_addr);
    $("#mobile_nbr").val(data.mobile_nbr).attr("value", data.mobile_nbr).trigger("blur");
    $("#mobile_cntry_cde").val($("#mobile_cntry_cde option:contains(" + data.mobile_cde + ")").val()).trigger("change");
    $("#rmks").val(data.custmr_rmrks);
    $("#customer-modal").modal("hide");

    $("#doc_typ_sk").attr("disabled", "disabled");
    $("#doc_customer_type").val("").trigger("change");
});

$("#customer-modal").on("shown.bs.modal", function () {
    var table = $('#tblSearchCustomer').DataTable();
    table.columns.adjust();
});

var beneficiaryRequestSent = 0;
// Beneficiary Advance Search
function AdvanceBeneficiarySearch(event) {
    if (beneficiaryRequestSent == 0) {
        var country_id = $("#country_sk option:selected").val();

        if (country_id != "") {
            beneficiaryRequestSent = 1;

            $.ajax({
                url: '/Transactions/Remittance/AdvanceBeneficiarySearch/',
                type: 'GET',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: { country_id: country_id },
                success: function (data) {

                    // If already is DataTable then clear and destroy
                    if ($.fn.dataTable.isDataTable("#tblSearchBeneficiary")) {
                        $("#tblSearchBeneficiary").DataTable().clear().destroy();
                    }

                    var table = $("#tblSearchBeneficiary").dataTable({
                        data: data,
                        "order": [],
                        columns: [
                            { "data": "cntr_party_nme" },
                            { "data": "mobile_nbr" },
                            { "data": "email_addr" },
                            { "data": "crsp_bank_nme" },
                            { "data": "crsp_bank_iban" },
                            { "data": "bank_acc_no" },
                            { "data": "crsp_bank_br" },
                            { "data": "cntr_party_rmrks" },
                            { "data": "cntr_party_addr" },
                            { "data": "city_desc" },
                            { "data": "country_desc" },
                            { "data": "mobile_cntry_cde" },
                            { "data": "cntr_party_sk" },
                            { "data": "custmr_sk" },
                            { "data": "country_sk" },
                            { "data": "city_sk" },
                            { "data": "img_url" },
                            { "data": "bank_sk" }
                        ],
                        columnDefs:
                            [
                                { "targets": 0, "width": "20%" }
                                , { "targets": 1, "width": "10%" }
                                , { "targets": 2, "width": "10%" }
                                , { "targets": 3, "width": "15%" }
                                , { "targets": 4, "width": "10%" }
                                , { "targets": 5, "width": "10%" }
                                , { "targets": 6, "width": "15%" }
                                , { "targets": 7, "width": "10%" }
                                , { "targets": 8, "visible": false }
                                , { "targets": 9, "visible": false }
                                , { "targets": 10, "visible": false }
                                , { "targets": 11, "visible": false }
                                , { "targets": 12, "visible": false }
                                , { "targets": 13, "visible": false }
                                , { "targets": 14, "visible": false }
                                , { "targets": 15, "visible": false }
                                , { "targets": 16, "visible": false }
                                , { "targets": 17, "visible": false }
                            ]
                    });

                    $("#beneficiary-modal").modal('show');

                    beneficiaryRequestSent = 0;
                },
                error: function (data) {
                    Notify("error", "Error getting beneficiary list");
                    beneficiaryRequestSent = 0;
                },
                failure: function (data) {
                    Notify("error", "Failure getting beneficiary list");
                    beneficiaryRequestSent = 0;
                }
            });
        }
        else {
            Notify("info", "Please select country");
        }
    }
}

$("#beneficiary-modal").on("shown.bs.modal", function () {
    var table = $('#tblSearchBeneficiary').DataTable();
    table.columns.adjust();
});

// Bind row click event
$(document).on("click", "#tblSearchBeneficiary tbody tr", function () {
    var data = $("#tblSearchBeneficiary").DataTable().row($(this)).data();
    $("#beneficiary_name").val(data.cntr_party_nme);
    $("#beneficiary_addr").val(data.cntr_party_addr);
    $("#beneficiary_sk").val(data.custmr_sk).trigger("change");
    //$("#beneficiary_location").html("<span><img src='" + data.img_url + "' style='margin-right: 10px;' width='20px'>&nbsp;" + data.city_desc + ",&nbsp;" + data.country_desc+"</span>");
    //$("#beneficiary_location").html("<span>" + data.city_desc + ",&nbsp;" + data.country_desc + "</span>");
    //$("#beneficiary_location").val(data.city_desc + ", " + data.country_desc);
    $("#bn_email_addr").val(data.email_addr).trigger("blur");
    $("#bn_mobile_nbr").val(data.mobile_nbr).attr("value", data.mobile_nbr).trigger("blur");
    //$("#bn_mobile_cntry_cde").val(data.mobile_cntry_cde).trigger("change");
    $("#bank_sk").val(data.bank_sk).trigger("change");
    $("#crsp_bank_br").val(data.crsp_bank_br);
    $("#crsp_bank_iban").val(data.crsp_bank_iban);
    $("#bank_acc_no").val(data.bank_acc_no);
    $("#beneficiary-modal").modal("hide");

    $("#doc_typ_sk").attr("disabled", "disabled");
    $("#doc_customer_type").val("").trigger("change");
});

var agentRequestSent = 0;
//function AdvanceAgentSearch() {
//    if (agentRequestSent == 0) {
//        var country_id = $("#country_sk option:selected").val();

//        if (country_id != "") {
//            agentRequestSent = 1;

//            $.ajax({
//                url: '/Transactions/Remittance/GetCountryAgentsList/',
//                type: 'GET',
//                contentType: "application/json; charset=utf-8",
//                dataType: "json",
//                data: { country_id: country_id },
//                success: function (data) {

//                    // If already is DataTable then clear and destroy
//                    if ($.fn.dataTable.isDataTable("#tblSearchAgent")) {
//                        $("#tblSearchAgent").DataTable().clear().destroy();
//                    }

//                    var table = $("#tblSearchAgent").dataTable({
//                        data: data,
//                        "order": [],
//                        columns: [
//                            { "data": "crsp_agnt_cde" },
//                            { "data": "crsp_agnt_desc" },
//                            { "data": "crsp_typ_desc" },
//                            { "data": "crsp_agnt_addr" },
//                            { "data": "crsp_agnt_rmrks" },
//                            { "data": "crsp_agnt_sk" },
//                            { "data": "crsp_typ_sk" }
//                        ],
//                        columnDefs:
//                            [
//                                { "targets": 0, "width": "10%" }
//                                , { "targets": 1, "width": "20%" }
//                                , { "targets": 2, "width": "10%" }
//                                , { "targets": 3, "width": "20%" }
//                                , { "targets": 4, "width": "20%" }
//                                , { "targets": 5, "visible": false }
//                                , { "targets": 6, "visible": false }
//                            ]
//                    });

//                    $("#agents-modal").modal('show');

//                    agentRequestSent = 0;
//                },
//                error: function (data) {
//                    Notify("error", "Error getting agents list");
//                    agentRequestSent = 0;
//                },
//                failure: function (data) {
//                    Notify("error", "Failure getting agents list");
//                    agentRequestSent = 0;
//                }
//            });
//        }
//        else {
//            Notify("info", "Please select country");
//        }
//    }
//}
function AdvanceAgentSearch() {
    if (agentRequestSent == 0) {
        var country_id = $("#country_sk option:selected").val();

        if (country_id != "") {
            agentRequestSent = 1;

            $.ajax({
                url: '/Transactions/Remittance/GetCountrySubAgentsList/',
                type: 'GET',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: { country_id: country_id },
                success: function (data) {

                    // If already is DataTable then clear and destroy
                    if ($.fn.dataTable.isDataTable("#tblSearchAgent")) {
                        $("#tblSearchAgent").DataTable().clear().destroy();
                    }

                    var table = $("#tblSearchAgent").dataTable({
                        data: data,
                        "order": [],
                        columns: [
                            { "data": "sub_agent_desc" },
                            { "data": "main_agent_desc" },
                            { "data": "sub_agent_sk" },
                            { "data": "main_agent_sk" }
                        ],
                        columnDefs:
                            [
                                { "targets": 0, "width": "40%" }
                                , { "targets": 1, "width": "60%" }
                                , { "targets": 2, "visible": false }
                                , { "targets": 3, "visible": false }
                            ]
                    });

                    $("#agents-modal").modal('show');

                    agentRequestSent = 0;
                },
                error: function (data) {
                    Notify("error", "Error getting agents list");
                    agentRequestSent = 0;
                },
                failure: function (data) {
                    Notify("error", "Failure getting agents list");
                    agentRequestSent = 0;
                }
            });
        }
        else {
            Notify("info", "Please select country");
        }
    }
}

$("#agents-modal").on("shown.bs.modal", function () {
    var table = $('#tblSearchAgent').DataTable();
    table.columns.adjust();
});

// Bind row click event
//$(document).on("click", "#tblSearchAgent tbody tr", function () {
//    var data = $("#tblSearchAgent").DataTable().row($(this)).data();
//    $("#agent_name").val(data.crsp_agnt_cde + " - " + data.crsp_agnt_desc).attr("value", (data.crsp_agnt_cde + " - " + data.crsp_agnt_desc));
//    $("#agent_sk").val(data.crsp_agnt_sk).trigger("change");
//    $("#agents-modal").modal("hide");
//});
$(document).on("click", "#tblSearchAgent tbody tr", function () {
    var data = $("#tblSearchAgent").DataTable().row($(this)).data();
    $("#agent_name").val(data.sub_agent_desc).attr("value", data.sub_agent_desc);
    $("#agent_sk").val(data.sub_agent_sk).trigger("change");
    $("#agents-modal").modal("hide");
});

var box;
// Bind events for calculating specific row cash till
$(document).on("focusin", "#rmtnc_dtl_f_currency_amt,#rmtnc_dtl_exch_rate,#rmtnc_dtl_b_currency_amt", function () {
    box = $(this);
}).on("focusout", "#rmtnc_dtl_f_currency_amt,#rmtnc_dtl_exch_rate,#rmtnc_dtl_b_currency_amt", function () {
    var fc_amount = $("#rmtnc_dtl_f_currency_amt").val().replace(/,/g, '');
    var b_amount = $("#rmtnc_dtl_b_currency_amt").val().replace(/,/g, '');
    var exchg_rate = $("#rmtnc_dtl_exch_rate").val().replace(/,/g, '');

    var regEx = /\d/;
    var calculate = 1;

    if (!regEx.test(exchg_rate) || !(parseFloat(exchg_rate) > 0)) {
        $("#rmtnc_dtl_exch_rate").val("0.000000");
        calculate = 0;
    }

    if (calculate == 1) {
        if ($(this).attr("id").endsWith("f_currency_amt")) {
            if (!regEx.test(fc_amount)) {
                $("#rmtnc_dtl_f_currency_amt").val("0.00");
            }
            else {
                var amount = (parseFloat(fc_amount).toFixed(2) / parseFloat(exchg_rate).toFixed(6)).toFixed(2);
                $("#rmtnc_dtl_b_currency_amt").val(amount);
            }
        }
        else if ($(this).attr("id").endsWith("b_currency_amt")) {
            if (!regEx.test(b_amount)) {
                $("#rmtnc_dtl_b_currency_amt").val("0.00");
            }
            else {
                var amount = (parseFloat(b_amount).toFixed(2) * parseFloat(exchg_rate).toFixed(6)).toFixed(2);
                $("#rmtnc_dtl_f_currency_amt").val(amount);
            }
        }
        else if ($(this).attr("id").endsWith("exch_rate")) {
            if (regEx.test(fc_amount) && (parseFloat(fc_amount) > 0)) {
                var amount = (parseFloat(fc_amount).toFixed(2) / parseFloat(exchg_rate).toFixed(6)).toFixed(2);
                $("#rmtnc_dtl_b_currency_amt").val(amount);
            }
            else if (regEx.test(b_amount) && (parseFloat(b_amount) > 0)) {
                var amount = (parseFloat(b_amount).toFixed(2) * parseFloat(exchg_rate).toFixed(6)).toFixed(2);
                $("#rmtnc_dtl_f_currency_amt").val(amount);
            }
        }
    }

    $("#rmtnc_dtl_f_currency_amt").attr("value", addCommas(parseFloat($("#rmtnc_dtl_f_currency_amt").val().replace(/,/g, '')).toFixed(2))).val(addCommas(parseFloat($("#rmtnc_dtl_f_currency_amt").val().replace(/,/g, '')).toFixed(2)));
    $("#rmtnc_dtl_b_currency_amt").attr("value", addCommas(parseFloat($("#rmtnc_dtl_b_currency_amt").val().replace(/,/g, '')).toFixed(2))).val(addCommas(parseFloat($("#rmtnc_dtl_b_currency_amt").val().replace(/,/g, '')).toFixed(2)));
    $("#rmtnc_dtl_exch_rate").attr("value", addCommas(parseFloat($("#rmtnc_dtl_exch_rate").val().replace(/,/g, '')).toFixed(6))).val(addCommas(parseFloat($("#rmtnc_dtl_exch_rate").val().replace(/,/g, '')).toFixed(6)));

    CalculateTotal();
});

function addCommas(nStr) {
    nStr += '';
    var x = nStr.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

$("#charges").on("change", function () {
    var charges = $(this).val().replace(/,/g, '');
    var vat_per = $("#vat_per").html();
    var regEx = /\d/;

    if (regEx.test(charges)) {
        var vat = ((parseFloat(charges) * parseFloat(vat_per)) / 100).toFixed(2);
        $("#VAT").val(addCommas(vat));

        $(this).val(addCommas(parseFloat(charges).toFixed(2)));
    }
    else {
        $(this).val("0.00");
        $("#VAT").val("0.00");
    }

    CalculateTotal();
});

function CalculateTotal() {
    var charges = $("#charges").val().replace(/,/g, '');
    var vat = $("#VAT").val().replace(/,/g, '');
    var b_amount = $("#rmtnc_dtl_b_currency_amt").val().replace(/,/g, '');

    var amount = (parseFloat(charges) + parseFloat(vat) + parseFloat(b_amount)).toFixed(2);

    $("#net_amount").val(addCommas(amount));
}

$("#country_sk").on("change", function () {
    ResertBeneficiaryFields();

    if ($("#country_sk option:selected").val() == "") {
        $("#country_sk").parent().find("span[data-valmsg-for='country_sk']").html("Country is required");
        $("#country_sk").focus();
    }
    else {
        $("#country_sk").parent().find("span[data-valmsg-for='country_sk']").html("");

        GetBanksList($("#country_sk option:selected").val());
        $("#beneficiary_location").html("<span><img src='" + $("#country_sk option:selected").attr("data-flag-url") + "' style='margin-right: 10px;' width='20px'>&nbsp;&nbsp;" + $("#country_sk option:selected").text() + "</span>");
        $("#custom_bn_mobile_cntry_cde").val($("#custom_bn_mobile_cntry_cde option:contains('" + $("#country_sk option:selected").text() + "')").val()).trigger("change");
        $("#bn_mobile_cntry_cde").val($("#custom_bn_mobile_cntry_cde option:contains('" + $("#country_sk option:selected").text() + "')").val()).attr("value", $("#custom_bn_mobile_cntry_cde option:contains('" + $("#country_sk option:selected").text() + "')").val());

        $("#crsp_bank_iban").attr("maxlength", $("#country_sk option:selected").attr("data-iban-length"));
        InitMaxLength("#crsp_bank_iban");
    }

    $("#agent_sk").val("");
    $("#agent_name").val("");
    ToggleBankList();
});

$("#agent_sk").on("change", function () {
    if ($(this).val() == "") {
        $(this).next("span").html("Agent is required");
    }
    else {
        $(this).next("span").html("");
    }
});

$("#remtr_sk").on("change", function () {
    if ($(this).val() == "") {
        $(this).next("span").html("Customer is required");
    }
    else {
        $(this).next("span").html("");
    }
});

$("#custom_status_sk").on("change", function () {
    if ($("#custom_status_sk option:selected").val() == "") {
        $("#status_sk").val("");
        $(this).focus();
    }
    else {
        $("#status_sk").val($("#custom_status_sk option:selected").val());
    }
});

$("#rmtnc_doc_dte").on("change", function () {
    if ($(this).val() == "") {
        $(this).next("span").html("Date is required");
        $(this).focus();
    }
    else {
        $(this).next("span").html("");
    }
});

$("#rmtnc_dtl_f_currency_sk").on("change", function () {
    if ($(this).val() == "" || $("#rmtnc_dtl_f_currency_sk").val() <= 0) {
        $(this).next("span").html("Frgn. Currency is required");
        $(this).focus();
    }
    else {
        $(this).next("span").html("");
    }
});

$("#rmtnc_dtl_status_sk").on("change", function () {
    if ($(this).find(":selected").val() == "") {
        $(this).focus();
    }
    else {
        if ($(this).find(":selected").text() == "Pre-Approved") {
            var ex = $("#rmtnc_dtl_exch_rate");
            var min = $(ex).attr("data-min");
            var max = $(ex).attr("data-max");

            $(ex).attr("min", min);
            $(ex).attr("max", max);
        }
        else {
            var ex = $("#rmtnc_dtl_exch_rate");
            $(ex).removeAttr("min");
            $(ex).removeAttr("max");
        }
    }
});

// Check Email Already Registered
function ValidateEmailRegistered(email, page) {
    $.ajax({
        url: '/UserManagement/User/ValidateEmailRegistered/',
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { email: email.val(), page: page },
        success: function (data) {
            if (data == false) {
                Notify("error", "Email already registered");
                email.attr('style', 'border:1px solid red;');
            }
            else { email.removeAttr('style'); }
            return data;

        },
        error: function (data) {
            Notify("error", "Error occured");
        },
        failure: function (data) {
            Notify("error", "Fail Error occured");
        }
    });
}

// Validate Form
function ValidateForm(sender) {
    var page = document.location.href.match(/[^\/]+$/)[0];
    //var val = page == "Create" ? "c" : "e";
    validate = true;
    //if ($("form").valid()) {
    //    if (!ValidateText($("#user_full_name"))) {
    //        validate = false;
    //    }

    //    if (!ValidatePasswordRegEx($("#password").val(), $("#pswdVal"))) {
    //        validate = false;
    //    }

    //    if (!ValidateNumber($("#mobile_nbr"))) {
    //        validate = false;
    //    }

    //    $("#sender").val(sender);
    //    if (validate) {
    //        $("form").submit();
    //    }
    //}
    if ($("#role_sk").val() == "") {
        $("span[data-valmsg-for='role_sk']").text('User Role is Required');
        validate = false;
    }
    else {
        $("span[for='role_sk']").text('');
    }
    if ($("#br_code").val() == "") {
        $("span[data-valmsg-for='br_code']").text('Branch Access required.');
        validate = false;
    }
    else {
        $("span[for='role_sk']").text('');
    }
    if (validate) {
        $("form").submit();
    }
    // $("form").submit();
}

$("#bn_new_check").on("change", function () {

    //$("#beneficiary-details").find("input[type='text']").val("");
    //$("#beneficiary-details").find("select").val("").trigger("change");
    //$("#beneficiary_sk").val("");
    //$("#bank_sk").val("").trigger("change");

    if ($(this).prop("checked")) {
        $("#beneficiary-details").find("input[type='text']").removeAttr("readonly");

        //$("#beneficiary-details").find("select").removeAttr("disabled");
    }
    else {
        $("#beneficiary_name").attr("readonly", "readonly");
        $("#beneficiary_addr").attr("readonly", "readonly");
        $("#beneficiary_location").attr("readonly", "readonly");

        $("#crsp_bank_br").attr("readonly", "readonly");
        $("#crsp_bank_iban").attr("readonly", "readonly");
        $("#bank_acc_no").attr("readonly", "readonly");
    }

    $("#beneficiary-details").find("input:first").focus();
    ToggleBankList();
});

function ResertBeneficiaryFields() {
    $("#bn_new_check").prop("checked", false);

    $("#beneficiary_name").attr("readonly", "readonly");
    $("#beneficiary_name").val("");

    $("#beneficiary_addr").attr("readonly", "readonly");
    $("#beneficiary_addr").val("");

    $("#beneficiary_location").attr("readonly", "readonly");
    $("#beneficiary_location").html("");

    $("#bn_mobile_nbr").val("");
    $("#bn_email_addr").val("");

    $("#crsp_bank_br").attr("readonly", "readonly");
    $("#crsp_bank_br").val("");

    $("#crsp_bank_iban").attr("readonly", "readonly");
    $("#crsp_bank_iban").val("");

    $("#bank_acc_no").attr("readonly", "readonly");
    $("#bank_acc_no").val("");
}

function AddNewBank() {
    var country_sk = $("#country_sk option:selected").val();

    if (country_sk != "") {
        $("#bank_country").html("<span><img src='" + $("#country_sk option:selected").attr("data-flag-url") + "' style='margin-right:10px;' width='20px'>&nbsp;" + $("#country_sk option:selected").text() + "</span>");
        $("#bank_iban_len").val($("#country_sk option:selected").attr("data-iban-length") + " characters");
        $("#bank_name").val("");

        $("#bank-modal").modal();
    }
    else {
        Notify("info", "Please select country");
    }
}

function SaveBank() {
    var bank_name = $("#bank_name").val().trim();
    var country_sk = $("#country_sk option:selected").val();

    var regEx = /[A-Za-z0-9. -]/;

    if (regEx.test(bank_name)) {
        $("#bank_name").next("span").html("");

        var jsonObj = {};
        jsonObj["bank_name"] = bank_name;
        jsonObj["country_sk"] = country_sk;

        var json = JSON.stringify(jsonObj);

        $.ajax({
            url: '/Setup/Bank/SaveBank',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: json,
            success: function (response) {
                if (response.status) {
                    GetBanksList(country_sk);
                    setTimeout(function () { $("#bank_sk").val($("#bank_sk option:contains('" + bank_name + "')").val()).trigger("change"); }, 1000);
                    $("#bank-modal").modal("hide");
                }
                else {
                    Notify("error", "Request failed!");
                }
            },
            error: function (response) {
                Notify("error", "Error saving bank");
                console.log(response);
            },
            failure: function (response) {
                Notify("error", "Failure saving bank");
                console.log(response);
            }
        });
    }
    else {
        $("#bank_name").next("span").html("Bank name contains invalid characters");
    }
}

function RemoveSelectCaret() {
    $(".remove-caret").next("span").find(".select2-selection__arrow").css("display", "none");
}

function ToggleBankList() {
    var country_sk = $("#country_sk option:selected").val();

    if (country_sk != "") {
        var banks_len = $("#bank_sk").find("option").length;
        if (banks_len > 1) {
            if ($("#bn_new_check").prop("checked"))
                $("#bank_sk").removeAttr("disabled");
            else
                $("#bank_sk").attr("disabled", "disabled");
        }
        else {
            var html = "<option value=''>--- Select Bank ---</option>";
            $("#bank_sk").html(html).trigger("change");
            $("#bank_sk").attr("disabled", "disabled");
        }
    }
    else {
        var html = "<option value=''>--- Select Bank ---</option>";
        $("#bank_sk").html(html).trigger("change");
        $("#bank_sk").attr("disabled", "disabled");
    }

    $("#bank_sk").next("span").find(".select2-selection__arrow").css("display", "block");
}

$("#doc_customer_type").on("change", function () {
    ToggleDocumentTypes();
});

function ToggleDocumentTypes() {
    var fileType = $("#doc_customer_type option:selected").val();

    if (fileType == "R") {
        var remtr_sk = $("#remtr_sk").val();

        if (remtr_sk > 0) {
            if ($("#customer_type").text().indexOf("Indv.") >= 0) {
                GetDocumentTypes("Individual");
            }
            else if ($("#customer_type").text().indexOf("Corp.") >= 0) {
                GetDocumentTypes("Corporate");
            }
        }
        else {
            Notify("info", "Please select remitter");
            $("#doc_typ_sk").val("").trigger("change");
            $("#doc_typ_sk").attr("disabled", "disabled");
        }
    }
    else if (fileType == "B") {
        var beneficiary_sk = $("#beneficiary_sk").val();

        if (beneficiary_sk > 0) {
            GetDocumentTypes("Beneficiary");
        }
        else {
            Notify("info", "Please select beneficiary");
            $("#doc_typ_sk").val("").trigger("change");
            $("#doc_typ_sk").attr("disabled", "disabled");
        }
    }

    return;
}

function GetDocumentTypes(member_type) {
    var html = "<option value=''>--- Select Document Type ---</option>";

    // Get banks list
    $.ajax({
        url: "/Transactions/Remittance/GetDocumentsList",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { member_type: member_type },
        success: function (data) {
            if (data != null && data != "") {
                $.each(data, function (index, object) {
                    html += "<option value='" + object.value + "'>" + object.text + "</option>";
                });

                $("#doc_typ_sk").html(html).trigger("change");
                $("#doc_typ_sk").removeAttr("disabled");
            }
            else {
                $("#doc_typ_sk").html(html).trigger("change");
                $("#doc_typ_sk").attr("disabled", "disabled");
            }

            ToggleBankList();
        },
        error: function (response) {
            console.log(response);
            $("#doc_typ_sk").html(html).trigger("change");
            $("#doc_typ_sk").attr("disabled", "disabled");

            Notify("error", "Document types are not available");
        },
        failure: function (response) {
            $("#doc_typ_sk").html(html).trigger("change");
            $("#doc_typ_sk").attr("disabled", "disabled");

            Notify("error", "Failure getting document types");
        }
    });
}

function GetBanksList(country_sk) {
    if (country_sk != "") {
        // Get banks list
        $.ajax({
            url: "/Transactions/Remittance/GetBanksByCountry",
            type: "GET",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { country_id: country_sk },
            success: function (data) {
                if (data != null && data != "") {
                    var html = "<option value=''>--- Select Bank ---</option>";
                    $.each(data, function (index, object) {
                        html += "<option value='" + object.value + "'>" + object.text + "</option>";
                    });

                    $("#bank_sk").html(html).trigger("change");
                }
                else {
                    var html = "<option value=''>--- Select Bank ---</option>";
                    $("#bank_sk").html(html).trigger("change");
                }

                ToggleBankList();
            },
            error: function (response) {
                var html = "<option value=''>--- Select Bank ---</option>";
                $("#bank_sk").html(html).trigger("change");
                ToggleBankList();
                Notify("error", "No banks are available for the country");
            },
            failure: function (response) {
                var html = "<option value=''>--- Select Bank ---</option>";
                $("#bank_sk").html(html).trigger("change");
                ToggleBankList();
                Notify("error", "Failure getting banks' list");
            }
        });
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
