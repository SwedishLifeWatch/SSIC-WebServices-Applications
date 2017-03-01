<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.SortChildTaxaViewModel>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">    
    <%: Resources.DyntaxaResource.TaxonSortChildTaxaTitle %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h1 class="readHeader"><%: Resources.DyntaxaResource.TaxonSortChildTaxaTitle %></h1>
   
<% if (ViewBag.Taxon != null)
    { %>
    <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
<% }
    else
    { %>
    <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
<% } %>     

        <% if (!this.ViewData.ModelState.IsValid)
        { %>
        <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
            <h2 class="validationSummaryHeader">
                <%:Html.ValidationSummary(false, "")%>
            </h2>
        </fieldset>
        <% } %>

<!-- Full container start -->
    <div id="fullContainer">

        <fieldset>
    
            <h2><%: Model.SortableChildrenLabel %></h2>
            <div class="fieldsetContent">

                <ol class="sortable">	
                
                <% foreach (var sortChildTaxonItem in Model.SortChildTaxaList)
                    { %>        
        
                    <li id="taxon_<%: sortChildTaxonItem.ChildTaxonId %>"><div><%:sortChildTaxonItem.ChildScientificName%> (<%:sortChildTaxonItem.ChildTaxonId%>)</div></li>                       
                           
                 <% } %>
                 </ol>
            </div>
        </fieldset>
        <fieldset>
        <%: Html.ActionLink(Resources.DyntaxaResource.TaxonSortChildTaxaAscending, "SortChildTaxa", new { @taxonId = Model.TaxonId, @sort = "asc" }, new { @class = "ap-ui-button", @style="margin-top: 5px;" }) %>
        <%: Html.ActionLink(Resources.DyntaxaResource.TaxonSortChildTaxaDescending, "SortChildTaxa", new { @taxonId = Model.TaxonId, @sort = "desc" }, new { @class = "ap-ui-button", @style = "margin-top: 5px;" })%>
        <%: Html.ActionLink(Resources.DyntaxaResource.TaxonSortChildTaxaSortOrder, "SortChildTaxa", new { @taxonId = Model.TaxonId, @sort = "sortorder" }, new { @class = "ap-ui-button", @style = "margin-top: 5px;" })%>
        <br class="clear" />
         </fieldset>
         <fieldset>
        <input id="btnPost" type="submit" value="<%:Resources.DyntaxaResource.SharedSaveButtonText%>" class="ap-ui-button" />
        </fieldset>

        <% using (Html.BeginForm("SortChildTaxa", "Taxon", FormMethod.Post, new { @id = "sortChildTaxaForm", @name = "sortChildTaxaForm" }))
           {%>
           <%: Html.Hidden("newSortOrder") %>
        <% } %>
    </div>
<!-- Full container end -->



<script type="text/javascript">

    $(document).ready(function () {

        $('ol.sortable').nestedSortable({
            disableNesting: 'no-nest',
            forcePlaceholderSize: true,
            handle: 'div',
            helper: 'clone',
            items: 'li',
            maxLevels: 1,
            opacity: .6,
            placeholder: 'placeholder',
            revert: 250,
            tabSize: 25,
            tolerance: 'pointer',
            toleranceElement: '> div'
        });


        $("#btnPost").click(function () {
            var frm = document.sortChildTaxaForm;
            var reorder = [];
            $('ol.sortable').children('li').each(function (i) {
                reorder[i] = $(this).attr('id'); // save the item id order in array
            });

            frm.newSortOrder.value = JSON.stringify(reorder);

            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Resources.DyntaxaResource.SharedSaving %></h1>' });            
            frm.submit();
        });

    });

        
                
</script>

</asp:Content>
