
// Biến khởi tạo
var view_collection = true;
var viewout = true;
var timeOut_modalCart;
var check_show_modal = true;
var timeOut_tabIndex;
var check_show_tabIndex = true;
var cur_scrollTop = 0;
if ( typeof(formatMoney) == 'undefined' ){
	formatMoney = '';
}
var check_header_fixTop = false;


// Modal Cart
function getCartModal(){
	var cart = null;
	jQuery('#cartform').hide();
	jQuery('#myCart #exampleModalLabel').text("Giỏ hàng");
	jQuery.getJSON('/cart.js', function(cart, textStatus) {
		if(cart)
		{
			jQuery('#cartform').show();
			jQuery('.line-item:not(.original)').remove();
			jQuery.each(cart.items,function(i,item){
				var total_line = 0;
				var total_line = item.quantity * item.price;
				tr = jQuery('.original').clone().removeClass('original').appendTo('table#cart-table tbody');
				if(item.image != null)
					tr.find('.item-image').html("<img src=" + Haravan.resizeImage(item.image,'small') + ">");
				else
					tr.find('.item-image').html("<img src='//hstatic.net/0/0/global/noDefaultImage6_large.gif'>");
				vt = item.variant_options;
				if(vt.indexOf('Default Title') != -1)
					vt = '';
				tr.find('.item-title').children('a').html(item.product_title + '<br>').attr('href', item.url);
				tr.find('.item-prices').html(Haravan.formatMoney(item.price, formatMoney));

				tr.find('.item-quantity').html("<input id='quantity1' name='updates[]' min='1' type='number' value=" + item.quantity + " class='' />");
				if ( typeof(formatMoney) != 'undefined' ){
					tr.find('.item-price').html(Haravan.formatMoney(total_line, formatMoney));
				}else {
					tr.find('.item-price').html(Haravan.formatMoney(total_line, ''));
				}
				tr.find('.item-delete').html("<a href='javascript:void(0);' onclick='deleteCart(" + item.variant_id + ")' ><i class='fa fa-trash-o'></i> Bỏ sản phẩm</a>");
			});
			jQuery('.item-total').html(Haravan.formatMoney(cart.total_price, formatMoney));
			jQuery('.modal-title').children('b').html(cart.item_count);
			jQuery('.cart-number').html(cart.item_count + ' sản phẩm');
			if(cart.item_count == 0){				
				jQuery('#exampleModalLabel').html('Giỏ hàng của bạn đang trống. Mời bạn tiếp tục mua hàng.');
				jQuery('#cart-view').html('<tr><td>Hiện chưa có sản phẩm</td></tr>');
				jQuery('#cartform').hide();
			}
			else{			
				jQuery('#exampleModalLabel').html('Giỏ hàng của bạn ' + '(' + cart.item_count + ' sản phẩm' + ')');
				jQuery('#cartform').removeClass('hidden');
				jQuery('#cart-view').html('');
			}
			if ( jQuery('#cart-pos-product').length > 0 ) {
				jQuery('#cart-pos-product span').html(cart.item_count + ' sản phẩm');
			}
			// Get product for cart view

			jQuery.each(cart.items,function(i,item){
				clone_item(item);
			});
			jQuery('#total-view-cart').html(Haravan.formatMoney(cart.total_price, formatMoney));
		}
		else{
			jQuery('#exampleModalLabel').html('Giỏ hàng của bạn đang trống. Mời bạn tiếp tục mua hàng.');
			if ( jQuery('#cart-pos-product').length > 0 ) {
				jQuery('#cart-pos-product span').html(cart.item_count + ' sản phẩm');
			}
			jQuery('#cart-view').html('<tr><td>Hiện chưa có sản phẩm</td></tr>');
			jQuery('#cartform').hide();
		}
	});
}

function clone_item(product){
	var item_product = jQuery('#clone-item-cart').find('.item_2');
	item_product.find('img').attr('src',Haravan.resizeImage(product.image,'small')).attr('alt', product.url);
	item_product.find('a').attr('href', product.url).attr('title', product.url);
	item_product.find('.pro-title-view').html(product.title);
	item_product.find('.pro-quantity-view').html('Số lượng: ' + product.quantity);
	item_product.find('.pro-price-view').html('Giá: ' + Haravan.formatMoney(product.price,formatMoney));
	item_product.clone().removeClass('hidden').prependTo('#cart-view');
}

// Delete variant in modalCart
function deleteCart(variant_id){
	var params = {
		type: 'POST',
		url: '/cart/change.js',
		data: 'quantity=0&id=' + variant_id,
		dataType: 'json',
		success: function(cart) {
			getCartModal();
		},
		error: function(XMLHttpRequest, textStatus) {
			Haravan.onError(XMLHttpRequest, textStatus);
		}
	};
	jQuery.ajax(params);
}

// Update product in modalCart
jQuery(document).on("click","#update-cart-modal",function(event){
	event.preventDefault();
	if (jQuery('#cartform').serialize().length <= 5) return;
	jQuery(this).html('Đang cập nhật');
	var params = {
		type: 'POST',
		url: '/cart/update.js',
		data: jQuery('#cartform').serialize(),
		dataType: 'json',
		success: function(cart) {
			if ((typeof callback) === 'function') {
				callback(cart);
			} else {
				getCartModal();
			}
			jQuery('#update-cart-modal').html('Cập nhật');
		},
		error: function(XMLHttpRequest, textStatus) {
			Haravan.onError(XMLHttpRequest, textStatus);
		}
	};
	jQuery.ajax(params);
});


// Add a product in checkout
var buy_now = function(id) {
	var quantity = 1;
	var params = {
		type: 'POST',
		url: '/cart/add.js',
		data: 'quantity=' + quantity + '&id=' + id,
		dataType: 'json',
		success: function(line_item) {
			window.location = '/checkout';
		},
		error: function(XMLHttpRequest, textStatus) {
			Haravan.onError(XMLHttpRequest, textStatus);
		}
	};
	jQuery.ajax(params);
}

// Add a product in cart
var add_item = function(id) {
	var quantity = 1;
	var params = {
		type: 'POST',
		url: '/cart/add.js',
		data: 'quantity=' + quantity + '&id=' + id,
		dataType: 'json',
		success: function(line_item) {
			window.location = '/cart';
		},
		error: function(XMLHttpRequest, textStatus) {
			Haravan.onError(XMLHttpRequest, textStatus);
		}
	};
	jQuery.ajax(params);
}

// Add a product and show modal cart
var add_item_show_modalCart = function(id) {

	if( check_show_modal ) {
		check_show_modal = false;
		timeOut_modalCart = setTimeout(function(){ 
			check_show_modal = true;
		}, 3000);
		if ( $('.addtocart-modal').hasClass('clicked_buy') ) {
			var quantity = $('#qty').val();
		} else {
			var quantity = 1;
		}
		var params = {
			type: 'POST',
			url: '/cart/add.js',
			async: true,
			data: 'quantity=' + quantity + '&id=' + id,
			dataType: 'json',
			success: function(line_item) {

				if ( jQuery(window).width() >= 768 ) {
					getCartModal();					
					jQuery('#myCart').modal('show');				
					jQuery('.modal-backdrop').css({'height':jQuery(document).height(),'z-index':'99'});
				} else {
					window.location = '/cart';
				}
				$('.addtocart-modal').removeClass('clicked_buy');
			},
			error: function(XMLHttpRequest, textStatus) {
				Haravan.onError(XMLHttpRequest, textStatus);
			}
		};
		jQuery.ajax(params);
	}
}

/* Add to cart, Buy now*/
$(document).on("click",".buy-now", function(e){
	e.preventDefault() ;
	var params = {
		type: 'POST',
		url: '/cart/add.js',
		async : false,
		data: {quantity:1,id:$(this).data('id')},
		dataType: 'json',
		success: function(line_item) {
			window.location = '/checkout';
		},
		error: function(XMLHttpRequest, textStatus) {
			Haravan.onError(XMLHttpRequest, textStatus);
		}
	};
	jQuery.ajax(params);
});


Haravan.onSuccess = function(data, form_id) {
	addToCartPopup(data);
	//update top cart: qty, total price
	var $product_page = $(form_id).parents('[class^="product-page"]'); 
	var quantity = parseInt($product_page.find('[name="quantity"]').val(), 10) || 1;
	var $item_qty_new = 0; 
	var $item_price_new = 0; 
	var $item_price_increase = 0; 
	var $boUpdated = false; 

	//insert "no_item" html  
	if($('.top-cart-block .top-cart-content .top-cart-item').size() <= 0) 
	{
		$('.top-cart-block .top-cart-content').html(top_cart_no_item);  
	} 
	//update items 
	$('.top-cart-block .top-cart-content .top-cart-item').each(function(){	
		if($(this).find('.item_id').val() == $product_page.find('[name="id"]').val() ){
			$item_qty_new = parseInt($(this).find('.item_qty').val()) + quantity ;
			$item_price_single = parseFloat($(this).find('.item_unit_price_not_formated').val());
			$item_price_new = $item_qty_new * $item_price_single;   

			$item_price_increase = quantity * parseFloat($(this).find('.item_unit_price_not_formated').val());   
			$(this).find('.item_qty').val($item_qty_new);  // !!!
			$(this).find('.top-cart-item-quantity').html('x ' + $item_qty_new); 
			$(this).find('.top-cart-item-price').html(Haravan.formatMoney($item_price_new, "{{ shop.money_with_currency_format }}") + 'đ');  // '{{ shop.currency }}'  shop.money_format
			$boUpdated = true; // updated item 
		} 
	});

	if($boUpdated == false){ // current item is not existed!!!  
		var $proURL = $product_page.find('.product_url').val();
		var $proTitle = $product_page.find('.product_title_hd').val();
		var $proUnitPrice = parseFloat($product_page.find('.unit_price_not_formated').val());
		var $strNewItem = '<div class="top-cart-item clearfix">'
		+ ' <input type="hidden" class="item_id" value="'+ $product_page.find('[name="id"]').val() +'"></input>'  
		+ ' <input type="hidden" class="item_qty" value="'+ quantity +'"></input>' 
		+ ' <input type="hidden" class="item_unit_price_not_formated" value="'+ $proUnitPrice +'"></input>' 

		+ '<div class="top-cart-item-image">'
		+ ' <a href="'+ $proURL +'"><img src="'+ $product_page.find('.product_img_small').val() +'" alt="'+ $proTitle +'" ></a>'
		+ '</div>'
		+ '<div class="top-cart-item-desc">'
		//+ ' <span class="cart-content-count">x'+ quantity +'</span>'
		+ '<a href="'+ $proURL +'">' + $proTitle + '</a>'
		+ '<span class="top-cart-item-price">'+ Haravan.formatMoney($proUnitPrice * quantity, "{{ shop.money_with_currency_format }}") + 'đ' +'</span>' 
		+ '<span class="top-cart-item-quantity">x '+ quantity +'</span>'
		+'<a class="top_cart_item_remove" onclick = "deleteCart('+ $product_page.find('[name="id"]').val() +');"><i class="fa fa-times-circle"></i></a>'
		+ ' </div>'
		+ '</div>';
		$('.top-cart-block .top-cart-content .top-cart-items').append($strNewItem); 
		$item_price_increase = $proUnitPrice * quantity; 

	}  
	//check is emptiness...   
	check_topcart_empty();  

	//update total 
	var $quantity_new = parseInt($('.top-cart-block #top-cart-trigger span').text()) + quantity;  
	var $price_new = parseFloat($('.top-cart-block .top_cart_total_price_not_format').val()) + $item_price_increase;  
	$('.top-cart-block .top_cart_total_price_not_format').val($price_new);  // !!!
	// top cart total quantity
	$('.top-cart-block #top-cart-trigger span').html($quantity_new); 
	// scroll cart total quantity
	$('.scroll_cart span').html($quantity_new);

};
$("#top-cart-trigger").click(function(){
	getCartModal();	
	jQuery('#myCart').modal('show');				
	jQuery('.modal-backdrop').css({'height':jQuery(document).height(),'z-index':'99'});
});

$(".bnt-freeship").click(function(){
	jQuery('#myCartship').modal('show');	
	jQuery('.modal-backdrop').css({'height':jQuery(document).height(),'z-index':'999'});

});













