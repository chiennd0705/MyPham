
function log(message) {
	console.log(message);
}
if(typeof isCallWL == "undefined") {
	var isCallWL = false;
}
var aeConfigWL = {};
var aeClsCoverNameWL = "ae-cover-wishlist";
var btnTriggerWL = ".ae-btn-wishlist";
function getURLParameter(url, name) {
	return (RegExp(name + '=' + '(.+?)(&|$)').exec(url)||[,null])[1];
}
function findAeWLContainer(b) {
	return a('a[href*="/products/"]', b.parent()).has('img[src*="/products/"] , img[src*="/no-image"]').length > 1 || b.parent().width() - b.width() > 50 ? b : findAeWLContainer(b.parent());
}
function getAeConfigWL(obj, strDefault) {
	if(typeof obj !== "undefined") {
		return obj;
	} else if(typeof strDefault !== "undefined") {
		return strDefault;
	} else {
		return "";
	}
}
var aeShare;
var baseUrl = "https://apollotran.com/app/haravan/apwishlist/install/";
(function(a) {
	aeShare = {
		initHtml: function() {
						a.ajax({
				url: baseUrl + "?frontend=" + Haravan.shop,
				dataType: "Json",
				success: function(response) {
					log(response);
					aeConfigWL = response;
					var itemTrigger = getAeConfigWL(aeConfigWL.btnfire, "");
					var item = getAeConfigWL(aeConfigWL.item, "");
					animate = getAeConfigWL(aeConfigWL.animate, "");
					btnTriggerWL = itemTrigger ? itemTrigger : btnTriggerWL;
										
					if(item && aeClsCoverNameWL != item) {
						a(item).addClass(aeClsCoverNameWL);
						aeClsCoverNameWL = item;
					}
									a(btnTriggerWL).each(function() {
										a(this).closest("div").addClass(aeClsCoverNameWL.replace(".", ""));
									});
									a("." + aeClsCoverNameWL.replace(".", "")).mouseenter(function(e) {
										a(this).find(".ap-cover-btn-wl").addClass(animate + " ap-animated");
									}).mouseleave(function() {
										a(this).find(".ap-cover-btn-wl").removeClass(animate + " ap-animated");
									});
									aeShare.addToWishlist();
				}
			});
		},
			addToWishlist: function() {
				a(btnTriggerWL).click(function(e) {
					var btn = a(this);
					if(a(btn).data("status") == "added") {
						window.location.href = a(btn).attr("href");
						return false;
					}
					var id = a(this).data("productid");
					var handle = a(this).data("handle");
					var title = a(this).data("title");
					var variant = a(this).data("variant");

					if(typeof customerId != "undefined" && customerId) {
						e.preventDefault();
						a.ajax({
							type: "Get",
							url: "https://apollotran.com/app/haravan/apwishlist/install/?add=" + Haravan.shop,
							data: {"action" : "add", "customerId" : customerId, "email" : customerEmail, "productId": id, "productHandle": handle, variant: variant, productTitle: title},
							dataType: "Json",
							beforeSend:function(){
								// a(btn).text("Adding...");
							},
							success:function(n) {
								
								if (!!a.prototype.fancybox) {
									if(!a.fancybox.isOpen) {
										a.fancybox.open([{
											type: 'inline',
											autoScale: true,
											minHeight: 30,
											content: '<p class="ae-fancybox-success">' + getAeConfigWL(aeConfigWL.added, "Added to your wishlist.") + ' <br/><a class="btn ae-btn-outline-inverse btn-wishlist" href="/pages/wish-list">Go to Wishlist</a></p>'
										}], {
											padding: 0
										});
									}
								} else {
									
									alert(getAeConfigWL(aeConfigWL.added, "Added to your Wishlist."));
								}
								var clsicon = getAeConfigWL(aeConfigWL.clsicon, "");
								var gotitle = getAeConfigWL(aeConfigWL.btngotitle, "Go to Wishlist.");
								if (gotitle) {
									var html = clsicon ? "<i class='" + clsicon + "'></i>" : "";
										html += gotitle;
									a(btn).find("span").html(html);
								}
								a(btn).attr("href", "/pages/" + getAeConfigWL(aeConfigWL.handle, "wish-list"));
								a(btn).data("status", "added");
							},
							error:function(hx, e) {
								alert(getAeConfigWL(aeConfigWL.error, "Can not add to wishlist, please try again."));
								// window.location.reload();
							},
							complete: function() {
							}
						});
						return false;
					}
				});
			}
	}
	a(document).ready(function() {
		if(!isCallWL) {
					aeShare.initHtml();
//           aeShare.addToWishlist();
					isCallWL = true;
		}
	});
})(jQuery);
