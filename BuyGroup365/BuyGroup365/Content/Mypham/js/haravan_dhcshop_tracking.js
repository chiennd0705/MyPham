var SHOP_ID = "4848682005209064613";

var AT = {
    JET_LAG: 200, // 200 miniseconds
    cookie_domain: null,
    cookie_duration: 30, // 30 days

    track: function () {
        var sid = this.get_param("aff_sid");
        var utm_source = this.get_param("utm_source");
        var get_utm_source = this.get_cookie("_aff_network")
        var get_aff_sid = this.get_cookie("_aff_sid")

        if(!get_utm_source && utm_source){
            console.log("[AT] Firs click with Affilates... ");
            this.set_cookie("_aff_network", utm_source, this.cookie_duration);
            if (sid) {
                this.set_cookie("_aff_sid", sid, this.cookie_duration);
            }else{
                this.set_cookie("_aff_sid", null, this.cookie_duration);
            }
        }

        if(utm_source == "accesstrade" && get_utm_source == "accesstrade") {
            console.log("[AT] Last click with Pub of Accesstrade... ");
            this.set_cookie("_aff_network", "accesstrade", this.cookie_duration);
            if (sid) {
                this.set_cookie("_aff_sid", sid, this.cookie_duration);
            }
        }
    },

    track_haravan_thank_you_page: function () {
        console.log("[AT] Tracking conversion...");
        var pathArray = window.location.pathname.split('/');
        var last_part = pathArray[pathArray.length - 1];
        var get_utm_source = this.get_cookie("_aff_network");

        if (last_part == 'thank_you' && get_utm_source=="accesstrade") {

            var sid = this.get_cookie('_aff_sid');
            if (!sid) {
                console.log("[AT] There is no session id found");
                return false;
            }

            if (typeof Haravan.checkout === "undefined") {
                console.log("[AT] There is no Haravan order info");
                return false;
            }

            var identifier = Haravan.checkout.order_id;
            var param = [];
            param.push("identifier=" + encodeURIComponent(identifier));
            param.push("shop_id=" + encodeURIComponent(SHOP_ID));
            param.push("sid=" + encodeURIComponent(sid));
            var a = document.createElement("img");
                a.width = 1;
                a.height = 1;
                a.border = 0;
                a.src = "https://partner.accesstrade.vn/haravan/tracking?" + param.join("&");
                console.log("[AT] Pixel tracking url: " + a.src);
                document.body.appendChild(a);
        }
    },

    get_param: function (name, url) {
        if (!url) url = location.href;
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(url);
        return results == null ? null : results[1];
    },

    set_cookie: function (n, v, e) {
        var d = new Date();
        d.setTime(d.getTime() + (e * 24 * 60 * 60 * 1000));
        var ee = "expires=" + d.toUTCString();
        cookie_domain = this.cookie_domain || window.location.hostname;
        document.cookie = n + "=" + v + "; " + ee + ";domain=" + cookie_domain + "; path=/";
    },

    get_cookie: function (cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1);
            if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
        }
        return null;
    }
};

AT.track();
AT.track_haravan_thank_you_page();