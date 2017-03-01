<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.Match.MatchSettingsViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%:Model.Labels.AmbiguitiesTitleLabel %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="readHeader"><%: Model.Labels.AmbiguitiesTitleLabel%></h1>     
     <% using (Html.BeginForm("MatchItems", "Match", FormMethod.Post, new { @id = "matchItemsForm", @name = "matchItemsForm" })) 
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
                            
        <fieldset> 
            <h2 class="open"><%: Model.Labels.PickCorrectMatchLabel%></h2>
            <div class="fieldsetContent">
                
                <% if ((ViewData["MatchItems"]) != null) %>
                <% {%>
                <table id="matchProblemGrid" class="grid">
                <thead>
                    <tr>                    
                        <th><%: Model.LabelForProvidedText %></th>
                        <th><%: Model.Labels.MatchStatusLabel %></th>
                        <th><%: Model.Labels.AmbiguitiesLabel %></th>
                    </tr>
                </thead>

                <% var items = (List<ArtDatabanken.WebApplication.Dyntaxa.Data.Match.DyntaxaMatchItem>)ViewData["MatchItems"]; %>
                <% foreach (var item in items) %>
                <% {%>
                
                <tbody>
                
                    <% if (item.Status == ArtDatabanken.WebApplication.Dyntaxa.Data.MatchStatus.NeedsManualSelection) %>
                    <% { %>
                    <tr class="gridrow">
                        <td><%: item.ProvidedText%></td>
                        <td><%: item.StatusDescription %></td>
                        <td><%: Html.DropDownList(item.DropDownListIdentifier)%></td>
                    </tr>
                    <% }
                    else
                    { %>
                    <tr class="gridrow">
                        <td><%: item.ProvidedText%></td>
                        <td><%: item.Status%></td>
                        <td>&nbsp;</td>
                    </tr>                                             
                    <% }%>
                    <% }%>                

                    <!--
                    <tr class="gridrow_alternate">
                        <td></td>
                        <td></td>                
                        <td></td>
                    </tr>
                    -->
                </tbody>
                </table>

                <% } %>
                         
            </div>            
        </fieldset>

        <p>
            <%--<button type="submit" class="ap-ui-button" name="submitButton" value="excelbutton"><%: Model.Labels.SaveAsExcelLabel %></button>--%>            
            <button id="btnGetExcelFile" type="submit" class="ap-ui-button" name="submitButton" value="excelbutton"><%: Model.Labels.SaveAsExcelLabel%></button>            
            <button type="submit" class="ap-ui-button"  name="submitButton" value="tablebutton"><%: Model.Labels.ViewResultOnScreenLabel %></button>            
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


        $(document).ready(function () {
            //initToggleFieldsetH2();

            $("#matchItemsForm").submit(function () {
                blockUIForDownload();
            });
            
//            $('#btnGetExcelFile').click(function () {
//                var form = document.matchItemsForm;
//                blockUIForDownload();
//                form.submit();
//            });

        });   

        
    </script>

</asp:Content>