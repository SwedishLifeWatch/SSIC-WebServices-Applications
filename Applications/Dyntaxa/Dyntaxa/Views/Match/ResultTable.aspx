<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.Match.MatchSettingsViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data.Match" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Resources.DyntaxaResource.MatchingResultsHeader %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="readHeader"><%: Resources.DyntaxaResource.MatchingResultsHeader %></h1>
     
    <% using (Html.BeginForm("ResultTable", "Match", FormMethod.Post, new { @id = "resultTableForm", @name = "resultTableForm" }))
   { %>
        <% if (!this.ViewData.ModelState.IsValid)
        { %>
        <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
            <h2 class="validationSummaryHeader">
                <%:Html.ValidationSummary(false, "")%>
            </h2>
        </fieldset>
        <% } %>        
        <%: Html.Hidden("downloadTokenValue")%>        
<!-- Full container start -->
    <div id="fullContainer">
        
        <%//: Html.HiddenFor(model => model.FileName) %> 
             
        <fieldset> 
            <h2 class="open"><%: Model.Labels.ResultLabel %></h2>
            <div class="fieldsetContent">               
                <% if ((ViewData["MatchItems"]) != null) %>
                <% { %>
                <% List<DyntaxaMatchItem> matchItems = (List<DyntaxaMatchItem>)ViewData["MatchItems"]; %>
                    <table class="grid">
                        <thead>
                            <tr>
                                <th><%: Model.LabelForProvidedText %></th>
                                <th><%: Model.Labels.MatchStatusLabel %></th>
                                
                                <% if (Model.OutputTaxonId) %>
                                <% { %>
                                <th><%: Model.Labels.TaxonIdLabel %></th>
                                <% } %>
                                
                                <% if (Model.OutputScientificName) %>
                                <% { %>
                                <th><%: Model.Labels.ScientificNameLabel %></th>
                                <% } %>
                                
                                <% if (Model.OutputAuthor) %>
                                <% { %>
                                <th><%: Model.Labels.AuthorLabel %></th>
                                <% } %>  

                                <% if (Model.OutputCommonName) %>
                                <% { %>
                                <th><%: Model.Labels.CommonNameLabel %></th>
                                <% } %>  

                                <% if (Model.OutputTaxonCategory) %>
                                <% { %>
                                <th><%: Model.Labels.TaxonCategoryLabel %></th>
                                <% } %>  

                                <% if (Model.OutputScientificSynonyms) %>
                                <% { %>
                                <th><%: Model.Labels.ScientificSynonymsLabel %></th>
                                <% } %>  

                                <% if (Model.OutputParentTaxa) %>
                                <% { %>
                                <th><%: Model.Labels.ParentTaxaLabel %></th>
                                <% } %>  

                                <% if (Model.OutputGUID) %>
                                <% { %>
                                <th><%: Model.Labels.GuidLabel %></th>
                                <% } %>  

                                <% if (Model.OutputRecommendedGUID) %>
                                <% { %>
                                <th><%: Resources.DyntaxaResource.ExportStraightColumnRecommendedGUID %></th>
                                <% } %>  
                                
                                <% if (Model.OutputSwedishOccurrence) %>
                                <% { %>
                                <th><%: Resources.DyntaxaResource.ExportStraightSwedishOccurrence %></th>
                                <% } %>  
                            </tr>
                        </thead>
                        <tbody>
                            <% foreach (var item in matchItems) %>
                            <% { %>
                            <tr class="gridrow">
                                <td><%:item.ProvidedText %></td>
                                <td><%:item.StatusDescription %></td>
                                
                                <% if (Model.OutputTaxonId) %>
                                <% { %>
                                <td><%:item.TaxonId %></td>
                                <% } %>
                                
                                <% if (Model.OutputScientificName) %>
                                <% { %>
                                <td><%:item.ScientificName %></td>
                                <% } %>
                                
                                <% if (Model.OutputAuthor) %>
                                <% { %>
                                <td><%:item.Author %></td>
                                <% } %>  

                                <% if (Model.OutputCommonName) %>
                                <% { %>
                                <td><%:item.CommonName %></td>
                                <% } %>  

                                <% if (Model.OutputTaxonCategory) %>
                                <% { %>
                                <td><%:item.TaxonCategory %></td>
                                <% } %>  

                                <% if (Model.OutputScientificSynonyms) %>
                                <% { %>
                                <td><%: item.ScientificSynonyms %></td>
                                <% } %>  

                                <% if (Model.OutputParentTaxa) %>
                                <% { %>
                                <td><%: item.ParentTaxa %></td>
                                <% } %>  

                                <% if (Model.OutputGUID) %>
                                <% { %>
                                <td><%: item.GUID %></td>
                                <% } %>  

                                <% if (Model.OutputRecommendedGUID) %>
                                <% { %>
                                <td><%: item.RecommendedGUID %></td>
                                <% } %>  
                                
                                <% if (Model.OutputSwedishOccurrence) %>
                                <% { %>
                                <td><%: item.SwedishOccurrence %></td>
                                <% } %>                                  
                            </tr>
                            <% } %>
                        </tbody>
                    </table> 
                <% } %>               
                         
            </div>            
        </fieldset>

        
        <p>
            <button id="btnGetExcelFile" type="submit" class="ap-ui-button"><%: Model.Labels.SaveAsExcelLabel%></button>            
        </p>

    </div>
<!-- Full container end -->

    <% } %>

    <script type="text/javascript">

        var fileDownloadCheckTimer;
        function blockUIForDownload() {
            var token = new Date().getTime(); //use the current timestamp as the token value
            $('#downloadTokenValue').val(token);
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Resources.DyntaxaResource.SharedGeneratingExcelFile %></h1>' });
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


        $(document).ready(function() {
//            $('fieldset h2').click(function() {
//                $(this)
//                    .toggleClass("closed")
//                    .toggleClass("open")
//                    .closest("fieldset").find("div.fieldsetContent").slideToggle('fast', function() {
//                        // Animation complete.
//                    });
//            });

//            $('#btnGetExcelFile').click(function () {
//                var form = document.resultTableForm;
//                blockUIForDownload();
//                form.submit();
//            });

            $("#resultTableForm").submit(function () {
                blockUIForDownload();
            });            
            
            
        });
    </script>

</asp:Content>
