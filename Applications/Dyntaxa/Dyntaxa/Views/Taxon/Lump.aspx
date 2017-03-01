<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonLumpViewModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%:Model.Labels.TaxonLumpHeaderLabel%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h1 class="readHeader"><%:Model.Labels.TaxonLumpHeaderLabel%></h1>

<% if (ViewBag.Taxon != null)
    { %> 
    <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
<% }
    else
    { %>
    <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
<% } %>

<% Html.EnableClientValidation(); %> 
    
<% using (Html.BeginForm("Lump", "Taxon", FormMethod.Post, new { @id = "lumpTaxonForm", @name = "lumpTaxonForm" }))
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
    <%: Html.HiddenFor(model => model.IsSelectedTaxonAlreadyInLumpList)%>
    <%: Html.HiddenFor(model => model.IsSelectedTaxonSetAsReplacingTaxon)%>
    <%: Html.HiddenFor(model => model.IsReplacingTaxonSet)%>
    <%: Html.HiddenFor(model => model.IsAnyLumpTaxonSet)%>
    <%: Html.HiddenFor(model => model.IsSelectedTaxonChildless)%>
    <%: Html.HiddenFor(model => model.IsOkToLump) %>


    <div id="fullContainer">
        <fieldset>
            <h2 class="open">
                <%:Model.Labels.TaxonLumpReplacingTaxonLabel%></h2>
            <div class="fieldsetContent">
                <% if (Model.IsReplacingTaxonSet)
                   {%>
                       <div class="editor-field" id="lumpToTaxon">
                            <select id="selectedTaxonList" size="2" name="ParentTaxon">
                            <% foreach (var item in Model.ReplacingTaxon)
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
                        <%: Model.Labels.TaxonLumpNoReplacingTaxonErrorLabel %>
                        </p>
                    <%}%>                                             
                    <p>
                        <% if (!(Model.IsSelectedTaxonAlreadyInLumpList || Model.IsSelectedTaxonSetAsReplacingTaxon || Model.IsReplacingTaxonSet))
                        { %>
                            <input type="submit" id="<%:Model.Labels.SetCurrentTaxon%>" name="submitButton" class="ap-ui-button" value="<%:Model.Labels.TaxonLumpSetCurrentTaxonToReplacingTaxonButtonLabel %>" />
                        <% }
                        else
                        { %>                
                            <input type="submit" class="ap-ui-button-disabled" id="Submit3" name="submitButtonRep"  value="<%:Model.Labels.TaxonLumpSetCurrentTaxonToReplacingTaxonButtonLabel %>" disabled="disabled" />
                        <% }%>
                        
                        <% if ((Model.ReplacingTaxon != null && Model.ReplacingTaxon.Count > 0))
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
            <h2 class="open"><%:Model.Labels.TaxonLumpListTaxaToLumpLabel%></h2>
            <div class="fieldsetContent">               
                <% if (Model.IsAnyLumpTaxonSet)
                {%>
                <div class="editor-field" id="lumpFromTaxon">
                    <select id="SelectedTaxa"  name="SelectedTaxa" multiple="multiple" size="5">
                    <% foreach (TaxonParentViewModelHelper item in Model.LumpTaxonList)
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
                        <p><%: Model.Labels.TaxonLumpNoTaxaSelectedErrorLabel %>
                        </p>
                    <%}%> 
                     <p>
                        <% if (Model.IsSelectedTaxonChildless && !Model.IsSelectedTaxonSetAsReplacingTaxon)
                            { %>
                                <input type="submit" id="<%:Model.Labels.AddCurrentTaxonToList%>" name="submitButton" class="ap-ui-button" value="<%:Model.Labels.TaxonLumpAddCurrentTaxonToListButtonLabel %>" />
                               
                            <% }
                        else
                        { %>                
                            <input type="submit" class="ap-ui-button-disabled" id="finalizeButton" name="submitButton3"  value="<%:Model.Labels.TaxonLumpAddCurrentTaxonToListButtonLabel %>" disabled="disabled" />
                        <% }%>
                            <% if ((Model.LumpTaxonList != null && Model.LumpTaxonList.Count > 0))
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
        <%-- <fieldset>
            <h2 class="open"><%: Model.Labels.SharedReferencesHeaderLabel%></h2> 
        </fieldset>    --%>
        <fieldset>
        <p>
              <% if (Model.LumpTaxonList != null && Model.LumpTaxonList.Count > 0 && Model.IsReplacingTaxonSet && Model.IsOkToLump)
              { %>
                   <input type="submit"  id="<%:Model.Labels.GetSelectedLump%>" name="submitButton" class="ap-ui-button" value="<%:Model.Labels.TaxonLumpButtonLabel %>" />
            <% }
            else
            { %>                
                <input type="submit" class="ap-ui-button-disabled" id="Submit1" name="submitButtonInvalid"  value="<%:Model.Labels.TaxonLumpButtonLabel %>" disabled="disabled" />
            <% }%>
             <%: Html.ActionLink(Model.Labels.ResetButtonLabel, "LumpReset", new { taxonId = Model.TaxonId }, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>  
        </p>
        </fieldset>
     </div>       
<% } %>


<script language="javascript" type="text/javascript">
    
    function storeSelectedTaxa() {
        // get selected data from list
       var selectedData = [];
       $('#lumpTaxon :selected').each(function(i, selected){ 

         selectedData[i] = $(selected).val(); 

        });
        //for each element in the array we will add to the list for posting it to the server.
        for (var i in selectedData) {                                
            $("#SelectedTaxa").append(new Option('', selectedData[i]));                 
        }
	}

    $(document).ready(function () {
   
        <% if (!Model.IsLumpTaxonCategoryValid && !Model.IsReloaded)
           { %>  
                showInfoDialog("<%:Model.Labels.TaxonLumpPopUpText%>", "<%:Model.Labels.TaxonLumpTaxonCategoryErrorText + Model.TaxonErrorName + "."%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
        <% } %>
        <% else if (!Model.IsReplacingTaxonValid && !Model.IsReloaded)
           { %>  
                showInfoDialog("<%:Model.Labels.TaxonLumpPopUpText%>", "<%:Model.Labels.TaxonLumpReplacingTaxonErrorText + Model.TaxonErrorName + "."%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
        <% } %>
        <% else if (!Model.IsLumpTaxonValid && !Model.IsReloaded)
           { %>  
                showInfoDialog("<%:Model.Labels.TaxonLumpPopUpText%>", "<%:Model.Labels.TaxonLumpTaxonErrorText + Model.TaxonErrorName + "."%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
        <% } %>
        <%else  if (!Model.IsOkToLump && !Model.IsReloaded)
           { %>  
                showInfoDialog("<%:Model.Labels.TaxonLumpPopUpText%>", "<%:Model.Labels.TaxonNotOkToLumpErrorText%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
        <% } %>
        <% else if (Model.IsSelectedTaxonChildless == false &&  !(Model.IsSelectedTaxonSetAsReplacingTaxon || Model.IsSelectedTaxonAlreadyInLumpList))
           { %>  
                showInfoDialog("<%:Model.Labels.TaxonLumpPopUpText%>", "<%:Model.Labels.TaxonLumpHaveChildrenErrorText%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
        <% } %>
            <% else if (Model.IsSelectedTaxonSetAsReplacingTaxon && !Model.IsReloaded)
           { %>  
                showInfoDialog("<%:Model.Labels.TaxonLumpPopUpText%>", "<%:Model.Labels.TaxonLumpSetAsReplacingErrorText%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
        <% } %>
        <% else if (Model.IsSelectedTaxonAlreadyInLumpList && !Model.IsReloaded)
           { %>  
                showInfoDialog("<%:Model.Labels.TaxonLumpPopUpText%>", "<%:Model.Labels.TaxonLumpAlreadyInLumpListErrorText%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
        <% } %>
        
           // animate visibility for fieldsets
        //initToggleFieldsetH2();
        //initToggleFieldsetH3();

     
        //get the selected values so we can update the database or whatever.
        $('#'+"<%:Model.Labels.GetSelectedLump%>" ).click(function () {
            var buttonId = this.id;
            storeSelectedTaxa();
            var theform = document.lumpTaxonForm;
            theform.buttonClicked.value = buttonId;
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>' });
            theform.submit();
        });
    
        //get the selected values so we can update the database or whatever.
        $('#'+"<%:Model.Labels.SetCurrentTaxon %>").click(function () {
            var buttonId = this.id;
            storeSelectedTaxa();
            var theform = document.lumpTaxonForm;
            theform.buttonClicked.value = buttonId;
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>' }); 
            theform.submit();  
        });
    
        //get the selected values so we can update the database or whatever.
        $('#'+"<%:Model.Labels.AddCurrentTaxonToList %>").click(function () {
            var buttonId = this.id;
            storeSelectedTaxa();
            var theform = document.lumpTaxonForm;
            theform.buttonClicked.value = buttonId;
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>' });
            theform.submit();  
        });
    
        //get the selected values so we can update the database or whatever.
        $('#'+"<%:Model.Labels.RemoveSelectedTaxon %>").click(function () {
            var buttonId = this.id;
            storeSelectedTaxa();
            var theform = document.lumpTaxonForm;
            theform.buttonClicked.value = buttonId;
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>' });
            theform.submit();  
        });
        
         //get the selected values so we can update the database or whatever.
        $('#'+"<%:Model.Labels.RemoveReplacingTaxon %>").click(function () {
            var buttonId = this.id;
            storeSelectedTaxa();
            var theform = document.lumpTaxonForm;
            theform.buttonClicked.value = buttonId;
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>' });
            theform.submit();  
        });
        


    });

</script>

</asp:Content>
