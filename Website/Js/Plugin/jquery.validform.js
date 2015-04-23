; (function ($, window, document, undefined) {

    var pluginName = "validform",
        defaults = {
            type: "empty"
        };

    function Plugin(element, options) {
        this.element = $(element);

        this.options = $.extend({}, defaults, options);

        this._defaults = defaults;
        this._name = pluginName;

        this.init();
    }

    Plugin.prototype = {
        init: function () {
            if (typeof this.options.invalid !== 'undefined')
                this.invalid = this.options.invalid;
        },
        // Funcao que da trim na string
        trim: function (_text) {
            return _text.replace(/^\s*|\s*$/g, "");
        },
        invalid: function (sClass) {
            var elem = this.element;
            if (sClass)
                elem = this.element.parent().find(sClass);
            else if (typeof this.options.parent !== 'undefined')
                elem = this.element.parent();

            if (typeof this.options.bgcolor !== 'undefined') {
                if (!elem.data('background-color'))
                    elem.data('background-color', elem.css('background-color')).css('background-color', this.options.bgcolor).on('focus, click', function () { elem.css('background-color', elem.data('background-color')) });
                else
                    elem.css('background-color', this.options.bgcolor);
            }
            if (typeof this.options.textcolor !== 'undefined') {
                if (!elem.data('color'))
                    elem.data('color', elem.css('color')).css('color', this.options.textcolor).on('focus, click', function () { elem.css('color', elem.data('color')) });
                else
                    elem.css('color', this.options.textcolor);
            }
            if (typeof this.options.border !== 'undefined') {
                if (!elem.data('border'))
                    elem.data('border', elem.css('border')).css('border', this.options.border).on('focus, click', function () { elem.css('border', elem.data('border')) });
                else
                    elem.css('border', this.options.border);
            }
            if (typeof this.options.labelcolor !== 'undefined') {
                var $label = (elem.hasClass('valida-prev')) ? elem.prev() : elem.parent().find('label');

                if (!$label.data('color')) {
                    $label.data('color', $label.css('color')).css('color', this.options.labelcolor);
                    elem.on('focus', function () { $label.css('color', $label.data('color')) });
                } else {
                    $label.css('color', this.options.labelcolor);
                }
            }
            if (typeof this.options.errorClass !== 'undefined') {
                var eClass = this.options.errorClass;
                elem.addClass(eClass);
                elem.on('focus click', function () { elem.removeClass(eClass); });
            }
        },
        empty: function () {
            val = this.element.val();
            if (this.trim(val) == '')
                return false;

            if (typeof this.element.data('minchar') !== 'undefined' && this.trim(val).length < this.element.data('minchar'))
                return false;

            return true;
        },
        select: function () {
            val = this.trim(this.element.val());
            if (val == 0 || val == '' || val == false) {
                return false;
            }
            return true;
        },
        email: function () {
            val = this.element.val();
            if (!this.empty(val)) return false;
            var reg1 = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/i;
            var reg2 = /\.\./i;
            if (!val.match(reg1) || val.match(reg2)) {
                return false;
            }
            return true;
        },
        equal: function (element2) {
            if (this.empty(this.element) && this.empty(element2)) {
                if (this.element.val() !== element2.val())
                    return false;
                else
                    return true;

            } else {
                return false;
            }
        },
        cpf: function () {
            var cpf = this.element.val();
            if (!this.empty(val)) return false;
            exp = /\.|\-/g;
            cpf = cpf.toString().replace(exp, "");
            var digitoDigitado = eval(cpf.charAt(9) + cpf.charAt(10));
            var soma1 = 0, soma2 = 0;
            var vlr = 11;

            for (i = 0; i < 9; i++) {
                soma1 += eval(cpf.charAt(i) * (vlr - 1));
                soma2 += eval(cpf.charAt(i) * vlr);
                vlr--;
            }
            soma1 = (((soma1 * 10) % 11) == 10 ? 0 : ((soma1 * 10) % 11));
            soma2 = (((soma2 + (2 * soma1)) * 10) % 11);

            soma2 = (soma2 == 10) ? 0 : soma2;

            var digitoGerado = (soma1 * 10) + soma2;
            if (digitoGerado != digitoDigitado) {
                return false;
            }
            return true;
        },
        data: function () {
            var date = this.element.val();
            if (!this.empty(date)) return false;
            var matches = /^(\d{2})[-\/](\d{2})[-\/](\d{4})$/.exec(date);
            if (matches == null) return false;
            var d = matches[1];
            var m = matches[2] - 1;
            var y = matches[3];
            var composedDate = new Date(y, m, d);

            var valid = composedDate.getDate() == d &&
                    composedDate.getMonth() == m &&
                    composedDate.getFullYear() == y;

            return valid;
        },
        checked: function (elements) {
            if (elements.filter(':checked').length > 0) {
                return true;
            } else {
                return false;
            }
        }
    };

    $.fn[pluginName] = function (options) {
        return this.each(function () {
            if (!$.data(this, "plugin_" + pluginName)) {
                $.data(this, "plugin_" + pluginName, new Plugin(this, options));
            }
        });
    };

})(jQuery, window, document);