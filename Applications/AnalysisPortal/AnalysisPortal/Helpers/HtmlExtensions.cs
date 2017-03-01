using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.UI;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ButtonGroups;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace AnalysisPortal.Helpers
{
    /// <summary>
    /// This class contains extension methods for the HtmlHelper obejct.
    /// </summary>
    public static class HtmlExtensions
    {
        /// <summary>
        /// Initialize attribues for bootstrap button
        /// </summary>
        /// <param name="buttonClass">Button class</param>
        /// <param name="href">Url</param>
        /// <param name="enabled">True if button should be enabled</param>
        /// <returns></returns>
        private static IDictionary<string, string> InitButtonAttributes(string buttonClass, string href, bool enabled)
        {
            var attributes = new Dictionary<string, string>()
            {
                { "href", href },
                { "class", string.Format("btn {0}", buttonClass) }
            };

            if (!enabled)
            {
                attributes.Add("disabled", "disabled");
            }

            return attributes;
        } 

        /// <summary>
        /// Get html for an element
        /// </summary>
        /// <param name="elementType">Type of element</param>
        /// <param name="attributes">Element attributes</param>
        /// <param name="innerHtml">Inner html of element</param>
        /// <returns></returns>
        private static string GetElementHtml(string elementType, IDictionary<string, string> attributes, string innerHtml = null)
        {
            //Create the link that will trigger the drop down and add the icon to the link
            var element = new TagBuilder(elementType)
            {
                InnerHtml = innerHtml
            };

            if (attributes == null)
            {
                return element.ToString(); 
            }
            
            foreach (var attribute in attributes)
            {
                element.MergeAttribute(attribute.Key, attribute.Value);
            }

            return element.ToString();
        }

        /// <summary>
        /// Get the HTML for a button
        /// </summary>
        /// <param name="buttonAttributes"></param>
        /// <param name="buttonInnerHtml"></param>
        /// <param name="containerInnerHtml"></param>
        /// <returns></returns>
        private static string GetBootstrapButtonHtml(IDictionary<string, string> buttonAttributes, string buttonInnerHtml = null, string containerInnerHtml = null)
        {
            //Create the link that will trigger the drop down and add the icon to the link
            return string.Format("{0}{1}", GetElementHtml("a", buttonAttributes, buttonInnerHtml), containerInnerHtml);
        }

        private static string GetUrl(HtmlHelper htmlHelper, string controller, string action)
        {
            var urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            return urlHelper.Action(action, controller);
        }

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

        // TODO: Test (MW)
        /// <summary>
        /// An ActionLink method with possibility to add one or more icons to the link
        /// </summary>
        /// <param name="htmlHelper">The htmlHelper</param>
        /// <param name="iconAttributes">The icon attributes</param>
        /// <param name="linkText">The link text</param>
        /// <param name="actionName">The Action name</param>
        /// <param name="controllerName">The Controller name</param>
        /// <returns>Returns anchor tag with icon</returns>
        public static MvcHtmlString IconActionLink(this HtmlHelper htmlHelper, IDictionary<string, object> iconAttributes, string linkText, string actionName, string controllerName)
        {
            var builder = new TagBuilder("i");
            builder.MergeAttributes(new RouteValueDictionary(iconAttributes));
            var link = htmlHelper.ActionLink("[replaceme] " + linkText, actionName, controllerName).ToHtmlString();
            return new MvcHtmlString(link.Replace("[replaceme]", builder.ToString()));
        }

        // TODO: Test (MW)
        /// <summary>
        /// An ActionLink method with possibility to add one or more icons to the link
        /// </summary>
        /// <param name="htmlHelper">The htmlHelper</param>
        /// <param name="icon">The icon css class</param>
        /// <param name="linkText">The link text</param>
        /// <param name="actionName">The Action name</param>
        /// <param name="controllerName">The Controller name</param>
        /// <param name="routeValues">The Route values.</param>
        /// <param name="htmlAttributes">The html attributes.</param>
        /// <returns>Returns anchor tag with icon</returns>
        public static MvcHtmlString IconActionLink(this HtmlHelper htmlHelper, string icon, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            var builder = new TagBuilder("i");
            builder.MergeAttribute("class", icon);
            var link = htmlHelper.ActionLink("[replaceme] " + linkText, actionName, controllerName, routeValues, htmlAttributes).ToHtmlString();
            return new MvcHtmlString(link.Replace("[replaceme]", builder.ToString()));
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
                    dic.Add(kvp.Key, kvp.Value);
            }
            return dic;
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


        public static SelectList CreateNullableBoolSelectlist(this HtmlHelper htmlHelper, bool? value)
        {
            return new SelectList(new[] {
                new SelectListItem() {Text = Resources.Resource.SharedDropDownAny, Value = null},
                new SelectListItem() {Text = Resources.Resource.SharedDropDownTrue, Value = bool.TrueString},
                new SelectListItem() {Text = Resources.Resource.SharedDropDownFalse, Value = bool.FalseString}                            
            }, "Value", "Text", value.HasValue ? value.Value.ToString() : "");
        }


        /// <summary>
        /// Creates a script import tag that imports a .js file
        /// that contains the information from a .resx resource file.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="filename">The name of the JavaScript file that will be referenced.</param>
        /// <param name="language">The language.</param>
        /// <param name="version">The version, used to add an ?ver={vernr} in the tag. This invalidates the browser cache.</param>
        /// <returns></returns>
        public static MvcHtmlString LocalizedJavaScriptImport(this HtmlHelper htmlHelper, string filename, string language, int version = 0)
        {
            if (string.IsNullOrEmpty(language))
                return MvcHtmlString.Create("");

            var cultureInfo = new CultureInfo(language);            
            string extension = string.Format(".{0}.js", cultureInfo.TwoLetterISOLanguageName);
            filename = Path.ChangeExtension(filename, extension);
            
            if (version > 0)
            {
                filename = string.Format("{0}?ver={1}", filename, version);
            }

            var tagBuilder = new TagBuilder("script");
            tagBuilder.Attributes.Add("type", "text/javascript");
            string applicationPath = htmlHelper.ViewContext.HttpContext.Request.ApplicationPath;            
            tagBuilder.Attributes.Add("src", string.Format("{0}/resources/{1}", applicationPath, filename).Replace("//", "/"));            
            string str = tagBuilder.ToString();
            return MvcHtmlString.Create(str);
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
                items.Add(String.Concat(name, "=", HttpUtility.UrlEncode(parameters[name].ToString())));
            return String.Join("&", items.ToArray());
        }

        public static MvcHtmlString RenderScientificName(this HtmlHelper htmlHelper, string name, string author, int sortOrder, bool isOriginal = false)
        {
            var result = new StringBuilder();

            ITaxonCategory genusTaxonCategory = CoreData.TaxonManager.GetTaxonCategory(CoreData.UserManager.GetCurrentUser(),
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
                result.Append("*");
            return MvcHtmlString.Create(result.ToString());
        }
   

        //public static MvcHtmlString RenderTaxonName(this HtmlHelper htmlHelper, TaxonNameViewModel model)
        //{
        //    return htmlHelper.RenderScientificName(model.Name, model.Author, model.IsScientificName ? model.TaxonCategorySortOrder : -1, model.IsOriginal);
        //}

        public static MvcHtmlString RenderScientificLink(this HtmlHelper htmlHelper, string name, int sortOrder, string action, string controller, object routeValues)
        {
            StringBuilder result = new StringBuilder();
            MvcHtmlString link = htmlHelper.ActionLink(name, action, controller, routeValues, null);

            ITaxonCategory genusTaxonCategory = CoreData.TaxonManager.GetTaxonCategory(CoreData.UserManager.GetCurrentUser(),
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
                return htmlHelper.RenderScientificLink(name, sortOrder, action, controller, routeValues);
            else
                return htmlHelper.RenderScientificLink(name, -1, action, controller, routeValues);
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
                url = urlHelper.Action(actionName, controllerName, routeValueDictionary);
            else
                url = urlHelper.Action(actionName, controllerName, routeValues);

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


        public static string HtmlTag(this HtmlHelper htmlHelper, HtmlTextWriterTag tag,
            object htmlAttributes, Func<HtmlHelper, string> action)
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

        public static MvcHtmlString TopMenuButton(this HtmlHelper htmlHelper, string linkText, string controller, string action, 
            string linkClass, string title, string iconClass, string linkId)
        {
            return TopMenuButton(htmlHelper, linkText, controller, action, linkClass, title, iconClass, null, linkId);
        }

        public static MvcHtmlString TopMenuButton(this HtmlHelper htmlHelper, string linkText, string controller, string action,
            string linkClass, string title, string iconClass, ButtonGroupModelBase buttonGroupModel, string linkId)
        {
            return TopMenuButton(htmlHelper, linkText, GetUrl(htmlHelper, controller, action), linkClass, title, iconClass, linkId, buttonGroupModel);
        }
        
        /// <summary>
        /// Method used to create a top menu navigation button
        /// </summary>
        /// <param name="htmlHelper">html helper to extend</param>
        /// <param name="linkText">Text displayed on button</param>
        /// <param name="linkUrl">Default link url</param>
        /// <param name="linkClass">Css class</param>
        /// <param name="title">Mouse hover text</param>
        /// <param name="iconClass">Button icon class</param>
        /// <param name="linkId">Id if link button</param>
        /// <param name="buttonGroupModel">Drop down menu items</param>
        /// <returns></returns>
        public static MvcHtmlString TopMenuButton(this HtmlHelper htmlHelper, string linkText, string linkUrl,
            string linkClass, string title, string iconClass = null, string linkId = null, ButtonGroupModelBase buttonGroupModel = null)
        {
            var dropDownHtml = string.Empty;
            var buttonAttributes = InitButtonAttributes(
                linkClass,
                linkUrl,
                true);

            buttonAttributes.Add("id", linkId);
            buttonAttributes.Add("title", title);
            buttonAttributes.Add("data-placement", "top");

            var iconHtml = string.Empty;
            if (!string.IsNullOrEmpty(iconClass))
            {
                var icon = new TagBuilder("i");
                icon.MergeAttribute("class", string.Format("icon-white {0}", iconClass));

                if (!string.IsNullOrEmpty(linkText))
                {
                    icon.MergeAttribute("style", "margin-right: 10px;");
                }

                // iconHtml = string.Format("{0}{1}", icon, string.IsNullOrEmpty(linkText) ? "" : "&nbsp;&nbsp;");
                iconHtml = icon.ToString();
            }

            var buttonTitle = iconHtml + linkText;

            if (buttonGroupModel != null)
            {
                var menuLinks = string.Empty;
                linkUrl = null; //Reset link url we use first menu item url

                //For each first level item
                foreach (var button in buttonGroupModel.Buttons)
                {
                    if (!string.IsNullOrEmpty(button.Title))
                    {
                        if (linkUrl == null) // Set button url to first link url
                        {
                            linkUrl = GetUrl(htmlHelper, button.DynamicPageInfo.Controller, button.DynamicPageInfo.Action);
                        }
                        
                        //bool isButtonAncestorToCurrentPage = PageInfoManager.IsPageEqualToOrAncestorToOtherPage(button.DynamicPageInfo, SessionHandler.CurrentPage);                        
                        //var cssClass = isButtonAncestorToCurrentPage ? "btn-active" : "";
                        var cssClass = button.IsCurrent ? "btn-active" : "";
                        var subMenuHtml = string.Empty;

                        if (button.Children != null && button.Children.Count != 0)
                        {
                            var subMenuLinks = string.Empty;
                            cssClass += " dropdown-submenu";

                            //For each second level item
                            foreach (var subMenuButton in button.Children)
                            {   
                                //Declare info icon attributes 
                                var subLinkInfoAttributes = new Dictionary<string, object>()
                                {
                                    { "class", "icon-info" },
                                    { "title", subMenuButton.Tooltip },
                                    { "data-placement", "left" },
                                    { "style", "padding: 0 5px 0 5px; margin-left: -15px;" }
                                };

                                //Add menu item to submenu
                                subMenuLinks += new TagBuilder("li")
                                {
                                    Attributes = { new KeyValuePair<string, string>("class", subMenuButton.IsCurrent ? "btn-active" : "") },
                                    InnerHtml = htmlHelper.IconActionLink(
                                        subLinkInfoAttributes,
                                        subMenuButton.Title,
                                        subMenuButton.DynamicPageInfo.Action,
                                        subMenuButton.DynamicPageInfo.Controller
                                        ).ToHtmlString()
                                }.ToString();
                            }

                            //Create the sub menu and add all links to it
                            subMenuHtml = new TagBuilder("ul")
                            {
                                Attributes = { new KeyValuePair<string, string>("class", "dropdown-menu") },
                                InnerHtml = subMenuLinks
                            }.ToString();
                        }

                        //Declare info icon attributes 
                        var menuLinkInfoAttributes = new Dictionary<string, object>()
                        {
                            { "class", "icon-info" },
                            { "title", button.Tooltip },
                            { "data-placement", "left" },
                            { "style", "padding: 0 5px 0 5px; margin-left: -15px;" }
                        };

                        //Add a menu link
                        menuLinks += new TagBuilder("li")
                        {
                            Attributes = { new KeyValuePair<string, string>("class", cssClass) },
                            InnerHtml = htmlHelper.IconActionLink(
                                menuLinkInfoAttributes,
                                button.Title,
                                button.DynamicPageInfo.Action,
                                button.DynamicPageInfo.Controller
                                ).ToHtmlString() + subMenuHtml
                        }.ToString();
                    }
                }

                //Create the drop down menu
                var dropDownMenu = new TagBuilder("ul")
                {
                    Attributes = { new KeyValuePair<string, string>("class", "dropdown-menu") },
                    InnerHtml = menuLinks
                };

                //Create the down arrow
                buttonTitle += GetElementHtml(
                    "i", 
                    new Dictionary<string,string>()
                    {
                        { "class", "icon-caret-down icon-white" },
                        { "style", "margin-left: 10px;" }
                    }
                );

                buttonAttributes["href"] = "#";
                buttonAttributes.Add("data-toggle", "dropdown");
                
                //Create the link that will trigger the drop down and add the icon to the link
                dropDownHtml = dropDownMenu.ToString();
            }
            
            //Create a default link 
            var defaultLinkHtml = GetBootstrapButtonHtml(
                buttonAttributes,
                buttonTitle,
                dropDownHtml
            ); 

            //Create a div and add all controls to it
            var btnGroup = new TagBuilder("div")
            {
                InnerHtml = defaultLinkHtml
            };
            btnGroup.MergeAttribute("class", "btn-group");

            return MvcHtmlString.Create(btnGroup.ToString());
        }

        /// <summary>
        /// Compose html for a state button
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="model"></param>
        /// <param name="isResultButton">True if it's a result button</param>
        /// <returns></returns>
        public static MvcHtmlString StateButton(this HtmlHelper htmlHelper, StateButtonModel model, bool isResultButton)
        {
            var buttonTooltip = model.Tooltip;
            if (string.IsNullOrEmpty(buttonTooltip))
            {
                buttonTooltip = string.Format("{0} {1}", Resources.Resource.SharedButtonSettingToolTip, model.Title);
            }

            var buttonClass = "btn-default";
            if (model.IsEnabled)
            {
                buttonClass =
                    model.IsCurrent ?
                        "btn-active"
                        :
                        isResultButton && model.HasSettings ?
                            "btn-success"
                            :
                            model.IsSettingsDefault ?
                                "btn-primary"
                                :
                                "btn-changedsettings";
            }

            var checkButtonIconClass = "icon-check-empty";
            var checkButtonTooltip = Resources.Resource.SharedButtonCheckToolTip;

            if (model.IsChecked)
            {
                checkButtonIconClass = "icon-check";
                checkButtonTooltip = Resources.Resource.SharedButtonUnCheckToolTip;
            }

            var buttonHtml = string.Empty;

            if (!isResultButton)
            {
                var infoAttributes = InitButtonAttributes(
                    string.Format("noTooltip {0}", buttonClass),
                    null,
                    true);

                infoAttributes.Add("title", model.Title);
                infoAttributes.Add("data-toggle", "popover");
                infoAttributes.Add("data-placement", "top");
                infoAttributes.Add("data-trigger", "click");
                infoAttributes.Add("data-content", buttonTooltip);

                buttonHtml += GetElementHtml(
                    "span",
                    infoAttributes,
                    GetElementHtml("i", new Dictionary<string, string>()
                    {
                        { "class", "icon-white icon-info" }
                    }));
            }

            if (model.Identifier == StateButtonIdentifier.DataProvidersMetadataSearch)
            {
                var attributes = InitButtonAttributes(
                    buttonClass,
                    GetUrl(htmlHelper, model.DynamicPageInfo.Controller, model.DynamicPageInfo.Action),
                    model.IsEnabled);

                attributes.Add("title", buttonTooltip);
                attributes.Add("data-placement", "bottom");

                buttonHtml += GetBootstrapButtonHtml(
                    attributes,
                    string.Format(
                        "{0}&nbsp;{1}",
                        model.Title,
                        GetElementHtml("i", new Dictionary<string, string>()
                        {
                            { "class", "icon-search icon-white" }
                        })));
            }
            else
            {
                if (model.ShowCheckbox && !isResultButton)
                {
                    var attributes = InitButtonAttributes(
                        buttonClass,
                        "#",
                        model.IsEnabled);

                    attributes.Add("title", checkButtonTooltip);
                    attributes.Add("data-placement", "bottom");
                    attributes.Add(
                        "onclick",
                        string.Format("AnalysisPortal.checkBoxClick(this, {0}); return false;", (int)model.Identifier));

                    buttonHtml += GetBootstrapButtonHtml(
                        attributes,
                        GetElementHtml("i", new Dictionary<string, string>()
                        {
                            { "class", string.Format("{0} icon-white", checkButtonIconClass) }
                        }));
                }

                var buttonAttributes = InitButtonAttributes(
                    buttonClass,
                    GetUrl(htmlHelper, model.DynamicPageInfo.Controller, model.DynamicPageInfo.Action),
                    model.IsEnabled);

                buttonHtml += GetBootstrapButtonHtml(
                    buttonAttributes,
                    string.Format(
                        "{0}{1}",
                        string.IsNullOrEmpty(model.Title) ? string.Empty : string.Format("{0}&nbsp;&nbsp;", model.Title),
                        isResultButton ?
                        GetElementHtml("i", new Dictionary<string, string>()
                        {
                            { "class", string.Format("icon-white {0}", "icon-bar-chart") },
                            { "data-identifier", model.Identifier.ToString() }
                        }) : null));

                if (model.Children != null)
                {
                    var dropDownAttributes = InitButtonAttributes(
                        buttonClass,
                        "#",
                        model.IsEnabled);

                    dropDownAttributes.Add("data-toggle", "dropdown");

                    //Create the link that will trigger the drop down and add the icon to the link
                    buttonHtml += GetBootstrapButtonHtml(
                        dropDownAttributes,
                        GetElementHtml("i", new Dictionary<string, string>()
                        {
                            { "class", "icon-caret-down icon-white" }
                        }));

                    //Create the drop down menu
                    var dropDownMenu = new TagBuilder("ul")
                    {
                        InnerHtml = string.Join(
                            "",
                            from b in model.Children
                            where !string.IsNullOrEmpty(b.Title)
                            select new TagBuilder("li")
                            {
                                Attributes = { new KeyValuePair<string, string>("class", b.IsCurrent ? "btn-active" : "") },
                                InnerHtml = htmlHelper.ActionLink(
                                    b.Title,
                                    b.DynamicPageInfo.Action,
                                    b.DynamicPageInfo.Controller,
                                    null,
                                    b.IsEnabled ? (object)new { title = b.Tooltip, data_placement = "right" } : new { disabled = "disabled" }).ToHtmlString()
                            }.ToString())
                    };
                    dropDownMenu.MergeAttribute("class", "dropdown-menu");

                    buttonHtml += dropDownMenu.ToString();
                }
            }

            //Create a div and add all controls to it
            var btnGroup = new TagBuilder("div")
            {
                InnerHtml = buttonHtml
            };
            btnGroup.MergeAttribute("class", "btn-group");

            return MvcHtmlString.Create(btnGroup.ToString());
        }
    }
}