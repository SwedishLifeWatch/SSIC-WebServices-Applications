<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<ul>
      
    <li>
        
         <%
        if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName != "sv")
            {
        %>
                <a style="float:right" title="<%:Resources.DyntaxaResource.SharedChangeLanguageLabel %>" href="<%= Url.Action("SetCulture", "Culture", new { culture = "sv-SE", returnUrl = ViewContext.HttpContext.Request.Url.PathAndQuery }) %>">
                    <span title="Svenska" class="IconFlag IconFlag-sv"></span>
                    <%--&nbsp;- <%: Resources.DyntaxaResource.SharedChangeLanguageLabel %>--%>
                </a>
        <%
            }
            else
            {
        %>
                <a style="float:right" title="<%: Resources.DyntaxaResource.SharedChangeLanguageLabel %>" href="<%= Url.Action("SetCulture", "Culture", new { culture = "en-GB", returnUrl = ViewContext.HttpContext.Request.Url.PathAndQuery }) %>">
                    <span title="English" class="IconFlag IconFlag-en"></span>
                    <!--&nbsp;- <%: Resources.DyntaxaResource.SharedChangeLanguageLabel %>-->
                </a>
        <%
            }
        %>
        
               
        <% if (ViewBag.IsLoggedIn != null && ViewBag.IsLoggedIn)
           { %>
                <%: ViewBag.UserName%> - [
                <a href="<%=Url.Action("Logout", "Account")%>">
                    <%: Resources.DyntaxaResource.AccountLogOutText %>
                </a> ]
        <% }
           else
           { %>
                [ <a href="<%= Url.Action("LogIn", "Account", new { returnUrl = ViewContext.HttpContext.Request.Url.PathAndQuery }) %>">
                    <%: Resources.DyntaxaResource.AccountLogInText %>
                </a> ]
        <% } %>        
       
    </li>
    
    
    <% if (ViewBag.IsLoggedIn != null && ViewBag.IsLoggedIn && ViewBag.RevisionId != null && ViewBag.RevisionId != 0) %>
    <% { %>
        <li class="revisionHeaderInfo">
            <%: Html.ActionLink(Resources.DyntaxaResource.RevisionStopMainHeaderText, "StopEditingRevision", "Revision", new { revisionId = ViewBag.RevisionId }, null)%>
            <%--<%: Html.ActionLink(Resources.DyntaxaResource.RevisionStopMainHeaderText + " " + Resources.DyntaxaResource.SharedCurrentRevision,"StopEditingRevision", "Revision", new { revisionId = ViewBag.RevisionId }, null)%>--%>
            <%--<%: string.Format(Resources.DyntaxaResource.SharedCurrentRevision, ViewBag.RevisionId ?? "0") %> - <a href="<%= Url.Action("StartEditing", "Revision", null) %>"><%:Resources.DyntaxaResource.SharedViewLabel %></a>--%>
        </li>
    <% } %>
    
</ul>