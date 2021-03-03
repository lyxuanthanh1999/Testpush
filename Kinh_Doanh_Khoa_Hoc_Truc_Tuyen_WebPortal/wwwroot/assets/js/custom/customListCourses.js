var ListCoursesAjax = function() {
    this.initialize = function() {
        registerEvents();
    }

    function registerEvents() {
        $("body").on("click", "#sortPrice", function(e) {
            e.preventDefault();
            var url = window.location.href;
            var min = $("#price-min").val();
            var max = $("#price-max").val();
            var pageSize = $("#ddlPageSize").val();
            var sortType = $("#ddlSortType").val();
            var index = url.indexOf("categoryId=");
            var sortFilter = $("#searchFilter").text();
            if (index === -1) {
                if (sortFilter !== undefined) {
                    window.location.href = "/courses.html?priceMin=" + min + "&priceMax=" + max + "&pageSize=" + pageSize + "&sortBy=" + sortType + "&filter=" + sortFilter;
                } else {
                    window.location.href = "/courses.html?priceMin=" + min + "&priceMax=" + max + "&pageSize=" + pageSize + "&sortBy=" + sortType;
                }

            }  else {
                var tmpString = "";
                var endIndex = url.indexOf("&");
                if (endIndex === -1) {
                    tmpString = url.substring(index);
                    window.location.href = "/courses.html?" + tmpString + "&priceMin=" + min + "&priceMax=" + max + "&pageSize=" + pageSize + "&sortBy=" + sortType;
                } else {
                    tmpString = url.substring(index, endIndex);
                    window.location.href = "/courses.html?" + tmpString + "&priceMin=" + min + "&priceMax=" + max + "&pageSize=" + pageSize + "&sortBy=" + sortType;
                }
            }
        });
        $("body").on("change", ".selectWrap ", function(e) {
            e.preventDefault();
            var url = window.location.href;
            var min = $("#price-min").val();
            var max = $("#price-max").val();
            var pageSize = $("#ddlPageSize").val();
            var sortType = $("#ddlSortType").val();
            var sortFilter = $("#searchFilter").text();
            
            var index = url.indexOf("categoryId=");
            if (index === -1) {
                if (sortFilter !== undefined) {
                    window.location.href = "/courses.html?priceMin=" + min + "&priceMax=" + max + "&pageSize=" + pageSize + "&sortBy=" + sortType + "&filter=" + sortFilter;
                } else {
                    window.location.href = "/courses.html?priceMin=" + min + "&priceMax=" + max + "&pageSize=" + pageSize + "&sortBy=" + sortType;
                }
              
            } else {
                var tmpString = "";
                var endIndex = url.indexOf("&");
                if (endIndex === -1) {
                    tmpString = url.substring(index);
                    window.location.href = "/courses.html?" + tmpString + "&priceMin=" + min + "&priceMax=" + max + "&pageSize=" + pageSize + "&sortBy=" + sortType;
                } else {
                    tmpString = url.substring(index, endIndex);
                    window.location.href = "/courses.html?" + tmpString + "&priceMin=" + min + "&priceMax=" + max + "&pageSize=" + pageSize + "&sortBy=" + sortType;
                }
               
            }
        });
    }
}