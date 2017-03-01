<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.SpeciesFactViewModelItem>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>

  

<% 
    
   foreach (var fieldValue in Model.FieldValues.FactorFieldValues)
   {
       if ( Model.FieldValues.MainParentFactorId == DyntaxaFactorId.SUBSTRATE)
        {
            if (fieldValue.Value >= 0)
            {
                if (Model.IsHost)
                {%>
                     <input type="radio" disabled="disabled" name="<%=string.Format("factorEnumFieldValue_{0}",Model.FactorId + "_" + Model.HostId + "_" + Model.IndividualCategoryId + "_" + Model.ReferenceId + "_" + Model.QualityId)%> " value="<%= fieldValue.Value %>" <% if (fieldValue.Value == Model.FieldValues.FieldValue){ %> checked="checked" <% } %>/>   <%= fieldValue.Value %>
            
                <%}
                else
                 {%>
                    <input type="radio" <% if(!Model.IsOkToUpdate) { %>  disabled="disabled" <% } %>  name="<%=string.Format("factorEnumFieldValue_{0}",Model.FactorId + "_" + Model.HostId + "_" + Model.IndividualCategoryId + "_" + Model.ReferenceId + "_" + Model.QualityId)%> " value="<%= fieldValue.Value %>" <% if (fieldValue.Value == Model.FieldValues.FieldValue){ %> checked="checked" <% } %>/>   <%= fieldValue.Value %>
                <%}
            }
        }
        if (Model.FieldValues.MainParentFactorId == DyntaxaFactorId.BIOTOPE )
        {
            if (fieldValue.Value >= 0)
            {
                %>
                    <input type="radio" <% if(!Model.IsOkToUpdate) { %>  disabled="disabled" <% } %> name="<%=string.Format("factorEnumFieldValue_{0}",Model.FactorId + "_" + Model.HostId + "_" + Model.IndividualCategoryId + "_" + Model.ReferenceId + "_" + Model.QualityId)%> " value="<%= fieldValue.Value %>" <% if (fieldValue.Value == Model.FieldValues.FieldValue){ %> checked="checked" <% } %>/>   <%= fieldValue.Value %>
                <%
            }
        }
       if (Model.FieldValues.MainParentFactorId == DyntaxaFactorId.INFLUENCE)
       {
           if (fieldValue.Value >= -2)
           {%>
                <input type="radio" <% if(!Model.IsOkToUpdate) { %>  disabled="disabled" <% } %> name="<%=string.Format("factorEnumFieldValue_{0}",Model.FactorId + "_" + Model.HostId + "_" + Model.IndividualCategoryId + "_" + Model.ReferenceId + "_" + Model.QualityId)%> " value="<%= fieldValue.Value %>" <% if (fieldValue.Value == Model.FieldValues.FieldValue){ %> checked="checked" <% } %>/>   <%= fieldValue.Value %>  
            <%
            }
       }
       if (Model.FieldValues.MainParentFactorId == DyntaxaFactorId.LIFEFORM)
       {
                if (Model.IsHost)
                {%>
                    <input type="radio" disabled="disabled" name="<%=string.Format("factorEnumFieldValue_{0}",Model.FactorId + "_" + Model.HostId + "_" + Model.IndividualCategoryId + "_" + Model.ReferenceId + "_" + Model.QualityId)%> " value="<%= fieldValue.Value %>" <% if (fieldValue.Value == Model.FieldValues.FieldValue){ %> checked="checked" <% } %>/>   <%= fieldValue.Value %>
                <%}
                else
                {%>
                    <input type="radio" <% if(!Model.IsOkToUpdate) { %>  disabled="disabled" <% } %> name="<%=string.Format("factorEnumFieldValue_{0}",Model.FactorId + "_" + Model.HostId + "_" + Model.IndividualCategoryId + "_" + Model.ReferenceId + "_" + Model.QualityId)%> " value="<%= fieldValue.Value %>" <% if (fieldValue.Value == Model.FieldValues.FieldValue){ %> checked="checked" <% } %>/>   <%= fieldValue.Value %>
                <%}
       }
    }%>


<%--  <% if (Model.IsPossibleToChangeRecomended)
    { %>--%>
        <%--<input type="radio" name="IsRecommended" checkboxgroup="<%= Model.FieldValues.FieldName %>" value="<%= Model.FactorName %>" <% if (true) { %> checked="checked" <% } %>  />--%>
<%--        <% }
    else
    {%>
        <input type="checkbox" name="IsRecommended" disabled="disabled" checkboxgroup="<%= Model.CategoryId %>" value="<%= Model.CustomIdentifier %>" <% if (Model.IsRecommended) { %> checked="checked" <% } %>  />
        <input type="hidden" name="IsRecommended" value="<%= Model.CustomIdentifier %>" /> 
    <% } %>  --%>


