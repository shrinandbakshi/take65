<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main-Menu.master" AutoEventWireup="true" CodeBehind="SuggestionBox.aspx.cs" Inherits="WebAdmin.Page.Page.SuggestionBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPage" runat="server">
    <div class="ident-bot-9">
        <div class="heading-wrapper-2 ident-bot-8">
            <div class="heading-before-2">
            </div>
            <h4 class="color-1">
                <asp:Label ID="lblTitle" runat="server" CssClass="capitalize"></asp:Label>
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
    <div class="div-button float-left">
        <asp:Button CssClass="button-btn" ID="btnDelete" runat="server" Text="<%$ Resources:Language, Delete %>" />
        <asp:Button CssClass="button-btn" ID="btnAdd" runat="server" Text="<%$ Resources:Language, Add %>"
            EnableTheming="True" />
        <asp:TextBox ID="txtPesquisar" CssClass="textbox" runat="server" Width="300px"
            Visible="False"></asp:TextBox>
        <asp:Button CssClass="button-btn" ID="btnSearch" runat="server"
            Text="<%$ Resources:Language, Search %>" Visible="False" />
    </div>
    <div runat="server" id="divSearchAddDel" visible="false">
        <div id="lbl-right-GoTo" class="div-button float-right" style="margin-left: 5px;">
            <asp:TextBox ID="txtGoTo" CssClass="textbox-goto float-left integer" runat="server"
                Width="35px" MaxLength="5" Text="<%$ Resources:Language, GoToItem %>"></asp:TextBox>
            <asp:ImageButton ID="btnGoTo" runat="server" CssClass="button-btn float-right" ImageUrl="~/Img/Icon/icon_arrow-right.png" />
        </div>
        <div id="btn-right" class="div-button float-right">
            <asp:ImageButton ID="btnBefore" runat="server" CssClass="button-btn float-left" ImageUrl="~/Img/Icon/icon_arrow-left.png" />
            <asp:ImageButton ID="btnNext" runat="server" CssClass="button-btn float-right" ImageUrl="~/Img/Icon/icon_arrow-right.png" />
        </div>
        <div id="lbl-right" class="div-label float-right">
            <asp:Label ID="lblInitialItem" runat="server" Font-Bold="True"></asp:Label>
            <asp:Label ID="lblTo" CssClass="lowercase" runat="server" Text="<%$ Resources:Language, At %>"></asp:Label>
            <asp:Label ID="lblLastItem" runat="server" Font-Bold="True"></asp:Label>
            <asp:Label ID="lblOf" CssClass="lowercase" runat="server" Text="<%$ Resources:Language, Of %>"></asp:Label>
            <asp:Label ID="lblTotalItens" CssClass="lowercase" runat="server" Text=""></asp:Label>
        </div>
    </div>
    <div class="clear">
    </div>
    <asp:Panel ID="pnlItemsNotFound" Visible="false" runat="server">
        <div class="table" style="width: 100%; text-align: center;">
            <div class="clear">
            </div>
            <%= Resources.Language.ItemsNotFound %>
            <div class="clear">
            </div>
        </div>
    </asp:Panel>
    <asp:GridView CssClass="table" ID="gdvAdmin" runat="server" AutoGenerateColumns="False"
        OnRowDataBound="gdvAdmin_RowDataBound" GridLines="None" Width="100%" CellSpacing="-1"
        DataKeyNames="Id">
        <Columns>
            <asp:TemplateField ItemStyle-Width="18px">
                <HeaderTemplate>
                    <asp:CheckBox ID="chkButtonAll" onclick="javascript:SelectAllCheckboxes(this);" runat="server" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox onclick="javascript:HighlightRow(this);" ID="chkButton" runat="server" />
                    <asp:Label ID="lblID" runat="server" Text='<%# Bind("Id") %>' Visible="False"></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="Name" HeaderText="Title" SortExpression="Name" HtmlEncode="false" />
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" HtmlEncode="false" />
        </Columns>
    </asp:GridView>
</asp:Content>
