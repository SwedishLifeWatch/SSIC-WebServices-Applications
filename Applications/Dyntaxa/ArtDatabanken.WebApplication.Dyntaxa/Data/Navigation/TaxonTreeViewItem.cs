using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using ArtDatabanken.Data;
using System;
using System.Diagnostics;
using System.Security.Policy;
using Newtonsoft.Json;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Navigation
{
    /// <summary>
    /// An tree item containing the item, its parent and its children
    /// The class is decorated with [DataMember] attributes on the properties that will be
    /// serialized as Json
    /// </summary>
    [DataContract]
    public class TaxonTreeViewItem
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "isLazy")]
        public bool IsLazy
        {
            get { return HasAjaxChildren; }
        }       

        [DataMember(Name = "isFolder")]
        public bool IsFolder 
        {
            get { return HasChildren || HasAjaxChildren; }            
        }

        [DataMember(Name = "expand")]
        public bool IsExpanded { get; set; }

        [DataMember(Name = "activate")]
        public bool IsActive { get; set; }

        [DataMember(Name = "key")]
        public string Key
        {
            get { return Id.ToString(); }
            //get { return TaxonId.ToString(); }
        }

        [DataMember(Name = "icon")]
        public string Icon
        {
            get
            {
                if (IsMainRelation)
                {
                    return null;
                }
                else
                {
                    return "../../../../Images/Icons/secondary_relation.png";
                }
            }
        } 

        [DataMember(Name = "children")]
        public IList<TaxonTreeViewItem> Children { get; set; }

        [DataMember(Name = "addClass")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CssClass { get; set; }

        public int Id { get; set; }

        public bool IsMainRelation { get; set; }

        public int TaxonId
        {
            get { return Taxon.Id; }
        }

        public TaxonTreeViewItem Parent;
        public int Level { get; set; }

        public ITaxon Taxon { get; set; }

        public bool IsLastElement
        {
            get
            {
                if (Parent != null && Parent.Children != null)
                {                    
                    return Parent.Children.Last() == this;
                }
                return true;
            }
        }

        public string Name
        {
            get
            {
                return Taxon.ScientificName.IsNotEmpty() ? Taxon.ScientificName : string.Format("Taxon Id: {0}", TaxonId);
            }            
        }

        public string CommonName
        {
            get
            {
                return Taxon.CommonName.IsNotEmpty() ? Taxon.CommonName : "-";
            }
        }

        public TaxonTreeViewItem(ITaxon taxon, TaxonTreeViewItem parent, int level, int id, bool isMainRelation)
        {            
            Taxon = taxon;
            Parent = parent;
            Level = level;
            Id = id;
            IsMainRelation = isMainRelation;
            if (!taxon.Category.IsTaxonomic)
            {
                CssClass = "non-taxonomic-label";
            }

            //Title = string.Format("{0} ({1})", HttpUtility.HtmlEncode(Name), HttpUtility.HtmlEncode(CommonName));
            if (taxon.CommonName.IsNotEmpty())
            {
                Title = string.Format("{0} ({1})", HttpUtility.HtmlEncode(Name), HttpUtility.HtmlEncode(CommonName));
            }
            else
            {
                Title = string.Format("{0}", HttpUtility.HtmlEncode(Name));
            }

            //string pathName = VirtualPathUtility.ToAbsolute(@"~/Images/Icons/minus-small.png", "Scripts/Dynatree/src/skin");
            //pathName = VirtualPathUtility.ToAppRelative(@"~/Images/Icons/minus-small.png", "Scripts/Dynatree/src/skin");
            //pathName = "/Images/Icons/minus-small.png";
            //if (parent.Taxon.rel)
            //string pathName = "../../../../Images/Icons/minus-small.png";
            //Icon = pathName;
        }
        
        /// <summary>
        /// Decides if the item has children in the database that is currently not yet loaded
        /// </summary>
        public bool HasAjaxChildren
        {
            get
            {                
                if (HasChildren)
                {
                    return false;
                }

                try
                {
                    IUserContext userContext = CoreData.UserManager.GetCurrentUser();
                    var children = Taxon.GetChildTaxonRelations(userContext, DyntaxaHelper.IsInRevision(), false);
                    return children.Count > 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("TaxonTreeViewItem->HasAjaxChildren. TaxonId: " + this.TaxonId);
                    var ex2 = new Exception("HasAjaxChildren Exception (IsLazy). TaxonId: " + this.TaxonId, ex);
                    DyntaxaLogger.WriteException(ex2);
                    //1007844
                    throw ex2;
                }
            }
        }

        public bool HasChildren
        {
            get { return Children != null && Children.Count > 0; }
        }
    }
}