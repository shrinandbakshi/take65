NB.Behaviors.menu = function (that) {
    var param = that.data('param') || {};
    var methods = this;
    this.miniMenu = $('#miniMenu');

    this.init = function () {
        /* Open e close menu */
        that.find('#openMenu').on('click', function (e) {
            e.preventDefault();
            methods.openMenu();
        });
        $('#closeMenu').on('click', function (e) {
            e.preventDefault();
            methods.closeMenu();
        });
        /* End Open e close menu */
        $('#searchOpen').on('click', function (e) {
            e.preventDefault();
            $('section.search').slideToggle(500, function () {
                $(window).trigger('resize');
            });
        });

        $('body').on('openpop', function () {
            methods.closeMenu();
        });
    }
    this.openMenu = function () {
        var $mainLinks = $('.main-links');
        methods.miniMenu.css('top', $mainLinks.offset().top + $mainLinks.outerHeight()).fadeIn(800).animate({
            right: 0
        }, { queue: false, duration: 500 });
    };
    this.closeMenu = function () {
        methods.miniMenu.fadeOut(500).animate({
            right: '-100%'
        }, { queue: false, duration: 800 });
    };

    methods.init();
}
NB.Behaviors.menu.param = '';