var connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:44342/notification", {
        transport: signalR.HttpTransportType.LongPolling,
        accessTokenFactory: () => $("#accessToken").val()
    })
    .configureLogging(signalR.LogLevel.Information)
    .build();
connection.start().catch(err => console.error(err.toString()));

connection.on("ReceiveMessage", (message) => {
    //var template = $("#announcement-template").html();
    //var url = "https://localhost:44342";
    //var html = Mustache.render(template, {
    //    Content: message.content,
    //    Id: message.id,
    //    Title: message.title,
    //    Image: url + message.image,
    //    CreationTime: moment(message.creationTime).fromNow()
    //});
    //if (parseInt($("#totalAnnouncement").text()) === 0) {
    //    html += $("#announcement-tag-template").html();
    //}
    //$("#announcementList").prepend(html);
    //var totalAnnounce = parseInt($("#totalAnnouncement").text()) + 1;
    //$("#totalAnnouncement").text(totalAnnounce);
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
                $("#announcementList").html('<li><span class="manage-o__text-2 u-c-silver" style="line-height: 1.3; text-align: justify">Không có thông báo chưa đọc</span></li>');
            }
        },
        error: function (status) {
            console.log("Có lỗi xảy ra");
        }
    });
});