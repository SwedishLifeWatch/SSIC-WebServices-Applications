using System;
using System.Collections.Generic;
using System.Net;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.About;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers
{
    /// <summary>
    /// This class handles the retrievment of user instructions from EpiServer CMS.
    /// </summary>
    public static class AboutManager
    {
        private static AboutViewModel _aboutDataProviders;
        private static AboutViewModel _aboutFilters;
        private static AboutViewModel _aboutCalculations;
        private static AboutViewModel _aboutPresentations;
        private static AboutViewModel _aboutResults;

        private static AboutViewModel _aboutDataProvidersSv;
        private static AboutViewModel _aboutFiltersSv;
        private static AboutViewModel _aboutCalculationsSv;
        private static AboutViewModel _aboutPresentationsSv;
        private static AboutViewModel _aboutResultsSv;
        
        private const string BASE_URL = "http://www.svenskalifewatch.se/en/";
        private const string SWEDISH_BASE_URL = "http://www.svenskalifewatch.se/sv/";

        ////http://www.svenskalifewatch.se/en/guides/analysis-portal/
        ////http://www.svenskalifewatch.se/sv/guider/analysportalen/

        #region General private methods

        /// <summary>
        /// This method retrieves the html text from EpiServer based on an Url string.
        /// </summary>
        /// <param name="url">The Url to the web page holding the information of interest.</param>
        /// <returns>The html text of the web page.</returns>
        private static string GetPageAsText(String url)
        {
            string text = "";
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; rv:5.0) Gecko/20100101 Firefox/5.0";
                try
                {
                    text = client.DownloadString(url);
                }
                catch { }
            }
            return text;
        }

        /// <summary>
        /// General method that extract title from html.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static string GetTitleLabelFromHtml(string html)
        {
            string text = "";
            
            if (html.Length > 0)
            {
                int start = html.IndexOf("<h1>");
                text = html.Remove(0, start + "<h1>".Length);
                start = text.IndexOf("</h1>");
                text = text.Substring(0, start);
                text = text.Replace("\r\n", "");
                text = text.Trim();
            }

            return text;
        }

        /// <summary>
        /// General method that extracts Description text from html.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static string GetDescriptionFromHtml(string html)
        {
            string text = "";
            string marker = "<p class=" + '"' + "introduction" + '"' + ">";

            if (html.Length > 0)
            {
                int start = html.IndexOf(marker);
                text = html.Remove(0, start + marker.Length);
                start = text.IndexOf("</p>");
                text = text.Substring(0, start);
                text = text.Trim();
            }

            return text;
        }

        /// <summary>
        /// General method for cration of single about items.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="localeIsoCode"></param>
        /// <returns></returns>
        private static AboutItem CreateAboutItem(string url, string localeIsoCode)
        {
            AboutItem item = new AboutItem();
            string html = GetPageAsText(url);
            item.Header = GetTitleLabelFromHtml(html);
            item.Description = GetDescriptionFromHtml(html);
            item.ReadMoreLinkUrl = url;
            if (localeIsoCode == "sv-SE")
            {
                item.ReadMoreLinkLabel = "Läs mer...";
                item.ReadMoreLinkHint = "Läs mer om " + item.Header.ToLower();
            }
            else
            {
                item.ReadMoreLinkLabel = "Read more...";
                item.ReadMoreLinkHint = "Read more about " + item.Header.ToLower();
            }
            return item;
        }
        #endregion

        #region Data sources

        /// <summary>
        /// This method creates a new updated version of a locale specific view model for Data sources.
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="localeIsoCode"></param>
        /// <returns></returns>
        private static AboutViewModel CreateAboutDataProvidersViewModel(string baseUrl, string localeIsoCode)
        {
            AboutViewModel model = new AboutViewModel();
            AboutItem item = null;
            string html = GetPageAsText(baseUrl + Resource.AboutUrlData);
            if (html.IsNotEmpty())
            {
                model.TitleLabel = GetTitleLabelFromHtml(html);
                model.Description = GetDescriptionFromHtml(html);
                model.Items = new List<AboutItem>();

                item = CreateAboutItem(baseUrl + Resource.AboutUrlDataSpeciesObservations, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }

                item = CreateAboutItem(baseUrl + Resource.AboutUrlDataEnvironmentalData, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                   model.Items.Add(item);
                }

                item = CreateAboutItem(baseUrl + Resource.AboutUrlDataMapLayers, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }

                item = CreateAboutItem(baseUrl + Resource.AboutUrlDataMetadata, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }
            }
            return model;
        }

        /// <summary>
        /// Get a view model with information about Data and data sources.
        /// This functions returns cached data. Use the reset cach to get new updates of the about information texts.
        /// </summary>
        /// <param name="localeIsoCode">A string representing the locale ISO Code, e.g. "sv-SE".</param>
        /// <returns>The view model for a specified locale.</returns>
        public static AboutViewModel GetAboutDataProvidersViewModel(string localeIsoCode)
        {
            //Swedish locale
            if (localeIsoCode == "sv-SE")
            {
                if (_aboutDataProvidersSv.IsNull())
                {
                    _aboutDataProvidersSv = CreateAboutDataProvidersViewModel(SWEDISH_BASE_URL, localeIsoCode);
                }
                if (_aboutDataProvidersSv.TitleLabel.IsNotEmpty())
                {
                    return _aboutDataProvidersSv;
                }
            }

            //Default locale
            if (_aboutDataProviders.IsNull())
            {
                _aboutDataProviders = CreateAboutDataProvidersViewModel(BASE_URL, localeIsoCode);
            }
            return _aboutDataProviders;
        }
        #endregion

        #region Filters

        /// <summary>
        /// This method creates a new updated version of a locale specific view model for filters.
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="localeIsoCode"></param>
        /// <returns></returns>
        private static AboutViewModel CreateAboutFiltersViewModel(string baseUrl, string localeIsoCode)
        {
            AboutViewModel model = new AboutViewModel();
            AboutItem item = null;
            string html = GetPageAsText(baseUrl + Resource.AboutUrlFilter);
            if (html.IsNotEmpty())
            {
                model.TitleLabel = GetTitleLabelFromHtml(html);
                model.Description = GetDescriptionFromHtml(html);
                model.Items = new List<AboutItem>();

                item = CreateAboutItem(baseUrl + Resource.AboutUrlFilterOccurrence, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }

                item = CreateAboutItem(baseUrl + Resource.AboutUrlFilterTaxa, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }

                item = CreateAboutItem(baseUrl + Resource.AboutUrlFilterSpatial, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }

                item = CreateAboutItem(baseUrl + Resource.AboutUrlFilterTemporal, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }

                item = CreateAboutItem(baseUrl + Resource.AboutUrlFilterQuality, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }
            }
            return model;
        }

        /// <summary>
        /// Get a view model with information about filters.
        /// This functions returns cached data. Use the reset cach to get new updates of the about information texts.
        /// </summary>
        /// <param name="localeIsoCode">A string representing the locale ISO Code, e.g. "sv-SE".</param>
        /// <returns>The view model for a specified locale.</returns>
        public static AboutViewModel GetAboutFiltersViewModel(string localeIsoCode)
        {
            //Swedish locale
            if (localeIsoCode == "sv-SE")
            {
                if (_aboutFiltersSv.IsNull())
                {
                    _aboutFiltersSv = CreateAboutFiltersViewModel(SWEDISH_BASE_URL, localeIsoCode);
                }
                if (_aboutFiltersSv.TitleLabel.IsNotEmpty())
                {
                    return _aboutFiltersSv;
                }
            }

            //Default locale
            if (_aboutFilters.IsNull())
            {
                _aboutFilters = CreateAboutFiltersViewModel(BASE_URL, localeIsoCode);
            }
            return _aboutFilters;
        }
        #endregion

        #region Calculations

        /// <summary>
        /// This method creates a new updated version of a locale specific view model for calculation.
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="localeIsoCode"></param>
        /// <returns></returns>
        private static AboutViewModel CreateAboutCalculationsViewModel(string baseUrl, string localeIsoCode)
        {
            AboutViewModel model = new AboutViewModel();
            AboutItem item = null;
            string html = GetPageAsText(baseUrl + Resource.AboutUrlCalculation);
            if (html.IsNotEmpty())
            {
                model.TitleLabel = GetTitleLabelFromHtml(html);
                model.Description = GetDescriptionFromHtml(html);
                model.Items = new List<AboutItem>();

                item = CreateAboutItem(baseUrl + Resource.AboutUrlCalculationSummaryStatistics, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }

                item = CreateAboutItem(baseUrl + Resource.AboutUrlCalculationGridStatistics, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }

                item = CreateAboutItem(baseUrl + Resource.AboutUrlCalculationTimeSeries, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }
                /*
                item = createAboutItem(baseUrl + LOCAL_URL_CALCULATION_MODEL, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }
                */
                //item = createAboutItem(baseUrl + LOCAL_URL_CALCULATION_REPEAT, localeIsoCode);
                //if (item.Header.IsNotEmpty())
                //{
                //    model.Items.Add(item);
                //}
            }
            return model;
        }

        /// <summary>
        /// Get a view model with information about calculations.
        /// This functions returns cached data. Use the reset cach to get new updates of the about information texts.
        /// </summary>
        /// <param name="localeIsoCode">A string representing the locale ISO Code, e.g. "sv-SE".</param>
        /// <returns>The view model for a specified locale.</returns>
        public static AboutViewModel GetAboutCalculationsViewModel(string localeIsoCode)
        {
            //Swedish locale
            if (localeIsoCode == "sv-SE")
            {
                if (_aboutCalculationsSv.IsNull())
                {
                    _aboutCalculationsSv = CreateAboutCalculationsViewModel(SWEDISH_BASE_URL, localeIsoCode);
                }
                if (_aboutCalculationsSv.TitleLabel.IsNotEmpty())
                {
                    return _aboutCalculationsSv;
                }
            }

            //Default locale
            if (_aboutCalculations.IsNull())
            {
                _aboutCalculations = CreateAboutCalculationsViewModel(BASE_URL, localeIsoCode);
            }
            return _aboutCalculations;
        }
        #endregion

        #region Format

        /// <summary>
        /// This method creates a new updated version of a locale specific view model for presentations.
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="localeIsoCode"></param>
        /// <returns></returns>
        private static AboutViewModel CreateAboutPresentationFormatViewModel(string baseUrl, string localeIsoCode)
        {
            AboutViewModel model = new AboutViewModel();
            AboutItem item = null;
            string html = GetPageAsText(baseUrl + Resource.AboutUrlFormat);
            if (html.IsNotEmpty())
            {
                model.TitleLabel = GetTitleLabelFromHtml(html);
                model.Description = GetDescriptionFromHtml(html);
                model.Items = new List<AboutItem>();

                //item = createAboutItem(baseUrl + LOCAL_URL_PRESENTATION_MAP, localeIsoCode);
                //if (item.Header.IsNotEmpty())
                //{
                //    model.Items.Add(item);
                //}

                //item = createAboutItem(baseUrl + LOCAL_URL_PRESENTATION_DIAGRAM, localeIsoCode);
                //if (item.Header.IsNotEmpty())
                //{
                //    model.Items.Add(item);
                //}

                item = CreateAboutItem(baseUrl + Resource.AboutUrlFormatTableColumns, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }

                //item = createAboutItem(baseUrl + LOCAL_URL_PRESENTATION_REPORT, localeIsoCode);
                //if (item.Header.IsNotEmpty())
                //{
                //    model.Items.Add(item);
                //}
            }
            return model;
        }

        /// <summary>
        /// Get a view model with information about presentation.
        /// This functions returns cached data. Use the reset cach to get new updates of the about information texts.
        /// </summary>
        /// <param name="localeIsoCode">A string representing the locale ISO Code, e.g. "sv-SE".</param>
        /// <returns>The view model for a specified locale.</returns>
        public static AboutViewModel GetAboutPresentationFormatViewModel(string localeIsoCode)
        {
            //Swedish locale
            if (localeIsoCode == "sv-SE")
            {
                if (_aboutPresentationsSv.IsNull())
                {
                    _aboutPresentationsSv = CreateAboutPresentationFormatViewModel(SWEDISH_BASE_URL, localeIsoCode);
                }
                if (_aboutPresentationsSv.TitleLabel.IsNotEmpty())
                {
                    return _aboutPresentationsSv;
                }
            }

            //Default locale
            if (_aboutPresentations.IsNull())
            {
                _aboutPresentations = CreateAboutPresentationFormatViewModel(BASE_URL, localeIsoCode);
            }
            return _aboutPresentations;
        }
        #endregion

        #region Result

        /// <summary>
        /// This method creates a new updated version of a locale specific view model for results.
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="localeIsoCode"></param>
        /// <returns></returns>
        private static AboutViewModel CreateAboutResultFormatViewModel(string baseUrl, string localeIsoCode)
        {
            AboutViewModel model = new AboutViewModel();
            AboutItem item = null;
            string html = GetPageAsText(baseUrl + Resource.AboutUrlResult);
            if (html.IsNotEmpty())
            {
                model.TitleLabel = GetTitleLabelFromHtml(html);
                model.Description = GetDescriptionFromHtml(html);
                model.Items = new List<AboutItem>();

                item = CreateAboutItem(baseUrl + Resource.AboutUrlResultMaps, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }

                item = CreateAboutItem(baseUrl + Resource.AboutUrlResultTables, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }

                item = CreateAboutItem(baseUrl + Resource.AboutUrlResultDiagrams, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }

                item = CreateAboutItem(baseUrl + Resource.AboutUrlResultReports, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }

                item = CreateAboutItem(baseUrl + Resource.AboutUrlResultDownload, localeIsoCode);
                if (item.Header.IsNotEmpty())
                {
                    model.Items.Add(item);
                }
            }
            return model;
        }

        /// <summary>
        /// Get a view model with information about presentation.
        /// This functions returns cached data. Use the reset cach to get new updates of the about information texts.
        /// </summary>
        /// <param name="localeIsoCode">A string representing the locale ISO Code, e.g. "sv-SE".</param>
        /// <returns>The view model for a specified locale.</returns>
        public static AboutViewModel GetAboutResultFormatViewModel(string localeIsoCode)
        {
            //Swedish locale
            if (localeIsoCode == "sv-SE")
            {
                if (_aboutResultsSv.IsNull())
                {
                    _aboutResultsSv = CreateAboutResultFormatViewModel(SWEDISH_BASE_URL, localeIsoCode);
                }
                if (_aboutResultsSv.TitleLabel.IsNotEmpty())
                {
                    return _aboutResultsSv;
                }
            }

            //Default locale
            if (_aboutResults.IsNull())
            {
                _aboutResults = CreateAboutResultFormatViewModel(BASE_URL, localeIsoCode);
            }
            return _aboutResults;
        }
        #endregion

        /// <summary>
        /// Clear all cached data enabling updates of information from the CMS.
        /// </summary>
        public static void ClearCache()
        {
            _aboutDataProviders = null;
            _aboutDataProvidersSv = null;
            _aboutFilters = null;
            _aboutFiltersSv = null;
            _aboutCalculations = null;
            _aboutCalculationsSv = null;
            _aboutPresentations = null;
            _aboutPresentationsSv = null;
            _aboutResults = null;
            _aboutResultsSv = null;
        }
    }
}
