<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Website.Prototype.Wagner.Facebook.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/2.0.0/jquery.min.js"></script>
     <script src="//connect.facebook.net/en_US/all.js"></script>

     <script>
        
     </script>

</head>

<body>

    <div>
        alert
        
        <div id="fb-root"></div>

        <a id="btnLoginFacebook">Login with facebook betch</a>

    </div>
    <script type="text/javascript">

        FB.init({
          appId: '634767319888769',
          channelUrl: 'http://take65-v2.local.netbiis.com/Prototype/Wagner/Facebook/Test.aspx',
        });     

        $("#btnLoginFacebook").on('click',function ()
        {
            var facebookIsLoggedResult = facebookIsLogged();
            if(facebookIsLoggedResult !== false)
            {
                //o cara ja ta logado
                console.log(facebookIsLoggedResult);

            }else{
                //o cara não ta logado, precisa logar

                var facebookLoginResult = facebookLogin();
                if(facebookLoginResult === false)
                {
                    //cara não autorizou a app
                }else{
                    console.log(facebookLoginResult);
                }
            }
         
        });

        
        /*
         * Verifica se o usuario esta conectado ao facebook
         * Se estiver, retorna um objeto do usuário no facebook
         * Se não estiver retorna false booleano. (wagner.leonardi@netbiis.com)
         */
        function facebookIsLogged()
        {
            FB.getLoginStatus(function(response) {
                if (response.status === 'connected') {
                    // Usuário está logado, retorna o objeto usuário do facebook
                    console.log(response.authResponse);
                    return response;
                } else if (response.status === 'not_authorized') {
                    // Usuário está no facebook , mas não autorizou a app
                    return false; 
                } else {
                    // Usuário não está no facebook
                    return false; 
                }
            });
        }

        /*
         * Checa se já tem um usuário cadastrado com esse id, se tiver já loga
         */
        function facebookRegisteredUser(facebookId)
        {
            $.ajax({
                    url: "/Services/User-Check-Login.aspx",
                    data: { login: that.find("#fldLoginName").val(), password: that.find("#fldLoginPassword").val() }
                }).done(function (data) {
                    if (data == "true") {
                        window.location.href = '/';
                    } else {
                        that.find("#loginLoading").html("Loading...").hide();
                        that.find("#loginMessage").html("Invalid login or password.").show();
                    }
                });

            $.ajax({
                url: "/Services/User-Login.aspx",
                data: { login: that.find("#fldLoginName").val(), password: that.find("#fldLoginPassword").val() }
            }).done(function (data) {
                if (data == "true") {
                    window.location.href = '/';
                } else {
                    that.find("#loginLoading").html("Loading...").hide();
                    that.find("#loginMessage").html("Invalid login or password.").show();
                }
            });

        }

        /*
         * Loga o usuário pelo facebook
         * Caso o usuário se logar (e autorizar a app), retorna um objeto do usuário no facebook
         * Caso não se logar (ou não autorizar a app), retorna false booleano. (wagner.leonardi@netbiis.com)
         */
        function facebookLogin()
        {
            FB.login(function(response) {
            if (response.authResponse) {
                return response;
                }
            });

            return false;
        }

        function facebookGetUser()
        {
            FB.api('/me', function (response) {
                return response;
            });
        }



        

        /*
        FB.login(function (response) {
            if (response.authResponse) {
                
                FB.api('/me', function (response) {
                    console.log('Good to see you, ' + response.name + '.');
                });

            } else {
                console.log('User cancelled login or did not fully authorize.');
            }
        }, { scope: 'email' });
        */

        

        
    </script>    
</body>
</html>
