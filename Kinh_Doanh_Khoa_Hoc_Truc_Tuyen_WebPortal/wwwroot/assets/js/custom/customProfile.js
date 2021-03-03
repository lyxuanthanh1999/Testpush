var ProfileJquery = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $("body").on("click", "#btnAvatar", function (e) {
            e.preventDefault();
            $("#fileAvatar").click();
        });
        $("body").on("change", "#fileAvatar", function (e) {
            e.preventDefault();
            $("#formFile").submit();
        });
    }
}