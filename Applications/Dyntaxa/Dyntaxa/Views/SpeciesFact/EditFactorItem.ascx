<%--<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.Reference.CreateNewReferenceViewModel>" %>--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.FactorViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>

<% Html.EnableClientValidation(); %> 
    <% using (Html.BeginForm("EditFactorItem", "SpeciesFact", FormMethod.Post, new { @id = "editFactorItemForm", @name = "editFactorItemForm" }))
    { %>    
        <% if (!this.ViewData.ModelState.IsValid)
        { %>
        <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
            <h2 class="validationSummaryHeader">
                <%:Html.ValidationSummary(false, "")%>
            </h2>
        </fieldset>
        <% } %>    

<div id="fullContainer">
     <%: Html.HiddenFor(model => model.TaxonId) %>
      <%: Html.HiddenFor(model => model.FactorDataTypeId) %>
     <%: Html.HiddenFor(model => model.DataTypeId) %>
      <%: Html.HiddenFor(model => model.ChildFactorId) %> 
    <%: Html.HiddenFor(model => model.HostId) %> 
    <%: Html.HiddenFor(model => model.ReferenceId) %>  
     <%: Html.HiddenFor(model => model.IsNewCategory) %>   
    <%: Html.HiddenFor(model => model.MainParentFactorId) %>   
        <fieldset>
            <h2 class="open"><%: Model.FactorName %></h2>
             <div class="fieldsetContent">
                 <br/>
                 <div class="formRow">
                        <div class="group">
                            <%: Html.LabelFor(model => model.IndividualCategoryId)%>
                            <%:Html.DropDownListFor(model => model.IndividualCategoryId, new SelectList(Model.IndividualCategoryList, "Id", "Text", Model.IndividualCategoryId),new {style="width: 300px;"})%>
                        </div>
                    </div> 
                 <div class="formRow">
                        <div class="group">
                            <%: Html.LabelFor(model => model.ExistingEvaluations)%> 
                            <%: Html.DisplayTextFor(model => model.ExistingEvaluations)%>
                        </div>
                    </div>
                     <div class="formRow">
                        <div class="group">
                            <%--<label for="FactorEnumId"><%: Model.FactorFieldEnumLabel %></label>--%>
                            <label><%: Model.FactorFieldEnumLabel %></label>
                             <%--<%:Html.LabelFor(model => model.FactorFieldEnumLabel)%> --%>                             
                             <%:Html.DropDownListFor(model => model.FactorFieldEnumValue, new SelectList(Model.FactorFieldEnumValueList, "Id", "Text", Model.FactorFieldEnumValue),new {style="width: 300px;"})%>
                        </div>
                    </div>
                 
               <% if (Model.FactorDataTypeId == (int)DyntaxaFactorDataType.AF_SUBSTRATE || Model.FactorDataTypeId == (int)DyntaxaFactorDataType.AF_INFLUENCE)
               { %> 
                 <div class="formRow">
                        <div class="group">
                            <%--<label for="FactorEnumId"><%: Model.FactorFieldEnumLabel %></label>--%>
                            <label><%: Model.FactorFieldEnumLabel2 %></label>
                             <%--<%:Html.LabelFor(model => model.FactorFieldEnumLabel)%> --%>                             
                             <%:Html.DropDownListFor(model => model.FactorFieldEnumValue2, new SelectList(Model.FactorFieldEnumValueList2, "Id", "Text", Model.FactorFieldEnumValue2),new {style="width: 300px;"})%>
                        </div>
                    </div>
                 <% } %>   
                        
                    <div class="editor-label">
                         <%: Html.LabelFor(model => model.FactorFieldComment)%> 
                    </div>
                    <div class="editor-field input-wrapper">
                        <%: Html.TextAreaFor(model => model.FactorFieldComment)%>
                        <%: Html.ValidationMessageFor(model => model.FactorFieldComment)%> 
                    </div>

                   
            
                  <div class="formRow">
                        <div class="group">
                            <%: Html.LabelFor(model => model.QualityId )%> <%--   new {style="font-weight: bold"}.ToString()--%>  
                            <%:Html.DropDownListFor(model => model.QualityId, new SelectList(Model.QualityValueList, "Id", "Text", Model.QualityId),new {style="width: 300px;"})%>
                        </div>
                    </div>

               <div class="formRow">
                        <div class="group">
                            <%: Html.LabelFor(model => model.FactorReferenceOld)%> 
                            <%: Html.DisplayTextFor(model => model.FactorReferenceOld)%>
                        </div>
                    </div>

             <div class="formRow">
                        <div class="group">
                            <%: Html.LabelFor(model => model.FactorReferenceId )%> <%--   new {style="font-weight: bold"}.ToString()--%>  
                            <%:Html.DropDownListFor(model => model.FactorReferenceId, new SelectList(Model.FaktorReferenceList, "Id", "Text", Model.FactorReferenceId),new {style="width: 300px;"})%>
                        </div>
                    </div>
            
                       
                 <div class="formRow">
                        <div class="group">
                            <%: Html.DisplayTextFor(model => model.UpdateUserData)%>
                    <%--        <%: Html.DropDownListFor(model => model.SwedishOccurrenceReferenceId, new SelectList(Model.SwedishOccurrenceReferenceList, "Id", "Text", Model.SwedishOccurrenceReferenceId))%>
      --%>                        
                        </div>
                    </div>
                 <br/>
            </div>
        </fieldset> 
 </div> 
                  
                           
           
<% } %>

