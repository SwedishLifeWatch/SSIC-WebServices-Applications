<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.Taxon.DistributionInSwedenViewModel>" %>

<% if (Model.DistributionLink == null && Model.ApReportLink == null)
   { %>
<div style="padding:5px; margin-left: 5px;">
<%: Resources.DyntaxaResource.SharedNoAvailableData %>
    </div>
<% }
   else
   {
%>
<ul id="linkList">
    <% if (Model.DistributionLink != null)
       { %>
    <li>
        <a href="<%: Model.DistributionLink.Url %>" target="_blank"><%: Model.DistributionLink.LinkText %></a>
    </li>
    <% } %>
    <% if (Model.ApReportLink != null)
       { %>
    <li>
        <a href="<%: Model.ApReportLink.Url %>" target="_blank"><%: Model.ApReportLink.LinkText %></a>
    </li>
    <% } %>
</ul>
<% } %>
