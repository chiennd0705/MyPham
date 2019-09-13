var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {
        $('#btnContinue').off('click').on('click', function () {
            //window.location.href = "/";
            //alert("Tiếp tục mua hàng");
            $('html, body').animate({
                scrollTop: $("#scrollHere").offset().top
            }, 2000);
        });
        $('#btnDeleteAll').off('click').on('click', function () {
            window.location.href = "/PayGoods/Payment";
            window.alert("Xóa hàng trong giỏ");
        });
        $('#btnUpdate').off('click').on('click', function () {
            var listProduct = $('.txtQuantity');
            var cartList = [];
            $.each(listProduct, function (i, item) {
                cartList.push({
                    Quantity: $(item).val(),
                    Product: {
                        ID: $(item).data('id')
                    }
                });
            });

            $.ajax({
                url: '/PayGoods/Update',
                data: { cartModel: JSON.stringify(cartList) },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        //window.location.href = "/gio-hang";
                        window.location.href = "/PayGoods/Payment";
                    }
                }
            })
        });

        $('#btnDeleteAll').off('click').on('click', function () {
            $.ajax({
                url: '/PayGoods/DeleteAll',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/PayGoods/Payment";
                    }
                }
            })
        });

        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                data: { id: $(this).data('id') },
                url: '/PayGoods/Delete',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/PayGoods/Payment";
                    }
                }
            })
        });
    }
}

cart.init();