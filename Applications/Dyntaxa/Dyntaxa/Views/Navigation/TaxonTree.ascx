<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.Navigation.TaxonTreeViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data.Navigation" %>

<% using (Html.BeginForm("SetSecondaryRelationsVisibility", "Navigation", FormMethod.Post, new {@id = "setSecondaryRelationsVisibilityForm", @name = "setSecondaryRelationsVisibilityForm"}))
   { %>
    <div>
        <input id="showSecondaryRelationsCheckbox" type="checkbox" <% if (Model.ShowSecondaryRelations.GetValueOrDefault(true)) { %> checked="checked" <% } %> /> 
        <%: Resources.DyntaxaResource.TaxonTreeShowSecondaryRelations %>
        <img src="<%= Url.Content("~/Images/Icons/secondary_relation.png") %>" alt="Secondary relation"/>
    </div>
    <%: Html.Hidden("returnUrl", Request.Url.PathAndQuery) %>
    <%: Html.Hidden("show", true) %>
<% } %>

<% using (Html.BeginForm("ChangeRootTaxon", "Navigation", FormMethod.Post, new { @id = "changeRootTaxonForm", @name = "changeRootTaxonForm" }))
   { %>

<% if (ViewBag.HideChangeRootTaxon == null || ViewBag.HideChangeRootTaxon == false)
   { %>
    <div id="navigateParentDiv">
        <div class="group">
               <%:Html.Label("selectRootTaxon", Resources.DyntaxaResource.TaxonTreeCurrentRootTaxon)%>                          
            <select id="selectRootTaxon" name="selectRootTaxon" style="max-width: 240px;">
                <% foreach (TaxonTreeViewTaxon item in Model.ParentTaxa)
                   { %>
                    <option <% if (item.IsCurrentRoot) {%> selected="selected" <% } %> value="<%=item.TaxonId%>"><%:item.Name%> (<%: item.Category%>)</option>               
              <% } %>                
            </select>
        </div>
      </div>   
<%} %>
    <%--<%: Html.DropDownListFor(x => x.SelectedCategoryId, new SelectList(Model.CategoryList, "Id", "Name"))%>--%>
<% } %>

<% using (Html.BeginForm("TaxonTreeNavigate", "Navigation", FormMethod.Post, new { @id = "navigateTaxonForm", @name = "navigateTaxonForm" }))
       { %>
        <%: Html.Hidden("NavigateId") %>     
        <%: Html.Hidden("ExpandedNodes") %>      
    <%} %>
	

	<div id="navigateTaxonTree"> </div>	
	
	<script type="text/javascript">

        $(document).ready(function () {
            $('#showSecondaryRelationsCheckbox').on('change', function() {
                $("#setSecondaryRelationsVisibilityForm input[name=show]").val($(this).is(":checked"));                
                $('#setSecondaryRelationsVisibilityForm').submit();                    
            });

            $("#selectRootTaxon").change(function() {
//                var value = $(this).val();
//                alert(value);
                var frm = document.changeRootTaxonForm;
                frm.submit();                
            });            
            
            $("#navigateTaxonTree").dynatree({
                onActivate: function (node) {
                    // Use status functions to find out about the calling context        
                    var isInitializing = node.tree.isInitializing(); // Tree loading phase        
                    var isReloading = node.tree.isReloading(); // Loading phase, and reading status from cookies        
                    var isUserEvent = node.tree.isUserEvent(); // Event was triggered by mouse or keyboard                            
 
                    var frm = document.navigateTaxonForm;
                    frm.NavigateId.value = node.data.key;

                    var expandedNodes = [];                    
                    this.visit(function(n) {
                        if (n.isExpanded())
                            expandedNodes.push(n.data.key);                            
                    }, true);
                    expandedNodes.shift(); // delete first node. Don't know why the element is there.
                    frm.ExpandedNodes.value = JSON.stringify(expandedNodes); 
                    
                    $.blockUI({ overlayCSS: { backgroundColor: 'transparent' }, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Resources.DyntaxaResource.SharedLoading %></h1>' });
                    frm.submit();                    
                },                          
                onLazyRead: function(node){   
                    node.appendAjax({
                        url: '<%= Url.Action("GetTreeData","Navigation") %>',                        
                        type: "GET",
                        data: {"taxonId": node.data.key},                         
                        dataType: "json",                           
                        success: function(node) {                               
                            // Called after nodes have been created and the waiting icon was removed.
                        },                           
                       cache: false // Append random '_' argument to url to prevent caching.                         
                    });    
                },                                                                                
                persist: false,
                generateIds: true,
                fx: { height: "toggle", duration: 100 }, 
                selectMode: 1,        
                clickFolderMode: 1,                                       
                children: <%= Model.GetDynatreeJson() %>
            });
                       
        });            
                                   
	</script>
