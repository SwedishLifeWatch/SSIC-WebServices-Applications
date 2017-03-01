namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of species observation data sources.
    /// </summary>
    public enum SpeciesObservationDataProviderId
    {
        /// <summary>
        /// Species gateway (swedish name Artportalen).
        /// </summary>
        SpeciesGateway = 1,

        /// <summary>
        /// Data source Observationsdatabasen.
        /// </summary>
        Observationsdatabasen = 2,

        /// <summary>
        /// Data source DINA. (GBIF)
        /// </summary>
        Dina = 3,

        /// <summary>
        /// Data source ArtDatabankenService.
        /// </summary>
        ArtDatabankenService = 4,

        /// <summary>
        /// Data source MVM.
        /// </summary>
        Mvm = 5,

        /// <summary>
        /// Data source SLU Aqua NORS.
        /// </summary>
        Nors = 6,

        /// <summary>
        /// Data source SLU Aqua SERS.
        /// </summary>
        Sers = 7,

        /// <summary>
        /// Data source Wram.
        /// </summary>
        Wram = 8,

        /// <summary>
        /// Data source SMHI Shark.
        /// </summary>
        Shark = 9,

        /// <summary>
        /// Data source KUL.
        /// </summary>
        Kul = 10,

        /// <summary>
        /// Data source Lund Botanical Museum (GBIF).
        /// </summary>
        LundBotanicalMuseum = 11,

        /// <summary>
        /// Data source Beetles (GBIF).
        /// </summary>
        Lsm = 12,

        /// <summary>
        /// Data source Lund Museum of Zoology (GBIF).
        /// </summary>
        LundMuseumOfZoology = 13,

        /// <summary>
        /// Data source Bird ringing centre in Sweden (GBIF).
        /// </summary>
        BirdRingingCentre = 14,

        /// <summary>
        /// Data source Porpoises (GBIF).
        /// </summary>
        Porpoises = 15,

        /// <summary>
        /// Data source Biologiska museet, Oskarshamn (GBIF).
        /// </summary>
        HerbariumOfOskarshamn = 16,

        /// <summary>
        /// Data source Herbarium of Umeå University (GBIF).
        /// </summary>
        HerbariumOfUmeaUniversity = 17,

        /// <summary>
        /// Data source Entomological Collections (NHRS) from Swedish Museum of Natural History (GBIF).
        /// </summary>
        EntomologicalCollections = 18,

        /// <summary>
        /// Data source Swedish Malaise Trap Project (SMTP) from Foundation Station Linné (GBIF).
        /// </summary>
        SwedishMalaiseTrapProject = 19
    }
}
