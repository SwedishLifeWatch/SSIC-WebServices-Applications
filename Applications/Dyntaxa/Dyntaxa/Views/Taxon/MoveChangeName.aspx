<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonMoveChangeNameViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.Labels.ChangeChildrenNamesLabel %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="readHeader">
        <%: Model.Labels.ChangeChildrenNamesLabel%>
    </h1>
    <% if (ViewBag.Taxon != null)
        { %>
        <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
    <% }
        else
        { %>
        <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
    <% } %>

 <!-- Full container start -->
    <div id="fullContainer">
        <% using (Html.BeginForm())
           {%>
        <% if (!this.ViewData.ModelState.IsValid)
        { %>
        <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
            <h2 class="validationSummaryHeader">
                <%:Html.ValidationSummary(false, "")%>
            </h2>
        </fieldset>
        <% } %>           
           <%: Html.Hidden("Id", Model.TaxonId) %>           
           <%: Html.HiddenFor(m=>m.OldParentTaxonId) %>
           <%: Html.HiddenFor(m=>m.NewParentTaxonId) %>      
           <% for (int i = 0; i < Model.SelectedChildTaxaToMove.Count; i++)
            { %>
             <%: Html.HiddenFor(m => m.SelectedChildTaxaToMove[i])%>
           <% } %>
           

        <fieldset>
            <h2>                
                <%:Model.Labels.MoveInfo%>
            </h2>
            <div class="fieldsetContent">
                <table>
                    <tr>
                        <td><strong><%: Model.Labels.OldParentLabel %></strong></td>
                        <td><%: Model.OldParentDescription %></td>
                    </tr>                
                    <tr>
                        <td><strong><%: Model.Labels.NewParentLabel %></strong></td>
                        <td><%: Model.NewParentDescription %></td>
                    </tr>                
                    <tr>
                        <td><strong><%: Model.Labels.MovedTaxonsLabel %></strong></td>
                        <td><%: string.Join(", ", Model.MovedTaxonsDescription)%></td>
                    </tr>                
                </table>
            </div>
        </fieldset>

        <%--New recommended scientific names--%>
        <fieldset>
            <h2>                
                <%:Model.Labels.NewRecommendedNamesLabel%>
            </h2>
            <div class="fieldsetContent">
               <table style="width: 857px;">
                <thead>
                    <tr>
                        <th style="width: 140px;"><%: Resources.DyntaxaResource.TaxonMoveCategory%></th>
                        <th style="width: 260px;"><%: Resources.DyntaxaResource.TaxonMoveName%></th>
                        <th><%: Resources.DyntaxaResource.TaxonMoveAuthor%></th>
                    </tr>
                </thead>
                <tbody>
                <% for (int i = 0; i < Model.MovedChildTaxons.Count; i++)                    
                {
                    var item = Model.MovedChildTaxons[i];
                  %>
                    <tr>
                        <td>
                            <%: item.Category %> 
                            <%: Html.Hidden(string.Format("MovedChildTaxons[{0}].TaxonId", i), item.TaxonId)%> 
                            <%: Html.Hidden(string.Format("MovedChildTaxons[{0}].NameId", i), item.NameId)%>
                        </td>
                        <td>                            
                            <%: Html.TextBox(string.Format("MovedChildTaxons[{0}].Name", i), item.Name, new { @style = "width:100%;" })%><br/>
                            <%: string.Format("({0})", item.OldName) %>
                        </td>
                        <td><%: Html.TextBox(string.Format("MovedChildTaxons[{0}].Author", i), item.Author, new { @style = "width:100%;" })%></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="2">
                            <% if (item.Synonyms != null && item.Synonyms.Count > 0)
                               {%>                                
                                <select style="width:100%;">
                                    <option value="<%: item.NameAndAuthorAsJson %>">Select synonym</option>
                                 <% foreach (SynonymName synonym in item.Synonyms)
                                    { %>
                                    <option value="<%: synonym.NameAndAuthorAsJson %>"><%: synonym.NameAndAuthor %></option>
                                    <% } %>                                    
                                </select>                                
                            <% }
                               else
                             { %>                                
                                <%: Model.Labels.NoSynonymsExists %>
                            <% } %>
                        </td>                                            
                    </tr>
                    <tr>
                        <td colspan="3"></td>
                    </tr>
                <% } %>
                </tbody>               
               </table>
            </div>
        </fieldset>
        <fieldset>
        <p>
            <input type="submit" value="<%: Resources.DyntaxaResource.SharedSaveButtonText %>" class="ap-ui-button" />
            <%: Html.ActionLink(Resources.DyntaxaResource.SharedCancelButtonText, "Info", "Taxon", new { taxonId = Model.TaxonId }, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>            
        </p>
        </fieldset>


        <% } %>
        <!-- end form -->
        <!-- full container end -->
    </div>


    <script type="text/javascript">

        $(document).ready(function () {

            $('select').change(function () {
                var val = $(this).val();
                var index = $(this)[0].selectedIndex;
                var obj = jQuery.parseJSON(val);
                var $inputBoxes = $(this).closest('tr').prev().children().find("input[type='text']");
                $inputBoxes.eq(0).val(obj.name);
                $inputBoxes.eq(1).val(obj.author);

//                if (obj.id == -1) {
//                    $inputBoxes.eq(0).removeAttr('readonly');
//                    $inputBoxes.eq(1).removeAttr('readonly');
//                } else {
//                    $inputBoxes.eq(0).attr('readonly', true);
//                    $inputBoxes.eq(1).attr('readonly', true);
//                }

                var $hiddenFields = $(this).closest('tr').prev().children().find("input[type='hidden']");
                $hiddenFields.eq(1).val(obj.id);

            });

        });

    </script>


</asp:Content>
