<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonDeleteViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.Labels.TitleLabel %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  
    <h1 class="readHeader">
        <%: Model.Labels.TitleLabel %>
    </h1>

    <% if (ViewBag.Taxon != null)
        { %>
        <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
    <% }
        else
        { %>
        <% Html.RenderAction("TaxonSummary", "Taxon", new { taxonId = Model.TaxonId }); %>    
    <% } %>
    

    <% using (Html.BeginForm("Delete", "Taxon", FormMethod.Post, new { @id = "deleteTaxonForm", @name = "deleteTaxonForm" }))           
       {%>    
       <%: Html.HiddenFor(m => m.TaxonId) %>  
       <%: Html.HiddenFor(m => m.RevisionId) %>     
      
    <!-- Full container start -->
    <div id="fullContainer">
        
          <fieldset>
            <h2 class="open">
                <%:Model.Labels.TitleLabel%>
        </h2>
            <div class="fieldsetContent">
               <%-- <p><strong><%: Model.Category %>:</strong>
                 <%= Model.ScientificName != null ? Html.RenderScientificName(Model.ScientificName, null, Model.SortOrder).ToString() : "-" %>
                  <%= Model.CommonName != null ? Html.Encode(string.Format(" - {0}", Model.CommonName)) : "" %> 
              --%>    
               <h3 class="open">
                    <%: Model.Labels.SharedDialogInformationHeader%>
                </h3>
                <div class="fieldsetSubContent fullWidth">
                     <table>
                     <tr>
                        <td><strong><%: Model.Labels.TaxonIdLabel %></strong></td>
                        <td><%: Model.TaxonId %></td>                   
                    </tr><tr>
                        <td><strong><%: Model.Labels.ScientificNameLabel%></strong></td>
                        <td><%: Model.ScientificName != null ? Model.ScientificName : "-" %></td>                   
                    </tr>
                    <tr>
                        <td><strong><%: Model.Labels.CommonNameLabel%></strong></td>
                        <td><%: Model.CommonName != null ?  Model.CommonName : "-" %></td>                   
                    </tr>
                    <tr>
                        <td><strong><%: Model.Labels.CategoryLabel %></strong></td>
                        <td><%: Model.Category %></td>                   
                   </tr>
                </table>
                </div>
                <p>  
                  <br/>
                    <strong><%: Model.Labels.DoYouWantToDeleteLabel %></strong>
                 <br/>
                </p>
                  
            </div>
        </fieldset>      
         <fieldset>  
        <p>
        
             <%  if (Model.IsSelectedTaxonChildless)
                 { %>               
                    <input type="submit"  id="<%:Model.Labels.GetSelectedSave %>" name="submitButton" class="ap-ui-button" value="<%:Model.Labels.SharedDeleteButtonText %>" />
 
                 <% }
                else 
                 { %>               
                    <input type="submit" class="ap-ui-button-disabled" id="finalizeButton" name="submitButton3"  value="<%:Model.Labels.SharedDeleteButtonText %>" disabled="disabled" />
                 <% }%> 
            <%: Html.ActionLink(Model.Labels.CancelButtonLabel, "Edit", new { taxonId = Model.TaxonId }, new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>                        
        </p>
        </fieldset>
        <!-- full container end -->
    </div>
    <% } %>
    
    <script language="javascript" type="text/javascript">
    
  

    $(document).ready(function () {
        //initToggleFieldsetH2();
        
        <% if (!Model.IsSelectedTaxonChildless)
           { %>  
                showInfoDialog("<%:Model.Labels.TitleLabel%>", "<%:Model.Labels.TaxonHaveChildrenErrorText%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
        <% } %>
        
              //get the selected values so we can update the database or whatever.
        $('#'+"<%:Model.Labels.GetSelectedSave %>" ).click(function () {
            
            return showDialog("<%:Model.Labels.TitleLabel%>", "<%:Model.Labels.DeleteInfoTextLabel%>", 
                               "<%:Model.Labels.SharedDeleteButtonText%>", "<%:Model.Labels.CancelButtonLabel%>", function() {                
                 $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Resources.DyntaxaResource.SharedDeleting %></h1>' });
                 $('#deleteTaxonForm').submit();                 
                } , null);
               
        });

    });

</script> 
</asp:Content>
