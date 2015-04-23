var replaceString = function (str, args) {
    return str.replace(/\{{(\w+)\}}/g, function (s, key) {
        return args[key] || s;
    });
};

// Calcula tamanho do box com padding e margens
var calcRealBox = function($el) {
    var elSize = {
        height: $el.outerHeight() + parseInt($el.css('margin-top'), 10) + parseInt($el.css('margin-bottom'), 10) + parseInt($el.css('border-top-width'), 10) + parseInt($el.css('border-bottom-width'), 10),
        widht: $el.outerWidth() + parseInt($el.css('margin-left'), 10) + parseInt($el.css('margin-right'), 10) + parseInt($el.css('border-left-width'), 10) + parseInt($el.css('border-right-width'), 10)
    };

    if ($el.css('display') !== 'none')
        return elSize;
    else
        return { height: 0, width: 0 };
};

// Scrolla o box para a esquerda
var scrollToLeft = function ($box, offsetLeft) {
    $box.stop(true, false).animate({
        scrollLeft: offsetLeft
    }, 500);
};

// Retorna a diferenca entre duas datas conforme o tipo solicitado (month/day/year hh:mm:ss)
var datediff = function (fromDate, toDate, interval) {
    var second = 1000, minute = second * 60, hour = minute * 60, day = hour * 24, week = day * 7;
    fromDate = new Date(fromDate);
    toDate = new Date(toDate);
    var timediff = toDate - fromDate;
    if (isNaN(timediff)) return NaN;
    switch (interval) {
        case "years": return toDate.getFullYear() - fromDate.getFullYear();
        case "months": return (
                        (toDate.getFullYear() * 12 + toDate.getMonth()) -
                        (fromDate.getFullYear() * 12 + fromDate.getMonth())
                    );
        case "weeks": return Math.floor(timediff / week);
        case "days": return Math.floor(timediff / day);
        case "hours": return Math.floor(timediff / hour);
        case "minutes": return Math.floor(timediff / minute);
        case "seconds": return Math.floor(timediff / second);
        default: return undefined;
    }
};


//Transforma o segundos em formato de horario 
var convertTime = function(sec) {
    var sec_numb = parseInt(sec, 10);
    var hours = Math.floor(sec_numb / 3600);
    var minutes = Math.round((sec_numb - (hours * 3600)) / 60);
    var seconds = sec_numb - (hours * 3600) - (minutes * 60);
    
    if (hours < 10) { hours = "0" + hours; }
    if (minutes < 10) { minutes = "0" + minutes; }
    if (seconds < 10) { seconds = "0" + seconds; }
    
    var time;
    time = (hours != "00")? hours + 'h ': '';
    time += (minutes != "00")? minutes + 'min': '';
    return time;
};

//Transforma hora em segundos ex(05:20:15)
var converHourToSec = function (hour) {
    var time = hour.split(':');
    var sec = (+time[0]) * 60 * 60 + (+time[1]) * 60 + (+time[2]);

    return sec;
};

//Transforma segundo em data completa
var converToDate = function (secs) {
    var t = new Date();
    t.setTime(secs*1000);
    return t;
};

// Função está sendo usada no backend
var triggerClick = function (elem) {
    $(function () {
        $(elem).trigger('click');
    });
};

/****/
/** Funcao para placeholder em browser antigos **/
/****/
var add = function() {
    if ($(this).val() === '') {
        $(this).val($(this).attr('placeholder')).addClass('placeholder');
    }
};

var remove = function() {
    if ($(this).val() === $(this).attr('placeholder')) {
        $(this).val('').removeClass('placeholder');
    }
};
var placeHolder = function () {
    // Create a dummy element for feature detection
    if (!('placeholder' in $('<input>')[0])) {

        // Select the elements that have a placeholder attribute
        $('input[placeholder], textarea[placeholder]').not('input[type=password]').blur(add).focus(remove).each(add);

        // Remove the placeholder text before the form is submitted
        $('form').submit(function () {
            $(this).find('input[placeholder], textarea[placeholder]').not('input[type=password]').each(remove);
        });
    }
    //Função que retorna os valores default dos campos input do formulário
    $('form').find('input[type=password][placeholder]').each(function (i) {
        var $this = $(this),
			valPlaceholder = $this.attr('placeholder'),
			idPlaceholder = "placeholder" + i,
			$label = $("<label class='placeholder show' id='" + idPlaceholder + "'>" + valPlaceholder + "</label>");
        if (valPlaceholder !== '') {
            $this.before($label).addClass(idPlaceholder).css({ position: 'relative', zIndex: 1 }).on({
                'focus setVal': function () {
                    $label.hide().removeClass('show');
                },
                'blur removeVal': function (e) {
                    if (!$label.hasClass('show') && $this.val() === '') {
                        $label.show().addClass('show');
                    }
                }
            });

            if ($label.height() < 1)
                $label.height(17);
            $label.css({
                color: $this.css('color'),
                marginTop: parseInt($this.height() - $label.height(), 10) / 2,
                marginLeft: $this.css('padding-left'),
                fontSize: $this.css('font-size'),
                height: $label.height(),
                left: 0,
                position: 'absolute',
                overflow: 'hidden',
                cursor: 'text',
                zIndex: 3
            }).data({
                width: $label.width()
            }).on('click', function () {
                $this.focus();
            });

            if ($this.val() !== '')
                $label.css({ width: 0 }).removeClass('show');

        }
    });
};
/****/
/** FIM - Funcao para placeholder em browser antigos **/
/****/

// Trigger click caso haja hash na página
var clickHash = function () {
    var hash = window.location.hash;

    if (typeof hash !== 'undefined') {
        var elem = $(hash);

        if (elem.length > 0)
            elem.trigger('click');
    }
};

var waitUser = function () {
    if ($(".take-me-home").hasClass("hide")) {
        $(".take-me-home").removeClass("hide");
        $(".take-me-home-background").removeClass("hide");
    }
}

$(function () {
    if (!Modernizr.placeholder)
        placeHolder();


    document.onreadystatechange = function () {
        if (document.readyState == "complete") {
            clickHash();
        }
    };

    


    //$('form')[0].reset();
});
