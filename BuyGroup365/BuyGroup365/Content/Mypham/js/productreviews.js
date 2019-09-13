/*
FileName: ProductReview.js
Version:        CreatedBy:              LastUpdate: 
1.0.0.6         NguyenNguyen            2017-03-21
1.0.0.5         NguyenNguyen            2015-08-06
1.0.0.4         NguyenNguyen            2015-08-05
1.0.0.3         NguyenNguyen            2015-07-31
1.0.0.3         NguyenNguyen            2015-07-28
1.0.0.2         NguyenNguyen            2015-07-28
1.0.0.1         NguyenNguyen            2015-07-27
1.0.0.0         NguyenNguyen            2015-07-27
*/

var Host = "https://inapp.haravan.com/productreviews/";

var ProductReviews = function () {

    //Init
    var MainPanel = jQuery("#hrv-product-reviews");
    var Shop = window.location.hostname;
    var PageCurrent = 1;
    var PageSize = 10;
    var Take = 5;
    var more = 10;
    var bodyMax = 1000;
    var StarColor = "#ECBA55";
    var GUI = function () {
        return {

            LoadProductReviewCSS: function () {
                var product_reviews_css_path = "content/css/productreviews.css?v=" + Math.floor(Date.now() / 1000);
                LoadCSS(Host + product_reviews_css_path);
            },

            LoadRatingCSS: function () {

                var rating_css_path = "content/css/jquery.raty.css";
                LoadCSS(Host + rating_css_path);
            },

            CreateDivSummary: function () {
                var html = "";
                html += "<span class=\"product-reviews-summary-actions\">";
                html += "<input type=\"button\" id=\"btnnewreview\" value = \"Viết đánh giá\" />";
                html += "</span>";
                jQuery('#hrv-product-reviews-list').before(html);
            },

            CreateSubmitForm: function () {
                var html = "";
                html += "<div class=\"hrv-product-reviews-form\" id=\"hrv-product-reviews-form\" style=\"display:none\">";
                html += "<h3>Viết đánh giá mới</h3>";
                html += "<form id=\"hrv-product-reviews-frm\" name=\"hrv-product-reviews-frm\" >"
                html += "<fieldset>";
                html += "<div id=\"dvName\">";
                html += "<label>Tên</label>";
                html += "<input id=\"hrv_product_reviews_form_name\"  type=\"text\" placeholder=\"Tên của bạn (>3 ký tự và < 20 ký tự)\" autocomplete=\"off\"></input>";
                html += "</div>";
                html += "</fieldset>";

                html += "<fieldset>";
                html += "<div id=\"dvEmail\">";
                html += "<label>Email</label>";
                html += "<input id=\"hrv_product_reviews_form_email\"  type=\"text\" placeholder=\"john.smith@example.com\" autocomplete=\"off\"></input>";
                html += "</div>";
                html += "</fieldset>";

                html += "<fieldset id=\"hrv_product_reviews_form_fieldset_rate\">";
                html += "<label>Đánh giá</label>";
                html += "<div id=\"dvRating\" >";
                html += "</div>"
                html += "<input type=\"hidden\" id=\"hrv_product_reviews_form_rate\" value=\"5\"/>";
                if (Haravan) {
                    html += "<input type=\"hidden\" id=\"hrv_product_reviews_form_shop\" value=\"" + Haravan.shop + "\"/>";
                }
                else {
                    html += "<input type=\"hidden\" id=\"hrv_product_reviews_form_shop\" value=\"" + window.location.hostname + "\"/>";
                }
                html += "<input type=\"hidden\" id=\"hrv_product_reviews_form_fullshop\" value=\"" + window.location.protocol + "//" + window.location.hostname + "/\"/>";
                html += "<input type=\"hidden\" id=\"hrv_product_reviews_form_product\" id=\"hrv_product_reviews_form_product\" value=\"" + jQuery('#hrv-product-reviews').attr('data-id') + "\"/>";
                html += "<input type=\"hidden\" id=\"hrv_product_reviews_form_callback\" value=\"" + window.location + "\" />";

                var listcut = window.location.href.split("/");
                var productname = listcut[listcut.length - 1];
                html += "<input type=\"hidden\" id=\"hrv_product_reviews_form_product_name\" value=\"" + productname + "\" />";
                html += "</fieldset>";
                html += "<fieldset>";
                html += "<div id=\"dvTitle\">";
                html += "<label>Tiêu đề</label>";
                html += "<input id=\"hrv_product_reviews_form_title\" type=\"text\" placeholder=\"Hãy cho một tiêu đề (>3 ký tự và < 50 ký tự)\" autocomplete=\"off\"></input>";
                html += "</div>";
                html += "</fieldset>";

                html += "<fieldset>";
                html += "<div id=\"dvBody\">";
                html += "<label>Nội dung</label> <label id=\"dvBody_character\"></label>";
                html += "<textarea id=\"hrv_product_reviews_form_body\"  rows=\"10\" placeholder=\"Viết nội dung đánh giá ở đây (>3 ký tự và < " + bodyMax + " ký tự)\"></textarea>";
                html += "</div>";
                html += "</fieldset>";
                html += "<p class=\"error\" />";
                html += "<input type=\"submit\" id=\"btnSubmitReview\" value=\"Gửi đánh giá\" width=\"80\"></input> ";
                html += "</form>";
                html += "<br />";
                html += "</div>";
                html += "<div class=\"hrv-product-reviews-form\" id=\"hrv-product-reviews-thanks\" style=\"display:none; padding-top:20px\" >";
                html += "<center><b>Cám ơn bạn đã đánh giá sản phẩm!</b><center>";
                html += "</div>"
                html += "<br />";
                jQuery('#hrv-product-reviews-list').before(html);
            },
        }

    }();

    var Act = function () {
        return {

            AddNewReview: function () {
                jQuery("#btnnewreview").hide();
                jQuery("#hrv-product-reviews-thanks").show();
                jQuery("#hrv-product-reviews-form").slideToggle();
                jQuery.ajax({
                    type: "POST",
                    url: Host + "clientrequest/postreview",
                    data: {
                        data: JSON.stringify({
                            hrv_product_reviews_form_name: jQuery('#hrv_product_reviews_form_name').val(),
                            hrv_product_reviews_form_email: jQuery('#hrv_product_reviews_form_email').val(),
                            hrv_product_reviews_form_rate: jQuery('#hrv_product_reviews_form_rate').val(),
                            hrv_product_reviews_form_shop: jQuery('#hrv_product_reviews_form_shop').val(),
                            hrv_product_reviews_form_fullshop: jQuery('#hrv_product_reviews_form_fullshop').val(),
                            hrv_product_reviews_form_product: jQuery('#hrv_product_reviews_form_product').val(),
                            hrv_product_reviews_form_callback: jQuery('#hrv_product_reviews_form_callback').val(),
                            hrv_product_reviews_form_title: jQuery('#hrv_product_reviews_form_title').val(),
                            hrv_product_reviews_form_body: jQuery('<div/>').text(jQuery('#hrv_product_reviews_form_body').val()).html(),
                            hrv_product_reviews_form_product_name: jQuery('#hrv_product_reviews_form_product_name').val()
                        })
                    },
                    dataType: 'json',
                    success: function (data) {

                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.log(errorThrown);
                    }

                }).done(function (data) {
                    Act.ResetForm();
                });
            },

            GetMoreReview: function () {
                jQuery("#btnMoreReview").on('click', function () {
                    Take += more;
                });
            },

            ToogleSubmitForm: function () {
                jQuery("#btnnewreview").on("click", function () {
                    jQuery("#hrv-product-reviews-form").slideToggle();
                });
            },

            ValidateSubmitForm: function () {
                jQuery("#hrv_product_reviews_form_body").keyup(function () {
                    var cs = jQuery(this).val().length;
                    jQuery('#dvBody_character').text("(" + cs + "/" + bodyMax + ")");
                });

                jQuery("#btnSubmitReview").on('click', function () {
                    var Name = jQuery("#hrv_product_reviews_form_name").val();
                    var Email = jQuery("#hrv_product_reviews_form_email").val();
                    var Title = jQuery("#hrv_product_reviews_form_title").val();
                    var Body = jQuery("#hrv_product_reviews_form_body").val();
                    var isName = false;
                    var isEmail = false;
                    var isTitle = false;
                    var isBody = false;
                    if (Name.length < 3 || Name.length > 20) {
                        jQuery("#dvName").addClass("has-error");
                    }
                    else {
                        jQuery("#dvName").removeClass("has-error");
                        isName = true;
                    }
                    if (Email == "" || !Act.IsValidEmailAddress(Email)) {
                        jQuery("#dvEmail").addClass("has-error");
                    }
                    else {
                        isEmail = true;
                        jQuery("#dvEmail").removeClass("has-error");
                    }
                    if (Title.length < 3 || Title.length > 50) {
                        jQuery("#dvTitle").parent().addClass("has-error");
                    }
                    else {
                        isTitle = true;
                        jQuery("#dvTitle").parent().removeClass("has-error");
                    }

                    if (Body.length < 3 || Body.length > bodyMax) {
                        jQuery("#dvBody").parent().addClass("has-error");
                    }
                    else {
                        isBody = true;
                        jQuery("#dvBody").parent().removeClass("has-error");
                    }

                    if (isName == false || isEmail == false || isTitle == false || isBody == false) {
                        jQuery(".error").html("Xin Vui Lòng Kiểm Tra Lại Thông Tin Đã Nhập");
                        jQuery(".error").show();
                    }
                    else {
                        jQuery(".error").hide();
                        Act.AddNewReview();
                    }
                    return false;
                });
            },

            GetReview: function (page) {

                if (page == null)
                    return;

                jQuery.ajax({
                    type: "GET",
                    dataType: 'json',
                    url: Host + "clientrequest/getreview",
                    data: {
                        data: JSON.stringify({
                            hrv_product_reviews_form_shop: jQuery('#hrv_product_reviews_form_shop').val(),
                            hrv_product_reviews_form_product: jQuery('#hrv_product_reviews_form_product').val(),
                            hrv_product_reviews_form_fullshop: jQuery('#hrv_product_reviews_form_fullshop').val(),
                            Take: page
                        })
                    },
                    success: function (data) {
                        jQuery("#hrv-product-reviews-list").html("");
                        if (data != null)
                            for (i = 0; i < data.length; i++) {
                                var metafield = "";
                                metafield += "<div itemscope='' itemtype='http://data-vocabulary.org/Review'>";
                                metafield += "<div>";
                                metafield += "<div class=\"hrv-product-reviews-star\" data-score=\"" + data[i].star_rate + "\" data-number=\"" + 5 + "\" >";
                                metafield += "</div>";
                                metafield += "<p style='color:#8B8B8B'>";
                                metafield += "<b>" + data[i].title + "</b>";
                                metafield += "- <span itemprop='reviewer'>" + data[i].name + "</span>";
                                metafield += "<i> - " + data[i].date_create;
                                metafield += "</i>";
                                metafield += "</p>";
                                metafield += "</div>";
                                metafield += "<span itemprop='description'>" + data[i].body + "</span>";
                                if (data[i].is_replied == true) {
                                    metafield += "<div class=\"dvhrv-prv-reply\">";
                                    metafield += "<p style='color:#8B8B8B'>";
                                    metafield += "<b>Admin</b> - " + data[i].reply_date;
                                    metafield += "</p>";
                                    metafield += "<p>";
                                    metafield += data[i].reply_content;
                                    metafield += "</p>";
                                    metafield += "</div>";
                                }
                                metafield += "<div style='display:none;'>";
                                metafield += "<span itemprop='itemreviewed'>" + data[i].product_name + "</span>";
                                metafield += "<span itemprop='rating'>" + data[i].star_rate + "</span>";
                                metafield += "</div>";
                                metafield += "</div>";
                                jQuery('#hrv-product-reviews-list').append(metafield);
                            }

                        Act.LoadStar();
                    }
                });
                PageCurrent = parseInt(page, 10);
                Act.LoadPagingTool();
                jQuery("#hrv-product-reviews-thanks").hide();
                Act.GoToByScroll('hrv-product-reviews');
            },
            GoToByScroll: function goToByScroll(id) {
                id = id.replace("link", "");
                jQuery('html,body').animate({
                    scrollTop: jQuery("#" + id).offset().top
                }, 'slow');
            },
            ResetForm: function () {
                jQuery('#dvBody_character').val("");
                jQuery("#hrv_product_reviews_form_name").val("");
                jQuery("#hrv_product_reviews_form_email").val("");
                jQuery("#hrv_product_reviews_form_title").val("");
                jQuery("#hrv_product_reviews_form_body").val("");
                Act.LoadDefaultStar();
            },

            LoadStar: function () {
                jQuery(".hrv-product-reviews-star").raty({
                    starType: 'i',
                    number: function () {
                        return jQuery(this).attr('data-number');
                    },
                    score: function () {
                        return jQuery(this).attr('data-score');
                    },
                    readOnly: true
                });
                jQuery(".hrv-product-reviews-star").css("color", StarColor);
                jQuery(window).resize();
            },

            LoadDefaultStar: function () {
                jQuery("#dvRating").raty({
                    starType: 'i',
                    number: 5,
                    score: 5,
                    click: function (score, evt) {
                        jQuery("#hrv_product_reviews_form_rate").val(score);
                    }
                });
                jQuery("#dvRating").css("color", StarColor);
                jQuery(window).resize();
            },

            IsValidEmailAddress: function (emailAddress) {
                var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                return regex.test(emailAddress);
            },

            ApplyLayoutSetting: function () {
                jQuery.ajax({
                    type: "GET",
                    url: Host + "configuration/getlayoutsetting",
                    dataType: 'json',
                    data: {
                        data: JSON.stringify({
                            hrv_product_reviews_form_shop: Haravan.shop,
                            hrv_product_reviews_form_fullshop: jQuery('#hrv_product_reviews_form_fullshop').val()
                        })
                    },
                    success: function (data) {
                    }
                }).done(function (data) {
                    StarColor = data.star_color;
                });
            },

            LoadNoReview: function () {
                jQuery(".haravan-product-reviews-badge").each(function () {
                    if (jQuery(this).children().length == 0) {
                        jQuery(this).append("<div class=\"hrv-product-reviews-star\" data-score=\"0\" data-number=\"5\" ></div>");
                        jQuery(this).append("</div><p>(0 đánh giá)</p></div>");
                    }
                });
                if (jQuery(".hrv-product-reviews-summary").children().length == 0) {
                    jQuery(".hrv-product-reviews-summary").append("<div style=\"text-align: center\"><div class=\"hrv-product-reviews-star\"  data-score=\"0\" data-number=\"5\" ></div><span>(0 đánh giá)</span></div>");
                }

            },

            LoadPagingTool: function () {
                jQuery.ajax({
                    type: "GET",
                    dataType: 'json',
                    url: Host + "clientrequest/getlistreviewpaginginfo",
                    data: {
                        data: JSON.stringify({
                            hrv_product_reviews_form_shop: jQuery('#hrv_product_reviews_form_shop').val(),
                            hrv_product_reviews_form_product: jQuery('#hrv_product_reviews_form_product').val(),
                            hrv_product_reviews_form_fullshop: jQuery('#hrv_product_reviews_form_fullshop').val(),
                            Take: Take
                        })
                    },
                    success: function (data) {

                    }
                }).done(function (data) {
                    var dvpaging = document.getElementById("dv-product-review-paging");

                    if (dvpaging != undefined) {
                        dvpaging.parentNode.removeChild(dvpaging);
                    }
                    var html = "";
                    if (data != null && data.number_of_page > 1) {


                        html += "<div id=\"dv-product-review-paging\">"
                        if (PageCurrent > 1) {
                            html += "<button class=\"btnPaging\" value ='" + 1 + "'>Trang Đầu</button>";
                            html += "<button class=\"btnPaging\" value = '" + (PageCurrent - 1) + "'>" + (PageCurrent - 1) + "</button>";
                        }

                        html += "<button class=\"btnPaging\" disabled value ='" + PageCurrent + "'>" + PageCurrent + "</button>";

                        if ((PageCurrent) < data.number_of_page) {
                            html += "<button class=\"btnPaging\" value ='" + (PageCurrent + 1) + "'>" + (PageCurrent + 1) + "</button>";
                            html += "<button class=\"btnPaging\" value ='" + data.number_of_page + "'>Trang Cuối</button>";
                        }
                        html += "</div>"
                        jQuery("#hrv-product-reviews-list").after(html);


                        jQuery(".btnPaging").each(function () {

                            jQuery(this).on("click", function () {
                                Act.GetReview(jQuery(this).val());
                            })
                        });

                    }

                });
            }
        }
    }();

    return {
        init: function () {
            GUI.LoadProductReviewCSS();
            GUI.LoadRatingCSS();
            GUI.CreateDivSummary();
            GUI.CreateSubmitForm();
            Act.ToogleSubmitForm();
            Act.ValidateSubmitForm();
            Act.ApplyLayoutSetting();
            Act.GetMoreReview();
            Act.LoadNoReview();
            Act.LoadStar();
            Act.LoadDefaultStar();
            Act.LoadPagingTool();
        },
        LoadStar: function () {
            Act.LoadStar();
        },
        LoadNoReview: function () {
            Act.LoadNoReview();
        }
    }
}();

function url_domain(data) {
    var a = document.createElement('a');
    a.href = data;
    return a.protocol + "//" + a.host + "/";
}

function LoadCSS(url) {
    var link = document.createElement('link');
    link.rel = 'stylesheet';
    link.type = 'text/css';
    link.href = url;
    link.media = 'all';

    var head = document.getElementsByTagName('head')[0];
    head.appendChild(link);
}

function LoadScript(url) {
    var link = document.createElement('script');
    link.type = 'text/javascript';
    link.src = url;
    var head = document.getElementsByTagName('head')[0];
    head.appendChild(link);
}

var jquery_raty_js_path = "content/scripts/jquery.raty.js";
jQuery.getScript(Host + jquery_raty_js_path).done(function () {
    ProductReviews.init();
});