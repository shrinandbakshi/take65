<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Website.Prototype.SocialApp.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Js/Lib/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="http://connect.facebook.net/en_US/all.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            FB.init({ appId: '<%: Facebook.FacebookApplication.Current.AppId %>', status: true, cookie: 'take65.com', xfbml: true, oauth: true });
            $('.login').addClass('active');

        });

        function Logout() {
            $(document).ready(function () {
                FB.logout(function () { window.location.reload(); });
            });
        }
        function Login() {
            FB.login(function (response) {
                if (response.authResponse) {
                    var accessToken = response.authResponse.accessToken;
                    $(".txtFbToken").val(accessToken);
                    __doPostBack('ctl00$content$lnkLoginFacebook', '');
                }
            }, { scope: 'user_about_me,email,read_friendlists,friends_photos' });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <a href="javascript:Login()">Login</a>
        <asp:TextBox ID="txtFbToken" CssClass="txtFbToken" runat="server" />
        <asp:LinkButton ID="lnkLoginFacebook" runat="server" CssClass="hidden" Text="Login using Facebook" />

        <asp:Label ID="lblFeedbackFacebook" CssClass="error" runat="server" Visible="false" meta:resourcekey="ErrorFeedback" />
    </div>
    </form>
</body>
</html>
