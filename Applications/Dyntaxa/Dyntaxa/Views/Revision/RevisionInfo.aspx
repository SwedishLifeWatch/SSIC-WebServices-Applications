<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.RevisionInfoViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.RevisionEditingActionHeaderText%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="readHeader">
        <%: Model.RevisionEditingHeaderText%>
    </h1>

    <% if (ViewBag.Taxon != null)
        { %>
        <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
    <% }
        else
        { %>
        <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
    <% } %>
    
     <!-- Full container start -->
    <div id="fullContainer">
        <% foreach (RevisionInfoItemModelHelper item in Model.RevisionInfoItems)
        { %>
              <input type="hidden" id="revisionId" name="revisionId" value="<%:item.RevisionId%>" />
              <input type="hidden" id="taxonId" name="taxonId" value="<%:Model.TaxonId%>" />
              <%: Html.Partial("~/Views/Shared/RevisionInfoControl.ascx", item)%>

        <% } %>
        <fieldset>
        <p>
              <%: Html.ActionLink(Model.Labels.ReturnToListText, "List", "Revision", new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>            
        </p>
        </fieldset>
        <!-- full container end -->
    </div>
    <!-- This code will be executed in the partial view RevisionInfoControl.ascx. Addressed when document has been loaded. -->
       <script type="text/javascript">
           $(document).ready(function () {
               // animate visibility for fieldsets
               //initToggleFieldsetH2();
           });
        </script>
</asp:Content>
