<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonDropParentViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
     <%:Model.Labels.ParentHeaderLabel %>
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
    
<% using (Html.BeginForm("DropParent", "Taxon", FormMethod.Post, new { @id = "dropParentForm", @name = "dropParentForm" })) 
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
    <%: Html.HiddenFor(model => model.EnableSaveDeleteParentTaxonButton) %>
   
                             
     <div id="fullContainer">
        <fieldset>
            <h2 class="open"><%:Model.Labels.DropParentLabel%></h2>
            <div class="fieldsetContent"> 
                <div class="editor-field">
                    <select id="SelectedTaxonList" multiple="multiple" class="required"  name="SelectedTaxonList" size="10" style="width: 500px;">
                    <% foreach (TaxonParentViewModelHelper item in Model.TaxonList)
                    { %>
                        <% if (item.IsMain)
                        { %>               
                            <option disabled="disabled" value="<%: item.TaxonId %>">
                                <%: item.Category %>:
                                <%= item.ScientificName != null ? Html.RenderScientificName(item.ScientificName, null, item.SortOrder).ToString() : "-"%>
                                <%= item.CommonName != null ? Html.Encode(string.Format(" - {0}", item.CommonName)) : ""%>  
                            </option>
                         <% }
                        else
                        { %>
                            <option value="<%: item.TaxonId %>">
                                <%: item.Category %>:
                                <%= item.ScientificName != null ? Html.RenderScientificName(item.ScientificName, null, item.SortOrder).ToString() : "-"%>
                                <%= item.CommonName != null ? Html.Encode(string.Format(" - {0}", item.CommonName)) : ""%>  
                            </option>
                         <% }%>
                        
                    <% } %>
                    </select>
                    <%--<%: Html.ListBoxFor(m => m.SelectedTaxonList, new MultiSelectList(Model.TaxonList, "TaxonId", "ScientificName"))%>--%>
                </div>
             </div>              
        </fieldset>
        <fieldset>
        <p>
            <input type="submit" id="selectedTaxon" name="submitButton" class="ap-ui-button-disabled" value="<%:Model.Labels.DeleteButtonLabel %>" disabled="disabled" />                                      
            <%: Html.ActionLink(Model.Labels.CancelButtonLabel, "Edit", new { taxonId = Model.TaxonId, revisionId = Model.RevisionId }, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>  
         </p>
         </fieldset>
   </div>  

<% } %>


<script language="javascript" type="text/javascript">
    $(document).ready(function() {
        <% if ((Model.TaxonList != null && Model.TaxonList.Count <= 1 && !Model.IsReloaded))
        { %>
        showInfoDialog("<%:Model.DialogTitlePopUpText%>", "<%:Model.DialogTextPopUpText%>", "<%:Model.Labels.ConfirmTextPopUpText%>", null);
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
        
         $("#dropParentForm").submit(function () {
             $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>'
            });
         });
    });
</script>

</asp:Content>
