﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class TaxonResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal TaxonResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ArtDatabanken.WebService.TaxonService.TaxonResource", typeof(TaxonResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [CurrentTaxonCategory].
        /// </summary>
        public static string ConceptDefinitionTagForCurrentCategory {
            get {
                return ResourceManager.GetString("ConceptDefinitionTagForCurrentCategory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [FormerTaxonCategory].
        /// </summary>
        public static string ConceptDefinitionTagForFormerCategory {
            get {
                return ResourceManager.GetString("ConceptDefinitionTagForFormerCategory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [TaxonCategoryOfReplacingTaxon].
        /// </summary>
        public static string ConceptDefinitionTagLumpedForCategoryOfReplacingTaxon {
            get {
                return ResourceManager.GetString("ConceptDefinitionTagLumpedForCategoryOfReplacingTaxon", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [TaxonNamesAndIdsOfOtherLumpedTaxa].
        /// </summary>
        public static string ConceptDefinitionTagLumpedForOtherLumpedTaxa {
            get {
                return ResourceManager.GetString("ConceptDefinitionTagLumpedForOtherLumpedTaxa", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [TaxonNameAndIdOfReplacingTaxon].
        /// </summary>
        public static string ConceptDefinitionTagLumpedForReplacingTaxon {
            get {
                return ResourceManager.GetString("ConceptDefinitionTagLumpedForReplacingTaxon", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [TaxonNamesAndIdsOfLumpedTaxa].
        /// </summary>
        public static string ConceptDefinitionTagLumpingForLumpedTaxa {
            get {
                return ResourceManager.GetString("ConceptDefinitionTagLumpingForLumpedTaxa", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [TaxonNamesOfReplacingTaxa].
        /// </summary>
        public static string ConceptDefinitionTagSplittedForReplacingTaxa {
            get {
                return ResourceManager.GetString("ConceptDefinitionTagSplittedForReplacingTaxa", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [TaxonCategoryOfReplacedTaxon] .
        /// </summary>
        public static string ConceptDefinitionTagSplittingForCategoryOfReplacedTaxon {
            get {
                return ResourceManager.GetString("ConceptDefinitionTagSplittingForCategoryOfReplacedTaxon", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [ReplacedTaxonNameAndId].
        /// </summary>
        public static string ConceptDefinitionTagSplittingForReplacedTaxon {
            get {
                return ResourceManager.GetString("ConceptDefinitionTagSplittingForReplacedTaxon", resourceCulture);
            }
        }
    }
}