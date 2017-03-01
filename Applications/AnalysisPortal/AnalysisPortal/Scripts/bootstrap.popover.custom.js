
if (!(jQuery.browser.mobile || (('ontouchstart' in window)
      || (navigator.MaxTouchPoints > 0)
      || (navigator.msMaxTouchPoints > 0)))) {
    //Make changes to bootstrap popover to prevent hiding when hover a popover whit links
    (function ($) {
        var oldHide = $.fn.popover.Constructor.prototype.hide;

        $.fn.popover.Constructor.prototype.hide = function () {
            if (this.options.trigger === "hover" && this.tip().is(":hover")) {
                var that = this;
                // try again after what would have been the delay
                setTimeout(function () {
                    return that.hide.call(that, arguments);
                }, that.options.delay.hide);
                return;
            }
           
            oldHide.call(this);
        };

    })(jQuery);
}