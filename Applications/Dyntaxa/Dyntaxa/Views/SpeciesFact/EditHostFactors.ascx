<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.SpeciesFactViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>
<%@ Import Namespace="Dyntaxa.Helpers.Extensions" %>

  
<% if (Model.SpeciesFactViewModelHeaderItemList != null)
{
    for (int i = 0; i < Model.SpeciesFactViewModelHeaderItemList.Count; i++)
    { %>
        <% SpeciesFactViewModelItem headerItem = Model.SpeciesFactViewModelHeaderItemList[i].SpeciesFactViewModelItem; %>
        <% IList<SpeciesFactViewModelSubHeaderItem> subHeaderList = Model.SpeciesFactViewModelHeaderItemList[i].SpeciecFactViewModelSubHeaderItemList; %>
        <fieldset>
        <h2 class="open"><%: headerItem.MainHeader%></h2>
        <div class="fieldsetContent">   
                
        <% for (int j = 0; j < subHeaderList.Count; j++)
        { %>
            <% SpeciesFactViewModelItem subHeaderItem = subHeaderList[j].SpeciesFactViewModelItem; %>
            <% IList<SpeciesFactViewModelItem> factList = subHeaderList[j].SpeciesFactViewModelItemList; %>
                    
                <% if (subHeaderItem.IsSubHeader)
                { %>
                <h3 class="open"><%: subHeaderItem.SubHeader%></h3>
                     
                <% } %>
                <%
                else
                { %>
                    <h3 class="open" style="background-color:azure"><%: headerItem.MainHeader%></h3>       
            <%  } %>

                <div class="fieldsetSubContent fullWidth">                       
                        <% if (factList != null && factList.Count > 0)
                        { %>   
                            <%  var grid = new WebGrid(source: factList, defaultSort: "Title", rowsPerPage: 100); %>
                            <div id="grid" >                 
                                <%: grid.GetHtml(tableStyle: "editFactorTable", headerStyle: "head",alternatingRowStyle: "alt", 
                                                 displayHeader: true,
                                                 columns: 
                                                    grid.Columns
                                                    (grid.Column("FactorName", Model.Labels.SpeciesFactMainHeader, style: "factorName", format: (item) =>
                                                                                                {
                                                                                                    var factorItem = ((SpeciesFactViewModelItem)item.Value);
                                                                                                    if (factorItem.IsSuperiorHeader)
                                                                                                    return Html.Raw(string.Format("<strong>{0}</strong>", factorItem.SuperiorHeader));
                                                                                                    else
                                                                                                    return Html.RenderSpeciesFactHostName(factorItem.FactorName, factorItem.UseDifferentColor, factorItem.UseDifferentColorFromIndex);
                                                                                                }),   
                                                    grid.Column("FactorFieldValue", Model.FactorFieldValueTableHeader
                                                                                                    , style: "factorValue",
                                                                                format: (item) => 
                                                                                    {
                                                                                        var factorItem = ((SpeciesFactViewModelItem)item.Value);

                                                                                        if (factorItem.FieldValues != null)
                                                                                        {
                                                                                            return Html.Partial("~/Views/SpeciesFact/FactorEnumValues.ascx", (SpeciesFactViewModelItem)item.Value);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            return factorItem.FactorFieldValue;
                                                                                        }
                                                                                    }),
                                                                        
                                                       grid.Column("FactorFieldValue2", Model.FactorFieldValue2TableHeader
                                                                                                    , style: "factorValue2",
                                                                                format: (item) => 
                                                                                    {
                                                                                        var factorItem = ((SpeciesFactViewModelItem)item.Value);

                                                                                        if (factorItem.FieldValues2 != null)
                                                                                        {
                                                                                            return Html.Partial("~/Views/SpeciesFact/FactorDropDownValues.ascx", (SpeciesFactViewModelItem)item.Value);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            return factorItem.FactorFieldValue2;
                                                                                        }
                                                                                    }),
                                                                             
                                                     grid.Column("IndividualCategoryName", Model.Labels.SpeciesFactCategoryHeader, style: "factorCategory"),
                                                                    //grid.Column("FactorFieldComment", Model.Labels.SpeciesFactCommentHeader, style: "factorComment"),
                                                     grid.Column("Quality", Model.Labels.SpeciesFactQualityHeader,style: "factorQuality"),
                                                     grid.Column("columnlast", "", style: "factorChange", format: (item) =>
                                                                                {
                                                                                    var factorItem = ((SpeciesFactViewModelItem)item.Value);
                                                                                    if (factorItem.FieldValues != null && !factorItem.IsHost)
                                                                                    {

                                                                                        return Html.Raw("<a href=\"#\" " + "onclick=\"showCreateNewReferenceDialog(" + factorItem.FactorId + "," + factorItem.ReferenceId + "," + factorItem.IndividualCategoryId + "," + factorItem.HostId + "," + factorItem.MainParentFactorId + ");\">" + Resources.DyntaxaResource.SpeciesFactChangeFactorText + "</a>");
                                                                        
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        return string.Empty;
                                                                                    }
                                                                                })

                                                                                                                //   , grid.Column("FactorId", Model.Labels.SpeciesFactFactorId, style: "factorId")
                                                                    //grid.Column("FactorSortOrder", Model.Labels.SpeciesFactSortOrder, style: "factorSortOrder")
                                            )
                                        ) %>
                                 </div>
                 <% } %>
               
                </div>
            <% } %>
                    
        </div>
        </fieldset>
    <%}
 } %> 


