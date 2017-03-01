<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.RevisionTaxonInfoViewModel>" %>
<%@Import namespace="Dyntaxa.Helpers.Extensions" %>

    <h1 class="readHeader">
        <%: Model.MainHeaderText + " " %>
        <strong><%: Model.Category %>:</strong>
        <%= Model.ScientificName != null ? Html.RenderScientificName(Model.ScientificName, null, Model.CategorySortOrder).ToString() : "" %>
        <%= Model.CommonName != null ? Html.Encode(string.Format(" - {0}", Model.CommonName)) : "" %> 
   <%--     <%: " " + Model.RevisionText + Model.Id.ToString()%>      --%>  
   </h1>



