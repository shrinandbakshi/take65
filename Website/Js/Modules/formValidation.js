NB.Behaviors.formValidation = function (that) {
    var param = this.param = that.data('param') || {};
    var methods = this;

    this.init = function () {
        that.data('formvalid', false).find('[data-valid="submit"]').on('click', function (e) {
            //e.preventDefault();
            var valid = true;
            var amsg = [];
            var $this = $(this);

            that.find('[data-valid][data-valid!="submit"]').each(function () {
                var $this = $(this);
                var fnValid;

                var validform = $this.validform({
                    //border: '1px solid red',
                    parent: true,
                    errorClass: 'error'
                }).data('plugin_validform');

                if ($this.data('valid') === 'equal')
                    fnValid = validform[$this.data('valid')](that.find('[data-group="' + $this.data('group') + '"]').not($this));
                else if ($this.data('valid') === 'checked')
                    fnValid = validform[$this.data('valid')](that.find('[data-group="' + $this.data('group') + '"]'));
                else
                    fnValid = validform[$this.data('valid')]();

                if (!fnValid) {
                    valid = false;
                    var sClass = $this.data('elem') || '';

                    validform.invalid(sClass);

                    // Reune em amsg o nome dos campos inválidos
                    if (typeof $this.data('name') !== 'undefined' && $.inArray($this.data('name'), amsg) === -1)
                        amsg.push($this.data('name'));
                }
            });



            // Cria mensagem de feedback
            if (typeof param.error !== 'undefined' && !valid) {
                var msg = '',
                    error = '';
                
                if (typeof param.msgtype !== 'undefined' && param.msgtype == 'summary') {
                    msg = '<ul>';

                    for (error in amsg)
                        msg += '<li>' + amsg[error] + '</li>';

                    msg += '</ul>';
                    that.find(param.error).html(msg).fadeIn(400);
                } else {
                    msg = 'Please fill out the required fields: ';
                    for (error in amsg)
                        msg += amsg[error] + ', ';

                    msg = msg.substring(0, msg.length - 2) + '.';

                    that.find(param.error).text(msg).fadeIn(400);
                }

            }
            that.data('formvalid', valid);
            if (valid && $this.data('trigger'))
                $this.trigger($this.data('trigger'));

            if (param.colorbox)
                $.colorbox.resize();

            return valid;
        });
    };

    methods.init();
};

NB.Behaviors.formValidation.version = '0.1';
