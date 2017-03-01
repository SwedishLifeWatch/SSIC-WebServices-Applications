<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.SpeciesFactViewModelItem>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>

<% if(Model.IsHost && ((int)Model.MainParentFactorId == (int)DyntaxaFactorId.SUBSTRATE)) { %>
    <% string val = "";
        foreach (var fieldValue in Model.FieldValues2.FactorFieldValues)
       {
           if (fieldValue.Value == Model.FieldValues2.FieldValue)
           {
               val = fieldValue.Key;
               break;
           }
       }        
      %>
    <%: val %>
<% } 
   else
   { %>
    <select name="<%=string.Format("factorEnumFieldValue2_{0}",Model.FactorId + "_" + Model.HostId + "_" + Model.IndividualCategoryId + "_" + Model.ReferenceId + "_" + Model.QualityId)%>">      
    <%     
       foreach (var fieldValue in Model.FieldValues2.FactorFieldValues)
       {
           if (fieldValue.Value >= 0 || fieldValue.Value == -1000)
            {
           %>
            <option value="<%= fieldValue.Value %>"  <% if (fieldValue.Value == Model.FieldValues2.FieldValue){ %> selected="selected" <% } %> > <%: fieldValue.Key %></option>   
        <%} %>
      <%}                  
    %>    
    </select>
<% } %>



<%--  <% if (Model.IsPossibleToChangeRecomended)
    { %>--%>
        <%--<input type="radio" name="IsRecommended" checkboxgroup="<%= Model.FieldValues.FieldName %>" value="<%= Model.FactorName %>" <% if (true) { %> checked="checked" <% } %>  />--%>
<%--        <% }
    else
    {%>
        <input type="checkbox" name="IsRecommended" disabled="disabled" checkboxgroup="<%= Model.CategoryId %>" value="<%= Model.CustomIdentifier %>" <% if (Model.IsRecommended) { %> checked="checked" <% } %>  />
        <input type="hidden" name="IsRecommended" value="<%= Model.CustomIdentifier %>" /> 
    <% } %>  --%>


