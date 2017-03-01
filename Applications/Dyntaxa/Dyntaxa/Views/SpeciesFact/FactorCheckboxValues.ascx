<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.SpeciesFactHostViewModelItem>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>

  

<input type="checkbox" name="<%=string.Format("taxonCheckboxValue_{0}",Model.TaxonId + "_" + Model.FactorId + "_" + Model.Type + "_" + Model.IsHostToFactor + "_" + Model.Name + "_" + Model.CategoryId)%> " value="<%= Model.Name %>" <% if (Model.IsChecked) { %> checked="checked" <% } %>  />
<%--<% 
    
   
    
    foreach (var fieldValue in Model.FieldValues.FactorFieldValues)
   {
       if (Model.FieldValues.MainParentFactorId == DyntaxaFactorEnumId.BIOTOPE || Model.FieldValues.MainParentFactorId == DyntaxaFactorEnumId.SUBSTRATE)
        {
            if (fieldValue.Value >= 0)
            {
                if (Model.IsHost)
                {%>
                     <input type="radio" disabled="disabled" name="<%=string.Format("factorEnumFieldValue_{0}",Model.FactorId + "_" + Model.HostId + "_" + Model.IndividualCategoryId + "_" + Model.ReferenceId + "_" + Model.QualityId)%> " value="<%= fieldValue.Value %>" <% if (fieldValue.Value == Model.FieldValues.FieldValue){ %> checked="checked" <% } %>/>   <%= fieldValue.Value %>
            
                <%}
                else
                 {%>
                    <input type="radio" name="<%=string.Format("factorEnumFieldValue_{0}",Model.FactorId + "_" + Model.HostId + "_" + Model.IndividualCategoryId + "_" + Model.ReferenceId + "_" + Model.QualityId)%> " value="<%= fieldValue.Value %>" <% if (fieldValue.Value == Model.FieldValues.FieldValue){ %> checked="checked" <% } %>/>   <%= fieldValue.Value %>
                <%}
            }
        }
    }%>--%>


<%--  <% if (Model.IsPossibleToChangeRecomended)
    { %>--%>
        <%--<input type="radio" name="IsRecommended" checkboxgroup="<%= Model.FieldValues.FieldName %>" value="<%= Model.FactorName %>" <% if (true) { %> checked="checked" <% } %>  />--%>
<%--        <% }
    else
    {%>
        <input type="checkbox" name="IsRecommended" disabled="disabled" checkboxgroup="<%= Model.CategoryId %>" value="<%= Model.CustomIdentifier %>" <% if (Model.IsRecommended) { %> checked="checked" <% } %>  />
        <input type="hidden" name="IsRecommended" value="<%= Model.CustomIdentifier %>" /> 
    <% } %>  --%>


