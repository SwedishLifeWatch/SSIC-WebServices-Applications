using System;
using System.Net;
using System.Text;
using System.Xml.Linq;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Logging;
using HtmlAgilityPack;

namespace AnalysisPortal.Helpers
{
    public class NewsScrapeHelper
    {
        /// <summary>
        /// Get html from remote page
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static HtmlDocument GetPage(string url)
        {
            using (var webClient = new WebClient() { Encoding = Encoding.UTF8 })
            {
                var html = webClient.DownloadString(url);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                return htmlDocument;
            }
        }

        /// <summary>
        /// Get news and update local xml file
        /// </summary>
        /// <param name="rootUrl"></param>
        /// <param name="subDirectory"></param>
        /// <param name="filePath"></param>
        private static void UpdateNewsXmlFile(string rootUrl, string subDirectory, string filePath)
        {
            var homePage = GetPage(string.Format("{0}{1}", rootUrl, subDirectory));

            //Get all news item from news list
            var newsItems = homePage.DocumentNode.SelectNodes("//ul/li[@class=\"pagelist-item \"]");
            
            if (newsItems != null)
            {
                var news = new XElement("news");
                //Loop throw all news items
                foreach (var newsItem in newsItems)
                {
                    //Get address to news page
                    var anchor = newsItem.SelectSingleNode("a").Attributes["href"].Value;
                    
                    //Get news
                    var newsPage = GetPage(string.Format("{0}{1}", rootUrl, anchor));

                    //Get all images 
                    var images = newsPage.DocumentNode.SelectNodes("//img");
                    if (images != null)
                    {   
                        //Loop throw images
                        foreach (var image in images)
                        {
                            //Update image source 
                            var src = image.Attributes["src"].Value;
                            image.Attributes["src"].Value = string.Format("{0}{1}", rootUrl, src);
                        }
                    }

                    //Get news properties
                    var title = newsPage.DocumentNode.SelectSingleNode("//meta[@name=\"title\"]").Attributes["content"].Value;
                    var published = newsPage.DocumentNode.SelectSingleNode("//div[@class=\"publish-info\"]").InnerText;
                    var introduction = newsPage.DocumentNode.SelectSingleNode("//p[@class=\"intro \"]").InnerHtml;
                    var text = newsPage.DocumentNode.SelectSingleNode("//div[@class=\"main-body \"]").InnerHtml;
                    var introImageContainer = newsPage.DocumentNode.SelectSingleNode("//div[@class=\"intro-image\"]");

                    //Add image div to body text if it exists
                    if (introImageContainer != null)
                    {
                        text = introImageContainer.OuterHtml + text;
                    }
                    
                    //Add news to xml doc
                    var article = new XElement(
                        "Article",
                        new XElement("Published", new XCData(published)),
                        new XElement("Title", new XCData(title)),
                        new XElement("Introduction", new XCData(introduction)),
                        new XElement("Text", new XCData(text)));

                    news.Add(article);
                }

                var doc = new XDocument(news);
                doc.Save(filePath);
            }
        }

        /// <summary>
        /// Get news from SLU site
        /// </summary>
        /// <param name="path"></param>
        public static void UpdateNews(string path)
        {
            var rootUrl = ConfigurationHelper.GetValue("NewsSiteUrl", "http://www.slu.se");

            try
            {
                UpdateNewsXmlFile(rootUrl, "/site/svenska-lifewatch", string.Format(@"{0}\Content\News\News.sv.xml", path));
                UpdateNewsXmlFile(rootUrl, "/en/site/swedish-lifewatch/", string.Format(@"{0}\Content\News\News.xml", path));
            }
            catch(Exception ex)
            {
                Logger.WriteMessage("Failed to update news");
                Logger.WriteException(ex);
            }
        }
    }
}