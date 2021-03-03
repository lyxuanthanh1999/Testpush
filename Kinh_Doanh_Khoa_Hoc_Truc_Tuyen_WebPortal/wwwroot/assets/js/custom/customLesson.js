var LessonJquery = function () {
    this.initialize = function () {
        var lessonId = parseInt($("#hid_lesson_id").val());
        loadComments(lessonId);
        registerEvents();
    }

    function registerEvents() {
        $("#comment-form").submit(function (e) {
            e.preventDefault(); // avoid to execute the actual submit of the form.
            var form = $(this);
            var url = form.attr("action");
            $.post(url, form.serialize()).done(function (response) {

                var template = $("#tmpl_comments").html();
                var newComment = Mustache.render(template, {
                    content: response.content,
                    creationTime: moment(response.creationTime).fromNow(),
                    ownerUser: response.ownerUser
                });
                $("#reviewer-text").val("");
                $("#comment_list").prepend(newComment);

                var valueData = parseInt($("#hid_number_of_comments").val());
                var countNonLogin = valueData + 1;
                var numberOfCommentsNonLogin = countNonLogin + " Bình Luận";
                $("#number-comments-non-login").text(numberOfCommentsNonLogin);
                $("#number-comments").text(numberOfCommentsNonLogin);
                $("#span-count-comments").text("(" + countNonLogin + ")");
                $("#hid_number_of_comments").val(countNonLogin);
            });
        });

        //Binding reply comment event
        $("body").on("click", ".comment-reply-link", function (e) {
            e.preventDefault();
            if ($("#hid_current_login_id").val() === "") {
                window.location.href = "/login.html";
            } else {
                var commentId = $(this).data("commentid");
                var template = $("#tmpl_reply_comment").html();
                var htmlComment = Mustache.render(template, {
                    commentId: commentId
                });
                $("#reply_comment_" + commentId).append(htmlComment);
                // this is the id of the form
                $("#frm_reply_comment_" + commentId).submit(function (e) {
                    e.preventDefault(); // avoid to execute the actual submit of the form.
                    var form = $(this);
                    var url = form.attr("action");
                    $.post(url, form.serialize()).done(function (response) {
                        var template = $("#tmpl_children_comments").html();
                        var newComment = Mustache.render(template, {
                            id: response.id,
                            content: response.content,
                            creationTime: moment(response.creationTime).fromNow(),
                            ownerUser: response.ownerUser
                        });

                        //Reset reply comment
                        $("#txt_reply_content_" + commentId).val("");
                        $("#reply_comment_" + commentId).html("");

                        //Prepend new comment to children
                        $("#children_comments_" + commentId).prepend(newComment);

                        //Update number of comment
                        var valueData = parseInt($("#hid_number_of_comments").val());
                        var countNonLogin = valueData + 1;
                        var numberOfCommentsNonLogin = countNonLogin + " Bình Luận";
                        $("#number-comments-non-login").text(numberOfCommentsNonLogin);
                        $("#number-comments").text(numberOfCommentsNonLogin);
                        $("#span-count-comments").text("(" + countNonLogin + ")");
                        $("#hid_number_of_comments").val(countNonLogin);
                    });
                });
            }


        });

        $("body").on("click", "#comment-pagination", function (e) {
            e.preventDefault();
            var lessonId = parseInt($("#hid_lesson_id").val());
            var nextPageIndex = parseInt($(this).data("page-index")) + 1;
            $(this).data("page-index", nextPageIndex);
            loadComments(lessonId, nextPageIndex);
        });

        $("body").on("click", ".replied-comment-pagination", function (e) {
            e.preventDefault();
            var lessonId = parseInt($("#hid_lesson_id").val());
            var commentId = parseInt($(this).data("id"));
            var nextPageIndex = parseInt($(this).data("page-index")) + 1;
            $(this).data("page-index", nextPageIndex);
            loadRepliedComments(lessonId, commentId, nextPageIndex);
        });
    }

    function loadComments(id, pageIndex) {

        if (pageIndex === undefined) {
            pageIndex = 1;
        }
        $.get("/Courses/GetCommentById?id=" + id + "&pageIndex=" + pageIndex + "&entityType=lessons").done(function (response, statusText, xhr) {
            if (xhr.status === 200) {
                var template = $("#tmpl_comments").html();
                var childrenTemplate = $("#tmpl_children_comments").html();
                if (response && response.items) {
                    var html = "";
                    $.each(response.items, function (index, item) {
                        var childrenHtml = "";
                        if (item.children && item.children.items) {
                            $.each(item.children.items, function (childIndex, childItem) {
                                childrenHtml += Mustache.render(childrenTemplate, {
                                    content: childItem.content,
                                    creationTime: moment(childItem.creationTime).fromNow(),
                                    ownerUser: childItem.ownerUser,
                                    id: childItem.id
                                });
                            });

                        }
                        if (item.children.pageIndex < item.children.pageCount) {
                            childrenHtml += '<div class="dash__link dash__link--brand u-s-m-y-15"><a href="#" class="replied-comment-pagination" id="replied-comment-pagination-' + item.id + '" data-page-index="1" data-id="' + item.id + '">Xem thêm bình luận</a></div>';
                        }
                        else {
                            childrenHtml += '<div class="dash__link dash__link--brand u-s-m-y-15"><a href="#" class="replied-comment-pagination" id="replied-comment-pagination-' + item.id + '" data-page-index="1" data-id="' + item.id + '" style="display:none">Xem thêm bình luận</a></div>';
                        }
                        html += Mustache.render(template, {
                            childrenHtml: childrenHtml,
                            content: item.content,
                            creationTime: moment(item.creationTime).fromNow(),
                            ownerUser: item.ownerUser,
                            id: item.id
                        });
                    });
                    $("#comment_list").append(html);
                    if (response.pageIndex < response.pageCount) {
                        $("#comment-pagination").show();
                    }
                    else {
                        $("#comment-pagination").hide();
                    }

                }
            }
        });
    }

    function loadRepliedComments(id, rootCommentId, pageIndex) {
        if (pageIndex === undefined) pageIndex = 1;
        $.get("/Courses/GetRepliedCommentById?id=" + id + "&pageIndex=" + pageIndex + "&rootCommentId=" + rootCommentId + "&entityType=lessons").done(function (response, statusText, xhr) {
            if (xhr.status === 200) {
                var childrenTemplate = $("#tmpl_children_comments").html();
                if (response && response.items) {
                    var html = "";
                    $.each(response.items, function (index, item) {
                        html += Mustache.render(childrenTemplate, {
                            content: item.content,
                            creationTime: moment(item.creationTime).fromNow(),
                            ownerUser: item.ownerUser,
                            id: item.id
                        });
                    });
                    $("#children_comments_" + rootCommentId).prepend(html);
                    if (response.pageIndex < response.pageCount) {
                        $("#replied-comment-pagination-" + rootCommentId).show();
                    }
                    else {
                        $("#replied-comment-pagination-" + rootCommentId).hide();
                    }

                }
            }
        });
    }

}