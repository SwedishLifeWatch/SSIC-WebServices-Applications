<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Resources.DyntaxaResource.SharedLicensingHeader %>
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
     <h1 class="readHeader">
       <%: Resources.DyntaxaResource.SharedLicensingHeader %>
      </h1>
   
        <div id="fullContainer">
        <fieldset>
            <h2><%: Resources.DyntaxaResource.SharedLicensingSubHeader %></h2>
            <div class="fieldsetContent ">
                <p>
                    <%= Resources.DyntaxaResource.HomeLicensingInformationText %>
                    <br/> <br/>
                    <a href="<%= Resources.DyntaxaResource.SharedLicensingReadMoreLink %>" target="_blank"><%: Resources.DyntaxaResource.SharedLicensingReadMoreText %></a><br/>
                    <br/> 
                </p>
            </div>
        </fieldset>   
         <!-- full container end -->
    </div>
</asp:Content>
