<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonSplitViewModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%:Model.Labels.TaxonSplitHeaderLabel%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h1 class="readHeader"><%:Model.Labels.TaxonSplitHeaderLabel%></h1>

<% if (ViewBag.Taxon != null)
    { %>
    <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
<% }
    else 
    { %>
    <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
<% } %>


<% Html.EnableClientValidation(); %> 
    
<% using (Html.BeginForm("Split", "Taxon", FormMethod.Post, new { @id = "splitTaxonForm", @name = "splitTaxonForm" }))
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
    <%: Html.HiddenFor(model => model.ReplacingTaxonId) %>
    <%: Html.HiddenFor(model => model.RevisionId) %>
    <%: Html.Hidden("buttonClicked") %>

    <div id="fullContainer">
            <fieldset>
            <h2 class="open">
                <%:Model.Labels.TaxonSplitTaxonToBeSplittedLabel%></h2>
            <div class="fieldsetContent">
                <% if (Model.IsSplitTaxonSet)
                   {%>
                       <div class="editor-field" id="splitTaxonTo">
                            <select id="selectedTaxonList" size="2" name="ParentTaxon">
                            <% foreach (var item in Model.SplitTaxon)
                            {%>
                                <option value="<%=item.TaxonId%>"><%:item.Category%>:  
                                <%= item.ScientificName != null ? Html.RenderScientificName(item.ScientificName, null, item.SortOrder).ToString() : "-"%>
                                <%= item.CommonName != null ? Html.Encode(string.Format(" - {0}", item.CommonName)) : ""%></option>
                            <% } %>
                            </select>
                        </div>
                    <%}
                    else
                    {%>
                        <p>
                        <%: Model.Labels.TaxonSplitNoTaxonSelectedErrorLabel %>
                        </p>
                    <%}%>                                             
                    <p>
                        <% if ((!(Model.IsSelectedTaxonAlreadyInReplacingList || Model.IsSelectedTaxonSetAsSplitTaxon || Model.IsSplitTaxonSet)) && Model.IsSelectedTaxonChildless )
                        { %>
                            <input type="submit" id="<%:Model.Labels.SetCurrentTaxon%>" name="submitButton" class="ap-ui-button" value="<%:Model.Labels.TaxonSplitSetCurrentTaxonToReplacingTaxonButtonLabel %>" />
                        <% }
                        else
                        { %>                
                            <input type="submit" class="ap-ui-button-disabled" id="Submit3" name="submitButtonRep"  value="<%:Model.Labels.TaxonSplitSetCurrentTaxonToReplacingTaxonButtonLabel %>" disabled="disabled" />
                        <% }%>
                        
                        <% if ((Model.SplitTaxon != null && Model.SplitTaxon.Count > 0))
                            { %>
                                <input type="submit"  id="<%:Model.Labels.RemoveReplacingTaxon %>"  name="submitButton" class="ap-ui-button" value="<%:Model.Labels.DeleteButtonLabel %>" />
                            <% }
                        else
                        { %>                
                            <input type="submit" class="ap-ui-button-disabled" id="Submit5" name="submitButtonRemove"  value="<%:Model.Labels.DeleteButtonLabel %>" disabled="disabled" />
                        <% }%>
                   </p>
            </div>
        </fieldset>   
         <fieldset>
            <h2 class="open"><%:Model.Labels.TaxonSplitReplacingTaxonLabel%></h2>
            <div class="fieldsetContent">               
                <% if (Model.IsAnyReplacingTaxonSet)
                {%>
                <div class="editor-field" id="splitTaxonFrom">
                    <select id="SelectedTaxa"  name="SelectedTaxa" multiple="multiple" size="5">
                    <% foreach (TaxonParentViewModelHelper item in Model.ReplacingTaxonList)
                    { %>
                        <option value="<%: item.TaxonId %>">
                        <%: item.Category %>:
                        <%= item.ScientificName != null ? Html.RenderScientificName(item.ScientificName, null, item.SortOrder).ToString() : "-"%>
                        <%= item.CommonName != null ? Html.Encode(string.Format(" - {0}", item.CommonName)) : ""%>  
                        </option>
                    <% } %>
                    </select>
                </div> 
                <%}
                    else
                    {%>
                        <p><%: Model.Labels.TaxonSplitNoReplacingTaxaErrorLabel %>
                        </p>
                    <%}%> 
                     <p>
                        <% if (!Model.IsSelectedTaxonSetAsSplitTaxon)
                            { %>
                                <input type="submit" id="<%:Model.Labels.AddCurrentTaxonToList%>" name="submitButton" class="ap-ui-button" value="<%:Model.Labels.TaxonSplitAddCurrentTaxonToListButtonLabel %>" />
                               
                            <% }
                        else
                        { %>                
                            <input type="submit" class="ap-ui-button-disabled" id="finalizeButton" name="submitButton3"  value="<%:Model.Labels.TaxonSplitAddCurrentTaxonToListButtonLabel %>" disabled="disabled" />
                        <% }%>
                            <% if ((Model.ReplacingTaxonList != null && Model.ReplacingTaxonList.Count > 0))
                            { %>
                                <input type="submit"  id="<%:Model.Labels.RemoveSelectedTaxon%>"  name="submitButton" class="ap-ui-button" value="<%:Model.Labels.DeleteButtonLabel %>" />
                            <% }
                        else
                        { %>                
                            <input type="submit" class="ap-ui-button-disabled" id="Submit2" name="submitButtonRemove"  value="<%:Model.Labels.DeleteButtonLabel %>" disabled="disabled" />
                        <% }%>
                    </p>
             </div>              
        </fieldset>
               
       
       <%--  <fieldset>
            <h2 class="open"><%: Model.Labels.SharedReferencesHeaderLabel%></h2> 
        </fieldset>    --%>
        <fieldset>
        <p>
              <% if (Model.ReplacingTaxonList != null && Model.ReplacingTaxonList.Count > 0 && Model.IsSplitTaxonSet)
              { %>
                   <input type="submit"  id="<%:Model.Labels.GetSelectedSplit%>" name="submitButton" class="ap-ui-button" value="<%:Model.Labels.TaxonSplitButtonLabel %>" />
            <% }
            else
            { %>                
                <input type="submit" class="ap-ui-button-disabled" id="Submit1" name="submitButtonInvalid"  value="<%:Model.Labels.TaxonSplitButtonLabel %>" disabled="disabled" />
            <% }%>
             <%: Html.ActionLink(Model.Labels.ResetButtonLabel, "SplitReset", new { taxonId = Model.TaxonId }, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>  
        </p>
        </fieldset>
     </div>       
<% } %>


<script language="javascript" type="text/javascript">
    
    function storeSelectedTaxa() {
        // get selected data from list
       var selectedData = [];
       $('#splitTaxon :selected').each(function(i, selected){ 

         selectedData[i] = $(selected).val(); 

        });
        //for each element in the array we will add to the list for posting it to the server.
        for (var i in selectedData) {                                
            $("#SelectedTaxa").append(new Option('', selectedData[i]));                 
        }
	}

    $(document).ready(function () {
   
          <% if (!Model.IsReplacingTaxonCategoryValid && !Model.IsReloaded)
           { %>  
                showInfoDialog("<%:Model.Labels.TaxonSplitPopUpText%>", "<%:Model.Labels.TaxonSplitTaxonCategoryErrorText + Model.TaxonErrorName + "."%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
        <% } %>
        <% else if (!Model.IsReplacingTaxonValid && !Model.IsReloaded)
           { %>  
                showInfoDialog("<%:Model.Labels.TaxonSplitPopUpText%>", "<%:Model.Labels.TaxonSplitTaxonErrorText + Model.TaxonErrorName + "."%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
        <% } %>
        <% else if (!Model.IsSplitTaxonValid && !Model.IsReloaded)
           { %>  
                showInfoDialog("<%:Model.Labels.TaxonSplitPopUpText%>", "<%:Model.Labels.TaxonSplitReplacingTaxonErrorText + Model.TaxonErrorName + "."%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
        <% } %>
        <%else  if (!Model.IsOkToSplit && !Model.IsReloaded)
           { %>  
                showInfoDialog("<%:Model.Labels.TaxonSplitPopUpText%>", "<%:Model.Labels.TaxonNotOkToSplitErrorText%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
        <% } %>
        <% else  if (!Model.IsSelectedTaxonChildless && !(Model.IsSelectedTaxonSetAsSplitTaxon || Model.IsSelectedTaxonAlreadyInReplacingList))
           { %>  
                showInfoDialog("<%:Model.Labels.TaxonSplitPopUpText%>", "<%:Model.Labels.TaxonSplitHaveChildrenErrorText%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
        <% } %>
            <% else if (Model.IsSelectedTaxonSetAsSplitTaxon && !Model.IsReloaded)
           { %>  
                showInfoDialog("<%:Model.Labels.TaxonSplitPopUpText%>", "<%:Model.Labels.TaxonSplitAlreadyInSetAsSplittingErrorLabel%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
        <% } %>
        <% else if (Model.IsSelectedTaxonAlreadyInReplacingList && !Model.IsReloaded)
           { %>  
                showInfoDialog("<%:Model.Labels.TaxonSplitPopUpText%>", "<%:Model.Labels.TaxonSplitAlreadyInReplaceListErrorLabel%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
        <% } %>
        
           // animate visibility for fieldsets
        //initToggleFieldsetH2();
        //initToggleFieldsetH3();
     
        //get the selected values so we can update the database or whatever.
        $('#'+"<%:Model.Labels.GetSelectedSplit%>" ).click(function () {
            var buttonId = this.id;
            storeSelectedTaxa();
            var theform = document.splitTaxonForm;
            theform.buttonClicked.value = buttonId;
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>' });
            theform.submit();
        });
    
        //get the selected values so we can update the database or whatever.
        $('#'+"<%:Model.Labels.SetCurrentTaxon %>").click(function () {
            var buttonId = this.id;
            storeSelectedTaxa();
            var theform = document.splitTaxonForm;
            theform.buttonClicked.value = buttonId;
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>' });
            theform.submit();  
        });
    
        //get the selected values so we can update the database or whatever.
        $('#'+"<%:Model.Labels.AddCurrentTaxonToList %>").click(function () {
            var buttonId = this.id;
            storeSelectedTaxa();
            var theform = document.splitTaxonForm;
            theform.buttonClicked.value = buttonId;
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>' }); 
            theform.submit();  
        });
    
        //get the selected values so we can update the database or whatever.
        $('#'+"<%:Model.Labels.RemoveSelectedTaxon %>").click(function () {
            var buttonId = this.id;
            storeSelectedTaxa();
            var theform = document.splitTaxonForm;
            theform.buttonClicked.value = buttonId;
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>' }); 
            theform.submit();  
        });
        
         //get the selected values so we can update the database or whatever.
        $('#'+"<%:Model.Labels.RemoveReplacingTaxon %>").click(function () {
            var buttonId = this.id;
            storeSelectedTaxa();
            var theform = document.splitTaxonForm;
            theform.buttonClicked.value = buttonId;
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>' });
            theform.submit();  
        });

    });

</script>

</asp:Content>
