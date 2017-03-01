<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.Export.ExportGraphVizViewModel>" %>
<%@ Import Namespace="ArtDatabanken.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%:Resources.DyntaxaResource.ExportGraphvizTitle %>    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="readHeader">
        <%: Resources.DyntaxaResource.ExportGraphvizTitle %>        
    </h1>    

    <% using (Html.BeginForm("Graphviz", "Export", FormMethod.Post, new { id = "graphVizForm"}))
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
            <h2 class="open"><%:Resources.DyntaxaResource.MatchOptionsInput%></h2>
            <div class="fieldsetContent">
                <table class="display-table">
                    <tr>
                        <td>                            
                            <select id="treeIterationMode" name="TreeIterationMode">
                                <option value="<%=TaxonRelationsTreeIterationMode.BothParentsAndChildren%>" selected="selected"><%: Resources.DyntaxaResource.ExportGraphvizIncludeBothParentsAndChildren %></option>
                                <option value="<%=TaxonRelationsTreeIterationMode.OnlyChildren%>"><%: Resources.DyntaxaResource.ExportGraphvizIncludeOnlyChildren %></option>
                                <option value="<%=TaxonRelationsTreeIterationMode.OnlyParents%>"><%: Resources.DyntaxaResource.ExportGraphvizIncludeOnlyParents %></option>
                            </select>                            
                        </td>
                    </tr>                    
                    <tr>
                        <td>
                            <input type="radio" name="RelationTypeMode" id="relationTypeMode1" value="<%= TaxonRelationsTreeRelationTypeMode.BothValidAndInvalidRelations %>" checked="checked"/> <label for="relationTypeMode1" class="checkboxLabel"><%: Resources.DyntaxaResource.ExportGraphvizBothValidAndInvalidRelations %></label> <br/>
                            <input type="radio" name="RelationTypeMode" id="relationTypeMode2" value="<%= TaxonRelationsTreeRelationTypeMode.OnlyValidRelations %>"/> <label for="relationTypeMode2" class="checkboxLabel"><%: Resources.DyntaxaResource.ExportGraphvizOnlyValidRelations %></label> <br/>                            
                          <%--  <select id="relationTypeMode" name="relationTypeMode">
                                <option value="<%=TaxonRelationsTreeRelationTypeMode.BothValidAndInvalidRelations%>" selected="selected">Visa både giltiga och ogiltiga relationer</option>    
                                <option value="<%=TaxonRelationsTreeRelationTypeMode.OnlyValidRelations%>">Visa bara giltiga relationer</option>                                                                
                            </select>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="checkbox" checked="checked" id="includeLumpSplits" name="IncludeLumpSplits" value="true"/>
                            <label for="includeLumpSplits" class="checkboxLabel"><%: Resources.DyntaxaResource.ExportGraphvizIncludelumpSplits %></label>                              
                        </td>
                    </tr>
                    <%--<tr>
                        <td>
                            <input type="checkbox" checked="checked" id="includeChildrenSecondaryParents" name="IncludeChildrenSecondaryParents" value="true"/>
                            <label for="includeChildrenSecondaryParents" class="checkboxLabel">Om ett taxon lägre ner i hierarkin har sekundära föräldrar. Inkludera detta taxons sekundära föräldrar i trädet.</label>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            <input type="checkbox" id="showRelationId" name="ShowRelationId" value="true"/>                                                                                    
                            <label for="showRelationId" class="checkboxLabel"><%: Resources.DyntaxaResource.ExportGraphvizShowRelationId %></label>                             
                        </td>
                    </tr>
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
        <%--<button id="btnGenerateGraphvizFile" type="submit" class="ap-ui-button"><%: Resources.DyntaxaResource.ExportGraphvizGenerateGraphviz %></button>                       --%>
        <button id="btnGenerateGraphvizFileAjax" type="button" class="ap-ui-button"><%: Resources.DyntaxaResource.ExportGraphvizGenerateGraphviz %></button>
                
    <% } %>    
                       
        <div style="clear: both;">
            <%: Resources.DyntaxaResource.ExportGraphvizLastUpdatedTime %>: <em><%: ViewBag.TreeLastUpdatedTime.ToString() %></em> <br/>
            <a target="_blank" href="http://www.webgraphviz.com/">WebGraphviz graph viewer online</a> <br/>
            <a target="_blank" href="http://www.graphviz.org/">Graphviz standalone application</a>
        </div>        
        
        <%--<br/><br/><br/>
        <% if (ViewBag.ShowRefreshDyntaxaTaxonTreeButton) %>
        <% { %>
            <%:Html.ActionLink(Resources.DyntaxaResource.ExportGraphvizRefreshTreeCache , "RefreshTree", "Export")%>        
        <% } %>--%>
    <!-- full container end -->
        
        <div id="dialog-form" title="GraphViz format">
          <%--<p class="validateTips">GraphViz file content.</p> --%>
          <form>
            <fieldset>
                <textarea id="graphVizAjaxTextArea" style="width: 100%; height: 330px;">                    
                </textarea>              
            </fieldset>
          </form>
        </div>
        
        <script type="text/javascript">
            var graphVizDialog;

            $(document).ready(function () {
                $('#btnGenerateGraphvizFileAjax')
                    .click(function() {                        
                        $.ajax({
                            type: "POST",
                            url: '<%=Url.Action("GetGraphVizJson")%>',
                            data: $("#graphVizForm").serialize(), // serializes the form's elements.
                            success: function (data) {
                                console.log($('#graphVizAjaxTextArea'));
                                $('#graphVizAjaxTextArea').val(data);
                                graphVizDialog.dialog("open");
                            }
                        });
                    });


                graphVizDialog = $("#dialog-form").dialog({
                    autoOpen: false,
                    height: 500,
                    width: 700,
                    modal: true,
                    buttons: {                        
                        Ok: function () {
                            graphVizDialog.dialog("close");
                        }
                    },
                    close: function () {

                    }
                });
                
            });
        </script>
    </div>              
</asp:Content>
