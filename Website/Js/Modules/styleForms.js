NB.Behaviors.styleForms = function (that) {
    var param = that.data('param') || {};

    if ($.browser.msie && $.browser.version < 8) {
        this.reinitForm = function () { };
    } else {
        this.reinitForm = function () {
            styleForm.reformSelect(that);
            //styleForm.reloadSelect(that);
        }

        if(window['styleForm'] !== 'undefined')
            styleForm.init(that);
    }
}

NB.Behaviors.styleForms.version = '0.1';