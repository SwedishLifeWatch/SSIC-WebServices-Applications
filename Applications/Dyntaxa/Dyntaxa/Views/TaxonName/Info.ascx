<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName.TaxonNameInfoViewModel>" %>

<table>
<tr>
    <td><%: Model.GUIDLabel %></td>
        <td><%: Model.GUID %></td>
    </tr>
    <tr>
        <td><%: Model.NameLabel %></td>
        <td><%: Model.Name %></td>
    </tr>
    <tr>
        <td><%: Model.AuthorLabel %></td>
        <td><%: Model.Author %></td>
    </tr>
    <tr>
        <td><%: Model.NameNameUsageLabel %></td>
        <td><%: Model.NameUsage %></td>
    </tr>
    <tr>
        <td><%: Model.NameNomenclatureLabel %></td>
        <td><%: Model.NameStatus %></td>
    </tr>
    <tr>
        <td><%: Model.NameCategoryLabel %></td>
        <td><%: Model.NameCategory %></td>
    </tr> 
    
    <tr>
        <td><%: Model.IsRecommendedLabel %></td>
        <td><%: (Model.IsRecommended ? Resources.DyntaxaResource.SharedBoolTrueText : Resources.DyntaxaResource.SharedBoolFalseText)%></td>
    </tr>    
    <tr>
        <td><%: Model.IsOriginalLabel %></td>
        <td><%: (Model.IsOriginal ? Resources.DyntaxaResource.SharedBoolTrueText : Resources.DyntaxaResource.SharedBoolFalseText)%></td>
    </tr>
    <tr>
        <td><%: Model.IsUniqueLabel %></td>
        <td><%: (Model.IsUnique ? Resources.DyntaxaResource.SharedBoolTrueText : Resources.DyntaxaResource.SharedBoolFalseText)%></td>        
    </tr>
    <tr>
        <td><%: Model.OkForObsSystemLabel %></td>
        <td><%: (Model.IsOkForSpeciesObservation ? Resources.DyntaxaResource.SharedBoolTrueText : Resources.DyntaxaResource.SharedBoolFalseText)%></td>
    </tr>
    <tr>
        <td><%: Model.CommentLabel %></td>
        <td><%: Model.Comment %></td>
    </tr>
    <tr>
        <td><%: Model.ModifiedLabel %></td>
        <td><%: Model.Modified %></td>
    </tr>
</table>
<%: Model.ReferenceLabel %>
<% Html.RenderAction("ListReferences", "Reference", new {@guid = Model.GUID}); %>
