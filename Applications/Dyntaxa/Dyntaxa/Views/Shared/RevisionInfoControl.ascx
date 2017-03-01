 <%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.RevisionInfoItemModelHelper>" %>
 
 <% string action = Model.EditingAction; %>
 <% string actionForm = Model.EditingAction + "RevisionForm"; %>
 <% using (Html.BeginForm(action, "Revision", FormMethod.Post, new { @id = action + actionForm, @name = action + actionForm }))
       {%>
        <% if (!this.ViewData.ModelState.IsValid)
        { %>
        <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
            <h2 class="validationSummaryHeader">
                <%:Html.ValidationSummary(false, "")%>
            </h2>
        </fieldset>
        <% } %>
  
        <fieldset>
            <%--<h2 class="<%=(Model.ShowRevisionInformation) ? "open" : "closed" %> ">--%>
            <h2 class="<%:(Model.ShowRevisionInformation)%> savestate">
                <%: Resources.DyntaxaResource.SharedRevisionEditingHeaderText + " " + Model.ScientificName%>
            </h2>
             <div class="fieldsetContent">
                 
                 <table class="revisionTable">
                     <tr>
                         <th><%: Html.LabelFor(model => model.RevisionStatus)%></th>
                         <th><%: Html.LabelFor(model => model.RevisionId)%></th>
                         <th><%: Html.LabelFor(model => model.ExpectedStartDate)%></th>
                         <th><%: Html.LabelFor(model => model.ExpectedPublishingDate)%></th>
                         <th><%: Html.LabelFor(model => model.RevisionDescription) %></th>
                     </tr>
                     <tr>
                         <td><%: Html.DisplayTextFor(model => model.RevisionStatus)%></td>
                         <td><%: Html.DisplayTextFor(model => model.RevisionId)%></td>
                         <td><%: Html.DisplayTextFor(model => model.ExpectedStartDate)%></td>
                         <td><%: Html.DisplayTextFor(model => model.ExpectedPublishingDate)%></td>
                         <td><%: Html.DisplayTextFor(model => model.RevisionDescription)%></td>
                     </tr>
                     
                     <tr>
                         <td colspan="5">
                             
                             <p><%: Html.DisplayTextFor(model => model.SelectedRevisionForEditingText)%></p>
                         
                         
                         <%: Html.Hidden("RevisionId", Model.RevisionId) %>
                         <%: Html.Hidden("TaxonId", Model.TaxonId) %>
           
                        <% if (Model.ShowRevisionEditingButton)  
                        { %>
                            <% if (Model.EnableRevisionEditingButton)  
                            { %>
                                    <% if (Model.EditingAction.Equals("Delete"))  
                                    { %>
                                         <input id="<%:Model.Labels.GetSelected %>" class="ap-ui-button" type="submit" name="submitButton" value="<%: Model.RevisionEditingButtonText%>" /> 
                                    <% }
                                    else
                                    {%>
                                        <input id="submitRevisionButton<%:Model.RevisionId%>" class="ap-ui-button" type="submit" name="submitButton" value="<%: Model.RevisionEditingButtonText%>" /> 
                                
                                  <% }%>
                            <% }
                                else
                                {%>
                                    <input class="ap-ui-button-disabled" type="submit" name="submitButton" value="<%: Model.RevisionEditingButtonText%>" disabled="disabled" />  
                                <% }%>
                        <% } %>
                        </td>
                     </tr>
                 </table>

                 <%--
                 <div class="formRow">
                    <div class="group">
                         <div class="editor-label">
                            <%: Html.LabelFor(model => model.RevisionStatus)%>
                        </div>
                        <div class="display-label">
                            <%: Html.DisplayTextFor(model => model.RevisionStatus)%>
                        </div>
                    </div>
                    <div class="group">
                         <div class="editor-label">
                            <%: Html.LabelFor(model => model.RevisionId)%>
                        </div>
                        <div class="display-label">
                            <%: Html.DisplayTextFor(model => model.RevisionId)%>
                        </div>
                    </div>
                    <div class="group">
                         <div class="editor-label">
                             <%: Html.LabelFor(model => model.ExpectedStartDate)%>
                        </div>
                        <div class="display-label">
                            <%: Html.DisplayTextFor(model => model.ExpectedStartDate)%>
                            
                        </div>
                    </div>
                     <div class="group">
                        <div class="editor-label">
                            <%: Html.LabelFor(model => model.ExpectedPublishingDate)%>
                        </div>
                        <div class="display-label">
                            <%: Html.DisplayTextFor(model => model.ExpectedPublishingDate)%>
                            
                        </div>                         
                    </div>
                </div>
                <div class="formRow">
                    <div class="group">
                        <div class="editor-label">
                            <span class="field-required"></span>
                            <%: Html.LabelFor(model => model.RevisionDescription) %>
                        </div>
                        <div class="display-label">
                            <%: Html.DisplayTextFor(model => model.RevisionDescription)%>
                    
                        </div>
                    </div>
                </div>

                 <div class="formRow">
                    <div class="group">
                        <div class="editor-label">
                            <%: Html.DisplayTextFor(model => model.SelectedRevisionForEditingText)%>
                        </div>
                    </div>
               </div>
               --%>
 
            </div>

        </fieldset>
            <% } %>

<script type="text/javascript">
     $(document).ready(function () {

      <% if (Model.EditingAction.Equals("Delete"))  
        { %>
         $('#' + "<%:Model.Labels.GetSelected %>").click(function () {
        
      
            return showDialog("<%:Model.Labels.DialogTitlePopUpText%>", "<%:Model.Labels.DialogTextPopUpText%>",
                    "<%:Model.Labels.ConfirmButtonLabel%>", "<%:Model.Labels.CancelButtonLabel%>", function () {
                        $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.RevisionWaitingLabel %></h1>' });
                        $('form').submit();    
                    }, null);
              });
        <% } 
        else
        { %>  
             $('#' + "submitRevisionButton<%:Model.RevisionId%>").click(function () {
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.RevisionWaitingLabel %></h1>' });
              $('actionForm').submit();   
               
        });
        <% } %>
  
       
   });

</script>
