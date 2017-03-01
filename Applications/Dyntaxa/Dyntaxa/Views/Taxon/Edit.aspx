<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonEditViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%: Model.Labels.EditTaxonLabel %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h1 class="readHeader"><%: Model.Labels.EditTaxonLabel %></h1>

<% if (ViewBag.Taxon != null)
    { %>
    <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
<% }
    else
    { %>
    <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
<% } %>


<%--<% Html.EnableClientValidation(); %> --%>
<% using (Html.BeginForm("Edit", "Taxon", FormMethod.Post, new { @id = "editTaxonForm", @name = "editTaxonForm" })) 
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
        <%: Html.HiddenFor(model => model.NoOfTaxonReferences) %>        
        <%: Html.HiddenFor(model => model.Author) %>
        <%: Html.HiddenFor(model => model.CommonName) %>
        <%: Html.HiddenFor(model => model.EnableSpeciesFact) %>
        <%: Html.HiddenFor(model => model.EnableTaxonIsProblematic) %>
        <%: Html.HiddenFor(model => model.ScientificName) %>
        <%: Html.HiddenFor(model => model.IsTaxonJustCreated) %>

                             
<!-- container start --> 
    <div id="fullContainer">            
        
        <fieldset>
            <h2 class="open"><%: Model.Labels.EditTaxonLabel %></h2>
            <div id="taxonPicker" class="fieldsetContent">
            
            
                <table>
                    <tr>
                        <td>
                                            
                            <% // Dropdown of taxon categories %>
                            <div class="editor-label">
                                <%: Html.LabelFor(model => model.TaxonCategoryId)%> 
                            </div>

                            <div class="editor-field">
                               <%-- <%: Html.DropDownList("Categories")%>--%>
                                <%:Html.DropDownListFor(model => model.TaxonCategoryId, new SelectList(Model.TaxonCategoryList, "Id", "Text", Model.TaxonCategoryId), new {style = "width:175px!important" })%>
                                <%: Html.ValidationMessageFor(model => model.TaxonCategoryId)%>
                               
                            </div>
                        
                        </td>
                        
                        
                        <% var colspan = 1;

                        if (Model.TaxonCategoryId == 17)
                        { // Is Microspecies
                            colspan = 2;%>
                            <td>
                                <div class="editor-label">
                                    <%: Html.LabelFor(model => model.IsMicrospecies) %>
                                </div>

                                    <div class="display-label">
                                    <%: Html.CheckBoxFor(model => model.IsMicrospecies) %>
                                </div>
                            </td>    
                        <% } %>               
                          

                        <td>
                            <% // Common name %>
                            <div class="editor-label">
                                <%: Html.LabelFor(model => model.CommonName)%>
                            </div>

                           <%-- <div class="editor-field">
                                <%: Html.TextBoxFor(model => model.CommonName, new { @readonly = "readonly" })%>                
                            </div>     --%> 
                             <div class="display-label">
                                <%: Html.DisplayTextFor(model => model.CommonName)%>
                            </div>               
                        </td>            
                    </tr>
                    <tr>
                        <td colspan="<%:colspan %>">
                              <%// Scientific name %> 
                            <div class="editor-label">
                                <%: Html.LabelFor(model => model.ScientificName)%> 
                            </div>

                           <%-- <div class="editor-field">
                                <%: Html.TextBoxFor(model => model.ScientificName, new { @readonly = "readonly" })%>                
                            </div>--%>
                            <div class="display-label">
                                <%: Html.DisplayTextFor(model => model.ScientificName)%>
                            </div> 
                    
                        </td>
                        <td>
                             <%// Author %>
                            <div class="editor-label">
                                <%: Html.LabelFor(model => model.Author)%>
                            </div>

                            <%--<div class="editor-field">
                                <%: Html.TextBoxFor(model => model.Author, new { @readonly = "readonly" })%>                
                            </div>--%>
                             <div class="display-label">
                                <%: Html.DisplayTextFor(model => model.Author)%>
                            </div> 
                        </td>
                
                    </tr>
                    <tr>
                        <td colspan="<%:colspan+1 %>">
                            <% // Taxon description  %>
                            <div class="editor-label">
                                <%: Html.LabelFor(model => model.Description)%>
                            </div>

                            <div class="editor-field">
                                <%: Html.TextAreaFor(model => model.Description)%> 
                                <%: Html.ValidationMessageFor(model => model.Description)%>                   
                            </div>
                        </td> 
                    
                    </tr>                   
                    <tr>
                        <td colspan="<%:colspan+1 %>">
                              <% // Taxon is problematic  %>
                             <% if (Model.EnableTaxonIsProblematic)
                             { %>       
                                <div class="editor-label editor-field">
                                    <%: Html.CheckBoxFor(model => model.TaxonIsProblematic)%>
                                    <%: Html.LabelFor(model => model.TaxonIsProblematic)%>  
                                </div>
                             <% }
                                else
                                {%>
                                    <div class="editor-label editor-field">
                                        <%: Html.CheckBoxFor(model => model.TaxonIsProblematic, new { @readonly = "readonly" })%>
                                        <%: Html.LabelFor(model => model.TaxonIsProblematic)%> 
                                    </div>
                               <% }%>
                        </td>                     
                    </tr>
                    <tr>
                        <td colspan="<%:colspan+1 %>">
                            <% // Exclude from reporting systems  %>
                            <div class="editor-label editor-field">
                                <%: Html.CheckBoxFor(model => model.ExcludeFromReportingSystem)%>
                                <%: Html.LabelFor(model => model.ExcludeFromReportingSystem)%>
                            </div>
                        </td> 
                    
                    </tr>
                    <tr>
                        <td colspan="3">
                            <% // Blocked for reporting  %>
                            <div class="editor-label editor-field">
                                <%: Html.CheckBoxFor(model => model.BlockedForReporting)%>
                                <%: Html.LabelFor(model => model.BlockedForReporting)%>
                            </div>
                        </td> 
                    
                    </tr>
                </table>
            </div>
        </fieldset>

        <fieldset>
            <h2 class="open"><%: Resources.DyntaxaResource.SharedReferences%></h2>
            <div class="fieldsetContent">
                <% if (Model.NoOfTaxonReferences > 0)
                { %>                
                    <% Html.RenderAction("ListReferences", "Reference", new { @guid = Model.TaxonGuid }); %> 
                <% }
            else
            {%>
                    <div class="editor-label">
                        <strong><%: Model.Labels.NoReferencesAvaliableText%></strong>
                    </div>
            <% }%>
            
                <%: Html.RenderAddReferenceLink(Model.TaxonGuid, Resources.DyntaxaResource.SharedManageReferences, "Edit", "Taxon", new { @taxonId = Model.TaxonId }, new { @class = "ap-ui-button", @style = "margin-left: 10px;" })%>
                <%: Html.ValidationMessageFor(model => model.NoOfTaxonReferences, "", new { @style = "display:inline;" })%>        
           
            </div>
        </fieldset>                                        

        <%-- Taxon Quality--%>
        <fieldset>
        <h2 class="open"><%: Model.Labels.TaxonEditQualityHeaderText%></h2> 
            <div class="fieldsetContent">
                 <div class="editor-label">
                    <%:Html.LabelFor(model => model.TaxonQualityId) %>
                </div>
                <div class="editor-field input-wrapper">
                    <%:Html.DropDownListFor(model => model.TaxonQualityId, new SelectList(Model.TaxonQualityList, "Id", "Text", Model.TaxonQualityId))%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.TaxonQualityDescription) %>
                    
                </div>
                <div class="editor-field input-wrapper">
                    <%: Html.TextAreaFor(model => model.TaxonQualityDescription)%>
                    <%: Html.ValidationMessageFor(model => model.TaxonQualityDescription)%>
                </div>
            </div>
        </fieldset> 
        
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
                            <%: Html.RenderAddReferenceImageLink(Model.TaxonGuid, "Edit", "Taxon", new { @taxonId = Model.TaxonId })%>   
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
                            <%: Html.RenderAddReferenceImageLink(Model.TaxonGuid, "Edit", "Taxon", new { @taxonId = Model.TaxonId })%>   
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
            <input class="ap-ui-button" id="GetSelectedSave"  type="submit" value="<%: Model.Labels.SaveButtonText %>" /> 
            <%: Html.ActionLink(Model.Labels.ResetButtonLabel, "Edit", new { taxonId = Model.TaxonId }, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%> 
        </fieldset>
    </div>

<% } %>


<script language="javascript" type="text/javascript">
    var showLeavePageMessage = true;

    function validForm(form) {
        var isValid = $(form).valid();
        return isValid;
    }
    $(document).ready(function() {        

        <% if(Model.IsTaxonJustCreated)
         { %>
            $(window).bind("beforeunload",function(event) {            
                if(showLeavePageMessage) {
                    $.unblockUI();
                    return "<%= Resources.DyntaxaResource.TaxonEditLeavePageWithoutSave %>";
                }
            });         
        <% } %>
        
        
        initDetectFormChanges();
        $('a[href*="Reference/Add"]').click(function (e) {
            showLeavePageMessage = false;
            var element = this;
            var changed = isAnyFormChanged();

            if (changed) {
                e.preventDefault();
                showYesNoDialog("<%: Resources.DyntaxaResource.SharedDoYouWantToContinue %>",
                        "<%: Resources.DyntaxaResource.SharedGoToReferenceViewQuestion %>",
                        "<%: Resources.DyntaxaResource.SharedDialogButtonTextYes %>",
                        function () {                            
                            e.view.window.location = element.href; // on click redirect                                                         
                        },
                        "<%: Resources.DyntaxaResource.SharedDialogButtonTextNo %>",
                        null);
            }
         });
                //get the selected values so we can update the database or whatever.
                $('#GetSelectedSave').click(function () {
                     var theform = document.editTaxonForm;
                    //if (validForm(theform)) {
                        $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>' });
                        showLeavePageMessage = false;
                        theform.submit();
                    //}
                });
        

//                $("#editTaxonForm").submit(function () 
//                {
//                    $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>'
//                 });
//               });
    });

</script>

</asp:Content>
