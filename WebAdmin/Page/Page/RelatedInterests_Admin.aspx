<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main-Menu.master" AutoEventWireup="true" CodeBehind="RelatedInterests_Admin.aspx.cs" Inherits="WebAdmin.Page.Page.RelatedInterests_Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPage" runat="server">
    <div style="display: none;">
        <asp:Timer ID="timerCheckLink" runat="server" OnTick="timerCheckLink_Tick" Interval="1000"></asp:Timer>
    </div>
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
    <table class="table" style="width: 100%; cellspacing: 0">
        <tr id="name">
            <td style="text-align: right; width: 40%;">Related Interests Name: </td>
            <td style="text-align: left; width: 60%;">
                <asp:TextBox CssClass="textbox" ID="txtName" runat="server" Width="200px"
                    MaxLength="256"></asp:TextBox>
                &nbsp;&nbsp;
                <asp:Label CssClass="alert" ID="lblName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 40%;">Icon:

            </td>
            <td style="text-align: left; width: 60%;">
                <asp:UpdatePanel ID="upFile" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:TextBox CssClass="textbox" ID="txtIcon" runat="server" Width="200px"
                            MaxLength="256"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="timerCheckLink" EventName="Tick" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:Label CssClass="alert" ID="lblIcon" runat="server"></asp:Label>
                <asp:Button ID="btnSelectImages" runat="server" CssClass="button-btn"
                    Text="Selecionar Imagem" />
            </td>
        </tr>
        </table>
    <br />
    <div style="width: 100%; text-align: center;">
        <asp:Button CssClass="button-btn" ID="btnSave" runat="server" Text="<%$ Resources:Language, Save %>" />
        <asp:Button CssClass="button-btn" ID="btnCancel" runat="server" Text="<%$ Resources:Language, Cancel %>" />
    </div>
</asp:Content>
