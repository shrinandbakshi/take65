//NB.Behaviors.resize = function (that) {
//    var param = that.data('param') || {};
//    var methods = this;

//    // Função para usar no resize de acordo com cada breakpoint
//    //this.resize = function () {
//    //    if ($(window).width() <= NB.Breakpoints.small) {
//    //        // Breakpoint de tablet para mobile
//    //        //this.alignBoxes(false);
//    //        //this.searchSize(false);
//    //        return false;
//    //    } else if ($(window).width() <= NB.Breakpoints.large + 16) {
//    //        // Breakpoint de desk para tablet
//    //        //this.alignBoxes(2);
//    //        //this.searchSize(false);
//    //    } else {
//    //        // Breakpoint de desk
//    //        //this.alignBoxes(3);
//    //        //this.searchSize(true);
//    //    }
//    //};

//    /* Calcula se qual elemento deve conter espaçamento 
//        e qual não deve conter de acordo com o tamanhos dos boxes 
//        passado por space e da resolução;
//        Alinha os boxes horizontalmente
//        */
//    //this.alignBoxes = function (maxCont) {
//    //    var cont = 0,
//    //        maxHeight = 0,
//    //        line = 0,
//    //        lineHeight = [];
//    //        $boxColumns = $('.content-main').find('.box-column');

//    //    if(!maxCont) {
//    //        $boxColumns.height('auto').find('.content').height('auto');
//    //        return false;
//    //    }
//    //    // Percorre nos boxes dizendo cada linha que pertencem e tira o espaço do primeiro elemento
//    //    $boxColumns.removeClass('no-gutter').each(function() {
//    //        var $this = $(this);
//    //        // Zera a altura deste box
//    //        $this.height('auto').find('.content').height('auto');

//    //        // Verifica se é o primeiro box
//    //        if (cont === 0) {
//    //            $this.addClass('no-gutter'); // retira margin-left
//    //            if ($this.attr('data-type') != '3' && $this.attr('data-type') != '6') {
//    //                maxHeight = $this.height(); // define maxHeight com valor da altura do primeiro box
//    //            }
//    //            line++; // line recebe o número da nova linha
//    //        }

            
//    //        cont += parseInt($this.attr('data-space'), 10); // Conta os elementos da linha
//    //        maxHeight = (maxHeight < $this.height() && ($this.attr('data-type') != '3' && $this.attr('data-type') != '6')) ? $this.height() : maxHeight; // verifica se este box é maior do que a altura dos anteriores
            
//    //        $this.attr('data-line', line); // diz a qual linha pertence esse box

//    //        if(cont >= maxCont || $this.next().length < 1 ||(parseInt($this.next().attr('data-space')) + cont > maxCont)) { // verifica se está no ultimo box da linha ou se o próximo box cabe na linha deste mesmo
//    //            lineHeight[line] = maxHeight; // insere no array lineHeight, na posição da linha, o tamanho do maior box pertencente a linha
//    //            cont = 0; // zera cont
//    //            maxHeight = 0; // zera maxHeight
//    //        }
//    //    })
//    //        .each(function() {
//    //            var $this = $(this),
//    //                newHeight = lineHeight[$this.attr('data-line')];
//    //            newHeight = (newHeight <= 0) ? 200 : newHeight; //min value 200, never zero

//    //            $this.height(newHeight) // percorre em todos os boxes mudando a altura de todos de acordo com o maior desta linha armazenado no array
//    //                .find('.content').height(newHeight - calcRealBox($this.find('header.nv-1')).height - calcRealBox($this.find('footer.nv-1')).height - 1);
//    //        });
//    //};

//    // Calcula o tamanho que deve ser o box de busca
//    //this.searchSize = function(wide) {
//    //    var filled = 0;
//    //    var $search = that.find('.search');
//    //    var $input = $search.find('input[type=text]');

//    //    if(wide) {
//    //        $search.find('.btn-search').each(function() {
//    //            var $this = $(this);
//    //            filled += $this.outerWidth() + parseInt($this.css('margin-left'), 10) + parseInt($this.css('margin-right'), 10);
//    //        });
//    //        $input.outerWidth($search.width() - filled - 5);
//    //    } else {
//    //        $input.width($search.width() - parseInt($input.css('padding-left'), 10) * 2 - parseInt($input.css('border-left-width'), 10) * 2);
//    //    }
//    //};

//    this.init = function () {
//        var $window = $(window);
//        NB.window = NB.window || {};

//        $window.on('load resize', function () {
//            clearTimeout(window.resizeEvt);
//            window.resizeEvt = setTimeout(function()
//            {
//                //methods.resize();
//                NB.window.sizes = {
//                    width: $window.width(),
//                    height: $window.height()
//                };
//            }, 250);
//        });
//        $('body').on('realignBoxes', function () {
//            //methods.resize();
//            NB.window.sizes = {
//                width: $window.width(),
//                height: $window.height()
//            };
//        });

//        //setTimeout(function () {
//            //methods.resize();
//        //}, 1000);
//    };

//    methods.init();
//};
//NB.Behaviors.resize.param = '';