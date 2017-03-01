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

    <% using (Html.BeginForm(Model.PostAction, "SpeciesFact", FormMethod.Post, new { @id = "speciesFactForm", @name = "speciesFactForm" }))
       { %>

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
      <%: Html.HiddenFor(model => model.TaxonId) %>
      <%: Html.Hidden("downloadTokenValue")%>
      
           
            <% for (int i = 0; i < Model.SpeciesFactViewModelHeaderItemList.Count; i++)
            { %>
              <% SpeciesFactViewModelItem headerItem = Model.SpeciesFactViewModelHeaderItemList[i].SpeciesFactViewModelItem; %>
              <% IList<SpeciesFactViewModelSubHeaderItem> subHeaderList = Model.SpeciesFactViewModelHeaderItemList[i].SpeciecFactViewModelSubHeaderItemList; %>
               <fieldset>
                <h2 class="closed savestate"><%: headerItem.MainHeader%></h2>
                <div class="fieldsetContent">   
                
                <% for (int j = 0; j < subHeaderList.Count; j++)
                { %>
                    <% SpeciesFactViewModelItem subHeaderItem = subHeaderList[j].SpeciesFactViewModelItem; %>
                    <% IList<SpeciesFactViewModelItem> factList = subHeaderList[j].SpeciesFactViewModelItemList; %>
                    
                     <%-- Get other parents ie not the main parent--%>
                     <% if (subHeaderItem.IsSubHeader)
                     { %>
                        <h3 class="closed"><%: subHeaderItem.SubHeader%></h3>
                     
                     <% } %>
                     <%
                        else
                        { %>
                         <h3 class="closed" style="background-color:azure"><%: headerItem.MainHeader%> </h3>       
                      <%  } %>
                        <div class="fieldsetSubContent fullWidth">                    
                         
                             <% if (factList.Count > 0)
                            { %>
                    
                                     <%-- Get other parents ie not the main parent--%>
                                    

                                   
                                              <%  var grid = new WebGrid(source: factList,
                                                             defaultSort: "Title",
                                                             rowsPerPage: 100); 
                                                         %>
                                                        <div id="grid" >
                          
                                                             <%: grid.GetHtml(tableStyle: "grid", headerStyle: "head",alternatingRowStyle: "alt",
                                                             displayHeader: true,
                                                                                        columns: 
                                                                                        grid.Columns(grid.Column("FactorName", Model.Labels.SpeciesFactMainHeader, style: "factorName",format: (item) =>
                                                                                                                 {
                                                                                                                     var factorItem = ((SpeciesFactViewModelItem)item.Value);
                                                                                                                     if (factorItem.IsSuperiorHeader)
                                                                                                                        return Html.Raw(string.Format("<strong>{0}</strong>", factorItem.SuperiorHeader));
                                                                                                                     else
                                                                                                                       return Html.RenderSpeciesFactHostName(factorItem.FactorName, factorItem.UseDifferentColor, factorItem.UseDifferentColorFromIndex);
                                                                                                                 }),   
                                                                                        grid.Column("FactorFieldValue", Model.Labels.SpeciesFactFactorValueHeader, style: "factorValue"),
                                                                                        grid.Column("IndividualCategoryName", Model.Labels.SpeciesFactCategoryHeader, style: "factorCategory"),
                                                                                        grid.Column("FactorFieldComment", Model.Labels.SpeciesFactCommentHeader, style: "factorComment"),
                                                                                        grid.Column("Quality", Model.Labels.SpeciesFactQualityHeader,style: "factorQuality"),
                                                                                        grid.Column("FactorId", Model.Labels.SpeciesFactFactorId, style: "factorId"),
                                                                                        grid.Column("FactorSortOrder", Model.Labels.SpeciesFactSortOrder, style: "factorSortOrder")
                                                                )
                                                    ) %>
                               
                                                    
                                                      </div>
                                                       
           
                    <% } %>
                        </div>
              

                <% } %>

               </div>
               </fieldset>
            <%} %> 
         
        
        <fieldset>
            <p>
                <button id="btnGetExcelFile" type="submit" class="ap-ui-button"><%: Model.Labels.GetExcelFile %></button>            
            </p>
        </fieldset>
<!-- Full container end -->
    </div>   
            
<% } %>

    
    <script type="text/javascript">

      
        $(document).ready(function () {
            //$('fieldset h1.taxonSummaryHeader.open').closest("fieldset").find("div.fieldsetContent").show();
           // $('fieldset h1.taxonSummaryHeader.closed').closest("fieldset").find("div.fieldsetContent").show();
           // $('fieldset h1.taxonSummaryHeader').closest("fieldset").find("div.fieldsetContent").show();
        });

        
        var fileDownloadCheckTimer;
        function blockUIForDownload() {
            var token = new Date().getTime(); //use the current timestamp as the token value
            $('#downloadTokenValue').val(token);
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Model.Labels.GeneratingExcelFile %></h1>' });            
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
            $('#filterTaxonCategories').dropdownchecklist({ firstItemChecksAll: true, emptyText: 'Select category', width: 700, maxDropHeight: 250, icon: {} });
            $('#outputTaxonCategories').dropdownchecklist({ firstItemChecksAll: true, emptyText: 'Select category', width: 700, maxDropHeight: 250, icon: {} });
            $('#outputTaxonNames').dropdownchecklist({ firstItemChecksAll: true, emptyText: 'Select category', width: 700, maxDropHeight: 250, icon: {} });
            $('#filterSwedishOccurrence').dropdownchecklist({ firstItemChecksAll: true, emptyText: 'Select category', width: 700, maxDropHeight: 250, icon: {} });
            $('#filterSwedishHistory').dropdownchecklist({ firstItemChecksAll: true, emptyText: 'Select category', width: 700, maxDropHeight: 250, icon: {} });

            $("#speciesFactForm").submit(function () {
                blockUIForDownload();
            });


            $('a.toggleCheckbox').toggle(
                function () {
                    $(this).closest("table").find(":checkbox").attr('checked', true);
                },
                function () {
                    $(this).closest("table").find(":checkbox").attr('checked', false);
                }
            );
        });        
        
    </script>

        
</asp:Content>