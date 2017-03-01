<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonAddParentViewModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="ArtDatabanken.Data" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%:Model.Labels.ParentHeaderLabel%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h1 class="readHeader"><%:Model.Labels.ParentHeaderLabel%></h1>

<% if (ViewBag.Taxon != null)
    { %>
    <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
<% }
    else
    { %>
    <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
<% } %>

<% Html.EnableClientValidation(); %> 
    
<% using (Html.BeginForm("AddParent", "Taxon", FormMethod.Post, new { @id = "addParentForm", @name = "addParentForm" })) 
{%>
        
     <% if (!this.ViewData.ModelState.IsValid)
        { %>
        <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
            <h2 class="validationSummaryHeader">
                <%:Html.ValidationSummary(false, "")%>
            </h2>
        </fieldset>
        <% } %>
                       
    <%: Html.HiddenFor(model => model.TaxonId) %>
    <%: Html.HiddenFor(model => model.RevisionId) %>
                             
    <div id="fullContainer">
          <fieldset>
            <h2 class="open">
                <%:Model.Labels.ExistingParentsLabel%></h2>
            <div class="fieldsetContent">
                <table class="display-table">
                    <tr>
                        <td>
                                                 
                            <select id="parentTaxonList" size="10" style="width: 500px;" name="ParentTaxon">
                                <% foreach (var item in Model.AvailableParents)
                                {%>
                                    <option disabled="disabled" value="<%=item.TaxonId%>"><%:item.Category%>: <%:item.ScientificName%></option>
                                <% } %>
                            </select>
                        </td>
                        <td style="vertical-align: top;">
                            <div id="selectedParentTaxon" style="font-size: 16px;">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>             
        <fieldset>
            <h2 class="open"><%:Model.Labels.AddParentLabel%></h2>
            <div class="fieldsetContent">                                
                <div class="editor-field">
                          <select id="SelectedTaxonList" class="required" multiple="multiple" name="SelectedTaxonList" size="10" style="width: 500px;">
                           <% foreach (TaxonParentViewModelHelper item in Model.TaxonList)
                            { %>
                                <option value="<%: item.TaxonId %>">
                                <%: item.Category %>:
                                <%= item.ScientificName != null ? Html.RenderScientificName(item.ScientificName, null, item.SortOrder).ToString() : "-"%>
                                <%= item.CommonName != null ? Html.Encode(string.Format(" - {0}", item.CommonName)) : ""%>  
                                </option>
                            <% } %>
                          </select>
                            <%--<%: Html.ListBoxFor(m => m.SelectedTaxonList, new MultiSelectList(Model.TaxonList, "TaxonId", "ScientificName"))%>--%>
                  </div>    
                  <div class="editor-field">
                    <%: Resources.DyntaxaResource.TaxonSharedListTaxaInCategory%>:
                    <select id="taxonCategorySelectBox" style="width: 300px;">
                        <option value="-1">[<%: Resources.DyntaxaResource.SharedChooseCategory%>]</option>                        
                        <% foreach (TaxonCategoryViewModel category in Model.TaxonCategories.Values)
                        { %>
                       <option value="<%=category.Id %>"><%: category.Name %></option>
                       <% } %>
                    </select>
                </div> 
             </div>              
        </fieldset>
        <fieldset>
        <p>
            <input type="submit" id="selectedTaxon" name="submitButton" class="ap-ui-button-disabled" value="<%:Model.Labels.AddButtonLabel %>" disabled="disabled" />                          
            <%: Html.ActionLink(Model.Labels.CancelButtonLabel, "Edit", new { taxonId = Model.TaxonId, revisionId = Model.RevisionId }, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>  
        </p>
        </fieldset>
     </div>       

<% } %>


<script language="javascript" type="text/javascript">
    
    var taxonCategory = [];
    <% foreach (var category in Model.TaxonCategories.Values) %>
    <% { %>
        taxonCategory.push({ id: <%= category.Id %>, name: '<%:category.Name %>' });    
    <% } %>
    //taxonCategory.push({ id: 1, name: 'test' });

    var possibleParents = [];
    <% foreach (var categoryId in Model.TaxonDictionary.Keys) %>
    <% { %>
    possibleParents[<%= categoryId %>] = [];
        <% foreach (var taxon in Model.TaxonDictionary[categoryId]) %>
        <% { %>
        possibleParents[<%= categoryId %>].push({ id: <%=taxon.TaxonId %>, name: '<%: taxon.ScientificName %> - <%: taxon.CommonName %>' });
        <% } %>
    <% } %>
    
    
    $(document).ready(function () {

        $('#taxonCategorySelectBox').change(function(e) {
            var categoryId = $(this).val();

            var $taxaSelectBox = $('#SelectedTaxonList');
            $taxaSelectBox.children().remove().end();
            if (categoryId == -1)
                return;

            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Resources.DyntaxaResource.SharedLoading %></h1>' });
            var options = [];
            for (var i = 0; i < possibleParents[categoryId].length; i++) {
                var taxon = possibleParents[categoryId][i];
                options.push('<option value="'+ taxon.id +'">'+ taxon.name +'</option>');
            }                       
            $taxaSelectBox.html(options.join(''));
            $.unblockUI();

        });

            <% if (!Model.IsOkToAdd)
            { %>  
                showInfoDialog("<%:Model.Labels.ParentHeaderLabel%>", "<%:Model.AddParentErrorText%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
        <% } %>
   
        <% else if ((Model.TaxonList != null && Model.TaxonList.Count <= 0))
        { %>  
            showInfoDialog("<%:Model.DialogTitlePopUpText%>", "<%:Model.DialogTextPopUpText%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
        <% } %>
        // animate visibility for fieldsets
        //initToggleFieldsetH2();        
        //initToggleFieldsetH3();

        $('select.required').change(function() {
            var nrSelected = $(this).find(":selected").length;            
            if (nrSelected > 0) {
                $('#selectedTaxon').prop('disabled', false);
                $('#selectedTaxon').removeClass('ap-ui-button-disabled').addClass('ap-ui-button');
            } else {
                $('#selectedTaxon').prop('disabled', true);
                $('#selectedTaxon').removeClass('ap-ui-button').addClass('ap-ui-button-disabled');
            }
        }); 
        if($('#select.required :selected').length > 0) {
            $('#selectedTaxon').prop('disabled', false);
            $('#selectedTaxon').removeClass('ap-ui-button-disabled').addClass('ap-ui-button');
        }
        
        $("#addParentForm").submit(function () {
                    $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>'
                 });
          });
    });

</script>

</asp:Content>
