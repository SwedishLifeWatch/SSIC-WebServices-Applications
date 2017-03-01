<%--<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.Reference.CreateNewReferenceViewModel>" %>--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.Reference.CreateNewReferenceViewModel>" %>

<% Html.EnableClientValidation(); %> 
    <% using (Html.BeginForm("New", "Reference", FormMethod.Post, new { @id = "newReferenceForm", @name = "newReferenceForm" }))
    { %>    
        <% if (!this.ViewData.ModelState.IsValid)
        { %>
        <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
            <h2 class="validationSummaryHeader">
                <%:Html.ValidationSummary(false, "")%>
            </h2>
        </fieldset>
        <% } %>    

    <table>        
        <tr>
            <td>
                <%: Resources.DyntaxaResource.ReferenceAddName%>
            </td>
            <td>
                <%: Html.TextBoxFor(m => Model.Reference.Name, new {@style ="width:400px;"}) %>
                <%:Html.ValidationMessageFor(m => m.Reference.Name)%>
            </td>
        </tr>    
        <tr>
            <td>
                <%: Resources.DyntaxaResource.ReferenceAddYear%>
            </td>
            <td>
                <%: Html.TextBoxFor(m => m.Reference.Year, new { @style = "width:400px;" })%>
                <%: Html.ValidationMessageFor(m => m.Reference.Year)%>
            </td>
        </tr>    
        <tr>
            <td>
                <%: Resources.DyntaxaResource.ReferenceAddText%>
            </td>
            <td>
                <%: Html.TextAreaFor(m => m.Reference.Text, 5, 160, new {@style= "width: 400px;"}) %>                                                            
                <%: Html.ValidationMessageFor(m => m.Reference.Text)%>
            </td>
        </tr>  
    </table>     
<% } %>
