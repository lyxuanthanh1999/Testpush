var BaseAjax = function () {
    this.initialize = function () {
        loadAnnouncement();
        registerEvents();
    }

    function registerEvents() {
        $("body").on("click", ".remove-product-in-cart", function (e) {
            e.preventDefault();
            var id = $(this).data("id");
            $.ajax({
                url: "/Cart/RemoveFromCart",
                type: "post",
                data: {
                    courseId: id
                },
                success: function () {
                    loadHeaderCart();
                    if (location.pathname === "/cart.html") {
                        location.reload(); 
                    }
                }
            });
        });

        $("body").on("click", ".add-product-in-cart", function (e) {
            e.preventDefault();
            var status = parseInt($(this).data("status"));
            if (status === 1) {
                var id = parseInt($(this).data("id"));
                $.ajax({
                    url: "/Cart/AddToCart",
                    type: "post",
                    data: {
                        courseId: id
                    },
                    success: function (res) {
                        var template = $("#template-modal-cart").html();
                        var render = "";
                        var valueZero = 0;
                        var env = "https://localhost:44342";
                        render += Mustache.render(template,
                            {
                                checkPromotion: !res.promotionPrice ? false : true,
                                modalCartImage: env + res.courseViewModel.image,
                                modalCartName: res.courseViewModel.name,
                                modalOwnerUser: res.courseViewModel.createdName,
                                modalCartPrice: res.price.toLocaleString("vi", { style: "currency", currency: "VND" }),
                                modalCartPromotion: !res.promotionPrice ? valueZero.toLocaleString("vi", { style: "currency", currency: "VND" }) : res.promotionPrice.toLocaleString("vi", { style: "currency", currency: "VND" })
                            });
                        $("#modal-add-to-cart").html(render);

                        loadHeaderCart();
                    },
                    error: function (res) {
                        $("#modal-add-to-cart").html('<div class="description-title" style="text-align: justify;font-size: 16px;font-weight: 600">Sản phẩm này đã có trong giỏ hàng</div>');
                        return false;
                    }
                });
            }
            else if (status === 2) {
                $("#modal-add-to-cart").html('<div class="description-title" style="text-align: justify;font-size: 16px;font-weight: 600">Khóa học này chưa phát hành</div>');
                return false;
            } 
            else
            {
                $("#modal-add-to-cart").html('<div class="description-title" style="text-align: justify;font-size: 16px;font-weight: 600">Khóa học này đã ngừng kình doanh</div>');
                return false;
            }
            //var auth = $("#hid_check_auth").val();
            //if (auth) {
            //    var id = parseInt($(this).data("id"));
            //    $.ajax({
            //        url: "/Cart/AddToCart",
            //        type: "post",
            //        data: {
            //            courseId: id
            //        },
            //        success: function (res) {
            //            var template = $("#template-modal-cart").html();
            //            var render = "";
            //            var valueZero = 0;
            //            var env = "https://localhost:44342";
            //            render += Mustache.render(template,
            //                {
            //                    checkPromotion: !res.promotionPrice ? false : true,
            //                    modalCartImage: env + res.courseViewModel.image,
            //                    modalCartName: res.courseViewModel.name,
            //                    modalOwnerUser: res.courseViewModel.createdName,
            //                    modalCartPrice: res.price.toLocaleString("vi", { style: "currency", currency: "VND" }),
            //                    modalCartPromotion: !res.promotionPrice ? valueZero.toLocaleString("vi", { style: "currency", currency: "VND" }) : res.promotionPrice.toLocaleString("vi", { style: "currency", currency: "VND" })
            //                });
            //            $("#modal-add-to-cart").html(render);

            //            loadHeaderCart();
            //        }
            //    });
            //} else {
            //    window.location.href = "/login.html";
            //}
        
        });

        $("#main-search").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Courses/GetCoursesByFilter",
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: {
                        filter: request.term
                    },
                    success: function (res) {
                        response(res);
                    }
                });
            },
            focus: function (event, ui) {
                $("#main-search").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#main-search").val(ui.item.label);
                return false;
            }
        });

        $("body").on("click", ".checkRead", function (e) {
            e.preventDefault();
            var announceId = $(this).data("id");
            $.ajax({
                type: "POST",
                url: "/Account/MarkAsRead",
                data: {
                    announceId: announceId
                },
                dataType: "json",
                success: function (response) {
                    loadAnnouncement();
                },
                error: function (status) {
                    console.log("Có lỗi xảy ra");
                }
            });
        });
    }


    function loadHeaderCart() {
        $("#headerCart").load("/Home/RefreshCart");
    }
    function loadAnnouncement() {
        $.ajax({
            url: "/Account/GetAnnouncements",
            type: "GET",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var template = $("#announcement-template").html();
                var render = "";
                var url = "https://localhost:44342";
                console.log(response.items);
                if (response.totalRecords > 0) {
                    $.each(response.items, function (i, item) {
                        render += Mustache.render(template, {
                            Content: item.content,
                            Id: item.id,
                            Title: item.title,
                            Image: url + item.image,
                            CreationTime: moment(item.creationTime).fromNow()
                        });
                    });
                    render += $("#announcement-tag-template").html();
                    $("#totalAnnouncement").text(response.totalRecords);
                    if (render !== "") {
                        $("#announcementList").html(render);
                    }
                }
                else {
                    $("#totalAnnouncement").text(0);
                    render += '<li style="text-align: center; align-items: center;"><span class="description-title u-c-silver" style="text-align:center;align-items: center; line-height: 1.9; text-align: justify; font-size: 12px; font-weight: 600">Không có thông báo chưa đọc</span></li>';
                    render += $("#announcement-tag-template").html();
                    $("#announcementList").html(render);
                }
            },
            error: function (status) {
                console.log("Có lỗi xảy ra");
            }
        });
    };
}