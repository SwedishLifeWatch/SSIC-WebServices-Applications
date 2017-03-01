<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.Reference.ReferenceListViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Lista
    <%--<%:Resources.DyntaxaResource.TaxonNameEditTitle %>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="readHeader">
        Lista
        <%--<%:Resources.DyntaxaResource.TaxonNameEditTitle %>--%>
    </h1>
    <%--<% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>--%>
    
    <!-- Full container start -->
    <div id="fullContainer">
        <fieldset>
            <h2><%: Resources.DyntaxaResource.TaxonNameAddEditNameHeader%></h2>
            <div class="fieldsetContent">
                <% var grid = new WebGrid(source: Model.References, canSort: true, canPage: true, defaultSort: "Id", rowsPerPage: 40); %>
                <%:
               grid.GetHtml(
                   tableStyle: "grid",
                   headerStyle: "head",
                   alternatingRowStyle: "alt",
                   numericLinksCount: 40,
                   columns: grid.Columns(
                       grid.Column(columnName: "Id", header: "Id"),
                        grid.Column(columnName: "Name", header: "Name"),
                        grid.Column(columnName: "Text", header: "Text"),
                        grid.Column(columnName: "Year", header: "Year")
                      )
                   )                                           
                %>  

                
            </div>
        </fieldset>
    </div>

</asp:Content>
