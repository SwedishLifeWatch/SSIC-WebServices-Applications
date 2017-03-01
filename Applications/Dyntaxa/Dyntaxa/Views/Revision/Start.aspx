<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.RevisionCommonInfoViewModel>" %>
<%@ Import Namespace="ArtDatabanken.WebApplication.Dyntaxa.Data" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.RevisionEditingActionHeaderText%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <h1 class="readHeader">
        <%: Model.RevisionEditingHeaderText%>
    </h1>

<%--    <% using (Html.BeginForm("ListEvent", "Revision", FormMethod.Post, new { @id = "commonInfoForm", @name = "commonInfoForm" }))
       {%>--%>
    <!-- Full container start -->
    <div id="fullContainer">
       <%: Html.HiddenFor(model => model.Submit)%>
        <% foreach (RevisionInfoItemModelHelper item in Model.RevisionInfoItems)
           { %>
                <%: Html.Partial("~/Views/Shared/RevisionInfoControl.ascx", item)%>
        <% } %>
        <% if (!Model.Submit)
           { %>
        <fieldset>
          <div class="fieldsetDivMargin">
                 
                  <%: Html.ActionLink(Model.Labels.ReturnToListText, "List", "Revision", new { @class = "ap-ui-button", @style = "margin-top: 10px;" })%>     
                    
          </div>
        </fieldset>
        <% } %>   
         <!-- full container end -->
    </div>
<%--    <%} %>--%>


    
       <!-- This code will be executed in the partial view RevisionInfoControl.ascx. Addressed when document has been loaded. -->
       <script type="text/javascript">
           $(document).ready(function () {

                <% if (!(Model.RevisionInfoItems != null && Model.RevisionInfoItems.Count > 0))
                { %>  
                    showInfoDialog("<%:Model.DialogTitlePopUpText%>", "<%:Model.DialogTextPopUpText%>",  "<%:Model.Labels.ConfirmTextPopUpText%>",null);
               <% } %>
                
           });
           

        </script>

</asp:Content>
