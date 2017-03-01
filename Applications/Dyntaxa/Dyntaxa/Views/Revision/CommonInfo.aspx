<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.RevisionCommonInfoViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.RevisionEditingActionHeaderText%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <% string action = Model.EditingAction; %>
    <% string actionForm = Model.EditingAction + "RevisionForm"; %>
    <% using (Html.BeginForm(action, "Revision", FormMethod.Post, new { @id = actionForm, @name = actionForm }))
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
        <%: Model.RevisionEditingHeaderText%>
    </h1>

    <!-- Full container start -->
    <div id="fullContainer">
       <%: Html.HiddenFor(model => model.Submit)%>
       <fieldset> 
        <% if (Model.RevisionInfoItems != null && Model.RevisionInfoItems.Count > 0 )
           {
               RevisionInfoItemModelHelper infoItem = Model.RevisionInfoItems.First(); %>
            <h2 class="<%:(infoItem.ShowRevisionInformation)%> savestate">
                <%: Resources.DyntaxaResource.SharedRevisionEditingHeaderText + " " + infoItem.ScientificName%>
            </h2>
             <div class="fieldsetContent">
                 
                 <table class="revisionTable">
                     <tr>
                         <th><%: Html.LabelFor(model => infoItem.RevisionStatus)%></th>
                         <th><%: Html.LabelFor(model => infoItem.RevisionId)%></th>
                         <th><%: Html.LabelFor(model => infoItem.ExpectedStartDate)%></th>
                         <th><%: Html.LabelFor(model => infoItem.ExpectedPublishingDate)%></th>
                         <th><%: Html.LabelFor(model => infoItem.RevisionDescription) %></th>
                     </tr>
                     <tr>
                         <td><%: Html.DisplayTextFor(model => infoItem.RevisionStatus)%></td>
                         <td><%: Html.DisplayTextFor(model => infoItem.RevisionId)%></td>
                         <td><%: Html.DisplayTextFor(model => infoItem.ExpectedStartDate)%></td>
                         <td><%: Html.DisplayTextFor(model => infoItem.ExpectedPublishingDate)%></td>
                         <td><%: Html.DisplayTextFor(model => infoItem.RevisionDescription)%></td>
                     </tr>
                     
                     <tr>
                         <td colspan="5">
                             
                             <p><%: Html.DisplayTextFor(model => infoItem.SelectedRevisionForEditingText)%></p>
                         
                         
                         <%: Html.Hidden("RevisionId", Model.RevisionId) %>
                         <%: Html.Hidden("TaxonId", Model.TaxonId) %>
           
                        <% if (infoItem.ShowRevisionEditingButton)  
                        { %>
                            <% if (infoItem.EnableRevisionEditingButton)  
                            { %>
                                    <% if (Model.EditingAction.Equals("Delete"))  
                                    { %>
                                         <input id="<%:infoItem.Labels.GetSelected %>" class="ap-ui-button" type="submit" name="submitButton" value="<%: infoItem.RevisionEditingButtonText%>" /> 
                                    <% }
                                    else
                                    {%>
                                        <input id="submitRevisionButton<%:infoItem.RevisionId%>" class="ap-ui-button" type="submit" name="submitButton" value="<%: infoItem.RevisionEditingButtonText%>" /> 
                                
                                  <% }%>
                            <% }
                                else
                                {%>
                                    <input class="ap-ui-button-disabled" type="submit" name="submitButton" value="<%: infoItem.RevisionEditingButtonText%>" disabled="disabled" />  
                                <% }%>
                        <% } %>
                        </td>
                     </tr>
                 </table>
        
           
            <% } %>
            </div>
            </fieldset>
            <% } %>
        <% if (!Model.Submit)
           { %>
        <fieldset>
          <div class="fieldsetDivMargin">
                 
                  <%: Html.ActionLink(Model.Labels.ReturnToListText, "List", "Revision", new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>     
                    
          </div>
        </fieldset>
        <% } %>   
         <!-- full container end -->
    </div>



    
       <!-- This code will be executed in the partial view RevisionInfoControl.ascx. Addressed when document has been loaded. -->
       <script type="text/javascript">
           $(document).ready(function () {

                <% if (!(Model.RevisionInfoItems != null && Model.RevisionInfoItems.Count > 0))
                { %>  
                    showInfoDialog("<%:Model.DialogTitlePopUpText%>", "<%:Model.DialogTextPopUpText%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
               <% } %>
               // animate visibility for fieldsets
               
               //initToggleFieldsetH2();
               
               <% if (Model.Submit)
                { %>  
                    var formName = '#<%= Model.EditingAction + "RevisionForm" %>';
                     $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /><%: " " + Model.RevisionEditingHeaderText + Model.Labels.RevisionIdText +  Model.RevisionId %></h1>' });
                    $(formName).submit();
               <% } %>
                   
               
                
           });
           

        </script>

</asp:Content>
