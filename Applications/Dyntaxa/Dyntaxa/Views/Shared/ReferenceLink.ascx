<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.RenderTaxonNameViewModel>" %>

<% if (Model.IsPublicMode)
   { %>
    <span class="referenceIcon" data-guid="<%:Model != null ? Model.TaxonName.GUID : ""%>"></span>
<% }
   else
   { %>                                                        
    <a href="<%:Url.Action(Model.ReferenceViewAction, "Reference", new {@guid = Model.TaxonName.GUID, taxonId = Model.TaxonId, returnController = Model.ReturnController, returnAction = Model.ReturnAction})%>"
        title="<%:Resources.DyntaxaResource.SharedReferenceLabel%> - <%:Model != null ? Model.TaxonName.Name : ""%>" 
        alt="<%:Resources.DyntaxaResource.SharedReferenceLabel%> - <%:Model != null ? Model.TaxonName.Name : ""%>">
        <span class="referenceIcon"></span>
    </a>
<% } %>