<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Resources.DyntaxaResource.SharedAboutCookiesHeader %>
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
     <h1 class="readHeader">
       <%: Resources.DyntaxaResource.SharedAboutCookiesHeader %>
      </h1>
   
        <div id="fullContainer">
        <fieldset>
            <h2><%: Resources.DyntaxaResource.SharedAboutCookiesSubHeader %></h2>
            <div class="fieldsetContent ">
                <p>
                    <%= Resources.DyntaxaResource.HomeCookieInformationText %>
                    <br/> <br/>
                    <a href="<%= Resources.DyntaxaResource.SharedCookiesReadMoreLink %>" target="_blank"><%: Resources.DyntaxaResource.SharedCookiesReadMoreText %></a><br/>
                    <br/> 
                </p>
            </div>
        </fieldset>   
         <!-- full container end -->
    </div>
</asp:Content>
