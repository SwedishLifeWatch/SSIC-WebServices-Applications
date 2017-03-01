<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonInfoViewModel>" %>
<%@ Import Namespace="ArtDatabanken.Data" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.Labels.TitleLabel %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
   <% if (ViewBag.MiniMode == true) 
      { %>
        <% if (ViewBag.Taxon != null)
           { %>
                <a href="<%= Url.Content("~/Taxon/SearchResult/")%><%: ViewBag.TaxonId %>?mode=mini"><%: Model.Labels.TaxonRestMasterBackToSearchLabel %></a><br/><br/>
        <% } %>
    <% } %>
    
    <h1 class="readHeader">
        <%: Model.Labels.TitleLabel %>
    </h1>    
    <% if (ViewBag.Taxon != null)
       { %>
        <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
    <% }
       else
       { %>
        <% Html.RenderAction("TaxonSummary", "Taxon", new {taxonId = Model.TaxonId}); %>    
    <% } %>

<!-- Left container start -->
    <div id="leftContainer">            
        
        <fieldset>
            <h2 id="recommendedNamesToggleBox" class="open savestate"><%: Model.Labels.RecommendedNamesLabel %></h2>
            <div class="fieldsetContent">
                <p>
                    <%: Html.Partial("RenderTaxonName", new RenderTaxonNameViewModel(Model.Labels.ScientificNameLabel, Model.ReferenceViewAction, Model.ScientificName, Model.IsPublicMode, "Info", "Taxon", Model.TaxonId))%>
                </p>                
                <% if (Model.AnamorphName != null)
                   { %>
                   <p>
                        <%: Html.Partial("RenderTaxonName", new RenderTaxonNameViewModel(Model.Labels.AnamorphNameLabel, Model.ReferenceViewAction, Model.AnamorphName, Model.IsPublicMode, "Info", "Taxon", Model.TaxonId))%>
                   </p>                               
                <% } %>
                <p>
                    <%: Html.Partial("RenderTaxonName", new RenderTaxonNameViewModel(Model.Labels.CommonNameLabel, Model.ReferenceViewAction, Model.CommonName, Model.IsPublicMode, "Info", "Taxon", Model.TaxonId))%>
                </p>

                <h3 class="open"><%: Model.Labels.OtherLanguagesLabel %></h3>
                <div class="fieldsetSubContent fullWidth">                    
                    <ul id="otherRecommendedNames">                    
                    <% if (Model.OtherLanguagesNames.Count > 0)
                       { %>
                       <% foreach (var item in Model.OtherLanguagesNames)
                          { %>
                            <li>
                                <span title="<%:Model.Labels.NameToolTip %>" data-guid="<%: item.GUID %>" class="referenceTooltip"><%: item.CategoryName %>: <%: item.Name %></span>
                                <%: Html.Partial("ReferenceLink", new RenderTaxonNameViewModel(item.CategoryName, Model.ReferenceViewAction, item, Model.IsPublicMode, "Info", "Taxon", Model.TaxonId))%>
                            </li>
                       <%} %>

                    <% } %>
                    </ul>
                    <% if (Model.OtherLanguagesNames.Count <= 0)
                    { %>                    
                        <p class="noData"><%: Model.BaseLabels.NoDataMessage %></p>
                    <%} %>
                </div>


             <%--  <% if (Model.AnamorphName != "" && Model.AnamorphName != null)
                   { %>
                <p><strong><%: Resources.DyntaxaResource.ReadTaxonInformationAnamorhNameLabel%></strong>:  <%: Model.AnamorphName%> <%: Model.AnamorphAuthor%></p>
                <% } %>--%>
   
            </div>            
        </fieldset>
          
  
        <%--Synonyms--%>
        <fieldset>
            <h2 class="open savestate"><%: Model.Labels.SynonymsLabel %></h2>
            <div class="fieldsetContent">            
            <% if (Model.Synonyms.Count > 0)
                { %>                    
                    <ul id="synonyms">            
                       <% foreach (TaxonNameViewModel item in Model.Synonyms)
                          { %>
                            <li>
                                <%--<span title="<%:Model.Labels.NameToolTip %>" data-guid="<%: item.GUID %>" class="referenceTooltip"><%: item.CategoryName %>: <%:Html.RenderTaxonName(item) %></span>--%>
                                <span title="<%:Model.Labels.NameToolTip %>" data-guid="<%: item.GUID %>" class="<%:item.ShowWarning ? "referenceTooltipWarning" : "referenceTooltip"%>"><%: item.CategoryName %>: <%:Html.RenderTaxonNameWithStatus(item) %></span>
                                <%: Html.Partial("ReferenceLink", new RenderTaxonNameViewModel(item.CategoryName, Model.ReferenceViewAction, item, Model.IsPublicMode, "Info", "Taxon", Model.TaxonId))%>                                
                            </li>
                       <%} %>                
                    </ul>
                <% } %>                

            <% if (Model.Synonyms.Count == 0)
            { %>                    
                <p class="noData"><%: Model.BaseLabels.NoDataMessage %></p>
            <%} %>               
      
            </div>
        </fieldset> 
        
        <%--proParte synonyms--%>
        <fieldset>
            <h2 class="open savestate"><%: Model.Labels.ProParteSynonymsLabel %></h2>
            <div class="fieldsetContent">            
            <% if (Model.ProParteSynonyms.Count > 0)
                { %>                    
                    <ul id="Ul2">            
                       <% foreach (var item in Model.ProParteSynonyms)
                          { %>
                            <li>
                                <%--<span title="<%:Model.Labels.NameToolTip %>" data-guid="<%: item.GUID %>" class="referenceTooltip"><%: item.CategoryName %>: <%:Html.RenderTaxonName(item) %></span>--%>
                                <span title="<%:Model.Labels.NameToolTip %>" data-guid="<%: item.GUID %>" class="<%:item.ShowWarning ? "referenceTooltipWarning" : "referenceTooltip"%>"><%: item.CategoryName %>: <%:Html.RenderTaxonNameWithStatus(item) %></span>
                                <%: Html.Partial("ReferenceLink", new RenderTaxonNameViewModel(item.CategoryName, Model.ReferenceViewAction, item, Model.IsPublicMode, "Info", "Taxon", Model.TaxonId))%>                                
                            </li>
                       <%} %>                
                    </ul>
                <% } %>                

            <% if ( Model.ProParteSynonyms.Count == 0)
            { %>                    
                <p class="noData"><%: Model.BaseLabels.NoDataMessage %></p>
            <%} %>               
      
            </div>
        </fieldset>        

        <%--Misapplied names--%>
        <fieldset>
            <h2 class="open savestate"><%: Model.Labels.MisapplicationsLabel %></h2>
            <div class="fieldsetContent">            
            <% if (Model.MisappliedNames.Count > 0)
                { %>                    
                    <ul id="Ul4">            
                       <% foreach (var item in Model.MisappliedNames)
                          { %>
                            <li>
                                <%--<span title="<%:Model.Labels.NameToolTip %>" data-guid="<%: item.GUID %>" class="referenceTooltip"><%: item.CategoryName %>: <%:Html.RenderTaxonName(item) %></span>--%>
                                <span title="<%:Model.Labels.NameToolTip %>" data-guid="<%: item.GUID %>" class="<%:item.ShowWarning ? "referenceTooltipWarning" : "referenceTooltip"%>"><%: item.CategoryName %>: <%:Html.RenderTaxonNameWithStatus(item) %></span>
                                <%: Html.Partial("ReferenceLink", new RenderTaxonNameViewModel(item.CategoryName, Model.ReferenceViewAction, item, Model.IsPublicMode, "Info", "Taxon", Model.TaxonId))%>                                
                            </li>
                       <%} %>                
                    </ul>
                <% } %>                

            <% if ( Model.MisappliedNames.Count == 0)
            { %>                    
                <p class="noData"><%: Model.BaseLabels.NoDataMessage %></p>
            <%} %>               
      
            </div>
        </fieldset>        


        <%--Taxonomic hierarchy, static indent--%>
        <fieldset>
            <h2 class="open savestate"><%: Model.Labels.TaxonomicHierarchyLabel %></h2>
            <div class="fieldsetContent">
                <ul id="parentTaxonList">                   
                   <% int index = 0; %>
         
                   <% foreach (var item in Model.ParentTaxa)
                      { %>
                        <li style='padding-left:<%= index %>px'><%: item.Category %>: <%: Html.RenderScientificLink(item.ScientificName, item.SortOrder, "Info", "Taxon", new {@taxonId = item.TaxonId, @changeRoot=true}) %>
                        <% if (!string.IsNullOrEmpty(item.CommonName)) { %> 
                            <%: " (" + item.CommonName + ")"%>
                        <% } %>
                        </li>
                        <% index += 10; %>
                   <%} %>                   
                   <li style='padding-left:<%= index %>px'><%: Model.Category %>: <%= Model.ScientificName != null ? Html.RenderScientificName(Model.ScientificName.Name, null, Model.TaxonCategorySortOrder).ToString() : "-"  %>
                        <% if (Model.CommonName != null && Model.CommonName.Name != null) { %> 
                            <%: " (" + Model.CommonName.Name + ")"%>
                        <% } %>
                   </li>              
                </ul>

                <% if (Model.ParentTaxa.Count == 0)
                { %>                    
                    <p class="noData"><%: Model.BaseLabels.NoDataMessage %></p>
                <%} %>               
            
             <%-- Get other parents ie not the main parent--%>
              <h3 class="open"><%: Model.Labels.OtherParentsLabel %></h3>
                <div class="fieldsetSubContent fullWidth">                    
                     <ul id="otherParentTaxonList">                   
                       <% index = 0; %>
                       <% foreach (var item in Model.OtherParentTaxa)
                          { %>
                            <li><%: item.Category %>: <%: Html.RenderScientificLink(item.ScientificName, item.SortOrder, "Info", "Taxon", new {@taxonId = item.TaxonId, @changeRoot=true}) %>
                            <% if (!string.IsNullOrEmpty(item.CommonName)) { %> 
                                <%: " (" + item.CommonName + ")"%>
                            <% } %>
                            </li>
                            <% index += 10; %>
                       <%} %>                   
                    </ul>
                    <% if (Model.OtherParentTaxa.Count == 0)
                    { %>                    
                        <p class="noData"><%: Model.BaseLabels.NoDataMessage %></p>
                    <%} %>
                </div>
                 <%-- Get other historical parents ie previous parents if not existing --%>
              <h3 class="open"><%: Model.Labels.HistoricalParentsLabel %></h3>
                <div class="fieldsetSubContent fullWidth">    
                    <ul id="historicalParentTaxonList">                         
                     <% index = 0; %>
                       <% foreach (var item in Model.HistoricalParentTaxa)
                          { %>
                            <li><%: item.Category %>: <%: Html.RenderScientificLink(item.ScientificName, item.SortOrder, "Info", "Taxon", new {@taxonId = item.TaxonId, @changeRoot=true}) %>
                            <% if (!string.IsNullOrEmpty(item.CommonName)) { %> 
                                <%: " (" + item.CommonName + ")"%>
                            <% } %>
                            <% if (item.EndDate.HasValue && item.EndDate.Value < DateTime.Now) { %> 
                                <%: Model.Labels.HistoricalParentsUntilText + " " + item.EndDate.Value.ToShortDateString() %>
                            <% } %>
                            </li>
                            <% index += 10; %>
                       <%} %>                   
                    </ul>
                    <% if (Model.HistoricalParentTaxa.Count == 0)
                    { %>                    
                        <p class="noData"><%: Model.BaseLabels.NoDataMessage %></p>
                    <%} %>
                </div>
            </div>
        </fieldset>
        
        <%--Immediate child taxa--%>
        <fieldset>
            <h2 class="open savestate"><%: Model.Labels.NearestChildTaxaLabel %></h2>
            <div class="fieldsetContent">
                <ul id="childTaxonList">                
                <% if (Model.ChildTaxa.Count > 0)
                   { %>
                   <% foreach (var item in Model.ChildTaxa)
                      { %>
                        <li><%: item.Category %>: <%: Html.RenderScientificLink(item.ScientificName, item.SortOrder, "Info", "Taxon", new {@taxonId = item.TaxonId, @changeRoot=true}) %>
                        <% if (!string.IsNullOrEmpty(item.CommonName)) { %> 
                            <%: " (" + item.CommonName + ")"%>
                        <% } %>
                        </li>
                   <%} %>
                <% } %>
                </ul>
                <% if (Model.ChildTaxa.Count <= 0)
                { %>                    
                    <p class="noData"><%: Model.BaseLabels.NoDataMessage %></p>
                <%} %> 
            </div>
        </fieldset>


        <%--Statistics--%>
        <fieldset>
            <h2 class="open savestate"><%: Model.Labels.TaxonStatisticsLabel %></h2>
            <div class="fieldsetContent">
                <% if (Model.TaxonStatistics.Count > 0)
                    { %>
                    <%  var grid = new WebGrid(source: Model.TaxonStatistics, rowsPerPage: 40, canSort: false); 
                    %>
                    
                        <div id="grid" >
                          <%:
                          grid.GetHtml(tableStyle: "grid", headerStyle: "head",alternatingRowStyle: "alt", columns: grid.Columns(
                                grid.Column("CategoryName", Model.Labels.TaxonStatisticsCategoryNameLabel),
                                grid.Column("ChildTaxaCount", Model.Labels.TaxonStatisticsInDyntaxaLabel),
                                grid.Column("SwedishChildTaxaCount", Model.Labels.TaxonStatisticsInSwedenLabel),
                                grid.Column("SwedishReproCount", Model.Labels.TaxonStatisticsReproInSwedenLabel)
                                )
                            )
                         %>
                        </div>
                <% } %>
                <% else
                   { %>                    
                    <p class="noData"><%: Model.BaseLabels.NoDataMessage %></p>                    
                <% } %>
            </div>
        </fieldset>

        <%--References--%>
        <fieldset>
            <h2 class="open savestate"><%: Model.Labels.ReferencesLabel %></h2>
            <div class="fieldsetContent">
                <h3 class="reference-group-title"><%: Resources.DyntaxaResource.TaxonReferenceTaxonomyTitle %></h3>
                <% Html.RenderAction("ListReferences", "Reference", new {@guid = Model.Guid}); %> 
                
                <% if (Model.SwedishOccurrenceSummary.SwedishOccurrenceFact != null && Model.SwedishOccurrenceSummary.SwedishOccurrenceFact.Reference != null)
                { %>
                <h3 class="reference-group-title"><%: Resources.DyntaxaResource.TaxonReferenceSwedishOccurrenceTitle %></h3>
                <% Html.RenderAction("ListSpeciesFactReference", "Reference", new { @reference = Model.SwedishOccurrenceSummary.SwedishOccurrenceFact.Reference }); %> 
                <% } %>

                <% if (Model.SwedishOccurrenceSummary.SwedishHistoryFact != null && Model.SwedishOccurrenceSummary.SwedishHistoryFact.Reference != null)
                { %>
                <h3 class="reference-group-title"><%: Resources.DyntaxaResource.TaxonReferenceSwedishImmigrationHistoryTitle %></h3>
                <% 
                    Html.RenderAction("ListSpeciesFactReference", "Reference", new { @reference = Model.SwedishOccurrenceSummary.SwedishHistoryFact.Reference }); %> 
                <% } %>                
            </div>
        </fieldset>

        <%--Identifiers--%>
        <fieldset>
            <h2 class="open savestate"><%: Model.Labels.IdentifiersLabel %></h2>
            <div class="fieldsetContent">
                <p><%: Model.Labels.TaxonIdLabel %>:  <%: Model.TaxonId %></p>
                <p><%: Model.Labels.GuidLabel %>: <%: Model.Guid %></p>
                <ul id="guidList">                
                <% if (Model.Identifiers.Count > 0)
                   { %>
                   <% foreach (var item in Model.Identifiers)
                      { %>                        
                        <%--<li><span title="<%:Model.Labels.NameToolTip %>" data-guid="<%: item.GUID %>" class="referenceTooltip"><%: item.CategoryName %>: <%: item.Name %> (<%: item.NameStatus %>) </span> </li>--%>
                    
                        <li>
                            <span title="<%:Model.Labels.NameToolTip %>" data-guid="<%: item.GUID %>" class="<%:item.ShowWarning ? "referenceTooltipWarning" : "referenceTooltip"%>"><%: item.CategoryName %>: <%:Html.RenderTaxonNameWithStatus(item) %></span>
                        </li>                    
                   <%} %>
                <% } %>
                </ul>
                
            </div>
        </fieldset>




<!-- Left container end -->
    </div>    
        
    


<!-- Right container start -->
    <div id="rightContainer">    

        <%--Swedish occurrence--%>        
        <% if (Model.IsValid && Model.SwedishOccurrenceSummary.Show)
         { %>
        <fieldset>
            <h2 class="open savestate"><%: Model.Labels.SwedishOccurrenceSummaryTitle%></h2>
            <div id="swedishOccurrenceRightSummary" class="fieldsetContent">                
                <% Html.RenderAction("SwedishOccurrenceSummary", new { @model = Model.SwedishOccurrenceSummary });%>                
            </div>            
        </fieldset>
        <% }%>

        <%--Recommended links--%>
        <% if (Model.IsValid)
         { %>
        <fieldset>
            <h2 class="open savestate"><%: Model.Labels.RecommendedLinksLabel %></h2>
            <div id="recommendedLinks" class="fieldsetContent">                
                <% Html.RenderAction("TaxonRecommendedLinks", new { @taxon = ViewBag.Taxon });%>                
            </div>            
        </fieldset>
        <% }%>
        
        <%--Distribution in Sweden--%>
        <% if (Model.IsValid && Model.IsBelowGenus)
                   { %>
        <fieldset>
            <h2 id="mapHeader" class="open savestate"><%: Model.Labels.DistributionInSwedenLabel %></h2>
                <div id="distributionInSwedenContent" class="fieldsetContent">
                    <% Html.RenderAction("DistributionInSwedenLinks", new { @taxon = ViewBag.Taxon, @model = Model.SwedishOccurrenceSummary });%>                
                </div>    
                <%--<div id="mapContent" class="fieldsetContent">
                    <img src="<%: Url.Content("~/")%>Taxon/CountyMap/<%: Model.TaxonId %>" alt="<%: Model.Labels.DistributionMapImageTitle %> <%: Model.CommonName %>" title="<%: Model.Labels.DistributionMapImageTitle %> <%: Model.CommonName %>"  height="406" />
                </div>--%>
        </fieldset>
        <% }%>

        <%--Quality chart--%>
        <% if (Model.IsValid)
                   { %>
        <fieldset>
            <h2 class="closed savestate"><%: Model.Labels.QualityChartLabel %></h2>
            <div id="qualityContent" class="fieldsetContent">
                
                 <%-- <img src="<%: Url.Content("~/Images/Temp/chart_temp.png")%>" alt="Sample" />
                 Example - http://stackoverflow.com/questions/6281520/charting-in-asp-net-mvc-3 --%>
                 
                 <%-- TODO: Lazy load with AJAX and check if null then show "no_chart_(sv/en).png" --%>
                 <img src="<%: Url.Content("~/")%>Taxon/TaxonQualitySummaryChart/<%: Model.TaxonId %>" alt="Chart" />
                 
                 
                 <%-- Html-based legend, easier to style / print
                 <table id="qualityChartLegends">
                    <tr>
                        <td class="pantone400C"> &nbsp; </td><td>- <%: Model.Labels.QualityChartLabelArray[0] %></td>
                        <td class="pantone131C"> &nbsp; </td><td>- <%: Model.Labels.QualityChartLabelArray[1] %></td>
                    </tr>
                    <tr>
                        <td class="pantone397Cp60"> &nbsp; </td><td>- <%: Model.Labels.QualityChartLabelArray[2] %></td>
                        <td class="pantone7479Cp60"> &nbsp; </td><td>- <%: Model.Labels.QualityChartLabelArray[3] %></td>
                    </tr>
                </table>
                --%>
                <p class="info"><%: Model.Labels.QualityChartUpdatedText %></p> 
            </div> 
        </fieldset>
        <% }%>
 

<!-- Right container end -->
    </div>

    <script type="text/javascript">
            
      function showReferenceDialog(guid) {
        //$.get('<%= Url.Action("ListReferences","Reference") %>',
        $.get('<%= Url.Action("Info","TaxonName") %>',            
            {
                guid: guid
            },
            function(data, textStatus, xhr) {                                
                var $dialog = $('<div></div>')
                .html(data)
                .dialog({
                    autoOpen: false,
                    title: "<%: Model.Labels.TaxonNameInformationLabel %>",
                    modal: true,
                    width: "700px",
                    resizable: false,
                    draggable: false,
                    zIndex: 999999,
			        buttons: {
				        Ok: function() {
					        $( this ).dialog( "close" );
				        }
			        }
                    });

                 $dialog.dialog('open');                                                                      
            }
        );           
      }
      
      $(document).ready(function() {          

          var $clickableReferenceObjects;
          <% if (Model.IsPublicMode)
             { %>
          $clickableReferenceObjects = $("span.referenceTooltip, span.referenceTooltipWarning, span.referenceIcon");
          <% }
             else
             { %>
          $clickableReferenceObjects = $("span.referenceTooltip, span.referenceTooltipWarning");
          <% } %>
          
          $clickableReferenceObjects.click(function() {
              var guid = $(this).attr("data-guid");  
              showReferenceDialog(guid);              
          });                              
          
    });

    </script>

</asp:Content>
