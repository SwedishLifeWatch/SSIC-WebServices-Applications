using System;
using System.Collections.Generic;
using System.Globalization;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.AnalysisService.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.AnalysisService.Data
{
    /// <summary>
    /// Contains extension to the WebTimeStepSpeciesObservationCount class.
    /// </summary>
    public static class WebTimeStepSpeciesObservationCountExtension
    {
        /// <summary>
        /// Load data into the WebTimeStepSpeciesObservationCount instance.
        /// </summary>
        /// <param name="timeStepSpeciesObservationCount"> Information on time step specific species observation counts.</param>
        /// <param name='dataReader'>An open data reader.</param>
        /// <param name="culture"></param>
        public static void LoadData(this WebTimeStepSpeciesObservationCount timeStepSpeciesObservationCount,
                                    DataReader dataReader, CultureInfo culture)
        {
            if (timeStepSpeciesObservationCount != null && dataReader != null)
            {
                Int32 day, month, week, year;
                bool isDaySpecified, isMonthSpecified, isWeekSpecified, isYearSpecified;

                //Retrieving time step specific information from data reader.
                timeStepSpeciesObservationCount.Count = Convert.ToInt64(dataReader.GetInt32(TimeSpeciesObservationCountData.SPECIES_OBSERVATION_COUNT));
                day = dataReader.GetInt32(TimeSpeciesObservationCountData.DAY);
                isDaySpecified = (day > 0);
                week = dataReader.GetInt32(TimeSpeciesObservationCountData.WEEK);
                isWeekSpecified = (week > 0);
                month = dataReader.GetInt32(TimeSpeciesObservationCountData.MONTH);
                isMonthSpecified = (month > 0);
                year = dataReader.GetInt32(TimeSpeciesObservationCountData.YEAR);
                isYearSpecified = (year > 0);

                //Estimating type and name
                if (isYearSpecified
                    && !isMonthSpecified
                    && !isWeekSpecified
                    && !isDaySpecified)
                {
                    timeStepSpeciesObservationCount.Periodicity = Periodicity.Yearly;
                    timeStepSpeciesObservationCount.IsDateSpecified = false;
                    timeStepSpeciesObservationCount.Name = year.ToString(CultureInfo.InvariantCulture);
                }

                if (isYearSpecified
                    && isMonthSpecified
                    && !isWeekSpecified
                    && !isDaySpecified)
                {
                    timeStepSpeciesObservationCount.Periodicity = Periodicity.Monthly;
                    DateTime date = new DateTime(year, month, 1);
                    timeStepSpeciesObservationCount.Date = date;
                    timeStepSpeciesObservationCount.IsDateSpecified = true;
                    if (culture.IsNotNull())
                    {
                        String monthName = culture.DateTimeFormat.GetMonthName(month);
                        monthName = monthName.Substring(0, 3);
                        monthName = year + " " + monthName;

                        timeStepSpeciesObservationCount.Name = monthName;
                    }
                }

                if (isYearSpecified
                    && !isMonthSpecified
                    && isWeekSpecified
                    && !isDaySpecified)
                {
                    timeStepSpeciesObservationCount.Periodicity = Periodicity.Weekly;
                    DateTime date = GetFirstDateOfWeek(year, week);
                    timeStepSpeciesObservationCount.Date = date;
                    timeStepSpeciesObservationCount.IsDateSpecified = true;
                    timeStepSpeciesObservationCount.Name = year + ":" + week;
                }

                if (isYearSpecified
                    && isMonthSpecified
                    && isDaySpecified)
                {
                    timeStepSpeciesObservationCount.Periodicity = Periodicity.Daily;
                    DateTime date = new DateTime(year, month, day);
                    timeStepSpeciesObservationCount.Date = date;
                    timeStepSpeciesObservationCount.IsDateSpecified = true;
                    timeStepSpeciesObservationCount.Name = date.ToString("d", culture.DateTimeFormat);
                }

                if (!isYearSpecified
                    && isMonthSpecified
                    && !isWeekSpecified
                    && !isDaySpecified)
                {
                    timeStepSpeciesObservationCount.Periodicity = Periodicity.MonthOfTheYear;
                    timeStepSpeciesObservationCount.IsDateSpecified = false;
                    timeStepSpeciesObservationCount.Name = month.ToString(CultureInfo.InvariantCulture);
                }

                if (!isYearSpecified
                    && !isMonthSpecified
                    && isWeekSpecified
                    && !isDaySpecified)
                {
                    timeStepSpeciesObservationCount.Periodicity = Periodicity.WeekOfTheYear;
                    timeStepSpeciesObservationCount.IsDateSpecified = false;
                    timeStepSpeciesObservationCount.Name = week.ToString(CultureInfo.InvariantCulture);
                }

                if (!isYearSpecified
                    && !isMonthSpecified
                    && !isWeekSpecified
                    && isDaySpecified)
                {
                    timeStepSpeciesObservationCount.Periodicity = Periodicity.DayOfTheYear;
                    timeStepSpeciesObservationCount.IsDateSpecified = false;
                    timeStepSpeciesObservationCount.Name = day.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        /// <summary>
        /// Load data into the WebTimeStepSpeciesObservationCount instance.
        /// </summary>
        /// <param name="timeStepSpeciesObservationCount"> Information on time step specific species observation counts.</param>
        /// <param name="uniqueValue"></param>
        /// <param name="periodicity"></param>
        /// <param name="culture"></param>
        public static void LoadData(this WebTimeStepSpeciesObservationCount timeStepSpeciesObservationCount,
                                                                    KeyValuePair<string, long> uniqueValue,
                                                                    Periodicity periodicity,
                                                                    CultureInfo culture)
        {
            if (timeStepSpeciesObservationCount != null)
            {
                timeStepSpeciesObservationCount.Count = uniqueValue.Value;
                timeStepSpeciesObservationCount.Periodicity = periodicity;
                timeStepSpeciesObservationCount.IsDateSpecified = false;
                timeStepSpeciesObservationCount.Name = uniqueValue.Key;

                //Special cases
                switch (periodicity)
                {
                    case Periodicity.Monthly:
                        var year = int.Parse(uniqueValue.Key.Split('-')[0]);
                        var month = int.Parse(uniqueValue.Key.Split('-')[1]);

                        timeStepSpeciesObservationCount.Date = new DateTime(year, month, 1);
                        timeStepSpeciesObservationCount.IsDateSpecified = true;
                        timeStepSpeciesObservationCount.Name = string.Format("{0} {1}", year, culture.DateTimeFormat.GetMonthName(month).Substring(0, 3));                        
                        break;

                    case Periodicity.Weekly:
                        year = int.Parse(uniqueValue.Key.Split('-')[0]);
                        var week = int.Parse(uniqueValue.Key.Split('-')[1]);

                        timeStepSpeciesObservationCount.Date = GetFirstDateOfWeek(year, week);
                        timeStepSpeciesObservationCount.IsDateSpecified = true;
                        timeStepSpeciesObservationCount.Name = string.Format("{0}:{1}", year, week);                        
                        break;

                    case Periodicity.Daily:
                        year = int.Parse(uniqueValue.Key.Split('-')[0]);
                        month = int.Parse(uniqueValue.Key.Split('-')[1]);
                        var day = int.Parse(uniqueValue.Key.Split('-')[2]);

                        timeStepSpeciesObservationCount.Date = new DateTime(year, month, day);
                        timeStepSpeciesObservationCount.IsDateSpecified = true;
                        timeStepSpeciesObservationCount.Name = timeStepSpeciesObservationCount.Date.ToString("d", culture.DateTimeFormat);
                        break;
                }
            }
        }

        private static DateTime GetFirstDateOfWeek(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }
    }
}
