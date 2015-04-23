NB.Behaviors.colorbox = function (that) {

    //console.log(that);

    var param = that.data('param') || {};
    var methods = this;
    param.loadingIco = param.loadingIco || 'white';
    param.type = param.type || 'inline';
    param.onComplete = param.onComplete || function () { };

    this.init = function () {
        this.colorbox();
    };

    this.colorbox = function () {
        $.colorbox.settings.close = NB.Lang.Close;
        param.close = param.close || NB.Lang.Close;
        param.closeButton = (typeof param.closeButton !== undefined) ? param.closeButton : true;
        param.allowClose = (typeof param.allowClose !== undefined) ? param.allowClose : true;

        param.width = param.width || 'auto';
        param.height = param.height || 'auto';
        param.className = param.className || 'box-white';
        param.action = param.action || false;
        param.resizeBoxLightbox = param.resizeBoxLightbox || false;

        that.on('click', function () {
            if (param.action === 'close') {
                $.colorbox.close();
            }
        });

        methods[param.type]();
    };
    
    this.simple = function () {
        var html = '<h2>' + param.title + '</h2>' + param.content;
        that.colorbox({
            html: html,
            //open: true,
            width: param.width,
            height: param.height,
            className: param.className,
            loadingIco: param.loadingIco,
            close: param.close,
            opacity: 0.8,
            onComplete: function () {
                //if (param.height == 'auto' || param.width == 'auto')
                $.colorbox.resize();
                NB.LoadBehavior($('#colorbox'));
                param.onComplete();
            }
        });
    };

    this.inline = function () {

        that.off('click.colorbox').on('click.colorbox', function (e) {
            e.preventDefault();
            //alert(param.width);
            //alert(param.onComplete);
            $.colorbox({
                inline: true,
                className: param.className,
                href: param.href || that.attr('href'),
                closeButton: param.closeButton,
                escKey: param.allowClose,
                overlayClose: param.allowClose,                
                height: param.height,
                width: param.width,
                close: param.close,
                opacity: 0.8,
                scrolling: false,
                scrollTop: true,
                onComplete: function () {
                    param.onComplete();
                }
            });
        });
    };

    this.iframe = function () {
        that.off('click.colorbox').on('click.colorbox', function(e) {
            e.preventDefault();
            if (!that.hasClass('openIframe')) {
                document.body.scrollTop = document.documentElement.scrollTop = 0;
                $.colorbox({
                    iframe: true,
                    className: param.className,
                    height: param.height,
                    width: param.width,
                    href: param.href || $(this).attr('href'),
                    close: param.close,
                    closeButton: param.closeButton,
                    opacity: 0.8,
                    onComplete: function () {
                        if (param.height == 'auto' || param.width == 'auto')
                            $.colorbox.resize();
                        NB.LoadBehavior($('#colorbox'));
                        param.onComplete();
                    }
                });
            }
        });
    };

    this.ajax = function () {
        that.colorbox({
            className: param.className,
            height: param.height,
            width: param.width,
            close: param.close,
            closeButton: param.closeButton,
            opacity: 0.8,
            onComplete: function () {
                //$.colorbox.resize();
                NB.LoadBehavior($('#colorbox'));
                param.onComplete();
                if (param.resizeBoxLightbox) {
                    $.colorbox.resize({
                        width: $('#colorbox').find('.box-lightbox').outerWidth() + 10
                    });
                }
            }
        });
    };
    this.param = param;
    methods.init();
};
NB.Behaviors.colorbox.param = 'title, content';