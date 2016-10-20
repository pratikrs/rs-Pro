var _relativeBaseDirectory = '@(Url.Action("", "") == "/" ? "" : Url.Action("", ""))';

        function OpenQuoteDialog(from, companyName, companyId, productName, productId) {
            $.ajax({
                url: _relativeBaseDirectory + "/Quote/Create",
                //data: {
                //    selectedGroup: intSelectedGroup
                //},
                type: 'GET',
                success: function (data) {
                    $("#dvAddQuote").html(data);
                    $("#dvProductHint").hide();
                    $("#dvCompnayHint").hide();
                    if (from == "company") {
                        $("#frmCreateQuote #CompanyName").val(companyName);
                        $("#frmCreateQuote #CompanyName").attr("readonly", "readonly");
                        $("#frmCreateQuote #CompanyId").val(companyId);
                        ProductAutoComplete();
                        $("#dvProductHint").show();
                    } else {
                        $("#frmCreateQuote #ProductName").val(productName);
                        $("#frmCreateQuote #ProductName").attr("readonly", "readonly");
                        $("#frmCreateQuote #ProductId").val(productId);
                        CompanyAutoComplete();
                        $("#dvCompnayHint").show();
                    }

                    $("#AddQuoteDialog").modal({ backdrop: 'static', keyboard: false });
                },
                error: function () {
                    alert("Something went wrong. Our team will look into this or Please try again.");
                    $("#AddQuoteDialog").modal("close");
                }
            });
        }
});		

function DeleteProductById() {
    var productid = $("#deleteModal #hdfProductId").val();
    if (productid != '' && productid != 0 && productid != undefined) {
        $.ajax({
            // Call CreatePartialView action method
            url: _relativeBaseDirectory + "/Product/Delete",
            type: 'POST',
            data: { "id": productid },
            success: function (data) {
                window.location.href = window.location.href;
            },
            error: function () {
                alert("Something went wrong. Our team will look into this or Please try again.");
                $("#deleteModal").modal("close");
            }
        });
    } else {
        $("#deleteModal #hdfProductId").val(0);
        $("#deleteModal").modal("close");
    }
}