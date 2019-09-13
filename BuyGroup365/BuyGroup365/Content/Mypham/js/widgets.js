/*
 onDomReady, Copyright © 2010 Jakob Mattsson
 This is a small implementation of an onDomReady-function, for situations when frameworks are no-no.
 It's loosely based on jQuery's implementation of $.ready, Copyright (c) 2010 John Resig, http://jquery.com/
 */

(function() {

    var onDomReadyIdentifier = 'onDomReady';
    var isBound = false;
    var readyList = [];

    if (window[onDomReadyIdentifier] && typeof window[onDomReadyIdentifier] == 'function') {
        return;
    }

    var whenReady = function() {
        // Make sure body exists, at least, in case IE gets a little overzealous.
        // This is taked directly from jQuery's implementation.
        if (!document.body) {
            return setTimeout(whenReady, 13);
        }

        for (var i=0; i<readyList.length; i++) {
            readyList[i]();
        }
        readyList = [];
    };

    var bindReady = function() {
        // Mozilla, Opera and webkit nightlies currently support this event
        if (document.addEventListener) {
            var DOMContentLoaded = function() {
                document.removeEventListener("DOMContentLoaded", DOMContentLoaded, false);
                whenReady();
            };

            document.addEventListener("DOMContentLoaded", DOMContentLoaded, false);
            window.addEventListener("load", whenReady, false); // fallback

            // If IE event model is used
        } else if (document.attachEvent) {

            var onreadystatechange = function() {
                if (document.readyState === "complete") {
                    document.detachEvent("onreadystatechange", onreadystatechange);
                    whenReady();
                }
            };

            document.attachEvent("onreadystatechange", onreadystatechange);
            window.attachEvent("onload", whenReady); // fallback

            // If IE and not a frame, continually check to see if the document is ready
            var toplevel = false;

            try {
                toplevel = window.frameElement == null;
            } catch(e) {}

            // The DOM ready check for Internet Explorer
            if (document.documentElement.doScroll && toplevel) {
                var doScrollCheck = function() {

                    // stop searching if we have no functions to call
                    // (or, in other words, if they have already been called)
                    if (readyList.length == 0) {
                        return;
                    }

                    try {
                        // If IE is used, use the trick by Diego Perini
                        // http://javascript.nwbox.com/IEContentLoaded/
                        document.documentElement.doScroll("left");
                    } catch(e) {
                        setTimeout(doScrollCheck, 1);
                        return;
                    }

                    // and execute any waiting functions
                    whenReady();
                }
                doScrollCheck();
            }
        }
    };

    window[onDomReadyIdentifier] = function(callback) {
        // Push the given callback onto the list of functions to execute when ready.
        // If the dom has alredy loaded, call 'whenReady' right away.
        // Otherwise bind the ready-event if it hasn't been done already
        readyList.push(callback);
        if (document.readyState == "complete") {
            whenReady();
        } else if (!isBound) {
            bindReady();
            isBound = true;
        }
    };
})();
//------------------------- End onDomReady -----------------

onDomReady(function(){
    window.SweetTooth = {config: {}, initializers: [], version: "0.1.1"}, function () {
        function e(e, t) {
            var n = t({});
            e.subscribe = function () {
                n.on.apply(n, arguments)
            }, e.unsubscribe = function () {
                n.off.apply(n, arguments)
            }, e.publish = function () {
                n.trigger.apply(n, arguments)
            }
        }

        var t = function (t, n) {
            t.queryParams = function (e) {
                var t, n, o = {};
                if ("" === e)return {};
                for (t = 0; t < e.length; t += 1)n = e[t].split("="), 2 === n.length && (o[n[0]] = decodeURIComponent(n[1].replace(/\+/g, " ")));
                return o
            }(window.location.search.substr(1).split("&")), t.parseIntent = function (e) {
                var t = e.split(":");
                return {org: t[0], app: t[1], resource: t[2], resourceId: t[3]}
            }, t.intent = function (e) {
                t.publish("st-intent", e)
            }, e(t, n)
        };
        SweetTooth.initializers.push(t)
    }(), function () {
        var e = function (e, t) {
            var n = t("<link>", {
                rel: "stylesheet",
                type: "text/css",
                href: "//onapp.haravan.com/loyalty/static/stylesheets/widgets-main.css?shopname=dhcshop.myharavan.com"
            });
            n.appendTo("head")
        };
        SweetTooth.initializers.push(e)
    }(), function () {
        var e;
        e = function (e) {
            var t, n;
            return t = !1, e(function () {
                var o;
                return o = (document.body || document.documentElement).style, t = void 0 !== o.animation || void 0 !== o.WebkitAnimation || void 0 !== o.MozAnimation || void 0 !== o.MsAnimation || void 0 !== o.OAnimation, e(window).bind("keyup.vex", function (e) {
                    return 27 === e.keyCode ? n.closeByEscape() : void 0
                })
            }), n = {
                globalID: 1,
                animationEndEvent: "animationend webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend",
                baseClassNames: {
                    vex: "vex",
                    content: "vex-content",
                    overlay: "vex-overlay",
                    close: "vex-close",
                    closing: "vex-closing",
                    open: "vex-open"
                },
                defaultOptions: {
                    content: "",
                    showCloseButton: !0,
                    escapeButtonCloses: !0,
                    overlayClosesOnClick: !0,
                    appendLocation: "body",
                    className: "",
                    css: {},
                    overlayClassName: "",
                    overlayCSS: {},
                    contentClassName: "",
                    contentCSS: {},
                    closeClassName: "",
                    closeCSS: {}
                },
                open: function (t) {
                    return t = e.extend({}, n.defaultOptions, t), t.id = n.globalID, n.globalID += 1, t.$vex = e("<div>").addClass(n.baseClassNames.vex).addClass(t.className).css(t.css).data({vex: t}), t.$vexOverlay = e("<div>").addClass(n.baseClassNames.overlay).addClass(t.overlayClassName).css(t.overlayCSS).data({vex: t}), t.overlayClosesOnClick && t.$vexOverlay.bind("click.vex", function (t) {
                        return t.target === this ? n.close(e(this).data().vex.id) : void 0
                    }), t.$vex.append(t.$vexOverlay), t.$vexContent = e("<div>").addClass(n.baseClassNames.content).addClass(t.contentClassName).css(t.contentCSS).append(t.content).data({vex: t}), t.$vex.append(t.$vexContent), t.showCloseButton && (t.$closeButton = e("<div>").addClass(n.baseClassNames.close).addClass(t.closeClassName).css(t.closeCSS).data({vex: t}).bind("click.vex", function () {
                        return n.close(e(this).data().vex.id)
                    }), t.$vexContent.append(t.$closeButton)), e(t.appendLocation).append(t.$vex), n.setupBodyClassName(t.$vex), t.afterOpen && t.afterOpen(t.$vexContent, t), setTimeout(function () {
                        return t.$vexContent.trigger("vexOpen", t)
                    }, 0), t.$vexContent
                },
                getSelectorFromBaseClass: function (e) {
                    return "." + e.split(" ").join(".")
                },
                getAllVexes: function () {
                    return e("." + n.baseClassNames.vex + ':not(".' + n.baseClassNames.closing + '") ' + n.getSelectorFromBaseClass(n.baseClassNames.content))
                },
                getVexByID: function (t) {
                    return n.getAllVexes().filter(function () {
                        return e(this).data().vex.id === t
                    })
                },
                close: function (e) {
                    var t;
                    if (!e) {
                        if (t = n.getAllVexes().last(), !t.length)return !1;
                        e = t.data().vex.id
                    }
                    return n.closeByID(e)
                },
                closeAll: function () {
                    var t;
                    return t = n.getAllVexes().map(function () {
                        return e(this).data().vex.id
                    }).toArray(), (null != t ? t.length : void 0) ? (e.each(t.reverse(), function (e, t) {
                        return n.closeByID(t)
                    }), !0) : !1
                },
                closeByID: function (o) {
                    var s, a, i, r, l;
                    return a = n.getVexByID(o), a.length ? (s = a.data().vex.$vex, l = e.extend({}, a.data().vex), i = function () {
                        return l.beforeClose ? l.beforeClose(a, l) : void 0
                    }, r = function () {
                        return a.trigger("vexClose", l), s.remove(), e("body").trigger("vexAfterClose", l), l.afterClose ? l.afterClose(a, l) : void 0
                    }, t ? (i(), s.unbind(n.animationEndEvent).bind(n.animationEndEvent, function () {
                        return r()
                    }).addClass(n.baseClassNames.closing)) : (i(), r()), !0) : void 0
                },
                closeByEscape: function () {
                    var t, o, s;
                    return s = n.getAllVexes().map(function () {
                        return e(this).data().vex.id
                    }).toArray(), (null != s ? s.length : void 0) ? (o = Math.max.apply(Math, s), t = n.getVexByID(o), t.data().vex.escapeButtonCloses !== !0 ? !1 : n.closeByID(o)) : !1
                },
                setupBodyClassName: function () {
                    return e("body").bind("vexOpen.vex", function () {
                        return e("body").addClass(n.baseClassNames.open)
                    }).bind("vexAfterClose.vex", function () {
                        return n.getAllVexes().length ? void 0 : e("body").removeClass(n.baseClassNames.open)
                    })
                },
                hideLoading: function () {
                    return e(".vex-loading-spinner").remove()
                },
                showLoading: function () {
                    return n.hideLoading(), e("body").append('<div class="vex-loading-spinner ' + n.defaultOptions.className + '"></div>')
                }
            }
        };
        var t = function (t, n) {
            t.vex = e(n), t.vex.defaultOptions.className = "vex-theme-plain"
        };
        SweetTooth.initializers.push(t)
    }.call(this), function () {
        var e = function (e) {
            e.subscribe("st-intent", function (t, n) {
                var params = '';
                if (Haravan.ProductURL && Haravan.ProductId) {
                    params = '&purl=' + Haravan.ProductURL + '&pid=' + Haravan.ProductId;
                }

                var o = e.parseIntent(n), s = e.config.widgetBaseUrl, a = e.config.customerToken, i = "string" == typeof a ? "?ct=" + a : "";
                "st:core:home" === n ? e.publish("open-modal", {url: s + i + params}) : -1 !== n.indexOf("st:referrals:customer-offers:") ? e.publish("open-modal", {url: s + i + params}) : "st:points:rules" === n ? e.publish("open-modal", {url: s + i + params}) : "st:points:rewards" === n ? e.publish("open-modal", {url: s + i + params}) : "st:referrals:offers" === n ? e.publish("open-modal", {url: s + i + params}) : console.error("Invalid Sweet Tooth intent: " + n)
            })
        };
        SweetTooth.initializers.push(e)
    }(), function () {
        var e = function (e) {
            var t = e.vex;
            e.subscribe("open-modal", function (e, n) {
                var o = n.url;
                return "string" != typeof o || 0 === o.length ? (console.err('No url attribute defined for data-toggle="points-store-modal" element. Cannot open iframe with no url.'), !1) : void t.open({
                    content: '<iframe allowtransparency="true"  class="st-modal-iframe" style=" background-color: transparent;width: 100%; height: 100%; border: 0;" src="' + o + '"></iframe>',
                    contentCSS: {width: n.width, height: n.height, padding: 0}
                });
            });
        };
        SweetTooth.initializers.push(e)
    }(), function () {
        var e = function (e, t) {
            if (widgets = t(".sweettooth-widget"), valid_widget_types = ["tab"], 0 !== widgets.length && (widget = widgets.first(), widget_type = widget.attr("data-widget-type"), -1 != t.inArray(widget_type, valid_widget_types))) {
                var n = "//onapp.haravan.com/loyalty/assets/widgetstab.js?shopname=dhcshop.myharavan.com";
                t.ajax({
                    url: n, data: widget.data(), dataType: "jsonp", success: function (e) {
                        t("body").append(e.template)
                    }
                })
            }
        };
        SweetTooth.initializers.push(e)
    }(), function () {
        function e(e, t) {
            function n() {
                a || (a = !0, t(e, "ok"))
            }

            function o() {
                var e;
                a || (e = i.readyState, "complete" === e && n())
            }

            function s() {
                a || (a = !0, t(e, "error"))
            }

            var a = !1, i = document.createElement("script");
            i.type = "text/javascript", t && (i.onload = n, i.onreadystatechange = o, i.onerror = s), i.src = e, (document.getElementsByTagName("head")[0] || document.documentElement).appendChild(i)
        }

        var t = window.SweetTooth;
        e("", function () {
            t.jquery = jQuery, t.$ = t.jquery, t.initializers.forEach(function (e) {
                e(t, t.$)
            })
        })

    }();

});

