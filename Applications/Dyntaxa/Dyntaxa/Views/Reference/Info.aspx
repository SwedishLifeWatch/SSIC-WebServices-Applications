<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.Reference.ReferenceInfoViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.Labels.TitleLabel %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h1 class="readHeader">
        <%: Model.Labels.TitleLabel %>        
    </h1>

    <% if(Model.TaxonId.HasValue)
       { %>
        <% if (ViewBag.Taxon != null)
            { %>
            <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
        <% }
            else
            { %>
            <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
        <% } %>        
    <% } %>
    
    <% Html.RenderAction("GuidObjectInfo", "Reference", new { guid = Model.Guid }); %>
    
    <!-- Full container start -->
    <div id="fullContainer">
        <fieldset>
            <h2><%: Model.Labels.ReferencesLabel %></h2>
            <div class="fieldsetContent">              
			    
             <% var grid = new WebGrid(source: Model.References,
                                            canSort: false,
                                            rowsPerPage: 20); %>
                           
            <%:grid.GetHtml(
                tableStyle: "grid",
                headerStyle: "head",
                alternatingRowStyle: "alt",
                columns: grid.Columns(
                        //grid.Column(columnName: "Id", header: Model.Labels.ColumnTitleId),
                        grid.Column(columnName: "Name", header: Model.Labels.ColumnTitleName),
                        grid.Column(columnName: "Year", header: Model.Labels.ColumnTitleYear),
                        grid.Column(columnName: "Text", header: Model.Labels.ColumnTitleText),                            
                        grid.Column(columnName: "Usage", header: Model.Labels.ColumnTitleUsage)                                                          
                    )
                )    
                %>

            </div>
        </fieldset>
    </div>
        

</asp:Content>
