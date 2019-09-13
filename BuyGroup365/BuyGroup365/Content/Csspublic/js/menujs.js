function $readyMenu() {
    function n(n) {
        var u = $(this),
            i = null,
            r = [],
            f = null,
            e = null,
            t = $.extend({
                rowSelector: "> li",
                submenuSelector: "*",
                submenuDirection: "right",
                tolerance: 75,
                enter: $.noop,
                exit: $.noop,
                activate: $.noop,
                deactivate: $.noop,
                exitMenu: $.noop
            }, n),
            h = 3,
            c = 300,
            l = function (n) {
                r.push({
                    x: n.pageX,
                    y: n.pageY
                });
                r.length > h && r.shift()
            },
            a = function () {
                e && clearTimeout(e);
                t.exitMenu(this) && (i && t.deactivate(i), i = null)
            },
            v = function () {
                e && clearTimeout(e);
                t.enter(this);
                s(this)
            },
            y = function () {
                t.exit(this)
            },
            p = function () {
                o(this)
            },
            o = function (n) {
                n != i && (i && t.deactivate(i), t.activate(n), i = n)
            },
            s = function (n) {
                var t = w();
                t ? e = setTimeout(function () {
                    s(n)
                }, t) : o(n)
            },
            w = function () {
                function a(n, t) {
                    return (t.y - n.y) / (t.x - n.x)
                }
                var s, h;
                if (!i || !$(i).is(t.submenuSelector)) return 0;
                var n = u.offset(),
                    v = {
                        x: n.left,
                        y: n.top - t.tolerance
                    },
                    p = {
                        x: n.left + u.outerWidth(),
                        y: v.y
                    },
                    y = {
                        x: n.left,
                        y: n.top + u.outerHeight() + t.tolerance
                    },
                    l = {
                        x: n.left + u.outerWidth(),
                        y: y.y
                    },
                    o = r[r.length - 1],
                    e = r[0];
                if (!o || (e || (e = o), e.x < n.left || e.x > l.x || e.y < n.top || e.y > l.y) || f && o.x == f.x && o.y == f.y) return 0;
                s = p;
                h = l;
                t.submenuDirection == "left" ? (s = y, h = v) : t.submenuDirection == "below" ? (s = l, h = y) : t.submenuDirection == "above" && (s = v, h = p);
                var w = a(o, s),
                    b = a(o, h),
                    k = a(e, s),
                    d = a(e, h);
                return w < k && b > d ? (f = o, c) : (f = null, 0)
            };
        u.mouseleave(a).find(t.rowSelector).mouseenter(v).mouseleave(y).click(p);
        $(document).mousemove(l)
    }
    $.fn.menuAim = function (t) {
        return this.each(function () {
            n.call(this, t)
        }), this
    }
}

function wait$() {
    if (typeof $ == "undefined") {
        setTimeout(wait$, 10);
        return
    }
    globalEvent.emit("jqready")
}

function $ready() {
    $.ajaxSetup({
        cache: !1
    });
    $.ajaxPrefilter(function (n, t) {
        var i = getUrlParam("clearcache");
        if (i != null && i != "") switch (typeof t.data) {
            case "undefined":
                n.data = "clearcache=" + i;
                break;
            case "string":
                n.data = t.data + "&clearcache=" + i;
                break;
            default:
                n.data = $.param($.extend(t.data, {
                    clearcache: i
                }))
        }
    });
    $("#tomobile").attr("href", location.pathname + (location.search === "" ? "?view=mobile" : location.search + "&view=mobile"));
    provincesBox();
    corebrain();
    $("#main-search").submit(function (n) {
        typeof IsSearchAccessories == "undefined" && (n.preventDefault(), typeof AutoComplete != "undefined" && AutoComplete.prototype.goToSearchPage($("#skw").val()))
    });
    $(window).load(function () {
        initMenu();
    });
    goTopdmx();
    $(window).on("scroll", function () {
        document.footerStoresLoaded || $(this).scrollTop() + 1500 >= document.body.scrollHeight && (document.footerStoresLoaded = !0, $.get("/webapi/BannerPromote", {
            provinceId: document.provinceId
        }, function (n) {
            $("#bn-promote").html(n)
        }))
    })
}

function showmorefootlink(n) {
    $(n).parents(".colfoot").find(".hidden").show();
    $(n).parent().remove()
}

function goTopdmx() {
    $("#gb-top-page").length && ($("#gb-top-page").hide(), $(window).scroll(function () {
        $(this).scrollTop() > 100 ? $("#gb-top-page").fadeIn() : $("#gb-top-page").fadeOut()
    }), $("#gb-top-page").click(function () {
        return $("body,html").animate({
            scrollTop: 0
        }, 800), !1
    }))
}

function slugify(n) {
    return n.toLowerCase().replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a").replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e").replace(/ì|í|ị|ỉ|ĩ/g, "i").replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o").replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u").replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y").replace(/đ/g, "d").replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, "-").replace(/\s+/g, "-").replace(/[^\w\-]+/g, "").replace(/\-\-+/g, "-").replace(/^-+/, "").replace(/-+$/, "")
}

function suggestSearch(n) {
    n.preventDefault();
    var t = "#search-result",
        i = $("#skw").val().replace(/:|;|!|@@|#|\$|%|\^|&|\*|'|"|>|<|,|\.|\?|`|~|\+|=|_|\(|\)|{|}|\[|\]|\\|\|/gi, ""),
        r = i.trim().toLowerCase();
    if (r.length < MIN_SSKEYWORD_LENGTH) {
        $(t).html("");
        return
    }
    if (n.which === 40) return $(t + " ul li.active").length == 0 ? $(t + " ul li:first").addClass("active") : $(t + " ul li.active").removeClass("active").next().addClass("active"), !1;
    if (n.which === 38) return $(t + " ul li.active").length == 0 ? $(t + " ul li:last").addClass("active") : $(t + " ul li.active").removeClass("active").prev().addClass("active"), !1;
    if (n.which === 13 && $(t + " ul li.active").length > 0) return n.preventDefault(), window.location = $(t + " ul li.active a").attr("href"), !1;
    n.type == "submit" ? goToSearchPage(slugify(i)) : typeof IsSearchAccessories == "undefined" && location.href.indexOf("may-doi-tra") < 0 && (timmer = setTimeout(function () {
        callSuggestSearch(n)
    }, 600))
}

function callSuggestSearch(n) {
    var t;
    n.preventDefault();
    t = "#search-result";
    $(t + " ul li.selected").length > 0 && (window.location = $(t + " ul li.selected a").attr("href"));
    var u = $("#skw").val().replace(/:|;|!|@@|#|\$|%|\^|&|\*|'|"|>|<|,|\.|\?|`|~|\+|=|_|\(|\)|{|}|\[|\]|\\|\|/gi, ""),
        i = u.trim().toLowerCase(),
        r = $("#search-result");
    if (i.length < MIN_SSKEYWORD_LENGTH) {
        r.html("");
        return
    }
    i.length >= MIN_SSKEYWORD_LENGTH && $.get("/webapi/suggestsearch", {
        keywords: i,
        provinceId: document.provinceId,
        categoryId: "-1"
    }, function (n) {
        clearTimeout(timmer);
        r.html(n)
    })
}

function getAutocomplete() {
    typeof AutoComplete == "undefined" ? include(document.cdn + "/Scripts/global/autocomplete" + document.minv + ".js", function () {
        globalEvent.emit("autocomplete")
    }) : globalEvent.emit("autocomplete")
}

function getUrlParam(n, t) {
    t || (t = window.location.href);
    n = n.replace(/[\[\]]/g, "\\$&");
    var r = new RegExp("[?&]" + n + "(=([^&#]*)|&|#|$)"),
        i = r.exec(t);
    return i ? i[2] ? decodeURIComponent(i[2].replace(/\+/g, " ")) : "" : null
}

function provincesBox() {
    var n = JSON.parse(monster.get("DMX_Personal"));
    $(".provinces-box .dd ul > li").on("click", function () {
        $.post("/aj/Common/ChangeProvince", {
            id: $(this).data("id"),
            url: location.href
        }, function (n) {
            history.replaceState(null, null, n);
            window.location.reload(!0)
        })
    });
    $(".provinces-box .dd .close").on("click", function () {
        $(".provinces-box .dd").hide();
        $(".pddefault").remove();
        n.IsDefault && $.post("/aj/Common/ChangeProvince", {
            id: document.provinceId
        })
    });
    $(".provinces-box > span").mouseenter(function () {
        $(".provinces-box .dd").removeAttr("style")
    });
    $(".search-province").on("keyup", function () {
        _ = $(this);
        $(".provinces-box ul li").hide();
        $(".provinces-box ul li:icontains('" + _.val() + "')").show()
    });
    $.expr[":"].icontains = function (n, t, i) {
        return slugify($(n).text().toUpperCase()).match("^" + slugify(i[3].toUpperCase()))
    }
}

function corebrain() {
    var t, n, i;
    document.showCoreBrain != undefined && document.showCoreBrain && $("header .cart").length > 0 && monster.get("DMX_CBCart") != undefined && (t = monster.get("DMX_CBCart"), n = JSON.parse(t), n != null && Object.keys(n).length > 0 && (i = Object.keys(n.BasicData).length, $("#cbCount").text(i), i > 0 ? ($("header #cart-box").removeClass("hidecart"), $("header #main-search").removeClass("ncart"), $.get("/webapi/corebrain/get", {
        data: t,
        provinceId: document.provinceId
    }, function (n) {
        $(document.head).append(n["<Css>k__BackingField"]);
        $("#cart-box").append(n["<Html>k__BackingField"]);
        $(document.body).append(n["<Js>k__BackingField"]);
        monster.set("DMX_CBCart", n["<Json>k__BackingField"], 7)
    })) : ($("header #cart-box").addClass("hidecart"), $("header #main-search").addClass("ncart"))), $("#cart-box").hover(function () {
        $("#list-cart img[data-src]").each(function () {
            $(this).attr("src", $(this).attr("data-src")).removeAttr("data-src")
        })
    }))
}

function viewedproduct() {
    var n = "";
    n += '<div class="nopro">';
    n += "<img src='" + document.cdn + "/Content/images/2018/nopro.png' alt='Không có sản phẩm'>";
    n += "<span>Bạn chưa xem sản phẩm nào<\/span>";
    n += "<\/div>";
    $(".btnviewed").hover(function () {
        if ($("header .btnviewed").length > 0) {
            var t = monster.get("utm_thegioididong_viewedproduct");
            t.length > 0 ? $(".viewedproduct").html() == undefined && $.get("/webapi/viewedproduct", function (n) {
                $(".btnviewed").append(n)
            }) : $("header .nopro").html() == undefined && $(".btnviewed").append(n)
        } else $("header .nopro").html() == undefined && $(".btnviewed").append(n)
    })
}

function showLoading() {
    $("#loading").show();
    $(document.body).css({
        overflow: "hidden"
    })
}

function hideLoading() {
    $("#loading").hide();
    $(document.body).css({
        overflow: "auto"
    })
}

function showstablestore(n, t) {
    t === 1 ? window.location.href = $("#divfstore_" + n).find("a").attr("href") : $("#divfstore_" + n).toggle()
}

function initMenu() {
    function n(n) {
        var t = $(n),
            i = t.data("submenu-id"),
            r = $("#" + i),
            u = $menu.outerHeight(),
            f = $menu.outerWidth();
        $(".mainmenu li").removeClass("active");
        t.addClass("active");
        r.css({
            display: "block"
        });
        t.find("a").addClass("maintainHover")
    }

    function i(n) {
        var t = $(n),
            i = t.data("submenu-id"),
            r = $("#" + i);
        r.css("display", "none");
        $(".mainmenu li").removeClass("active");
        t.find("a").removeClass("maintainHover")
    }
    if ($menu = $("#menu2017"), $menu.menuAim({
        activate: n,
        deactivate: i,
        exitMenu: function () {
            $(".subcate").css("display", "none");
            $(".mainmenu li").removeClass("active");
            $("a.maintainHover").removeClass("maintainHover")
        },
        enter: function (t) {
            $(".maintainHover").length == 0 && ($(".subcate").hide(), n(t))
        }
    }), typeof isHome == "undefined" || isHome == !1) {
        var t = !1;
        $(".mainmenu").hover(function () {
            $menu.show()
        }, function () {
            t || $menu.hide()
        })
    }
}
var monster = {
    set: function (n, t, i, r, u) {
        var f = new Date,
            o = "",
            s = typeof t,
            e = "",
            h = "",
            c = location.host.replace("www", "").replace("beta", "");
        if (r = r || "/", i && (f.setTime(f.getTime() + i * 864e5), o = "; expires=" + f.toUTCString()), s === "object" && s !== "undefined") {
            if (!("JSON" in window)) throw "Bummer, your browser doesn't support JSON parsing.";
            e = encodeURIComponent(JSON.stringify({
                v: t
            }))
        } else e = encodeURIComponent(t);
        u && (h = "; secure");
        document.cookie = n + "=" + e + o + "; path=" + r + "; domain=" + c + h
    },
    get: function (n) {
        for (var t, f = n + "=", e = document.cookie.split(";"), i = "", o = "", r = {}, u = 0; u < e.length; u++) {
            for (t = e[u]; t.charAt(0) == " ";) t = t.substring(1, t.length);
            if (t.indexOf(f) === 0) {
                if (i = decodeURIComponent(t.substring(f.length, t.length)), o = i.substring(0, 1), o == "{") try {
                    if (r = JSON.parse(i), "v" in r) return r.v
                } catch (s) {
                    return i
                }
                return i == "undefined" ? undefined : i
            }
        }
        return null
    },
    remove: function (n) {
        this.set(n, "", -1)
    },
    increment: function (n, t) {
        var i = this.get(n) || 0;
        this.set(n, parseInt(i, 10) + 1, t)
    },
    decrement: function (n, t) {
        var i = this.get(n) || 0;
        this.set(n, parseInt(i, 10) - 1, t)
    }
},
    globalEvent, clear, timmer, MIN_SSKEYWORD_LENGTH;
smokesignals = {
    convert: function (n, t) {
        return t = {}, n.on = function (i, r) {
            return (t[i] = t[i] || []).push(r), n
        }, n.once = function (t, i) {
            function r() {
                i.apply(n.off(t, r), arguments)
            }
            r.h = i;
            return n.on(t, r)
        }, n.off = function (i, r) {
            for (var f = t[i], u = 0; r && f && f[u]; u++) f[u] != r && f[u].h != r || f.splice(u--, 1);
            return u || delete t[i], n
        }, n.emit = function (i) {
            for (var r = t[i], u = 0; r && r[u];) r[u++].apply(n, r.slice.call(arguments, 1));
            return n
        }, n
    }
};
include = function () {
    function e() {
        var n = this.readyState;
        (!n || /ded|te/.test(n)) && (t-- , !t && f && u())
    }
    var n = arguments,
        r = document,
        t = n.length,
        u = n[t - 1],
        f = u.call,
        i;
    for (f && t-- , i = 0; i < t; i++) n = r.createElement("script"), n.src = arguments[i], n.async = !0, n.onload = n.onerror = n.onreadystatechange = e, (r.head || r.getElementsByTagName("head")[0]).appendChild(n)
};
globalEvent = {};
smokesignals.convert(globalEvent);
globalEvent.on("jqready", $readyMenu);
(document.cookie.match(/DMX_CBCart/g) || []).length > 1 && (clear = "DMX_CBCart=; expires=Fri, 31 Dec 1999 23:59:59 GMT;", document.cookie = clear, document.cookie = clear + " domain=." + location.host + ";");
document.cdn == undefined && (document.cdn = "");
document.minv == undefined && (document.minv = "");
globalEvent.on("jqready", $ready);
wait$();
MIN_SSKEYWORD_LENGTH = 2;
slugify = function (n) {
    return n.toLowerCase().replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a").replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e").replace(/ì|í|ị|ỉ|ĩ/g, "i").replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o").replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u").replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y").replace(/đ/g, "d").replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, "-").replace(/\s+/g, "-").replace(/[^\w\-]+/g, "").replace(/\-\-+/g, "-").replace(/^-+/, "").replace(/-+$/, "")
};
goToSearchPage = function (n) {
    var i, t, u, r;
    t = slugify(n);
    u = ["op-lung", "bao-da", "pin", "sac-du-phong", "the-nho", "cap-sac", "tai-nghe", "usb", "mieng-dan", "chuot", "loa", "o-cam", "phu-kien", "hdmi"];
    r = !1;
    u.indexOf(t) != -1 && (r = !0);
    location.href.indexOf(".com/may-doi-tra") > 0 ? i = "/may-doi-tra/tim-kiem?kw=" + t.replace("-", "+") : location.href.indexOf(".com/kinh-nghiem-hay") > 0 ? i = "/hoi-dap/tim-kiem?key=" + t : typeof IsSearchAccessories != "undefined" || r ? (t == "bao-da" && (t = "op-lung"), i = "/phu-kien?key=" + t) : i = "/tag/" + t;
    location.href = i
};