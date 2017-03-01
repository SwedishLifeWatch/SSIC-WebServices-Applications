<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.Subscription.SubscriptionsViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data.Subscription" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data.Taxon" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: DyntaxaResource.SubscriptionsTitle %>    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="readHeader">
        <%: DyntaxaResource.SubscriptionsTitle %>
    </h1>
    
    <% if (ViewBag.Taxon != null)
        { %>
        <% Html.RenderAction("TaxonSummaryTaxon", "Taxon", new { taxon = ViewBag.Taxon }); %>    
    <% } %>
        

 
     <style type="text/css">
         div#fullContainer button.subscribeButton {
             margin-bottom: 0px;
             margin-top: 0px;
         }
     </style>
    <!-- Full container start -->
    <div id="fullContainer">
         <fieldset>            
            <h2 class="open"><%: DyntaxaResource.SubscriptionsSubscribe %></h2>
            <div class="fieldsetContent"> 
                <a class="ap-ui-button subscribeButton" style="font-weight: inherit; margin: 10px;" href="<%=Url.Action("Subscribe", new { taxonId=Model.CurrentTaxon.TaxonId, subscribeTaxonId = Model.CurrentTaxon.TaxonId }) %>">
                    <%: DyntaxaResource.SubscriptionsSubscribe %> <strong><%: Html.RenderTaxonText(Model.CurrentTaxon.ScientificName, Model.CurrentTaxon.CommonName, null, Model.CurrentTaxon.SortOrder) %></strong>
                </a>
                
            </div>
        </fieldset>

        <fieldset>
            <%--<h2 class="open"><%:Model.Labels.Input%></h2>--%>
            <h2 class="open"><%: DyntaxaResource.SubscriptionsMySubscriptions %></h2>
            <div class="fieldsetContent">
                <table class="">
                    <% foreach (TaxonViewModel item in Model.Subscriptions)
                       { %>
                        
                    <tr>                        
                        <td><%: Html.RenderTaxonLink(item.ScientificName, item.CommonName, item.SortOrder, "Info", "Taxon", new {@taxonId = item.TaxonId}) %></td>                        
                        <td><a class="ap-ui-button subscribeButton" style="margin-bottom: 0px; margin-top: 0px;" href="<%=Url.Action("Unsubscribe", new { taxonId=ViewBag.TaxonId, unsubscribeTaxonId = item.TaxonId }) %>"><%: DyntaxaResource.SubscriptionsUnsubscribe %></a></td>
                    </tr>
                    <% } %>
                </table>                                                                                   
            </div>            
        </fieldset>        
    <!-- full container end -->
    </div>        
</asp:Content>
