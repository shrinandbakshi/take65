NB.Behaviors.placeholder = function (that) {
    var param = that.data('param') || {};
    var methods = this;

    this.init = function () {
        if (!Modernizr.placeholder)
            $('input, textarea').placeholder();
    };

    methods.init();
};
NB.Behaviors.placeholder.param = '';