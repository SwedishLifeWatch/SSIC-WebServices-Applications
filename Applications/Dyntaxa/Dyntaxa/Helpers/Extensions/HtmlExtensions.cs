// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HtmlExtensions.cs" company="Artdatabanken SLU">
//   Copyright (c) 2009 Artdatabanken SLU. All rights reserved.
// </copyright>
// <summary>
//   This class contains extension methods for the HtmlHelper obejct.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.UI;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using System.Web;

namespace Dyntaxa.Helpers.Extensions
{
    /// <summary>
    /// This class contains extension methods for the HtmlHelper obejct.
    /// </summary>
    public static class HtmlExtensions
    {
        /// <summary>
        /// An ActionLink method that accepts MvcHtmlString as linktext but runs HTML encoding on it.
        /// </summary>
        /// <param name="htmlHelper">The extension point HtmlHelper.</param>
        /// <param name="linkText">The link text as MvcHtmlString.</param>
        /// <param name="action">The action.</param>
        /// <param name="controller">The controller.</param>
        /// <returns>Returns an anchor tag containing the virtual path to the specified action.</returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, MvcHtmlString linkText, string action, string controller)
        {
            return ActionLink(htmlHelper, linkText, action, controller, null);
        }

        /// <summary>
        /// An ActionLink method that accepts MvcHtmlString as linktext but runs HTML encoding on it.
        /// </summary>
        /// <param name="htmlHelper">The extension point HtmlHelper.</param>
        /// <param name="linkText">The link text as MvcHtmlString.</param>
        /// <param name="action">The action.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>Returns an anchor tag containing the virtual path to the specified action.</returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, MvcHtmlString linkText, string action, string controller, object routeValues)
        {
            return ActionLink(htmlHelper, linkText, action, controller, routeValues, null);
        }

        /// <summary>
        /// An ActionLink method that accepts MvcHtmlString as linktext but runs HTML encoding on it.
        /// </summary>
        /// <param name="htmlHelper">The extension point HtmlHelper.</param>
        /// <param name="linkText">The link text as MvcHtmlString.</param>
        /// <param name="action">The action.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The html attributes.</param>
        /// <returns>Returns an anchor tag containing the virtual path to the specified action.</returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, MvcHtmlString linkText, string action, string controller, object routeValues, object htmlAttributes)
        {
            return htmlHelper.ActionLink(linkText.ToHtmlString(), action, controller, routeValues, htmlAttributes);
        }

        /// <summary>
        /// An ActionLink method that has an inner SPAN tag
        /// </summary>
        /// <param name="htmlHelper">The extension point HtmlHelper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">The action name.</param>
        /// <param name="controllerName">The controller name.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The html attributes.</param>
        /// <param name="innerSpanHtmlAttributes">The inner span html attributes.</param>
        /// <returns>Returns an anchor tag containing the virtual path to the specified action.</returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes, object innerSpanHtmlAttributes)
        {
            var tagBuilder = new TagBuilder("a");
            var spanTagBuilder = new TagBuilder("span");
            var outerSpanTagBuilder = new TagBuilder("span");
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            tagBuilder.MergeAttribute("href", urlHelper.Action(actionName, controllerName, routeValues));
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            spanTagBuilder.MergeAttributes(new RouteValueDictionary(innerSpanHtmlAttributes));
            spanTagBuilder.SetInnerText(linkText);
            spanTagBuilder.InnerHtml = spanTagBuilder.InnerHtml.Replace("&amp;", @"<span class=""amp"">&amp;</span>");
            outerSpanTagBuilder.MergeAttribute("class", "rightcorner");
            outerSpanTagBuilder.InnerHtml = spanTagBuilder.ToString();
            tagBuilder.InnerHtml = outerSpanTagBuilder.ToString();

            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        /// <summary>
        /// An ActionLink method that has an inner SPAN tag
        /// </summary>
        /// <param name="htmlHelper">The extension point HtmlHelper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">The action name.</param>
        /// <param name="controllerName">The controller name.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The html attributes.</param>
        /// <param name="innerSpanHtmlAttributes">The inner span html attributes.</param>
        /// <returns>Returns an anchor tag containing the virtual path to the specified action.</returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, MvcHtmlString linkText, string actionName, string controllerName, object routeValues, object htmlAttributes, object innerSpanHtmlAttributes)
        {
            var tagBuilder = GetTagBuilder(htmlHelper, actionName, controllerName, routeValues, htmlAttributes, innerSpanHtmlAttributes, linkText);
            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        /// <summary>
        /// A security aware ActionLink method with authorization
        /// </summary>
        /// <param name="htmlHelper">
        /// The html Helper.
        /// </param>
        /// <param name="sharedResource">
        /// The shared resource.
        /// </param>
        /// <param name="actionName">
        /// The action name.
        /// </param>
        /// <param name="controllerName">
        /// The controller name.
        /// </param>
        /// <returns>
        /// <returns>
        /// Returns an anchor tag containing the virtual path to the specified action.
        /// </returns>
        /// </returns>
        public static MvcHtmlString SecurityTrimmedActionLink(this HtmlHelper htmlHelper, MvcHtmlString sharedResource, string actionName, string controllerName)
        {
            return htmlHelper.HasActionPermission(actionName, controllerName) ? ActionLink(htmlHelper, sharedResource, actionName, controllerName, null, null) : MvcHtmlString.Create(string.Empty);
        }

        /// <summary>
        /// A security aware List Item with an ActionLink method with authorization
        /// </summary>
        /// <param name="htmlHelper">
        /// The html Helper.
        /// </param>
        /// <param name="sharedResource">
        /// The shared resource.
        /// </param>
        /// <param name="actionName">
        /// The action name.
        /// </param>
        /// <param name="controllerName">
        /// The controller name.
        /// </param>
        /// <param name="routeValues">
        /// The route Values.
        /// </param>
        /// <returns>
        /// <returns>
        /// Returns an anchor tag containing the virtual path to the specified action.
        /// </returns>
        /// </returns>
        public static MvcHtmlString SecurityTrimmedListItemWithActionLink(this HtmlHelper htmlHelper, MvcHtmlString sharedResource, string actionName, string controllerName, object routeValues)
        {
            var listItemTagBuilder = new TagBuilder("li");
            var tagBuilder = new TagBuilder("a");
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            tagBuilder.MergeAttribute("href", urlHelper.Action(actionName, controllerName, routeValues));
            tagBuilder.SetInnerText(sharedResource.ToString());
            listItemTagBuilder.InnerHtml = tagBuilder.ToString();

            return htmlHelper.HasActionPermission(actionName, controllerName) ? MvcHtmlString.Create(listItemTagBuilder.ToString()) : MvcHtmlString.Create(string.Empty);
        }

        /// <summary>
        /// A security aware ActionLink method with authorization
        /// Supports span tag
        /// </summary>
        /// <param name="htmlHelper">The extension point HtmlHelper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">The action name.</param>
        /// <param name="controllerName">The controller name.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The html attributes.</param>
        /// <param name="innerSpanHtmlAttributes">The inner span html attributes.</param>
        /// <returns>Returns an anchor tag containing the virtual path to the specified action.</returns>
        public static MvcHtmlString SecurityTrimmedTopLevelActionLink(this HtmlHelper htmlHelper, MvcHtmlString linkText, string actionName, string controllerName, object routeValues, object htmlAttributes, object innerSpanHtmlAttributes)
        {
            var tagBuilder = GetTagBuilder(htmlHelper, actionName, controllerName, routeValues, htmlAttributes, innerSpanHtmlAttributes, linkText);
            return htmlHelper.HasActionPermission(actionName, controllerName) ? MvcHtmlString.Create(tagBuilder.ToString()) : MvcHtmlString.Create(string.Empty);
        }

        /// <summary>
        /// Checks if the current user has action permissions on given controller and action
        /// </summary>
        /// <param name="htmlHelper">
        /// The html helper.
        /// </param>
        /// <param name="actionName">
        /// The action name.
        /// </param>
        /// <param name="controllerName">
        /// The controller name.
        /// </param>
        /// <returns>
        /// True if access is granted and false otherwise
        /// </returns>
        public static bool HasActionPermission(this HtmlHelper htmlHelper, string actionName, string controllerName)
        {
            // if the controller name is empty the ASP.NET convention is:
            // "we are linking to a different controller
            var controllerToLinkTo = string.IsNullOrEmpty(controllerName)
                                                    ? htmlHelper.ViewContext.Controller
                                                    : GetControllerByName(htmlHelper, controllerName);

            var controllerContext = new ControllerContext(htmlHelper.ViewContext.RequestContext, controllerToLinkTo);

            var controllerDescriptor = new ReflectedControllerDescriptor(controllerToLinkTo.GetType());

            var actionDescriptor = controllerDescriptor.FindAction(controllerContext, actionName);

            return ActionIsAuthorized(controllerContext, actionDescriptor);
        }

        /// <summary>
        /// Checks if the user is permitted to perform the requested action
        /// </summary>
        /// <param name="controllerContext">
        /// The controller context.
        /// </param>
        /// <param name="actionDescriptor">
        /// The action descriptor.
        /// </param>
        /// <returns>
        /// True if the current user is authorized and False otherwise
        /// </returns>
        private static bool ActionIsAuthorized(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            if (actionDescriptor == null)
            {
                return false; // action does not exist so say yes - should we authorise this?!
            }

            var authContext = new AuthorizationContext(controllerContext, actionDescriptor);

            // run each auth filter until on fails
            // performance could be improved by some caching
            foreach (var filter in FilterProviders.Providers.GetFilters(controllerContext, actionDescriptor))
            {
                IAuthorizationFilter authorizationFilter = filter.Instance as IAuthorizationFilter;

                if (authorizationFilter != null)
                {
                    authorizationFilter.OnAuthorization(authContext);

                    if (authContext.Result != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Get a Controller by Name
        /// </summary>
        /// <param name="helper">
        /// The helper.
        /// </param>
        /// <param name="controllerName">
        /// The controller name.
        /// </param>
        /// <returns>
        /// The controller as a ControllerBase
        /// </returns>
        private static ControllerBase GetControllerByName(HtmlHelper helper, string controllerName)
        {
            // Instantiate the controller and call Execute
            var factory = ControllerBuilder.Current.GetControllerFactory();

            var controller = factory.CreateController(helper.ViewContext.RequestContext, controllerName);

            if (controller == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentUICulture,
                        "Controller factory {0} controller {1} returned null",
                        factory.GetType(),
                        controllerName));
            }

            return (ControllerBase)controller;
        }

        /// <summary>
        /// Get a Tag Builder
        /// </summary>
        /// <param name="htmlHelper">
        /// The html helper.
        /// </param>
        /// <param name="actionName">
        /// The action name.
        /// </param>
        /// <param name="controllerName">
        /// The controller name.
        /// </param>
        /// <param name="routeValues">
        /// The route values.
        /// </param>
        /// <param name="htmlAttributes">
        /// The html attributes.
        /// </param>
        /// <param name="innerSpanHtmlAttributes">
        /// The inner span html attributes.
        /// </param>
        /// <param name="linkText">
        /// The link text.
        /// </param>
        /// <returns>
        /// The Tag builder
        /// </returns>
        private static TagBuilder GetTagBuilder(HtmlHelper htmlHelper, string actionName, string controllerName, object routeValues, object htmlAttributes, object innerSpanHtmlAttributes, MvcHtmlString linkText)
        {
            var tagBuilder = new TagBuilder("a");
            var spanTagBuilder = new TagBuilder("span");
            var outerSpanTagBuilder = new TagBuilder("span");
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            tagBuilder.MergeAttribute("href", urlHelper.Action(actionName, controllerName, routeValues));
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            spanTagBuilder.MergeAttributes(new RouteValueDictionary(innerSpanHtmlAttributes));
            ////spanTagBuilder.SetInnerText(linkText.ToHtmlString());
            ////spanTagBuilder.InnerHtml = spanTagBuilder.InnerHtml.Replace("&amp;", @"<span class=""amp"">&amp;</span>");
            outerSpanTagBuilder.MergeAttribute("class", "rightcorner");
            ////outerSpanTagBuilder.InnerHtml = spanTagBuilder.ToString();
            outerSpanTagBuilder.InnerHtml = spanTagBuilder + linkText.ToHtmlString().Replace("&amp;", @"<span class=""amp"">&amp;</span>");
            tagBuilder.InnerHtml = outerSpanTagBuilder.ToString();
            return tagBuilder;
        }

        /// <summary>
        /// Returns a dictionary with with all querystring values and routing data, except action and controller
        /// merged with the parameter routeValues.
        /// Can be used to preserve query string in links:
        /// ie: Html.ActionLink(action, controller, MergeRouteDataWithQueryString(new {@sort=asc}), null);
        /// </summary>        
        /// <returns></returns>
        public static RouteValueDictionary MergeRouteDataWithQueryString(this HtmlHelper helper, object routeValues)
        {
            return DictionaryHelper.MergeDictionaries(helper.GetUrlParametersDictionary(), routeValues);
        }

        /// <summary>
        /// Returns a dictionary with all querystring values and routing data, except action and controller
        /// </summary>        
        public static RouteValueDictionary GetUrlParametersDictionary(this HtmlHelper helper)
        {                 
            RouteValueDictionary routeDic = helper.GetRouteParametersDictionary();
            RouteValueDictionary queryDic = helper.ViewContext.RequestContext.HttpContext.Request.QueryString.ToRouteValueDictionary();
            return DictionaryHelper.MergeDictionaries(routeDic, queryDic);
        }

        /// <summary>
        /// Gets RouteData except action and controller value
        /// </summary>
        /// <returns></returns>
        public static RouteValueDictionary GetRouteParametersDictionary(this HtmlHelper helper)
        {                        
            RouteValueDictionary dic = new RouteValueDictionary();
            foreach (KeyValuePair<string, object> kvp in helper.ViewContext.RouteData.Values)
            {
                if (kvp.Key != "action" && kvp.Key != "controller")
                {
                    dic.Add(kvp.Key, kvp.Value);
                }
            }
            return dic;
        }

        /// <summary>
        /// Begins a conditional rendering statement
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="condition"></param>
        /// <param name="ifAction"></param>
        /// <returns></returns>
        public static ConditionalHtmlRender If(this HtmlHelper helper, bool condition, Func<HtmlHelper, string> ifAction)
        {
            return new ConditionalHtmlRender(helper, condition, ifAction);
        }

        public static MvcHtmlString RenderAddReferenceLink(this HtmlHelper htmlHelper, string guid, string linkText, string returnAction, string returnController, object returnParams, object htmlAttributes)
        {
            var dicReturnParams = new RouteValueDictionary(returnParams);
            var dic = new RouteValueDictionary
                                           {
                                               { "guid", guid },
                                               { "returnController", returnController },
                                               { "returnAction", returnAction }
                                           };
            if (dicReturnParams.Count > 0)
            {
                dic.Add("returnParameters", EncodeRouteQueryString(dicReturnParams));
            }

            var url = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            return htmlHelper.ActionLink(linkText, "Add", "Reference", dic, htmlAttributes.ToDictionary());
        }

        public static IDictionary<string, object> ToDictionary(this object data)
        {
            if (data == null)
            {
                return null; // Or throw an ArgumentNullException if you want 
            }

            const BindingFlags publicAttributes = BindingFlags.Public | BindingFlags.Instance;
            var dictionary = new Dictionary<string, object>();

            foreach (PropertyInfo property in
                     data.GetType().GetProperties(publicAttributes))
            {
                if (property.CanRead)
                {
                    dictionary.Add(property.Name, property.GetValue(data, null));
                }
            }
            return dictionary;
        } 

        public static MvcHtmlString RenderAddReferenceImageLink(this HtmlHelper htmlHelper, string guid, string returnAction, string returnController, object returnParams, bool showReferenceApplyMode = false)
        {                       
            var dicReturnParams = new RouteValueDictionary(returnParams);
            var dic = new RouteValueDictionary
                                {
                                    { "guid", guid },
                                    { "returnController", returnController },
                                    { "returnAction", returnAction },
                                    { "showReferenceApplyMode", showReferenceApplyMode }
                                };
            if (dicReturnParams.Count > 0)
            {
                dic.Add("returnParameters", EncodeRouteQueryString(dicReturnParams));
            }

            var url = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            return ImageLink(htmlHelper, url.Content("~/Images/icons/book-open-text.png"), "Reference", "Add", "Reference", dic, null, null);           
        }

        public static MvcHtmlString RenderInfoReferenceLink(this HtmlHelper htmlHelper, string guid, string returnAction, string returnController, object returnParams, bool showReferenceApplyMode = false)
        {
            var dicReturnParams = new RouteValueDictionary(returnParams);
            var dic = new RouteValueDictionary
                                           {
                                               { "guid", guid },
                                               { "returnController", returnController },
                                               { "returnAction", returnAction },
                                               { "showReferenceApplyMode", showReferenceApplyMode }
                                           };
            if (dicReturnParams.Count > 0)
            {
                dic.Add("returnParameters", EncodeRouteQueryString(dicReturnParams));
            }

            var url = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            return ImageLink(htmlHelper, url.Content("~/Images/icons/book-open-text.png"), "Reference", "Info", "Reference", dic, null, null);
        }

        public static SelectList CreateNullableBoolSelectlist(this HtmlHelper htmlHelper, bool? value)
        {            
            return new SelectList(
                new[] 
                {
                    new SelectListItem() { Text = Resources.DyntaxaResource.SharedDropDownAny, Value = null },
                    new SelectListItem() { Text = Resources.DyntaxaResource.SharedDropDownTrue, Value = bool.TrueString },
                    new SelectListItem() { Text = Resources.DyntaxaResource.SharedDropDownFalse, Value = bool.FalseString }
                }, 
                "Value", 
                "Text", 
                value.HasValue ? value.Value.ToString() : "");
        }

        /// <summary>
        /// Encodes a RouteValueDictionary into a string that can be used in the url as a querystring value
        /// </summary>
        /// <param name="parameters">the dictionary to encode</param>
        /// <returns></returns>
        private static String EncodeRouteQueryString(RouteValueDictionary parameters)
        {
            List<String> items = new List<String>();
            foreach (String name in parameters.Keys)
            {
                items.Add(String.Concat(name, "=", HttpUtility.UrlEncode(parameters[name].ToString())));
            }

            return String.Join("&", items.ToArray());
        } 

        public static MvcHtmlString RenderScientificName(this HtmlHelper htmlHelper, string name, string author, int sortOrder, bool isOriginal = false)
        {            
            var result = new StringBuilder();

            ITaxonCategory genusTaxonCategory = CoreData.TaxonManager.GetTaxonCategory(
                CoreData.UserManager.GetCurrentUser(),
                TaxonCategoryId.Genus);
            if (sortOrder >= genusTaxonCategory.SortOrder)
            {
                var tag = new TagBuilder("em");    
                tag.SetInnerText(name);
                result.Append(tag.ToString());
            }
            else
            {
                result.Append(htmlHelper.Encode(name));
            }

            if (!string.IsNullOrEmpty(author))
            {
                result.Append(" ");
                result.Append(htmlHelper.Encode(author));
            }
            if (isOriginal)
            {
                result.Append("*");
            }

            return MvcHtmlString.Create(result.ToString());            
        }

        /// <summary>
        /// Creates different colors in a host string for species fact.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="useDifferentColor"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static MvcHtmlString RenderSpeciesFactHostName(this HtmlHelper htmlHelper, string name, bool useDifferentColor, int startIndex)
        {
            var result = new StringBuilder();

            string str2 = string.Empty;                                                      
            if (useDifferentColor)
            {
                if (startIndex > 1)
                {
                    string str1 = name.Substring(0, startIndex);
                    result.Append(str1);
                    str2 = name.Substring(startIndex, name.Length - startIndex);
                }
                else
                {
                    str2 = name.Substring(startIndex - 1, name.Length);
                }
                
                var tag = new TagBuilder("span");
                tag.AddCssClass("speciesFactHostColored");
                tag.SetInnerText(str2);
                result.Append(tag.ToString());
            }
            else
            {
                result.Append(htmlHelper.Encode(name));
            }

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString RenderTaxonNameWithStatus(this HtmlHelper htmlHelper, TaxonNameViewModel taxonName)
        {            
            string name = taxonName.Name;
            string author = taxonName.Author;
            int sortOrder = taxonName.IsScientificName ? taxonName.TaxonCategorySortOrder : -1;
            bool isOriginal = taxonName.IsOriginal;

            var result = new StringBuilder();

            ITaxonCategory genusTaxonCategory = CoreData.TaxonManager.GetTaxonCategory(
                CoreData.UserManager.GetCurrentUser(),
                TaxonCategoryId.Genus);
            if (sortOrder >= genusTaxonCategory.SortOrder)
            {
                var tag = new TagBuilder("em");
                tag.SetInnerText(name);
                result.Append(tag.ToString());
            }
            else
            {
                result.Append(htmlHelper.Encode(name));
            }

            if (!string.IsNullOrEmpty(author))
            {
                result.Append(" ");
                result.Append(htmlHelper.Encode(author));
            }
            if (isOriginal)
            {
                result.Append("*");
            }

            List<string> extraInfo = new List<string>();

            // Spelling variant
            if (taxonName.NameUsageId == (int)TaxonNameUsageId.Accepted &&
                taxonName.IsScientificName && !taxonName.IsRecommended)
            {
                extraInfo.Add(Resources.DyntaxaResource.TaxonNameSharedSpellingVariant);                
            }

            // Heterotypic or Homotypic
            if (taxonName.NameUsageId == (int)TaxonNameUsageId.Heterotypic ||
                taxonName.NameUsageId == (int)TaxonNameUsageId.Homotypic)
            {
                extraInfo.Add(taxonName.NameUsage);                
            }

            // Name status isn't Correct.
            if (taxonName.NameStatusId != (int)TaxonNameStatusId.ApprovedNaming)
            {
                extraInfo.Add(taxonName.NameStatus);
            }
            else if (taxonName.CategoryTypeId == (int)TaxonNameCategoryTypeId.Identifier &&
                !(taxonName.NameStatusId == (int)TaxonNameStatusId.ApprovedNaming || taxonName.NameStatusId == (int)TaxonNameStatusId.Informal))
            { // Identifier status isn't correct.
                extraInfo.Add(taxonName.NameStatus);
            }

            if (extraInfo.IsNotEmpty())
            {
                result.Append(" [");
                result.Append(string.Join(", ", extraInfo));
                result.Append("]");
            }

            //// Add information inside [] if name status isn't Correct, 
            //// or name usage is Heterotypic or homotypic.
            //if (taxonName.NameStatusId != (int)TaxonNameStatusId.ApprovedNaming || taxonName.NameUsageId == (int)TaxonNameUsageId.Heterotypic || taxonName.NameUsageId == (int)TaxonNameUsageId.Homotypic)
            //{
            //    bool itemHasBeenAdded = false;
            //    result.Append(" [");
            //    if (taxonName.NameUsageId == (int)TaxonNameUsageId.Accepted &&
            //        taxonName.IsScientificName && !taxonName.IsRecommended)
            //    {
            //        result.Append(Resources.DyntaxaResource.TaxonNameSharedSpellingVariant);
            //        itemHasBeenAdded = true;
            //    }

            //    if (taxonName.NameUsageId == (int)TaxonNameUsageId.Heterotypic ||
            //        taxonName.NameUsageId == (int)TaxonNameUsageId.Homotypic)
            //    {
            //        if (itemHasBeenAdded)
            //        {
            //            result.Append(", ");
            //        }

            //        result.Append(taxonName.NameUsage);
            //        itemHasBeenAdded = true;
            //    }

            //    if (taxonName.NameStatusId != (int)TaxonNameStatusId.ApprovedNaming)
            //    {
            //        if (itemHasBeenAdded)
            //        {
            //            result.Append(", ");
            //        }

            //        result.Append(taxonName.NameStatus);
            //    }

            //    result.Append("]");
            //}

            return MvcHtmlString.Create(result.ToString());  
        }

        public static MvcHtmlString RenderTaxonName(this HtmlHelper htmlHelper, TaxonNameViewModel model)
        {
            return htmlHelper.RenderScientificName(model.Name, model.Author, model.IsScientificName ? model.TaxonCategorySortOrder : -1, model.IsOriginal);
        }

        public static MvcHtmlString RenderTaxonText(this HtmlHelper htmlHelper, string scientificName, string commonName, string author, int sortOrder, bool isOriginal = false)
        {
            var result = new StringBuilder();
            ITaxonCategory genusTaxonCategory = CoreData.TaxonManager.GetTaxonCategory(CoreData.UserManager.GetCurrentUser(), TaxonCategoryId.Genus);
            if (sortOrder >= genusTaxonCategory.SortOrder)
            {
                var tag = new TagBuilder("em");
                tag.SetInnerText(scientificName);
                result.Append(tag);
            }
            else
            {
                result.Append(htmlHelper.Encode(scientificName));
            }

            if (!string.IsNullOrEmpty(author))
            {
                result.Append(" ");
                result.Append(htmlHelper.Encode(author));
            }

            if (!string.IsNullOrEmpty(commonName))
            {
                result.Append(htmlHelper.Encode(string.Format(" ({0})", commonName)));
            }

            if (isOriginal)
            {
                result.Append("*");
            }

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString RenderTaxonLink(this HtmlHelper htmlHelper, string scientificName, string commonName, int sortOrder, string action, string controller, object routeValues)
        {                                    
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            TagBuilder linkTag = new TagBuilder("a");
            string url = urlHelper.Action(action, controller, routeValues);
            linkTag.MergeAttribute("href", url);
            ITaxonCategory genusTaxonCategory = CoreData.TaxonManager.GetTaxonCategory(
                CoreData.UserManager.GetCurrentUser(),
                TaxonCategoryId.Genus);
            if (sortOrder >= genusTaxonCategory.SortOrder)
            {
                TagBuilder tag = new TagBuilder("em");
                tag.SetInnerText(scientificName);
                linkTag.InnerHtml = tag.ToString(TagRenderMode.Normal);                
            }
            else
            {
                linkTag.SetInnerText(scientificName);
            }

            if (!string.IsNullOrEmpty(commonName))
            {
                linkTag.InnerHtml += string.Format(" ({0})", commonName);                
            }

            return MvcHtmlString.Create(linkTag.ToString(TagRenderMode.Normal));      
        }

        public static MvcHtmlString RenderScientificLink(this HtmlHelper htmlHelper, string name, int sortOrder, string action, string controller, object routeValues)
        {                        
            StringBuilder result = new StringBuilder();
            MvcHtmlString link = htmlHelper.ActionLink(name, action, controller, routeValues, null);

            ITaxonCategory genusTaxonCategory = CoreData.TaxonManager.GetTaxonCategory(
                CoreData.UserManager.GetCurrentUser(),
                TaxonCategoryId.Genus);
            if (sortOrder >= genusTaxonCategory.SortOrder)
            {
                TagBuilder tag = new TagBuilder("em");                
                tag.InnerHtml = link.ToHtmlString();
                result.Append(tag.ToString());
            }
            else
            {
                result.Append(link.ToHtmlString());
            }
           
            return MvcHtmlString.Create(result.ToString());      
        }

        public static MvcHtmlString RenderTaxonLink(this HtmlHelper htmlHelper, string name, bool isScientificName, int sortOrder, string action, string controller, object routeValues)
        {
            if (isScientificName)
            {
                return htmlHelper.RenderScientificLink(name, sortOrder, action, controller, routeValues);
            }
            else
            {
                return htmlHelper.RenderScientificLink(name, -1, action, controller, routeValues);
            }
        }

        //public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes, object innerSpanHtmlAttributes)
        public static MvcHtmlString SpanTag(this HtmlHelper htmlHelper, string text, object htmlAttributes)
        {            
            var spanTagBuilder = new TagBuilder("span");
            spanTagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            spanTagBuilder.SetInnerText(text);
            spanTagBuilder.InnerHtml = spanTagBuilder.InnerHtml.Replace("&amp;", @"<span class=""amp"">&amp;</span>");
            return MvcHtmlString.Create(spanTagBuilder.ToString());
        }

        public static MvcHtmlString ImageLink(this HtmlHelper htmlHelper, string imgSrc, string alt, string actionName, string controllerName, object routeValues, object htmlAttributes, object imgHtmlAttributes)
        {
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            TagBuilder imgTag = new TagBuilder("img");
            imgTag.MergeAttribute("src", imgSrc);
            imgTag.MergeAttributes(new RouteValueDictionary(imgHtmlAttributes), true);
            RouteValueDictionary routeValueDictionary = routeValues as RouteValueDictionary;
            string url;
            if (routeValueDictionary != null)
            {
                url = urlHelper.Action(actionName, controllerName, routeValueDictionary);
            }
            else
            {
                url = urlHelper.Action(actionName, controllerName, routeValues);
            }

            TagBuilder imglink = new TagBuilder("a");
            imglink.MergeAttribute("href", url);
            imglink.InnerHtml = imgTag.ToString();
            imglink.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
            
            return MvcHtmlString.Create(imglink.ToString());
        }

        public static MvcHtmlString Image(this HtmlHelper htmlHelper, string imgSrc, string alt, object htmlAttributes)
        {            
            var imgTag = new TagBuilder("img");
            imgTag.MergeAttribute("src", imgSrc);
            imgTag.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);             
            return MvcHtmlString.Create(imgTag.ToString());
        } 

        public static string HtmlTag(
            this HtmlHelper htmlHelper, 
            HtmlTextWriterTag tag,
            object htmlAttributes, 
            Func<HtmlHelper, string> action)
        {
            var attributes = new RouteValueDictionary(htmlAttributes);

            using (var sw = new StringWriter())
            {
                using (var htmlWriter = new HtmlTextWriter(sw))
                {
                    // Add attributes
                    foreach (var attribute in attributes)
                    {
                        htmlWriter.AddAttribute(attribute.Key, attribute.Value != null ?
                    attribute.Value.ToString() : string.Empty);
                    }

                    htmlWriter.RenderBeginTag(tag);
                    htmlWriter.Write(action.Invoke(htmlHelper));
                    htmlWriter.RenderEndTag();
                }

                return sw.ToString();
            }
        }
    }

    public class ConditionalHtmlRender
    {
        private readonly HtmlHelper _helper;
        private readonly bool _ifCondition;
        private readonly Func<HtmlHelper, string> _ifAction;

        private readonly Dictionary<bool, Func<HtmlHelper, string>> _elseActions =
            new Dictionary<bool, Func<HtmlHelper, string>>();

        public ConditionalHtmlRender(HtmlHelper helper, bool ifCondition, Func<HtmlHelper, string> ifAction)
        {
            _helper = helper;
            _ifCondition = ifCondition;
            _ifAction = ifAction;
        }

        /// <summary>
        /// Ends the conditional block with an else branch
        /// </summary>
        /// <param name="renderAction"></param>
        /// <returns></returns>
        public ConditionalHtmlRender Else(Func<HtmlHelper, string> renderAction)
        {
            return ElseIf(true, renderAction);
        }

        /// <summary>
        /// Adds an else if branch to the conditional block
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="renderAction"></param>
        /// <returns></returns>
        public ConditionalHtmlRender ElseIf(bool condition, Func<HtmlHelper, string> renderAction)
        {
            _elseActions.Add(condition, renderAction);
            return this;
        }

        public override string ToString()
        {
            if (_ifCondition) // if the IF condition is true, render IF action
            {
                return _ifAction.Invoke(_helper);
            }

            // find the first ELSE condition that is true, and render it
            foreach (KeyValuePair<bool, Func<HtmlHelper, string>> elseAction in _elseActions)
            {
                if (elseAction.Key)
                {
                    return elseAction.Value.Invoke(_helper);
                }
            }

            // no condition true; render nothing
            return String.Empty;
        }
    }
}
