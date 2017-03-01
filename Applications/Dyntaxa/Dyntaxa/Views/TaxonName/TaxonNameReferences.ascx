<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName.TaxonNameEditingViewModel>" %>
<% if (Model.References.Count > 0)
   { %>
<ul style="list-style-type: disc">
    <% foreach (string strReference in Model.References)
       {%>
            <li><%:strReference%></li>
    <% }%>
</ul>
<% } %>
