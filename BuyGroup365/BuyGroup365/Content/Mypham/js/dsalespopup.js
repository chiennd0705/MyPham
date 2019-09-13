var dsalespopup_API = 'https://salespopup.doke.app',
		dsalespopup_SHOPNAME = Haravan.shop.replace('.myharavan.com',''),
		dsalespopup_CSS = '<link href="https://cdn.doke.app/doke-sales-popup/dsalespopup.css" rel="stylesheet" type="text/css" >';
function dsalesPopup(p) {

	this.delay_time = p.delay_time, this.display_time = p.display_time, this.limit_per_page = p.limit_per_page;
	var e, t, a = this,
			o = 0;
	this.sales_dom = $(".dwrapper__popup"), this.load_data = function(p) {
		var url = dsalespopup_API + '/data/' + dsalespopup_SHOPNAME;
		jQuery.ajax({
			url: url,
			method: "GET",
			context: this
		}).done(function(e) {
			"function" == typeof p && p(e)
		})
	}, this.dsales_Loop = function() {
		var p = this;
		t.notifications.length > 0 && o < this.limit_per_page && (o++, p.renderNotify(t, function() {
			$(".dwrapper__popup").addClass("dwrapper__popup--active"), e = setTimeout(function() {
				$(".dwrapper__popup").removeClass("dwrapper__popup--active"), setTimeout(function() {
					p.dsales_Loop()
				}, p.delay_time)
			}, p.display_time)

			//Get Count View
			//console.log('get count view');
			var url = dsalespopup_API + '/statistic/' + dsalespopup_SHOPNAME + '/view';
			var body = {
				"path": window.location.href,
				"popup_product_id": $('.dwrapper__popup .dpopup__body a').attr('product_id')
			}
			jQuery.ajax({
				url: url,
				method: "POST",
				context: this,
				data: body,
			}).done(function(p) {
				
			})
		}))
	}, this.timeConvert = function(p) {
		var e = p / 60,
				t = Math.floor(e),
				a = 60 * (e - t);
		return t + " giờ " + Math.round(a) + " phút trước."
	}, this.renderNotify = function(p, e) {
		var t = p.format,
				a = p.notifications,
				o = a[Math.floor(Math.random() * a.length)],
				r = this.timeConvert(Math.floor(1e3 * Math.random())),
				i = this.sales_dom.find(".dpopup__title"),
				d = this.sales_dom.find(".dpopup__product-image"),
				s = this.sales_dom.find(".dpopup__body"),
				n = this.sales_dom.find(".dpopup__save"),
				_ = this.sales_dom.find(".dpopup__time"),
				u = {
					"{{ họ_tên }}": o.customer.first_name + ' ' + o.customer.last_name,
					"{{ họ_tênđệm }}": o.customer.first_name,
					"{{ tên }}": o.customer.last_name,
					"{{ tỉnh_thànhphố }}": o.location.province,
					"{{ quận_huyện }}": o.location.district
				};
		t = t.replace(/{{ họ_tên }}|{{ họ_tênđệm }}|{{ tên }}|{{ quận_huyện }}|{{ tỉnh_thànhphố }}/g, function(p) {
			return u[p]
		}), i.text(t), d.find("img").attr("src", o.product.image.replace("http:", "").replace(".jpg", "_small.jpg").replace(".png", "_small.png")), null != o.product.handle ? s.find("a").attr("href", "/products/" + o.product.handle).attr('product_id', o.product.product_id).text(o.product.title) : s.find("a").attr("href", "javascript:;").text(o.product.title);
		var l = o.product.variant[0].price,
				m = o.product.variant[0].compare_at_price;
		if ((m > 0 || "" != m || null != m) && m > l) {
			var g = m - l;
			n.html("Tiết kiệm " + g.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,').replace(".00", "") + "đ <span>trong đơn hàng này</span>")
		} else n.html("");
		_.text(r), $(".dpowered_by").css("cssText", "display: block !important"), "function" == typeof e && e()
	}, $(document).on("click", ".dwrapper__popup .dpopup__body a", function(e) {
		e.preventDefault();
		//console.log($(this).attr('href'));
		//Get Count Click
		//console.log('get count click')
		var url = dsalespopup_API + '/statistic/' + dsalespopup_SHOPNAME + '/click';
		var body = {
			"path": window.location.href,
			"popup_product_id": $(this).attr('product_id')
		}
		jQuery.ajax({
			url: url,
			method: "POST",
			data: body,
			context: this
		}).done(function(p) {
			window.location.href = $(this).attr('href');
		})
	}), 
		$(document).on("click", ".dclose__popup", function() {
		$(".dwrapper__popup").removeClass("dwrapper__popup--active")
	}), $(document).on("mouseenter", ".dwrapper__popup", function() {
		clearTimeout(e)
	}).on("mouseleave", ".dwrapper__popup", function() {
		e = setTimeout(function() {
			$(".dwrapper__popup").removeClass("dwrapper__popup--active"), setTimeout(function() {
				a.dsales_Loop()
			}, a.delay_time)
		}, a.display_time)
	}), "undefined" != typeof Storage && null != sessionStorage.getItem("dsales_data") ? (t = JSON.parse(sessionStorage.getItem("dsales_data")), e = setTimeout(function() {
		a.dsales_Loop()
	}, a.delay_time)) : this.load_data(function(p) {
		(t = p).error || (e = setTimeout(function() {
			a.dsales_Loop()
		}, a.delay_time), sessionStorage.setItem("dsales_data", JSON.stringify(p)))
	})
}
window.addEventListener("load", function() {
	var url = dsalespopup_API + '/template/' + dsalespopup_SHOPNAME;
	jQuery.ajax({
		url: url,
		method: "GET",
		context: this
	}).done(function(p) {
		$('head').append(dsalespopup_CSS);
		jQuery(document.body).append(p)
	})
});