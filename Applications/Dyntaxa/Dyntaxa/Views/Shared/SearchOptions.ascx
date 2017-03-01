<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonSearchOptions>" %>

<%: Html.HiddenFor(m => m.RestrictToTaxonId) %>
<table>
<% if (Model.RestrictToTaxonId.HasValue)
    {%>
    <tr>
        <td>
            <%:Html.LabelFor(x => Model.RestrictToTaxonChildren, Model.RestrictToTaxonDescription)%>
        </td>
        <td>
            <%:Html.CheckBoxFor(x => Model.RestrictToTaxonChildren)%>
        </td>
    </tr>        
<% } %>
    <tr>
        <td>
            <%:Html.LabelFor(x => Model.NameCompareOperator, Model.Labels.TaxonNameCompareOperatorLabel)%>
        </td>
        <td>
            <%: Html.DropDownListFor(x => Model.NameCompareOperator, Model.CreateCompareOperatorSelectlist(Model.NameCompareOperator))%>
        </td>
    </tr>
    <tr>
        <td>
            <%:Html.LabelFor(x => Model.IsUnique, Model.Labels.IsUniqueLabel)%>
        </td>
        <td>
            <%: Html.DropDownListFor(x => x.IsUnique, Model.CreateNullableBoolSelectlist(Model.IsUnique))%>
        </td>
    </tr>
    <tr>
        <td>
            <%:Html.LabelFor(x => Model.IsValidTaxon, Model.Labels.IsValidTaxonLabel)%>
        </td>
        <td>
            <%: Html.DropDownListFor(x => Model.IsValidTaxon, Model.CreateIsValidSelectlist(Model.IsValidTaxon))%>
        </td>
    </tr>   
    <tr>
        <td>
            <%:Html.LabelFor(x => Model.IsRecommended, Model.Labels.IsRecommendedLabel)%>
        </td>
        <td>
            <%: Html.DropDownListFor(x => Model.IsRecommended, Model.CreateNullableBoolSelectlist(Model.IsRecommended))%>
        </td>
    </tr>
    <tr>
        <td>
            <%:Html.LabelFor(x => Model.IsOkForObsSystems, Model.Labels.IsOkForObsSystemLabel)%>
        </td>
        <td>
            <%: Html.DropDownListFor(x => Model.IsOkForObsSystems, Model.CreateNullableBoolSelectlist(Model.IsOkForObsSystems))%>
        </td>
    </tr>
    <tr>
        <td>
            <%:Html.LabelFor(x => Model.IsValidTaxonName, Model.Labels.IsValidTaxonNameLabel)%>
        </td>
        <td>
            <%: Html.DropDownListFor(x => Model.IsValidTaxonName, Model.CreateNullableBoolSelectlist(Model.IsValidTaxonName))%>
        </td>
    </tr>
    <tr>
        <td>
            <%:Html.LabelFor(x => Model.NameCategoryId, Model.Labels.TaxonNameCategoryLabel)%>
        </td>
        <td>
            <%: Html.DropDownListFor(x => Model.NameCategoryId, Model.CreateTaxonNameCategorySelectList(Model.NameCategoryId))%>
        </td>
    </tr>
    <% if (!Model.HideAuthorTextbox)
       { %>
    <tr>
        <td>
            <%:Html.LabelFor(x => Model.AuthorSearchString, Model.Labels.AuthorLabel)%>
        </td>
        <td>
            <%:Html.TextBoxFor(x => Model.AuthorSearchString)%>
        </td>
    </tr>
    <% } %>
    <tr class="date-input-row">
        <td>
            <%:Html.LabelFor(x => Model.LastUpdatedStartDate, Model.Labels.LastUpdatedLabel)%>                        
        </td>
        <td>
            <table style="margin:0px; border-style:none;">
                <tr>
                    <td><%: Model.Labels.LastUpdatedStartLabel %></td>
                    <td><%:Html.TextBoxFor(x => Model.LastUpdatedStartDate, new { style="width:120px; height:16px;", @class="date-input", @Value = Model.LastUpdatedStartDate.HasValue ? Model.LastUpdatedStartDate.Value.ToString("yyyy-MM-dd") : null })%></td>
                </tr>
                <tr>
                    <td><%: Model.Labels.LastUpdatedEndLabel %></td>
                    <td><%:Html.TextBoxFor(x => Model.LastUpdatedEndDate, new { style="width:120px; height:16px;", @class="date-input", @Value = Model.LastUpdatedEndDate.HasValue ? Model.LastUpdatedEndDate.Value.ToString("yyyy-MM-dd") : null})%></td>
                </tr>
            </table>            
        </td>        
    </tr>
</table>

<script type="text/javascript">
$(function () {
    var $datePicker = $('.date-input-row .date-input');

    if ($datePicker.length !== 0) {
        $datePicker.datepicker({
                showAnim: '',
                showOtherMonths: true,
                showWeek: true,
                changeMonth: true,
                changeYear: true,
                yearRange: 'c-100:c+10',
                duration: 0
        },
        $.datepicker.setDefaults($.datepicker.regional['sv']));
        //$.datepicker.setDefaults($.datepicker.regional['<%: System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName %>']));
    }
});

</script>