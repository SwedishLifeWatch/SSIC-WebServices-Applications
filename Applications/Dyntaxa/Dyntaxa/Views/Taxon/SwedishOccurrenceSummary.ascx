<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.Taxon.SwedishOccurrenceSummaryViewModel>" %>

<div style="padding: 3px 6px 6px 8px;">
<strong><%: Model.Labels.SwedishOccurrenceLabel %></strong>: <%= Html.Encode(!string.IsNullOrEmpty(Model.SwedishOccurrence) ? Model.SwedishOccurrence : "-"   ) %><br/>
<strong><%: Model.Labels.SwedishHistoryLabel %></strong>: <%= Html.Encode(!string.IsNullOrEmpty(Model.SwedishHistory) ? Model.SwedishHistory : "-"   ) %><br/>
<strong><%: Model.Labels.RedListClassLabel %></strong>: <%= Html.Encode(!string.IsNullOrEmpty(Model.RedListInfo) ? Model.RedListInfo : "-"   ) %>
<% if (Model.RedListLink != null) {%>
<br/><a href="<%: Model.RedListLink.Url %>" target="_blank"><%: Model.RedListLink.LinkText%></a>
<%} %>
</div>
