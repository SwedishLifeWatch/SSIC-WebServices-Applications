<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.Match.MatchSettingsViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.Labels.TitleLabel %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="readHeader">
        <%: Model.Labels.TitleLabel %>
    </h1>      

    <% using (Html.BeginForm("Settings", "Match", FormMethod.Post, new { enctype = "multipart/form-data" })) { %>
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

        <%: Html.HiddenFor(model => model.FileName) %>
        <%: Html.HiddenFor(model => model.TaxonId) %>
        <%: Html.HiddenFor(model => model.MatchInputType)%> 
        <%: Html.HiddenFor(model => model.LimitToParentTaxonId)%>
        
<%--        <% if (Model.LimitToTaxonLabel != null) %>
        <% { %>
            <h2 id="LimitToCurrentTaxon" style="padding: 0px; margin-top:10px;">
                <%: Html.CheckBoxFor(model => model.LimitToTaxon)%> <%: Model.LimitToTaxonLabel%>
            </h2>        
        <% } %>--%>
               
        <fieldset>
            <h2 class="open"><%: Model.Labels.Input %></h2>
            <div class="fieldsetContent">
                <div id="tabs">
	                <ul>
		                <li><a href="#tabs-1"><%: Model.Labels.ClipboardLabel %></a></li>
		                <li><a href="#tabs-2"><%: Model.Labels.ExcelFileLabel %></a></li>		                
	                </ul>
                    <div id="tabs-1" style="padding:4px;">
                        <table class="display-table">
                            <tr>
                                <td>
                                    <div class="group">
                                        <%: Html.LabelFor(model => model.ClipBoard) %>
                                        <%: Html.TextAreaFor(model => model.ClipBoard, new { id = "clipBoardTextArea", @class = "" })%>                                
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
    <%--                            <td>
                                    <div class="group">
                                        <%: Html.LabelFor(model => model.ColumnDelimiter) %>
                                        <%: Html.DropDownList("ColumnDelimiter")%>
                                    </div>
                                </td>--%>
                                <%--<td>
                                    <%: Html.CheckBoxFor(model => model.AuthorNameIsPartOfTaxonName)%>
                                    <strong><%: Resources.DyntaxaResource.MatchOptionsInputAuthorNamePartOfTaxonName%></strong>
                                </td>--%>
                            </tr>      
                        </table>    
                    </div>
                    <div id="tabs-2" style="padding:4px;">
                        <table class="display-table">
                            <tr>
                                <td colspan="2">
                                    <div class="group">
                                        <%: Html.LabelFor(model => model.FileName) %>
                                        <input id="FileName" name="FileName" type="file" />
                                        <%: Html.ValidationMessageFor(model => model.FileName)%>
                                        <p class="info"><%: Resources.DyntaxaResource.MatchOptionsAllowedFiletypesText %></p>                    
                                    </div>
                                </td>                        
                            </tr>
                            <tr>
                                <td>
                                    <%: Html.CheckBoxFor(model => model.IsFirstRowColumnName)%>
                                    <%: Html.LabelFor(model => model.IsFirstRowColumnName) %>
                                </td>
                            </tr>
                        </table>                   
                    </div>
                </div>
            </div>            
        </fieldset>
    
                  <%-- <td>
                        <div class="group">
                            <%: Html.LabelFor(model => model.ColumnContentAlternative) %>
                            <%: Html.DropDownList("ColumnContentAlternative")%>
                        </div>
                    </td>--%>
        <fieldset>
            <h2 class="open savestate"><%: Model.Labels.FilterLabel %></h2>
            <div class="fieldsetContent">
              <table>
                <tr>
                    <td>
                        <%: Model.Labels.MatchToTypeLabel %> 
                    </td>
                    <td>                                                    
                        <%: Html.DropDownList("MatchToType")%>                        
                    </td>
                </tr>              
               </table>
               
               <strong style="margin-left: 12px;"><%: Model.Labels.SearchOptionsLabel %></strong> 
                <% Html.RenderPartial("SearchOptions", Model.SearchOptions, new ViewDataDictionary { TemplateInfo = new TemplateInfo { HtmlFieldPrefix = "SearchOptions" } }); %>                                                             
            </div>
         </fieldset>

        
        <fieldset>
            <h2 class="open savestate"><%: Model.Labels.OutputLabel %></h2>
            <div class="fieldsetContent">
              <table class="display-table">        
                <tr>
                    <td>
                        <%: Html.CheckBoxFor(model => model.OutputTaxonId)%>
                        <%: Html.LabelFor(model => model.OutputTaxonId)%>     
                    </td>
                </tr>
                <tr>
                    <td>
                        <%: Html.CheckBoxFor(model => model.OutputScientificName)%>
                        <%: Html.LabelFor(model => model.OutputScientificName) %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%: Html.CheckBoxFor(model => model.OutputAuthor)%>
                        <%: Html.LabelFor(model => model.OutputAuthor) %>    
                    </td>
                </tr>
                <tr>
                    <td>
                        <%: Html.CheckBoxFor(model => model.OutputCommonName)%>
                        <%: Html.LabelFor(model => model.OutputCommonName) %>   
                    </td>
                </tr>
                <tr>
                    <td>
                        <%: Html.CheckBoxFor(model => model.OutputTaxonCategory)%>
                        <%: Html.LabelFor(model => model.OutputTaxonCategory) %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%: Html.CheckBoxFor(model => model.OutputScientificSynonyms)%>
                        <%: Html.LabelFor(model => model.OutputScientificSynonyms) %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%: Html.CheckBoxFor(model => model.OutputParentTaxa)%>
                        <%: Html.LabelFor(model => model.OutputParentTaxa) %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%: Html.CheckBoxFor(model => model.OutputGUID)%>
                        <%: Html.LabelFor(model => model.OutputGUID) %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%: Html.CheckBoxFor(model => model.OutputRecommendedGUID)%>
                        <%: Html.LabelFor(model => model.OutputRecommendedGUID)%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%: Html.CheckBoxFor(model => model.OutputSwedishOccurrence)%>
                        <%: Html.LabelFor(model => model.OutputSwedishOccurrence)%>
                    </td>
                </tr>
              </table>
               
            </div>

        </fieldset>        
        <fieldset>
            <p>
                <input type="submit" class="ap-ui-button" value="<%: Resources.DyntaxaResource.MatchOptionsSubmitButtonValue %>" />                
            </p>
        </fieldset>
<!-- full container end -->
    </div>    
        
    

    
<% } %>
    
    <script type="text/javascript">

        $(document).ready(function() {
            // animate visibility for fieldsets
            //initToggleFieldsetH2();

            $("#tabs").tabs();
        });

    </script>
        
</asp:Content>