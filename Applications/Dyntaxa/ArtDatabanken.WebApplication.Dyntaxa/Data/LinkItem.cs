using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// Enum that represents different types of links.
    /// </summary>
    public enum LinkType
    {
        Action,
        Url
    }

    /// <summary>
    /// Enum that represents different types of parameters.
    /// </summary>
    public enum LinkParameterType
    {
        NoParameter,
        Id
    }

    /// <summary>
    /// Enum that represents link quality alternatives.
    /// </summary>
    public enum LinkQuality
    {
        ApprovedByExpert,
        Automatic
    }

    /// <summary>
    /// A class that holds a set of link items.
    /// </summary>
    public class LinkListModel
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Instruction { get; set; }

        public List<LinkItem> Links { get; set; }

        /// <summary>
        /// Get the subset of available links that has a sertain quality value.
        /// </summary>
        /// <param name="quality">Enum representing quality alternatives.</param>
        /// <returns>.A list of link items</returns>
        public List<LinkItem> GetLinksByQuality(LinkQuality quality)
        {
            List<LinkItem> items = new List<LinkItem>();
            foreach (LinkItem item in this.Links)
            {
                if (item.Quality.Equals(quality))
                {
                    items.Add(item);
                }
            }
            return items;
        }

        /// <summary>
        /// Get the subset of available links of a sertain type.
        /// </summary>
        /// <param name="quality">Enum representing link type.</param>
        /// <returns>.A list of link items</returns>
        public List<LinkItem> GetLinksByType(LinkType type)
        {
            List<LinkItem> items = new List<LinkItem>();
            foreach (LinkItem item in this.Links)
            {
                if (item.Type.Equals(type))
                {
                    items.Add(item);
                }
            }
            return items;
        }
    }

    /// <summary>
    /// This class holds all information needed to handle a link to external och internal sources.
    /// </summary>
    public class LinkItem
    {
        public LinkItem()
        {
        }

        public LinkItem(LinkType type, LinkQuality quality)
        {
            this.Type = type;
            this.Quality = quality;
        }

        public LinkItem(LinkType type, LinkQuality quality, string linkText, string url)
        {
            this.Type = type;
            this.Quality = quality;
            this.LinkText = linkText;
            this.Url = url;
        }

        public static LinkItem CreateActionLink(string action, string controller, LinkQuality quality, string linkText, string parameterValue)
        {
            var link = new LinkItem();
            link.Action = action;
            link.Controller = controller;
            link.Quality = quality;
            link.LinkText = linkText;
            link.ParameterValue = parameterValue;
            return link;
        }

        public string LinkText { get; set; }

        public string LinkSubText { get; set; }

        public string Action { get; set; }

        public string Controller { get; set; }

        public LinkParameterType ParameterType { get; set; }

        public string ParameterValue { get; set; }

        public string SecondId { get; set; }

        public string Url { get; set; }

        public LinkQuality Quality { get; set; }

        public LinkType Type { get; set; }
    }
}
