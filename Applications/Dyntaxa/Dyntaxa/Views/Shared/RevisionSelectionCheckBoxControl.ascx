<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.RevisionSelectionItemModelHelper>" %>
    
    <%//  Autoposts form %>
    <%--<input type="checkbox" name="IsChecked" id="RevisionSelctionStatusId<%: Model.RevisionSelctionStatusId %>" value="<%= Model.RevisionSelctionStatusId %>" <% if (Model.IsChecked) { %> checked="checked" <% } %>  onclick="document.listRevisionForm.submit()"/>--%>
    
    <input type="checkbox" name="IsChecked" id="RevisionSelctionStatusId<%: Model.RevisionSelctionStatusId %>" value="<%= Model.RevisionSelctionStatusId %>" <% if (Model.IsChecked) { %> checked="checked" <% } %> />
    