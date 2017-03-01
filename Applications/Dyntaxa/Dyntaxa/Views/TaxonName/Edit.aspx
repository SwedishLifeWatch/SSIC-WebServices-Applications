<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName.TaxonNameDetailsViewModel>" %>

<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%:Resources.DyntaxaResource.TaxonNameEditTitle %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="readHeader">
        <%:Resources.DyntaxaResource.TaxonNameEditTitle %>
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
    <% using (Html.BeginForm("Edit", "TaxonName", FormMethod.Post, new { @id = "editTaxonNameForm", @name = "editTaxonNameForm" })) 
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
        <%: Html.HiddenFor(model => model.Version) %>
        <%: Html.HiddenFor(model => model.TaxonNameGuid) %>
        <%: Html.HiddenFor(model => model.NoOfTaxonNameReferences) %>
    <!-- Full container start -->
    <div id="fullContainer">
        <fieldset>
            <h2 class="open savestate"><%: Resources.DyntaxaResource.TaxonNameAddEditExistingNames%></h2>
            <div class="fieldsetContent">
              <% var grid = new WebGrid(source: Model.ExistingNames, canSort : false, canPage: false);
                 if (Model.ExistingNamesCurrentIndex.HasValue)
                 {
                     grid.SelectedIndex = Model.ExistingNamesCurrentIndex.Value;
                 }                                            
              %>            
              <%--Namnstatus, Vetenskapligt, Original, Obs-system, Senast uppdaterad, Uppdaterad av, NamnID.--%>
                <%: grid.GetHtml(
                        tableStyle: "grid",
                        headerStyle: "head",
                        alternatingRowStyle: "alt",
                        selectedRowStyle: "selected",
                        columns: grid.Columns(
                            grid.Column(columnName: "Name", header: Resources.DyntaxaResource.TaxonNameSharedName),
                            grid.Column(columnName: "Author", header: Resources.DyntaxaResource.TaxonNameSharedAuthor),
                            grid.Column("IsRecommended", Resources.DyntaxaResource.TaxonNameSharedIsRecommended,
                                 (m => m.IsRecommended ? Resources.DyntaxaResource.SharedBoolTrueText : Resources.DyntaxaResource.SharedBoolFalseText)),
                            grid.Column(columnName: "NameUsageText", header: Resources.DyntaxaResource.TaxonNameSharedNameUsage),
                            grid.Column(columnName: "NameStatusText", header: Resources.DyntaxaResource.TaxonNameSharedNomenclature),
                            // Vetenskapligt?
                            grid.Column("IsOriginal", Resources.DyntaxaResource.TaxonNameAddEditIsOriginal,
                                 (m => m.IsOriginal ? Resources.DyntaxaResource.SharedBoolTrueText : Resources.DyntaxaResource.SharedBoolFalseText)),
                            grid.Column("IsNotOkForObsSystem", Resources.DyntaxaResource.TaxonNameSharedOkForObsSystem,
                                 (m => !m.IsNotOkForObsSystem ? Resources.DyntaxaResource.SharedBoolTrueText : Resources.DyntaxaResource.SharedBoolFalseText)),
                            grid.Column("LastUpdated", Resources.DyntaxaResource.TaxonNameSharedLastUpdated,
                                 (m => m.LastUpdated )),
                            grid.Column(columnName: "UpdatedBy", header: Resources.DyntaxaResource.TaxonNameSharedUpdatedBy),
                            grid.Column(columnName: "CategoryName", header: Resources.DyntaxaResource.TaxonNameDeleteCategory),
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
                                 <% if (Model.IsPossibleToChangeUsage)
                                { %>
                                    <%: Html.DropDownListFor(x => x.SelectedTaxonNameStatusId, new SelectList(Model.TaxonNameStatusList, "Id", "Text", Model.SelectedTaxonNameStatusId))%>    
                                 <% }
                                else
                                {%>
                                    <%: Html.DropDownListFor(x => x.SelectedTaxonNameStatusId, new SelectList(Model.TaxonNameStatusList, "Id", "Text", Model.SelectedTaxonNameStatusId), new { @disabled = "disabled" })%>
                                <% } %>
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
                      <%--      <%: Html.ValidationMessageFor(model => model.IsNotOkForObsSystem)%>   --%>                     
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
            <h2 class="open"><%: Resources.DyntaxaResource.SharedReferences%></h2>
            <div class="fieldsetContent">                
               <% if (Model.NoOfTaxonNameReferences > 0)
               { %>
                      <% Html.RenderAction("ListReferences", "Reference", new {@guid = Model.TaxonNameGuid}); %> 
                <% }
                else
                {%>
                    <div class="editor-label">
                        <strong><%: Resources.DyntaxaResource.SharedNoValidReferenceErrorText%></strong>
                    </div>
                <% }%>
                <%: Html.ValidationMessageFor(model => model.NoOfTaxonNameReferences, "", new { @style = "display:inline;" })%>         
                <%: Html.RenderAddReferenceLink(Model.TaxonNameGuid, Resources.DyntaxaResource.SharedManageReferences, "Edit", "TaxonName", new { @taxonId = Model.TaxonId, @nameId = Model.Version }, new { @class = "ap-ui-button", @style = "margin-left: 10px;" })%>
            </div>
        </fieldset>
              
        <fieldset>
            <input id="btnPost" type="submit" value="<%: Resources.DyntaxaResource.SharedSaveButtonText %>" class="ap-ui-button" />
            <%: Html.ActionLink(Resources.DyntaxaResource.SharedResetButtonText, "Edit", new { taxonId = Model.TaxonId, nameId = Model.Version }, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>
            <%: Html.ActionLink(Resources.DyntaxaResource.SharedReturnToListText, "List", new { taxonId = Model.TaxonId }, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>   
        </fieldset>
        <!-- full container end -->
    </div>
    <% } %>

    <script type="text/javascript">
        $(document).ready(function () {            

            initDetectFormChanges();
            $('a[href*="Reference/Add"]').click(function (e) {
                var element = this;
                var changed = isAnyFormChanged();

                if (changed) {
                    e.preventDefault();
                    showYesNoDialog("<%: Resources.DyntaxaResource.SharedDoYouWantToContinue %>",
                        "<%: Resources.DyntaxaResource.SharedGoToReferenceViewQuestion %>",
                        "<%: Resources.DyntaxaResource.SharedDialogButtonTextYes %>",
                        function () {
                            e.view.window.location = element.href; // on click redirect                             
                        },
                        "<%: Resources.DyntaxaResource.SharedDialogButtonTextNo %>",
                        null);
                }
            });

            $("#btnPost").click(function () {
                var frm = document.editTaxonNameForm;
                $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Resources.DyntaxaResource.SharedSaving %></h1>' });
                frm.submit();
            });
        });

//        function onSelectedIndexChanged() {
////            var sub = document.getElementsByName("co.formAction");
////            sub.value = "";
//            //            document.forms[0].submit();
//            var theform = document.editTaxonNameForm;
//            theform.submit();  
//        }
  </script>

</asp:Content>
