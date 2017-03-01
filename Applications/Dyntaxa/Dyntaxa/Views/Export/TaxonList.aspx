<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.Export.ExportViewModel>" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.Title %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="readHeader"><%: Model.Title %></h1>    
    <% if (ViewBag.Taxon != null)
        { %>
        <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
    <% }
        else
        { %>
        <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
    <% } %>    


    <% using (Html.BeginForm(Model.PostAction, "Export", FormMethod.Post, new { @id = "exportForm", @name = "exportForm" })) { %>

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
        <%: Html.HiddenFor(m => m.TaxonId) %>        
        <%: Html.Hidden("downloadTokenValue")%>        

        <fieldset>
            <h2 class="closed savestate"><%: Model.Labels.FilterLabel %></h2>
            <div class="fieldsetContent" id="MainTaxonCategoriesDiv">                
                <table class="display-table">
                    <tr>
                        <td>
                            <%: Model.Labels.CategoriesLabel %>
                        </td>
                        <td>
                            <select id="filterTaxonCategories" name="filterTaxonCategories" multiple="multiple">
				                <optgroup label="<%: Model.Labels.SelectLabel %>">
					                <option value="<%= Model.AllTaxonCategoryId %>"><%: Model.Labels.AllOptionLabel %></option>					        
				                </optgroup>
                                <% if (Model.FilterParentTaxonCategories != null && Model.FilterParentTaxonCategories.Count > 0)
                                { %>
				                    <optgroup label="<%: Model.Labels.ParentsLabel %>">
                                        <% foreach (var category in Model.FilterParentTaxonCategories)
                                            {%>
                                                <option value="<%=category.CategoryId%>" <% if (category.IsChecked){ %> selected="selected" <% } %>><%:category.CategoryName%></option>
                                        <% } %>
				                    </optgroup>
                                <% } %>
				                    <optgroup label="<%: Model.Labels.CurrentLabel %>">
                                        <option value="<%=Model.FilterCurrentTaxonCategory.CategoryId%>" <% if (Model.FilterCurrentTaxonCategory.IsChecked){ %> selected="selected" <% } %>><%: Model.FilterCurrentTaxonCategory.CategoryName%></option>
				                    </optgroup>
                                <% if (Model.FilterChildrenTaxonCategories != null && Model.FilterChildrenTaxonCategories.Count > 0)
                                   { %>
	                            <optgroup label="<%: Model.Labels.ChildrenLabel %>">
                                <% foreach (var category in Model.FilterChildrenTaxonCategories)
                                   {%>
                                        <option value="<%= category.CategoryId %>" <% if(category.IsChecked) { %> selected="selected" <% } %>><%: category.CategoryName%></option>
                                <% } %>
				                </optgroup>
                                <%} %>
	                        </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%: Model.Labels.SwedishOccurrenceLabel %>:
                        </td>                        
                        <td>                                                        
                            <select id="filterSwedishOccurrence" name="filterSwedishOccurrence" multiple="multiple">				                
                                <option value="<%= Model.AllTaxonCategoryId %>"><%: Model.Labels.AllOptionLabel %></option>
                                <% foreach (var val in Model.FilterSwedishOccurrenceValues)
                                   {%>
                                        <option value="<%=val.Id%>" <% if (val.IsChecked){ %> selected="selected" <% } %>><%:val.Name%></option>
                                <% } %>
	                        </select>
                        <%--Html.ListBoxFor(x => x.Vals, new MultiSelectList(new [] {"A", "B"}))--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%: Model.Labels.SwedishHistoryLabel %>:
                        </td>                        
                        <td>                                                        
                            <select id="filterSwedishHistory" name="filterSwedishHistory" multiple="multiple">				                
                                <option value="<%= Model.AllTaxonCategoryId %>"><%: Model.Labels.AllOptionLabel %></option>
                                <% foreach (var val in Model.FilterSwedishHistoryValues)
                                   {%>
                                        <option value="<%=val.Id%>" <% if (val.IsChecked){ %> selected="selected" <% } %>><%:val.Name%></option>
                                <% } %>
	                        </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%: Model.Labels.IsValidTaxonLabel %>
                        </td>
                        <td>
                            <%: Html.DropDownListFor(x => x.FilterIsValidTaxon, Html.CreateNullableBoolSelectlist(Model.FilterIsValidTaxon))%>
                        </td>
                    </tr>
                </table>

            </div>
        </fieldset>       
                

        <fieldset>
            <h2 class="closed savestate"><%: Model.Labels.OutputTitle %></h2>
            <div class="fieldsetContent" id="GeneralOptionsDiv">
                <table class="display-table">            
                    <tr>  
                        <td>
                            <%: Model.Labels.CategoriesLabel %>:
                        </td>                      
                        <td>                            
                            <select id="outputTaxonCategories" name="outputTaxonCategories" multiple="multiple">
				                <optgroup label="Select">
					                <option value="<%= Model.AllTaxonCategoryId %>"><%: Model.Labels.AllOptionLabel %></option>					        
				                </optgroup>
                                <% if (Model.OutputParentTaxonCategories != null && Model.OutputParentTaxonCategories.Count > 0)
                                { %>
				                    <optgroup label="<%: Model.Labels.ParentsLabel %>">
                                        <% foreach (var category in Model.OutputParentTaxonCategories)
                                            {%>
                                                <option value="<%=category.CategoryId%>" <% if (category.IsChecked){ %> selected="selected" <% } %>><%:category.CategoryName%></option>
                                        <% } %>
				                    </optgroup>
                                <% } %>
				                    <optgroup label="<%: Model.Labels.CurrentLabel %>">
                                        <option value="<%=Model.OutputCurrentTaxonCategory.CategoryId%>" <% if (Model.OutputCurrentTaxonCategory.IsChecked){ %> selected="selected" <% } %>><%: Model.OutputCurrentTaxonCategory.CategoryName%></option>
				                    </optgroup>
                                <% if (Model.OutputChildTaxonCategories != null && Model.OutputChildTaxonCategories.Count > 0)
                                   { %>
	                            <optgroup label="<%: Model.Labels.ChildrenLabel %>">
                                <% foreach (var category in Model.OutputChildTaxonCategories)
                                   {%>
                                        <option value="<%= category.CategoryId %>" <% if(category.IsChecked) { %> selected="selected" <% } %>><%: category.CategoryName%></option>
                                <% } %>
				                </optgroup>
                                <%} %>
	                        </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%: Model.Labels.TaxonNamesLabel %>:
                        </td>                        
                        <td>                                                    
                            <select id="outputTaxonNames" name="outputTaxonNames" multiple="multiple">				                
                                <option value="<%= Model.AllTaxonCategoryId %>"><%: Model.Labels.AllOptionLabel %></option>
                                <% foreach (var nameType in Model.OutputTaxonNameCategories)
                                   {%>
                                        <option value="<%=nameType.Id%>" <% if (nameType.IsChecked){ %> selected="selected" <% } %>><%:nameType.Name%></option>
                                <% } %>				                
	                        </select>
                        </td>
                    </tr>        
                </table>

                <table class="display-table">
                    <tr>
                        <td>
                            <div class="checkboxToggle">
                                <a href="#" class="toggleCheckbox"><%: Model.Labels.CheckAllNoneLabel %></a>
                            </div>
                        </td>
                    </tr>  
                    <tr>
                        <td>                            
                            <%: Html.CheckBoxFor(m => m.OutputScientificName)%>
                            <%: Html.LabelFor(m => m.OutputScientificName)%>                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%: Html.CheckBoxFor(m => m.OutputAuthor)%>
                            <%: Html.LabelFor(m => m.OutputAuthor)%>                                                        
                        </td>
                    </tr>
                    <tr>
                        <td>                            
                            <%: Html.CheckBoxFor(m => m.OutputAuthorInAllNameCells)%>
                            <%: Html.LabelFor(m => m.OutputAuthorInAllNameCells)%>                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%: Html.CheckBoxFor(m => m.OutputCommonName)%>
                            <%: Html.LabelFor(m => m.OutputCommonName)%>                                                        
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%: Html.CheckBoxFor(m => m.OutputCommonNameInAllNameCells)%>
                            <%: Html.LabelFor(m => m.OutputCommonNameInAllNameCells)%>                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%: Html.CheckBoxFor(m => m.OutputTaxonCategory)%>
                            <%: Html.LabelFor(m => m.OutputTaxonCategory) %>                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%: Html.CheckBoxFor(m => m.OutputTaxonId)%>
                            <%: Html.LabelFor(m => m.OutputTaxonId)%>                                
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%: Html.CheckBoxFor(m => m.OutputGUID)%>
                            <%: Html.LabelFor(m => m.OutputGUID)%>                                
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%: Html.CheckBoxFor(m => m.OutputRecommendedGUID)%>
                            <%: Html.LabelFor(m => m.OutputRecommendedGUID)%>                                
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%: Html.CheckBoxFor(m => m.OutputTaxonUrl)%>                            
                            <%: Html.LabelFor(m => m.OutputTaxonUrl)%> 
                        </td>
                    </tr>                    
                    <tr>
                        <td>
                            <%: Html.CheckBoxFor(m => m.OutputSwedishOccurrence)%>                            
                            <%: Html.LabelFor(m => m.OutputSwedishOccurrence)%> 
                        </td>
                    </tr>       
                    <tr>
                        <td>
                            <%: Html.CheckBoxFor(m => m.OutputSwedishHistory)%>                            
                            <%: Html.LabelFor(m => m.OutputSwedishHistory)%> 
                        </td>
                    </tr>       
                    <tr>
                        <td>
                            <%: Html.CheckBoxFor(m => m.OutputSynonyms)%>                            
                            <%: Html.LabelFor(m => m.OutputSynonyms)%> 
                        </td>
                    </tr>    
                    <tr>
                        <td>
                            <%: Html.CheckBoxFor(m => m.OutputProParteSynonyms)%>                            
                            <%: Html.LabelFor(m => m.OutputProParteSynonyms)%> 
                        </td>
                    </tr>    
                    <tr>
                        <td>
                            <%: Html.CheckBoxFor(m => m.OutputMisappliedNames)%>                            
                            <%: Html.LabelFor(m => m.OutputMisappliedNames)%> 
                        </td>
                    </tr>        
                     <tr>
                        <td>
                            <%: Html.CheckBoxFor(m => m.OutputAuthorForSynonyms)%>                            
                            <%: Html.LabelFor(m => m.OutputAuthorForSynonyms)%>

                        </td>
                    </tr>             
                </table>

            </div>
        </fieldset>
        
        <fieldset>
            <p>
                <button id="btnGetExcelFile" type="submit" class="ap-ui-button"><%: Model.Labels.GetExcelFile %></button>
            </p>
            <p style="line-height: 26px; float: left"><%: Model.Labels.WarningText %></p>
            
        </fieldset>

<!-- Full container end -->
    </div>    
            
<% } %>
    
    <script type="text/javascript">

        var fileDownloadCheckTimer;
        function blockUIForDownload() {
            var token = new Date().getTime(); //use the current timestamp as the token value
            $('#downloadTokenValue').val(token);
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Resources.DyntaxaResource.SharedGeneratingExcelFile %></h1>' });            
            fileDownloadCheckTimer = window.setInterval(function () {
                var cookieValue = $.cookie('fileDownloadToken');
                if (cookieValue == token)
                    finishDownload();
            }, 500);
        }

        function finishDownload() {
            window.clearInterval(fileDownloadCheckTimer);
            $.cookie('fileDownloadToken', null); //clears this cookie value
            $.unblockUI();
        }
        
        $(document).ready(function () {
            $('#filterTaxonCategories').dropdownchecklist({ firstItemChecksAll: true, emptyText: '<%: Resources.DyntaxaResource.ExportSharedSelectCategories %>', width: 700, maxDropHeight: 250, icon: {} });
            $('#outputTaxonCategories').dropdownchecklist({ firstItemChecksAll: true, emptyText: '<%: Resources.DyntaxaResource.ExportSharedSelectCategories %>', width: 700, maxDropHeight: 250, icon: {} });
            $('#outputTaxonNames').dropdownchecklist({ firstItemChecksAll: true, emptyText: '<%: Resources.DyntaxaResource.ExportSharedSelectNameCategories %>', width: 700, maxDropHeight: 250, icon: {} });
            $('#filterSwedishOccurrence').dropdownchecklist({ firstItemChecksAll: true, emptyText: '<%: Resources.DyntaxaResource.ExportSharedSelectSwedishOccurrence %>', width: 700, maxDropHeight: 250, icon: {} });
            $('#filterSwedishHistory').dropdownchecklist({ firstItemChecksAll: true, emptyText: '<%: Resources.DyntaxaResource.ExportSharedSelectSwedishImmigrationHistory %>', width: 700, maxDropHeight: 250, icon: {} });

            $("#exportForm").submit(function () {
                blockUIForDownload();
            });

            //            $('filterTaxonCategories').removeClass('ui-widget'); 



            // animate visibility for fieldsets
            //initToggleFieldsetH2();

            $('a.toggleCheckbox').toggle(
                function () {
                    $(this).closest("table").find(":checkbox").attr('checked', true);
                },
                function () {
                    $(this).closest("table").find(":checkbox").attr('checked', false);
                }
            );
        });        
        
    </script>

        
</asp:Content>
