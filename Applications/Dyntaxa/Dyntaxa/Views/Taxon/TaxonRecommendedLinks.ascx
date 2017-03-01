<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<ArtDatabanken.WebApplication.Dyntaxa.Data.LinkItem>>" %>
<%@Import namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<ul id="linkList">
<% foreach (var item in Model) 
    { %>
    <% if (item.Type == LinkType.Url) 
        { %>
        <li><a href="<%: item.Url %>" target="_blank"><%: item.LinkText%></a></li>
    <% } %>
    <% else if (item.Type == LinkType.Action) 
        { %>            
        <li><%: Html.ActionLink(item.LinkText, item.Action, item.Controller, new {taxonId = item.ParameterValue}, new {target = "_blank"}) %></li>
    <% } %>
    <% } %>
</ul>

