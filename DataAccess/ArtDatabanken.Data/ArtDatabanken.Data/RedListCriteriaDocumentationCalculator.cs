using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class calculates red list criteria documentation
    /// based on values for red list data.
    /// </summary>
    [Serializable]
    public class RedListCriteriaDocumentationCalculator : RedListCalculator
    {
        private Boolean _immigrationOccurs;
        private Boolean _isPopulationEstimateAdded;
        private Int32 _lastEncounter;
        private String _criteriaDocumentation;
        private String _generationLengthClarification;
        private String _generationLengthUnitLabel;
        private String _globalRedlistCategory;
        private String _introduction;

        /// <summary>
        /// Creates a RedListCriteriaDocumentationCalculator instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="taxonTypeNameIndefinite">Taxon type name on indefinite form.</param>
        /// <param name="taxonTypeNameDefinite">Taxon type name on definite form.</param>
        public RedListCriteriaDocumentationCalculator(IUserContext userContext,
                                                      String taxonTypeNameIndefinite,
                                                      String taxonTypeNameDefinite)
            : base(userContext, taxonTypeNameIndefinite, taxonTypeNameDefinite)
        {
            _criteriaDocumentation = null;
            _generationLengthClarification = null;
            _generationLengthUnitLabel = null;
            _globalRedlistCategory = null;
            _immigrationOccurs = false;
            _introduction = String.Empty;
            _isPopulationEstimateAdded = false;
            _lastEncounter = Int32.MinValue;
        }

        /// <summary>
        /// Test if last encounter has been set.
        /// </summary>
        public Boolean HasLastEncounter
        {
            get { return _lastEncounter != Int32.MinValue; }
            set
            {
                if (!value)
                {
                    _lastEncounter = Int32.MinValue;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use method SetLastEncounter if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Get last encounter.
        /// Unit is year.
        /// Factor: LastEncounter, Id = 1948,
        /// </summary>
        public Int32 LastEncounter
        {
            get
            {
                return _lastEncounter;
            }
        }

        /// <summary>
        /// Add area of occupancy and extent of occurrence
        /// values to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        private void AddAOOAndEOO(TextBuilder textBuilder)
        {
            if (Category != RedListCategory.RE)
            {
                if (CalculationProbable.HasAreaOfOccupancy &&
                    CalculationProbable.HasExtentOfOccurrence &&
                    (CalculationProbable.AreaOfOccupancy >= RedListCalculation.CRITERIA_B2_NT_LIMIT) &&
                    (CalculationProbable.ExtentOfOccurrence >= RedListCalculation.CRITERIA_B1_NT_LIMIT))
                {
                    // Both AOO and EOO exceeds red list limits.
                    textBuilder.AddSentence("Utbredningsområdets storlek (EOO) och förekomstarean (AOO) överskrider gränsvärdena för rödlistning.");
                    textBuilder.AddSentence(ExtentOfOccurrenceClarification);
                    textBuilder.AddSentence(AreaOfOccupancyClarification);
                }
                else if (CalculationProbable.HasAreaOfOccupancy &&
                         CalculationProbable.HasExtentOfOccurrence &&
                         (CalculationProbable.AreaOfOccupancy < RedListCalculation.CRITERIA_B2_NT_LIMIT) &&
                         (CalculationProbable.ExtentOfOccurrence < RedListCalculation.CRITERIA_B1_NT_LIMIT))
                {
                    // Show information about both AOO and EOO.
                    textBuilder.BeginSentence("Utbredningsområdets storlek (EOO) skattas till");
                    AddValues(textBuilder,
                              CalculationWorstCase.ExtentOfOccurrence,
                              CalculationProbable.ExtentOfOccurrence,
                              CalculationBestCase.ExtentOfOccurrence,
                              ExtentOfOccurrenceUnitLabel);
                    textBuilder.AddText(" och förekomstarean (AOO) till");
                    AddValues(textBuilder,
                              CalculationWorstCase.AreaOfOccupancy,
                              CalculationProbable.AreaOfOccupancy,
                              CalculationBestCase.AreaOfOccupancy,
                              AreaOfOccupancyUnitLabel);
                    textBuilder.EndSentence();
                    textBuilder.AddSentence(ExtentOfOccurrenceClarification);
                    textBuilder.AddSentence(AreaOfOccupancyClarification);
                }
                else
                {
                    if (CalculationProbable.HasExtentOfOccurrence)
                    {
                        // Add information about extent of occurrence.
                        if (CalculationProbable.ExtentOfOccurrence >= RedListCalculation.CRITERIA_B1_NT_LIMIT)
                        {
                            textBuilder.AddSentence("Utbredningsområdets storlek (EOO) överskrider gränsvärdet för rödlistning.");
                        }
                        else
                        {
                            textBuilder.BeginSentence("Utbredningsområdets storlek (EOO) skattas till");
                            AddValues(textBuilder,
                                      CalculationWorstCase.ExtentOfOccurrence,
                                      CalculationProbable.ExtentOfOccurrence,
                                      CalculationBestCase.ExtentOfOccurrence,
                                      ExtentOfOccurrenceUnitLabel);
                            textBuilder.EndSentence();
                        }
                        textBuilder.AddSentence(ExtentOfOccurrenceClarification);
                    }

                    if (CalculationProbable.HasAreaOfOccupancy)
                    {
                        // Add information about area of occupancy.
                        if (CalculationProbable.AreaOfOccupancy >= RedListCalculation.CRITERIA_B2_NT_LIMIT)
                        {
                            textBuilder.AddSentence("Förekomstarean (AOO) överskrider gränsvärdet för rödlistning.");
                        }
                        else
                        {
                            textBuilder.BeginSentence("Förekomstarean (AOO) skattas till");
                            AddValues(textBuilder,
                                      CalculationWorstCase.AreaOfOccupancy,
                                      CalculationProbable.AreaOfOccupancy,
                                      CalculationBestCase.AreaOfOccupancy,
                                      AreaOfOccupancyUnitLabel);
                            textBuilder.EndSentence();
                        }
                        textBuilder.AddSentence(AreaOfOccupancyClarification);
                    }
                }
            }
        }

        /// <summary>
        /// Add category information to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        private void AddCategoryInformation(TextBuilder textBuilder)
        {
            switch (Category)
            {
                case RedListCategory.DD:
                    textBuilder.AddSentence(TaxonTypeNameDefinite +
                                            " rödlistas som " +
                                            GetCategoryInformation(Category) +
                                            " eftersom faktaunderlaget bedöms vara otillräckligt för att avgöra vilken av de olika rödlistningskategorierna som är mest trolig.");
                    break;
                case RedListCategory.CR:
                case RedListCategory.VU:
                case RedListCategory.EN:
                case RedListCategory.NT:
                case RedListCategory.LC:
                    if (CategoryBestCaseNoGrading != CategoryWorstCaseNoGrading)
                    {
                        textBuilder.AddSentence("Beroende på vilka av de skattade värdena som används varierar bedömningen från " +
                                                GetCategoryInformation(CategoryBestCaseNoGrading) + " till " +
                                                GetCategoryInformation(CategoryWorstCaseNoGrading) + ".");

                        textBuilder.AddSentence("Baserat på de troligaste värdena hamnar " +
                                                TaxonTypeNameDefinite + " i kategorin " +
                                                GetCategoryInformation(CategoryProbableNoGrading) + ".");
                    }
                    else
                    {
                        textBuilder.AddSentence("De skattade värdena som bedömningen baserar sig på ligger alla inom intervallet för kategorin " +
                                                  GetCategoryInformation(CategoryProbableNoGrading) + ".");
                    }
                    break;
                case RedListCategory.RE:
                    textBuilder.AddSentence(TaxonTypeNameDefinite +
                                            " rödlistas som " +
                                            GetCategoryInformation(Category) +
                                            " eftersom det bedömts som sannolikt att den upphört att regelbundet reproducera sig inom landet.");


                    if (HasLastEncounter)
                    {
                        textBuilder.AddSentence("Den påträffades senast år " + LastEncounter + ".");
                    }
                    break;
            }
        }

        /// <summary>
        /// Add information about conservation dependency.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        private void AddConservationDependent(TextBuilder textBuilder)
        {
            if ((CategoryProbableGraded == RedListCategory.LC) &&
                IsConservationDependent)
            {
                textBuilder.AddSentence(TaxonTypeNameDefinite + " bedöms inte vara hotad i dagsläget eftersom åtgärder genomförs för att minska utdöenderisken.");
                textBuilder.AddSentence("Skulle dessa åtgärder upphöra kommer " + TaxonTypeNameDefinite + " sannolikt att uppfylla kriterierna för lägst kategorin Sårbar (VU).");
                textBuilder.AddSentence(IsConservationDependentClarification);
            }
        }

        /// <summary>
        /// Add information about continuing decline.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        private void AddContinuingDecline(TextBuilder textBuilder)
        {
            Int32 numberOfFulfilledCriterias;

            if (CalculationProbable.HasContinuingDecline)
            {
                // Add continuing decline value.
                switch (CalculationProbable.ContinuingDecline)
                {
                    case -1:
                        textBuilder.AddSentence("Populationen är ökande.");
                        break;
                    case 0:
                        textBuilder.AddSentence("Det finns inga tecken på betydande populationsförändring.");
                        break;
                    case 1:
                        textBuilder.AddSentence("Det föreligger indikation på eller misstanke om populationsminskning.");
                        break;
                    case 2:
                        textBuilder.AddSentence("En minskning av populationen pågår eller förväntas ske.");
                        break;
                    case 3:
                        textBuilder.BeginSentence("Populationen");
                        if (!_isPopulationEstimateAdded)
                        {
                            textBuilder.AddText(" (<20000 individer)");
                        }
                        textBuilder.EndSentence(" minskar med mer än 5% inom " + GetNumberOfYears(10, 3, true) + " år.");
                        break;
                    case 4:
                        textBuilder.BeginSentence("Populationen");
                        if (!_isPopulationEstimateAdded)
                        {
                            textBuilder.AddText(" (<10000 individer)");
                        }
                        textBuilder.EndSentence(" minskar med mer än 10% inom " + GetNumberOfYears(10, 3, true) + " år.");
                        break;
                    case 5:
                        textBuilder.BeginSentence("Populationen");
                        if (!_isPopulationEstimateAdded)
                        {
                            textBuilder.AddText(" (<2500 individer)");
                        }
                        textBuilder.EndSentence(" minskar med mer än 20% inom " + GetNumberOfYears(5, 2, true) + " år.");
                        break;
                    case 6:
                        textBuilder.BeginSentence("Populationen");
                        if (!_isPopulationEstimateAdded)
                        {
                            textBuilder.AddText(" (<250 individer)");
                        }
                        textBuilder.EndSentence(" minskar med mer än 25% inom " + GetNumberOfYears(3, 1, true) + " år.");
                        break;
                }

                // Add continuing decline reason.
                if (CalculationProbable.ContinuingDecline > 0)
                {
                    // Count number of fulfilled criterias.
                    numberOfFulfilledCriterias = 0;
                    if (CalculationProbable.IsCriteriaBB1Fulfilled)
                    {
                        numberOfFulfilledCriterias++;
                    }
                    if (CalculationProbable.IsCriteriaBB2Fulfilled)
                    {
                        numberOfFulfilledCriterias++;
                    }
                    if (CalculationProbable.IsCriteriaBB3Fulfilled)
                    {
                        numberOfFulfilledCriterias++;
                    }
                    if (CalculationProbable.IsCriteriaBB4Fulfilled)
                    {
                        numberOfFulfilledCriterias++;
                    }
                    if (CalculationProbable.IsCriteriaBB5Fulfilled)
                    {
                        numberOfFulfilledCriterias++;
                    }

                    if (numberOfFulfilledCriterias > 0)
                    {
                        // Add information about fulfilled criterias.
                        textBuilder.BeginAndSentence("Minskningen avser", numberOfFulfilledCriterias);
                        if (CalculationProbable.IsCriteriaBB1Fulfilled)
                        {
                            textBuilder.AddAndText("utbredningsområde");
                            textBuilder.AddBracketedText(IsCriteriaBB1FulfilledClarification);
                        }
                        if (CalculationProbable.IsCriteriaBB2Fulfilled)
                        {
                            textBuilder.AddAndText("förekomstarea");
                            textBuilder.AddBracketedText(IsCriteriaBB2FulfilledClarification);
                        }
                        if (CalculationProbable.IsCriteriaBB3Fulfilled)
                        {
                            textBuilder.AddAndText("kvalitén på " + TaxonTypeNameDefinite + "s habitat");
                            textBuilder.AddBracketedText(IsCriteriaBB3FulfilledClarification);
                        }
                        if (CalculationProbable.IsCriteriaBB4Fulfilled)
                        {
                            textBuilder.AddAndText("antalet lokalområden");
                            textBuilder.AddBracketedText(IsCriteriaBB4FulfilledClarification);
                        }
                        if (CalculationProbable.IsCriteriaBB5Fulfilled)
                        {
                            textBuilder.AddAndText("antalet reproduktiva individer");
                            textBuilder.AddBracketedText(IsCriteriaBB5FulfilledClarification);
                        }
                        textBuilder.EndSentence();
                    }
                }

                // Add continuing decline clarification.
                textBuilder.AddSentence(ContinuingDeclineClarification);
            }
        }

        /// <summary>
        /// Add criteria to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        private void AddCriteria(TextBuilder textBuilder)
        {
            if (Criteria.IsNotEmpty())
            {
                textBuilder.AddSentence(" (" + Criteria + ")");
            }
        }

        /// <summary>
        /// Add fluctuation information to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        private void AddExtremeFluctuations(TextBuilder textBuilder)
        {
            Int32 numberOfFulfilledCriterias;

            if (CalculationProbable.IsCriteriaBCFulfilled)
            {
                // Count number of fulfilled criterias.
                numberOfFulfilledCriterias = 0;
                if (CalculationProbable.IsCriteriaBC1Fulfilled)
                {
                    numberOfFulfilledCriterias++;
                }
                if (CalculationProbable.IsCriteriaBC2Fulfilled)
                {
                    numberOfFulfilledCriterias++;
                }
                if (CalculationProbable.IsCriteriaBC3Fulfilled)
                {
                    numberOfFulfilledCriterias++;
                }
                if (CalculationProbable.IsCriteriaBC4Fulfilled)
                {
                    numberOfFulfilledCriterias++;
                }

                if (numberOfFulfilledCriterias > 0)
                {
                    switch (CalculationProbable.ExtremeFluctuations)
                    {
                        case 1:
                            textBuilder.BeginAndSentence("Extrema fluktuationer förekommer förmodligen i", numberOfFulfilledCriterias);
                            break;
                        case 2:
                            textBuilder.BeginAndSentence("Extrema fluktuationer förekommer i", numberOfFulfilledCriterias);
                            break;
                    }

                    // Add information about fulfilled criterias.
                    if (CalculationProbable.IsCriteriaBC1Fulfilled)
                    {
                        textBuilder.AddAndText("utbredningsområdets storlek");
                        textBuilder.AddBracketedText(IsCriteriaBC1FulfilledClarification);
                    }
                    if (CalculationProbable.IsCriteriaBC2Fulfilled)
                    {
                        textBuilder.AddAndText("förekomstarean");
                        textBuilder.AddBracketedText(IsCriteriaBC2FulfilledClarification);
                    }
                    if (CalculationProbable.IsCriteriaBC3Fulfilled)
                    {
                        textBuilder.AddAndText("antalet lokaler eller subpopulationer");
                        textBuilder.AddBracketedText(IsCriteriaBC3FulfilledClarification);
                    }
                    if (CalculationProbable.IsCriteriaBC4Fulfilled)
                    {
                        textBuilder.AddAndText("antalet fullvuxna individer");
                        textBuilder.AddBracketedText(IsCriteriaBC4FulfilledClarification);
                    }
                    textBuilder.EndSentence();
                }
            }
        }

        /// <summary>
        /// Add generation length information to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        private void AddGenerationLength(TextBuilder textBuilder)
        {
            // Test if generation length should be shown.

            // Test if category is showing that the species is threatened but not extinct.
            if (((Category == RedListCategory.DD) ||
                 (Category == RedListCategory.CR) ||
                 (Category == RedListCategory.EN) ||
                 (Category == RedListCategory.VU) ||
                 (Category == RedListCategory.NT)) &&
                // Test if population is decreasing.
                ((CalculationProbable.HasProbabilityOfExtinction &&
                  (CalculationProbable.ProbabilityOfExtinction <= 3)) ||
                 (CalculationProbable.HasContinuingDecline &&
                  (CalculationProbable.ContinuingDecline >= 1)) ||
                 CalculationProbable.IsCriteriaAFulfilled(RedListCategory.NT)) &&
                // Test if generation length has been set.
                CalculationProbable.HasGenerationLength)
            {
                if (CalculationBestCase.GenerationLength != CalculationWorstCase.GenerationLength)
                {

                    textBuilder.AddSentence("Generationslängden varierar mellan " +
                                            CalculationWorstCase.GenerationLength + " och " +
                                            CalculationBestCase.GenerationLength + " " + _generationLengthUnitLabel + " men är troligen oftast " +
                                            CalculationProbable.GenerationLength + " " + _generationLengthUnitLabel + ".");
                }
                else
                {
                    textBuilder.AddSentence("Generationslängden är " + CalculationProbable.GenerationLength + " " + _generationLengthUnitLabel + ".");
                }
                textBuilder.AddSentence(_generationLengthClarification);
            }
        }

        /// <summary>
        /// Add global redlist category information to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        private void AddGlobalRedlistCategory(TextBuilder textBuilder)
        {
            if (_globalRedlistCategory.IsNotEmpty())
            {
                textBuilder.AddSentence("Global rödlistningskategori: " + _globalRedlistCategory + ".");
            }
        }

        /// <summary>
        /// Add grading information to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        private void AddGradingInformation(TextBuilder textBuilder)
        {
            // String grade = @"\u+00B0"; (unicode for grade symbol hopefully

            if (IsGraded &&
                ((Category == RedListCategory.CR) ||
                 (Category == RedListCategory.EN) ||
                 (Category == RedListCategory.VU) ||
                 (Category == RedListCategory.NT) ||
                 (Category == RedListCategory.LC)))
            {
                if (_immigrationOccurs && (Grading < 0))
                {
                    textBuilder.AddSentence("Eftersom det finns möjlighet att " +
                        //TaxonTypeNameDefinite + " kan immigrera från kringliggande länder bedöms risken för utdöende i landet vara lägre än vad övriga tillgängliga data antyder.");
                                            TaxonTypeNameDefinite + " kan invandra från kringliggande länder bedöms utdöenderisken vara lägre än vad övriga tillgängliga data antyder.");
                    if (CategoryProbableGraded == CategoryProbableNoGrading)
                    {
                        textBuilder.AddSentence("Rödlistningskategorin markeras därför med en gradsymbol som indikerar att justering har skett. Kategorin påverkas inte av detta utan behåller sitt värde " +
                                                Category + ".");
                    }
                    else
                    {
                        textBuilder.AddSentence("Därför har rödlistningskategorin justerats från " +
                                                CalculationProbable.Category + " till " +
                                                CategoryProbableGraded + ".");
                    }
                }
                else
                {
                    if (CategoryProbableGraded == CategoryProbableNoGrading)
                    {
                        textBuilder.AddSentence("Rödlistningskategorin har markerats med en gradsymbol. Kategorin påverkas inte av detta utan behåller sitt värde " +
                                              CategoryProbableGraded + ".");
                    }
                    else
                    {
                        textBuilder.AddSentence("Rödlistningskategorin har justerats från " +
                                                CalculationProbable.Category + " till " +
                                                CategoryProbableGraded + ".");
                    }
                }
                if (IsGraded)
                {
                    textBuilder.AddSentence(GradingClarification);
                }
            }
        }

        /// <summary>
        /// Add introduction to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        private void AddIntroduction(TextBuilder textBuilder)
        {
            textBuilder.AddSentence(_introduction);
        }

        /// <summary>
        /// Add information, about which criterias that triggered
        /// the red list category, to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        private void AddMotive(TextBuilder textBuilder)
        {
            switch (Category)
            {
                case RedListCategory.CR:
                case RedListCategory.EN:
                case RedListCategory.VU:
                case RedListCategory.NT:
                case RedListCategory.LC:
                    break;
                default:
                    // No need to add motive.
                    return;
            }

            if (CalculationProbable.Category == RedListCategory.NT)
            {
                // Add information about criteria A.
                if (CalculationProbable.IsCriteriaAFulfilled(CalculationProbable.Category))
                {
                    textBuilder.AddSentence("Minskningstakten för den svenska populationen bedöms vara nära gränsvärdet för Sårbar (VU).");
                }

                // Add information about criteria B.
                if (CalculationProbable.IsCriteriaBFulfilled(CalculationProbable.Category))
                {
                    if ((CalculationProbable.ExtentOfOccurrence < RedListCalculation.CRITERIA_B1_EN_LIMIT) ||
                        (CalculationProbable.AreaOfOccupancy < RedListCalculation.CRITERIA_B2_EN_LIMIT))
                    {
                        AddMotiveGeographyArea(textBuilder, RedListCategory.EN);
                    }
                    else
                    {
                        // Neither AOO or EOO has a value that is lower
                        // than the limit for EN.
                        AddMotiveGeographyArea(textBuilder, CalculationProbable.Category);
                    }
                    AddMotiveGeographyReason(textBuilder);
                }

                // Add information about criteria C.
                if (CalculationProbable.IsCriteriaCFulfilled(CalculationProbable.Category))
                {
                    textBuilder.AddSentence("Fortgående minskning förekommer i kombination med att antalet reproduktiva individer är lågt vilket gör att " +
                                            TaxonTypeNameDefinite + " rödlistas som " +
                                            GetCategoryInformation(CalculationProbable.Category) + ".");
                }

                // Add information about criteria D.
                if (CalculationProbable.IsCriteriaDFulfilled(CalculationProbable.Category))
                {
                    if (CalculationProbable.IsCriteriaD1Fulfilled(CalculationProbable.Category))
                    {
                        textBuilder.AddSentence("Antalet individer bedöms överstiga gränsvärdet för Sårbar (VU) enligt D-kriteriet.");
                    }

                    if (CalculationProbable.IsCriteriaD2Fulfilled(CalculationProbable.Category))
                    {
                        //textBuilder.AddSentence("Utbredningen är så kraftigt begränsad att den nästan uppfyller gränsvärdena för Sårbar (VU) enligt D-kriteriet.");
                        textBuilder.AddSentence("Utbredningens storlek är nära gränsvärdena för Sårbar (VU) enligt D-kriteriet.");
                        textBuilder.AddSentence("Därför rödlistas " + TaxonTypeNameDefinite + " som " +
                                                GetCategoryInformation(CalculationProbable.Category) + ".");
                        textBuilder.AddSentence(VeryRestrictedAreaClarification);
                    }
                }

                // Add information about criteria E.
                if (CalculationProbable.IsCriteriaEFulfilled(CalculationProbable.Category))
                {
                    textBuilder.AddSentence("Den beräknade risken för utdöende i landet ligger nära gränsvärdet för Sårbar (VU) vilket gör att " + TaxonTypeNameDefinite + " rödlistas som Nära hotad (NT).");
                }
            }

            if ((CalculationProbable.Category == RedListCategory.CR) ||
                (CalculationProbable.Category == RedListCategory.EN) ||
                (CalculationProbable.Category == RedListCategory.VU))
            {
                // Add information about criteria A.
                if (CalculationProbable.IsCriteriaAFulfilled(CalculationProbable.Category))
                {
                    textBuilder.AddSentence("Minskningstakten överstiger gränsvärdet för " + GetCategoryInformation(CalculationProbable.Category) + " enligt A-kriteriet.");
                }

                // Add information about criteria B.
                if (CalculationProbable.IsCriteriaBFulfilled(CalculationProbable.Category))
                {
                    AddMotiveGeographyArea(textBuilder, CalculationProbable.Category);
                    AddMotiveGeographyReason(textBuilder);
                }

                // Add information about criteria C.
                if (CalculationProbable.IsCriteriaCFulfilled(CalculationProbable.Category))
                {
                    textBuilder.AddSentence("Fortgående minskning förekommer i kombination med att antalet reproduktiva individer är lågt vilket gör att " +
                                            TaxonTypeNameDefinite + " hamnar i kategorin " + GetCategoryInformation(CalculationProbable.Category) + ".");
                }

                // Add information about criteria D.
                if (CalculationProbable.IsCriteriaDFulfilled(CalculationProbable.Category))
                {
                    if ((CalculationProbable.Category == RedListCategory.CR) ||
                        (CalculationProbable.Category == RedListCategory.EN) ||
                        CalculationProbable.IsCriteriaD1Fulfilled(CalculationProbable.Category))
                    {
                        textBuilder.AddSentence("Antalet individer bedöms vara lägre än gränsvärdet för " + GetCategoryInformation(CalculationProbable.Category) + " enligt D-kriteriet.");
                    }

                    if (CalculationProbable.IsCriteriaD2Fulfilled(CalculationProbable.Category))
                    {
                        textBuilder.AddSentence("Utbredningen är så kraftigt begränsad att gränsvärdet för " + GetCategoryInformation(CalculationProbable.Category) + " uppfylls enligt D-kriteriet.");
                        textBuilder.AddSentence(VeryRestrictedAreaClarification);
                    }
                }

                // Add information about criteria E.
                if (CalculationProbable.IsCriteriaEFulfilled(CalculationProbable.Category))
                {
                    textBuilder.AddSentence("Den beräknade utdöenderisken överskrider gränsvärdet för " + GetCategoryInformation(CalculationProbable.Category) + ".");
                }
            }
        }

        /// <summary>
        /// Add information about geography area to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        /// <param name="category">Category to use when producing text.</param>
        private void AddMotiveGeographyArea(TextBuilder textBuilder,
                                            RedListCategory category)
        {
            Boolean isCriteriaB1Fulfilled, isCriteriaB2Fulfilled;

            isCriteriaB1Fulfilled = CalculationProbable.IsCriteriaB1Fulfilled(CalculationProbable.Category);
            isCriteriaB2Fulfilled = CalculationProbable.IsCriteriaB2Fulfilled(CalculationProbable.Category);

            // Add information about AOO or EOO.
            if ((isCriteriaB1Fulfilled &&
                 (CalculationBestCase.ExtentOfOccurrence != CalculationWorstCase.ExtentOfOccurrence)) ||
                (isCriteriaB2Fulfilled &&
                 (CalculationBestCase.AreaOfOccupancy != CalculationWorstCase.AreaOfOccupancy)) ||
                (isCriteriaB1Fulfilled && isCriteriaB2Fulfilled))
            {
                textBuilder.BeginSentence("De skattade värdena för");
            }
            else
            {
                textBuilder.BeginSentence("Det skattade värdet för");
            }
            if (isCriteriaB1Fulfilled && isCriteriaB2Fulfilled)
            {
                textBuilder.AddText(" utbredningsområde och förekomstarea");
            }
            else if (isCriteriaB1Fulfilled)
            {
                textBuilder.AddText(" utbredningsområde");
            }
            else if (isCriteriaB2Fulfilled)
            {
                textBuilder.AddText(" förekomstarea");
            }
            if (category == RedListCategory.NT)
            {
                textBuilder.EndSentence(" ligger i närheten av gränsvärdet för Sårbar (VU).");
            }
            else
            {
                textBuilder.EndSentence(" ligger under gränsvärdet för " + GetCategoryInformation(category) + ".");
            }
        }

        /// <summary>
        /// Add information about geography reason to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        private void AddMotiveGeographyReason(TextBuilder textBuilder)
        {
            Int32 numberOfFulfilledCriterias;

            // Count number of fulfilled criterias.
            numberOfFulfilledCriterias = 0;
            if (CalculationProbable.IsCriteriaBA1Fulfilled(CalculationProbable.Category))
            {
                numberOfFulfilledCriterias++;
            }
            if (CalculationProbable.IsCriteriaBA2Fulfilled(CalculationProbable.Category))
            {
                numberOfFulfilledCriterias++;
            }
            if (CalculationProbable.IsCriteriaBCFulfilled)
            {
                numberOfFulfilledCriterias++;
            }
            if (CalculationProbable.IsCriteriaBBFulfilled)
            {
                numberOfFulfilledCriterias++;
            }

            if (numberOfFulfilledCriterias > 0)
            {
                // Add information about fulfilled criterias.
                textBuilder.BeginAndSentence("Detta i kombination med att", numberOfFulfilledCriterias);

                if (CalculationProbable.IsCriteriaBA1Fulfilled(CalculationProbable.Category))
                {
                    switch (CalculationProbable.SeverlyFragmented)
                    {
                        case 1:
                            textBuilder.AddAndText("utbredningsområdet förmodligen är kraftigt fragmenterat");
                            break;
                        case 2:
                            textBuilder.AddAndText("utbredningsområdet är kraftigt fragmenterat");
                            break;
                    }
                }

                if (CalculationProbable.IsCriteriaBA2Fulfilled(CalculationProbable.Category))
                {
                    textBuilder.AddAndText("antalet lokalområden är extremt få");
                }

                if (CalculationProbable.IsCriteriaBCFulfilled)
                {
                    switch (CalculationProbable.ExtremeFluctuations)
                    {
                        case 1:
                            textBuilder.AddAndText("extrema fluktuationer förmodligen förekommer");
                            break;
                        case 2:
                            textBuilder.AddAndText("extrema fluktuationer förekommer");
                            break;
                    }
                }

                if (CalculationProbable.IsCriteriaBBFulfilled)
                {
                    if (CalculationProbable.ContinuingDecline == 1)
                    {
                        textBuilder.AddAndText("fortgående minskning förmodligen förekommer");
                    }
                    else if (CalculationProbable.ContinuingDecline >= 2)
                    {
                        textBuilder.AddAndText("fortgående minskning förekommer");
                    }
                }

                if (Category == RedListCategory.NT)
                {
                    textBuilder.EndSentence(" gör att " +
                                            TaxonTypeNameDefinite +
                                            " uppfyller kriterierna för kategorin " +
                                            GetCategoryInformation(CalculationProbable.Category) + ".");
                }
                else
                {
                    textBuilder.EndSentence(" gör att " +
                                            TaxonTypeNameDefinite +
                                            " uppfyller B-kriteriet.");
                }
            }
        }

        /// <summary>
        /// Add information about number of locations to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        private void AddNumberOfLocations(TextBuilder textBuilder)
        {
            if ((Category != RedListCategory.RE) &&
                CalculationProbable.HasNumberOfLocations)
            {
                textBuilder.BeginSentence("Antalet lokalområden i landet skattas till");
                AddValues(textBuilder,
                          CalculationWorstCase.NumberOfLocations,
                          CalculationProbable.NumberOfLocations,
                          CalculationBestCase.NumberOfLocations);
                textBuilder.EndSentence(".");
                textBuilder.AddSentence(NumberOfLocationsClarification);
            }
        }

        /// <summary>
        /// Add information about population size to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        private void AddPopulationEstimate(TextBuilder textBuilder)
        {
            if (Category != RedListCategory.RE)
            {
                _isPopulationEstimateAdded = CalculationProbable.HasPopulationSize;
                if (CalculationProbable.HasPopulationSize)
                {
                    if (CalculationProbable.PopulationSize >= RedListCalculation.CRITERIA_C_NT_LIMIT)
                    {
                        textBuilder.AddSentence("Antalet reproduktiva individer överstiger gränsvärdet för rödlistning.");
                    }
                    else
                    {
                        textBuilder.BeginSentence("Antalet reproduktiva individer skattas till");
                        AddValues(textBuilder,
                                  CalculationWorstCase.PopulationSize,
                                  CalculationProbable.PopulationSize,
                                  CalculationBestCase.PopulationSize);
                        textBuilder.EndSentence(".");
                    }
                    textBuilder.AddSentence(PopulationSizeClarification);
                }
            }
        }

        /// <summary>
        /// Add population reduction information to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        private void AddPopulationReduction(TextBuilder textBuilder)
        {
            Boolean isCriteriaAAFulfilled,
                    isCriteriaABFulfilled,
                    isCriteriaACFulfilled,
                    isCriteriaADFulfilled,
                    isCriteriaAEFulfilled;
            Int32 numberOfFulfilledCriterias;

            // Test if all values (PopulationReductionA2,
            // PopulationReductionA3 and PopulationReductionA4)
            // are equal or if only PopulationReductionA4 is set.
            if ((CalculationProbable.HasPopulationReductionA2 &&
                 (CalculationProbable.PopulationReductionA2 >= RedListCalculation.CRITERIA_A2_NT_LIMIT) &&
                 CalculationProbable.HasPopulationReductionA3 &&
                 (CalculationProbable.PopulationReductionA3 >= RedListCalculation.CRITERIA_A3_NT_LIMIT) &&
                 CalculationProbable.HasPopulationReductionA4 &&
                 (CalculationProbable.PopulationReductionA4 >= RedListCalculation.CRITERIA_A4_NT_LIMIT) &&
                 (CalculationProbable.PopulationReductionA2 == CalculationProbable.PopulationReductionA3) &&
                 (CalculationProbable.PopulationReductionA3 == CalculationProbable.PopulationReductionA4) &&
                 (CalculationBestCase.PopulationReductionA2 == CalculationBestCase.PopulationReductionA3) &&
                 (CalculationBestCase.PopulationReductionA3 == CalculationBestCase.PopulationReductionA4) &&
                 (CalculationWorstCase.PopulationReductionA2 == CalculationWorstCase.PopulationReductionA3) &&
                 (CalculationWorstCase.PopulationReductionA3 == CalculationWorstCase.PopulationReductionA4)) ||
                ((!CalculationProbable.HasPopulationReductionA2 || (CalculationProbable.PopulationReductionA2 < RedListCalculation.CRITERIA_A2_NT_LIMIT)) &&
                 (!CalculationProbable.HasPopulationReductionA3 || (CalculationProbable.PopulationReductionA3 < RedListCalculation.CRITERIA_A3_NT_LIMIT)) &&
                 CalculationProbable.HasPopulationReductionA4 &&
                 (CalculationProbable.PopulationReductionA4 >= RedListCalculation.CRITERIA_A4_NT_LIMIT)))
            {
                // Add values.
                textBuilder.BeginSentence("Minskningstakten uppgår till");
                AddValues(textBuilder,
                          CalculationBestCase.PopulationReductionA4,
                          CalculationProbable.PopulationReductionA4,
                          CalculationWorstCase.PopulationReductionA4);
                textBuilder.EndSentence(" % inom " + GetNumberOfYears(10, 3, false) + " år.");
                textBuilder.AddSentence("Den minskande trenden har pågått en tid och bedöms fortsätta.");

                // Add clarifications.
                if (CalculationProbable.HasPopulationReductionA2 &&
                    (CalculationProbable.PopulationReductionA2 >= RedListCalculation.CRITERIA_A2_NT_LIMIT))
                {
                    textBuilder.AddSentence(PopulationReductionA2Clarification);
                }
                if (CalculationProbable.HasPopulationReductionA3 &&
                    (CalculationProbable.PopulationReductionA3 >= RedListCalculation.CRITERIA_A3_NT_LIMIT))
                {
                    textBuilder.AddSentence(PopulationReductionA3Clarification);
                }
                if (CalculationProbable.HasPopulationReductionA4 &&
                    (CalculationProbable.PopulationReductionA4 >= RedListCalculation.CRITERIA_A4_NT_LIMIT))
                {
                    textBuilder.AddSentence(PopulationReductionA4Clarification);
                }
            }
            else
            {
                if (CalculationProbable.HasPopulationReductionA2 &&
                    (CalculationProbable.PopulationReductionA2 >= RedListCalculation.CRITERIA_A2_NT_LIMIT))
                {
                    textBuilder.BeginSentence("Minskningstakten har uppgått till");
                    AddValues(textBuilder,
                              CalculationBestCase.PopulationReductionA2,
                              CalculationProbable.PopulationReductionA2,
                              CalculationWorstCase.PopulationReductionA2);
                    textBuilder.EndSentence(" % under de senaste " + GetNumberOfYears(10, 3, false) + " åren.");
                    textBuilder.AddSentence(PopulationReductionA2Clarification);
                }

                if (CalculationProbable.HasPopulationReductionA3 &&
                    (CalculationProbable.PopulationReductionA3 >= RedListCalculation.CRITERIA_A3_NT_LIMIT))
                {
                    textBuilder.BeginSentence("Under de kommande " + GetNumberOfYears(10, 3, false) + " åren förväntas minskningstakten uppgå till");
                    AddValues(textBuilder,
                              CalculationBestCase.PopulationReductionA3,
                              CalculationProbable.PopulationReductionA3,
                              CalculationWorstCase.PopulationReductionA3);
                    textBuilder.EndSentence(" %.");
                    textBuilder.AddSentence(PopulationReductionA3Clarification);
                }

                if (CalculationProbable.HasPopulationReductionA4 &&
                    (CalculationProbable.PopulationReductionA4 >= RedListCalculation.CRITERIA_A4_NT_LIMIT))
                {
                    textBuilder.BeginSentence("Under en tidsperiod om " + GetNumberOfYears(10, 3, false) + " år, som sträcker sig både bakåt och framåt i tiden, så bedöms minskningstakten uppgå till");
                    AddValues(textBuilder,
                              CalculationBestCase.PopulationReductionA4,
                              CalculationProbable.PopulationReductionA4,
                              CalculationWorstCase.PopulationReductionA4);
                    textBuilder.EndSentence(" %.");
                    textBuilder.AddSentence(PopulationReductionA4Clarification);
                }
            }

            // Add information about PopulationReductionA1.
            if (CalculationProbable.HasPopulationReductionA1 &&
                (CalculationProbable.PopulationReductionA1 >= RedListCalculation.CRITERIA_A1_NT_LIMIT))
            {
                textBuilder.BeginSentence("Minskningen har under de senaste " + GetNumberOfYears(10, 3, false) + " åren uppgått till");
                AddValues(textBuilder,
                          CalculationBestCase.PopulationReductionA1,
                          CalculationProbable.PopulationReductionA1,
                          CalculationWorstCase.PopulationReductionA1);
                textBuilder.EndSentence(" % men har nu upphört och populationen förväntas återhämta sig helt.");
                textBuilder.AddSentence(PopulationReductionA1Clarification);
            }

            // Gather information about causes to population reduction.
            isCriteriaAAFulfilled = false;
            isCriteriaABFulfilled = false;
            isCriteriaACFulfilled = false;
            isCriteriaADFulfilled = false;
            isCriteriaAEFulfilled = false;
            if (CalculationProbable.HasPopulationReductionA1 &&
                (CalculationProbable.PopulationReductionA1 >= RedListCalculation.CRITERIA_A1_NT_LIMIT))
            {
                isCriteriaAAFulfilled |= CalculationProbable.IsCriteriaA1AFulfilled;
                isCriteriaABFulfilled |= CalculationProbable.IsCriteriaA1BFulfilled;
                isCriteriaACFulfilled |= CalculationProbable.IsCriteriaA1CFulfilled;
                isCriteriaADFulfilled |= CalculationProbable.IsCriteriaA1DFulfilled;
                isCriteriaAEFulfilled |= CalculationProbable.IsCriteriaA1EFulfilled;
            }
            if (CalculationProbable.HasPopulationReductionA2 &&
                (CalculationProbable.PopulationReductionA2 >= RedListCalculation.CRITERIA_A2_NT_LIMIT))
            {
                isCriteriaAAFulfilled |= CalculationProbable.IsCriteriaA2AFulfilled;
                isCriteriaABFulfilled |= CalculationProbable.IsCriteriaA2BFulfilled;
                isCriteriaACFulfilled |= CalculationProbable.IsCriteriaA2CFulfilled;
                isCriteriaADFulfilled |= CalculationProbable.IsCriteriaA2DFulfilled;
                isCriteriaAEFulfilled |= CalculationProbable.IsCriteriaA2EFulfilled;
            }
            if (CalculationProbable.HasPopulationReductionA3 &&
                (CalculationProbable.PopulationReductionA3 >= RedListCalculation.CRITERIA_A3_NT_LIMIT))
            {
                isCriteriaABFulfilled |= CalculationProbable.IsCriteriaA3BFulfilled;
                isCriteriaACFulfilled |= CalculationProbable.IsCriteriaA3CFulfilled;
                isCriteriaADFulfilled |= CalculationProbable.IsCriteriaA3DFulfilled;
                isCriteriaAEFulfilled |= CalculationProbable.IsCriteriaA3EFulfilled;
            }
            if (CalculationProbable.HasPopulationReductionA4 &&
                (CalculationProbable.PopulationReductionA4 >= RedListCalculation.CRITERIA_A4_NT_LIMIT))
            {
                isCriteriaAAFulfilled |= CalculationProbable.IsCriteriaA4AFulfilled;
                isCriteriaABFulfilled |= CalculationProbable.IsCriteriaA4BFulfilled;
                isCriteriaACFulfilled |= CalculationProbable.IsCriteriaA4CFulfilled;
                isCriteriaADFulfilled |= CalculationProbable.IsCriteriaA4DFulfilled;
                isCriteriaAEFulfilled |= CalculationProbable.IsCriteriaA4EFulfilled;
            }
            numberOfFulfilledCriterias = 0;
            if (isCriteriaAAFulfilled)
            {
                numberOfFulfilledCriterias++;
            }
            if (isCriteriaABFulfilled)
            {
                numberOfFulfilledCriterias++;
            }
            if (isCriteriaACFulfilled)
            {
                numberOfFulfilledCriterias++;
            }
            if (isCriteriaADFulfilled)
            {
                numberOfFulfilledCriterias++;
            }
            if (isCriteriaAEFulfilled)
            {
                numberOfFulfilledCriterias++;
            }

            // Add information about causes to population reduction.
            if (numberOfFulfilledCriterias > 0)
            {
                // Add information about fulfilled criterias.
                textBuilder.BeginAndSentence("Bedömningen baseras på", numberOfFulfilledCriterias);
                if (isCriteriaAAFulfilled)
                {
                    textBuilder.AddAndText("direkt observation");
                    if (CalculationProbable.HasPopulationReductionA1 &&
                        (CalculationProbable.PopulationReductionA1 >= RedListCalculation.CRITERIA_A1_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA1AFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA1AFulfilledClarification);
                    }
                    if (CalculationProbable.HasPopulationReductionA2 &&
                        (CalculationProbable.PopulationReductionA2 >= RedListCalculation.CRITERIA_A2_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA2AFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA2AFulfilledClarification);
                    }
                    if (CalculationProbable.HasPopulationReductionA4 &&
                        (CalculationProbable.PopulationReductionA4 >= RedListCalculation.CRITERIA_A4_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA4AFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA4AFulfilledClarification);
                    }
                }
                if (isCriteriaABFulfilled)
                {
                    textBuilder.AddAndText("ett för " + TaxonTypeNameDefinite + " lämpligt abundansindex");
                    if (CalculationProbable.HasPopulationReductionA1 &&
                        (CalculationProbable.PopulationReductionA1 >= RedListCalculation.CRITERIA_A1_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA1BFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA1BFulfilledClarification);
                    }
                    if (CalculationProbable.HasPopulationReductionA2 &&
                        (CalculationProbable.PopulationReductionA2 >= RedListCalculation.CRITERIA_A2_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA2BFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA2BFulfilledClarification);
                    }
                    if (CalculationProbable.HasPopulationReductionA3 &&
                        (CalculationProbable.PopulationReductionA3 >= RedListCalculation.CRITERIA_A3_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA3BFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA3BFulfilledClarification);
                    }
                    if (CalculationProbable.HasPopulationReductionA4 &&
                        (CalculationProbable.PopulationReductionA4 >= RedListCalculation.CRITERIA_A4_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA4BFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA4BFulfilledClarification);
                    }
                }
                if (isCriteriaACFulfilled)
                {
                    textBuilder.AddAndText("minskad geografisk utbredning och/eller försämrad habitatkvalitet");
                    if (CalculationProbable.HasPopulationReductionA1 &&
                        (CalculationProbable.PopulationReductionA1 >= RedListCalculation.CRITERIA_A1_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA1CFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA1CFulfilledClarification);
                    }
                    if (CalculationProbable.HasPopulationReductionA2 &&
                        (CalculationProbable.PopulationReductionA2 >= RedListCalculation.CRITERIA_A2_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA2CFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA2CFulfilledClarification);
                    }
                    if (CalculationProbable.HasPopulationReductionA3 &&
                        (CalculationProbable.PopulationReductionA3 >= RedListCalculation.CRITERIA_A3_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA3CFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA3CFulfilledClarification);
                    }
                    if (CalculationProbable.HasPopulationReductionA4 &&
                        (CalculationProbable.PopulationReductionA4 >= RedListCalculation.CRITERIA_A4_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA4CFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA4CFulfilledClarification);
                    }
                }
                if (isCriteriaADFulfilled)
                {
                    textBuilder.AddAndText("faktisk eller potentiell exploatering av " + TaxonTypeNameDefinite);
                    if (CalculationProbable.HasPopulationReductionA1 &&
                        (CalculationProbable.PopulationReductionA1 >= RedListCalculation.CRITERIA_A1_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA1DFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA1DFulfilledClarification);
                    }
                    if (CalculationProbable.HasPopulationReductionA2 &&
                        (CalculationProbable.PopulationReductionA2 >= RedListCalculation.CRITERIA_A2_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA2DFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA2DFulfilledClarification);
                    }
                    if (CalculationProbable.HasPopulationReductionA3 &&
                        (CalculationProbable.PopulationReductionA3 >= RedListCalculation.CRITERIA_A3_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA3DFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA3DFulfilledClarification);
                    }
                    if (CalculationProbable.HasPopulationReductionA4 &&
                        (CalculationProbable.PopulationReductionA4 >= RedListCalculation.CRITERIA_A4_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA4DFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA4DFulfilledClarification);
                    }
                }
                if (isCriteriaAEFulfilled)
                {
                    textBuilder.AddAndText("negativ påverkan");
                    if (CalculationProbable.HasPopulationReductionA1 &&
                        (CalculationProbable.PopulationReductionA1 >= RedListCalculation.CRITERIA_A1_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA1EFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA1EFulfilledClarification);
                    }
                    if (CalculationProbable.HasPopulationReductionA2 &&
                        (CalculationProbable.PopulationReductionA2 >= RedListCalculation.CRITERIA_A2_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA2EFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA2EFulfilledClarification);
                    }
                    if (CalculationProbable.HasPopulationReductionA3 &&
                        (CalculationProbable.PopulationReductionA3 >= RedListCalculation.CRITERIA_A3_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA3EFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA3EFulfilledClarification);
                    }
                    if (CalculationProbable.HasPopulationReductionA4 &&
                        (CalculationProbable.PopulationReductionA4 >= RedListCalculation.CRITERIA_A4_NT_LIMIT) &&
                        CalculationProbable.IsCriteriaA4EFulfilled)
                    {
                        textBuilder.AddBracketedText(IsCriteriaA4EFulfilledClarification);
                    }
                }
                textBuilder.EndSentence();
            }
        }

        /// <summary>
        /// Add information about probable regional extinction.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        private void AddProbablyRegionalExtinct(TextBuilder textBuilder)
        {
            if (IsProbablyRegionalExtinct)
            {
                textBuilder.AddSentence("Möjligen kan " +
                                        TaxonTypeNameDefinite +
                                        " vara nationellt utdöd.");
            }
        }

        /// <summary>
        /// Add propability of extinction to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        private void AddPropabilityOfExtinction(TextBuilder textBuilder)
        {
            switch (CalculationProbable.ProbabilityOfExtinction)
            {
                case 0:
                    textBuilder.AddSentence("En kvantitativ analys visar att sannolikheten för utdöende i vilt tillstånd är minst 50% inom " + GetNumberOfYears(10, 3, true) + " år.");
                    break;
                case 1:
                    textBuilder.AddSentence("En kvantitativ analys visar att sannolikheten för utdöende i vilt tillstånd är minst 20% inom " + GetNumberOfYears(20, 5, true) + " år.");
                    break;
                case 2:
                    textBuilder.AddSentence("En kvantitativ analys visar att sannolikheten för utdöende i vilt tillstånd är minst 10 % inom 100 år.");
                    break;
                case 3:
                    textBuilder.AddSentence("En kvantitativ analys visar att sannolikheten för utdöende i vilt tillstånd är minst 5 % inom 100 år.");
                    break;
            }
            if (CalculationProbable.ProbabilityOfExtinction <= 3)
            {
                textBuilder.AddSentence(ProbabilityOfExtinctionClarification);
            }
        }

        /// <summary>
        /// Add information about minimum, probable and
        /// maximum values to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="probableValue">Probable value.</param>
        /// <param name="maxValue">Maximum value.</param>
        private void AddValues(TextBuilder textBuilder,
                               Double minValue,
                               Double probableValue,
                               Double maxValue)
        {
            AddValues(textBuilder, minValue, probableValue, maxValue, null);
        }

        /// <summary>
        /// Add information about minimum, probable and
        /// maximum values to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="probableValue">Probable value.</param>
        /// <param name="maxValue">Maximum value.</param>
        /// <param name="unitLabel">Unit for values.</param>
        private void AddValues(TextBuilder textBuilder,
                               Double minValue,
                               Double probableValue,
                               Double maxValue,
                               String unitLabel)
        {
            if (minValue == maxValue)
            {
                textBuilder.AddText(" " + probableValue);
            }
            else if (Category == RedListCategory.DD)
            {
                textBuilder.AddText(" mellan " +
                                    minValue + " och " +
                                    maxValue);
            }
            else
            {
                textBuilder.AddText(" " +
                                    probableValue + " (" +
                                    minValue + "-" +
                                    maxValue + ")");
            }
            if (unitLabel.IsNotEmpty())
            {
                textBuilder.AddText(" " + unitLabel);
            }
        }

        /// <summary>
        /// Add information about minimum, probable and
        /// maximum values to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="probableValue">Probable value.</param>
        /// <param name="maxValue">Maximum value.</param>
        private void AddValues(TextBuilder textBuilder,
                               Int64 minValue,
                               Int64 probableValue,
                               Int64 maxValue)
        {
            AddValues(textBuilder, minValue, probableValue, maxValue, null);
        }

        /// <summary>
        /// Add information about minimum, probable and
        /// maximum values to criteria documentation.
        /// </summary>
        /// <param name="textBuilder">Text builder.</param>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="probableValue">Probable value.</param>
        /// <param name="maxValue">Maximum value.</param>
        /// <param name="unitLabel">Unit for values.</param>
        private void AddValues(TextBuilder textBuilder,
                               Int64 minValue,
                               Int64 probableValue,
                               Int64 maxValue,
                               String unitLabel)
        {
            if (minValue == maxValue)
            {
                textBuilder.AddText(" " + probableValue);
            }
            else if (Category == RedListCategory.DD)
            {
                textBuilder.AddText(" mellan " +
                                    minValue + " och " +
                                    maxValue);
            }
            else
            {
                textBuilder.AddText(" " +
                                    probableValue + " (" +
                                    minValue + "-" +
                                    maxValue + ")");
            }
            if (unitLabel.IsNotEmpty())
            {
                textBuilder.AddText(" " + unitLabel);
            }
        }

        /// <summary>
        /// Get information about specified category.
        /// </summary>
        /// <param name="category">Category to get information about.</param>
        /// <returns>Information about the category.</returns>
        private String GetCategoryInformation(RedListCategory category)
        {
            switch (category)
            {
                case RedListCategory.EX:
                    return "Utdöd (" + category + ")";
                case RedListCategory.DD:
                    return "Kunskapsbrist (" + category + ")";
                case RedListCategory.RE:
                    return "Nationellt utdöd (" + category + ")";
                case RedListCategory.CR:
                    return "Akut hotad (" + category + ")";
                case RedListCategory.EN:
                    return "Starkt hotad (" + category + ")";
                case RedListCategory.VU:
                    return "Sårbar (" + category + ")";
                case RedListCategory.NT:
                    return "Nära hotad (" + category + ")";
                case RedListCategory.LC:
                    return "Livskraftig (" + category + ")";
                case RedListCategory.NA:
                    return "Ej tillämplig (" + category + ")";
                case RedListCategory.NE:
                    return "Ej bedömd (" + category + ")";
                default:
                    throw new ApplicationException("Not handled redlist category " + category);
            }
        }

        /// <summary>
        /// Get criteria documentation.
        /// </summary>
        /// <returns>Criteria documentation</returns>
        protected override String GetCriteriaDocumentation()
        {
            return _criteriaDocumentation;
        }

        /// <summary>
        /// Get number of years equal to a specified number
        /// of years or specified amount of generations
        /// whichever is longer (up to a maximum of 100 years).
        /// </summary>
        /// <param name="minNumberOfYears">Min value for number of years.</param>
        /// <param name="numberOfGenerations">Number of generations.</param>
        /// <param name="displayNoOfGenerations">Indicates if generation count should be displayed in output.</param>
        /// <returns>Numbers of years as a string.</returns>
        private String GetNumberOfYears(Int64 minNumberOfYears,
                                        Int32 numberOfGenerations,
                                        Boolean displayNoOfGenerations)
        {
            Int64 numberOfYears;

            numberOfYears = minNumberOfYears;
            if (CalculationProbable.HasGenerationLength &&
                (0 < numberOfGenerations) &&
                (minNumberOfYears < ((Int64)(CalculationProbable.GenerationLength * numberOfGenerations))))
            {
                // Show generations instead.
                numberOfYears = (Int64)(CalculationProbable.GenerationLength * numberOfGenerations);
                if (numberOfYears > 30)
                {
                    // Round numberOfYears to nearest ten number.
                    numberOfYears = (Int64)(Math.Round(((Double)numberOfYears) / 10) * 10);
                }
            }
            else
            {
                displayNoOfGenerations = false;
            }
            if (100 < numberOfYears)
            {
                // max 100 years
                numberOfYears = 100;
                displayNoOfGenerations = false;
            }

            if (displayNoOfGenerations)
            {
                if (numberOfGenerations == 1)
                {
                    return numberOfYears.ToString() + " (= " + numberOfGenerations + " generation)";
                }
                else
                {
                    return numberOfYears.ToString() + " (= " + numberOfGenerations + " generationer)";
                }
            }
            else
            {
                return numberOfYears.ToString();
            }
        }

        /// <summary>
        /// Set generationLength.
        /// Unit is year.
        /// Factor: GenerationTime, Id = 671,
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="generationLengthMin">Generation length min value.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="generationLengthProbable">Generation length probable value.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="generationLengthMax">Generation length max value.</param>
        /// <param name="clarification">Clarification about values.</param>
        /// <param name="unitLabel">Label for unit of values.</param>
        public void SetGenerationLength(Boolean hasMin,
                                        Double generationLengthMin,
                                        Boolean hasProbable,
                                        Double generationLengthProbable,
                                        Boolean hasMax,
                                        Double generationLengthMax,
                                        String clarification,
                                        String unitLabel)
        {
            _generationLengthClarification = clarification;
            _generationLengthUnitLabel = unitLabel;

            if (HasNoValues(hasMin, hasProbable, hasMax))
            {
                // No values to set.
                CalculationWorstCase.HasGenerationLength = false;
                CalculationProbable.HasGenerationLength = false;
                CalculationBestCase.HasGenerationLength = false;
            }
            else
            {
                // Fill in missing values.
                SetValues(hasMin,
                          ref generationLengthMin,
                          hasProbable,
                          ref generationLengthProbable,
                          hasMax,
                          ref generationLengthMax,
                          GENERATION_LENGTH_MIN,
                          GENERATION_LENGTH_MAX);

                // Set values in RedListCalculation.
                CalculationWorstCase.GenerationLength = generationLengthMin;
                CalculationProbable.GenerationLength = generationLengthProbable;
                CalculationBestCase.GenerationLength = generationLengthMax;
            }
            SetRedListValues();
        }

        /// <summary>
        /// Set global redlist category.
        /// Factor: GlobalRedlistCategory, Id = 978,
        /// </summary>
        /// <param name="globalRedlistCategory">Global redlist category.</param>
        public void SetGlobalRedlistCategory(String globalRedlistCategory)
        {
            _globalRedlistCategory = globalRedlistCategory;
            SetRedListValues();
        }

        /// <summary>
        /// Set immigration occurs.
        /// Factor: ImmigrationOccurs, Id = 738,
        /// </summary>
        /// <param name="immigrationOccurs">True if immigration occurs.</param>
        public void SetImmigrationOccurs(Boolean immigrationOccurs)
        {
            _immigrationOccurs = immigrationOccurs;
            SetRedListValues();
        }

        /// <summary>
        /// Set introduction.
        /// Factor: RedListCriteriaDocumentationIntroduction, Id = 1950,
        /// </summary>
        /// <param name="introduction">Introduction.</param>
        public void SetIntroduction(String introduction)
        {
            _introduction = introduction;
            SetRedListValues();
        }

        /// <summary>
        /// Set last encounter.
        /// Unit is year.
        /// Factor: LastEncounter, Id = 1948,
        /// </summary>
        /// <param name="hasLastEncounter">Indicates if last encounter has a value.</param>
        /// <param name="lastEncounter">Value for last encounter.</param>
        public void SetLastEncounter(Boolean hasLastEncounter,
                                     Int32 lastEncounter)
        {
            if (hasLastEncounter)
            {
                _lastEncounter = lastEncounter;
            }
            else
            {
                HasLastEncounter = false;
            }
            SetRedListValues();
        }

        /// <summary>
        /// Recalculated red list criteria documentation
        /// based on current red list values.
        /// </summary>
        protected override void SetRedListValues()
        {
            TextBuilder textBuilder;

            if (!IsInInit)
            {
                _criteriaDocumentation = null;
                if (IsEvaluationStatusSet)
                {
                    textBuilder = new TextBuilder();
                    AddIntroduction(textBuilder);
                    AddProbablyRegionalExtinct(textBuilder);
                    if (IsCategorySet &&
                        ((Category == RedListCategory.DD) ||
                         (Category == RedListCategory.RE) ||
                         (Category == RedListCategory.NE) ||
                         (Category == RedListCategory.NA)))
                    {
                        switch (Category)
                        {
                            case RedListCategory.DD:
                                textBuilder.AddSentence("Faktaunderlaget bedöms vara otillräckligt för att avgöra vilken av de olika rödlistningskategorierna som är mest trolig.");
                                break;
                            case RedListCategory.RE:
                                AddCategoryInformation(textBuilder);
                                break;
                            default:
                                // Don't add any explanations.
                                break;
                        }
                    }
                    else
                    {
                        AddPopulationEstimate(textBuilder);
                        AddNumberOfLocations(textBuilder);
                        AddAOOAndEOO(textBuilder);
                        AddContinuingDecline(textBuilder);
                        AddPopulationReduction(textBuilder);
                        AddExtremeFluctuations(textBuilder);
                        AddPropabilityOfExtinction(textBuilder);
                        // AddGenerationLength(textBuilder);
                        AddCategoryInformation(textBuilder);
                        AddMotive(textBuilder);
                        AddGradingInformation(textBuilder);
                        AddConservationDependent(textBuilder);
                        AddCriteria(textBuilder);
                        AddGlobalRedlistCategory(textBuilder);
                    }

                    _criteriaDocumentation = textBuilder.ToString();
                }
            }
        }
    }
}
