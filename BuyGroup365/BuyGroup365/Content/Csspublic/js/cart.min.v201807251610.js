
$(".choosenumber").click(function (n) {
    var t = this;
    if (!t._isProcessing) {
        if (n) var i = n.find(t.opts.augmentSelector),
            r = n.find(t.opts.abateSelector),
            u = n.find(t.opts.augmentColorSelector),
            f = n.find(t.opts.abateColorSelector);
        else var i = t.get("el", t.opts.augmentSelector),
            r = t.get("el", t.opts.abateSelector),
            u = t.get("el", t.opts.augmentColorSelector),
            f = t.get("el", t.opts.abateColorSelector);
        i.on("click", function () {
            var n = $(this).parent(".choosenumber"),
                r = n.find("input[type='hidden']"),
                i = parseInt(r.val()) + 1;
            r.val(i);
            n.find(".number").html(i);
            i <= 1 && (n.find(".abate").removeClass("active"), n.find(".augment ").removeClass("disable"));
            i > 1 && i < 10 ? (n.find(".abate").addClass("active"), n.find(".augment ").removeClass("disable")) : i >= 10 && (n.find(".abate").addClass("active"), n.find(".augment ").addClass("disable"));
            t.recalc()
        });
        r.on("click", function () {
            var n = $(this).parent(".choosenumber"),
                r = n.find("input[type='hidden']"),
                i = parseInt(r.val()) - 1;
            r.val(i);
            n.find(".number").html(i);
            i <= 1 && (n.find(".abate").removeClass("active"), n.find("augment ").addClass("active"));
            i > 1 && i < 10 ? (n.find(".abate").addClass("active"), n.find(".augment ").removeClass("disable")) : i >= 10 && (n.find(".abate").addClass("active"), n.find(".augment ").addClass("disable"));
            t.recalc()
        });
        u.on("click", function () {
            var n = $(this).parent(),
                i = n.find("input[type='hidden']"),
                t;
            i.val() != 10 && (t = parseInt(i.val()) + 1, i.val(t), n.find(".number").html(t), t <= 0 && (n.find(".abate").removeClass("active"), n.find(".augment ").removeClass("disable")), t > 0 && t < 10 ? (n.find(".abate").addClass("active"), n.find(".augment ").removeClass("disable")) : t >= 10 && (n.find(".abate").addClass("active"), n.find(".augment ").addClass("disable")))
        });
        f.on("click", function () {
            var n = $(this).parent(),
                i = n.find("input[type='hidden']"),
                t;
            i.val() != 0 && (t = parseInt(i.val()) - 1, i.val(t), n.find(".number").html(t), t <= 0 && (n.find(".abate").removeClass("active"), n.find("augment ").addClass("active")), t > 0 && t < 10 ? (n.find(".abate").addClass("active"), n.find(".augment ").removeClass("disable")) : t >= 10 && (n.find(".abate").addClass("active"), n.find(".augment ").addClass("disable")))
        })
    }
});