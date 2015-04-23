<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Website.Prototype.Wagner.Home1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
    .widget
    {
        width:200px;
        height:200px;
        border:1px solid #000000;
        overflow:scroll;
        float:left;
    }
    
    .news
    {
        border:1px dotted #000000;
        padding:2px;
        margin:2px;
    }
    

</style>
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
<script type="text/javascript">
    /*
    função que carrega o conteudo de um widget.

    wagner.leonardi@netbiis.com
    */
    function loadUserWidgetContent(userWidgetId, widgetTypeId) {
        var url = "User-Widget-Content.aspx";

        if(widgetTypeId == 1)
        {
            url = "User-Widget-Content.aspx";
        }else{
            url = "User-Widget-Content-Bookmark.aspx";
        }

        $.ajax({
            url: url,
            data: { widgetId: userWidgetId, widgetTypeId: widgetTypeId, count: 10, skip: 0 }
        }).done(function (data) {
            $(".user-widget-" + userWidgetId).html(data);
        });
    }

    /*
    ao usuario clicar em "add new widget" na home dele, vai chamar essa função, 
    que só vai carregar o ajax de selecionar o tipo de widget

    #TODO: por enquanto não vou programar porque vai ter só o tipo feed, depois precisa dinamico

    wagner.leonardi@netbiis.com
    */
    function addWidget() {
        $.ajax({
            url: "/Services/Add-New-Widget.aspx",
        }).done(function (data) {
            $(".addNewWidget").html(data);
        });
    }

    /*
    Quando clicar em adionar widget do tipo "FEED" vai chamar esse cara

    ai aqui só vai chamar o ajax trazendo um conteudo html com as categorias pra ele selecionar

    wagner.leonardi@netbiis.com
    */
    function addWidgetSelectType(widgetType) {
        $.ajax({
            url: "/Services/Category-List.aspx",
        }).done(function (data) {
            $(".addNewWidget").html(data);

            //Quando por pra adicionar aparece o lugar de por o nome, isso aparece em 2 passos, lembra?
            $(".addNewWidgetName").show();
          

            //manipula os eventos do conteudo carregado via ajax
            //Handle checkbox click event
            $(".checkboxWidgetCategory").click(function () {
                var isFirstHiddenElement = true;
                $("#hidCategory").val("");

                //loop through checkbox and parse selected items into 
                //a hidden field, for reading data on .net's postback 
                $(".checkboxWidgetCategory").each(function () {
                    if (this.checked) {
                        //if it's the first element, don't put a comma (,) after its value
                        if (isFirstHiddenElement) {
                            $("#hidCategory").val($("#hidCategory").val() + $(this).val());
                            isFirstHiddenElement = false;
                        } else {
                            $("#hidCategory").val($("#hidCategory").val() + "," + $(this).val());
                        }
                    }
                });

            });
        });
    }

    /*
    O ajax que mostra as categorias pra selecionar, ao clicar na categoria
    vai chamar essa função passando a categoria que ele informou. Como esse ajax
    vai servir tanto pra selecionar 1x só , ou multiplos, depois #TODO de receber um array ou
    separado por virgulas e quebrar, mas por enquanto so recebe um e salva num hidden field
    para que depois que fizer todos os steps, mandar por ajax pro backend salvar!

    wagner.leonardi@netbiis.com
    */
    function selectWidgetCategory()
    {
        $.ajax({
            url: "/Services/Trusted-Source-List.aspx",
        }).done(function (data) {
            $(".addNewWidget").html(data);

            //Quando por pra adicionar aparece o lugar de por o nome, isso aparece em 2 passos, lembra?
            $(".addNewWidgetName").show();

            //manipula os eventos do conteudo carregado via ajax
            //Handle checkbox click event
            $(".checkboxTrustedSource").click(function () {
                var isFirstHiddenElement = true;
                $("#hidTrustedSource").val("");

                //loop through checkbox and parse selected items into 
                //a hidden field, for reading data on .net's postback 
                $(".checkboxTrustedSource").each(function () {
                    if (this.checked) {
                        //if it's the first element, don't put a comma (,) after its value
                        if (isFirstHiddenElement) {
                            $("#hidTrustedSource").val($("#hidTrustedSource").val() + $(this).val());
                            isFirstHiddenElement = false;
                        } else {
                            $("#hidTrustedSource").val($("#hidTrustedSource").val() + "," + $(this).val());
                        }
                    }
                });
            });
        });
            
    }

    function saveWidget(widgetId)
    {
         $.ajax({
            url: "/Services/Save-Widget.aspx",
            data: { widgetId: userWidgetId, count: 10, skip: 0 }
        }).done(function (data) {
            $(".addNewWidget").html(data);

            //Quando por pra adicionar aparece o lugar de por o nome, isso aparece em 2 passos, lembra?
            $(".addNewWidgetName").show();

            //manipula os eventos do conteudo carregado via ajax
            //Handle checkbox click event
            $(".checkboxTrustedSource").click(function () {
                var isFirstHiddenElement = true;
                $("#hidTrustedSource").val("");

                //loop through checkbox and parse selected items into 
                //a hidden field, for reading data on .net's postback 
                $(".checkboxTrustedSource").each(function () {
                    if (this.checked) {
                        //if it's the first element, don't put a comma (,) after its value
                        if (isFirstHiddenElement) {
                            $("#hidTrustedSource").val($("#hidTrustedSource").val() + $(this).val());
                            isFirstHiddenElement = false;
                        } else {
                            $("#hidTrustedSource").val($("#hidTrustedSource").val() + "," + $(this).val());
                        }
                    }
                });
        });
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <h1>Home logada (protótipo), viu como o login funciona?</h1>
    <div>
        Hello, <%=this.UserLogged.Name%>
    </div>


    <a href="javascript:addWidget()">Add a new widget</a><br />

    <!--nessa div esta tudo sobre adicionar novo widget -->
    <div>
        <input type="hidden" id="hidCategory" />
        <input type="hidden" id="hidTrustedSource" />
        <!-- como esse conteudo aparece em 2 steps , como combinamos will, deixei ele aqui escondido aonde nao aparece wagner.leonardi@netbiis.com-->
        <div class="addNewWidgetName" style="display:none;">
            Widget's name <input type="text" id="fldAddWidgetName" />
        </div>
        
        <div class="addNewWidget">
            <!-- aqui o conteudo é trazido dinamico dos ajax wagner.leonardi@netbiis.com-->
        </div>
    </div>

    <!--aqui é aonde fica os widgets do usuario-->
    <div class="widgets>
        <asp:Repeater ID="rptWidget" runat="server">
            <ItemTemplate>
                <div class="widget">
                    <div><%# ((Model.Widget)Container.DataItem).Name %></div>
                    <div class="user-widget-<%# ((Model.Widget)Container.DataItem).Id %>">loading...</div>
                </div>

                <script type="text/javascript">
                    loadUserWidgetContent(<%# ((Model.Widget)Container.DataItem).Id %>,<%# ((Model.Widget)Container.DataItem).TemplateId %>);
                </script>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    </form>
</body>
</html>
