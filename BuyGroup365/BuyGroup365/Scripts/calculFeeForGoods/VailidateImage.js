$(".validateimage").change(function () {
    var fileExtension = ['jpeg', 'jpg', 'png', 'gif', 'bmp'];
    if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
        alert("Bạn chỉ được upload các file có đuôi : " + fileExtension.join(', '));
        $(".validateimage").val("");
        $(".validateimage").focus();
    }
});