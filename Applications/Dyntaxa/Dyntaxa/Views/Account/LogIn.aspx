<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Rest.Master" Inherits="System.Web.Mvc.ViewPage<ArtDatabanken.WebApplication.Dyntaxa.Data.LogInModel>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Resources.DyntaxaResource.AccountLogInText %>
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">    
    
    <h1 class="readHeader"><%: Resources.DyntaxaResource.HomeIndexHeaderText %></h1>
    
    <% using (Html.BeginForm()) { %>
            
    <% if (!this.ViewData.ModelState.IsValid)
    { %>
    <fieldset id="validationSummaryContainer" class="validationSummaryFieldset">
        <h2 class="validationSummaryHeader">
            <%:Html.ValidationSummary(false, "")%>
        </h2>
    </fieldset>
    <% } %>

    <div id="leftContainer">

        <fieldset>
    
            <h2><%: Resources.DyntaxaResource.AccountLogInText%></h2>
            
            <fieldset class="loginFieldset">
                <legend><%: Resources.DyntaxaResource.SharedFieldsetLegendEditCreateText %></legend>            
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.UserName) %>
                </div>
                <div class="editor-field input-wrapper">
                    <%: Html.TextBoxFor(m => m.UserName) %>
                    <%: Html.ValidationMessageFor(m => m.UserName) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Password) %>
                </div>
                <div class="editor-field input-wrapper">
                    <%: Html.PasswordFor(m => m.Password) %>
                    <%: Html.ValidationMessageFor(m => m.Password) %>
                </div>
                
                <p>
                    <input type="submit" class="ap-ui-button" value="<%: Resources.DyntaxaResource.AccountLogInText %>" />                    
                </p>

            </fieldset>
            <div class="fieldsetDivMargin">
                <%//: Html.ActionLink(Resources.DyntaxaResource.AccountRegisterText, "Create")%>
                <%//: Html.ActionLink(Resources.DyntaxaResource.AccountResetPasswordTitleText, "ResetPassword")%>
                
                <% if (ViewBag.Debug)
                   { %>
                        <a href="<%: Resources.DyntaxaSettings.Default.UrlToUserAdminMoneses %>" target="_blank"><%: Resources.DyntaxaResource.AccountUserAdminLinkText%></a>
                <% }
                   else
                   { %>
                        <a href="<%: Resources.DyntaxaSettings.Default.UrlToUserAdminLampetra %>" target="_blank"><%: Resources.DyntaxaResource.AccountUserAdminLinkText %></a>
                <% } %>

                <%--<a href="https://lampetra2-1.artdata.slu.se/UserAdmin/Account/ResetPassword" target="_blank">Återställ lösenord</a>--%>

                
            </div>            
 
    <% } %>

            
            <br class="clear"/>      
                
            </fieldset>
        </div>

        <% if (ViewBag.Debug)
        { %>
            <div id="rightContainer">
            
                <fieldset>
                    <h2>Autologin</h2>
                    <div style="padding: 10px;">
                            <% //: Html.ActionLink(Resources.DyntaxaResource.AccountRegisterText, "Create")%>
                            <%--<%: Html.ActionLink(Resources.DyntaxaResource.AccountResetPasswordTitleText, "ResetPassword")%>--%>
                
                            <% /* TODO: inactivate autologin on publish (action to) */ %>            
                            <%:Html.ActionLink("testuser autologin", "AutoLogIn", "Account",
                                          new {returnUrl = Request.QueryString["ReturnUrl"] ?? ""}, null)%>
                            <br/>
                            <%:Html.ActionLink("testuser autologin english", "AutoLogInEN", "Account",
                                          new {returnUrl = Request.QueryString["ReturnUrl"] ?? ""}, null)%>
                            <%--<br/>
                             <%: Html.ActionLink("testuserOnlyEditor autologin", "AutoLogInOnlyEditor", "Account",
                                           new { returnUrl = Request.QueryString["ReturnUrl"] ?? "" }, null)%>--%>
                        
            
                    </div>
                </fieldset>

            </div>
        <% } %>
<%--    <% } %>--%>
</asp:Content>
