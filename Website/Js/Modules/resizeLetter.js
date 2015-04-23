NB.Behaviors.resizeLetter = function (that) {
    var param = that.data('param') || {};
    var methods = this;
    param.coeff = param.coeff || 1;

    this.init = function () {
        if (typeof that.data('sizes') !== 'undefined')
            return false;

        that.data({
            sizes: {
                min: that.css('font-size'),
                max: 2,
                actual: 0
            }
        }).find(param.plus).on('click', function (e) {
            e.preventDefault();
            if (that.data('sizes').actual === that.data('sizes').max)
                return false;

            that.data('sizes').actual++;
            that.css('font-size', '+=' + param.coeff);
            $('body').trigger('realignBoxes');
        }).end()
            .find(param.minus).on('click', function (e) {
                e.preventDefault();
                if (parseInt(that.data('sizes').min) < parseInt(that.css('font-size'))) {
                    that.css('font-size', '-=' + param.coeff);
                    that.data('sizes').actual--;
                } else {
                    that.css('font-size', that.data('sizes').min);
                    that.data('sizes').actual = 0; 
                }
                $('body').trigger('realignBoxes');
            });
    };

    methods.init();
};
NB.Behaviors.resizeLetter.param = '';