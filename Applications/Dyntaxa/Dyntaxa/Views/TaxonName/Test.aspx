<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName.TaxonNameDetailsViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Test
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Test</h2>

<script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
<script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

<% using (Html.BeginForm()) { %>
        <% if (!this.ViewData.ModelState.IsValid)
        { %>
        <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
            <h2 class="validationSummaryHeader">
                <%:Html.ValidationSummary(false, "")%>
            </h2>
        </fieldset>
        <% } %>
    <fieldset>
        <legend>AddTaxonNameModel</legend>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.TaxonId) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.TaxonId) %>
            <%: Html.ValidationMessageFor(model => model.TaxonId) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Name) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Name) %>
            <%: Html.ValidationMessageFor(model => model.Name) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Author) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Author) %>
            <%: Html.ValidationMessageFor(model => model.Author) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.SelectedCategoryId) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.SelectedCategoryId) %>
            <%: Html.ValidationMessageFor(model => model.SelectedCategoryId) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Comment) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Comment) %>
            <%: Html.ValidationMessageFor(model => model.Comment) %>
        </div>

        <p>
            <input type="submit" value="Save" />
        </p>
    </fieldset>
<% } %>

<div>
    <%: Html.ActionLink("Back to List", "Index") %>
</div>

</asp:Content>
