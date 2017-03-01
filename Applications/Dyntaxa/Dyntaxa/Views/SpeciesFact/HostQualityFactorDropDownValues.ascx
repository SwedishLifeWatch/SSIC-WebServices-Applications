<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.SpeciesFactViewModelItem>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>


  

    <select name="<%=string.Format("factorEnumFieldValue3_{0}",Model.FactorId + "_" + Model.HostId + "_" + Model.IndividualCategoryId + "_" + Model.ReferenceId + "_" + Model.QualityId)%>">      
    <%     
        foreach (var qualityValue in Model.QualityValues.QualityValues)
       {
           if (qualityValue.Key >= 0 || qualityValue.Key == -1000)
            {
           %>
            <option value="<%= qualityValue.Key %>"  <% if (qualityValue.Key == Model.QualityId)
                                                        { %> selected="selected" <% } %> > <%: qualityValue.Value %></option>   
        <%} %>
      <%}                    
    %>    
    </select>




<%--  <% if (Model.IsPossibleToChangeRecomended)
    { %>--%>
        <%--<input type="radio" name="IsRecommended" checkboxgroup="<%= Model.FieldValues.FieldName %>" value="<%= Model.FactorName %>" <% if (true) { %> checked="checked" <% } %>  />--%>
<%--        <% }
    else
    {%>
        <input type="checkbox" name="IsRecommended" disabled="disabled" checkboxgroup="<%= Model.CategoryId %>" value="<%= Model.CustomIdentifier %>" <% if (Model.IsRecommended) { %> checked="checked" <% } %>  />
        <input type="hidden" name="IsRecommended" value="<%= Model.CustomIdentifier %>" /> 
    <% } %>  --%>


