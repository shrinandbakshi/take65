NB.Behaviors.openPop = function (that) {
    //console.log(that.data('param'));
    //console.log(this);
    var param = that.data('param') || {};
    var methods = this;
    this.init = function () {
        if ($(window).width() <= NB.Breakpoints.small) {
            // Breakpoint de tablet para mobile
            this.callSlider();
        } else if ($(window).width() <= NB.Breakpoints.large) {
            // Breakpoint de desk para tablet
            this.callSlider();
        } else {
            // Breakpoint de desk
            this.callColorbox();
        }
    };
    this.callColorbox = function () {
        if (typeof that.data('callColorbox') === 'undefined' || !that.data('callColorbox')) {
            $('#cboxOverlay').height($('.wrap').height());
            that.data('callSlider', false).data('callColorbox', true).off('click', methods.openSlider);
            $('body').trigger('removeSlider');
            NB.instant(that, 'colorbox');
            $(document).off('cbox_closed.openpop').on('cbox_closed.openpop', function () {
                $('#modalSliderAjax').empty();
            });
        }
    };
    this.callSlider = function () {
        that.colorbox.close();
        that.colorbox.remove();
        that.off('click.colorbox');
        if (typeof that.data('callSlider') === 'undefined' || !that.data('callSlider')) {
            that.removeData('instcolorbox').data('callColorbox', false);
            that.data('callSlider', true).on('click', methods.openSlider);
        }
    };

    this.openSlider = function (e) {
        e.preventDefault();
        if (!$(that).hasClass('openIframe')) {
            NB.instant(that, 'openSlider');
            $('body').trigger('openpop');
        }
    };
    
    $(window).on('resize', function () {
        methods.init();
    });
    methods.init();
};
NB.Behaviors.openPop.param = '';