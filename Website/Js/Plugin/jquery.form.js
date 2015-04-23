/**** Plugin Form ****/
var styleForm = function () {
    return {
        init: function ($forms, not) {
            //this.checkbox($forms.find('input[type=checkbox]').not(not));
            //this.radio($forms.find('input[type=radio]').not(not));
            this.select($forms.find('select').not(not));
            //this.file($forms.find('input[type=file]').not(not));
            //this.inputFile($forms.find('input[type=file]'))

            $forms.find('button[type=reset], .btn-reset').click(function (e) {
                e.preventDefault();
                $forms[0].reset();
                styleForm.reloadSelect($forms);
            });

            return this;
        },
        file: function ($file, recipient) {
            $aFile = new Array();
            $file.each(function (i) {
                $aFile[i] = $(this).css('opacity', 0);

                //create custom control container
                $aFile[i]['upload'] = $('<div class="file-input"></div>');
                //create custom control feedback
                $aFile[i]['uploadFeedback'] = $('<span class="file-input-feedback" aria-hidden="true">No file selected...</span>').appendTo($aFile[i]['upload']);
                //create custom control button
                $aFile[i]['uploadButton'] = $('<span class="file-input-button" aria-hidden="true">Browse</span>').appendTo($aFile[i]['upload']);


                //on mousemove, keep file input under the cursor to steal click 
                $aFile[i]['upload'].insertAfter($aFile[i]); //insert after the input

                $aFile[i].appendTo($aFile[i]['upload']);

                $aFile[i].addClass('file-input')
                    .focus(function () {
                        $aFile[i]['upload'].addClass('file-input-focus');
                        $aFile[i].data('val', $aFile[i].val());
                    })
                    .blur(function () {
                        $aFile[i].removeClass('file-input');
                        $(this).trigger('checkChange');
                    })
                    .bind('checkChange', function () {
                        if ($aFile[i].val() && $aFile[i].val() != $aFile[i].data('val')) {
                            $aFile[i].trigger('change');
                        }
                    })
                    .bind('change', function () {
                        //get file name
                        var fileName = $(this).val().split(/\\/).pop();
                        //get file extension
                        var fileExt = 'file-input-ext-' + fileName.split('.').pop().toLowerCase();
                        //update the feedback
                        $aFile[i]['uploadFeedback']
                            .text(fileName) //set feedback text to filename
                            .removeClass($aFile[i]['uploadFeedback'].data('fileExt') || '') //remove any existing file extension class
                            .addClass(fileExt) //add file extension class
                            .data('fileExt', fileExt) //store file extension for class removal on next change
                            .addClass('file-input-feedback-populated'); //add class to show populated state
                        //change text of button
                        $aFile[i]['uploadButton'].text('Change');
                    })
                    .click(function () { //for IE and Opera, make sure change fires after choosing a file, using an async callback
                        $aFile[i].data('val', $aFile[i].val());
                        setTimeout(function () {
                            $aFile[i].trigger('checkChange');
                        }, 100);
                    });
            });
        },

        checkbox: function ($checkbox) {
            var $html, className, check, $el;
            var $checkboxArr = new Array;
            var $elArr = new Array;
            $checkbox.each(function (i) {
                $el = $(this);
                $checkboxArr[i] = $el;

                className = 'sCheckbox ';
                className += ($el.attr('class') != 'undefined') ? $el.attr('class') : '';
                className += ($el.is(':checked')) ? ' checked' : '';

                $el.hide(0);

                $html = $('<div class="' + className + '" id="' + $el.attr('name').replace(/$/gi, '_') + '"><span class="check"></span></div>');
                $elArr[i] = $html;

                $el.before($html);

                // Verifica se checkbox esta dentro de label, se estiver, label tera evento onclick, senao, elementro criado tera
                var $label = $('label[for="' + $el.attr('id') + '"]');
                if ($el.parent().is('label'))
                    $click = $el.parent();
                else if($label.length > 0)
                    $click = $($label).add($html);
                else
                    $click = $html;

                $click.on('click toggleCheck', function (e) {
                    e.preventDefault();
                    check = ($checkboxArr[i].is(':checked')) ? false : true;

                    //$checkboxArr[i].get(0).checked = check;
                    $checkboxArr[i].trigger('click').attr('checked', check);

                    if (check)
                        $elArr[i].addClass('checked');
                    else
                        $elArr[i].removeClass('checked');
                });
                $checkboxArr[i].on({
                    checked: function (e, checked) {
                        $checkboxArr[i].prop('checked', checked);
                        $elArr[i].removeClass('checked');
                    },
                    toggleCheck: function () {
                        check = ($checkboxArr[i].is(':checked')) ? false : true;

                        //$checkboxArr[i].get(0).checked = check;
                        $checkboxArr[i].trigger('click').attr('checked', check);

                        if (check)
                            $elArr[i].addClass('checked');
                        else
                            $elArr[i].removeClass('checked');
                    }
                });
            });
        },
        radio: function ($radio) {
            var $radioArr = new Array;
            var $elArr = new Array;
            $radio.each(function (i) {
                $radioArr[i] = $(this);
                className = 'sRadio ' + $radioArr[i].attr('name').replace(/\$/gi, '_') + ' ';
                className += ($radioArr[i].attr('class') != 'undefined') ? $radioArr[i].attr('class') : '';
                className += ($radioArr[i].is(':checked')) ? ' checked' : '';

                $radioArr[i].hide(0);

                $html = $('<div class="' + className + '" id="' + $radioArr[i].attr('name').replace(/\$/gi, '_') + i + '"><span class="check"></span></div>');
                $elArr[i] = $html;

                $radioArr[i].before($html).trigger('onclick');

                // Verifica se checkbox esta dentro de label, se estiver, label tera evento onclick, senao, elementro criado tera
                $click = ($radioArr[i].parent().is('label') || $radioArr[i].parent().find('label').length > 0) ? $radioArr[i].parent() : $html;

                $click.on({
                    click: function (e) {
                        e.preventDefault();

                        $radio.attr('checked', false);
                        $('.' + $radioArr[i].attr('name').replace(/\$/gi, '_')).removeClass('checked');

                        $radioArr[i].attr('checked', true);

                        $elArr[i].addClass('checked');
                    },
                    'mouseover focus': function () {
                        $elArr[i].toggleClass('hover');
                    },
                    'mouseout focusout': function () {
                        $elArr[i].toggleClass('hover');
                    }
                });
            });
        },
        select: function ($select) {
            var $selectArr = new Array;
            var $htmlArr = new Array;
            var nSelect = $select.length;

            $select.each(function (i) {
                $selectArr[i] = $(this).hide();
                var tSelected = ($selectArr[i].find('option:checked').length > 0) ? $selectArr[i].find('option:checked') : $selectArr[i].find('option').eq(0);
                var resizeOptions = false;

                className = 'sSelect ';
                className += ($selectArr[i].attr('class') != 'undefined') ? $selectArr[i].attr('class') : '';
                className += ($selectArr[i].attr('disabled')) ? ' disabled' : '';
                $htmlArr[i] = $("<div class='" + className + "' id='" + $selectArr[i].attr('name') + "' style='z-index: " + (nSelect - i) + "'>" +
                        "   <div class='select-arrow'></div>" +
						"	<a href='#'>" +
						"		<span class='select'>" + tSelected.text() + "</span>" +
						"		<span class='options'></span>" +
						"	</a>" +
						"</div>");

                $selectArr[i].find('option').each(function () {
                    $htmlArr[i].find('.options').css('display', 'none').append(
						$('<span>' + $(this).text() + '</span>')
							.attr('data-value', $(this).val())
							.addClass(($(this).val() == tSelected.val()) ? 'selected' : '')
					);
                });

                $selectArr[i].before($htmlArr[i]);

                $htmlArr[i].find('a').css('width', $htmlArr[i].width());
                if($htmlArr[i].css('width').indexOf('%') > 0) resizeOptions = true;
                
                $htmlArr[i].on({
                    mouseover: function () {
                        $(this).addClass('hover');
                    },
                    mouseout: function () {
                        $(this).removeClass('hover');
                    },
                    click: function (e) {
                        e.preventDefault();
                        var $this = $(this);
                        var $nextObj = $(this).find('.selected').next();

                        if (!$this.hasClass('disabled')) {
                            if(resizeOptions)
                                $htmlArr[i].find('a').css('width', $htmlArr[i].width() - (parseFloat($htmlArr[i].css('padding-left')) * 2));
                            console.log($htmlArr[i].width(), parseFloat($htmlArr[i].css('padding-left')) * 2)

                            $this.addClass('open').find('.options').slideDown(500, function () {
                                $(document).one('click', function () {
                                    $htmlArr[i].removeClass('open').find('.options').hide();
                                });

                                // Scrolla o select para o item seguinte do que está relacionado
                                $this.find('.options').scrollTop($nextObj.index() * $nextObj.height());

                            });
                        }

                    },
                    disabled: function () {
                        $(this).addClass('disabled');
                        $selectArr[i].attr('disabled', 'disabled');
                    },
                    enabled: function () {
                        $(this).removeClass('disabled');
                        $selectArr[i].removeAttr('disabled');
                    }
                });

                $htmlArr[i].find('.options span').on('click select', function () {
                    var $elLi = $(this);

                    $selectArr[i].val($elLi.data('value')).trigger('change');

                    $htmlArr[i]
						.find('.selected').removeClass('selected').end()
						.find('span.select').text($elLi.addClass('selected').text());
                })

                // Desabilita dragging de span caso esteja com barra de rolagem // Firefox
                $htmlArr[i].find('.options')[0].draggable = false;
                $htmlArr[i].find('.options')[0].onmousedown = function (event) {
                    return false;
                };

            });
        },
        reloadSelect: function ($forms) {
            $forms.find('select').each(function () {
                var $select = $(this);
                newSelected = ($select.find('option:checked').length > 0) ? $select.find('option:checked').text() : $select.find('option').eq(0).text();
                $('#' + $select.attr('name')).find('.select').text(newSelected);
            });
        },
        reformSelect: function ($forms) {
            $forms.find('.sSelect').remove();
            this.select($forms.find('select'));
        }
    }
}();