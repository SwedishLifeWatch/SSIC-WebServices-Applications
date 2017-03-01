<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonSearchViewModel>" %>
<%@ Import Namespace="ArtDatabanken.Data" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Search
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm())
   {%>
<div id="fullContainer">
    
    <div class="searchForm">
        <div id="searchLogo">
            <div class="searchLogoText1">Dyntaxa</div>
            <div class="searchLogoText2">Taxonsök</div>            
        </div>
        <div class="searchTip">
            Här kan du söka efter arter och andra taxa som förekommer i Sverige
        </div>

        <div class="searchBox">
            <%:Html.TextBoxFor(m => m.SearchString)%>            
            <button name="button"><img alt="Search" src="<%= Url.Content("~/Images/Icons/search4.png")%>" /></button>                            
        </div>
    </div>

</div>
<% } %>

<script type="text/javascript">
    $(document).ready(function () {
        $("#SearchString").focus();
    }); 
</script> 
</asp:Content>
