using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ButtonGroups;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo
{
    /// <summary>
    /// This class holds information about a Page.
    /// </summary>
    public class PageInfo
    {      
        private Expression<Func<string>> _titleExpression;

        public string Title
        {
            get
            {
                string value = _titleExpression.Compile()();
                return value;
            }
        }
        public string Controller { get; set; }
        public string Action { get; set; }        
        public StateButtonIdentifier StateButton { get; set; }
        public ButtonGroupIdentifier ButtonGroup { get; set; }
        public PageInfo ParentPage { get; set; }
        public bool BreadcrumbNavigation { get; set; }
        //public PageInfo NextPage { get; set; }
        //public PageInfo PreviousPage { get; set; }

        public PageInfo()
        {
        }

        public List<PageInfo> GetParentPages()
        {            
            PageInfo currentPage = this;
            List<PageInfo> parentPages = new List<PageInfo>();
            while (currentPage.ParentPage != null)
            {
                currentPage = currentPage.ParentPage;
                parentPages.Add(currentPage);
            }
            parentPages.Reverse();
            return parentPages;            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageInfo"/> class.
        /// </summary>
        /// <param name="controller">The controller name.</param>
        /// <param name="action">The action name.</param>
        /// <param name="stateButton">The state button.</param>
        /// <param name="buttonGroup">The button group.</param>
        public PageInfo(string controller, string action, StateButtonIdentifier stateButton, ButtonGroupIdentifier buttonGroup)
        {
            Controller = controller;
            Action = action;
            StateButton = stateButton;
            ButtonGroup = buttonGroup;            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageInfo" /> class.
        /// </summary>
        /// <param name="controller">The controller name.</param>
        /// <param name="action">The action name.</param>
        /// <param name="stateButton">The state button.</param>
        /// <param name="buttonGroup">The button group.</param>
        /// <param name="parentPage">The parent page.</param>
        /// <param name="titleExpression">The title expression.</param>
        /// <param name="breadcrumbNavigation"></param>
        public PageInfo(string controller, string action, StateButtonIdentifier stateButton, ButtonGroupIdentifier buttonGroup, PageInfo parentPage, Expression<Func<string>> titleExpression, bool breadcrumbNavigation)
        {
            Controller = controller;
            Action = action;
            StateButton = stateButton;
            ButtonGroup = buttonGroup;
            ParentPage = parentPage;
            _titleExpression = titleExpression;
            BreadcrumbNavigation = breadcrumbNavigation;
        }

        public bool IsCurrent
        {
            get
            {
                PageInfo pageInfo = SessionHandler.CurrentPage;
                if (pageInfo.IsNotNull())
                {
                    return pageInfo.StateButton == StateButton;
                }
                return false;
            }
        }

        /// <summary>
        /// Determines if this is the Analysis portal start page. I.e. Home/Index
        /// </summary>        
        public bool IsAnalysisPortalStartPage
        {
            get
            {
                if (Controller == null || Action == null)
                {
                    return false;
                }

                return Controller == "Home" && Action == "Index";
            }
        }

        protected bool Equals(PageInfo other)
        {
            return string.Equals(Action, other.Action) && string.Equals(Controller, other.Controller);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((PageInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Action != null ? Action.GetHashCode() : 0) * 397) ^ (Controller != null ? Controller.GetHashCode() : 0);
            }
        }

        public static bool operator ==(PageInfo left, PageInfo right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PageInfo left, PageInfo right)
        {
            return !Equals(left, right);
        }
    }
}
