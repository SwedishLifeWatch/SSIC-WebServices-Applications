<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName.ListTaxonNameViewModel>" %>

<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Resources.DyntaxaResource.TaxonNameListTitle %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="readHeader">
        <%: Resources.DyntaxaResource.TaxonNameListTitle %>
    </h1>
    <% if (ViewBag.Taxon != null)
        { %>
        <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
    <% }
        else
        { %>
        <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
    <% } %>
    

    <% using (Html.BeginForm("List", "TaxonName", FormMethod.Post, new { @id = "recommendedTaxonNameForm", @name = "recommendedTaxonNameForm" }))    
       {%>

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
        <input type="hidden" id="taxonId" name="taxonId" value="<%:Model.TaxonId%>" />
        <% if (Model.NameCategories.Count > 0)
           { %>
        <% foreach (TaxonNameCategoryViewModel nameCategory in Model.NameCategories)
           { %>
        <fieldset>
            <%--<h2 class="<%=(nameCategory.IsScientificNameCategory || nameCategory.IsCommonNameCategory) ? "open" : "closed" %>">--%>
            <h2 class="open">
                <%: nameCategory.TaxonNameCategory %>
            </h2>
            <div class="fieldsetContent">            
                <% var grid = new WebGrid(source: nameCategory.Names, canSort: false, canPage: false); %>
                <%:
               grid.GetHtml(
                   tableStyle: "grid",
                   headerStyle: "head",
                   alternatingRowStyle: "alt",
                   htmlAttributes: new {@style = "width:926px;"},
                   columns: grid.Columns(
                        grid.Column(header: Resources.DyntaxaResource.TaxonNameSharedName, style : "taxonNameListTableColumn1",
                            format: (item) =>
                                {
                                    var nameItem = ((TaxonNameEditingViewModel) item.Value);
                                    string name = nameItem.IsOriginal ? nameItem.Name + "*" : nameItem.Name;
                                    return Html.ActionLink(name, "Edit", new {@taxonId = Model.TaxonId, nameId = nameItem.Version}, null);
                                }),
                       grid.Column(columnName: "Author", style: "taxonNameListTableColumn2", header: Resources.DyntaxaResource.TaxonNameSharedAuthor),
                       grid.Column(columnName: "IsRecommended",  header: Resources.DyntaxaResource.TaxonNameSharedIsRecommended, style : "taxonNameListTableColumn3", format: (item) =>
                            {                                                                                           
                                return Html.Partial("~/Views/TaxonName/TaxonNameRecommended.ascx", (TaxonNameEditingViewModel)item.Value);
                            }),
                       grid.Column(columnName: "NameUsageName", style: "taxonNameListTableColumn4", header: Resources.DyntaxaResource.TaxonNameSharedNameUsage),
                       grid.Column(columnName: "NameStatusName", style: "taxonNameListTableColumn4", header: Resources.DyntaxaResource.TaxonNameSharedNomenclature),                       
                       grid.Column(header: Resources.DyntaxaResource.TaxonNameSharedReferences, style: "taxonNameListTableColumn5",
                            format: (item) => Html.Partial("~/Views/TaxonName/TaxonNameReferences.ascx", (TaxonNameEditingViewModel)item.Value)),
                       grid.Column(style: "taxonNameListTableColumn6", format: (item) =>
                                                                                   {
                                                                                      var nameItem = ((TaxonNameEditingViewModel) item.Value);
                                                                                      return nameItem.IsRemoved ? null: Html.ImageLink(Url.Content("~/Images/Icons/recyclebin_empty_16.png"), "Delete", "Delete", "TaxonName", new { @taxonId = Model.TaxonId, nameId = item.Version }, null, null);
                                                                                   }),
                       grid.Column(style: "taxonNameListTableColumn7", format: (item) => Html.RenderAddReferenceImageLink(((TaxonNameEditingViewModel)item.Value).Guid, "List", "TaxonName", new { @taxonId = Model.TaxonId }))
                          )
                       )                       
                    
                %>          
                <%--grid.Column(format: (item) => Html.RenderAddReferenceLink(item.Guid, "List", "TaxonName", new { @taxonId = Model.TaxonId }))--%>
            </div>
        </fieldset>
        <%} %>
        <% } %>
        
        <fieldset>
            <div class="fieldsetDivMargin">
                <input id="btnPost" type="submit" value="<%: Resources.DyntaxaResource.SharedSaveButtonText %>" class="ap-ui-button" style="margin-left: 0px; margin-top: 10px;" />
                <%: Html.ActionLink(Resources.DyntaxaResource.TaxonNameListAddNewNameToCategory, "Add", new { taxonId = Model.TaxonId }, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>
                <%--<br class="clear" />
                <input type="submit" value="<%: Resources.DyntaxaResource.SharedSaveButtonText %>" class="ap-ui-button" style="margin-left: 0px; margin-top: 16px;" />--%>
            </div>
        </fieldset>
        <!-- Full container end -->
    </div>    
    

    <% } %>
    <script type="text/javascript">

        Array.prototype.getUnique = function () {
            var o = {}, a = [];
            for (var i = 0; i < this.length; i++) o[this[i]] = 1;
            for (var e in o) a.push(e);
            return a;
        };

        $(document).ready(function () {
            // animate visibility for fieldsets
            //initToggleFieldsetH2();

            // Handle checkbox click. Maximum one in each category can be selected.
            var checkboxGroups = [];
            $('input[checkboxgroup]').each(function () {
                checkboxGroups.push($(this).attr('checkboxgroup'));
            });

            var uniqueCheckboxGroups = checkboxGroups.getUnique();
            $.each(uniqueCheckboxGroups, function (index, value) {
                var $unique = $('input[checkboxgroup="' + value + '"]');

                $unique.click(function () {
                    if (index == 0) { // must select at least one scientific name
                        $unique.removeAttr('checked');
                        $(this).attr('checked', true);
                    } else { // must select maximum one in other names
                        $unique.filter(':checked').not(this).removeAttr('checked');
                    }
                });
            });

            $("#btnPost").click(function() {
                var frm = document.recommendedTaxonNameForm;                
                $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Resources.DyntaxaResource.SharedSaving %></h1>' });
                frm.submit();
            });
        });


                

    </script>
</asp:Content>
