<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonAddViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.Labels.AddTaxonPageTitle %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 

<h1 class="readHeader"><%: Model.Labels.AddTaxonPageHeader %></h1>


<% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.ParentTaxonId }); %>

<% Html.EnableClientValidation(); %> 

<% using (Html.BeginForm("Add", "Taxon", FormMethod.Post, new { @id = "addTaxonForm", @name = "addTaxonForm" }))
   { %>  

 <% if (!this.ViewData.ModelState.IsValid)
        { %>
        <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
            <h2 class="validationSummaryHeader">
                <%:Html.ValidationSummary(false, "")%>
            </h2>
        </fieldset>
        <% } %>
                             
        <%: Html.HiddenFor(model => model.ParentTaxonId) %>
                             

    <div id="fullContainer">            
         
        <fieldset>
            <h2 class="open"><%: Model.Labels.AddTaxonPageHeader %></h2>
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
                                <%:Html.DropDownListFor(model => model.TaxonCategoryId, new SelectList(Model.TaxonCategoryList, "Id", "Text", Model.TaxonCategoryId))%>
                                <%: Html.ValidationMessageFor(model => model.TaxonCategoryId)%>
                               
                            </div>
                        
                    </td>


                    <td> 
                        <% // Common name %>
                        <div class="editor-label">
                            <%: Html.LabelFor(model => model.CommonName)%>
                        </div>

                        <div class="editor-field">
                            <%: Html.TextBoxFor(model => model.CommonName)%>                
                            <%: Html.ValidationMessageFor(model => model.CommonName)%>
                        </div>                      

                    </td>            
                </tr>
                <tr>
                    <td>
                         <%// Scientific name %> 
                        <div class="editor-label">
                            <%: Html.LabelFor(model => model.ScientificName)%> 
                        </div>

                        <div class="editor-field">
                            <%: Html.TextBoxFor(model => model.ScientificName)%>                
                            <%: Html.ValidationMessageFor(model => model.ScientificName)%>
                        </div>
                    
                    </td>
                    <td>
                         <%// Author %>
                        <div class="editor-label">
                            <%: Html.LabelFor(model => model.Author)%>
                        </div>

                        <div class="editor-field">
                            <%: Html.TextBoxFor(model => model.Author)%>                
                            <%: Html.ValidationMessageFor(model => model.Author)%>
                             
                        </div>
                    </td>
                
                </tr>
                <tr>
                    <td colspan="2">
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
                        <td colspan="2">
                              <% // Taxon is problematic  %>
                            <div class="editor-label editor-field">
                                    <%: Html.CheckBoxFor(model => model.TaxonIsProblematic)%>
                                    <%: Html.LabelFor(model => model.TaxonIsProblematic)%>  
                            </div>
                        </td>                     
                    </tr>
                </table>

            <%--<input class="ap-ui-button" type="submit" value="Save" />
            
            <input id="infoDialog" class="ap-ui-button" type="submit" value="Save" />
            
            <input id="deleteDialog" class="ap-ui-button" type="submit" value="delete dialog" />
            --%>
            
            <%-- Render a dialog box and trigger link --%>
<%--            <%: Html.Partial("~/Views/Shared/Dialogs/ConfirmDialogUserControl.ascx")%>--%>
            

            </div>      
        
            
            
                               
            
        </fieldset>
        
          <fieldset>
            <%--<button style="clear: both; float: left;" id="GetSelectedSave" type="button" class="ap-ui-button"><%: Model.Labels.SaveButtonText %></button>--%>

            <input class="ap-ui-button" id="<%:Model.Labels.GetSelectedSave %>" type="submit" value="<%: Model.Labels.SaveButtonText %>" /> 
            <%: Html.ActionLink(Model.Labels.ResetButtonLabel, "Add", new { taxonId = Model.ParentTaxonId }, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%> 
        </fieldset>



    </div>    
        
    


<% } %>


<script language="javascript" type="text/javascript">
        
        
         $(document).ready(function () {
                
        <% if (!Model.IsOkToCreateTaxon)
           { %>  
                showInfoDialog("<%:Model.Labels.AddTaxonPageTitle%>", "<%:Model.Labels.IsNotOkToCreateTaxonErrorText%>",  "<%:Model.Labels.ConfirmButtonText%>",null);
        <% } %>
        
        function validateNameExist(form) {
            var exists = false;
            $.ajax({ 
                type: 'GET', 
                url: '<%= Url.Action("DoesScientificNameExist","Taxon") %>', 
                dataType: 'json', 
                success: function(data) {
                    if (data.returnvalue == true) {
                        alert('<%: Resources.DyntaxaResource.ValidateTaxonWithScientificName %> "' + data.scientificName + '" <%: Resources.DyntaxaResource.ValidateAlreadyExists %>' + '\nTaxonId: ' + data.taxonId + '\n<%: Resources.DyntaxaResource.TaxonSharedCategory %>: ' + data.category);
                        exists = true;
                    }                                         
                }, 
                data: { scientificName: form.ScientificName.value }, 
                async: false 
            }); 
                                    
            return exists;
        }
             
        function validForm(form) { 
            var isValid = $(form).valid();
            return isValid;
        }
             
           //get the selected values so we can update the database or whatever.
        $('#'+"<%:Model.Labels.GetSelectedSave %>" ).click(function (e) {
            e.preventDefault();
             var theform = document.addTaxonForm;

            validateNameExist(theform);
            
             if(validForm(theform)){ 
                 return showDialog("<%:Model.Labels.AddTaxonPageTitle%>", "<%:Model.Labels.DialogAddTaxonInfoText%>", 
                                   "<%:Model.Labels.ConfirmButtonText%>", "<%:Model.Labels.CancelButtonText%>", function() {                
                  $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>' });
               
                  theform.submit();                 
                } , null);
            }
        });

             
//        $('#addButton').click(function() {
//            var searchTable = $("#tableSearch").dataTable();
//            var frm = document.selectReferencesForm;
//            
//            
//            
//            
//            frm.submit();
//        });
             
    });
    

</script>

</asp:Content>
