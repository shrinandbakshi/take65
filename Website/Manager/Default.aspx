<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Master/Manager.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Website.Manager.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Default.aspx.js"></script>
    <link href="/Html/Css/Colorbox.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cntMain" runat="server">
    <div class="span12 no_space">

                  <div class="widget">
                    <header>
                      <h3>Feed Content <span>- Manager</span></h3>
                      <ul class="toggle_content">                         
                          <li class="arrow"><a href="#">Toggle Content</a></li>
                      </ul>
                    </header>
                    <section class="welly"> 

                     
                      <h5 class="titlee group">
                        Please select below the channel to browser the content

                        <div class="btn-group-cont">
                          <div class="btn-group" data-toggle="buttons-radio" data-original-title="Position">
                            <button id="posLeft" type="button" class="btn btn-mini"><i class="icon-chevron-left"></i> Previous</button>                          
                            <button id="posRight" type="button" class="btn btn-mini">Next <i class="icon-chevron-right"></i></button>
                          </div>                      
                        </div>

                      </h5>      
                                      
                      <div id="tabStyles" class="tabbable tabs-left">
                        <ul class="nav nav-tabs tab_styled_light">
                          <asp:Repeater ID="rptMenu" runat="server">
                            <ItemTemplate>
                                <li><a id="lnk<%# ((Model.TrustedSource)Container.DataItem).Name.Replace(" ", "")%>" href="#<%# ((Model.TrustedSource)Container.DataItem).Name.Replace(" ", "")%>" data-toggle="tab"><%# ((Model.TrustedSource)Container.DataItem).Name%></a></li>  
                            </ItemTemplate>
                          </asp:Repeater>
                        </ul>                        
                        <div class="tab-content tab_content_light">
                          <asp:Repeater ID="rptContentDiv" runat="server">
                              <ItemTemplate>
                                  <div id="<%# ((Model.TrustedSource)Container.DataItem).Name.Replace(" ", "")%>" class="tab-pane fade">
                                    <asp:Repeater ID="rptContent" runat="server">
                                        <ItemTemplate>
                                            <p>
                                                <b><%# ((Model.FeedContent)Container.DataItem).Title%></b> | <%# ((Model.FeedContent)Container.DataItem).PublishedDate.ToString("MM/dd/yyyy HH:ss")%><br />
                                                <%# RemoveHtmlTags(((Model.FeedContent)Container.DataItem).Description)%><br />
                                                <img src="<%# ((Model.FeedContent)Container.DataItem).Thumb%>" /><br /><br />
                                                <a href="<%# ((Model.FeedContent)Container.DataItem).Link%>" class="colorbox">View More</a>
                                            </p>
                                            <hr />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                  </div>
                              </ItemTemplate>
                          </asp:Repeater>
                        </div>                         
                      </div>                                                           
                      <hr>
                                                         
                      <hr>
                    </section>
                  </div>

                                    
                  
              </div>
              <script>
                  function SetTotalNews(pLink, pTotal) {
                      var lnkObj = document.getElementById("lnk" + pLink);
                      lnkObj.innerHTML = lnkObj.innerHTML + " (" + pTotal + ")";
                  }
                  <%=JsEnd %>
            </script>
            <script src="/Html/Js/jquery.colorbox-min.js"></script>
    <script>
        $(function () {
            $('.colorbox').colorbox({
                iframe: true,
                width: 1200,
                height: 900
            });
        });
    </script>
</asp:Content>
