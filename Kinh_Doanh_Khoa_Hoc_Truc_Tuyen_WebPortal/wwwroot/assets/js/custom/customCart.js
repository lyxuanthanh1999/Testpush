var check = 0;
var CartAjax = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        $("body").on("click", ".a-delete", function (e) {
            e.preventDefault();
            var id = $(this).data("id");
            $.ajax({
                url: "/Cart/RemoveFromCart",
                type: "post",
                data: {
                    courseId: id
                },
                success: function () {
                    check--;
                    loadHeaderCart();
                    loadData();
                    if (check === 0) {
                        location.reload();
                    }
                }
            });
        });
        $("#aClearAll").on("click", function (e) {
            e.preventDefault();
            $.ajax({
                url: "/Cart/ClearCart",
                type: "post",
                success: function () {
                    check = 0;
                    loadHeaderCart();
                    loadData();
                    location.reload();
                }
            });
        });
    }
    function loadHeaderCart() {
        $("#headerCart").load("/Home/RefreshCart");
    }

    function loadData() {
        $.ajax({
            url: "/Cart/GetCart",
            type: "GET",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var template = $("#template-cart").html();
                var render = "";
                var totalAmount = 0;
                var valueZero = 0;
                var env = "https://localhost:44342";
                $.each(response, function (i, item) {
                    render += Mustache.render(template,
                        {
                            courseId: item.courseViewModel.id,
                            courseImg: env + item.courseViewModel.image,
                            urlCourseId: "/courses-" + item.courseViewModel.id + ".html",
                            courseName: item.courseViewModel.name,
                            urlCategoryId: "/courses.html?categoryId=" + item.courseViewModel.categoryId,
                            categoryName: item.courseViewModel.categoryName,
                            ownerUser: item.courseViewModel.createdName,
                            coursePrice: item.price.toLocaleString("vi", { style: "currency", currency: "VND" }),
                            coursePromotionPrice: !item.promotionPrice ? valueZero.toLocaleString("vi", { style: "currency", currency: "VND" }) : item.promotionPrice.toLocaleString("vi", { style: "currency", currency: "VND" })
                        });
                    check++;
                    totalAmount += !item.promotionPrice ? item.price : item.promotionPrice;
                });
                $("#totalAmountCart").text(totalAmount.toLocaleString("vi", { style: "currency", currency: "VND" }));
                if (render !== "") {
                    $("#table-cart-content").html(render);
                }

            }
        });
    }
}