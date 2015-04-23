$('html, body').css({
    overflow: 'auto',
    height: 'auto',
    width: 'auto'
});
NB.Behaviors.openSlider = function (that) {
    var param = that.data('param') || {};
    var methods = this;
    var $modalSlider = $('#modalSlider');
    var $modalSliderAjax = $modalSlider.find('#modalSliderAjax');
    var $htmlBody = $('html, body');

    this.init = function () {
        //this.getAjax();
        methods.openSlider();
        $htmlBody.on({
            removeSlider: function () {
                methods.closeSlider();
            },
            resizeModalBox: function () {
                methods.resizeModal();
            }
        });

        $modalSlider.find('#closeModal').on('click', function (e) {
            e.preventDefault();
            methods.closeSlider();
        });

        $(window).on('resize.openSlider', function () {
            methods.resizeModal();
        });
    };
    this.getAjax = function () {
        //methods.openSlider();
        if (param.type === 'iframe') // {
            $modalSliderAjax.html('<iframe frameborder="0" src="' + that.attr('href') + '" class="cboxIframe" allowfullscreen="true" webkitallowfullscreen="true" mozallowfullscreen="true"></iframe>');
    };
    this.openSlider = function () {
        methods.removeScrollPage();
        setTimeout(function () {
            try { window.scrollTo(0); } catch (e) { };
            document.body.scrollTop = document.documentElement.scrollTop = 0;
            $htmlBody.css({
                display: 'none'
            });
            $modalSlider.show();
            //$modalSliderAjax.height($modalSlider.height() - $modalSlider.find('#closeModal').height());
            $modalSliderAjax.height('100%');
            methods.getAjax();
            $htmlBody.css({
                display: 'block'
            });
        }, 100);

        
        
        
        //$("body,html,document").scrollTop($("#map_canvas").offset().top);
        /*
        alert(window.pageYOffset);
        alert(top.window.pageYOffset);
        alert(document.getElementById('miniMenu'));
        alert(document.getElementById('miniMenu').pageYOffset);
        alert(document.getElementById('miniMenu').scrollTop);
        alert($("#miniMenu").offset().top);
        alert(document.body.scrollTop);
        
        */

        //document.body.scrollTop = document.documentElement.scrollTop = 0;
        
        //methods.removeScrollPage();

/*
        $modalSlider.show();
        $modalSliderAjax.height($modalSlider.height() - $modalSlider.find('#closeModal').height());
        methods.getAjax();*/

        /*
        $modalSlider.stop(true, true).css({
            top: NB.window.sizes.height,
            height: NB.window.sizes.height
        }).show().animate({
            top: 0,
            opacity: 1
        }, 700, function () {
            $modalSliderAjax.height($modalSlider.height() - $modalSlider.find('#closeModal').height());
            methods.getAjax();
        });*/
    };
    this.closeSlider = function () {
        $modalSlider.hide().removeAttr('style');
        $modalSliderAjax.html('').removeAttr('style');
        methods.returnScrollPage();
        $(window).off('resize.openSlider');
        $htmlBody.off('removeSlider resizeModalBox');

        /*
        $modalSlider.stop(true, false).animate({
            top: NB.window.sizes.height,
            opacity: 0
        }, 1000, function () {
            $modalSlider.hide().removeAttr('style');
            $modalSliderAjax.html('').removeAttr('style');
            methods.returnScrollPage();
            $(window).off('resize.openSlider');
            $htmlBody.off('removeSlider resizeModalBox');
        });*/
    };

    this.resizeModal = function () {
        $modalSlider.stop(true, true).css('height', NB.window.sizes.height);
        $modalSliderAjax.height($modalSlider.height() - $modalSlider.find('#closeModal').height());
    };

    this.removeScrollPage = function () {
        $htmlBody.css({
            overflow: 'hidden',
            height: '100%',
            width: '100%'
        });
    };

    this.returnScrollPage = function () {
        $htmlBody.css({
            overflow: 'auto',
            height: 'auto',
            width: 'auto'
        });
    };

    methods.init();
};
NB.Behaviors.openSlider.param = '';