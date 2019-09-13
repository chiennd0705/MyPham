var engine = null;
if (window.navigator.appName == "Microsoft Internet Explorer") {
    // This is an IE browser. What mode is the engine in?
    if (document.documentMode) // IE8 or later
    {
        engine = document.documentMode;
    }
    else // IE 5-7
    {
        engine = 5; // Assume quirks mode unless proven otherwise
        if (document.compatMode) {
            if (document.compatMode == "CSS1Compat")
                engine = 7; // standards mode
        }
        // There is no test for IE6 standards mode because that mode
        // was replaced by IE7 standards mode; there is no emulation.
    }
}
var ieUserAgent = {
    init: function () {
        // Get the user agent string
        var ua = navigator.userAgent;
        this.compatibilityMode = false;

        // Detect whether or not the browser is IE
        var ieRegex = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (ieRegex.exec(ua) == null)
            this.exception = "The user agent detected does not contai Internet Explorer.";

        // Get the current "emulated" version of IE
        this.renderVersion = parseFloat(RegExp.$1);
        this.version = this.renderVersion;

        // Check the browser version with the rest of the agent string to detect compatibility mode
        if (ua.indexOf("Trident/6.0") > -1) {
            if (ua.indexOf("MSIE 7.0") > -1) {
                this.compatibilityMode = true;
                this.version = 10;                  // IE 10
            }
        }
        else if (ua.indexOf("Trident/5.0") > -1) {
            if (ua.indexOf("MSIE 7.0") > -1) {
                this.compatibilityMode = true;
                this.version = 9;                   // IE 9
            }
        }
        else if (ua.indexOf("Trident/4.0") > -1) {
            if (ua.indexOf("MSIE 7.0") > -1) {
                this.compatibilityMode = true;
                this.version = 8;                   // IE 8
            }
        }
        else if (ua.indexOf("MSIE 7.0") > -1)
            this.version = 7;                       // IE 7
        else
            this.version = 6;                       // IE 6
    }
};
ieUserAgent.init();
var isSafari = /Safari/.test(navigator.userAgent) && /Apple Computer/.test(navigator.vendor);
function checkSupportBrowser() {
    if (engine != null && (engine == 5 || ieUserAgent.version != engine)) {
        var elem = document.getElementsByTagName("body")[0];
        elem.parentElement.removeChild(elem);
        //var child = document.createElement('div');
        //child.innerHTML = htmlNotSupport();
        //child = child.firstChild;
        //elem.parentElement.appendChild(child);
    }
}