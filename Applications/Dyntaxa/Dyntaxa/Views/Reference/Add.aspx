<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.Reference.ReferenceAddViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data.Reference" %>
<%@Import namespace="Dyntaxa.Helpers.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.Labels.TitleLabel %>    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1 class="readHeader">
        <%: Model.Labels.TitleLabel %>        
    </h1>

    <% if(Model.TaxonId.HasValue)
       { %>
        <% if (ViewBag.Taxon != null)
            { %>
            <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
        <% }
            else
            { %>
            <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
        <% } %>
    <% } %>
    
    <% using (Html.BeginForm("Add", "Reference", FormMethod.Post, new { @id = "selectReferencesForm", @name = "selectReferencesForm" }))
    { %>    
    <!-- Full container start -->
    <div id="fullContainer">
         <fieldset>
            <h2><%:  Model.Labels.SharedDialogInformationHeader%></h2>
            <div class="fieldsetContent">       
                <% Html.RenderAction("GuidObjectInfo", "Reference", new { guid = Model.Guid }); %>
            </div>
        </fieldset>
        <fieldset>
            <h2><%: Model.Labels.ReferencesLabel %></h2>
            <div class="fieldsetContent">              
			    <div class="referenceTableWrapper">     
                    <table id="selectedReferencesTable" class="referenceTableAutoWidth"></table>
			    </div>
            </div>
        </fieldset>

        <fieldset>
            <h2><%: Model.Labels.SearchReferencesLabel %></h2>
            <div class="fieldsetContent">              
                <%--<button id="addButton" type="button">Add</button>                            --%>
			    <div class="referenceTableWrapper">
                    <table id="tableSearch" class="referenceTable">
	
                    </table>                    
			    </div>                
                <button style="clear: both; float: left;" id="addButton" type="button" class="ap-ui-button"><%: Model.Labels.AddButtonLabel %></button>
                <%--<button class="ap-ui-button" style="clear: both; float: left;" id="Button1" type="button"><%: Model.Labels.AddButtonLabel %></button>--%>
            </div>
        </fieldset>
                
        <fieldset>
            <% if (Model.ShowReferenceApplyMode) %>
            <% { %>
            <div style="margin-left: 5px; margin-top: 5px;">
                <h4 style="font-weight: bold;"><%:Resources.DyntaxaResource.ReferenceAddReferencesRelatesTo%>:</h4>
                <ul>
                    <li><input type="radio" checked="checked" name="applyMode" value="<%= (int) ReferenceApplyMode.OnlySelected %>"><%:Resources.DyntaxaResource.ReferenceApplyModeOnlySelected %></li>
                    <li><input type="radio" name="applyMode" value="<%= (int) ReferenceApplyMode.AddToUnderlyingTaxa %>"><%:Resources.DyntaxaResource.ReferenceApplyModeAddToUnderlyingTaxa %></li>
                    <li><input type="radio" name="applyMode" value="<%= (int) ReferenceApplyMode.ReplaceUnderlyingTaxa %>"><%:Resources.DyntaxaResource.ReferenceApplyModeReplaceUnderlyingTaxa %></li>
                    <li><input type="radio" name="applyMode" value="<%= (int) ReferenceApplyMode.ReplaceOnlySourceInUnderlyingTaxa %>"><%:Resources.DyntaxaResource.ReferenceApplyModeReplaceOnlySourceInUnderlyingTaxa %></li>
                </ul>
            </div>
            <% } %>
            <button id="btnPost" type="button" class="ap-ui-button"><%:Resources.DyntaxaResource.SharedSaveButtonText%></button>
            <%: Html.ActionLink(Resources.DyntaxaResource.SharedCancelButtonText, Model.ReturnAction, Model.ReturnController, Model.RouteValues, new Dictionary<string, object>() { { "class", "ap-ui-button" },{ "style", "margin-top: 10px;" }})%>  
            <button id="btnCreateNewReference" type="button" class="ap-ui-button" style="float: right;"><%: Model.Labels.CreateNewReferenceLabel %></button>     
        </fieldset>       

        <button type="button" id="referenceSearchButton" class="ap-ui-button" style="margin-left: 5px; margin-top: 0px; margin-bottom: 5px; height: 22px;"><%: Resources.DyntaxaResource.ReferenceAddSearchLabel%></button>
        
            <%: Html.HiddenFor(m => m.Guid) %>
            <%: Html.HiddenFor(m => m.ReturnAction) %>
            <%: Html.HiddenFor(m => m.ReturnController) %>
            <%: Html.HiddenFor(m => m.ReturnParameters) %>
            <%: Html.HiddenFor(m => m.TaxonId) %>
            <%: Html.Hidden("SelectedReferences") %>            
        <% } %>

    </div>
    
<script type="text/javascript">

    $("#btnCreateNewReference").click(function() {
        showCreateNewReferenceDialog();        
    });
    
    function showCreateNewReferenceDialog() {   
        var dialogDiv = $("<div></div>");        
        
        // Load the form into the dialog div
        $(dialogDiv).load('<%= Url.Action("New","Reference") %>', function () {
            $(this).dialog({                                   
                modal: true,
                width: "500px",
                resizable: false,
                draggable: false,
                zIndex: 999999,                    
                title: '<%: Model.Labels.CreateNewReferenceLabel %>',
                buttons: {
                    "<%: Resources.DyntaxaResource.SharedSaveButtonText %>": function () {                 
                        var form = $('form', this);
                        $(form).submit();
                    },
                    "<%: Resources.DyntaxaResource.SharedCancelButtonText %>": function () { $(this).dialog('close'); }
                }
            });

            // Enable client side validation
            $.validator.unobtrusive.parse(dialogDiv);

            // Setup the ajax submit logic
            wireUpReferenceForm(dialogDiv);                        
        });                       
      }    
    
    
    // sumbit the form
    $("#btnPost").click(function () {
        var frm = document.selectReferencesForm;
        var selectedReferences = [];

        var selectedReferencesTable = $("#selectedReferencesTable").dataTable();
        $(selectedReferencesTable.fnGetNodes()).each(function(i) {
            var $selectBox = $(this).find('select');
            var usageTypeId = parseInt($selectBox.val());            
            var aPos = selectedReferencesTable.fnGetPosition(this);
            var rowData = selectedReferencesTable.fnGetData(aPos);            
            selectedReferences.push({ Id: rowData.Id, UsageTypeId: usageTypeId });            
        });
        
        frm.SelectedReferences.value = JSON.stringify(selectedReferences);
        frm.submit();
    });


    $('#addButton').click(function () {
        var searchTable = $("#tableSearch").dataTable();
        var selectedReferencesTable = $("#selectedReferencesTable").dataTable();
        var numberAdded = 0;
        
        $(searchTable.fnGetNodes()).each(function (i) {
            var $selectBox = $(this).find('select');
            var usageTypeId = parseInt($selectBox.val());
            var usageTypeText = $selectBox.find("option:selected").text();

            if (usageTypeId != -1) {
                var aPos = searchTable.fnGetPosition(this);
                var rowData = searchTable.fnGetData(aPos);
                var val = { Id: rowData.Id, Name: rowData.Name, Year: rowData.Year, Text: rowData.Text, Usage: usageTypeText, UsageTypeId: usageTypeId };
                if (!doesRecordExist(val)) {
                    selectedReferencesTable.fnAddData(val);
                    numberAdded++;
                } else {
                    alert("Can't add duplicate");
                }
                
            }
            $selectBox.val(-1);            
        });
        if (numberAdded == 0) {
            alert("No references added");            
        }                    
        selectedReferencesTable.fnAdjustColumnSizing(true);        
        
        if (numberAdded > 0) {
            $('html, body').animate({ scrollTop: 0 }, 'slow');
        }
        
    });

    function doesRecordExist(newRecord) {
        var selectedReferencesTable = $("#selectedReferencesTable").dataTable();
        var aData = selectedReferencesTable.fnGetData();
        for (var i = 0; i < aData.length; i++) {
            if (aData[i].Id == newRecord.Id && aData[i].UsageTypeId == newRecord.UsageTypeId) {
                return true;
            }            
        }
        return false;
    }
    
//    $('#searchButton').click(function () {
//        var strSearch = $('#searchString').val();
//        alert(strSearch);
//        search(strSearch);
//    });


    function reloadTable() {
        var searchTable = $("#tableSearch").dataTable();        
        searchTable.fnReloadAjax();
        searchTable.fnSort([ [1,'desc']] );        
    }
    
    
    function removeReference(imgClicked) {
        var tableRow = $(imgClicked).closest("tr").get(0);
        var selectedReferencesTable = $("#selectedReferencesTable").dataTable();        
        selectedReferencesTable.fnDeleteRow(tableRow);
    }    
    
    
    function search(strSearch) {
        var searchTable = $("#tableSearch").dataTable();
        searchTable.fnClearTable();

        $.getJSON('<%= Url.Action("SearchReference","Reference") %>', { searchString: strSearch}, function (json) {
            $.each(data, function (i, row) {
                searchTable.fnAddData(row);
            });            
        });                 
    };
    
    
    
    function makeUsageTypeDropDown(oObj) {
        var usageTypeId = oObj.aData["UsageTypeId"];
        //var referenceTypes = [{ Id: 1, Name: "Används i"}, { Id: 2, Name: "Källa"} ];
        var referenceTypes = <%= Model.GetReferenceTypesJavascriptArray() %>;
        
        var dropdown = "<select name='usageType' style='width: 100%'>";
        for (var i=0; i < referenceTypes.length; i++) {
            if (usageTypeId == referenceTypes[i].Id) {
                dropdown += "<option value='" + referenceTypes[i].Id + "' selected='selected'>" + referenceTypes[i].Name +"</option>";
            } else {
                dropdown += "<option value='" + referenceTypes[i].Id + "' >" + referenceTypes[i].Name +"</option>";                
            }
        }
        dropdown += "</select>";
                
        return dropdown;                
    }
 

    $(document).ready(function () {
               
        $('#tableSearch').dataTable({
            "bProcessing": true,
            "sAjaxSource": '<%: Url.Action("GetReferenceData", "Reference") %>',
            "sScrollY": 200,              
            "bLengthChange": false,
            "sDom": '<"#searchDiv"f>lrtip',
            "bAutoWidth": false,
            "bPaginate": false,                                 
            "aoColumns": [
                { "sTitle": "<%: Model.Labels.ColumnTitleType %>", "sWidth": "70px", "bSortable": false, "fnRender": function (oObj) {
                    var strSelect = '<%= Model.ReferenceTypesSelectBoxString %>';
                    return strSelect;
                } 
                },
                { "sTitle": "<%: Model.Labels.ColumnTitleId %>", "mDataProp": "Id", "sWidth": "30px" },
                { "sTitle": "<%: Model.Labels.ColumnTitleName %>", "mDataProp": "Name", "sWidth": "100px" },
                { "sTitle": "<%: Model.Labels.ColumnTitleYear %>", "mDataProp": "Year", "sWidth": "40px" },
                { "sTitle": "<%: Model.Labels.ColumnTitleText %>", "mDataProp": "Text", "sWidth": "170px" }
             ],
            "oLanguage": {
                "sSearch": "<%: Model.Labels.SearchLabel %>:",
                "sInfo": "<%: Model.Labels.NumberOfFilteredElementsLabel %>",
                "sEmptyTable": "<%: Model.Labels.NoDataAvailableLabel %>",
                "sInfoFiltered": "<%: Model.Labels.FilteringLabel %>",
                "sZeroRecords": "<%: Model.Labels.NoRecordsLabel %>"
            }                
                
        });
        
        $('#tableSearch').dataTable().fnFilterOnButton();
        $("#referenceSearchButton").appendTo("#searchDiv");
        
        
        $('#selectedReferencesTable').dataTable({
            "aaData": <%= Model.GetSelectedReferencesAsJSON() %>,
            "bAutoWidth": false,
            "bInfo": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bFilter": false,
            "sScrollY": "160px",
            "bScrollCollapse": false,
            "aoColumns": [
                { "sTitle": "<%: Model.Labels.ColumnTitleId %>", "mDataProp": "Id", "sWidth": "5%" },
                { "sTitle": "<%: Model.Labels.ColumnTitleName %>", "mDataProp": "Name", "sWidth": "25%" },
                { "sTitle": "<%: Model.Labels.ColumnTitleYear %>", "mDataProp": "Year", "sWidth": "8%" },
                { "sTitle": "<%: Model.Labels.ColumnTitleText %>", "mDataProp": "Text", "sWidth": "45%" },
                { "sTitle": "<%: Model.Labels.ColumnTitleUsage %>", "sWidth": "15%", "bSortable": false, "fnRender": makeUsageTypeDropDown},            
                { "sTitle": "<%: Model.Labels.ColumnTitleUsageTypeId %>", "mDataProp": "UsageTypeId", "bVisible": false, "sWidth": "0px" },
                { "sTitle": "", "sWidth": "3%", "bSortable": false, "fnRender": function (oObj) {
                    var strImageButton = '<img src="<%= Url.Content("~/Images/Icons/delete_12x12.png")%>" style="cursor: pointer;" onclick="removeReference(this)" />';
                    return strImageButton;
                }
                }
             ],
            "oLanguage": {                
                "sEmptyTable": "<%: Model.Labels.NoDataAvailableLabel %>"
             }
        });

    }); 
</script> 


</asp:Content>
