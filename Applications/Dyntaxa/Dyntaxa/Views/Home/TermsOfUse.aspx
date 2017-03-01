<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Resources.DyntaxaResource.SharedTermsOfUseText %>
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
     <h1 class="readHeader">
       <%: Resources.DyntaxaResource.SharedTermsOfUseText%>
      </h1>
   
        <div id="fullContainer">
        <fieldset>
            <h2><%: Resources.DyntaxaResource.SharedDialogInformationHeader%></h2>
            <div class="fieldsetContent">
                <ul>
                    <li>TBD</li>
                </ul>
            </div>
        </fieldset>   
         <!-- full container end -->
    </div>
</asp:Content>
