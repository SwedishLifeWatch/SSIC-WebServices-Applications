<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.RevisionListViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%: Resources.DyntaxaResource.RevisionListHeaderText%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <% using (Html.BeginForm("List", "Revision", FormMethod.Post, new { @id = "listRevisionForm", @name = "listRevisionForm" }))
       {%>
           
    <% if (!this.ViewData.ModelState.IsValid)
    { %>
    <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
        <h2 class="validationSummaryHeader">
            <%:Html.ValidationSummary(false, "")%>
        </h2>
    </fieldset>
    <% } %>

    <h1 class="readHeader">
      <%:Resources.DyntaxaResource.RevisionListHeaderText %></h1>
     <% if (Model.ShowTaxonNameLabelForRevisions)
        { %>

            <%--<fieldset class="limitToTaxonFieldset">
                <h2 class="limitToCurrentTaxon"><%: Html.Partial("~/Views/Shared/RevisionSelectionCheckBoxControl.ascx", Model.RevisionSelectionItemHelper)%> <label for="RevisionSelctionStatusId<%:Model.RevisionSelectionItemHelper.RevisionSelctionStatusId %>"><%: Resources.DyntaxaResource.RevisionListLimitToCurrentTaxon %></label></h2>            
            </fieldset> --%>

            <% if (ViewBag.Taxon != null)
                { %>
                <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
            <% }
                else
                { %>
                <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
            <% } %>            
            
             
      <% } %>
      <% else
         { %>
            <fieldset class="taxonSummaryFieldset">
  
            <h1 class="taxonSummaryHeader"><strong><%: Resources.DyntaxaResource.RevisionListSelectedRevisonsAllRevisionsText%></strong></h1>

            <div class="fieldsetContent">          
            </div>
        </fieldset>
      <% } %>

  
           <div id="fullContainer">
                <input type="hidden" id="taxonId" name="taxonId" value="<%:Model.TaxonId%>" />
                 <fieldset>
                    <h2 class="open">
                    
                        <%--<input type="checkbox" id ="allid" name="ShowAllRevisionStates"  value="<%= Model.ShowAllRevisionStates.RevisionSelctionStatusId %>" <% if (Model.ShowAllRevisionStates.IsChecked) { %> checked="checked" <% } %> />--%>
                        <%//: Html.Partial("~/Views/Shared/RevisionSelectionCheckBoxControl.ascx", Model.ShowAllRevisionStates)%>    
                        <!--Revision list filters-->
                        
                        <%// TODO: Delete resources %>
                        <!--
                        <%: Resources.DyntaxaResource.RevisionListSelectedRevisionStatusText+ ": "%>
                        <strong><%: Resources.DyntaxaResource.RevisionListSelectedRevisonsAllRevisionsText%></strong>
                        -->

                        <%: Resources.DyntaxaResource.RevisionListFilterHeader %>
                        
                        
                    </h2>
                    <div class="fieldsetContent">

                        <div class="fieldsetDivMargin">
                            
                            
                            <%--<div class="checkboxToggle">
                                <a href="#toggle all" class="toggleCheckbox">Check all/none</a>
                            </div>--%>

                            <table>
                                <tr>
                                    <td>
                                        <%: Html.Partial("~/Views/Shared/RevisionSelectionCheckBoxControl.ascx", Model.RevisionSelectionItemHelper)%> 
                                    </td>
                                    <td>
                                        <label for="RevisionSelctionStatusId<%:Model.RevisionSelectionItemHelper.RevisionSelctionStatusId %>"><%: Resources.DyntaxaResource.RevisionListLimitToCurrentTaxon %></label>
                                    </td>
                                    
                                </tr>       

                                <%// TODO: maybe refactor, possible to use ordinary checkboxes %>
                                 <% foreach (RevisionStatusItemModelHelper item in Model.RevisionStatus)
                                   { %>
                                    <%: Html.Partial("~/Views/Shared/RevisionStatusCheckBoxControl.ascx", item)%>
                                <% } %>
                            
                            </table>

                            <input type="submit" class="ap-ui-button" value="<%: Resources.DyntaxaResource.RevisionListFilterButtonLabelText %>" />
                    
                        </div>

                    </div>
                </fieldset>
      
                <fieldset>
                <h2 class="open">
                    <%:  Resources.DyntaxaResource.RevisionListTitleLabelText+ " " + Model.Revisions.Count.ToString() + ")"%>
                </h2>
                <% if (Model.Revisions.Count > 0)
                    { %>
                    <%  var grid = new WebGrid(source: Model.Revisions,
                                               defaultSort: "StartDate",
                                               rowsPerPage: 100);

                        //Set default sort direction.
                        if (string.IsNullOrEmpty(Request.QueryString["sortdir"]))
                        {
                            grid.SortDirection = System.Web.Helpers.SortDirection.Descending;
                        }
                    %>
                    
                    <div class="fieldsetContent">
                        <div id="grid" >
                          <% if (Model.IsViewReadonly)
                             { %>
                                 <%: grid.GetHtml(tableStyle: "grid", headerStyle: "head",alternatingRowStyle: "alt",
                                           columns: grid.Columns(grid.Column("RevisionId", "Id"),
                                                                 grid.Column("TaxonScentificRecomendedName", "Taxon"),
                                                                 grid.Column("RevisionStatus", Resources.DyntaxaResource.RevisionListRevisionStatusTableHeaderText),
                                                                 grid.Column("StartDate", Resources.DyntaxaResource.RevisionListStartDateTableHeaderText),
                                                                 grid.Column("PublishingDate", Resources.DyntaxaResource.RevisionListPublishingDateTableHeaderText),
                                                                 grid.Column("IsRevisionEditable", Resources.DyntaxaResource.RevisionListInfoButton,
                                                                                                                                                (m => m.IsRevisionEditable.Equals(false) ? Html.ActionLink(Resources.DyntaxaResource.RevisionListInfoButton, "Info", new { revisionInfoId = m.RevisionId })
                                                                                                                      : null)),                                                                                                                                                                                                                                                                                                         
                                                                 grid.Column("Revision", Resources.DyntaxaResource.SharedReferences,format: (item) =>
                                                                 {
                                                                     var revisionItem = ((RevisionItemModel)item.Value);
                                                                     return Html.RenderInfoReferenceLink(revisionItem.GUID, "List", "Revision", new { revisionId = revisionItem.RevisionId });
                                                                 })                                       
                                                                )
                                                    )
                                %>
                           <% }
                             else
                             {%>
                                  <%:grid.GetHtml(tableStyle: "grid", headerStyle: "head",alternatingRowStyle: "alt",
                                           columns: grid.Columns(grid.Column("RevisionId", "Id"),
                                                                 grid.Column("TaxonScentificRecomendedName", "Taxon"),
                                                                 grid.Column("RevisionStatus", Resources.DyntaxaResource.RevisionListRevisionStatusTableHeaderText),
                                                                 grid.Column("StartDate", Resources.DyntaxaResource.RevisionListStartDateTableHeaderText),
                                                                 grid.Column("PublishingDate", Resources.DyntaxaResource.RevisionListPublishingDateTableHeaderText),
                                                                 grid.Column("IsRevisionEditable", Resources.DyntaxaResource.RevisionListInfoButton + "/" + Resources.DyntaxaResource.RevisionListEditButton,
                                                                                                               (m => m.IsRevisionEditable.Equals(true) ? Html.ActionLink(Resources.DyntaxaResource.RevisionListEditButton, "Edit", new { revisionId = m.RevisionId })
                                                                                                                                                       : Html.ActionLink(Resources.DyntaxaResource.RevisionListInfoButton, "Info", new { revisionInfoId = m.RevisionId }))),
                                                                 grid.Column("IsRevisionPossibleToStart", Resources.DyntaxaResource.RevisionStartMainHeaderText,
                                                                                                                (m => m.IsRevisionPossibleToStart.Equals(true) ? Html.ActionLink(Resources.DyntaxaResource.RevisionStartMainHeaderText, "StartEditingRevision", new { revisionId = m.RevisionId })
                                                                                                                              : Html.SpanTag(Resources.DyntaxaResource.RevisionStartMainHeaderText, new { @class = "ap-ui-text-disabled" }))),
                                                                 grid.Column("IsRevisionPossibleToStop", Resources.DyntaxaResource.RevisionStopMainHeaderText,
                                                                                                               (m => m.IsRevisionPossibleToStop.Equals(true) ? Html.ActionLink(Resources.DyntaxaResource.RevisionStopMainHeaderText, "StopEditingRevision", new { revisionId = m.RevisionId })
                                                                                                                            : Html.SpanTag(Resources.DyntaxaResource.RevisionStopMainHeaderText, new { @class = "ap-ui-text-disabled" }))),
                                                                 grid.Column("IsRevisionPossibleToDelete", Resources.DyntaxaResource.SharedDeleteButtonText,
                                                                                                                (m => m.IsRevisionPossibleToDelete.Equals(true) ? Html.ActionLink(Resources.DyntaxaResource.SharedDeleteButtonText, "Delete", new { revisionId = m.RevisionId })
                                                                                                                               : Html.SpanTag(Resources.DyntaxaResource.SharedDeleteButtonText, new { @class = "ap-ui-text-disabled" }))),
                                                               
                                                                 grid.Column("Revision", Resources.DyntaxaResource.SharedReferences, 
                                                               
                                                                                                            format: (item) =>
                                                                                                                 {
                                                                                                                     var revisionItem = ((RevisionItemModel)item.Value);
                                                                                                                     if(revisionItem.IsRevisionEditable)
                                                                                                                        return Html.RenderAddReferenceImageLink(revisionItem.GUID, "List", "Revision", new { revisionId = revisionItem.RevisionId });
                                                                                                                     else
                                                                                                                        return Html.RenderInfoReferenceLink(revisionItem.GUID, "List", "Revision", new { revisionId = revisionItem.RevisionId });
                                                                                                                 }                                                                         
                                                                                                     )
                                                               
                                                                
                                                                
                                                                                                                                                   
                                                               
                                                                )
                                                   )
                               %>
                           <%}%>
                          
                        </div>
                    </div>
                <% } %>
                <% else
                   { %>
                    <br/>
                    <strong><%: Resources.DyntaxaResource.RevisionListNoRevisionsAvailableText%></strong>
                    <br/>
                <% } %>
            </fieldset>          

        </div>

        
    <% } %>
    <script type="text/javascript">
        
        // Defaults
        $(function () {
//            var subkey = '<%=ViewBag.CookieName%>';
//            restoreH2States(subkey);            
            //initToggleFieldsetH2();
                    
        });

        $(window).unload(function () {
//            var subkey = '<%=ViewBag.CookieName%>';
//            saveH2States(subkey);
        });
 $(document).ready(function () {
     $("#listRevisionForm").submit(function () {
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Resources.DyntaxaResource.SharedLoading %></h1>'
            });
        });
    });                        
    </script>


</asp:Content>


