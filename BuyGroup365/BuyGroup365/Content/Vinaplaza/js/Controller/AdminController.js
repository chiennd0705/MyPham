var common = {
    init: function () {
        common.registerEvent();
    },
    registerEvent: function () {
        $("#NewAbout").autocomplete({
            minLength: 2,
            source: function (request, response) {
                $.ajax({
                    url: "/Manage/Json/GetNewsname",
                    dataType: "json",
                    type: 'GET',
                    data: {
                        term: request.term
                    },
                    success: function (data) {
                        response(data);
                    }
                });
            },
            focus: function (event, ui) {
                $("#NewAbout").val(ui.item.Title);
                return false;
            },
            select: function (event, ui) {
                $("#NewAbout").val(ui.item.Title);
                $("#AboutNewID").val(ui.item.Id);
                //$("#project-description").html(ui.item.desc);
                return false;
            }
        })
            .autocomplete("instance")._renderItem = function (ul, item) {
                return $("<li>")
                    .append("<div class=\"img_s\"><div class=\"content_s\"><h3>" + item.Title + "</h3></div>")
                    .appendTo(ul);
            };

        $("#NewAboutHD").autocomplete({
            minLength: 2,
            source: function (request, response) {
                $.ajax({
                    url: "/Manage/Json/GetNewsname",
                    dataType: "json",
                    type: 'GET',
                    data: {
                        term: request.term
                    },
                    success: function (data) {
                        response(data);
                    }
                });
            },
            focus: function (event, ui) {
                $("#NewAboutHD").val(ui.item.Title);
                return false;
            },
            select: function (event, ui) {
                $("#NewAboutHD").val(ui.item.Title);
                $("#AboutNewIDHD").val(ui.item.Id);
                //$("#project-description").html(ui.item.desc);
                return false;
            }
        })
            .autocomplete("instance")._renderItem = function (ul, item) {
                return $("<li>")
                    .append("<div class=\"img_s\"><div class=\"content_s\"><h3>" + item.Title + "</h3></div>")
                    .appendTo(ul);
            };

        $("#RelativesProduct").autocomplete({
            minLength: 2,
            source: function (request, response) {
                $.ajax({
                    url: "/Manage/Json/GetProductsname",
                    dataType: "json",
                    type: 'GET',
                    data: {
                        term: request.term
                    },
                    success: function (data) {
                        response(data);
                    }
                });
            },
            focus: function (event, ui) {
                $("#RelativesProduct").val(ui.item.ProductName);
                return false;
            },
            select: function (event, ui) {
                $("#RelativesProduct").val(ui.item.ProductName);
                $("#RelativesProductID").val(ui.item.Id);

                return false;
            }
        })
            .autocomplete("instance")._renderItem = function (ul, item) {
                return $("<li>")
                    .append("<div class=\"img_s\"><div class=\"content_s\"><h3>" + item.ProductName + "</h3></div>")
                    .appendTo(ul);
            };
    }
}
common.init();