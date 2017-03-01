<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.ErrorViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.ErrorTitleHeader%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm())
{%>
    
    <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
        <h2 class="validationSummaryHeader">
            <%:Html.ValidationSummary(false, "")%>
        </h2>
    </fieldset>
   
    <div id="fullContainer">
         <fieldset>
            <h1 class="readHeader"><%: Model.Labels.ErrorLabel + Model.ErrorMainHeader%></h1>    
            <div class="fieldsetContent">   
                <br/>
                <p>
                    <%: Html.TextAreaFor(model => model.ErrorInformationText, new { rows="10", style="width:600px;" })%>   
                </p>
                <br/>   
                <%: Html.HiddenFor(model => model.TaxonId) %>
            </div>
        </fieldset>
        <% if ((ViewBag.Debug != null && ViewBag.Debug) || (ViewBag.IsLoggedIn != null && ViewBag.IsLoggedIn))
           { %>
            <% if (!string.IsNullOrEmpty(Model.AdditionalErrorInformationText))
               { %> 
                    <fieldset>
                    <h2><%: Model.Labels.AdditionErrorLabel%></h2>    
                    <div class="fieldsetContent">   
                        <br/>
                            <div class="editor-field">
                                <p>
                                <%: Html.TextAreaFor(model => model.AdditionalErrorInformationText, new { style = "height: 500px;width:900px;" })%> 
                                </p>
                                
                            </div>
                        <br/>   
                    </div>
                </fieldset>
                 <% }%> 
             <%} %>
         <p>     
    
         </p>          
     </div>

<% } %>

</asp:Content>
