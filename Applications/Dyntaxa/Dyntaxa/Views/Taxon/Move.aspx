<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonMoveViewModel>" %>

<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.Labels.TitleLabel %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="readHeader">
        <%: Model.Labels.TitleLabel %>
    </h1>
        
    <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.OldParentTaxonId }); %>            
        <%--<% Html.EnableClientValidation(); %> --%>

        <% using (Html.BeginForm("Move", "Taxon", FormMethod.Post, new { @id = "moveTaxaForm", @name = "moveTaxaForm" }))
           {%>
                    
        <% if (!this.ViewData.ModelState.IsValid)
        { %>
        <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
            <h2 class="validationSummaryHeader">
                <%:Html.ValidationSummary(false, "")%>
            </h2>
        </fieldset>
        <% } %>
        
        <%: Html.HiddenFor(m => m.OldParentTaxonId) %> 
      
    <!-- Full container start -->
    <div id="fullContainer">        
      
        <%--Select children to move--%>
        <fieldset>
            <h2>
                <input type="checkbox" id="selectAllChildrenCheckBox" />
                <%:Model.Labels.SelectChildrenToMoveLabel%>
            </h2>
            <div class="fieldsetContent">
                <ul>
                    <% var childMainRealationsExists = false;
                        if (Model.HasChildren && Model.ChildTaxa != null && Model.ChildTaxa.Count > 0)
                        { %>
                    <% foreach (var item in Model.ChildTaxa)
                       {
                           childMainRealationsExists = childMainRealationsExists || item.MainRelation;
                    %>
                    <li>
                        <input type="checkbox" value="<%= item.TaxonId %>" name="SelectedChildTaxaToMove" <%= item.MainRelation ? "" : "disabled" %> />
                        <%: item.Category %>:
                        <%: Html.RenderScientificName(item.ScientificName, null, item.SortOrder) %>
                        <% if (!string.IsNullOrEmpty(item.CommonName))
                           { %>
                        <%: " - " + item.CommonName %>
                        <% }
                           if (!item.MainRelation)
                           { %>
                                <img src="/Images/Icons/secondary_relation.png"/>
                        <% } %>
                    </li>
                    <% } %>
                    <% }
                       else
                       { %>
                            <p>
                                <%: Model.Labels.TaxonMoveNoChildrenErrorLabel %>
                            </p>
                       <% }%>
                </ul>
                <% if (Model.ChildTaxa == null && Model.ChildTaxa.Count <= 0)
                   { %>
                    <p class="noData">
                        <%:Resources.DyntaxaResource.ErrorNoData%>
                    </p>
                <% } %>
            </div>
        </fieldset>
        <%--Select new parent--%>
        <fieldset>
            <h2>
                <%:Model.Labels.SelectNewParentLabel%></h2>
            <div class="fieldsetContent">
                <table class="display-table">
                    <tr>
                        <td>
                            <label for="parentTaxonList" class="editor-label"><%:Model.Labels.AvailableParentsLabel%></label>                            
                        </td>
                        <td>
                            <label class="editor-label"><%:Model.Labels.SelectedParentLabel%></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                                                 
                            <select id="parentTaxonList" size="10" style="width: 300px;" name="NewParentTaxon">
                                <% foreach (var item in Model.AvailableParents)
                                {%>
                                    <option value="<%=item.TaxonId%>"><%:item.Category%>: <%:item.ScientificName%></option>
                                <% } %>
                            </select>
                        </td>
                        <td style="vertical-align: top;">
                            <div id="selectedNewParentTaxon" style="font-size: 16px;">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%: Resources.DyntaxaResource.TaxonSharedListTaxaInCategory%>:
                            <select id="taxonCategoryListBox">
                                <option value="-1">[<%: Resources.DyntaxaResource.SharedChooseCategory%>]</option>
                                <% foreach (var category in Model.TaxonCategories)
                                 {%>
                                    <option value="<%= category.Id %>"><%: category.Name %></option>
                               <%} %>                                
                            </select>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </fieldset>
      
        <fieldset>
        <p>
            <% if (Model.HasChildren && childMainRealationsExists)
            { %>
            <input type="submit" value="<%:Model.Labels.MoveSelectedTaxaLabel%>" class="ap-ui-button" />
             <% } %>   
            <%:Html.ActionLink(Resources.DyntaxaResource.SharedCancelButtonText, "Info", "Taxon",
                                                 new {taxonId = Model.OldParentTaxonId},
                                                 new {@class = "ap-ui-button", @style = "margin-top: 10px;"})%>
        </p>
        </fieldset>
        <% } %>
        <!-- end form -->
        <!-- full container end -->
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            
            <% if (!Model.IsOkToMove)
           { %>  
                showInfoDialog("<%:Model.Labels.MoveSelectedTaxaLabel%>", "<%:Model.MoveErrorText%>",  "<%:Model.Labels.ConfirmText%>",null);
        <% } %>

            // checkbox click event
            $('#selectAllChildrenCheckBox').click(function () {
                $(this).parents('fieldset:eq(0)').find(':checkbox').attr('checked', this.checked);
            });


            $("#taxonCategoryListBox").change(function(eventData) {
                var val = $("#taxonCategoryListBox").val();
                $("#parentTaxonList").empty();
                if (val != -1) {                         
                    $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Resources.DyntaxaResource.SharedLoading %></h1>' });
                    $.getJSON('<%= Url.Action("GetCategoryTaxa","Taxon") %>', { categoryId: val}, function (data) {
                        $.unblockUI();
//                        var output = []; 
//                        var length = data.length; 
//                        for(var i=0; i < length; i++) { 
//                            output[i] = '<option value="'+ data[i].TaxonId +'">'+ data[i].Name +'</option>'; 
//                        }  
//                        $('#parentTaxonList').get(0).innerHTML = output.join('');                         
//                        
                        
                        var auxArr = []; 
                        $.each(data, function(i, option) 
                        { 
                            auxArr[i] = "<option value='" + option.TaxonId + "'>" + option.Name + "</option>"; 
                        }); 
 
                        $('#parentTaxonList').append(auxArr.join('')); 
                        
                        
                        
                    }); 
                                        
                }                
            });

            
            
            
            // listbox change event
            $("#parentTaxonList").change(function () {
                $("#selectedNewParentTaxon").text($("#parentTaxonList option:selected").text());
            }).change();
            
              $("#moveTaxaForm").submit(function () {
                    $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Resources.DyntaxaResource.SharedSaving %></h1>'
                 });
              });
        });

    </script>
</asp:Content>
