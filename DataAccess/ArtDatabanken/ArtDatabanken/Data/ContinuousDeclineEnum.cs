namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enum that contains values for ContinuingDecline enum.
    /// This enum should only be used if a program handles
    /// the values differently.
    /// </summary>
    public enum ContinuousDeclineEnum
    {
        /// <summary>Population grows = -1</summary>
        Grows = -1,

        /// <summary>Population shows no trend = 0</summary>
        ShowsNoTrend = 0,

        /// <summary>Decline likely to go on = 1</summary>
        GoesOn = 1,

        /// <summary>Declines or is expected to decline = 2</summary>
        DeclinesOrIsExpectedToDecline = 2,

        /// <summary>Declines more than 5% in 10 years or 3 generations (less than 20000 individuals) = 3</summary>
        DeclinesMoreThan5 = 3,

        /// <summary>Declines more than 10% in 10 years or 3 generations (less than 10000 individuals) = 4</summary>
        DeclinesMoreThan10 = 4,

        /// <summary>Declines more than 20% in 5 years or 2 generations (less than 2500 individuals) = 5</summary>
        DeclinesMoreThan20 = 5,

        /// <summary>Declines more than 25% in 3 years or 1 generations (less than 250 individuals) = 6</summary>
        DeclinesMoreThan25 = 6
    }
}