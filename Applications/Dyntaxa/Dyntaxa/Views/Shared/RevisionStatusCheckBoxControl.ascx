<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.RevisionStatusItemModelHelper>" %>

<tr>
    <td>
        <input type="checkbox" id="<%= Model.RevisionStatusId %>" name="IsChecked" value="<%= Model.RevisionStatusId %>" <% if (Model.IsChecked) { %> checked="checked" <% } %>/>        
    </td>
    <td>
        <label for="<%: Model.RevisionStatusId %>" class="checkboxLabel"><%: Model.RevisionStatusName%></label>
    </td>
</tr>