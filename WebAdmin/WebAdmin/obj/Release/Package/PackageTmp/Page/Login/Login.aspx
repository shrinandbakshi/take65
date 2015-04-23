<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebAdmin.Page.Login.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPage" runat="server">
    <div style="text-align: center;">
        <div class="ident-bot-9">
            <div class="heading-wrapper-2 ident-bot-8">
                <div class="heading-before-2">
                </div>
                <h4 class="color-1">
                    <asp:Label ID="lblTitle" runat="server"></asp:Label>
                </h4>
                <div class="heading-after-2">
                </div>
            </div>
            <div class="clear">
            </div>
            <div class="block-3">
                <p class="text-4">
                    <asp:Label ID="lblSubTitle" runat="server"></asp:Label>
                </p>
            </div>
        </div>
    </div>
    <br />
    <asp:Panel ID="pnlLogin" runat="server" DefaultButton="btnLogin">
        <table style="text-align: center; margin: auto;" class="contact-form table" align="center">
            <tr>
                <td style="text-align: left; vertical-align: middle;">
                    <asp:TextBox ID="txtLogin" CssClass="textbox" placeholder="User" runat="server"
                        Width="350px" MaxLength="255"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: left;">
                    <asp:TextBox CssClass="textbox" ID="txtSenha" placeholder="Password" runat="server"
                        Width="350px" MaxLength="255" TextMode="Password"></asp:TextBox>
                    &nbsp;&nbsp;
                </td>
            </tr>
        </table>
        <div style="text-align: center;">
            <asp:Label ID="lblAvisoLogin" runat="server" ForeColor="Red"></asp:Label>
            <br />
            <asp:Button CssClass="button-btn" ID="btnLogin" runat="server" Text="Login" />
            <asp:LinkButton ID="btnEsqSenha" runat="server" Font-Bold="False" Font-Underline="True"
                ForeColor="#0066FF" Visible="false">Forgot Password</asp:LinkButton>
        </div>
    </asp:Panel>
</asp:Content>
