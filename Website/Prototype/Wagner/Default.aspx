<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Website.Prototype.Wagner.Home" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            //Handle checkbox click event
            $(".preferenceCheckbox").click(function () {
                var isFirstHiddenElement = true;
                $("#hidPreference").val("");

                //loop through checkbox and parse selected items into 
                //a hidden field, for reading data on .net's postback 
                $(".preferenceCheckbox").each(function () {
                    if (this.checked) {
                        //if it's the first element, don't put a comma (,) after its value
                        if (isFirstHiddenElement) {
                            $("#hidPreference").val($("#hidPreference").val() + $(this).val());
                            isFirstHiddenElement = false;
                        } else {
                            $("#hidPreference").val($("#hidPreference").val() + "," + $(this).val());
                        }
                    }
                });
            });

        });
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scpManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="updRegister" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
        <h1>Home não logada (protótipo)</h1>
        <h3>
            <i>will , não usa o codigo dessa página pra nada, faz o html de registro/login com os inputs de html mesmo , esse update panel está só pra prototipo, o real vai ser ajax, viva.
            <br /><br />
            Faz o esquema dos checkbox fixo, que quando eu pegar pra integrar eu passo pra dinamico que nem tá agora. O que você pode fazer é pegar esse codigo javascript
            que esta no 'onready' e passar no esquema do modules e tal <s>que em breve eu também vou aprender como se faz</s> :)
            <br /><br />
              O esquema de consultar se nome de usuario ja existe se chama "/Services/User-Check-Email.aspx",
            mandei a 'documentação' do serviço no seu e-mail. (eu sei q não era pra verificar email e sim usuario, mas usa assim que quando eu integrar eu mudo isso)
            <br /><br />loga lá embaixo que na outra página tem bastante coisa pra aproveitar <s>que já esta tudo pronto</s> :)

            </i>
            
            </b>
        </h3>


        <h1>Register</h1>
        Name: <asp:TextBox ID="fldName" runat="server" ClientIDMode="Static"></asp:TextBox>    
        Email: <asp:TextBox ID="fldEmail" runat="server" ClientIDMode="Static"></asp:TextBox>
        Password: <asp:TextBox ID="fldPassword" runat="server" ClientIDMode="Static"></asp:TextBox>

        <asp:HiddenField ID="hidPreference" runat="server" ClientIDMode="Static" />
        <asp:Repeater ID="rptPreference" runat="server">
            <ItemTemplate>
                <div>
                    <input type="checkbox" value="<%# ((Model.Tag)Container.DataItem).Id %>" class="preferenceCheckbox" /><%# ((Model.Tag)Container.DataItem).Display %>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:LinkButton ID="btnRegister" runat="server" ClientIDMode="Static">Register</asp:LinkButton>
        <asp:Literal ID="lblRegisterError" runat="server" ClientIDMode="Static"></asp:Literal>

        <br />

        
        <h1>Login</h1>
        <h3>usr: wagner@teste.com <br />pass: 123</h3>

        Email: <asp:TextBox ID="fldLoginName" runat="server" ClientIDMode="Static"></asp:TextBox>
        Password: <asp:TextBox ID="fldLoginPassword" runat="server" ClientIDMode="Static"></asp:TextBox>

        <asp:LinkButton ID="btnLogin" runat="server" ClientIDMode="Static">Login</asp:LinkButton>
        <asp:Literal ID="lblLoginError" runat="server" ClientIDMode="Static"></asp:Literal>

    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnRegister" />
        <asp:AsyncPostBackTrigger ControlID="btnLogin" />
    </Triggers>
    </asp:UpdatePanel>
    </form>
    
</body>
</html>
