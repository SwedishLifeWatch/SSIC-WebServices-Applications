<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName.TaxonNameEditingViewModel>" %>

<input type="checkbox" name="IsNotOkForObs" value="<%= Model.CustomIdentifier %>" <% if (!Model.IsOkForObsSystems) { %> checked="checked" <% } %>  />


