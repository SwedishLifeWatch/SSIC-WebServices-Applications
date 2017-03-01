<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.SpeciesFactHostViewModel>" %>
<%@ Import Namespace="System.Security.Policy" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.Labels.TitleLabel%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="readHeader"><%: Model.Labels.TitleLabel%></h1>    
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
        
        
           
        
        <% using (Html.BeginForm("EditHostFactors", "SpeciesFact", FormMethod.Post, new { @id = "editHostTaxonAndFactorForm", @name = "editHostTaxonAndFactorForm" }))
       { %>
       
    
            <%: Html.HiddenFor(model => model.TaxonId) %>
            <%: Html.HiddenFor(model => model.MainParentFactorId) %>
            <%: Html.HiddenFor(model => model.FactorDataType) %>
            <%: Html.HiddenFor(model => model.DataType) %>
            <%: Html.HiddenFor(model => model.ReferenceId) %>
            <%: Html.Hidden("downloadTokenValue")%>
    <fieldset>
                
       <h2 class="open"><%: Model.Labels.FastTitleLabel%></h2>
       <div class="fieldsetContent">
              
<% if (Model.SpeciesFactViewModel.SpeciesFactViewModelHeaderItemList != null)
{
    for (int i = 0; i < Model.SpeciesFactViewModel.SpeciesFactViewModelHeaderItemList.Count; i++)
    { %>
        <% SpeciesFactViewModelItem headerItem = Model.SpeciesFactViewModel.SpeciesFactViewModelHeaderItemList[i].SpeciesFactViewModelItem; %>
        <% IList<SpeciesFactViewModelSubHeaderItem> subHeaderList = Model.SpeciesFactViewModel.SpeciesFactViewModelHeaderItemList[i].SpeciecFactViewModelSubHeaderItemList; %>
        <fieldset>
        <%--<h2 class="open"><%: headerItem.MainHeader%></h2>--%>
        <div class="fieldsetContent">   
                
        <% for (int j = 0; j < subHeaderList.Count; j++)
        { %>
            <% SpeciesFactViewModelItem subHeaderItem = subHeaderList[j].SpeciesFactViewModelItem; %>
            <% IList<SpeciesFactViewModelItem> factList = subHeaderList[j].SpeciesFactViewModelItemList; %>
                    
                <% if (subHeaderItem.IsSubHeader)
                { %>
                <%--<h3 class="open"><%: subHeaderItem.SubHeader%></h3>--%>
                     
                <% } %>
                <%
                else
                { %>
                    <%--<h3 class="open" style="background-color:azure"><%: headerItem.MainHeader%></h3>--%>       
            <%  } %>

                <div class="fieldsetSubContent fullWidth">                       
                        <% if (factList != null && factList.Count > 0)
                        { %>   
                            <%  var grid = new WebGrid(source: factList, defaultSort: "Title", rowsPerPage: 100); %>
                            <div id="Div1" >                 
                                <%: grid.GetHtml(tableStyle: "editFactorTable", headerStyle: "head",alternatingRowStyle: "alt", htmlAttributes: new {id ="editFactorTableGrid"},
                                                 displayHeader: true,
                                                 columns: 
                                                    grid.Columns
                                                    (grid.Column("", "", style:"",  format: (item) =>
                                                                                        {
                                                                                            var factorItem = ((SpeciesFactViewModelItem)item.Value);
                                                                                            if (factorItem.IsShortList)
                                                                                                return "*";
                                                                                            return " ";
                                                                                        }
                                                                                    ),
                                                                                    grid.Column("FactorName", Model.SpeciesFactViewModel.Labels.SpeciesFactMainHeader, style: "factorName", format: (item) =>
                                                                                                {
                                                                                                    var factorItem = ((SpeciesFactViewModelItem)item.Value);
                                                                                                    if (factorItem.IsSuperiorHeader || factorItem.IsHeader)
                                                                                                    return Html.Raw(string.Format("<strong>{0}</strong>", factorItem.SuperiorHeader));
                                                                                                    else
                                                                                                    return Html.RenderSpeciesFactHostName(factorItem.FactorName, factorItem.UseDifferentColor, factorItem.UseDifferentColorFromIndex);
                                                                                                }),   
                                                    grid.Column("FactorFieldValue", Model.SpeciesFactViewModel.FactorFieldValueTableHeader
                                                                                                    , style: "factorValue",
                                                                                format: (item) => 
                                                                                    {
                                                                                        var factorItem = ((SpeciesFactViewModelItem)item.Value);

                                                                                        if (factorItem.FieldValues != null)
                                                                                        {
                                                                                            return Html.Partial("~/Views/SpeciesFact/HostFactorEnumValues.ascx", (SpeciesFactViewModelItem)item.Value);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            return factorItem.FactorFieldValue;
                                                                                        }
                                                                                    }),
                                                                        
                                                       grid.Column("FactorFieldValue2", Model.SpeciesFactViewModel.FactorFieldValue2TableHeader
                                                                                                    , style: "factorValue2",
                                                                                format: (item) => 
                                                                                    {
                                                                                        var factorItem = ((SpeciesFactViewModelItem)item.Value);

                                                                                        if (factorItem.FieldValues2 != null)
                                                                                        {
                                                                                            return Html.Partial("~/Views/SpeciesFact/HostFactorDropDownValues.ascx", (SpeciesFactViewModelItem)item.Value);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            return factorItem.FactorFieldValue2;
                                                                                        }
                                                                                    }),
                                                                             
                                                     grid.Column("IndividualCategoryName", Model.SpeciesFactViewModel.Labels.SpeciesFactCategoryHeader, style: "factorCategory"),
                                                                    //grid.Column("FactorFieldComment", Model.Labels.SpeciesFactCommentHeader, style: "factorComment"),
                                                    // grid.Column("Quality", Model.SpeciesFactViewModel.Labels.SpeciesFactQualityHeader,style: "factorQuality"),
                                                     grid.Column("Quality",Model.SpeciesFactViewModel.Labels.SpeciesFactQualityHeader,style: "factorQuality",
                                                                                    format: (item) => 
                                                                                        {
                                                                                            var factorItem = ((SpeciesFactViewModelItem)item.Value);

                                                                                            if (factorItem.QualityValues != null)
                                                                                            {
                                                                                                return Html.Partial("~/Views/SpeciesFact/HostQualityFactorDropDownValues.ascx", (SpeciesFactViewModelItem)item.Value);
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                return factorItem.Quality;
                                                                                            }
                                                                                        }),
                                                     grid.Column("", "", style: "factorChange", format: (item) =>
                                                                                {
                                                                                    var factorItem = ((SpeciesFactViewModelItem)item.Value);
                                                                                    if (factorItem.FieldValues != null )
                                                                                    {

                                                                                        return Html.Raw("<a href=\"#\" " + "onclick=\"showCreateNewReferenceDialog(" + factorItem.FactorId + "," + factorItem.ReferenceId + "," + factorItem.IndividualCategoryId + "," + factorItem.HostId + "," + factorItem.MainParentFactorId + "," + factorItem.IndividualCategoryId + ");\">" + Resources.DyntaxaResource.SpeciesFactChangeFactorText + "</a>");
                                                                        
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        return string.Empty;
                                                                                    }
                                                                                })

                                                                    //    , grid.Column("FactorId", "Id", style: "factorId")
                                                                   // ,grid.Column("FactorSortOrder", Model.SpeciesFactViewModel.Labels.SpeciesFactSortOrder, style: "factorSortOrder")
                                            )
                                        ) %>
                                 </div>
                 <% } %>
               
                </div>
            <% } %>
                    
        </div>
        </fieldset>
    <%}
 } %> 
                      
        <button id="editHostTaxonAndFactorForm"  type="submit" class="ap-ui-button"><%: Model.SpeciesFactViewModel.Labels.SaveNewValuesLabel %></button>      
        <%: Html.ActionLink(Model.Labels.ResetLabelFast, "EditHostFactorsForSubstrate", new { taxonId = Model.TaxonId, referenceId = Model.ReferenceId, reset = true}, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>
     
              
              

          
    
       </div>    
           
     </fieldset>  
     <% } %> 
        
     <% using (Html.BeginForm(Model.PostAction, "SpeciesFact", FormMethod.Post, new { @id = "editHostFactorForm", @name = "editHostFactorForm" }))
      { %>
      <%: Html.HiddenFor(model => model.TaxonId) %>
      <%: Html.HiddenFor(model => model.FactorId) %>
      <%: Html.HiddenFor(model => model.ReferenceId) %>
      <%: Html.HiddenFor(model => model.IndividualCategoryId) %>
      <%: Html.HiddenFor(model => model.MainParentFactorId) %>
      <%: Html.HiddenFor(model => model.FactorDataType) %>
      <%: Html.HiddenFor(model => model.DataType) %>
      <%: Html.Hidden("downloadTokenValue")%>
     
             <fieldset id="editHostTaxaFieldset">
                
                 <h2 class="open"><%: Model.Labels.TitleLabel%></h2>
                <div class="fieldsetContent">   
                  <%--<div class="group" style="margin-left: 10px; ">
                        <%: Html.LabelFor(model => model.IndividualCategoryId)%>
                         <select id="DropDownCategoryId" name="DropDownCategoryId" style="width: 300px;">
                             <% foreach (var item in Model.IndividualCategoryList)
                                {
                                   
                                     %>
                                    <option value="<%=item.Id%>"><%: item.Text %></option>
                                    <%
                                                                   
                                } %>                          
                        </select> 
                     </div>  --%>
                          <h3 class="open"><%: Model.Labels.ExistingDataLabel%></h3>       
                     
                        <div class="fieldsetSubContent fullWidth">
                         
                          <div style="display: inline-block; vertical-align: top; margin-left: 5px;">               
                          <% IList<SpeciesFactHostViewModelItem> hostList = Model.HostTaxonList; %>
                             <% if (Model.HostTaxonList.Count > 0)
                            { %>   
                                <%  var grid = new WebGrid(source: hostList,
                                                defaultSort: "Title",
                                                rowsPerPage: 100); 
                                            %>
                                        <div id="grid" >
                                            <%: grid.GetHtml(tableStyle: "editHostTaxonTable", headerStyle: "head",alternatingRowStyle: "alt", 
                                                displayHeader: true,
                                                columns: 
                                                    grid.Columns(grid.Column("   ", ""
                                                                                                        , style: "editHostChecked",
                                                                                    format: (item) => 
                                                                                        {
                                                                                            
                                                                                            
                                                                                            
                                                                                           return Html.Partial("~/Views/SpeciesFact/FactorCheckboxValues.ascx", (SpeciesFactHostViewModelItem)item.Value);
                                                                                           
                                                                                        }),
                                                                  grid.Column("Name", "Taxonnamn", style: "editHostName", format: (item) =>
                                                                                                    {
                                                                                                        var taxonItem = ((SpeciesFactHostViewModelItem)item.Value);
                                                                                                        return Html.Raw(string.Format("<strong>{0}</strong>", taxonItem.Name));
                                                                                                    })
                                                                           , grid.Column("categryname", "Kategorinamn", style: "editHostCategory",format: (item) =>
                                                                                                    {
                                                                                                        var taxonItem = ((SpeciesFactHostViewModelItem)item.Value);
                                                                                                        return Html.Raw(string.Format(taxonItem.CategoryName));
                                                                                                    })

                                                                        // , grid.Column("TaxonId", "Taxonid", style: "editHostId")
                                                                          , grid.Column("factorname", "Faktornamn", style: "editHostfactor",format: (item) =>
                                                                                                    {
                                                                                                        var taxonItem = ((SpeciesFactHostViewModelItem)item.Value);
                                                                                                        return Html.Raw(string.Format(taxonItem.IsHostToFactor));
                                                                                                    }),   
                                                                   
                                                                        grid.Column("       ", "", style: "editHostRemove", format: (item) =>
                                                                                    {
                                                                                        var taxonItem = ((SpeciesFactHostViewModelItem)item.Value);
                                                                                        return Html.Raw("<a href=\"#\" " + "onclick=\"showDeleteHostTaxonDialog(" + taxonItem.TaxonId + "," + taxonItem.FactorId + "," + taxonItem.CategoryId + "," + taxonItem.IsHostToFactorId + ");\">" + Model.Labels.SpeciesFactRemoveItemText + "</a>");
                                                                        
                                                                                       
                                                                                    })
                                                                        //grid.Column("FactorSortOrder", Model.Labels.SpeciesFactSortOrder, style: "factorSortOrder")
                                                )
                                    ) %>
                                            
                                                
                               </div>

                            
                             <% } %>
                       </div>
                         <div style="display: inline-block; vertical-align: top; margin-left: 5px;">
                             <% IList<SpeciesFactHostViewModelItem> factorList = Model.HostFactorList; %>
                             <% if (Model.HostFactorList.Count > 0)
                            { %>   
                                <%  var grid2 = new WebGrid(source: factorList,
                                                defaultSort: "Title",
                                                rowsPerPage: 100); 
                                            %>
                                        <div id="grid2" >
                                            <%: grid2.GetHtml(tableStyle: "editHostFactorTable", headerStyle: "head",alternatingRowStyle: "alt", 
                                                displayHeader: true,
                                                columns: 
                                                    grid2.Columns(grid2.Column("   ", ""
                                                                                                        , style: "editHostChecked",
                                                                                    format: (item) => 
                                                                                        {
                                                                                            
                                                                                            
                                                                                            
                                                                                           return Html.Partial("~/Views/SpeciesFact/FactorCheckboxValues.ascx", (SpeciesFactHostViewModelItem)item.Value);
                                                                                           
                                                                                        }),
                                                                  grid2.Column("Name", "Faktornamn", style: "editHostName", format: (item) =>
                                                                                                    {
                                                                                                        var factorItem = ((SpeciesFactHostViewModelItem)item.Value);
                                                                                                        return Html.Raw(string.Format("<strong>{0}</strong>", factorItem.Name));
                                                                                                    })

                                                                       //  , grid2.Column("FactorId", "Faktorid", style: "editHostId"),   
                                                                   
                                                                        ,grid2.Column("       ", "", style: "editHostRemove", format: (item) =>
                                                                                    {
                                                                                        var factorItem = ((SpeciesFactHostViewModelItem)item.Value);
                                                                                       
                                                                                        return Html.Raw("<a href=\"#\" " + "onclick=\"showDeleteHostFactorDialog(" + factorItem.FactorId + "," + factorItem.CategoryId + ");\">" + Model.Labels.SpeciesFactRemoveItemText + "</a>");
                                                                        
                                                                                       
                                                                                    })
                                                                        //grid.Column("FactorSortOrder", Model.Labels.SpeciesFactSortOrder, style: "factorSortOrder")
                                                )
                                    ) %>
                                            
                                                
                               </div>
                             <% } %>
                             
                        </div>   
                            
                                                        
                    </div>
                    
                    <div>
                         <button id="saveEditHostFactorForm"  style="margin-right: 10px;" type="submit" class="ap-ui-button"><%: Model.Labels.SpeciesFactSaveSettings %></button>      
                         <%: Html.ActionLink(Model.Labels.ResetLabel, "EditHostFactorsForSubstrate", new { taxonId = Model.TaxonId, referenceId = Model.ReferenceId, categoryId = Model.IndividualCategoryId, reset = true}, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>
                        <button id="showEditHostFactors" type="button" class="ap-ui-button"  style="margin-top: 10px;" onclick="showEditHostFactorsDialog();" ><%: Model.Labels.SubstrateChangeMany %></button>                                               
                    </div> 
                
               </div>
               </fieldset>
           
    <% } %> 
        
        <fieldset>
         <h2 class="open"><%: Model.Labels.AddLabel%></h2>
             <div class="fieldsetContent">
                  <div style="display: inline-block; vertical-align: top; margin-left: 5px;">
        <% using (Html.BeginForm("AddHostTaxonAndFactor", "SpeciesFact", FormMethod.Post, new { @id = "addHostTaxonAndFactorForm", @name = "addHostTaxonAndFactorForm" }))
       { %>
       
    
            <%: Html.HiddenFor(model => model.TaxonId) %>
            <%: Html.HiddenFor(model => model.FactorId) %>
            <%: Html.HiddenFor(model => model.ReferenceId) %>
            <%: Html.HiddenFor(model => model.IndividualCategoryId) %>
            <%: Html.HiddenFor(model => model.MainParentFactorId) %>
             <%: Html.HiddenFor(model => model.FactorDataType) %>
            <%: Html.HiddenFor(model => model.DataType) %>
            <%: Html.Hidden("downloadTokenValue")%>

            <div>
                 
                     <div style="display: inline-block; vertical-align: top; padding: 15px;">
                         <div class="group">
                            <%: Html.LabelFor(model => model.FactorId)%>
                             <select id="Select2" name="DropDownHostFactorId" style="width: 300px; margin-top: 5px;">
                                 <% foreach (var item in Model.AddFactorToHostList)
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
                         
                 </div>

                  <div style="display: inline-block; vertical-align: bottom; padding: 15px; ">
                     <div class="group" style="margin-left: 10px; ">
                        <%: Html.LabelFor(model => model.TaxonId)%>
                         <select id="Select1" name="DropDownTaxonId" style="width: 300px; margin-top: 5px;">
                             <% foreach (var item in Model.AddTaxonToHostList)
                                {
                                   
                                     %>
                                    <option value="<%=item.Id%>">&nbsp;&nbsp;&nbsp;<%: item.Text %></option>
                                    <%
                                                                   
                                } %>                          
                        </select> 
                     </div>    
                        <div>
                            <button id="showSearchTaxon" type="button" class="ap-ui-button" style="margin-top:15px;  margin-bottom:15px;" onclick="showSearchTaxonDialog()"><%: Model.Labels.SearchTaxon %></button>     
                       </div>
                        <%--<%:Html.DropDownListFor(model => model.DropDownFactorId, new SelectList(Model.FactorList, "Id", "Text", Model.DropDownFactorId),new {style="width: 300px;"})%>                               --%>
                    </div>                        
                    
                
                  <div style="display: inline-block; vertical-align: top; margin-left: 5px;  margin-top: 37px;">
                        <button id="addHostTaxonAndFactorButton" type="button" class="ap-ui-button" style="margin:0px;" ><%: Model.Labels.AddButtonLabel %></button>                                               
                     </div>
              
               
            </div>
            
              
    <% } %> 
      </div>   
    
       </div>    
           
     </fieldset>  
        
     
            
              
   
     
   </div>      
    <%--  <fieldset>  
        <button id="saveEditHostFactorForm" form="editHostFactorForm" type="submit" class="ap-ui-button"><%: Model.Labels.SaveLabel %></button>      
        <%: Html.ActionLink(Model.Labels.ResetLabel, "EditHostFactorsForSubstrate", new { taxonId = Model.TaxonId, referenceId = Model.ReferenceId, reset = true}, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>
      </fieldset>--%>
   

    
   
    <div id="dialog-confirm" style="display:none" title="<%:Model.Labels.SpeciesFactDeleteFactorItemDialogHeader%>" >
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 20px 0;"></span><%: Model.Labels.SpeciesFactDeleteFactorItemDialogText %></p>
   </div>  
     <div id="dialog-confirm-taxon" style="display:none" title="<%:Model.Labels.SpeciesFactDeleteTaxonItemDialogHeader%>" >
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 20px 0;"></span><%: Model.Labels.SpeciesFactDeleteTaxonItemDialogText %></p>
   </div>  
     <div id="dialog-confirm-host-factor" style="display:none" title="<%:Model.Labels.SpeciesFactInvalidSelectionDialogHeader%>" >
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 20px 0;"></span><%: Model.Labels.SpeciesFactInvalidSelectionDialogText %></p>
   </div>
    <div id="dialog-confirm-no-host-no-factor" style="display:none" title="<%:Model.Labels.SpeciesFactNoItemsSelectedDialogHeader%>" >
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 20px 0;"></span><%: Model.Labels.SpeciesFactNoItemsSelectedDialogText %></p>
   </div>       
    <div id="dialog-confirm-no-host-selected" style="display:none" title="<%:Model.Labels.SpeciesFactNoHostsItemsSelectedDialogHeader%>" >
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 20px 0;"></span><%: Model.Labels.SpeciesFactNoHostItemsSelectedDialogText %></p>
   </div>
    
     <div id="dialog-confirm-no-saved" style="display:none" title="<%:Model.Labels.SpeciesFactNotSavedDialogHeader%>" >
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 20px 0;"></span><%: Model.Labels.SpeciesFactNotSavedDialogText %></p>
   </div>
    
    <div id="dialog-search-taxon" style="display:none" />        
       
<%--<div id="dialog-search-taxon" style="display:none" title="<%:Model.Labels.SpeciesFactNotSavedDialogHeader%>" >
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 20px 0;"></span><%: Model.Labels.SpeciesFactNotSavedDialogText %></p>
   </div>    --%>

    <script type="text/javascript">
        var origStrCheckBoxesStates;
        var dataSaved = false;
        var fileDownloadCheckTimer;
        
        function blockUIForDownload(isDelete) {
            var token = new Date().getTime(); //use the current timestamp as the token value
            var displayMessage = '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SaveLabel %></h1>';
            if (isDelete) {
                displayMessage = '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.SpeciesFactRemoveItemText %></h1>';
            }
            $('#downloadTokenValue').val(token);
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: displayMessage });
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
            
           
           $('#editFactorTableGrid tr td:first-child').filter(':contains("*")').each(function () {
                $(this).closest('tr').find("td").css("background", "#98FB98");
                $(this).closest('tr').find("td").css("color", "#000000");
            });

            $("#editHostFactorForm").submit(function () {
                blockUIForDownload();
            });
            $("#editHostTaxonAndFactorForm").submit(function () {
                blockUIForDownload();
            });

            registerHandlers();
            //$('#addHostFactorButton').attr('Color', 'grey');
            //$('#addHostTaxonButton').attr('Color', 'grey');
            //$('#showEditHostFactorsDialog').attr('Color', 'grey');
            //$('#addHostFactorButton').attr('disabled', 'disabled');
            //$('#addHostTaxonButton').attr('disabled', 'disabled');
            //$('#showEditHostFactors').attr('disabled', 'disabled');
            function registerHandlers() {
               

                $('input[type="checkbox"]').change(function () {
                       dataSaved = false;
                   
                    //$('#addHostFactorButton').attr('disabled','disabled');
                    //$('#addHostTaxonButton').attr('disabled','disabled');
                    //$('#showEditHostFactors').attr('disabled', 'disabled');
                    //$('#addHostFactorButton').attr('Color', 'grey');
                    //$('#addHostTaxonButton').attr('Color', 'grey');
                    //$('#showEditHostFactors').attr('Color', 'grey');
                    
                });
            }
            
          
          
            
            
            $('#addHostTaxonAndFactorButton').click(function () {                
                var val = $("#Select1").val();
                var val2 = $("#Select2").val();
                var isTaxonChecked = isItemChecked("taxon");
                var isTaxonSelected = true;
                if (val == -1000) {
                    isTaxonSelected = false;
                    
                }
                var isFactorChecked = isItemChecked("factor");
                var isFactorSelected = true;
                if (val2 == 0) {
                    

                    isFactorSelected = false;
                }
                var okToContinue = false;
                if ((isFactorSelected && isTaxonChecked) && (isTaxonSelected && isFactorChecked)) {
                    okToContinue = true;

                }
                else if ((isFactorSelected && isTaxonChecked) && (!isTaxonSelected && !isFactorChecked)) {
                    okToContinue = true;

                }
                else if ((isTaxonSelected && isFactorChecked) && (!isFactorSelected && !isTaxonChecked)) {
                    okToContinue = true;

                }
                else if ((isTaxonSelected && isFactorSelected) && (!isFactorChecked && !isTaxonChecked)) {
                    okToContinue = true;

                }
                
                if (val == -1000 && val2 == 0) {
                    
                }
                else if (!okToContinue && origStrCheckBoxesStates == createPageStateString()) {
                    $("#dialog-confirm-host-factor").dialog({
                        resizable: false,
                        height: 160,
                        width: 450,
                        modal: true,
                        buttons: {

                            "<%: Model.Labels.ContinueLabel %>": function () {
                                $(this).dialog("close");
                                return;
                            }
                        }
                    });

                }
                else {
                    var currentCheckBoxesStatesString = createPageStateString();
                        if (origStrCheckBoxesStates != currentCheckBoxesStatesString) {
                            $("#dialog-confirm-no-saved").dialog({
                                resizable: false,
                                height: 160,
                                width: 450,
                                modal: true,
                                buttons: {

                                    "<%: Model.Labels.ContinueLabel %>": function () {
                                    $(this).dialog("close");
                                    return;
                                }
                            }
                        });
                        }
                        else {
                        $("#addHostTaxonAndFactorForm").submit();

                        }
  
                }
               
                

             });


            $('#Select1').select2();
            $('#Select2').select2();

            origStrCheckBoxesStates = createPageStateString();
        });        

        
        var dialog;

        
    

        function loadDialog(dialogDiv, url, baseUrl, baseHostId, baseMainParentFactorId, intIndividualCatBox) {
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.LoadingLabel %></h1>' });

            $(dialogDiv).load(url, function () {
                dialog = $(this).dialog({
                    modal: true,
                    width: "550px",
                    resizable: false,
                    draggable: true,
                    zIndex: 999999,
                    title: "<%: Model.Labels.HostPopUpLabel %>",
                    buttons: {
                        "<%: Model.Labels.SaveLabel %>": function () {
                            var form = $('form', this);
                            $(form).submit();
                        },
                        "<%: Model.Labels.CancelLabel %>": function () { $(this).dialog('close'); }
                    },
                    open: function (event, ui) {
                        $.unblockUI();
                        if (intIndividualCatBox) {
                            initIndividualCategorySelectBox(dialogDiv, baseUrl, baseHostId, baseMainParentFactorId);
                        }
                        // Enable client side validation
                        $.validator.unobtrusive.parse(dialogDiv);

                        // Setup the ajax submit logic
                        wireUpEditFactorForm(dialogDiv);
                    }
                });
            });

        }

        function createPageStateString() {
        
            return createCheckboxStatesString();
        }

        
        function createCheckboxStatesString() {
            var $checkBoxes = $('input[type=checkbox]');
            var strCheckboxesStates = "";
            $checkBoxes.each(function (index, value) {
                var name = $(value).attr('name');
                var isChecked = $(value).attr('checked') == 'checked';
                strCheckboxesStates += name + "_";
                var checked = "false";
                if (isChecked) {
                    checked = "true";
                }
                
                strCheckboxesStates += checked + "_";
            });
            return strCheckboxesStates;
        }
        
        function createCheckboxDropDownCorrectSelectedString() {
            var $checkBoxes = $('input[type=checkbox]');
            var strCheckboxesStates = "";
            var taxonChecked = false;
            var factorChecked = false;
            var taxonCheckedList = new Array();
            var factorCheckedList = new Array();
            $checkBoxes.each(function (index, value) {
                var itemIsFactor = false;
                var name = $(value).attr('name');
               
                if ($(value).attr('name').indexOf('factor') > -1) {
                    itemIsFactor = true;
                }
                var isChecked = $(value).attr('checked') == 'checked';
                strCheckboxesStates += name + "_";
                var checked = "false";
                if (isChecked) {
                    if (itemIsFactor) {
                        factorChecked = true;
                        factorCheckedList.push(name);

                    } else {
                        taxonChecked = true;
                        taxonCheckedList.push(name);
                    }
                    checked = "true";
                }

                strCheckboxesStates += checked + "_";
            });
            //var factorExist = false;
            //var match = true;
            //taxonCheckedList.forEach(function (value) {

            //    factorCheckedList.forEach(function (value2) {
            //        var index = value2.lastIndexOf("_");
            //        var len = value2.length - 1;
            //        var test = value2.substring(index+1, len);
            //        if (value.contains(test)) {
            //            factorExist = true;

            //        }
            //    });
            //    if (!factorExist) {
            //        match = false;

            //    }
            //});
            //// One to one match
            //if (factorChecked && taxonChecked && match) {
            //    return true;
            //}
            ////Create new factors
            //if (taxonChecked && match) {
            //    return true;

            //}
            if (taxonChecked) {
                return true;

            }
            return false;
        }
        function isItemChecked(item) {
            var $checkBoxes = $('#editHostTaxaFieldset input[type=checkbox]');
            var taxonChecked = false;
            var factorChecked = false;
            $checkBoxes.each(function (index, value) {                
                var itemIsFactor = false;                
                var name = $(value).attr('name');                
                if ($(value).attr('name').indexOf('factor') > -1) {
                    itemIsFactor = true;
                }
                var isChecked = $(value).attr('checked') == 'checked';
                if (isChecked) {
                    if (itemIsFactor) {
                        factorChecked = true;


                    } else {
                        taxonChecked = true;

                    }

                }


            });

            if (taxonChecked &&  (item =="taxon")) {
                return true;

            }
            if (factorChecked && (item =="factor")) {
                return true;

            }
            return false;
        }

        function createCheckboxCorrectSelectedString() {
            var $checkBoxes = $('input[type=checkbox]');
           var taxonChecked = false;
            var factorChecked = false;
           $checkBoxes.each(function (index, value) {
                var itemIsFactor = false;
                var name = $(value).attr('name');
                if ($(value).attr('name').indexOf('factor') > -1) {
                    itemIsFactor = true;
                }
                var isChecked = $(value).attr('checked') == 'checked';
                if (isChecked)
                {
                    if (itemIsFactor) {
                        factorChecked = true;
                      
                        
                    } else {
                        taxonChecked = true;
                        
                    }
                   
                }

             
            });

            if (taxonChecked || factorChecked) {
                return true;

            }
            return false;
        }

        function createCheckboxFactorSelectedString(hostFactorId) {
            var $checkBoxes = $('input[type=checkbox]');
            var taxonChecked = false;
            var factorChecked = false;
            $checkBoxes.each(function (index, value) {
                var itemIsFactor = false;
                var name = $(value).attr('name');
                if ($(value).attr('name').indexOf('factor') > -1) {
                    itemIsFactor = true;
                }
                var isChecked = $(value).attr('checked') == 'checked';
                if (isChecked) {
                    if (itemIsFactor) {
                       

                    } else {
                        if ($(value).attr('name').indexOf(hostFactorId) > -1) 
                            // if (name.contains(hostFactorId)) 
                        {
                            taxonChecked = true;
                        }

                    }

                }


            });

            if (taxonChecked) {
                return true;

            }
            return false;
        }
        
        var baseUrl;
        var baseHostId;
        var baseMainParentFactorId;
        
        function showDeleteHostTaxonDialog(hostTaxonId, factorId, categoryId, hostFactorId) {
            //int hostTaxonId, int factorId, int taxonId, int referenceId, int categoryId,int hostFactorId
           
            var dialogDiv = $("<div></div>");
            var url = '<%= Url.Action("DeleteHostTaxonItem","SpeciesFact") %>';
            url += "?hostTaxonId=" + hostTaxonId;
            url += "&taxonId=" + "<%: Model.TaxonId%>";
            url += "&factorId=" + factorId;
            url += "&categoryId=" + categoryId;
            url += "&referenceId=" + "<%: Model.ReferenceId%>";
            url += "&hostFactorId=" + hostFactorId;
            
            $("#dialog-confirm-taxon").dialog({
                resizable: false,
                height: 160,
                width: 450,
                modal: true,
                buttons: {
                    "<%: Model.Labels.YesLabel %>": function () {
                                $(this).dialog("close");
                                submitForm(url, true);
                            },
                            "<%: Model.Labels.NoLabel %>": function () {
                                $(this).dialog("close");
                                return;
                            }
                        }
             });
            return;


        }
        function showDeleteHostFactorDialog(hostFactorId, categoryId) {
            //DeleteHostFactorItem(int factorId, int taxonId, factorId, int referenceId, int categoryId)
            var currentCheckBoxesStatesString = createPageStateString();
            if (origStrCheckBoxesStates != currentCheckBoxesStatesString) {
                $("#dialog-confirm-no-saved").dialog({
                    resizable: false,
                    height: 160,
                    width: 450,
                    modal: true,
                    buttons: {

                        "<%: Model.Labels.ContinueLabel %>": function () {
                            $(this).dialog("close");
                            return;
                        }
                    }
                });
            } else {


               
                    var dialogDiv = $("<div></div>");
                    url = '<%= Url.Action("DeleteHostFactorItem","SpeciesFact") %>';
                    url += "?hostFactorId=" + hostFactorId;
                    //url += "&hostTaxonId=" + taxonId;
                    url += "&taxonId=" + "<%: Model.TaxonId%>";
                    url += "&factorId=" + "<%: Model.FactorId%>";
                    url += "&referenceId=" + "<%: Model.ReferenceId%>";
                    url += "&categoryId=" + categoryId;

                    $("#dialog-confirm").dialog({
                        resizable: false,
                        height: 160,
                        width: 450,
                        modal: true,
                        buttons: {
                            "<%: Model.Labels.YesLabel %>": function () {
                                $(this).dialog("close");
                                submitForm(url, true);
                            },
                            "<%: Model.Labels.NoLabel %>": function () {
                                $(this).dialog("close");
                                return;
                            }
                        }
                    });
                    return;

               
            }
          

        }
        
        function submitForm(url, isDelete) {
            // Do not submit if the form
            // does not pass client side validation
            if (!$("#editHostFactorForm").valid())
                return false;
            $("#editHostFactorForm").attr("action", url);
            blockUIForDownload(isDelete);
            dataSaved = true;
           
            //$('#addHostFactorButton').removeAttr('disabled');
            //$('#addHostTaxonButton').removeAttr('disabled');;
            //$('#showEditHostFactors').removeAttr('disabled');
            //$('#addHostFactorButton').attr('Color', 'black');
            //$('#addHostTaxonButton').attr('Color', 'black');
            //$('#showEditHostFactors').attr('Color', 'black');
            $("#editHostFactorForm").submit();

       
            return false;
        };
        
        function submitHostForm(url) {
            // Do not submit if the form
            // does not pass client side validation
            if (!$("#editHostTaxonAndFactorForm").valid())
                return false;
            $("#editHostTaxonAndFactorForm").attr("action", url);
            blockUIForDownload(false);
            dataSaved = true;

            //$('#addHostFactorButton').removeAttr('disabled');
            //$('#addHostTaxonButton').removeAttr('disabled');;
            //$('#showEditHostFactors').removeAttr('disabled');
            //$('#addHostFactorButton').attr('Color', 'black');
            //$('#addHostTaxonButton').attr('Color', 'black');
            //$('#showEditHostFactors').attr('Color', 'black');
            $("#editHostTaxonAndFactorForm").submit();


            return false;
        };
        
        function showSearchTaxonDialog() {
            //alert("Test Sök ut taxon här!!!");

            baseUrl = '<%= Url.Action("SearchTaxonPartial","Taxon") %>';
           
            var url = baseUrl;


            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.LoadingLabel %></h1>' });

            
            $('#dialog-search-taxon').load(url, function () {
                dialog = $(this).dialog({
                    modal: true,
                    width: 700,
                    //minHeight: 300,
                    height: 500,
                    resizable: false,
                    draggable: true,
                    //zIndex: 999999,
                    title: "<%: Resources.DyntaxaResource.TaxonSearchTitle %>",
                    buttons: {                        
                        "<%: Model.Labels.ContinueLabel %>": function () { $(this).dialog('close'); }
                    },
                    open: function (event, ui) {
                        $.unblockUI();                        
                    }
                });
            });


        }
        
        function showEditHostFactorsDialog() {
            var currentCheckBoxesStatesString = createPageStateString();
            if (origStrCheckBoxesStates != currentCheckBoxesStatesString) {
                $("#dialog-confirm-no-saved").dialog({
                    resizable: false,
                    height: 160,
                    width: 450,
                    modal: true,
                    buttons: {

                        "<%: Model.Labels.ContinueLabel %>": function () {
                            $(this).dialog("close");
                            return;
                        }
                    }
                });
            }
            else {
                var selectionOk = createCheckboxCorrectSelectedString();
                if (selectionOk) {
                    //var statusString = createCheckboxStatesString();
                    var dialogDiv = $("<div></div>");
                    baseUrl = '<%= Url.Action("EditHostFactorItems","SpeciesFact") %>';
                    baseUrl += "?taxonId=" + "<%: Model.TaxonId%>";
                    baseUrl += "&referenceId=" + "<%: Model.ReferenceId%>";
                    //baseUrl += "&individualCategoryId=" + individualCategory;
                    var url = baseUrl;

                    // Load the form into the dialog div
                    loadDialog(dialogDiv, url, null, null, null, false);
                    return;

                }
                $("#dialog-confirm-no-host-no-factor").dialog({
                    resizable: false,
                    height: 160,
                    width: 450,
                    modal: true,
                    buttons: {

                        "<%: Model.Labels.ContinueLabel %>": function () {
                            $(this).dialog("close");
                            return;
                        }
                    }
                });
            }


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
        

      //  var dialog2;

        //function initIndividualCategorySelectBox2() {
        //    $("#IndividualCategoryId").change(function () {
        //        var val = $(this).val();
        //        var url2 = baseUrl2 + "&individualCategoryId=" + val + "&hostId=" + baseHostId2 + "&mainParentFactorId=" + baseMainParentFactorId2;

        //        $.get(url2, function (data) {
        //            dialog.html(data);
        //            initIndividualCategorySelectBox2();
        //        });
        //    });
        //}
        $("#IndividualCategoryId").change(function () {
            var test = 0;
            var val = $(this).val();
            var form = $('form', this);
            var a = a;
            
        });
        
        function initIndividualCategorySelectBox(dialogDiv, baseUrl, baseHostId, baseMainParentFactorId) {
            $(dialogDiv).find("#IndividualCategoryId").change(function () {                
                var val = $(this).val();
                var oldCat = $("#oldIndividualCategoryId").val();
                if (oldCat == null) {
                    oldCat = "10";
                }
                var url = baseUrl + "&individualCategoryId=" + val + "&hostId=" + baseHostId + "&mainParentFactorId=" + baseMainParentFactorId + "&oldIndividualCategoryId=" + oldCat;
                $.unblockUI();
                $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.LoadingLabel %></h1>' });

                $.get(url, function (data) {

                    dialog.html(data);
                    // $.unblockUI();
                    initIndividualCategorySelectBox(dialogDiv, baseUrl, baseHostId, baseMainParentFactorId);
                    $.unblockUI();
                });                

            });
        }

        function loadDialog2(dialogDiv, url) {
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
                        wireUpEditFactorForm2(dialogDiv);
                    }
                });
            });

        }
        function wireUpEditFactorForm2(dialog) {
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
                            wireUpEditFactorForm2(dialog);
                        }
                    }
                });
                return false;
            });
        }
        
        var baseUrl2;
        var baseHostId2;
        var baseMainParentFactorId2;

        function showCreateNewReferenceDialog(childFactorId, referenceId, individualCategory, hostId, mainParentFactorId, oldIndividualCategory) {
            //string taxonId, string dataType, string factorDataType, string childFactorId, string referenceId, string individualCategoryId, string hostId, string mainParentFactorId
            baseHostId2 = hostId;
            baseMainParentFactorId2 = mainParentFactorId;
            baseMainParentFactorId2 = "<%: Model.MainParentFactorId%>";
            var dialogDiv = $("<div></div>");
            baseUrl2 = '<%= Url.Action("EditHostFactorItem","SpeciesFact") %>';
            baseUrl2 += "?taxonId=" + "<%: Model.TaxonId %>";
            baseUrl2 += "&dataType=" + "<%: (int)Model.DataType %>";
            baseUrl2 += "&factorDataType=" + "<%: (int)Model.FactorDataType %>";
            baseUrl2 += "&childFactorId=" + childFactorId;
            baseUrl2 += "&referenceId=" + "<%: Model.ReferenceId %>";
            var url2 = baseUrl2 + "&individualCategoryId=" + individualCategory + "&hostId=" + hostId + "&mainParentFactorId=" + baseMainParentFactorId2 + "&oldIndividualCategoryId=" + oldIndividualCategory;

            
            
            // Load the form into the dialog div
            loadDialog(dialogDiv, url2, baseUrl2, hostId, baseMainParentFactorId2, true);






        }


    </script>

        
</asp:Content>