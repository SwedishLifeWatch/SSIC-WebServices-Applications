<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.AccessIsNotAllowedViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.Labels.Title %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Full container start -->
    <div id="fullContainer">
        <fieldset>
            <h2><%: Model.Labels.Title %></h2>
            <div class="fieldsetContent" style="margin: 10px;">
                <%: Model.Labels.AccessIsNotAllowed %>:<br/><br/>
                <em><%: Model.Url %></em>

            </div>
        </fieldset>

    <!-- Full container end -->
    </div>


    <h2></h2>

    
</asp:Content>
