<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.AboutViewModel>" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.Labels.SharedAboutDyntaxaText%>
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
     <h1 class="readHeader">
       <%: Model.Labels.SharedAboutDyntaxaText %>
      </h1>
   
        <div id="fullContainer">
        <fieldset>
            <h2><%: Model.Labels.SharedDialogInformationHeader%></h2>
            <div class="fieldsetContent">
                <h3 class="open">
                    <%: Model.Labels.DyntaxaUserName%>
                </h3>
                <div class="fieldsetSubContent fullWidth"> 
                <ul>
                    <li>Server: <%:Model.ServerName%></li>
                    <li><%: Model.Labels.SharedVersionText%>: <%:Model.Version%></li>
                    <li><%: Model.Labels.SharedDateText%>: <%:Model.CreationDate%></li>
                </ul>
                <% if (ViewBag.ShowClearCacheLink != null && ViewBag.ShowClearCacheLink)
                   { %>
                    <span style="margin:4px; padding:5px;"><%:Html.ActionLink("Clear cache", "ClearCache", "Taxon")%></span>
                <% } %>
            </div>
            </div>
        </fieldset>   
         <!-- full container end -->
    </div>
</asp:Content>


