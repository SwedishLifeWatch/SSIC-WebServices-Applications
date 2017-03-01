<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.RevisionEventViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.Labels.PageTitle %>
   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <% using (Html.BeginForm("ListEvent", "Revision", FormMethod.Post, new { @id = "listEventForm", @name = "listEventForm" }))       
    {%>
     <% if (!this.ViewData.ModelState.IsValid)
        { %>
        <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
            <h2 class="validationSummaryHeader">
                <%:Html.ValidationSummary(false, "")%>
            </h2>
        </fieldset>
     <% } %>

     <%: Html.Partial("~/Views/Revision/RevisionTaxonInfo.ascx", Model.RevisionTaxonInfoViewModel)%>
    
        
     <% if (ViewBag.Taxon != null)
        { %>
            <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
     <% }
        else
        { %>
            <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
     <% } %>        
        
        <div id="fullContainer">
            <input type="hidden" id="taxonId" name="taxonId" value="<%:Model.TaxonId%>" />
            <input type="hidden" id="revisionId" name="revisionId" value="<%:Model.RevisionId%>" />
            <input type="hidden" id="revisionEventId" name="revisionEventId" value="<%:Model.RevisionEventId%>" />
                
            <fieldset>
                <h2 class="open"><%: Model.Labels.EventListLabel%></h2>
                <p>
                 <% if (Model.ExistEvents)
                    { %>               
                        <input type="submit" id="<%:Model.Labels.GetUndo %>" name="submitButton" class="ap-ui-button" style="margin: 15px 10px 15px 5px;" value="<%:Model.Labels.UndoText %>" />
                        <span style="float: left; margin: 18px 10px 15px 5px;"><%: string.Format("({0} {1})", Model.Labels.EventListCountLabel, Model.RevisionEventItems.Count) %></span>
                  <%}
                    else 
                    { %>               
                        <input type="submit" id="submitButton3" name="submitButton3" class="ap-ui-button-disabled" style="margin: 15px 10px 15px 5px;" value="<%:Model.Labels.UndoText %>" disabled="disabled" />
                 <% } %> 
                </p>
             <% if (Model.RevisionEventItems != null  && Model.RevisionEventItems.Count > 0)
                { %>
                    <% var grid = new WebGrid(source: Model.RevisionEventItems,
                                        defaultSort: "RevisionEventIndex",
                                        rowsPerPage: 100);
                        
                    //Set default sort direction.
                    if (string.IsNullOrEmpty(Request.QueryString["sortdir"]))
                    {
                        grid.SortDirection = System.Web.Helpers.SortDirection.Descending;
                    }%>
                   
                    <div class="fieldsetContent" style="clear: both;">
                        <div id="grid" >
                            <%: grid.GetHtml(tableStyle: "grid", headerStyle: "head",alternatingRowStyle: "alt",
                                    columns: grid.Columns(
                                        grid.Column("RevisionEventIndex", Model.Labels.StepText), 
                                        grid.Column("ChangeEventType", Model.Labels.ChangeTypeText),
                                        grid.Column("AffectedTaxa", Model.Labels.AffectedTaxaText),
                                        grid.Column("NewValue", Model.Labels.NewValueText),
                                        grid.Column("FormerValue", Model.Labels.FormerValueText)
                                    )
                                )%>
                        </div>
                    </div>
             <% } %>
             <% else
                { %>
                    <br/>
                    <strong><%: Model.Labels.RevisionListNoRevisionEventsAvailableLabel%></strong>
                    <br/>
             <% } %>
            </fieldset>
        </div>

        
    <% } %>
    <script type="text/javascript">

       $(document).ready(function () {
   
               
        $('#'+"<%:Model.Labels.GetUndo %>" ).click(function () {

            return showDialog("<%:Model.Labels.PopUpTitle%>", "<%:Model.Labels.UndoTextPopUp%>",
                               "<%:Model.Labels.ConfirmText%>", "<%:Model.Labels.CancelText%>", function () {
                                   $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Resources.DyntaxaResource.SharedDeleting %></h1>' });
            $('#listEventForm').submit();                 
                } , null);
               
        });

    });

//        $('fieldset h2.open, fieldset h2.close').click(function () {
//            $(this)
//            .toggleClass("closed")
//            .toggleClass("open")
//            .closest("fieldset").find("div.fieldsetContent").slideToggle('fast', function () {

//            });
//        });

        $(function () {
            $('a.toggleCheckbox').toggle(
                function () {
                    $(this).closest("div.fieldsetContent").find(":checkbox").attr('checked', '');                    
                },
                function () {
                    $(this).closest("div.fieldsetContent").find(":checkbox").attr('checked', 'checked');                    
                }
            );
        });
    </script>


</asp:Content>


