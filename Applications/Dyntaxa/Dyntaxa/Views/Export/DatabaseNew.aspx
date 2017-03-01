<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.ExportDatabaseViewModel>" %>

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

    <% using (Html.BeginForm("DatabaseNew", "Export", FormMethod.Post))
       { %>
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
            <h2 class="open"><%:Model.Labels.Input%></h2>
            <div class="fieldsetContent">
                <table class="display-table">
                    <tr>
                        <td>
                            <div class="group">
                                <%:Html.LabelFor(model => model.ClipBoard)%>
                                <%:Html.TextAreaFor(model => model.ClipBoard, new {id = "clipBoardTextArea", @class = ""})%>                                
                            </div>
                        </td>
                    </tr>
                </table>
                <table class="display-table">
                    <tr>
                        <td>
                            <div class="group">
                                <%: Html.LabelFor(model => model.RowDelimiter) %>
                                <%: Html.DropDownList("RowDelimiter")%>
                            </div>
                        </td>                        
                    </tr>      
                </table>                                                                              
            </div>            
        </fieldset>

        <p>            
            <button id="btnGetExcelFileNew" type="submit" class="ap-ui-button"><%:Model.Labels.GetExcelFile%> New</button>            
        </p>

    <!-- full container end -->
    </div>    

    <% } %>
</asp:Content>
