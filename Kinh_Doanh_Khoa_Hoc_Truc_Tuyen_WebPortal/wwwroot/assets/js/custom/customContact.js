var ContactJquery = function () {
    this.initialize = function () {
        registerEvent();
    }
    function registerEvent() {
        initMap();
    }

    function initMap() {
        var address = { lat: parseFloat(10.806137), lng: parseFloat(106.62884) };
        var map = new google.maps.Map(document.getElementById("map"), {
            zoom: 17,
            center: address
        });

        var infoWindow = new google.maps.InfoWindow({
            content: "140 Lê Trọng Tấn, Tây Thạnh, Tân Phú, Thành phố Hồ Chí Minh"
        });

        var marker = new google.maps.Marker({
            position: address,
            map: map,
            title: "KHOL Shop"
        });
        infoWindow.open(map, marker);
    }
}