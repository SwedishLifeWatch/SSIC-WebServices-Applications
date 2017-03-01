﻿using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Proxy.Test.TestDataFactories
{
    public class WebDyntaxaRevisionSpeciesFactTestFactory
    {
        /// <summary>
        /// Creates a WebDyntaxaRevisionSpeciesFact out of predefined data
        /// </summary>
        /// <returns>WebDyntaxaRevisionSpeciesFact</returns>
        public static WebDyntaxaRevisionSpeciesFact Create(int taxonId)
        {
            WebDyntaxaRevisionSpeciesFact refSpeciesFact = new WebDyntaxaRevisionSpeciesFact();
            refSpeciesFact.TaxonId = taxonId;
            refSpeciesFact.RevisionId = 1;
            refSpeciesFact.FactorId = (Int32)FactorId.SwedishOccurrence;
            refSpeciesFact.StatusId = 1;
            refSpeciesFact.QualityId = 1;
            refSpeciesFact.Description = "Test description";
            refSpeciesFact.ReferenceId = 1;
            refSpeciesFact.CreatedBy = Settings.Default.TestUserId;
            refSpeciesFact.CreatedDate = DateTime.Now;
            refSpeciesFact.IsRevisionEventIdSpecified = true;
            refSpeciesFact.RevisionEventId = 1;
            refSpeciesFact.IsChangedInRevisionEventIdSpecified = true;
            refSpeciesFact.ChangedInRevisionEventId = 1;
            refSpeciesFact.IsPublished = false;

            return refSpeciesFact;
        }
    }
}
