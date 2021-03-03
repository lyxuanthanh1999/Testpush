var CheckoutAjax = function () {
    this.initialize = function () {
        loadPaymentInformation();
        registerEvents();
    }

    function registerEvents() {
        $("#cash-on-delivery").on("change", function (e) {
            e.preventDefault();
            if (this.checked) {
                $("#check-payment-info").show("slow");
                var userId = $("#hid_current_login_id").val();
                if (userId !== "") {
                    $.ajax({
                        type: "GET",
                        url: "/Cart/GetUserById",
                        data: {
                            id: userId
                        },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            $("#billing-name").val(response.name);
                            $("#billing-phone").val(response.phoneNumber);
                            $("#billing-email").val(response.email);
                        }
                    });
                }
            }
        });
        $("#payment-gateway").on("change", function (e) {
            e.preventDefault();
            if (this.checked) {
                $("#check-payment-info").hide("slow");
            }
        });
        $("#btnCheckout").click(function(e) {
            var userId = $("#hid_current_login_id").val();
            var name = $("#billing-name").val();
            var phone = $("#billing-phone").val();
            var address = $("#billing-street").val();
            var note = $("#order-note").val();
            var email = $("#billing-email").val();
            var paymentChecked = $("#payment-gateway").is(":checked");
            if (paymentChecked) {
                if (userId !== "") {
                    $.ajax({
                        type: "GET",
                        url: "/Cart/CheckoutForPayment",
                        success: function (response) {
                            if (response.code === '00') {
                                if (window.vnpay) {
                                    vnpay.open({ width: 480, height: 600, url: response.data });
                                } else {
                                    location.href = response.data;
                                }
                              
                                return false;
                            }
                            return false;
                        }
                    });
                } else {
                    window.location.href = "/login.html";
                }

            }
            else {
                if (phone.length < 1) {
                    $("#billing-phone-message").show();
                } else if (name.length < 1) {
                    $("#billing-name-message").show();
                } else if (address.length < 1) {
                    $("#billing-address-message").show();
                } else if (email.length < 1) {
                    $("#billing-email-message").show();
                }
                else {
                    $("#billing-phone-message").hide();
                    $("#billing-name-message").hide();
                    $("#billing-address-message").hide();
                    $.ajax({
                        type: "POST",
                        url: "/Cart/CheckoutForDelivery",
                        data: {
                            Name: name,
                            PhoneNumber: phone,
                            Address: address,
                            Message: note,
                            PaymentMethod: 0,
                            Email: email
                        },
                        success: function (response) {
                            //connection.invoke("SendMessageToGroup", "Admin", response).catch(function (err) {
                            //    return console.error(err.toString());
                            //});
                            var s = "Thanh toán thành công";
                            window.location.href = "/checkout-confirm.html?message=" + s;
                        },
                        error: function(response) {
                            var s = "Có lỗi xảy ra trong quá trình xử lý";
                            window.location.href = "/checkout-confirm.html?message=" + s;
                        } 
                    });
                }
            }
        });
    }

    function loadPaymentInformation() {
        if ($("#cash-on-delivery").checked) {
            $("#check-payment-info").show();
        } else {
            $("#check-payment-info").hide();
        }
    }
}