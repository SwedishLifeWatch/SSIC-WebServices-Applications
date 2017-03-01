<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Resources.DyntaxaResource.AccountLogOutText %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1 class="readHeader"><%: Resources.DyntaxaResource.AccountLogOutText %></h1>
        
    <% using (Html.BeginForm("LogOut", "Account", new { returnUrl = Url.Action("LogIn") }))
       {%>
    
    <!-- Full container start -->
    <div id="fullContainer">
        <fieldset>            
            <h2><%: Resources.DyntaxaResource.SharedInfo %></h2>
            <div class="fieldsetContent" style="margin: 10px;">
                <%: Resources.DyntaxaResource.AccountLogOutInfoLine1 %><br/>
                <%: Resources.DyntaxaResource.AccountLogOutInfoLine2 %><br/>
                <input id="btnPost" type="submit" value="<%: Resources.DyntaxaResource.AccountLogOutText %>" class="ap-ui-button" />            
            </div>
        </fieldset>
        
    <!-- Full container end -->
    </div>
    <% } %>
</asp:Content>
