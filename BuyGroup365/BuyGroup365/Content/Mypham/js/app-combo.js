(function($){
	comboApp = {
		checkCombo: false,
		init: function(idProduct, idTitle){
			var self = this;
			if(typeof idProduct != 'undefined' && idProduct != '' && idProduct != null ){
				self.comboCall('GET', 'https://combo.haravan.com/js/list_recommendeds?product_id=' + idProduct, {}, function(data){
					// success
					if(data.length > 0){
						self.checkCombo = true;
						self.comboRender(data, idProduct, idTitle);
					}
				}, function(){
					//error
				}, function(){
					//always
				});
			}
		},
		comboCall: function(method, path, data, successcallback, errorcallback, alwayscallback){
			var xhr = new XMLHttpRequest();
			xhr.open(method, path);
			xhr.setRequestHeader('Content-Type', 'application/json; charset=UTF-8');
			xhr.onload = function () {
				if (xhr.readyState === 4 && xhr.status === 200) {
					if (Object.prototype.toString.call(successcallback) === '[object Function]') successcallback(JSON.parse(xhr.responseText));
				}
				else {
					if (Object.prototype.toString.call(errorcallback) === '[object Function]') errorcallback();
				}
			};
			xhr.onloadend = function () {
				if (Object.prototype.toString.call(alwayscallback) === '[object Function]') alwayscallback();
			};
			xhr.send(data);
		},
		comboRender: function(result, id, title){
			var giftHtml = '';
			$.each(result, function(key, value){
				console.log(result);
				var noti = '';
				var itemCombo = '';
				$.each(value.recommendeds, function(i,v){
					noti += '<span> <b>'+v.quantity +'</b></span><span> '+ (v.product_name).split('|')[0]+'</span> +';
				});
				var total_price_combo = 0;
				$.each(value.recommendeds, function(i,v){
					var typeHtml = '';
					var xprice = 0;
					var imm = "https://hstatic.net/0/0/global/noDefaultImage6_large.gif";
					var variant = null;
					var additem = '';
					var pricetotal = '';

					var pid = null;
					$.ajax({
						type: 'GET',
						url: v.product_url+'.js',
						dataType: 'json',
						async: false,
						success:function(response){
							xprice = parseInt(response.price)/100;
							imm = (response.featured_image != null)?Haravan.resizeImage(response.featured_image, 'small'):'https://hstatic.net/0/0/global/noDefaultImage6_large.gif';
							variant = response.variants[0].id;
							pid = response.id;
						}
					});
					if(v.type === 1 ){
						var fixedPrice = xprice - v.promotion_value;
					}else{
						var fixedPrice = (xprice*(100-v.promotion_value))/100;
					}

					total_price_combo = total_price_combo + fixedPrice;
					typeHtml += "<p><b>"+ (v.product_name).split('|')[0] + "</b></p><p class='discount-promotion-price'><b>"+(fixedPrice).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + "₫"+"</b><del>"+(xprice).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + "₫"+"</del></p>";
					var html = '<tr haravan-promotion><td><label class="combo-product" data-pid="'+pid+'" data-countbuy="'+v.quantity+'" data-id="'+variant+'"><img class="combo-img" src="'+imm+'"><p>'+typeHtml+'</p></label></td></tr>';
					pricetotal += "<b>"+(total_price_combo).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + "₫"+"</b>";
					additem += "<span> Thêm tất cả vào giỏ hàng</span>";

					console.log(itemCombo);


					$('.price-total').html(pricetotal);
					$('#discount-promotion-combo-add-btn').html(additem);
					itemCombo += html;
				});
				giftHtml += "<div class='combo-w "+((key==0)?'checked':'')+"'>"+"<div class='combo-list'><table>"+itemCombo+"</table></div></div>";
			});
			$('#combo-popup #combo-program').html(giftHtml);
			if(giftHtml != ""){
				$("#combo-popup").addClass("introoo"); 
				console.log(giftHtml);
			}else{
				("#combo-popup").removeClass("introoo");
			}
		},
		comboAddCart: function(pid, vid, callback){
			var self = this;
			var data = [];
			$('#combo-program .combo-w.checked .combo-product').each(function(k, e){
				if(parseInt($(this).attr('data-pid')) == pid){
					data[k] = {"vid": vid, "quan": parseInt($(this).attr("data-countbuy"))};
				}else{
					data[k] = {"vid": $(this).attr('data-id'), "quan": parseInt($(this).attr("data-countbuy"))};
				}
			});
			var success = 0;
			$.each(data, function(index, value){
				var params = {quantity:value.quan,id:value.vid};
				jQuery.ajax({
					type: 'POST',
					url: '/cart/add.js',
					data: params,
					dataType: 'json',
					async: false,
					success: function(cart) {
						success = index;
					},
					error: function(XMLHttpRequest, textStatus) {
					}
				}); 
			});
			if((success+1) == data.length){
				callback(true);
			}else{
				callback(false);
			}
		}
	}
	$(document).ready(function (){
		var idProduct = $('#combo-program').data('id');
		var idTitle = $('#combo-program').data('title');
		comboApp.init(idProduct, idTitle);

		$('body').on('click', '.combo-w', function(e){
			$('.combo-w').removeClass('checked');
			$(this).addClass('checked');
		})
	})
})(jQuery)