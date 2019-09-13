$(document).ready(function ($) {
	awe_backtotop();
	awe_owl();
	awe_category();
	awe_menumobile();
	awe_tab();
	$('.btn--view-more').on('click', function(e){
		e.preventDefault();
		var $this = $(this);
		$this.parents('#home').find('.product-well').toggleClass('expanded');
		return false;
	});
	$(window).scroll(function() {
		if ($(this).scrollTop() > 160){  
			$('.main-nav-border').addClass("sticky");
		}
		else{
			$('.main-nav-border').removeClass("sticky");
		}
	});
	$('.aside-filter .fiter-title').on('click', function(e){
		e.preventDefault();
		var $this = $(this);
		$this.parents('.aside-filter').find('.aside-hidden-mobile').stop().slideToggle();
		$(this).toggleClass('active')
		return false;
	});
	$('.header-search img').click(function(e){
		e.stopPropagation();
		$( ".search-header" ).toggle( "slow", function() {
		});
	});
	if ($(window).width() < 1025) {
		$('.user-hover .a-users').on('click', function(e){
			e.preventDefault();
			var $this = $(this);
			$this.parents('.user-hover').find('.user-popup').stop().slideToggle();
			$(this).toggleClass('active');
			$(".header-search .search-header").hide();
			return false;
		});
		$('.header-search i').click(function(e){
			$(".user-hover .user-popup").hide();
		});
	}
	$('#cart-sidebars .cart_btn-close').click(function() {
		$("#cart-sidebars").removeClass('active');
		$(".backdrop__body-backdrop___1rvky").removeClass('active');
	});
	$('.backdrop__body-backdrop___1rvky').click(function() {
		$("#cart-sidebars").removeClass('active');
		$(".backdrop__body-backdrop___1rvky").removeClass('active');
	});
	if($('.cart_body>div').length == '0' ){
		$('.cart-footer').hide();
		jQuery('<div class="cart-empty">'
					 + '<img src="//bizweb.dktcdn.net/100/270/860/themes/606449/assets/empty-bags.jpg?1510132489127" class="img-responsive center-block" alt="Giỏ hàng trống" />'
					 + '<div class="btn-cart-empty">'
					 + '<a class="btn btn-default" href="/" title="Tiếp tục mua sắm">Tiếp tục mua sắm</a>'
					 + '</div>'
					 + '</div>').appendTo('.cart_body');
	};
	var $scrollToTop = $("#scroll-to-top"),
			$cart = $(".cart-float-right");
	function totop_button(status) {
		if (status == "on") { 
			$cart.addClass("on fadeInRight ").removeClass("off fadeOutRight");
			$scrollToTop.addClass("on fadeInRight ").removeClass("off fadeOutRight"); 
		} else {
			$cart.addClass("off fadeOutRight animated").removeClass("on fadeInRight");
			$scrollToTop.addClass("off fadeOutRight animated").removeClass("on fadeInRight"); 
		}
	}
	$(window).scroll(function() {
		var $scrollTop = $(this).scrollTop();
		var scrollHeight = $(this).height();
		if ($scrollTop > 0) { 
			var position = $scrollTop + scrollHeight / 2;
		} 
		else { 
			var position = 1 ;
		}    
		if (position < 1e3 && position < scrollHeight) { 
			totop_button("off");
		} 
		else {
			totop_button("on"); 
		}
	});  

	$scrollToTop.on('click', function(e){
		e.preventDefault();
		$('body,html').animate({scrollTop:0},800,'swing');
	});
});
$(document).ready(function (){
	var owlh = $('.home-slider');
	owlh.owlCarousel({
		nav:true,
		slideSpeed : 600,
		paginationSpeed : 400,
		singleItem:true,
		pagination : false,
		dots: false,
		autoplay:true,
		autoplayTimeout:4500,
		autoplayHoverPause:false,
		autoHeight: false,
		loop: true,
		responsive: {
			0: {
				items: 1
			},
			543: {
				items: 1
			},
			768: {
				items: 1
			},
			991: {
				items: 1
			},
			992: {
				items: 1
			},
			1300: {
				items: 1,
			},
			1590: {
				items: 1,
			}
		}
	});
	function setAnimation ( _elem, _InOut ) {
		var animationEndEvent = 'webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend';
		_elem.each ( function () {
			var $elem = $(this);
			var $animationType = 'animated ' + $elem.data( 'animation-' + _InOut );

			$elem.addClass($animationType).one(animationEndEvent, function () {
				$elem.removeClass($animationType);
			});
		});
	}
	owlh.on('changed.owl.carousel', function(event) {
		var $currentItem = $('.owl-item', owlh).eq(event.item.index);
		var $elemsToanim = $currentItem.find("[data-animation-out]");
		setAnimation ($elemsToanim, 'out');
	});
	owlh.on('changed.owl.carousel', function(event) {
		var $currentItem = $('.owl-item', owlh).eq(event.item.index);
		var $elemsToanim = $currentItem.find("[data-animation-in]");
		setAnimation ($elemsToanim, 'in');
	})
});
$(".products-best-sell-owl").owlCarousel({
	nav:true,
	slideSpeed : 600,
	paginationSpeed : 400,
	singleItem:true,
	pagination : false,
	dots: false,
	autoplay:true,
	margin:10,
	autoplayTimeout:6000,
	autoplayHoverPause:true,
	autoHeight: false,
	loop: false,
	responsive: {
		0: {
			items: 2
		},
		543: {
			items: 2
		},
		767: {
			items: 3
		},
		768: {
			items: 4
		},
		991: {
			items: 4
		},
		992: {
			items: 4
		},
		1300: {
			items: 4
		},
		1590: {
			items: 4
		}
	}
});
$(".article-slider").owlCarousel({
	nav:true,
	slideSpeed : 600,
	paginationSpeed : 400,
	singleItem:true,
	pagination : false,
	dots: false,
	autoplay:true,
	margin:10,
	autoplayTimeout:6000,
	autoplayHoverPause:true,
	autoHeight: false,
	loop: false,
	responsive: {
		0: {
			items: 1
		},

		480: {
			items: 1
		},
		768: {
			items: 2
		},

		992: {
			items: 3
		},
		1300: {
			items: 3
		},
		1590: {
			items: 3
		}
	}
});
$(".blogs-owl").owlCarousel({
	nav:true,
	pagination : false,
	dots: false,
	autoplay:true,
	autoplayTimeout:4000,
	autoplayHoverPause:true,
	loop: true,
	responsive: {
		0: {
			items: 1
		},
		768: {
			items: 1
		},
		992: {
			items: 1
		},
		1590: {
			items: 1
		}
	}
});
$(".products-view-owl").owlCarousel({
	nav:true,
	slideSpeed : 600,
	paginationSpeed : 400,
	singleItem:true,
	pagination : false,
	dots: false,
	autoplay:true,
	margin:10,
	autoplayTimeout:6000,
	autoplayHoverPause:true,
	autoHeight: false,
	loop: false,
	responsive: {
		0: {
			items: 2
		},
		543: {
			items: 2
		},
		767: {
			items: 3
		},
		768: {
			items: 4
		},
		991: {
			items: 4
		},
		992: {
			items: 4
		},
		1300: {
			items: 4
		},
		1590: {
			items: 4
		}
	}
});
$(".instagram-owl").owlCarousel({
	nav:true,
	slideSpeed : 600,
	paginationSpeed : 400,
	singleItem:true,
	pagination : false,
	dots: false,
	autoplay:true,
	margin:0,
	autoplayTimeout:6000,
	autoplayHoverPause:true,
	autoHeight: false,
	loop: false,
	responsive: {
		0: {
			items: 2
		},
		543: {
			items: 2
		},
		768: {
			items: 4
		},
		991: {
			items: 5
		},
		992: {
			items: 6
		},
		1300: {
			items: 6
		},
		1590: {
			items: 6
		}
	}
});
$(".owl-policy-mobile").owlCarousel({
	nav:false,
	slideSpeed : 600,
	paginationSpeed : 400,
	singleItem:false,
	pagination : false,
	dots: false,
	autoplay:true,
	autoplayTimeout:4500,
	autoplayHoverPause:false,
	autoHeight: false,
	loop: false,
	responsive: {
		0: {
			items: 2
		},
		543: {
			items: 2
		},
		768: {
			items: 3
		},
		991: {
			items: 3
		},
		992: {
			items: 4
		},
		1300: {
			items: 4,
		},
		1590: {
			items: 4,
		}
	}
});
$(window).on("load resize",function(e){	
	setTimeout(function(){					 
		awe_resizeimage();
	},200);
	setTimeout(function(){	
		awe_resizeimage();
	},1000);
});
$(document).on('click','.overlay, .close-popup, .btn-continue, .fancybox-close', function() {   
	hidePopup('.awe-popup'); 	
	setTimeout(function(){
		$('.loading').removeClass('loaded-content');
	},500);
	return false;
})
$(document).on("click", "#trigger-mobile", function(){
	if ($('body').hasClass('responsive') == false) {
		$('body').addClass('responsive helper-overflow-hidden');
		$('html').addClass('helper-overflow-hidden');
		$(window).scrollTop(0);
		$('body').removeClass('hideresponsive');
		$("#box-wrapper").bind('click', function () {
			$("body").removeClass("responsive helper-overflow-hidden");
			$('html').removeClass('helper-overflow-hidden');
			$('body').addClass('hideresponsive');
			$("#box-wrapper").unbind('click');
		});
	}
	else {
		$("body").removeClass("responsive helper-overflow-hidden");
		$('html').removeClass('helper-overflow-hidden');
		$('body').addClass('hideresponsive');
	}
});
$('#menu-mobile .menu-mobile .submenu-level1-children-a').on('click', function(e){
	e.preventDefault();
	var $this = $(this);
	$this.parents('#menu-mobile .menu-mobile li').find('.submenu-level1-children').stop().toggleClass('active');
	$this.parents('#menu-mobile .menu-mobile li .account_mobile i.fa').find('.submenu-level1-children').stop().toggleClass('active');

	$(this).toggleClass('active')
	return false;
});
$(document).ready(function(){
	$(".account_mobile i.fa").click(function(){
		$(".submenu-level1-children").removeClass('active');
	});
});


$(window).scroll(function () {
	if($(window).width() < 769){
		if ($(this).scrollTop() > 70) {
			$('.hrvproduct-tabs .tabs').addClass('fix-nav');
		} else {
			$('.hrvproduct-tabs .tabs').removeClass('fix-nav');
		}
	}
});

function awe_showNoitice(selector) {
	$(selector).animate({right: '0'}, 500);
	setTimeout(function() {
		$(selector).animate({right: '-300px'}, 500);
	}, 3500);
}  window.awe_showNoitice=awe_showNoitice;
function awe_showLoading(selector) {
	var loading = $('.loader').html();
	$(selector).addClass("loading").append(loading); 
}  window.awe_showLoading=awe_showLoading;
function awe_hideLoading(selector) {
	$(selector).removeClass("loading"); 
	$(selector + ' .loading-icon').remove();
}  window.awe_hideLoading=awe_hideLoading;
function awe_showPopup(selector) {
	$(selector).addClass('active');
}  window.awe_showPopup=awe_showPopup;
function awe_hidePopup(selector) {
	$(selector).removeClass('active');
}  window.awe_hidePopup=awe_hidePopup;
function awe_convertVietnamese(str) { 
	str= str.toLowerCase();
	str= str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g,"a"); 
	str= str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g,"e"); 
	str= str.replace(/ì|í|ị|ỉ|ĩ/g,"i"); 
	str= str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g,"o"); 
	str= str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g,"u"); 
	str= str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g,"y"); 
	str= str.replace(/đ/g,"d"); 
	str= str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g,"-");
	str= str.replace(/-+-/g,"-");
	str= str.replace(/^\-+|\-+$/g,""); 
	return str; 
} window.awe_convertVietnamese=awe_convertVietnamese;
function awe_resizeimage() { 
	$('.product-box .product-thumbnail a img').each(function(){
		var t1 = (this.naturalHeight/this.naturalWidth);
		var t2 = ($(this).parent().height()/$(this).parent().width());
		if(t1<= t2){
			$(this).addClass('bethua');
		}
		var m1 = $(this).height();
		var m2 = $(this).parent().height();
		if(m1 <= m2){
			$(this).css('padding-top',(m2-m1)/2 + 'px');
		}
	})	
} window.awe_resizeimage=awe_resizeimage;
function awe_category(){
	$('.nav-category .fa-angle-down').click(function(e){
		$(this).parent().toggleClass('active');
	});
} window.awe_category=awe_category;
function awe_menumobile(){
	$('.menu-bar').click(function(e){
		e.preventDefault();
		$('#nav').toggleClass('open');
	});
	$('#nav .fa').click(function(e){		
		e.preventDefault();
		$(this).parent().parent().toggleClass('open');
	});
} window.awe_menumobile=awe_menumobile;
function awe_accordion(){
	$('.accordion .nav-link').click(function(e){
		e.preventDefault;
		$(this).parent().toggleClass('active');
	})
} window.awe_accordion=awe_accordion;
function awe_owl() { 
	$('.owl-carousel:not(.not-dqowl)').each( function(){
		var xs_item = $(this).attr('data-xs-items');
		var md_item = $(this).attr('data-md-items');
		var sm_item = $(this).attr('data-sm-items');	
		var margin=$(this).attr('data-margin');
		var dot=$(this).attr('data-dot');
		if (typeof margin !== typeof undefined && margin !== false) {    
		} else{
			margin = 30;
		}
		if (typeof xs_item !== typeof undefined && xs_item !== false) {    
		} else{
			xs_item = 1;
		}
		if (typeof sm_item !== typeof undefined && sm_item !== false) {    

		} else{
			sm_item = 3;
		}	
		if (typeof md_item !== typeof undefined && md_item !== false) {    
		} else{
			md_item = 3;
		}
		if (typeof dot !== typeof undefined && dot !== true) {   
			dot= true;
		} else{
			dot = false;
		}
		$(this).owlCarousel({
			loop:false,
			margin:Number(margin),
			responsiveClass:true,
			dots:dot,
			nav:true,
			responsive:{
				0:{
					items:Number(xs_item)				
				},
				600:{
					items:Number(sm_item)				
				},
				1000:{
					items:Number(md_item)				
				}
			}
		})
	})
} window.awe_owl=awe_owl;
$('#gallery_01').owlCarousel({
	loop:false,
	margin:20,
	responsiveClass:true,
	dots:false,
	nav:true,
	responsive:{
		0:{
			items:3,
			margin:10
		},
		543:{
			items:3
		},
		768:{
			items:4
		},
		992:{
			items:4
		},
		1200:{
			items:4
		},
		1500:{
			items:4
		}
	}
})
function awe_backtotop() { 
	if ($('.back-to-top').length) {
		var scrollTrigger = 100, // px
				backToTop = function () {
					var scrollTop = $(window).scrollTop();
					if (scrollTop > scrollTrigger) {
						$('.back-to-top').addClass('show');
					} else {
						$('.back-to-top').removeClass('show');
					}
				};
		backToTop();
		$(window).on('scroll', function () {
			backToTop();
		});
		$('.back-to-top').on('click', function (e) {
			e.preventDefault();
			$('html,body').animate({
				scrollTop: 0
			}, 700);
		});
	}
} window.awe_backtotop=awe_backtotop;
function awe_tab() {
	$(".e-tabs").each( function(){
		$(this).find('.tabs-title li:first-child').addClass('current');
		$(this).find('.tab-content').first().addClass('current');

		$(this).find('.tabs-title li').click(function(){
			var tab_id = $(this).attr('data-tab');
			var url = $(this).attr('data-url');
			$(this).closest('.e-tabs').find('.tab-viewall').attr('href',url);
			$(this).closest('.e-tabs').find('.tabs-title li').removeClass('current');
			$(this).closest('.e-tabs').find('.tab-content').removeClass('current');
			$(this).addClass('current');
			$(this).closest('.e-tabs').find("#"+tab_id).addClass('current');
		});    
	});
} window.awe_tab=awe_tab;
$('.dropdown-toggle').click(function() {
	$(this).parent().toggleClass('open'); 	
}); 
$('.btn-close').click(function() {
	$(this).parents('.dropdown').toggleClass('open');
}); 
$('body').click(function(event) {
	if (!$(event.target).closest('.dropdown').length) {
		$('.dropdown').removeClass('open');
	};
});
$(document).on('keydown','#qty, #quantity-detail, .number-sidebar',function(e){-1!==$.inArray(e.keyCode,[46,8,9,27,13,110,190])||/65|67|86|88/.test(e.keyCode)&&(!0===e.ctrlKey||!0===e.metaKey)||35<=e.keyCode&&40>=e.keyCode||(e.shiftKey||48>e.keyCode||57<e.keyCode)&&(96>e.keyCode||105<e.keyCode)&&e.preventDefault()});
$(document).on('click','.qtyplus',function(e){
	e.preventDefault();   
	fieldName = $(this).attr('data-field'); 
	var currentVal = parseInt($('input[data-field='+fieldName+']').val());
	if (!isNaN(currentVal)) { 
		$('input[data-field='+fieldName+']').val(currentVal + 1);
	} else {
		$('input[data-field='+fieldName+']').val(0);
	}
});
(function($) {
	"use strict";
	$(document).on(
		"show.bs.tab",
		'.nav-tabs-responsive [data-toggle="tab"]',
		function(e) {
			var $target = $(e.target);
			var $tabs = $target.closest(".nav-tabs-responsive");
			var $current = $target.closest("li");
			var $parent = $current.closest("li.dropdown");
			$current = $parent.length > 0 ? $parent : $current;
			var $next = $current.next();
			var $prev = $current.prev();
			var updateDropdownMenu = function($el, position) {
				$el
				.find(".dropdown-menu")
				.removeClass("pull-xs-left pull-xs-center pull-xs-right")
				.addClass("pull-xs-" + position);
			};
			$tabs.find(">li").removeClass("next prev");
			$prev.addClass("prev");
			$next.addClass("next");
			updateDropdownMenu($prev, "left");
			updateDropdownMenu($current, "center");
			updateDropdownMenu($next, "right");
		}
	);
})(jQuery);
(function(b){function c(){}for(var d="assert,count,debug,dir,dirxml,error,exception,group,groupCollapsed,groupEnd,info,log,timeStamp,profile,profileEnd,time,timeEnd,trace,warn".split(","),a;a=d.pop();){b[a]=b[a]||c}})((function(){try
{console.log();return window.console;}catch(err){return window.console={};}})());
var GLOBAL = {
	common : {
		init: function(){
			$('.add_to_cart').bind( 'click', addToCart );
		}
	},
	templateIndex : {
		init: function(){

		}
	},
	templateProduct : {
		init: function(){

		}
	},
	templateCart : {
		init: function(){

		}
	}
}
var UTIL = {
	fire : function(func,funcname, args){
		var namespace = GLOBAL;
		funcname = (funcname === undefined) ? 'init' : funcname;
		if (func !== '' && namespace[func] && typeof namespace[func][funcname] == 'function'){
			namespace[func][funcname](args);
		}
	},

	loadEvents : function(){
		var bodyId = document.body.id;
		UTIL.fire('common');
		$.each(document.body.className.split(/\s+/),function(i,classnm){
			UTIL.fire(classnm);
			UTIL.fire(classnm,bodyId);
		});
	}

};
$(document).ready(UTIL.loadEvents);
Number.prototype.formatMoney = function(c, d, t){
	var n = this, 
			c = isNaN(c = Math.abs(c)) ? 2 : c, 
			d = d == undefined ? "." : d, 
			t = t == undefined ? "." : t, 
			s = n < 0 ? "-" : "", 
			i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", 
			j = (j = i.length) > 3 ? j % 3 : 0;
	return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};
function addToCart(e){
	if (typeof e !== 'undefined') e.preventDefault();
	var $this = $(this);
	var form = $this.parents('form');		
	$.ajax({
		type: 'POST',
		url: '/cart/add.js',
		async: false,
		data: form.serialize(),
		dataType: 'json',
		error: addToCartFail,
		beforeSend: function() {  
		},
		success: addToCartSuccess,
		cache: false
	});
}
function addToCartSuccess (jqXHR, textStatus, errorThrown){
	$.ajax({
		type: 'GET',
		url: '/cart.js',
		async: false,
		cache: false,
		dataType: 'json',
		success: function (cart){
			awe_hidePopup('.loading');
			Haravan.updateCartFromForm(cart, '.top-cart-content .mini-products-list');
			Haravan.updateCartPopupForm(cart, '#popup-cart-desktop .tbody-popup');
		}
	});
	var url_product = jqXHR['url'];
	var class_id = jqXHR['product_id'];
	var name = jqXHR['name'];
	var textDisplay = ('<i style="margin-right:5px; color:red; font-size:13px;" class="fa fa-check" aria-hidden="true"></i>Sản phẩm vừa thêm vào giỏ hàng');
	var id = jqXHR['variant_id'];
	if( jqXHR['image'] != null){
		var src = Haravan.resizeImage(jqXHR['image'], 'medium');
	}else{
		var src = "https://bizweb.dktcdn.net/thumb/large/assets/themes_support/noimage.gif";
	}
	var dataList = $(".item-name a").map(function() {
		var plus = $(this).text();
		return plus;
	}).get();
	$('.title-popup-cart .cart-popup-name').html('<a href="'+ url_product +'"style="color:red;" title="'+name+'">'+ name + '</a> ');
	var nameid = dataList,
			found = $.inArray(name, nameid);
	var textfind = found;
	$(".item-info > p:contains("+id+")").html('<span class="add_sus" style="color:#898989;"><i style="margin-right:5px; color:red; font-size:13px;" class="fa fa-check" aria-hidden="true"></i>Sản phẩm vừa thêm!</span>');
	var windowW = $(window).width();
	if(windowW > 768){				
		$(".add-to-cart-success").css("display", "block");
		$('.add-to-cart-success .close').click(function() {
			$(".add-to-cart-success").css("display", "none");
		});
	}else{
		$('.mobile-success-notification').html('');
		var $popupMobile = '<div class="layout vertical">'
		+ '<div class="layout horizontal status center">'
		+ '<img src="http://bizweb.dktcdn.net/100/199/207/themes/519291/assets/icon-tick.svg" alt="Giỏ hàng" width="18" style="margin-right:8px;">'
		+ '<span class="paper-font-body1" style="color:#3c763d">Đã thêm thành công sản phẩm vào giỏ hàng</span>'
		+ '</div><div class="layout horizontal content center">'
		+ '<img id="img" src="'+ src +'" style="opacity: 1; transition: opacity 0.5s;" alt="'+ jqXHR['title'] +'" />'
		+ '<div class="flex-auto">'
		+ '<div class="paper-font-subhead">'+ jqXHR['title'] +'</div>'
		+ '<div class="paper-font-body1 black-54" style="margin: 2px 0 4px"></div></div></div></div>';
		$('.mobile-success-notification').html($popupMobile);
		$('.mobile-success-notification').addClass("opened");
		setTimeout(function(){
			$('.mobile-success-notification').removeClass('opened');
		}, 5000)
	}
}
function addToCartFail(jqXHR, textStatus, errorThrown){
	var response = $.parseJSON(jqXHR.responseText);
	var $info = '<div class="error">'+ response.description +'</div>';
}
function bindGrid() {
	var view = jQuery.totalStorage('display');

	if (!view && (typeof displayList != 'undefined') && displayList) view = 'list';

	if (view && view != 'grid') display(view);
	else $('.display').find('li#grid').addClass('selected');

	jQuery(document).on('click', '#grid', function(e) {
		e.preventDefault();
		display('grid');
	});

	jQuery(document).on('click', '#list', function(e) {
		e.preventDefault();
		display('list');
	});
}


function display(view) {
	if (view == 'list') {

		jQuery('.products-view-grid').removeClass('grid').addClass('list');
		jQuery('.products-view-grid').find('.ajax_block_product').removeClass('col-sm-4 col-md-3');
		jQuery('.left-block').addClass('col-sm-4 col-md-3');
		jQuery('.right-block').addClass('col-sm-8 col-md-9');
		jQuery('.product-desc').show();
		jQuery('.display').find('li#list').addClass('selected');
		jQuery('.display').find('li#grid').removeAttr('class');
		jQuery(window).resize();
		jQuery.totalStorage('display', 'list');
	} else {
		jQuery('.pproducts-view-grid').removeClass('list').addClass('grid');
		jQuery('.products-view-grid').find('.ajax_block_product').addClass('col-sm-4 col-md-3');
		jQuery('.left-block').removeClass('col-sm-4 col-md-3');
		jQuery('.right-block').removeClass('col-sm-8 col-md-9');
		jQuery('.product-desc').hide();
		jQuery('.display').find('li#grid').addClass('selected');
		jQuery('.display').find('li#list').removeAttr('class');
		jQuery(window).resize();
		jQuery.totalStorage('display', 'grid');
	}

}

















