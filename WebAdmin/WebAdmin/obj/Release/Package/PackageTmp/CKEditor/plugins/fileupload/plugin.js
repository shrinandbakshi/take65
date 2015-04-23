CKEDITOR.dialog.add('fileupload', function (editor) {
    return {
        title: 'Inserir Arquivo',
        minWidth: 390,
        minHeight: 230,
        contents: [
        {
            id: 'urlTab',
            label: 'URL do Vídeo',
            title: 'URL do Vídeo',
            elements:
            [
                {
                    id: 'nome',
                    type: 'text',
                    label: 'Nome Arquivo'
                },
                {
                    id: 'arquivo',
                    type: 'file',
                    label: 'Enviar para o Servidor'
                }
            ]
        }
        ],
        onOk: function () {
            var editor = this.getParentEditor();
            var contentUrl = this.getValueOf('urlTab', 'arquivo');
            
            if (contentUrl.length > 0) {
                    editor.insertHtml(contentUrl);
            }



            var img =  $(this).attr("img");
            $.ajax({
                type: "POST",
                url: "/Upload.ashx",
                data: "imgname="+img,
                contentType: 'application/x-www-form-urlencoded',
                success: function(response) {
                    if(response == "true")
                    {
                    }
                },
                error: function(response) {
                    alert('There was an error. ' + response);
                }
            });
        },
        buttons: [CKEDITOR.dialog.okButton, CKEDITOR.dialog.cancelButton]
    };
});


CKEDITOR.plugins.add('fileupload',
{
    init: function (editor) {
        var command = editor.addCommand('fileupload', new CKEDITOR.dialogCommand('fileupload'));
        command.modes = { wysiwyg: 1, source: 1 };
        command.canUndo = false;

        editor.ui.addButton('fileupload',
        {
            label: 'Inserir Arquivo',
            command: 'fileupload',
            icon: this.path + 'images/icon.png'
        });

        CKEDITOR.dialog.add('fileupload', 'fileupload');
    }
});