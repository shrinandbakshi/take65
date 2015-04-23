<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload_SimpleImage.aspx.cs" Inherits="WebAdmin.Page.Toolbox.Upload_SimpleImage" %>

<%@ Register Assembly="Imazen.Crop" Namespace="Imazen.Crop" TagPrefix="ic" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="/Css/Style.css" rel="stylesheet" />
    <link href="/Css/Table.css" rel="stylesheet" />
    <link href="/Css/Tools.css" rel="stylesheet" />
</head>
<body style="min-width: 775px; width: 775px; border: 0; background-color: #ffffff;">
    <form id="form1" runat="server">
        <br />
        <table class="table" style="width: 765px; cellspacing: 0">
            <tr>
                <td style="text-align: center;" colspan="2">
                    <asp:Panel ID="pnlUpload" runat="server">
                        <asp:FileUpload ID="fuImagem" runat="server" CssClass="textbox button-btn" Width="600px" />
                        <asp:Button ID="btnUpload" runat="server" CssClass="button-btn" Text="Upload" />
                        <asp:Label ID="lblImagem" runat="server" CssClass="alert"></asp:Label>
                    </asp:Panel>
                    <br />
                    <br />
                    <asp:Panel ID="pnlImagem" runat="server">
                        <div align="center">
                            <ic:CropImage ID="wci1" runat="server" Image="imgRed" CanvasWidth="900" FixedAspectRatio="true" />
                            <asp:Image ID="imgRed" runat="server" Visible="false" />
                        </div>
                    </asp:Panel>
                    <br />
                    <asp:HiddenField ID="hfId" runat="server" />
                    <asp:HiddenField ID="hfNameImge" runat="server" />
                    <asp:HiddenField ID="hfExtensionImage" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 40%;">Name:
                </td>
                <td style="text-align: left; width: 60%;">
                    <asp:TextBox CssClass="textbox" ID="txtNome" runat="server" Width="200px" MaxLength="256"></asp:TextBox>
                    &nbsp;&nbsp;
                <asp:Label CssClass="alert" ID="lblNome" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 40%;">Description:
                </td>
                <td style="text-align: left; width: 60%;">
                    <asp:TextBox CssClass="textbox" ID="txtDescricao" runat="server" Width="200px" MaxLength="256"></asp:TextBox>
                    &nbsp;&nbsp;
                <asp:Label CssClass="alert" ID="lblDescricao" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <div style="width: 765px; text-align: center;">
            <asp:Button CssClass="button-btn" ID="btnSave" runat="server" Text="<%$ Resources:Language, Save %>" />
        </div>
    </form>
</body>
</html>
