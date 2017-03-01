<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.RenderTaxonNameViewModel>" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>

<% if (Model != null)
{ %>

    <% if (Model.TaxonName != null && !string.IsNullOrEmpty(Model.TaxonName.GUID))
       {%>
        <span title="<%: Resources.DyntaxaResource.TaxonInfoNameTooltip %>" data-guid="<%:Model != null ? Model.TaxonName.GUID : ""%>" class="referenceTooltip">
            <strong><%:Model.Label%></strong>: <%:Html.RenderTaxonName(Model.TaxonName)%>             
        </span>        
        <%: Html.Partial("ReferenceLink", Model) %>        
    <% }
       else
       { %>        
        <strong><%:Model.Label%></strong>: 
        <% if (Model.TaxonName != null)
        { %></>
            <%:Html.RenderTaxonName(Model.TaxonName)%> 
     <%   }
           
    } %>


    
<% } %>
