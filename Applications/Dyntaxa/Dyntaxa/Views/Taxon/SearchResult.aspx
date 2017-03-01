<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonSearchViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@Import namespace="Dyntaxa.Helpers.Extensions" %>
<%@Import namespace="Dyntaxa.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.Labels.TaxonSearchLabel %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% using (Html.BeginForm("SearchResult","Taxon", FormMethod.Post, new {@name = "searchForm", @id = "searchform"}))
   {%>
    <%: Html.HiddenFor(m => m.IsAmbiguousResult) %>
    <%: Html.HiddenFor(m => m.LinkAction) %>
    <%: Html.HiddenFor(m => m.LinkController) %>
    <%: Html.HiddenFor(m => m.LinkParamsString) %>
  
    <% if (ViewBag.MiniMode == null || ViewBag.MiniMode == false) 
      { %>
        <% if (ViewBag.Taxon != null)
           { %>
            <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon, ignoreExpand = true }); %>    
        <% }
           else
           { %>
            <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = ViewBag.TaxonId, ignoreExpand = true }); %>    
        <% } %>    
    <% }
       else
       {
           if (ViewBag.Taxon != null)
           { %>
                <a href="<%= Url.Content("~/Taxon/Info/")%><%: ViewBag.TaxonId %>?mode=mini"><%: Model.Labels.TaxonRestMasterViewCurrentTaxon %> <%: ViewBag.TaxonId %>)</a>
           <% } 
       }%>


    <% if (!Model.IsTaxonExisting || !Model.IsTaxonInRevision || Model.IsAmbiguousResult)
       {%>
        <fieldset class="limitToTaxonFieldset">
            <h2 class="validationSummaryHeader">
            
                <% if (!Model.IsTaxonExisting)
                   { %>
                     <%: Resources.DyntaxaResource.TaxonSearchTaxonNotExistErrorText %>: <%: string.Format(" ~{0}/{1}/", Model.LinkController, Model.LinkAction) %>
                <% } %>
                <%
                   else if (!Model.IsTaxonInRevision)
                   {%>
                     <%: Resources.DyntaxaResource.TaxonSearchTaxonNotInRevisionErrorText %>: <%: string.Format(" ~{0}/{1}/", Model.LinkController, Model.LinkAction) %>
                <% } %>
                <%
                   else if (Model.IsAmbiguousResult)
                   {%>
                    <% if (Model.IsZeroRowsResult)
                       {%>
                        <%: string.Format(Resources.DyntaxaResource.TaxonSearchZeroResults, Model.SearchString) %>
                    <% }
                       else
                       { %>
                         <%: Resources.DyntaxaResource.TaxonSearchAmbiguousResult %>: <%: string.Format(" /{0}/{1}/", Model.LinkController, Model.LinkAction) %>
                     <% } %>
                <% } %>

            </h2>
        </fieldset>
    <% }%>
    

<div id="fullContainer">
<%--
<% if (!Model.IsTaxonExisting) { %>
<div id="messageArea2" >
     <%: Resources.DyntaxaResource.TaxonSearchTaxonNotExistErrorText %>: <%: string.Format(" ~{0}/{1}/", Model.LinkController, Model.LinkAction) %>
</div>
<% } %>
<% else if (!Model.IsTaxonInRevision) { %>
<div id="messageArea3" >
     <%: Resources.DyntaxaResource.TaxonSearchTaxonNotInRevisionErrorText %>: <%: string.Format(" ~{0}/{1}/", Model.LinkController, Model.LinkAction) %>
</div>
<% } %>
<% else if (Model.IsAmbiguousResult) { %>
<div id="messageArea">
    <% if (Model.IsZeroRowsResult)
       {%>
        <%: string.Format(Resources.DyntaxaResource.TaxonSearchZeroResults, Model.SearchString) %>
    <% }
       else
    { %>
         <%:Resources.DyntaxaResource.TaxonSearchAmbiguousResult%>: <%:string.Format(" /{0}/{1}/", Model.LinkController, Model.LinkAction)%>
     <% } %>
</div>
<% } %>
--%>
<fieldset>
<h2><%: Model.Labels.TaxonSearchLabel %></h2>
<div id="upperSearchArea"> 
    <div class="searchBox">       
        <table class="display-table">
            <tr>
                <td>
                    <%:Html.TextBoxFor(m => m.SearchString)%>
                </td>
                <td>
                    <button type="submit" name="button">
                        <img alt="Search" src="<%=Url.Content("~/Images/Icons/search4.png")%>" style="vertical-align: text-top;" />
                    </button>
                </td>
            </tr>
        </table>
    </div>
</div>
</fieldset>

<fieldset>
    <h2 id="searchOptions" class="closed savestate"><%: Model.Labels.SearchOptionsLabel %></h2>
    <div class="fieldsetContent">        
        <% Html.RenderPartial("SearchOptions", Model.SearchOptions, new ViewDataDictionary { TemplateInfo = new TemplateInfo { HtmlFieldPrefix = "SearchOptions" } }); %>        
        <button type="submit" name="button" style="float: left;">
            <img alt="Search" src="<%=Url.Content("~/Images/Icons/search4.png")%>" style="vertical-align: text-top;" />
        </button>        

        <%: Html.ActionLink(Resources.DyntaxaResource.TaxonSearchResetSearchOptions, "ResetSearchOptions", "Taxon", null, new Dictionary<string, object>() { { "class", "ap-ui-button" }, { "style", "margin-left: 10px;" } })%>        
    </div>
</fieldset>

<fieldset>
    <div id="numberOfResultsArea">
        <h2><%: Resources.DyntaxaResource.TaxonSearchResult %>: <%: Model.NumberOfResults %> <%: Resources.DyntaxaResource.TaxonSearchHits %> (<%: string.Format("{0:N2}", Model.SearchTime.TotalSeconds) %> <%: Resources.DyntaxaResource.TaxonSearchSeconds %>)</h2>
    </div>
    
    <div id="resultsArea">    
        <% if (!string.IsNullOrEmpty(Model.SearchString))               
         {%>
            <% List<Tuple<string, string>> searchDescription = Model.SearchOptions.GetSearchDescription(); %>
            <% if (searchDescription.Count > 0)
               { %>
                <div style="margin-left: 10px;">
                    <%: Resources.DyntaxaResource.TaxonSearchSettings%>:
                    <ul style="list-style: disc inside none; margin-left: 10px;">
                    <% foreach (Tuple<string, string> tuple in searchDescription)
                       { %>    
                        <li><%: tuple.Item1%>: <em><%: tuple.Item2%></em></li>
                    <%} %>                    
                    </ul>
                </div>
            <%}%>
        <%}%>

        
    <% if (Model.SearchResult != null && Model.SearchResult.Count > 0)
    {%>
    <% var grid = new WebGrid(source: Model.SearchResult, rowsPerPage: 40);
        string gridCssStyle = "width:924px;";
        if (ViewBag.MiniMode != null && ViewBag.MiniMode)
        {
            gridCssStyle = "width:848px;";
        }
    %>
                            
    <%:grid.GetHtml(
        tableStyle: "grid",
        headerStyle: "webgrid-header",
        footerStyle: "webgrid-footer",
        //selectedRowStyle: "webgrid-selected-row",
        alternatingRowStyle: "alt",
        mode: WebGridPagerModes.All,
        firstText: Resources.DyntaxaResource.SharedWebGridFirstPage,
        lastText: Resources.DyntaxaResource.SharedWebGridLastPage,
        htmlAttributes: new { @style = gridCssStyle },        
        columns: grid.Columns(
                grid.Column(columnName: "SearchMatchName", header: Resources.DyntaxaResource.TaxonSearchHit,
                    format: (item) =>
                                {
                                    TaxonSearchResultItem searchResultItem = item.Value;
                                    string title = searchResultItem.SearchMatchName;
                                    if (!string.IsNullOrEmpty(searchResultItem.SearchMatchAuthor))
                                    {
                                        title += " " + searchResultItem.SearchMatchAuthor;
                                    }
                                    MvcHtmlString strHtml = Html.ActionLink(((string)title), Model.LinkAction, Model.LinkController, DictionaryHelper.MergeDictionaries(Model.LinkParams, new { @taxonId = item.TaxonId, @changeRoot = true }), null);                        
                                    return strHtml;
                    }),
                grid.Column(columnName: "NameCategory", header: Resources.DyntaxaResource.TaxonSearchNameCategory),
                grid.Column(columnName: "ScientificName", header: Resources.DyntaxaResource.TaxonSharedScientificName),
                grid.Column(columnName: "Author", header: Resources.DyntaxaResource.TaxonSharedAuthor),
                grid.Column(columnName: "CommonName", header: Resources.DyntaxaResource.TaxonSharedCommonName),
                grid.Column(columnName: "Category", header: Resources.DyntaxaResource.TaxonSharedCategory),
                grid.Column(columnName: "TaxonId", header: Resources.DyntaxaResource.TaxonSearchTaxonId),
                grid.Column(columnName: "TaxonStatus", header: " ",
                    //format: (item) => Html.ActionLink(((string)item.SearchMatchName), Model.LinkAction, Model.LinkController, DictionaryHelper.MergeDictionaries(Model.LinkParams, new { @taxonId = item.TaxonId, @changeRoot = true }), null)
                    format: (item) =>
                                {
                                    TaxonSearchResultItem searchResultItem = item.Value;
                                    return Html.Image(Url.Content(searchResultItem.StatusImageUrl), "", new {width = "12", height = "12"});
                                }
                    )
                                
            //grid.Column(columnName: "CommonName", header: "Common name",
            //        format: (item) => Html.ActionLink(((string)item.CommonName), Model.LinkAction, Model.LinkController, DictionaryHelper.MergeDictionaries(Model.LinkParams, new { @Id = item.Id }), null)),                
            //grid.Column(columnName: "Id", header: "Id"),                
            )
        )    
        %>
    <% } %>
    </div>

</fieldset>
    
<%-- TODO: Set dynamic path unstead of relative --%>
<div id="SearchLoader" style="display:none;">
    <h1><img src="/Images/Icons/ajax-loader.gif" /> <%: Resources.DyntaxaResource.TaxonSearchSearching %></h1>
</div>

<%--<% if (ViewBag.MiniMode != null && ViewBag.MiniMode == true)
   { %>
    <% Html.RenderAction("TaxonTree", "Navigation", new { taxonId = ViewBag.RootTaxonId, revisionId = ViewBag.RevisionId, hideChangeRootTaxon = true }); %>  
<% } %>--%>

</div>
<% } %>

<script type="text/javascript">
    $(document).ready(function () {
        $("#SearchString").focus().select();

        //        $('fieldset h2#searchOptions').click(function () {
        //            $(this)
        //            .toggleClass("closed")
        //            .toggleClass("open")
        //            .closest("fieldset").find("div.fieldsetContent").slideToggle('fast', function () {
        //                // Animation complete.
        //            });
        //        });
        //        $('fieldset h2#searchOptions.closed').closest("fieldset").find("div.fieldsetContent").hide();
        //initToggleFieldsetH2();

        $("#searchform").submit(function () {
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: $('#SearchLoader') });
        });

//        $("#restoreDefaultSearchOptions").click(function() {

//        });

        $('table.grid tbody tr').live('hover', function () {
            $(this).toggleClass('pantone397Cp40');
        }).live('click', function () {
            location.href = '<%= Url.Content("~/") %>Taxon/Info/' + $(this).find('td:last').prev().text() + '?changeRoot=True';
        });

    }); 
    
</script> 

</asp:Content>
