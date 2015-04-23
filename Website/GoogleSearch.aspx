<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="GoogleSearch.aspx.cs" Inherits="Website.GoogleSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <style>
        
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    
    <div id="SearchContent" class="google-search" data-ng-controller="GoogleSearchCtrl">
        <a href="/" class="gs-btn-take-me-home">
            <button type="button" class="button" id="cboxClose">TAKE ME <span class="bold">HOME</span></button>
        </a>
            <ol id="rso" eid="UDaVUoyqJ6ewsQS_i4HIBQ" data-ng-target="">
	            <li class="g" data-ng-repeat="result in searchResult">
		            <!--m-->
		            <div class="rc">
			            <span class="altcts"></span>
			            <h3 class="r">
				            <a ng-href="{{result.unescapedUrl}}" class="openIframe" data-fn="openPop" data-param='{ "type": "iframe" ,"width": "80%", "height":"80%", "close": "TAKE ME <span class=bold>HOME</span>"}'>{{result.titleNoFormatting}}</a>
			            </h3>
			            <div class="s">
				            <div>
					            <div style="white-space:nowrap" class="f kv">
						            <cite class="vurls"><a data-ng-href="{{result.unescapedUrl}}" class="openIframe" data-fn="openPop" data-param='{ "type": "iframe" ,"width": "80%", "height":"80%", "close": "TAKE ME <span class=bold>HOME</span>"}'>{{result.visibleUrl}}</a></cite>
					            </div>
					            <div class="f slp"></div>
					            <span class="st" data-ng-bind-html="result.content">
						        
                                </span>
				            </div>
			            </div>
		            </div>
		            <!--n-->
	            </li>
            </ol>
        
    </div>
</asp:Content>
