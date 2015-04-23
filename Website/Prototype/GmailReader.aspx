<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GmailReader.aspx.cs" Inherits="Website.Prototype.GmailReader" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        User: <asp:TextBox ID="txtUser" runat="server" /><br />
        Pass: <asp:TextBox ID="txtPass" TextMode="Password" runat="server" />
        <br />
        <asp:Button ID="btnSubmit" runat="server" Text="Test" />
        <br /><br />
        <asp:Label ID="lblFeedback" runat="server" />
    </div>
    </form>
</body>
</html>
