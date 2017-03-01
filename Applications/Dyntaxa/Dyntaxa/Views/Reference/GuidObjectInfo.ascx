<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.Reference.GuidObjectViewModel>" %>
<table>
    <tr>
        <td>
           <%: Model.Labels.GuidLabel %>
        </td>
        <td>
            <%: Model.GUID %>
        </td>
    </tr>
    <% if (!string.IsNullOrEmpty(Model.ID))
       {%>
    <tr>
        <td>
           <%:Model.Labels.IdLabel%>
        </td>
        <td>
            <%:Model.ID%>
        </td>
    </tr>
    <% } %>
    <tr>
        <td>
           <%: Model.Labels.TypeDescriptionLabel %>
        </td>
        <td>
            <%: Model.TypeDescription %>
        </td>
    </tr>
    <% if (!string.IsNullOrEmpty(Model.Description))
       {%>
    <tr>
        <td>
           <%:Model.Labels.DescriptionLabel%>
        </td>
        <td>
            <%:Model.Description%>
        </td>
    </tr>
    <% } %>
</table>


