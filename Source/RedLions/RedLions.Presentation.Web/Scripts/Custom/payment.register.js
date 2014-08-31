$(document).ready(function () {
    $("input[type='checkbox']").change(toggleQuantity);

    $(".payment-quantity").each(function (index) {
        var quantity = $(this).val();
        console.log("Quantity: " + quantity);
        if (quantity == 0) {
            $(this).hide();
        }
    });

    $(".payment-quantity").keydown(function (event) {
        if (event.shiftKey) {
            event.preventDefault();
        }

        if (event.keyCode == 46 || event.keyCode == 8) {
        }
        else {
            if (event.keyCode < 95) {
                if (event.keyCode < 48 || event.keyCode > 57) {
                    event.preventDefault();
                }
            }
            else {
                if (event.keyCode < 96 || event.keyCode > 105) {
                    event.preventDefault();
                }
            }
        }
    });
});

function toggleQuantity() {


    var quantityTextBox = $(this).siblings(".payment-quantity").first();

    var enableCertificate = $(this).is(":checked");

    if (enableCertificate) {
        console.log("Enable checkbox");
        quantityTextBox.show();
        quantityTextBox.val("1");
    }
    else {
        console.log("Disable checkbox");
        quantityTextBox.val("0");
        quantityTextBox.hide();
    }    
}