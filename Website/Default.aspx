<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Website.Default1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script src="Scripts/jquery-2.0.3.min.js"></script>
<%--<script>
    $(document).ready(function test() {
        //$('.popup').click(function p() {
        //    alert("dsdsd");
        //});
        var btn = document.getElementById("cboxClose");
        alert(btn);
        btn.onclick = function() { // Note this is a function
            alert("blabla");
        };
        alert("sa");
    });
</script>--%>
    <script type="text/javascript">
        var homepage = '<%= defaultHome%>';
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div data-ng-class="{ show: widgets.length > 0, hide: widgets.length == 0 }" class="dashboard gridster hide" id="userWidget" data-ng-controller="DashCtrl">
        <div data-loading="" data-ng-class="{ hidden: wasLoaded }"></div>
        <ul class="list-gridster">
            <li data-row="{{widget.row}}" data-col="{{widget.col}}" data-sizex="{{widget.size}}" data-sizey="1" class="box-column {{widget.size | numberForString}}-column type-{{widget.typeId}}" data-widget="{{widget.id}}" dashboxes="" data-type="{{widget.typeId}}" data-fn="resizeLetter" data-param='{ "plus": ".box-plus", "minus": ".box-minus" }' data-ng-repeat="widget in widgets" data-ng-show="!category || widget.categoryId.indexOf(category) !== -1">
                <article class="box-border">
                    <header class="nv-1">
                        <div class="title-and-edit" data-ng-hide="widget.editTitle">
                            <h1 data-ng-click="showEditTitle($index);" data-ng-show="widget.isDeletable && widget.typeId != 5" data-ng-bind="widget.title"></h1>
                            <h1 data-ng-show="widget.typeId == 5">
                                <span data-ng-bind="widget.title"></span>
                                <span class="back-to-friends" data-ng-show="widget.typeId === 5 && facebookPhoto.viewDetail"><a href="javascript:void(0);" data-ng-click="updateFacebookFriendsList()">Back to all Friends</a></span>
                            </h1>
                            <h1 data-ng-hide="widget.isDeletable" data-ng-bind="widget.title"></h1>
                        </div>
                        <div class="custom-select" data-ng-show="widget.typeId === 5">
                            <input type="text" data-ng-model="facebookPhoto.friendName" ng-keyup="findFriend()" value="" />
                            <span class="icon-search-header"></span>

                            <ul class="custom-select-list" data-ng-show="userFriendsSearch">
                                <li data-ng-repeat="result in userFriendsSearch"><a href="#" data-ng-click="viewUserPhotos(result)" data-ng-bind="result.name"></a></li>
                            </ul>
                        </div>

                        <div class="options" data-ng-hide="widget.editTitle">
                            <ul>
                                <li class="hide-handhelds" data-ng-show="widget.isDeletable" data-ng-switch on="widget.typeId">
                                    <a href="#modalSliderAjax" title="EDIT" class="link-edit-name-widget" data-ngopenpop="/Templates/Settings/Feed.html" data-id="{{widget.id}}" data-fn="openPop" data-param='{ "type": "inline", "height": "auto", "width": "910px", "closeButton": false }' ng-switch-when="1" data-ng-click="edit(widget);">Edit</a>
                                    <a href="#modalSliderAjax" title="EDIT" class="link-edit-name-widget" data-ngopenpop="/Templates/Settings/MyWebsites.html" data-id="{{widget.id}}" data-fn="openPop" data-param='{ "type": "inline", "height": "auto", "width": "75%", "closeButton": false }' ng-switch-when="2" data-ng-click="edit(widget);">Edit</a>
                                    <a href="#modalSliderAjax" title="EDIT" class="link-edit-name-widget" data-ngopenpop="/Templates/Settings/SocialPhoto.html" data-id="{{widget.id}}" data-fn="openPop" data-param='{ "type": "inline", "height": "auto", "width": "910px", "closeButton": false }' ng-switch-when="5" data-ng-click="edit(widget);">Edit</a>
                                    <a href="#" title="EDIT" class="link-edit-name-widget" ng-switch-default data-ng-click="showEditTitle($index);">Edit</a>
                                </li>
                                <li data-ng-show="widget.typeId === 6 && widget.isSystemDefault && !widget.isDefault && widget.webmailUrl" data-ng-target="">
                                    <a href="#modalSliderAjax" class="link-edit-name-widget openIframe" data-id="{{widget.id}}" ng-href="{{widget.webmailUrl}}" data-fn="openPop" data-param='{ "type": "iframe" ,"width": "80%", "height": "80%" }'>Open Inbox</a>
                                </li>
                                <li data-ng-show="widget.typeId === 6 && widget.isSystemDefault && !widget.isDefault && widget.userHasEmailAccount">
                                    <a href="#modalSliderAjax" title="EDIT" class="link-edit-name-widget" data-ngopenpop="/Templates/Settings/EmailAccount.html" data-id="{{widget.id}}" data-fn="openPop" data-param='{ "type": "inline", "height": "auto", "width": "710px", "close": "TAKE ME <span class=bold>HOME</span>" }'>Edit</a>
                                </li>
                                <li class="change-zoom-box">
                                    <a title="Increase text size" class="box-plus">A+</a>
                                    <a title="Decrease text size" class="box-minus">A-</a>
                                </li>
                                <li class="move-box">
                                    <a class="icon icon-move">+</a>
                                </li>
                                <li class="hide-handhelds" data-ng-show="widget.isDeletable">
                                    <a href="#modalSliderAjax" title="DELETE" data-ngopenpop="/Templates/DeleteWidget.html" data-ng-click="delSelected(widget.id, $index);" data-fn="openPop" data-param='{ "type": "inline" ,"width": 400, "height": 400, "close": "TAKE ME <span class=bold>HOME</span>" }' class="icon icon-delete-widget"></a>
                                </li>
                            </ul>
                        </div>
                        <form ng-submit="submitTitle($index);" class="edit-title" data-ng-show="widget.editTitle">
                            <!-- ng-show="widget.editTitle && (widget.typeId !== 1 && widget.typeId !== 2)" -->
                            <input type="text" ng-model="widget.newTitle" />
                            <div class="options">
                                <ul>
                                    <li ng-show="widget.isDeletable">
                                        <button type="submit">Ok</button>
                                    </li>
                                    <li class="change-zoom-box">
                                        <a class="link-cancel-name-widget" data-ng-click="widget.editTitle = false">Cancel</a>
                                    </li>
                                </ul>
                            </div>
                        </form>
                    </header>
                    <div class="content" id="content_{{widget.id}}" ng-switch on="widget.typeId">
                        <div ng-switch-when="1" ng-id="{{widget.id}}">
                            <div ng-include="'/Templates/Feed.html'"></div>
                        </div>
                        <div ng-switch-when="2" ng-id="{{widget.id}}">
                            <div ng-include="'/Templates/MyWebsite.html'"></div>
                        </div>
                        <div ng-switch-when="3" ng-id="{{widget.id}}">
                            <div ng-include="'/Templates/Facebook.html'"></div>
                        </div>
                        <div ng-switch-when="4" ng-id="{{widget.id}}">
                            <div ng-include="'/Templates/WeatherForecast.html'"></div>
                        </div>
                        <div ng-switch-when="5" ng-id="{{widget.id}}">
                            <div ng-include="'/Templates/FacebookPhotos.html'"></div>
                        </div>
                        <div ng-switch-when="6" ng-id="{{widget.id}}" ng-show="widget.isSystemDefault && !widget.isDefault">
                            <div ng-include="'/Templates/EmailFeed.html'"></div>
                        </div>
                        <div ng-switch-when="6" ng-id="{{widget.id}}" ng-hide="widget.isSystemDefault && !widget.isDefault">
                            <div ng-include="'/Templates/Email.html'"></div>
                        </div>
                    </div>

                    <footer class="nv-1" data-ng-hide="widget.typeId === 3 || widget.typeId === 5 || widget.typeId === 6 || widget.sourcesLength <= 8">
                        <a href="#modalSliderAjax" ng-hide="widget.typeId === 4" data-ngopenpop="{{widget.typeId | linkService}}" title="View More" data-id="{{widget.id}}" data-index-widget="{{$index}}" data-title="{{widget.title}}" data-fn="openPop" class="f-left" data-param='{ "type": "inline" ,"width": "{{widget.typeId | sizeLightbox}}", "height": "auto", "close": "TAKE ME <span class=bold>HOME</span>" }'>View <span class="bold">More</span></a>
                        <%--<a  href="#modalSliderAjax" data-ngopenpop="/Templates/WeatherForecast.html" data-fn="openPop" data-param='{ "type": "inline", "width": 950, "height": 420, "closeButton": false }' title="Extend Forecast" ng-show="widget.typeId === 4" title="Extended Forecast" data-id="{{widget.id}}" data-index-widget="{{$index}}" data-title="{{widget.title}}">Extend Forecast</a>--%>
                        <%--<a href="#modalSliderAjax" data-ngopenpop="/Services/{{widget.typeId | categoryWdiget}}.aspx?widgetId={{widget.id}}&widgetName={{widget.title}}" data-fn="openPop" class="f-left" data-fn="openPop" data-param='{ "type": "inline" ,"width": "800px", "close": "TAKE ME <span class=bold>HOME</span>" }'>View <span class="bold">More</span></a>--%>
                        <%--<% if (this.WidgetList[i].SystemTagId == (int)Model.Enum.enWidgetType.BOOKMARK)
                            {%>
                        <a href="/Services/Bookmark.aspx?widgetId=<%=this.WidgetList[i].Id %>&widgetName=<%=HttpUtility.UrlEncode(this.WidgetList[i].Name) %>" class="f-left" data-fn="openPop" data-param='{ "type": "ajax" ,"width": "800px", "close": "TAKE ME <span class=bold>HOME</span>" }'>View <span class="bold">More</span></a>
                        <a href="/Services/Add-New-Widget.aspx" data-fn="openPop" data-param='{ "type": "ajax" ,"width": "80%", "resizeBoxLightbox": true, "closeButton": false }' class="f-right">
                        <%}else{ %>
                        <a href="/Services/Feed.aspx?widgetId=<%=this.WidgetList[i].Id %>&widgetName=<%=HttpUtility.UrlEncode(this.WidgetList[i].Name) %>" class="f-left" data-fn="openPop" data-param='{ "type": "ajax" ,"width": "80%", "height":"800px", "close": "TAKE ME <span class=bold>HOME</span>" }'>View <span class="bold">More</span></a>
                        <a href="/Services/Add-New-Widget.aspx" data-fn="openPop" data-param='{ "type": "ajax" ,"width": "80%", "resizeBoxLightbox": true, "closeButton": false }' class="f-right">
                        <%} %>
                        </a>--%>
                    </footer>
                </article>
            </li>
        </ul>
    </div>

    <form id="frmController" runat="server"></form>
</asp:Content>
