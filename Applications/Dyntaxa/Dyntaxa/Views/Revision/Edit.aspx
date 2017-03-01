<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.RevisionEditViewModel>" %>

<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.Labels.RevisionEditActionHeaderText%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% IList<RevisionUserItemModelHelper> userList = Model.UserList; %>
<% IList<RevisionUserItemModelHelper> selectedUserList = Model.SelectedUserList; %>
<% var noOfReferences = Model.NoOfRevisionReferences; %>

    <%: Html.Partial("~/Views/Revision/RevisionTaxonInfo.ascx", Model.RevisionTaxonInfoViewModel)%>
    <% if (ViewBag.Taxon != null)
        { %>
        <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
    <% }
        else
        { %>
        <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
    <% } %>    
    <% Html.EnableClientValidation(); %>  
    <% using (Html.BeginForm("Edit", "Revision", FormMethod.Post, new { @id = "editRevisionForm", @name = "editRevisionForm" }))
       {%>
        
        <% if (!this.ViewData.ModelState.IsValid)
        { %>
        <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
            <h2 class="validationSummaryHeader">
                <%:Html.ValidationSummary(false, "")%>
            </h2>
        </fieldset>
        <% } %>
        
        <% Html.EnableClientValidation(); %> 
    
    <%: Html.Hidden("buttonClicked") %>
    <!-- Full container start -->
    <div id="fullContainer">
      <%: Html.HiddenFor(model => model.TaxonId) %>
     <%: Html.HiddenFor(model => model.RevisionTaxonId) %>
     <%: Html.HiddenFor(model => model.RevisionId) %>
     <%: Html.HiddenFor(model => model.RevisionStatus) %>
     <%: Html.HiddenFor(model => model.IsTaxonInRevision) %>
     <%: Html.HiddenFor(model => model.RoleId) %>    
     <%: Html.HiddenFor(model => model.ShowDeleteButton) %>
     <%: Html.HiddenFor(model => model.ShowFinalizeButton) %>
     <%: Html.HiddenFor(model => model.ShowInitalizeButton) %>
     <%: Html.HiddenFor(model => model.GUID) %>
     <%: Html.HiddenFor(model => model.NoOfRevisionReferences) %>

        <fieldset>
            <h2 class="open"><%: Model.Labels.RevisionAddPropertiesHeaderText%></h2>
            <div class="fieldsetContent">
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
                        <div class="editor-field">
                            <%: Html.EditorFor(model => model.ExpectedStartDate)%>
                            <%: Html.ValidationMessageFor(model => model.ExpectedStartDate)%>
                        </div>
                    </div>
                     <div class="group">
                        <div class="editor-label">
                            <%: Html.LabelFor(model => model.ExpectedPublishingDate)%>
                             
                        </div>
                        <div class="editor-field">  
                            <%: Html.EditorFor(model => model.ExpectedPublishingDate)%>
                            <%: Html.ValidationMessageFor(model => model.ExpectedPublishingDate)%>
                        </div>                         
                    </div>
                                     
                </div>
                 <div class="editor-label">
                    <%: Html.LabelFor(model => model.RevisionDescription) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.RevisionDescription, new { @rows = "5" })%>
                    <%: Html.ValidationMessageFor(model => model.RevisionDescription)%>
                </div>
            </div>
        </fieldset>

        <fieldset>
            <h2 class="open"><%: Model.Labels.ReferencesText%></h2>
            <div class="fieldsetContent">                
                 <% if (Model.NoOfRevisionReferences > 0)
                { %>
                    <% Html.RenderAction("ListReferences", "Reference", new {@guid = Model.GUID}); %> 
                 <% }
                 else
                  {%>
                    <div class="editor-label">
                        <strong><%: Model.Labels.NoReferencesAvaliableText%></strong>
                    </div>
                 <% }%>
                <%: Html.ValidationMessageFor(model => model.NoOfRevisionReferences, "", new { @style = "display:inline;" })%>         
                <%: Html.RenderAddReferenceLink(Model.GUID, Resources.DyntaxaResource.SharedManageReferences, "Edit", "Revision", new { @revisionId = Model.RevisionId }, new { @class = "ap-ui-button", @style = "margin-left: 10px;" })%>
            </div>
        </fieldset>

     <%--   <fieldset>
            <h2 class="open"><%: Model.Labels.RevisionEditReferencesHeaderText%></h2> 
        </fieldset>     --%>

        <fieldset>
            <h2 class="open"><%: Model.Labels.RevisionEditEditorsHeaderText%></h2>            
            <div class="fieldsetContent"> 
                <div id="demo"></div>                
                <a href="<%: Model.UserAdminLink.Url %>" target="_blank" style="margin-left: 10px"><%: Model.UserAdminLink.LinkText%></a>
                <select id="SelectedUsers" name="SelectedUsers" style="visibility: hidden; height : 0px; margin-bottom: 10px;" multiple="multiple">
                </select>
             </div>
        </fieldset>

         <fieldset>
            <h2 class="open"><%: Model.Labels.RevisionEditQualityHeaderText%></h2> 
                <div class="fieldsetContent">
                    <div class="formRow">
                    <div class="group">
                        <%:Html.LabelFor(model => model.RevisionQualityId) %>
                        <%:Html.DropDownListFor(model => model.RevisionQualityId, new SelectList(Model.RevisionQualityList, "Id", "Text", Model.RevisionQualityId))%>
                    </div>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.RevisionQualityDescription) %>
                    
                </div>
                <div class="editor-field input-wrapper">
                    <%: Html.TextAreaFor(model => model.RevisionQualityDescription)%>
                    <%: Html.ValidationMessageFor(model => model.RevisionQualityDescription)%>
                </div>
                </div>
        </fieldset>   
        
        <fieldset>            
           <input type="submit" id="<%:Model.Labels.GetSelectedSave %>" name="submitButton" class="ap-ui-button" value="<%: Model.Labels.SaveButtonLabel%>" />
            <% if (Model.ShowInitalizeButton && Model.NoOfRevisionReferences > 0)
             { 
                                  
                if (Model.IsTaxonInRevision)
                 { %>               
                    <input type="submit" id="<%:Model.Labels.GetSelectedInRevision %>" name="submitButton" class="ap-ui-button" value="<%:Model.Labels.InitializeButtonLabel %>" />
 
                 <% }
                else 
                 { %>               
                    <input type="submit" id="<%:Model.Labels.GetSelectedInitialize %>" name="submitButton" class="ap-ui-button" value="<%:Model.Labels.InitializeButtonLabel %>" />
                 <% }
             }
            else
            { %>
                <input type="submit" name="submitButton2" class="ap-ui-button-disabled" value="<%:Model.Labels.InitializeButtonLabel %>" disabled="disabled"/>
            <% }  %>
           <% if (Model.ShowUpdateSpeciesFactButton)
             { %>
                <input type="submit" id="<%:Model.Labels.GetSelectedFinalize %>" name="submitButton" class="ap-ui-button" value="<%:Model.Labels.SpeciesFactButtonLabel %>" />
             <% }
              else if ((Model.ShowFinalizeButton && Model.NoOfRevisionReferences > 0))
             { %>
                <input type="submit" id="<%:Model.Labels.GetSelectedFinalize %>" name="submitButton" class="ap-ui-button" value="<%:Model.Labels.FinalizeButtonLabel %>" />
             <% }
            else
            { %>                
                <input type="submit" class="ap-ui-button-disabled" id="finalizeButton" name="submitButton3"  value="<%:Model.Labels.FinalizeButtonLabel %>" disabled="disabled" />
           <% }%>
            
            <%: Html.ActionLink(Model.Labels.ResetButtonLabel, "Edit", new { revisionId = Model.RevisionId }, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>            
        </fieldset>
    </div>
    <% } %>

    <script language="javascript" type="text/javascript">

    function storeSelectedUsers() {
        //clear out old selected
        $('#SelectedItems').html('');
        var selectedData = $('#demo').data('MoverBoxes').SelectedValues();

        //for each element in the array we will add to the listbox.
        for (var i in selectedData) {                                
            $("#SelectedUsers").append(new Option('', selectedData[i]));                 
        }
        $("#SelectedUsers option").attr("selected","selected");        
    }


     function validForm(form) { 
            var isValid = $(form).valid();
            return isValid;
            }
     
    $(document).ready(function () {

        <% if ((Model.SelectedUserList != null && Model.SelectedUserList.Count < 1))
        { %>  
                showInfoDialog("<%:Model.Labels.DialogNoEditorsTitlePopUpText%>", "<%:Model.Labels.DialogNoEditorsTextPopUpText%>",  "<%:Model.Labels.ConfirmButtonLabel%>", null);
        <% } %>
       
         <% if ((Model.NoOfRevisionReferences < 1))
        { %>  
                showInfoDialog("<%:Model.Labels.DialogNoReferencesTitlePopUpText%>", "<%:Model.Labels.DialogNoReferencesTextPopUpText%>",  "<%:Model.Labels.ConfirmButtonLabel%>", null);
          <% } %>
  
        //This array will hold the values in the unassigned listbox
        var dataLeft = [];
        <% foreach (var user in userList)
           { %>
                dataLeft.push("<%: user.Id %>, <%: user.PersonName %>");
        <% } %>
        
        //this array will contain the data in the assigned list
        var dataRight = [];
         <% foreach (var selecetdUser in selectedUserList)
           { %>
                dataRight.push("<%: selecetdUser.Id %>, <%: selecetdUser.PersonName %>");
        <% } %>
        var avaliable = "<%:Model.Labels.AvaliableLabel%>";
        var selected = "<%:Model.Labels.SelectedLabel%>";
        /*
        //Options:
        dataleft: The data in left side listbox
        dataright: The data in right side listbo
        size: the height of the boxes
        */
        $('#demo').MoverBoxes({ 'dataleft': dataLeft, 'dataright': dataRight, 'size': 15, 'leftLabel': avaliable, 'rightLabel': selected });
        
//          initDetectFormChanges();
//        $('a[href*="Reference/Add"]').click(function (e) {
//            var element = this;
//            var changed = isAnyFormChanged();

//            if (changed) {
//                e.preventDefault();
//                showYesNoDialog("<%: Resources.DyntaxaResource.SharedDoYouWantToContinue %>",
//                        "<%: Resources.DyntaxaResource.SharedGoToReferenceViewQuestion %>",
//                        "<%: Resources.DyntaxaResource.SharedDialogButtonTextYes %>",
//                        function () {
//                            e.view.window.location = element.href; // on click redirect                             
//                        },
//                        "<%: Resources.DyntaxaResource.SharedDialogButtonTextNo %>",
//                        null);
//            }
//         });
        
         //get the selected values so we can update the database or whatever.
        $('#'+"<%:Model.Labels.GetSelectedSave%>" ).click(function () {
                var buttonId = this.id;
                var theform = document.editRevisionForm;
                if(validForm(theform)){
                storeSelectedUsers();
                theform.buttonClicked.value = buttonId;
                 $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>' });
                theform.submit();
                }
        });
        
//         $('#'+"<%:Model.Labels.GetSelectedSaveNoReferences%>" ).click(function () {
//                var buttonId = this.id;
//                var theform = document.editRevisionForm;
//                if(validForm(theform)){
//                storeSelectedUsers();
//                theform.buttonClicked.value = buttonId;
//                 $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>' });
//                theform.submit();
//                }
        
          //get the selected values so we can update the database or whatever.
        $('#'+"<%:Model.Labels.GetSelectedInitialize %>" ).click(function () {
            var buttonId = this.id;
            var theform = document.editRevisionForm;
            if(validForm(theform)){
            storeSelectedUsers();
            
            return showDialog("<%:Model.Labels.DialogInitTitlePopUpText%>", "<%:Model.Labels.DialogInitTextPopUpText%>", 
                               "<%:Model.Labels.RevisionYesButtonText%>", "<%:Model.Labels.RevisionNoButtonText%>", function() {                
                theform.buttonClicked.value = buttonId;
                 $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.InitializingLabel %></h1>' });
                theform.submit();                 
                } , null);
               }
        });
        
         //get the selected values so we can update the database or whatever, later on.
        $('#'+"<%:Model.Labels.GetSelectedInRevision %>").click(function () {
             var theform = document.editRevisionForm;
            if(validForm(theform)){
            storeSelectedUsers();
            
            return showInfoDialog("<%:Model.Labels.DialogInitTitlePopUpText%>", "<%:Model.Labels.DialogTaxonIsInRevisionTextPopUpText%>", 
                               "<%:Model.Labels.ConfirmButtonLabel%>", null);
            }

        });
        
          //get the selected values so we can update the database or whatever.
        $('#'+"<%:Model.Labels.GetSelectedFinalize %>").click(function () {
            var buttonId = this.id;
             var theform = document.editRevisionForm;
            if(validForm(theform)){
            storeSelectedUsers();
             return showDialog("<%:Model.Labels.DialogFinalizeTitlePopUpText%>", "<%:Model.Labels.DialogFinalizeTextPopUpText%>", 
                               "<%:Model.Labels.RevisionYesButtonText%>", "<%:Model.Labels.RevisionNoButtonText%>", function() {                
                theform.buttonClicked.value = buttonId;
                $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.FinalizingLabel %></h1>' });
                theform.submit();                 
                } , null);
            }
            
        });

        $('#GetUnSelected').click(function() {
            //clear out old 
            $('#SelectedItems').html('');
            var selectedData = $('#demo').data('MoverBoxes').NotSelectedValues();

            //for each element in the array we will add to the listbox.
            for (var i in selectedData) {
                var li_item = document.createElement("li");
                $(li_item).html(selectedData[i]);
                $('#SelectedItems').append(li_item);
            }
        });
    });
    
    //initToggleFieldsetH2();
    //initToggleFieldsetH3();    
    
      $(function () {
            var $datePicker = $('input#ExpectedStartDate, input#ExpectedPublishingDate');

            if ($datePicker.length !== 0) {
                $datePicker.datepicker(
                {
                    showAnim: '',
                    showOtherMonths: true,
                    showWeek: true,
                    changeMonth: true,
                    changeYear: true,
                    yearRange: 'c-100:c+10',
                    duration: 0
                },
                $.datepicker.setDefaults($.datepicker.regional['<%: System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName %>']));
            }

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
          
            //$("div.editor-field(0) :input:first").focus();

        });
        // DatePicker options > http://jqueryui.com/demos/datepicker/#options 
    

</script>
</asp:Content>
