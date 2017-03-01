<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonSwedishOccuranceEditViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%: Model.Labels.SwedishOccurrenceLabel%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h1 class="readHeader"><%: Model.Labels.SwedishOccurrenceLabel%></h1>

<% if (ViewBag.Taxon != null)
    { %>
    <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
<% }
    else
    { %>
    <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
<% } %>

<% Html.EnableClientValidation(); %> 
    
<% using (Html.BeginForm("EditSwedishOccurance", "Taxon", FormMethod.Post, new { @id = "editSwedishOccuranceForm", @name = "editSwedishOccuranceForm" })) 
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

       <% if (Model.EnableSpeciesFact)
        { %>               
            <fieldset>
                <h2 class="open"><%: Model.Labels.SwedishOccurrenceLabel %></h2>
                <div class="fieldsetContent">
                     <% if (!Model.SpeciesFactError)
                        { %> 
                        <div class="formRow">
                        <div class="group">
                            <%:Html.LabelFor(model => model.SwedishOccurrenceStatusId)%>
                            <%: Html.DropDownListFor(model => model.SwedishOccurrenceStatusId, new SelectList(Model.SwedishOccurrenceStatusList, "Id", "Text", Model.SwedishOccurrenceStatusId))%>
                        </div>
                    </div>
                        <div class="formRow">
                        <div class="group">
                            <%:Html.LabelFor(model => model.SwedishOccurrenceQualityId)%>
                            <%: Html.DropDownListFor(model => model.SwedishOccurrenceQualityId, new SelectList(Model.SwedishOccurrenceQualityList, "Id", "Text", Model.SwedishOccurrenceQualityId))%>
                        </div>
                    </div>
                        <div class="formRow">
                        <div class="group">
                            <%:Html.LabelFor(model => model.SwedishOccurrenceReferenceId)%>
                            <%: Html.DropDownListFor(model => model.SwedishOccurrenceReferenceId, new SelectList(Model.SwedishOccurrenceReferenceList, "Id", "Text", Model.SwedishOccurrenceReferenceId))%>
                            <%: Html.RenderAddReferenceImageLink(Model.TaxonGuid, "EditSwedishOccurance", "Taxon", new { @taxonId = Model.TaxonId })%>   
                        </div>
                    </div>
                        <div class="editor-label">
                        <%: Html.LabelFor(model => model.SwedishOccurrenceDescription)%>
                   
                    </div>
                        <div class="editor-field input-wrapper">
                        <%: Html.TextAreaFor(model => model.SwedishOccurrenceDescription)%>
                        <%: Html.ValidationMessageFor(model => model.SwedishOccurrenceDescription)%>
                    </div>
                   <% }
                    else
                    {%>
                        <p>
                            <%: Model.Labels.SpeciesFactErrorText %>
                        </p>
                  <% }%>
                </div>
            </fieldset>
        
            <fieldset>
                <h2 class="open"><%: Model.Labels.SwedishHistoryLabel %></h2>
                <div class="fieldsetContent">
                    <% if (!Model.SpeciesFactError)
                        { %> 
                            <div class="formRow">
                        <div class="group">
                            <%:Html.LabelFor(model => model.SwedishImmigrationHistoryStatusId)%>
                            <%: Html.DropDownListFor(model => model.SwedishImmigrationHistoryStatusId, new SelectList(Model.SwedishImmigrationHistoryStatusList, "Id", "Text", Model.SwedishImmigrationHistoryStatusId))%>
                        </div>
                    </div>
                            <div class="formRow">
                        <div class="group">
                            <%:Html.LabelFor(model => model.SwedishImmigrationHistoryQualityId)%>
                            <%: Html.DropDownListFor(model => model.SwedishImmigrationHistoryQualityId, new SelectList(Model.SwedishImmigrationHistoryQualityList, "Id", "Text", Model.SwedishImmigrationHistoryQualityId))%>
                        </div>
                    </div>
                     <div class="formRow">
                        <div class="group">
                            <%:Html.LabelFor(model => model.SwedishImmigrationHistoryReferenceId)%>
                            <%: Html.DropDownListFor(model => model.SwedishImmigrationHistoryReferenceId, new SelectList(Model.SwedishImmigrationHistoryReferenceList, "Id", "Text", Model.SwedishImmigrationHistoryReferenceId))%>
                            <%: Html.RenderAddReferenceImageLink(Model.TaxonGuid, "EditSwedishOccurance", "Taxon", new { @taxonId = Model.TaxonId })%>   
                        </div>
                    </div>
                    <div class="editor-label">
                         <%: Html.LabelFor(model => model.SwedishImmigrationHistoryDescription)%>
                     
                     </div>  
                     <div class="editor-field input-wrapper">
                        <%: Html.TextAreaFor(model => model.SwedishImmigrationHistoryDescription)%>
                        <%: Html.ValidationMessageFor(model => model.SwedishImmigrationHistoryDescription)%>
                    </div>
                    <% }
                    else
                    {%>
                        <p>
                            <%: Model.Labels.SpeciesFactErrorText %>
                        </p>
                  <% }%>
                </div> 
            </fieldset>     
       <% }%>
        <fieldset>
             <% if (Model.EnableSpeciesFact)
            { %>  
            <input class="ap-ui-button" type="submit" value="<%: Model.Labels.SaveButtonText %>" /> 
            <% }
            else
            {%>
                 <input type="submit" class="ap-ui-button-disabled" value="<%:Model.Labels.SaveButtonText %>" disabled="disabled" />
             <% }%>
           <%-- <%: Html.ActionLink(Resources.DyntaxaResource.SharedDeleteButtonText, "Delete", new { taxonId = Model.TaxonId }, new { @class = "ap-ui-button", @style="margin-top: 10px;" })%> 
  --%>      
        </fieldset>
    </div>

<% } %>


<script language="javascript" type="text/javascript">

    $(document).ready(function () {
        //initToggleFieldsetH2();
        //initToggleFieldsetH3();
        $("#editSwedishOccuranceForm").submit(function () {
            
            // make sure everything is submitted
            $("#SwedishImmigrationHistoryStatusId").attr('disabled', false);
            $("#SwedishImmigrationHistoryQualityId").attr('disabled', false);
            $("#SwedishImmigrationHistoryReferenceId").attr('disabled', false);
            $("#SwedishImmigrationHistoryDescription").attr('disabled', false);
            
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>'
            });
        });

        $("#SwedishImmigrationHistoryStatusId").change(function (eventData) {
            var val = $("#SwedishImmigrationHistoryStatusId").val();
            if (val == 0) {
                $("#SwedishImmigrationHistoryQualityId").attr('disabled', true);
                $("#SwedishImmigrationHistoryReferenceId").attr('disabled', true);
                $("#SwedishImmigrationHistoryDescription").attr('disabled', true);
            } else {
                $("#SwedishImmigrationHistoryQualityId").attr('disabled', false);
                $("#SwedishImmigrationHistoryReferenceId").attr('disabled', false);
                $("#SwedishImmigrationHistoryDescription").attr('disabled', false);
            }
        });

    });

</script>

</asp:Content>
