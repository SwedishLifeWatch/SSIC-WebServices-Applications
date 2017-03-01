<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.Menu.MenuModel>" %>
<%@ Import namespace="Dyntaxa.Helpers.Extensions"  %>


<script type="text/javascript">

	// initialise plugins
    jQuery(function () {
        jQuery('div#menuContainer > ul.sf-menu').superfish({            
            delay: 100,
            animation: { opacity: 'show' },
            speed: 'fast',
            autoArrows: true,
            dropShadows: true
        }
        );

        $("div#menuContainer > ul.sf-menu").find('a[href!="#"]').click(function () {
            $.blockUI({ overlayCSS: { backgroundColor: 'transparent'}, message: '<h1><img src="<%= Url.Content("~/Images/Icons/ajax-loader.gif") %>" /> <%: Resources.DyntaxaResource.SharedLoading %></h1>' });
        });


    });

</script>

<%--<%: Html.Menu(Model) %>--%>

<ul class="sf-menu">

<% foreach (var menuItem in Model.MenuItems)
   { %>
       <li <%if(menuItem.Current){%> class="current" <%}%>>
            <% if (menuItem.ChildItems == null || menuItem.ChildItems.Count == 0)
               { %>
                <%: Html.ActionLink(menuItem.Text, menuItem.Action, menuItem.Controller, menuItem.Parameters, null)%>
            <% } else{ %>
                <a href="#"><%:menuItem.Text%></a>
                <ul>
                    <% foreach (var subItem in menuItem.ChildItems)
                        { %>       
                        
                                     
                        <li><%: Html.ActionLink(subItem.Text, subItem.Action, subItem.Controller, subItem.Parameters, null)%></li>                                                            
                    <% } %>
                </ul>
            <% } %>
            <% if (menuItem.ChildItems != null && menuItem.ChildItems.Count > 0)
               { %>
                <ul>
                <% foreach (var subItem in menuItem.ChildItems)
                    { %>                    
                    <li><%: Html.ActionLink(subItem.Text, subItem.Action, subItem.Controller, subItem.Parameters, null)%></li>                                                            
                <% } %>
                </ul>
            <% } %>
       </li>
       
   <% } %>
</ul>
