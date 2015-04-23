<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main-Menu.master" AutoEventWireup="true" CodeBehind="SuggestionBox_Admin.aspx.cs" Inherits="WebAdmin.Page.Page.SuggestionBox_Admin" %>
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
            <td style="text-align: right; width: 40%;">Website Name: </td>
            <td style="text-align: left; width: 60%;">
                <asp:TextBox CssClass="textbox" ID="txtSuggestionBoxName" runat="server" Width="200px"
                    MaxLength="256"></asp:TextBox>
                &nbsp;&nbsp;
                <asp:Label CssClass="alert" ID="lblName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr id="link">
            <td style="text-align: right; width: 40%;">Website URL:</td>
            <td style="text-align: left; width: 60%;">
                <asp:TextBox CssClass="textbox" ID="txtSuggestionBoxUrl" runat="server" Width="200px"
                    MaxLength="256"></asp:TextBox>
                &nbsp;&nbsp;
                <asp:Label CssClass="alert" ID="lblUrl" runat="server"></asp:Label>
            </td>
        </tr>
        <tr id="auxText">
            <td style="text-align: right; " class="auto-style1">Website Short Description:
            </td>
            <td style="text-align: left; " class="auto-style2">
                <asp:TextBox CssClass="textbox" ID="txtSuggestionBoxDescription" runat="server" Width="250px"
                    MaxLength="256" Height="100px" TextMode="MultiLine"></asp:TextBox>
                &nbsp;&nbsp;
                <asp:Label CssClass="alert" ID="lblShortDescription" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 40%;">Website Logo/Image: (<%=ConfigurationManager.AppSettings["Application.Upload.Image.SuggestionBox.Width"] %>x<%=ConfigurationManager.AppSettings["Application.Upload.Image.SuggestionBox.Height"] %>)

            </td>
            <td style="text-align: left; width: 60%;">
                <asp:UpdatePanel ID="upFile" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:TextBox CssClass="textbox" ID="txtUploadSuggestionBoxImage" runat="server" Width="200px"
                            MaxLength="256"></asp:TextBox>
                        <asp:Image ID="imgPreview" runat="server" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="timerCheckLink" EventName="Tick" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:Label CssClass="alert" ID="lblLogo" runat="server"></asp:Label>
                <asp:Button ID="btnSelectImages" runat="server" CssClass="button-btn"
                    Text="Browse Files" />
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 40%;">Related Interests:
            </td>
            <td style="text-align: left; width: 60%;">
                <asp:CheckBoxList ID="cblRelatedInterests" runat="server" RepeatColumns="4">
                </asp:CheckBoxList>
                &nbsp;&nbsp;
                <asp:Label CssClass="alert" ID="lblRelatedInterests" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <div style="width: 100%; text-align: center;">
        <asp:Button CssClass="button-btn" ID="btnSave" runat="server" Text="<%$ Resources:Language, Save %>" />
        <asp:Button CssClass="button-btn" ID="btnCancel" runat="server" Text="<%$ Resources:Language, Cancel %>" />
    </div>
</asp:Content>
