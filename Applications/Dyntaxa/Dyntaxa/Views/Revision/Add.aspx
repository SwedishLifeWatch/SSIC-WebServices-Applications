<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.RevisionAddViewModel>" %>

<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.Labels.RevisionAddActionHeaderText%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% IList<RevisionUserItemModelHelper> userList = Model.UserList; %>
    <h1 class="readHeader">
        <%: Model.Labels.RevisionAddMainHeaderText%>
    </h1>
    <% if (ViewBag.Taxon != null)
        { %>
        <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
    <% }
        else
        { %>
        <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
    <% } %>    
    <% Html.EnableClientValidation(); %> 
    <% using (Html.BeginForm("Add", "Revision", FormMethod.Post, new { @id = "addRevisionForm", @name = "addRevisionForm" }))
       {%>
 
        <% if (!this.ViewData.ModelState.IsValid)
        { %>
        <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
            <h2 class="validationSummaryHeader">
                <%:Html.ValidationSummary(false, "")%>
            </h2>
        </fieldset>
        <% } %>
        
         <%: Html.Hidden("buttonClicked") %>
    <div id="fullContainer">
        <input type="hidden" id="taxonId" name="taxonId" value="<%:Model.TaxonId%>" />
        <input type="hidden" id="revisionId" name="revisionId" value="<%:Model.RevisionId%>" />
        <fieldset>
            <h2><%: Model.Labels.RevisionAddPropertiesHeaderText%></h2>
            <div class="fieldsetContent">
                

                <div class="editor-label">
                        <%: Html.LabelFor(model => model.ExpectedStartDate)%>
                </div>
                <div class="editor-field">
                    <%: Html.EditorFor(model => model.ExpectedStartDate)%>
                    <%: Html.ValidationMessageFor(model => model.ExpectedStartDate)%>
                </div>
             
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.ExpectedPublishingDate)%>
                </div>
                <div class="editor-field">
                    <%: Html.EditorFor(model => model.ExpectedPublishingDate)%>
                    <%: Html.ValidationMessageFor(model => model.ExpectedPublishingDate)%>
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
            <h2><%: Model.Labels.RevisionAddSelectEditorsHeaderText%></h2>            
            <div class="fieldsetContent">              
                <div id="demo"></div>                                
                <a href="<%: Model.UserAdminLink.Url %>" target="_blank" style="margin-left: 10px"><%: Model.UserAdminLink.LinkText%></a>
                <select id="SelectedUsers" name="SelectedUsers" style="visibility: hidden; height : 0px; margin-bottom: 10px;" multiple="multiple">
                </select>
            </div>
        </fieldset>
          <fieldset>
             <input type="submit" id="<%:Model.Labels.GetSelectedSave %>" name="submitButton" class="ap-ui-button" value="<%: Model.Labels.SaveButtonLabel%>" />
             <%: Html.ActionLink(Model.Labels.ResetButtonLabel, "Add", new { taxonId = Model.TaxonId }, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>            
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
        
        //This array will hold the values in the unassigned listbox
        var dataLeft = [];
        <% foreach (var user in userList)
           { %>
                dataLeft.push("<%: user.Id %>, <%: user.PersonName %>");
        <% } %>
        
        //this array will contain the data in the assigned list
        var dataRight = [];
        var avaliable = "<%:Model.Labels.AvaliableLabel%>";
        var selected = "<%:Model.Labels.SelectedLabel%>";
        /*
        //Options:
        dataleft: The data in left side listbox
        dataright: The data in right side listbo
        size: the height of the boxes
        */
        $('#demo').MoverBoxes({ 'dataleft': dataLeft, 'dataright': dataRight, 'size': 15, 'leftLabel': avaliable, 'rightLabel': selected });
        
        
        
         //get the selected values so we can update the database or whatever.
        $('#'+"<%:Model.Labels.GetSelectedSave%>" ).click(function () {
             var theform = document.addRevisionForm;
             if(validForm(theform)){ 
                 return showDialog("<%:Model.Labels.AddButtonLabel%>", "<%:Model.Labels.DialogAddRevisionInfoText%>", 
                                   "<%:Model.Labels.ConfirmButtonLabel%>", "<%:Model.Labels.CancelButtonLabel%>", function() {                
                 
               
                var buttonId = this.id;
                storeSelectedUsers();
                theform.buttonClicked.value = buttonId;
                 $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SavingLabel %></h1>' });
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

            //$("div.editor-field(0) :input:first").focus();

        });
        // DatePicker options > http://jqueryui.com/demos/datepicker/#options 
    

</script>
</asp:Content>
