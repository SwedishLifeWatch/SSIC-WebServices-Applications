<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonEditQualityViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data.Taxon" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%: Model.Labels.EditTaxonQualityLabel%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h1 class="readHeader"><%: Model.Labels.EditTaxonQualityLabel%></h1>

<% if (ViewBag.Taxon != null)
    { %>
    <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
<% }
    else
    { %>
    <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
<% } %>

<% Html.EnableClientValidation(); %> 
    
<% using (Html.BeginForm("EditQuality", "Taxon", FormMethod.Post, new { @id = "editQualityForm", @name = "editQualityForm" })) 
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
        <%: Html.HiddenFor(model => model.TaxonGuid) %>
                             
<!-- container start --> 
    <div id="fullContainer">            

        <fieldset>
        <h2 class="open"><%: Model.Labels.TaxonEditQualityHeaderText%></h2> 
            <div class="fieldsetContent">
                <div class="formRow">
                    <div class="group">
                        <%:Html.LabelFor(model => model.TaxonQualityId) %>
                        <%:Html.DropDownListFor(model => model.TaxonQualityId, new SelectList(Model.TaxonQualityList, "Id", "Text", Model.TaxonQualityId))%>
                    </div>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.TaxonQualityDescription) %>
                    
                </div>
                <div class="editor-field input-wrapper">
                    <%: Html.TextAreaFor(model => model.TaxonQualityDescription)%>
                    <%: Html.ValidationMessageFor(model => model.TaxonQualityDescription)%>

                    <div class="editor-label">
                        <%:Html.Label(Resources.DyntaxaResource.QualityApplyModeHeading)%>:
                    </div>
                    <ul>
                        <li><input type="radio" checked="checked" name="applyMode" value="<%= (int) QualityApplyMode.OnlySelected %>"><span style="padding-left: 5px;"><%:Resources.DyntaxaResource.QualityApplyModeOnlySelected %></span></li>
                        <li><input type="radio" name="applyMode" value="<%= (int) QualityApplyMode.AddToAllUnderlyingTaxa %>"><span style="padding-left: 5px;"><%:Resources.DyntaxaResource.QualityApplyModeReplaceInUnderlyingTaxa %></span></li>
                        <li><input type="radio" name="applyMode" value="<%= (int) QualityApplyMode.AddToUnderlyingTaxaExceptWhereAlreadyDeclared %>"><span style="padding-left: 5px;"><%:Resources.DyntaxaResource.QualityApplyModeAddToUnderlyingTaxaExceptWhereAlreadyDeclared %></span></li>
                        <li><input type="radio" name="applyMode" value="<%= (int) QualityApplyMode.AddToUnderlyingTaxaExceptWhereAlreadyDeclaredHigher %>"><span style="padding-left: 5px;"><%:Resources.DyntaxaResource.QualityApplyModeReplaceInUnderlyingTaxaExceptWhereAlreadyDeclaredHigher %></span></li>
                    </ul>
                </div>
            </div>
        </fieldset>   
        <fieldset>
            <input class="ap-ui-button" type="submit" value="<%: Model.Labels.SaveButtonText %>" /> 
           <%-- <%: Html.ActionLink(Resources.DyntaxaResource.SharedDeleteButtonText, "Delete", new { taxonId = Model.TaxonId }, new { @class = "ap-ui-button", @style="margin-top: 10px;" })%> 
  --%>      </fieldset>
    </div>

<% } %>


<script language="javascript" type="text/javascript">

    $(document).ready(function() {
        //initToggleFieldsetH2();
        //initToggleFieldsetH3();

        $("#editQualityForm").submit(function () {
                    $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>'
                 });
          });
    });

</script>

</asp:Content>
