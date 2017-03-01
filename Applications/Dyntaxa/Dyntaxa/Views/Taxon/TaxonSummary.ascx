<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonSummaryViewModel>" %>
<%@ Import Namespace="ArtDatabanken.Data" %>
<%@Import namespace="Dyntaxa.Helpers.Extensions" %>
<fieldset class="taxonSummaryFieldset">

    
    <% if (Model.AlertStatus == TaxonAlertStatusId.Green || (ViewBag.IgnoreExpand != null && ViewBag.IgnoreExpand == true))
       { %>
       <h1 class="taxonSummaryHeader closed" style='background-image: <%= string.Format(" Url({0})", Url.Content(Model.AlertImageUrl)) %> ' >
    <% }
       else
       {%>
       <h1 class="taxonSummaryHeader open" style='background-image: <%= string.Format(" Url({0})", Url.Content(Model.AlertImageUrl)) %> ' >
    <% } %>    
        <strong><%: Model.Category %>:</strong>
        <%= Model.ScientificName != null ? Html.RenderScientificName(Model.ScientificName.Name, null, Model.CategorySortOrder).ToString() : "-" %>
        <%= Model.CommonName != null ? Html.Encode(string.Format(" - {0}", Model.CommonName.Name)) : "" %>         
    </h1>

    <div class="fieldsetContent">
        <table>
            <tr>
                <td><strong><%: Model.Labels.IdLabel %></strong></td>
                <td><strong><%: Model.Id %></strong></td>
            </tr>

            <tr>
                <td><strong><%: Model.Labels.GuidLabel %></strong></td>
                <td><%: Model.Guid %></td>
            </tr>

           <tr>
                <td><strong><%: Model.Labels.TaxonStatusLabel %></strong></td>
                <td><%: Model.Validity %></td>
            </tr>      

            <tr>
                <td><strong><%: Model.Labels.ConceptDefinitionLabel %></strong></td>
                <td><%: Model.ConceptDefinition %></td>
            </tr>

            <tr>
                <td><strong><%: Model.Labels.CategoryLabel %></strong></td>
                <td><%: Model.Category %></td>
            </tr>
            
            <% if (Model.CategoryId == 17)
               {%>
                   <tr>
                        <td><strong><%: Model.Labels.IsMicrospeciesLabel %></strong></td>
                        <td><%: Model.IsMicrospecies ? Model.Labels.True : Model.Labels.False %></td>
                    </tr>
             <%}%>
             
            <tr>
                <td><strong><%: Model.Labels.ScientificNameLabel %></strong></td>
                <td>
                    <strong><%= Model.ScientificName != null ? Html.RenderScientificName(Model.ScientificName.Name, Model.ScientificName.Author, Model.CategorySortOrder).ToString() : "-" %></strong>
                </td>
            </tr>   
            
             <tr>            
                <td><strong><%: Model.Labels.CommonNamesLabel %></strong></td>
                <td>
                    <%: Model.CommonName != null ? Model.CommonName.Name : "-" %>
                    <% if (Model.OtherValidCommonNames.Count > 0)
                       {%>
                       <%: string.Format("({0})", string.Join(", ", Model.OtherValidCommonNames.Select(x => x.Name))) %>
                    <% } %>
                </td>
            </tr>  

            <tr>
                <td><strong><%: Model.Labels.SynonymsLabel %></strong></td>
                <td>
                    <% if (Model.Synonyms.Count > 0)
                       {
                           for (int i = 0; i < Model.Synonyms.Count; i++)
                           {
                               var item = Model.Synonyms[i];
                               if (i > 0)
                               {
                                    %>
                                 <%: "; "%>
                            <% } %>

                            <%: Html.RenderScientificName(item.Name, item.Author, Model.CategorySortOrder)%>

                        <% }%>

                    <% }
                       else
                       { %> 
                       <%: "-" %>
                    <% } %>
                </td>
            </tr>           
                     
            
            <tr>
                <td><strong><%: Resources.DyntaxaResource.TaxonInfoProParteSynonymsLabel %></strong></td>
                <td>
                    <% if (Model.ProParteSynonyms.Count > 0)
                       {
                           for (int i = 0; i < Model.ProParteSynonyms.Count; i++)
                           {
                               var item = Model.ProParteSynonyms[i];
                               if (i > 0)
                               {
                                    %>
                                 <%: "; "%>
                            <% } %>

                            <%: Html.RenderScientificName(item.Name, item.Author, Model.CategorySortOrder)%>

                        <% }%>

                    <% }
                       else
                       { %> 
                       <%: "-" %>
                    <% } %>
                </td>
            </tr>    
            
            <tr>
                <td><strong><%: Resources.DyntaxaResource.TaxonInfoMisapplicationsLabel %></strong></td>
                <td>
                    <% if (Model.MisappliedNames.Count > 0)
                       {
                           for (int i = 0; i < Model.MisappliedNames.Count; i++)
                           {
                               var item = Model.MisappliedNames[i];
                               if (i > 0)
                               {
                                    %>
                                 <%: "; "%>
                            <% } %>

                            <%: Html.RenderScientificName(item.Name, item.Author, Model.CategorySortOrder)%>

                        <% }%>

                    <% }
                       else
                       { %> 
                       <%: "-" %>
                    <% } %>
                </td>
            </tr>                    
                         
            <tr>
                <td><strong><%: Model.Labels.ClassificationLabel%></strong></td>
                <td>
                    <% for (int i = 0; i < Model.Classification.Count; i++)
                       {
                           var item = Model.Classification[i];
                           %>
                        <%:Model.Classification[i].Category %> <strong><%: Html.RenderScientificName(item.ScientificName, null, item.SortOrder)%></strong><%: string.Format("{0}", i < Model.Classification.Count-1 ? ", " : "")%>
                    <% } %>
                </td>
            </tr>  

            <tr>
                <td><strong><%: Model.Labels.SwedishOccurrenceLabel %></strong></td>
                <td><%: Model.SwedishOccurrence %></td>
            </tr>   
                            
            <tr>
                <td><strong><%: Model.Labels.SwedishHistoryLabel%></strong></td>
                <td><%: Model.SwedishHistory %></td>
            </tr> 

            <tr>
                <td><strong><%: Model.Labels.CreateInformationLabel %></strong></td>
                <td><%: Model.CreatedInformation %></td>
            </tr>
            
            <tr>
                <td><strong><%: Model.Labels.UpdateInformationLabel %></strong></td>
                <td><%: Model.UpdateInformation %></td>
            </tr>                  
        </table>                
    </div>
</fieldset>


<script type="text/javascript">

    function OnTaxonSummaryLoaded() {
                 
        $('fieldset h1.taxonSummaryHeader').click(function () {
            $(this)
            .toggleClass("closed")
            .toggleClass("open")
            .closest("fieldset").find("div.fieldsetContent").slideToggle('fast', function () {
                // Animation complete.
            });
        });
        $('fieldset h1.taxonSummaryHeader.closed').closest("fieldset").find("div.fieldsetContent").hide();        
    }

    $(document).ready(function () {
        OnTaxonSummaryLoaded();
    });
    
    //    $('h1.taxonSummaryHeader').css("background-image",<%= string.Format("\"Url({0})\"",Url.Content("~/Images/Icons/info_red.png"))  %> );
</script>