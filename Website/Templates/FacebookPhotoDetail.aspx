<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FacebookPhotoDetail.aspx.cs" Inherits="Website.Templates.FacebookPhotoDetail" %>


<link href="/Css/Main.css" type="text/css" rel="stylesheet" />

<div class="fb-photo-detail">
    <img src="<%= Request.QueryString["p"] %>" alt="<%= Request.QueryString["c"] %>" />
    <div class="caption">
        <p><%= Request.QueryString["c"] %></p>
    </div>
</div>

