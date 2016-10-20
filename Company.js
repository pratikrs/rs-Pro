
CheckJQueryCompany();
var nextRowId;
function CheckJQueryCompany() {
    if (!window.jQuery) {
        setTimeout(CheckJQueryCompany, 1);
    } else {
        $(document).ready(function () {

            $("#btnAddContact").click(function () {
                AddNewContactDetailsRow();
            });

            //$("#btnAddCompany").click(function () {

            //    //return $('#frmCompanyDetail').valid();

            //    //if ($('#frmCompanyDetail').valid()) {

            //    //}

            //    var isValid = false;
            //    $('#tblContactDetails > tbody  > tr').each(function () {
            //        alert($(this).find("input[name*='ContactName']").val());
            //    });

            //    return isValid;
            //});

            //$(".delete-contact0").click(function () {
            $(document).on("click", ".delete-contact", function () {
                var thisRowId = $(this).closest("tr").find("span").attr("id").split("-")[1];
                if (thisRowId == "" || thisRowId == null || thisRowId == "0") {
                    thisRowId = 0;
                }
                nextRowId = ((nextRowId == 0) || (nextRowId == undefined) ? 0 : nextRowId - 1);
                $("#hdfNextInsertValue").val(nextRowId);

                $(this).closest("tr").remove();

                $('#tblContactDetails > tbody  > tr').each(function (index) {
                    if (index >= thisRowId) {
                        $(this).find("span").eq(0).attr("id", "span-" + index);
                        $(this).find("input[name*='lstCompanyContacts']").each(function (i) {
                            var thisName = $(this).attr("name");
                            var temp = thisName.split(index + 1)[0] + index + thisName.split(index + 1)[1];
                            $(this).attr("name", (thisName.split(index + 1)[0] + index + thisName.split(index + 1)[1]));
                        });

                        $(this).find("input[id*='lstCompanyContacts']").each(function (i) {
                            var thisName = $(this).attr("id");
                            var temp = thisName.split(index + 1)[0] + index + thisName.split(index + 1)[1];
                            $(this).attr("id", (thisName.split(index + 1)[0] + index + thisName.split(index + 1)[1]));
                        });
                    }
                });



            });

            //$(".delete-contact").live("click", function () {
            //    debugger
            //});

            $(document).on("change", ".chkCompanyContacts", function () {

                var isChecked = $(this).is(':checked');
                $(".chkCompanyContacts").prop('checked', false);
                $("#tblContactDetails > input[name*='Principal']").val(false);
                $("#tblContactDetails").find("input[name*='Principal']").val(false);
                //debugger

                $(this).prop('checked', isChecked);
                $(this).val(isChecked);
                //$(this).next("input[name*='Principal']").val(isChecked);

            });
            
            if ($("#CompanyId").val() == undefined) {
                if (parseInt($("#CompanyId").val()) <= 0) {
                    $("#hdfNextInsertValue").val("0");
                }
            }
            nextRowId = parseInt($("#hdfNextInsertValue").val());

            $("[data-mask]").inputmask();
        });


    }
}

function AddNewContactDetailsRow() {


    if ($("#hdfNextInsertValue").length) {
        if ($("#hdfNextInsertValue").val() == "" || $("#hdfNextInsertValue").val() == null || $("#hdfNextInsertValue").val() == "0") {
            nextRowId = 0;
        } else {
            nextRowId = parseInt($("#hdfNextInsertValue").val());
        }
        $("#hdfNextInsertValue").val(nextRowId + 1);
    }

    var newRowHtml = '<tr>'
                        + '<td>'
                            + '<span style="display: none;" id="span-' + nextRowId + '">'
                                    //+ '<input type="text" value="" name="lstCompanyContacts['+ nextRowId +'].ContactId" id="lstCompanyContacts_'+ nextRowId +'__ContactId" data-val-required="The ContactId field is required." data-val-number="The field ContactId must be a number." data-val="true" class="form-control">'
                                    //+ '<input type="text" value="" name="lstCompanyContacts['+ nextRowId +'].CompanyId" id="lstCompanyContacts_'+ nextRowId +'__CompanyId" data-val-required="The CompanyId field is required." data-val-number="The field CompanyId must be a number." data-val="true" class="form-control">'
                                + '</span>'
                            + '<input type="text" value="" name="lstCompanyContacts[' + nextRowId + '].ContactName" id="lstCompanyContacts_' + nextRowId + '__ContactName" data-val-required="The ContactName field is required." data-val-length-max="50" data-val-length="The field ContactName must be a string with a maximum length of 50." data-val="true" class="form-control">'
                        + '</td>'
                        + '<td>'
                            + '<input type="text" value="" name="lstCompanyContacts[' + nextRowId + '].Mobile" id="lstCompanyContacts_' + nextRowId + '__Mobile" data-val-length-max="15" data-val-length="The field Mobile must be a string with a maximum length of 15." data-val="true" class="form-control" data-inputmask="' + '\'mask\': \'99999 999 999 99\'"' + 'data-mask>'
                        + '</td>'
                        + '<td>'
                            + '<input type="text" value="" name="lstCompanyContacts[' + nextRowId + '].Phone" id="lstCompanyContacts_' + nextRowId + '__Phone" data-val-length-max="15" data-val-length="The field Phone must be a string with a maximum length of 15." data-val="true" class="form-control" data-inputmask="'+'\'mask\': \'(999) 999-9999\'"'+ 'data-mask>'
                        + '</td>'
                        + '<td>'
                            + '<input type="text" value="" name="lstCompanyContacts[' + nextRowId + '].Email" id="lstCompanyContacts_' + nextRowId + '__Email" data-val-length-max="50" data-val-length="The field Email must be a string with a maximum length of 50." data-val="true" class="form-control">'
                        + '</td>'
                        + '<td style="text-align: center;">'
                            + '<input type="checkbox" value="" name="lstCompanyContacts[' + nextRowId + '].Principal" id="lstCompanyContacts_' + nextRowId + '__Principal" data-val-required="The Principal field is required." data-val="true" class="chkCompanyContacts" checked="checked">'
                        + '</td>'
                        + '<td>'
                            + '<input type="text" value="" name="lstCompanyContacts[' + nextRowId + '].Department" id="lstCompanyContacts_' + nextRowId + '__Department" class="form-control">'
                        + '</td>'
                        + '<td><button type="button" class="btn btn-danger delete-contact"><b>X</b></button></td>'
                    + '</tr>';
    $("#tblContactDetails tbody").append(newRowHtml);

    if (nextRowId == 0) {
        $(".chkCompanyContacts").eq(0).prop("checked", true);
        $(".chkCompanyContacts").eq(0).val(true);
    } else {
        $(".chkCompanyContacts").eq(nextRowId).prop("checked", false);
        $(".chkCompanyContacts").eq(nextRowId).val(false);
    }
    //CheckPrincipal();
    $("[data-mask]").inputmask();
}