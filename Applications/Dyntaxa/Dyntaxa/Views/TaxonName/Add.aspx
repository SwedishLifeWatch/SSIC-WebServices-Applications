<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName.TaxonNameDetailsViewModel>" %>

<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
     <%:Resources.DyntaxaResource.TaxonNameAddTitle %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="readHeader">
        <%:Resources.DyntaxaResource.TaxonNameAddTitle %>
    </h1>

    <% if (ViewBag.Taxon != null)
        { %>
        <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
    <% }
        else
        { %>
        <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
    <% } %>
    
    <% Html.EnableClientValidation(); %> 

    <% using (Html.BeginForm("Add", "TaxonName", FormMethod.Post, new { @id = "taxonNameAddForm", @name = "taxonNameAddForm" }))        
    {%> 
        
        <% if (!this.ViewData.ModelState.IsValid)
        { %>
        <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
            <h2 class="validationSummaryHeader">
                <%:Html.ValidationSummary(false, "")%>
            </h2>
        </fieldset>
        <% } %>
        
        <%: Html.HiddenFor(model => model.TaxonId) %>
        <%: Html.HiddenFor(model => model.NoOfTaxonNameReferences) %>
       
       
    <!-- Full container start -->
    <div id="fullContainer">
        <fieldset>
            <h2 class="open savestate"><%: Resources.DyntaxaResource.TaxonNameAddEditExistingNames%></h2>
            <div class="fieldsetContent">
              <% var grid = new WebGrid(source: Model.ExistingNames, canSort: false, canPage: false); %>
                                      
               <%: grid.GetHtml(
                    tableStyle: "grid",
                    headerStyle: "head",
                    alternatingRowStyle: "alt",                    
                    columns: grid.Columns(
                        grid.Column(columnName: "Name", header: Resources.DyntaxaResource.TaxonNameSharedName),
                        grid.Column(columnName: "Author", header: Resources.DyntaxaResource.TaxonNameSharedAuthor),
                        grid.Column("IsRecommended", Resources.DyntaxaResource.TaxonNameSharedIsRecommended,
                                (m => m.IsRecommended ? Resources.DyntaxaResource.SharedBoolTrueText : Resources.DyntaxaResource.SharedBoolFalseText)),
                        grid.Column(columnName: "NameStatusText", header: Resources.DyntaxaResource.TaxonNameAddEditNameUsageHeader),
                        // Vetenskapligt?
                        grid.Column("IsOriginal", Resources.DyntaxaResource.TaxonNameAddEditIsOriginal,
                                (m => m.IsOriginal ? Resources.DyntaxaResource.SharedBoolTrueText : Resources.DyntaxaResource.SharedBoolFalseText)),
                        grid.Column("IsNotOkForObsSystem", Resources.DyntaxaResource.TaxonNameSharedOkForObsSystem,
                                (m => !m.IsNotOkForObsSystem ? Resources.DyntaxaResource.SharedBoolTrueText : Resources.DyntaxaResource.SharedBoolFalseText)),
                        grid.Column("LastUpdated", Resources.DyntaxaResource.TaxonNameSharedLastUpdated,
                                (m => m.LastUpdated )),
                        grid.Column(columnName: "UpdatedBy", header: Resources.DyntaxaResource.TaxonNameSharedUpdatedBy),
                        grid.Column(columnName: "Name", header: Resources.DyntaxaResource.TaxonNameDeleteCategory),
                        grid.Column(columnName: "Id", header: Resources.DyntaxaResource.TaxonNameSharedId)
                    )
                )    
                %>

            </div>
        </fieldset>

        <fieldset>
            <h2 class="open"><%: Resources.DyntaxaResource.TaxonNameAddEditNameHeader%></h2>
            <div class="fieldsetContent">
                <table class="display-table">
                    <tr>
                        <td colspan="2">
                            <div class="group">
                                <%:Html.LabelFor(m=>m.SelectedCategoryId) %>
                                <%: Html.DropDownListFor(x => x.SelectedCategoryId, new SelectList(Model.CategoryList, "Id", "Text", Model.SelectedCategoryId))%>
                                <%--<%: Html.DropDownListFor(x => x.SelectedCategoryId, new SelectList(Model.CategoryList, "Id", "Name"), "-- Please select a category --")%>                          --%>
                            </div>                        
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="group">
                                <%: Html.LabelFor(m => m.Name) %>
                                <%: Html.TextBoxFor(m => Model.Name, new {@style ="width:400px;"}) %>
                                <%:Html.ValidationMessageFor(model => model.Name)%>
                            </div>
                        </td>
                        <td>
                            <div class="group">
                                <%: Html.LabelFor(m => m.Author) %>
                                <%: Html.TextBoxFor(m => Model.Author, new { @style = "width:400px;" })%>
                                <%: Html.ValidationMessageFor(model => model.Author)%>                        
                            </div>
                        </td>
                    </tr>                
                </table>                    
                <table class="display-table">
                    <tr>
                        <td>
                            <div class="group">
                                <%:Html.LabelFor(m=>m.SelectedTaxonNameUsageId) %>
                                <%: Html.DropDownListFor(x => x.SelectedTaxonNameUsageId, new SelectList(Model.TaxonNameUsageList, "Id", "Text", Model.SelectedTaxonNameUsageId))%>
                            </div>                                       
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="group">
                                <%:Html.LabelFor(m=>m.SelectedTaxonNameStatusId) %>
                                <%: Html.DropDownListFor(x => x.SelectedTaxonNameStatusId, new SelectList(Model.TaxonNameStatusList, "Id", "Text", Model.SelectedTaxonNameStatusId))%>
                            </div>                                       
                        </td>
                    </tr>                    
                    <tr>
                        <td>
                            <%: Resources.DyntaxaResource.TaxonNameSharedIsRecommended %>:
                            <%: (Model.IsRecommended ? Resources.DyntaxaResource.SharedBoolTrueText : Resources.DyntaxaResource.SharedBoolFalseText)%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%: Html.CheckBoxFor(m => Model.IsNotOkForObsSystem)%>
                            <%: Html.LabelFor(m => m.IsNotOkForObsSystem)%>                    
                           <%-- <%: Html.ValidationMessageFor(model => model.IsNotOkForObsSystem)%>   --%>              
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%: Html.CheckBoxFor(m => Model.IsOriginal)%>
                            <%: Html.LabelFor(m => m.IsOriginal)%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="group">
                                <%: Html.LabelFor(m => m.Comment) %>                        
                                <%: Html.TextAreaFor(m => m.Comment, 5, 160, new {@style= "width: 700px;"}) %>                                            
                                <%: Html.ValidationMessageFor(model => model.Comment)%>
                            </div>                              
                        </td>
                    </tr>
                </table>

            </div>
        </fieldset>

         <fieldset>
            <input id="btnPost" type="submit" value="<%: Resources.DyntaxaResource.SharedSaveButtonText %>" class="ap-ui-button" />
            <%: Html.ActionLink(Resources.DyntaxaResource.SharedResetButtonText, "Add", new { taxonId = Model.TaxonId }, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>            
            <%: Html.ActionLink(Resources.DyntaxaResource.SharedReturnToListText, "List", new { taxonId = Model.TaxonId }, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>   
        </fieldset>
        <!-- full container end -->
    </div>
    <% } %>

 <script type="text/javascript">
     $(document).ready(function () {
         // animate visibility for fieldsets
         //initToggleFieldsetH2();

         $("#btnPost").click(function () {
             var frm = document.taxonNameAddForm;
             $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Resources.DyntaxaResource.SharedSaving %></h1>' });
             frm.submit();
         });
     });
  </script>
</asp:Content>
