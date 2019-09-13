/* Jonathan Snook - MIT License - https://github.com/snookca/prepareTransition */
(function(a){a.fn.prepareTransition=function(){return this.each(function(){var b=a(this);b.one("TransitionEnd webkitTransitionEnd transitionend oTransitionEnd",function(){b.removeClass("is-transitioning")});var c=["transition-duration","-moz-transition-duration","-webkit-transition-duration","-o-transition-duration"];var d=0;a.each(c,function(a,c){d=parseFloat(b.css(c))||d});if(d!=0){b.addClass("is-transitioning");b[0].offsetWidth}})}})(jQuery);

/* replaceUrlParam - http://stackoverflow.com/questions/7171099/how-to-replace-url-parameter-with-javascript-jquery */
function replaceUrlParam(e,r,a){var n=new RegExp("("+r+"=).*?(&|$)"),c=e;return c=e.search(n)>=0?e.replace(n,"$1"+a+"$2"):c+(c.indexOf("?")>0?"&":"?")+r+"="+a};

/*============================================================================
  Money Format
  - Haravan.format money is defined in option_selection.js.
    If that file is not included, it is redefined here.
==============================================================================*/
if ((typeof Haravan) === 'undefined') { Haravan = {}; }
if (!Haravan.formatMoney) {
	Haravan.formatMoney = function(cents, format) {
		var value = '',
				placeholderRegex = /\{\{\s*(\w+)\s*\}\}/,
				formatString = (format || this.money_format);

		if (typeof cents == 'string') {
			cents = cents.replace('.','');
		}

		function defaultOption(opt, def) {
			return (typeof opt == 'undefined' ? def : opt);
		}

		function formatWithDelimiters(number, precision, thousands, decimal) {
			precision = defaultOption(precision, 2);
			thousands = defaultOption(thousands, ',');
			decimal   = defaultOption(decimal, '.');

			if (isNaN(number) || number == null) {
				return 0;
			}

			number = (number/100.0).toFixed(precision);

			var parts   = number.split('.'),
					dollars = parts[0].replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1' + thousands),
					cents   = parts[1] ? (decimal + parts[1]) : '';

			return dollars + cents;
		}

		switch(formatString.match(placeholderRegex)[1]) {
			case 'amount':
				value = formatWithDelimiters(cents, 2);
				break;
			case 'amount_no_decimals':
				value = formatWithDelimiters(cents, 0);
				break;
			case 'amount_with_comma_separator':
				value = formatWithDelimiters(cents, 2, '.', ',');
				break;
			case 'amount_no_decimals_with_comma_separator':
				value = formatWithDelimiters(cents, 0, '.', ',');
				break;
		}

		return formatString.replace(placeholderRegex, value);
	};
}


