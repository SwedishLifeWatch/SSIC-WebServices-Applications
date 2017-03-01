<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.RevisionInitializeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Resources.DyntaxaResource.SharedRevisionInitializeLabel%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm())
{%>
    
    <% if (!this.ViewData.ModelState.IsValid)
        { %>
        <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
            <h2 class="validationSummaryHeader">
                <%:Html.ValidationSummary(false, "")%>
            </h2>
        </fieldset>
        <% } %>

    <div id="fullContainer">
         <fieldset>
            <h1 class="readHeader"><%: Resources.DyntaxaResource.SharedRevisionInitializeLabel %></h1>    
            <div class="fieldsetContent">   
                    <br/>
                    <%: Resources.DyntaxaResource.SharedRevisionInitializeInformation %> 
                    <br/>
            <input type="hidden" id="revisionId" name="revisionId" value="<%:Model.RevisionId%>" />
            <input type="hidden" id="taxonId" name="taxonId" value="<%:Model.TaxonId%>" />
            </div>
        </fieldset>
         <p>
            <input class="ap-ui-button" type="submit" name="submitButton" value="<%: Resources.DyntaxaResource.SharedRevisionInitializeButtonText%>"/>
            <%: Html.ActionLink(Resources.DyntaxaResource.SharedCancelButtonText, "Edit", new { revisionId = Model.RevisionId }, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>   
         </p>          
     </div>

<% } %>

</asp:Content>
