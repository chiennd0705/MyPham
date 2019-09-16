var common = {
    init: function () {
        common.registerEvent();
    },
   
        registerEvent: function () {
            $("#txtKeyword").autocomplete({
                minLength: 2,
                source: function (request, response) {
                    $.ajax({
                        url: "/Home/ListName",
                        dataType: "json",
                        data: {
                            term: request.term
                        },
                        success: function (data) {
                            response(data);
                        }
                    });
                },
                focus: function (event, ui) {
                    $("#txtKeyword").val(ui.item.ProductName);
                    return false;
                },
                select: function (event, ui) {
                    $("#txtKeyword").val(ui.item.ProductName);
                    //$("#project-description").html(ui.item.desc);
                    return false;
                }
            })
                .autocomplete("instance")._renderItem = function (ul, item) {
                    var im = item.ImgSource;
                    var p = item.Price.toLocaleString('vi', { style: 'currency', currency: 'VND' });;
                    return $("<li>")
                        .append("<div class=\"img_s\"><a href=\"/" + item.FriendlyUrl + "\"><img src=\"" + im + "\"></div><div class=\"content_s\"><h3>" + item.ProductNameShort + "</h3><span class=\"price\">Giá sản phẩm: " + p + "</span></a></div>")
                        .appendTo(ul);
                };
        }
  
}
common.init();