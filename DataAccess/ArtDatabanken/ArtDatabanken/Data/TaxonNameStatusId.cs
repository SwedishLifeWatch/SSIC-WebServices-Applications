namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of possible status for taxon names.
    /// </summary>
    public enum TaxonNameStatusId
    {
        /// <summary>Removed name.</summary>
        Removed = -1,

        /// <summary>Approved naming (Correct naming).</summary>
        ApprovedNaming = 0,

        /// <summary>Preliminary suggestion.</summary>
        PreliminarySuggestion = 1,

        /// <summary>Invalid naming (Incorrect (other)).</summary>
        InvalidNaming = 2,

        /// <summary>Misspelled name.</summary>
        Misspelled = 3,

        /// <summary>Obsrek.</summary>
        Obsrek = 4,

        /// <summary>Provisional.</summary>
        Provisional = 5,

        /// <summary>Informal.</summary>
        Informal = 6,

        /// <summary>Suppressed.</summary>
        Suppressed = 7,

        /// <summary>Incorrect citation.</summary>
        IncorrectCitation = 8,

        /// <summary>Unneccessary.</summary>
        Unneccessary = 9,

        /// <summary>Undescribed.</summary>
        Undescribed = 10,

        /// <summary>Unpublished.</summary>
        Unpublished = 11   



        ///// <summary>Deleted name.</summary>
        //Deleted = -1,

        ///// <summary>Correct naming.</summary>
        //Correct = 0,

        ///// <summary>Preliminary suggestion.</summary>
        //Preliminary = 1,

        ///// <summary>Incorrect (other)</summary>
        //IncorrectOther = 2,

        ///// <summary>Misspelled name.</summary>
        //Misspelled = 3,

        ///// <summary>Obsrek.</summary>
        //Obsrek = 4,

        ///// <summary>Provisional.</summary>
        //Provisional = 5,

        ///// <summary>Informal.</summary>
        //Informal = 6,

        ///// <summary>Suppressed.</summary>
        //Suppressed = 7,

        ///// <summary>IncorrectCitation.</summary>
        //IncorrectCitation = 8,

        ///// <summary>Unneccessary.</summary>
        //Unneccessary = 9,

        ///// <summary>Undescribed.</summary>
        //Undescribed = 10,

        ///// <summary>Unpublished.</summary>
        //Unpublished = 11    
    }
}
