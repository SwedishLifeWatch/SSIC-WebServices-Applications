<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.SpeciesFactViewModel>" %>
<%@ Import Namespace="System.Security.Policy" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.Labels.TitleLabel%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="readHeader"><%: Model.Labels.TitleLabel %></h1>    
    <% if (ViewBag.Taxon != null)
        { %>
        <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
    <% }
        else
        { %>
        <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
    <% } %>    



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
     <% using (Html.BeginForm(Model.PostAction, "SpeciesFact", FormMethod.Post, new { @id = "editFactorForm", @name = "editFactorForm" }))
      { %>
      <%: Html.HiddenFor(model => model.TaxonId) %>
      <%: Html.HiddenFor(model => model.MainParentFactorId) %>
      <%: Html.HiddenFor(model => model.FactorDataType) %>
      <%: Html.HiddenFor(model => model.DataType) %>
      <%: Html.HiddenFor(model => model.ReferenceId) %>
      <%: Html.Hidden("downloadTokenValue")%>

      
           <% if (Model.SpeciesFactViewModelHeaderItemList != null)
            {
             for (int i = 0; i < Model.SpeciesFactViewModelHeaderItemList.Count; i++)
            { %>
              <% SpeciesFactViewModelItem headerItem = Model.SpeciesFactViewModelHeaderItemList[i].SpeciesFactViewModelItem; %>
              <% IList<SpeciesFactViewModelSubHeaderItem> subHeaderList = Model.SpeciesFactViewModelHeaderItemList[i].SpeciecFactViewModelSubHeaderItemList; %>
               <fieldset>
                <h2 class="open"><%: headerItem.MainHeader%></h2>
                <div class="fieldsetContent">   
                
                <% for (int j = 0; j < subHeaderList.Count; j++)
                { %>
                    <% SpeciesFactViewModelItem subHeaderItem = subHeaderList[j].SpeciesFactViewModelItem; %>
                    <% IList<SpeciesFactViewModelItem> factList = subHeaderList[j].SpeciesFactViewModelItemList; %>
                    
                     <% if (subHeaderItem.IsSubHeader)
                     { %>
                        <h3 class="open"><%: subHeaderItem.SubHeader%></h3>
                     
                     <% } %>
                     <%
                        else
                        { %>
                         <h3 class="open" style="background-color:azure"><%: headerItem.MainHeader%></h3>       
                      <%  } %>
                        <div class="fieldsetSubContent fullWidth">                    
                         
                             <% if (factList != null && factList.Count > 0)
                            { %>   
                                <%  var grid = new WebGrid(source: factList,
                                                defaultSort: "Title",
                                                rowsPerPage: 100); 
                                            %>
                                        <div id="grid" >
                                            <%-- TODO remove columns ie IndividualCategory, Faktor id whem implementattion is final only used for debug --%>
                                              <% if (Model.MainParentFactorId == DyntaxaFactorId.SUBSTRATE)
                                                { %>     
                                                     <%: grid.GetHtml(tableStyle: "editFactorTable", headerStyle: "head",alternatingRowStyle: "alt", htmlAttributes: new {id ="editFactorTableGrid"},
                                                displayHeader: true,
                                                columns:                                                 
                                                    grid.Columns(grid.Column("", "", style:"",  format: (item) =>
                                                                                        {
                                                                                            var factorItem = ((SpeciesFactViewModelItem)item.Value);
                                                                                            if (factorItem.IsShortList)
                                                                                                return "*";
                                                                                            return " ";
                                                                                        }
                                                                                    ),
                                                                grid.Column("FactorName", Model.Labels.SpeciesFactMainHeader, style: "factorName", format: (item) =>
                                                                                                    {
                                                                                                        var factorItem = ((SpeciesFactViewModelItem)item.Value);
                                                                                                        if (factorItem.IsSuperiorHeader || factorItem.IsHeader)
                                                                                                        return Html.Raw(string.Format("<strong>{0}</strong>", factorItem.SuperiorHeader));
                                                                                                        else
                                                                                                        return Html.RenderSpeciesFactHostName(factorItem.FactorName, factorItem.UseDifferentColor, factorItem.UseDifferentColorFromIndex);
                                                                                                    }),   
                                                                   grid.Column("FactorFieldValue", Model.FactorFieldValueTableHeader
                                                                                                        , style: "factorValue",
                                                                                    format: (item) => 
                                                                                        {
                                                                                            var factorItem = ((SpeciesFactViewModelItem)item.Value);

                                                                                            if (factorItem.FieldValues != null)
                                                                                            {
                                                                                                return Html.Partial("~/Views/SpeciesFact/FactorEnumValues.ascx", (SpeciesFactViewModelItem)item.Value);
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                return factorItem.FactorFieldValue;
                                                                                            }
                                                                                        }),
                                                                        
                                                                         grid.Column("FactorFieldValue2", Model.FactorFieldValue2TableHeader
                                                                                                        , style: "factorValue2",
                                                                                    format: (item) => 
                                                                                        {
                                                                                            var factorItem = ((SpeciesFactViewModelItem)item.Value);

                                                                                            if (factorItem.FieldValues2 != null)
                                                                                            {
                                                                                                return Html.Partial("~/Views/SpeciesFact/FactorDropDownValues.ascx", (SpeciesFactViewModelItem)item.Value);
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                return factorItem.FactorFieldValue2;
                                                                                            }
                                                                                        }),
                                                                             
                                                                        grid.Column("IndividualCategoryName", Model.Labels.SpeciesFactCategoryHeader, style: "factorCategory"),
                                                                        //grid.Column("FactorFieldComment", Model.Labels.SpeciesFactCommentHeader, style: "factorComment"),
                                                                        grid.Column("Quality", Model.Labels.SpeciesFactQualityHeader,style: "factorQuality",
                                                                                    format: (item) => 
                                                                                        {
                                                                                            var factorItem = ((SpeciesFactViewModelItem)item.Value);

                                                                                            if (factorItem.QualityValues != null)
                                                                                            {
                                                                                                return Html.Partial("~/Views/SpeciesFact/FactorQualityDropDownValues.ascx", (SpeciesFactViewModelItem)item.Value);
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                return factorItem.Quality;
                                                                                            }
                                                                                        }),
                                                                        grid.Column("", "", style: "factorChange", format: (item) =>
                                                                                    {
                                                                                        var factorItem = ((SpeciesFactViewModelItem)item.Value);
                                                                                        if (factorItem.FieldValues != null && !(factorItem.IsHost && ((int)factorItem.MainParentFactorId == (int)DyntaxaFactorId.SUBSTRATE)))
                                                                                        {

                                                                                            return Html.Raw("<a href=\"#\" " + "onclick=\"showCreateNewReferenceDialog(" + factorItem.FactorId + "," + factorItem.ReferenceId + "," + factorItem.IndividualCategoryId + "," + factorItem.HostId + "," + factorItem.MainParentFactorId + ");\">" + Resources.DyntaxaResource.SpeciesFactChangeFactorText + "</a>");
                                                                        
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            return string.Empty;
                                                                                        }
                                                                                    })
                                                                                    

                                                                                                               //       , grid.Column("FactorId", Model.Labels.SpeciesFactFactorId, style: "factorId")
                                                                       // ,grid.Column("FactorSortOrder", Model.Labels.SpeciesFactSortOrder, style: "factorSortOrder")
                                                )
                                    ) %>
                                             <% } 
                                                else
                                                {%>
                                                     <%: grid.GetHtml(tableStyle: "editFactorTable", headerStyle: "head",alternatingRowStyle: "alt", htmlAttributes: new {id ="editFactorTableGrid2"},
                                                                displayHeader: true,
                                                                columns: 
                                                                    grid.Columns(grid.Column("", "", style:"",  format: (item) =>
                                                                                        {
                                                                                            var factorItem = ((SpeciesFactViewModelItem)item.Value);
                                                                                            if (factorItem.IsShortList)
                                                                                                return "*";
                                                                                            return " ";
                                                                                        }
                                                                                    ),
                                                                                grid.Column("FactorName", Model.Labels.SpeciesFactMainHeader, style: "factorName", format: (item) =>
                                                                                                                    {
                                                                                                                        var factorItem = ((SpeciesFactViewModelItem)item.Value);
                                                                                                                        if (factorItem.IsSuperiorHeader || factorItem.IsHeader)
                                                                                                                        return Html.Raw(string.Format("<strong>{0}</strong>", factorItem.SuperiorHeader));
                                                                                                                        else
                                                                                                                        return Html.RenderSpeciesFactHostName(factorItem.FactorName, factorItem.UseDifferentColor, factorItem.UseDifferentColorFromIndex);
                                                                                                                    }),   
                                                                                   grid.Column("FactorFieldValue", Model.FactorFieldValueTableHeader
                                                                                                                        , style: "factorValue",
                                                                                                    format: (item) => 
                                                                                                        {
                                                                                                            var factorItem = ((SpeciesFactViewModelItem)item.Value);

                                                                                                            if (factorItem.FieldValues != null)
                                                                                                            {
                                                                                                                return Html.Partial("~/Views/SpeciesFact/FactorEnumValues.ascx", (SpeciesFactViewModelItem)item.Value);
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                return factorItem.FactorFieldValue;
                                                                                                            }
                                                                                                        }),
                                                                        
                                                                        
                                                                                        grid.Column("IndividualCategoryName", Model.Labels.SpeciesFactCategoryHeader, style: "factorCategory"),
                                                                                        //grid.Column("FactorFieldComment", Model.Labels.SpeciesFactCommentHeader, style: "factorComment"),
                                                                                        //grid.Column("Quality", Model.Labels.SpeciesFactQualityHeader,style: "factorQuality"),
                                                                                         grid.Column("Quality", Model.Labels.SpeciesFactQualityHeader,style: "factorQuality",
                                                                                    format: (item) => 
                                                                                        {
                                                                                            var factorItem = ((SpeciesFactViewModelItem)item.Value);

                                                                                            if (factorItem.QualityValues != null)
                                                                                            {
                                                                                                return Html.Partial("~/Views/SpeciesFact/FactorQualityDropDownValues.ascx", (SpeciesFactViewModelItem)item.Value);
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                return factorItem.Quality;
                                                                                            }
                                                                                        }),
                                                                                        grid.Column("", "", style: "factorChange", format: (item) =>
                                                                                                    {
                                                                                                        var factorItem = ((SpeciesFactViewModelItem)item.Value);
                                                                                                        if (factorItem.FieldValues != null && factorItem.IsOkToUpdate)
                                                                                                        {

                                                                                                            return Html.Raw("<a href=\"#\" " + "onclick=\"showCreateNewReferenceDialog(" + factorItem.FactorId + "," + factorItem.ReferenceId + "," + factorItem.IndividualCategoryId + "," + factorItem.HostId + "," + factorItem.MainParentFactorId + ");\">" + Resources.DyntaxaResource.SpeciesFactChangeFactorText + "</a>");
                                                                        
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            return string.Empty;
                                                                                                        }
                                                                                                    })

                                                                                  // ,grid.Column("FactorId", Model.Labels.SpeciesFactFactorId, style: "factorId")
                                                                                       // ,grid.Column("FactorSortOrder", Model.Labels.SpeciesFactSortOrder, style: "factorSortOrder")
                                                                )
                                                    ) %>
                                                <% } %> 
                               </div>
                    <% } %>
                                
                    </div>
                <% } %>
                    
                    <button id="saveEditFactorForm"  type="submit" class="ap-ui-button"><%: Model.Labels.SaveNewValuesLabel %></button>      
                    <%: Html.ActionLink(Model.Labels.ResetLabel, "EditFactors", new { taxonId = Model.TaxonId, factorId = (int)Model.MainParentFactorId,  dataType = (int)Model.DataType,  factorDataType  = (int)Model.FactorDataType}, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>
     
               </div>
               </fieldset>
            <%}
         } %> 
    <% } %> 
          <fieldset>
             
             <h2 class="open"><%: Model.Labels.AddLabel%></h2>
             <div class="fieldsetContent">
                 <div style="display: inline-block;">
      <% using (Html.BeginForm("AddFactor", "SpeciesFact", FormMethod.Post, new { @id = "addFactorForm", @name = "addFactorForm"  }))
       { %>
       
    
            <%: Html.HiddenFor(model => model.TaxonId) %>
            <%: Html.HiddenFor(model => model.MainParentFactorId) %>
            <%: Html.HiddenFor(model => model.FactorDataType) %>
            <%: Html.HiddenFor(model => model.DataType) %>
            <%: Html.HiddenFor(model => model.ReferenceId) %>
            <%: Html.Hidden("downloadTokenValue")%>
       
                 <div style="padding:15px; display: inline-block;">
                     <div class="group">
                        <%: Html.LabelFor(model => model.DropDownFactorId)%>
                         <select id="DropDownFactorId" name="DropDownFactorId" style="width: 300px; margin-top: 5px;">
                             <% foreach (var item in Model.FactorList)
                                {
                                    if (item.Selectable == false) // header
                                    {
                                        
                                        %>
                                            <optgroup label="<%: item.Text %>"> 
                                        <%
                                        
                                    }
                                    else
                                    {
                                     %>
                                    <option value="<%=item.Id%>">&nbsp;&nbsp;&nbsp;<%: item.Text %></option>
                                    <%
                                    }                                  
                                } %>                          
                        </select> 
                         

                        <%--<%:Html.DropDownListFor(model => model.DropDownFactorId, new SelectList(Model.FactorList, "Id", "Text", Model.DropDownFactorId),new {style="width: 300px;"})%>                               --%>
                    </div>    
                                       
                     <div style="display: inline-block; vertical-align: bottom; margin-left: 5px;">
                        <button id="addFactorButton" type="button" class="ap-ui-button" style="margin:0px;"><%: Model.Labels.AddButtonLabel %></button>                                               
                     </div>
                 </div>               
               
    <% } %>    
       </div> 
       <div style="display: inline-block;">
          <% using (Html.BeginForm("AddFromAllAvaliableFactor", "SpeciesFact", FormMethod.Post, new { @id = "addFromAllAvaliableFactorForm", @name = "addFromAllAvaliableFactorForm" }))
       { %>
       
    
            <%: Html.HiddenFor(model => model.TaxonId) %>
            <%: Html.HiddenFor(model => model.MainParentFactorId) %>
            <%: Html.HiddenFor(model => model.FactorDataType) %>
            <%: Html.HiddenFor(model => model.DataType) %>
            <%: Html.HiddenFor(model => model.ReferenceId) %>
            <%: Html.Hidden("downloadTokenValue")%>
         
                 <div style="padding:15px; display: inline-block;">
                     <div class="group">
                        <%: Html.LabelFor(model => model.DropDownAllFactorId)%>
                         <select id="DropDownAllFactorId" name="DropDownAllFactorId" style="width: 300px; margin-top: 5px;">
                             <% foreach (var item in Model.AllAvaliableFactors)
                                {
                                    if (item.Selectable == false) // header
                                    {
                                        
                                        %>
                                            <optgroup label="<%: item.Text %>"> 
                                        <%
                                        
                                    }
                                    else
                                    {
                                     %>
                                    <option value="<%=item.Id%>">&nbsp;&nbsp;&nbsp;<%: item.Text %></option>
                                    <%
                                    }                                  
                                } %>                          
                        </select> 
                         

                        <%--<%:Html.DropDownListFor(model => model.DropDownFactorId, new SelectList(Model.FactorList, "Id", "Text", Model.DropDownFactorId),new {style="width: 300px;"})%>                               --%>
                    </div>    
                                       
                     <div style="display: inline-block; vertical-align: bottom; margin-left: 5px;">
                        <button id="addAllFactorButton" type="button" class="ap-ui-button" style="margin:0px;"><%: Model.Labels.AddButtonLabel %></button>                                               
                     </div>
                 </div>               
              
    <% } %>   
        </div>    
      </div>
        </fieldset>    
   
<!-- Full container end -->
    </div>   
   
    <div id="dialog-confirm" style="display:none" title="<%:Model.Labels.SpeciesFactAddFactorContinueWithoutSaveDialogHeader%>" >
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 20px 0;"></span><%: Model.Labels.SpeciesFactAddFactorContinueWithoutSaveDialogText %></p>
   </div>     
    

    <script type="text/javascript">
        var origStrRadioButtonStates;
        
        var fileDownloadCheckTimer;
        function blockUIForDownload() {
            var token = new Date().getTime(); //use the current timestamp as the token value
            $('#downloadTokenValue').val(token);
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SaveLabel %></h1>' });            
            fileDownloadCheckTimer = window.setInterval(function () {
                var cookieValue = $.cookie('fileDownloadToken');
                if (cookieValue == token)
                    finishDownload();
            }, 500);
        }

        function finishDownload() {
            window.clearInterval(fileDownloadCheckTimer);
            $.cookie('fileDownloadToken', null); //clears this cookie value
            $.unblockUI();
        }

        $(document).ready(function () {
           
         
            $("#editFactorForm").submit(function () {
                blockUIForDownload();
            });
            $("#addFromAllAvaliableFactorForm").submit(function () {
                blockUIForDownload();
            });
            $("#addFactorForm").submit(function () {
                blockUIForDownload();
            });
            
            $('#addFactorButton').click(function () {
                var val = $("#DropDownFactorId").val();
                if (val == 0) {
                    return;
                }
                var currentRadioButtonStatesString = createPageStateString();
                if (origStrRadioButtonStates != currentRadioButtonStatesString) {
                    $("#dialog-confirm").dialog({
                        resizable: false,
                        height: 160,
                        width:  450,
                        modal: true,
                        buttons: {
                            "<%: Model.Labels.YesLabel %>": function () {
                                $(this).dialog("close");
                                $("#addFactorForm").submit();
                            },
                            "<%: Model.Labels.NoLabel %>": function () {
                                $(this).dialog("close");
                                return;
                            }
                        }
                    });                    
                    return;
                }
                $("#addFactorForm").submit();

            });
            
            $('#addAllFactorButton').click(function () {
                var val = $("#DropDownAllFactorId").val();
                if (val == 0) {
                    return;
                }
                var currentRadioButtonStatesString = createPageStateString();
                if (origStrRadioButtonStates != currentRadioButtonStatesString) {
                    $("#dialog-confirm").dialog({
                        resizable: false,
                        height: 160,
                        width: 450,
                        modal: true,
                        buttons: {
                            "<%: Model.Labels.YesLabel %>": function () {
                                $(this).dialog("close");
                                $("#addFromAllAvaliableFactorForm").submit();
                            },
                            "<%: Model.Labels.NoLabel %>": function () {
                                $(this).dialog("close");
                                return;
                            }
                        }
                    });
                    return;
                }
                $("#addFromAllAvaliableFactorForm").submit();

            });
            $('#DropDownFactorId').select2();
            $('#DropDownAllFactorId').select2();
            origStrRadioButtonStates = createPageStateString();
            
            $('#editFactorTableGrid tr td:first-child').filter(':contains("*")').each(function () {
                $(this).closest('tr').find("td").css("color", "#000000");
                $(this).closest('tr').find("td").css("background", "#98FB98");
               
            });

            $('#editFactorTableGrid2 tr td:first-child').filter(':contains("*")').each(function () {
                $(this).closest('tr').find("td").css("background", "#98FB98");
                $(this).closest('tr').find("td").css("color", "#000000");
            });

        });        

        
        var dialog;

        function initIndividualCategorySelectBox() {
            $("#IndividualCategoryId").change(function () {
                var val = $(this).val();
                var url = baseUrl + "&individualCategoryId=" + val + "&hostId=" + baseHostId + "&mainParentFactorId=" + baseMainParentFactorId;
                $.unblockUI();
                $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.LoadingLabel %></h1>' });

                $.get(url, function (data) {
                    
                    dialog.html(data);
                   // $.unblockUI();
                    initIndividualCategorySelectBox();
                    $.unblockUI();
                });
               // $.unblockUI();
                
            });
        }

        function loadDialog(dialogDiv, url) {
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.LoadingLabel %></h1>' });

            $(dialogDiv).load(url, function () {
                dialog = $(this).dialog({
                    modal: true,
                    width: "550px",
                    resizable: false,
                    draggable: true,
                    zIndex: 999999,
                    title: "<%: Model.Labels.FactorPopUpLabel %>",
                    buttons: {
                        "<%: Model.Labels.SaveLabel %>": function () {
                            var form = $('form', this);
                            $(form).submit();
                            
                        },
                        "<%: Model.Labels.CancelLabel %>": function () { $(this).dialog('close'); }
                    },
                    open: function (event, ui) {
                       $.unblockUI();
                        initIndividualCategorySelectBox();
                        
                        // Enable client side validation
                        $.validator.unobtrusive.parse(dialogDiv);

                        // Setup the ajax submit logic
                        wireUpEditFactorForm(dialogDiv);
                        
                    }
                });
            });
           
        }

        function createPageStateString() {
            return createRadioButtonStatesString() + createSelectBoxStatesString();
        }

        function createSelectBoxStatesString() {
            var $selectBoxes = $('#editFactorForm select');
            var strSelectBoxesStates = "";
            $selectBoxes.each(function (index, value) {
                var name = $(value).attr('name');
                var val = $(value).val();
                strSelectBoxesStates += name;
                strSelectBoxesStates += val;
            });            
            return strSelectBoxesStates;
        }

        function createRadioButtonStatesString() {
            var $radioButtons = $('input[type=radio]');
            var strRadioButtonStates = "";
            $radioButtons.each(function (index, value) {
                var name = $(value).attr('name');
                var isChecked = $(value).attr('checked') == 'checked';
                strRadioButtonStates += name;
                strRadioButtonStates += isChecked;                
            });
            return strRadioButtonStates;
        }


        var baseUrl;
        var baseHostId;
        var baseMainParentFactorId;
        
        function showCreateNewReferenceDialog(childFactorId, referenceId, individualCategory, hostId, mainParentFactorId) {
            //string taxonId, string dataType, string factorDataType, string childFactorId, string referenceId, string individualCategoryId, string hostId, string mainParentFactorId
            baseHostId = hostId;
            baseMainParentFactorId = mainParentFactorId;
            var dialogDiv = $("<div></div>");
            baseUrl = '<%= Url.Action("EditFactorItem","SpeciesFact") %>';
            baseUrl += "?taxonId=" + "<%: Model.TaxonId %>";
            baseUrl += "&dataType=" + "<%: (int)Model.DataType %>";
            baseUrl += "&factorDataType=" + "<%: (int)Model.FactorDataType %>";
            baseUrl += "&childFactorId=" + childFactorId;
            baseUrl += "&referenceId=" + "<%: Model.ReferenceId %>";
            //baseUrl += "&individualCategoryId=" + individualCategory;
            var url = baseUrl + "&individualCategoryId=" + individualCategory + "&hostId=" + hostId + "&mainParentFactorId=" + mainParentFactorId;


            // Load the form into the dialog div
            loadDialog(dialogDiv, url);


        }


        function wireUpEditFactorForm(dialog) {
            $('form', dialog).submit(function () {

                // Do not submit if the form
                // does not pass client side validation
                if (!$(this).valid())
                    return false;
              
                // Client side validation passed, submit the form
                // using the jQuery.ajax form
                $.ajax({
                    url: this.action,
                    type: this.method,
                    data: $(this).serialize(),
                    success: function (result) {
                        // Check whether the post was successful
                        if (result.success) {
                            $(dialog).dialog('close');
                            window.location.reload();
                        } else {
                            // Reload the dialog to show model errors                    
                            $(dialog).html(result);

                            // Enable client side validation
                            $.validator.unobtrusive.parse(dialog);

                            // Setup the ajax submit logic
                            wireUpEditFactorForm(dialog);
                        }
                    }
                });
                 return false;
            });
        }


    </script>

        
</asp:Content>