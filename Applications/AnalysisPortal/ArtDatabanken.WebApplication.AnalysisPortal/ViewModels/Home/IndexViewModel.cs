using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ArtDatabanken.WebApplication.AnalysisPortal.Extensions;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Home
{
    /// <summary>
    /// View model for start page
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="culture">current culture</param>
        public IndexViewModel(string culture)
        {
            var directory = string.Format(@"{0}content\News\", AppDomain.CurrentDomain.BaseDirectory);

            var xmlFile = string.Format("News{0}.xml", culture == "en" ? "" : string.Format(".{0}", culture));
            var path = string.Format("{0}{1}", directory, xmlFile);

            if (!File.Exists(path))
            {
                path = string.Format("{0}News.xml", directory);
            }
            
            //Get news from xml file
            var newsList = XElement.Load(path);

            //Populate news from file
            News = (
                from a in newsList.Elements("Article")
                select a.ToObject<Article>()).ToArray();

            xmlFile = string.Format("Puffs{0}.xml", culture == "en" ? "" : string.Format(".{0}", culture));
            path = string.Format("{0}{1}", directory, xmlFile);

            if (!File.Exists(path))
            {
                path = string.Format("{0}Puffs.xml", directory);
            }

            //Get news from xml file
            var puffList = XElement.Load(path);

            //Populate news from file
            Puffs = (
                from p in puffList.Elements("Puff")
                select p.ToObject<Puff>()).ToArray();
        }

        public Article[] News { get; set; }
        public Puff[] Puffs { get; set; }
    }

    /// <summary>
    /// Puff class
    /// </summary>
    public class Puff
    {
        public string Published { get; set; }
        public string Text { get; set; }
    }

    /// <summary>
    /// News article class
    /// </summary>
    public class Article : Puff
    {
        public string Title { get; set; }
        public string Introduction { get; set; }
    }
}
