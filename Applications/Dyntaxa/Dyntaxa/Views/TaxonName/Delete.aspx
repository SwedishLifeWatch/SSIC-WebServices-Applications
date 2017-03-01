<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName.TaxonNameDeleteViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.Labels.TitleLabel %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="readHeader">
        <%: Model.Labels.TitleLabel %>
    </h1>
    <% if (ViewBag.Taxon != null)
        { %>
        <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
    <% }
        else
        { %>
        <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
    <% } %>
    
    <% using (Html.BeginForm())
       {%>    
       <%: Html.HiddenFor(m => m.TaxonId) %>
       <%: Html.HiddenFor(m => m.Version) %>  
        <% if (!this.ViewData.ModelState.IsValid)
        { %>
        <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
            <h2 class="validationSummaryHeader">
                <%:Html.ValidationSummary(false, "")%>
            </h2>
        </fieldset>
        <% } %>
    <!-- Full container start -->
    <div id="fullContainer">
        <fieldset>
            <h2><%: Model.Labels.TitleLabel %></h2>
            <div class="fieldsetContent">
                <%--<table class="display-table">--%>
                 <h3 class="open">
                    <%: Model.Labels.SharedDialogInformationHeader%>
                </h3>
                <div class="fieldsetSubContent fullWidth">
                    <table>
                    <tr>
                        <td><strong><%: Model.Labels.NameLabel %></strong></td>
                        <td><%: Model.Name %></td>                   
                    </tr>
                    <tr>
                        <td><strong><%: Model.Labels.AuthorLabel %></strong></td>
                        <td><%: Model.Author %></td>                   
                    </tr>
                    <tr>
                        <td><strong><%: Model.Labels.CategoryLabel %></strong></td>
                        <td><%: Model.Category %></td>                   
                    </tr>
                    <tr>
                        <td><strong><%: Model.Labels.NameUsageLabel %></strong></td>
                        <td><%: Model.NameUsage %></td>                   
                    </tr>
                    <tr>
                        <td><strong><%: Model.Labels.NameStatusLabel %></strong></td>
                        <td><%: Model.NameStatus %></td>                   
                    </tr>
                    <tr>
                        <td><strong><%: Model.Labels.RecommendedLabel %></strong></td>
                        <td><%:Html.CheckBoxFor(m => m.IsRecommended, new { @disabled = "disabled" })%></td>                   
                    </tr>
                </table>
                </div>
                  <p>  
                  <br/>
                    <strong><%: Model.Labels.DoYouWantToDeleteLabel %></strong>
                 <br/>
                </p>
                </div>
        </fieldset> 
        <fieldset>
            <div>
                 <p>
                    <input type="submit" id="<%:Model.Labels.GetSelectedDelete %>" value="<%: Resources.DyntaxaResource.SharedDeleteButtonText %>" class="ap-ui-button" />
                    <%: Html.ActionLink(Resources.DyntaxaResource.SharedCancelButtonText, "List", new { taxonId = Model.TaxonId }, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>                        
                </p>
            </div>
        </fieldset>   
         <!-- full container end -->
    </div>
    <% } %>
    
    <script language="javascript" type="text/javascript">



        $(document).ready(function () {


            $('#' + "<%:Model.Labels.GetSelectedDelete %>").click(function () {

                return showDialog("<%:Model.Labels.TitleLabel%>", "<%:Model.Labels.DeleteInfoTextLabel%>",
                               "<%:Model.Labels.SharedDeleteButtonText%>", "<%:Model.Labels.CancelButtonLabel%>", function () {
                                   $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Resources.DyntaxaResource.SharedDeleting %></h1>' });
                                   $('form').submit();
                               }, null);

            });

        });

</script> 
</asp:Content>
 