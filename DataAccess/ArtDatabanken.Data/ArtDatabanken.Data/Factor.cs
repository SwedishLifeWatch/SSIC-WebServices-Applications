using System;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a factor.
    /// </summary>
    [Serializable]
    public class Factor : IFactor
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Factor data type for this factor.
        /// </summary>
        public IFactorDataType DataType { get; set; }

        /// <summary>
        /// Taxon id for parent taxon for all potential hosts associated with this factor.
        /// </summary>
        public Int32 DefaultHostParentTaxonId { get; set; }

        /// <summary>
        /// Host label for this factor.
        /// </summary>
        public String HostLabel { get; set; }

        /// <summary>
        /// Id for this factor.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Information for this factor.
        /// </summary>
        public String Information { get; set; }

        /// <summary>
        /// Indicates if this factor is a leaf in the factor tree.
        /// </summary>
        public Boolean IsLeaf { get; set; }

        /// <summary>
        /// Indication about whether or not this factor is periodic.
        /// </summary>
        public Boolean IsPeriodic { get; set; }

        /// <summary>
        /// Indication about whether or not this factor should be available for public use.
        /// </summary>
        public Boolean IsPublic { get; set; }

        /// <summary>
        /// Indication about whether or not this factor can be associated with a host taxon.
        /// </summary>
        public Boolean IsTaxonomic { get; set; }

        /// <summary>
        /// Label for this factor.
        /// </summary>
        public String Label { get; set; }

        /// <summary>
        /// Name for this factor.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Factor origin for this factor.
        /// </summary>
        public IFactorOrigin Origin { get; set; }

        /// <summary>
        /// Sort order for this factor.
        /// </summary>
        public Int32 SortOrder { get; set; }

        /// <summary>
        /// Factor update mode for this factor.
        /// </summary>
        public IFactorUpdateMode UpdateMode { get; set; }

        /// <summary>
        /// Get all factors that have an impact on this factors value.
        /// Only factors that are automatically calculated
        /// has dependent factors.
        /// </summary>
        /// <param name="userContext">Information about the user that makes this method call.</param>
        /// <returns>Dependent factors.</returns>
        public FactorList GetDependentFactors(IUserContext userContext)
        {
            FactorList factors;

            factors = new FactorList();
            switch (Id)
            {
                case ((Int32)FactorId.RedListCategoryAutomatic):
                case ((Int32)FactorId.RedListCriteriaDocumentationAutomatic):
                case ((Int32)FactorId.RedListCriteriaAutomatic):
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.AreaOfOccupancy_B2Estimated));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ConservationDependent));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ContinuingDecline));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ContinuingDeclineBasedOn_Bbi));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ContinuingDeclineBasedOn_Bbii));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ContinuingDeclineBasedOn_Bbiii));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ContinuingDeclineBasedOn_Bbiv));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ContinuingDeclineBasedOn_Bbv));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ExtentOfOccurrence_B1Estimated));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ExtremeFluctuations));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ExtremeFluctuationsIn_Bci));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ExtremeFluctuationsIn_Bcii));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ExtremeFluctuationsIn_Bciii));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ExtremeFluctuationsIn_Bciv));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.Grading));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.MaxProportionLocalPopulation));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.MaxSizeLocalPopulation));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.NumberOfLocations));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.PopulationSize_Total));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ProbabilityOfExtinction));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.RedlistEvaluationProgressionStatus));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.Reduction_A1));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.Reduction_A2));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.Reduction_A3));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.Reduction_A4));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A1a));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A1b));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A1c));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A1d));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A1e));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A2a));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A2b));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A2c));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A2d));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A2e));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A3b));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A3c));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A3d));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A3e));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A4a));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A4b));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A4c));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A4d));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ReductionBasedOn_A4e));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.SeverelyFragmented));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.SwedishOccurrence));
                    factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.VeryRestrictedArea_D2VU));
                    if (Id == ((Int32)FactorId.RedListCriteriaDocumentationAutomatic))
                    {
                        factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.RedListCriteriaDocumentationIntroduction));
                        factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.GlobalRedlistCategory));
                        factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.GenerationTime));
                        factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.LastEncounter));
                        factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ImmigrationOccurs));
                    }

                    break;
            }

            return factors;
        }

        /// <summary>
        /// Get the factor tree node for this factor.
        /// </summary>
        /// <param name="userContext">Information about the user that makes this method call.</param>
        /// <returns>Tree where this factor is top node.</returns>
        public IFactorTreeNode GetFactorTree(IUserContext userContext)
        {
            return CoreData.FactorManager.GetFactorTree(userContext, Id);
        }

        /// <summary>
        /// Get a text string with the basic information about the 
        /// factor that can be presented in applications as a hint text.
        /// </summary>
        /// <returns>The hint text.</returns>
        public String GetHint()
        {
            StringBuilder hintText = new StringBuilder();

            hintText.AppendLine("Namn: " + Label + ", FaktorId: " + Id);

            if (Origin.IsNotNull())
            {
                hintText.AppendLine("Ursprung: " + Origin.Name);
            }

            if (DataType.IsNotNull())
            {
                hintText.AppendLine(
                    "Datatyp: " + DataType.Name + ", DatatypId: " + DataType.Id);
            }

            if (UpdateMode.IsHeader)
            {
                hintText.AppendLine("Denna faktor fungerar enbart som rubrik för en grupp underliggande faktorer");
            }
            else
            {
                if (IsLeaf)
                {
                    hintText.AppendLine("Det finns inga underordnade faktorer under denna faktor (faktorn är ett löv)");
                }
                else
                {
                    hintText.AppendLine("Denna faktor har underordnade faktorer");
                }

                if (!UpdateMode.AllowManualUpdate)
                {
                    if (UpdateMode.AllowAutomaticUpdate)
                    {
                        hintText.AppendLine("Manuell uppdatering tillåts inte, värdet beräknas automatiskt baserat på andra faktorers värden");
                    }
                    else
                    {
                        hintText.AppendLine("Uppdatering av information knuten till denna faktor sker inte längre (arkiv)");
                    }
                }

                if (IsPeriodic)
                {
                    hintText.AppendLine("Denna faktor uppdateras periodiskt");
                }
                else
                {
                    hintText.AppendLine("Denna faktor kan uppdateras kontinuerligt vid behov");
                }

                if (IsTaxonomic)
                {
                    hintText.AppendLine("Bedömningar av ett taxons relation till denna faktor kan specificeras olika för valfritt många värdtaxa");
                }

                if (IsPublic)
                {
                    hintText.AppendLine("Publik: Ja");
                }
                else
                {
                    hintText.AppendLine("Publik: Nej");
                }

                if (DataType.Fields.Count == 1)
                {
                    hintText.AppendLine("Faktorn har endast ett fält: " + DataType.Fields[0].Label);
                }
                else
                {
                    hintText.Append("Förutom huvudfältet finns ");
                    hintText.Append((DataType.SubstantialFields.Count - 1) + " substantiella värdefält");
                    hintText.AppendLine(" och " + (DataType.Fields.Count - DataType.SubstantialFields.Count)
                                        + " övriga värdefält");
                }
            }

            return hintText.ToString();
        }
    }
}