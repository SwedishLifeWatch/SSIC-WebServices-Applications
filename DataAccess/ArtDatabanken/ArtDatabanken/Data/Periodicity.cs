using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enum describing a time step in terms of its temperal extent.
    /// </summary>
    [DataContract]
    public enum Periodicity
    {
        /// <summary>
        /// When time steps correspond to actual years.
        /// </summary>
        [EnumMember]
        Yearly,

        /// <summary>
        /// When time steps correspond to actual months.
        /// </summary>
        [EnumMember]
        Monthly,

        /// <summary>
        /// When time steps correspond to acutal weeks.
        /// </summary>
        [EnumMember]
        Weekly,

        /// <summary>
        /// When time steps correspond to actual days.
        /// </summary>
        [EnumMember]
        Daily,

        /// <summary>
        /// When time step corresponds to the month of year independent on acutal year.
        /// </summary>
        [EnumMember]
        MonthOfTheYear,

        /// <summary>
        /// When time step corresponds to the week of year independent on acutal year.
        /// </summary>
        [EnumMember]
        WeekOfTheYear,

        /// <summary>
        /// When time step corresponds to the day of year independent on actutal year.
        /// </summary>
        [EnumMember]
        DayOfTheYear
    }
}
