using System;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class contains handling of period related objects.
    /// </summary>
    public class PeriodManager : ManagerBase
    {
        private static Period _currentPublicPeriod = null;
        private static PeriodList _periods = null;
        private static PeriodList _publicPeriods = null;
        private static PeriodTypeList _periodTypes = null;

        /// <summary>Swedish Redlist is the default period type.</summary>
        private const PeriodTypeId DEFAULT_PERIOD_TYPE = PeriodTypeId.SwedishRedlist;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static PeriodManager()
        {
            ManagerBase.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Makes access to the private member _currentPublicPeriods thread safe.
        /// </summary>
        private static Period CurrentPublicPeriod
        {
            get
            {
                Period currentPublicPeriod;

                lock (_lockObject)
                {
                    currentPublicPeriod = _currentPublicPeriod;
                }
                return currentPublicPeriod;
            }
            set
            {
                lock (_lockObject)
                {
                    _currentPublicPeriod = value;
                }
            }
        }

        /// <summary>
        /// Makes access to the private member _periods thread safe.
        /// </summary>
        private static PeriodList Periods
        {
            get
            {
                PeriodList periods;

                lock (_lockObject)
                {
                    periods = _periods;
                }
                return periods;
            }
            set
            {
                lock (_lockObject)
                {
                    _periods = value;
                }
            }
        }

        /// <summary>
        /// Makes access to the private member _periodTypes thread safe.
        /// </summary>
        private static PeriodTypeList PeriodTypes
        {
            get
            {
                PeriodTypeList periodTypes;

                lock (_lockObject)
                {
                    periodTypes = _periodTypes;
                }
                return periodTypes;
            }
            set
            {
                lock (_lockObject)
                {
                    _periodTypes = value;
                }
            }
        }

        /// <summary>
        /// Makes access to the private member _publicPeriods thread safe.
        /// </summary>
        private static PeriodList PublicPeriods
        {
            get
            {
                PeriodList publicPeriods;

                lock (_lockObject)
                {
                    publicPeriods = _publicPeriods;
                }
                return publicPeriods;
            }
            set
            {
                lock (_lockObject)
                {
                    _publicPeriods = value;
                }
            }
        }

        /// <summary>
        /// A method that retrieves a period object
        /// representing the current public period.
        /// This method only handles the default period type, i.e. 
        /// The Swedish Redlist.
        /// </summary>
        /// <returns>The current public period.</returns>
        public static Period GetCurrentPublicPeriod()
        {
            Period currentPeriod = null;

            for (Int32 getAttempts = 0; (currentPeriod.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadPeriods();
                currentPeriod = CurrentPublicPeriod;
            }
            return currentPeriod;
        }

        

        /// <summary>
        /// Get the requested period object.
        /// </summary>
        /// <param name='periodId'>Id of requested period.</param>
        /// <returns>Requested period.</returns>
        public static Period GetPeriod(Int32 periodId)
        {
            return GetPeriods().Get(periodId);
        }

        /// <summary>
        /// Convert a Period to a WebPeriod.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <returns>A WebPeriod.</returns>
        public static WebPeriod GetPeriod(Period period)
        {
            WebPeriod webPeriod;

            webPeriod = new WebPeriod();
            webPeriod.Id = period.Id;
#if DATA_SPECIFIED_EXISTS
            webPeriod.IdSpecified = true;
#endif
            webPeriod.Information = period.Information;
            webPeriod.Name = period.Name;
            webPeriod.StopUpdate = period.StopUpdate;
#if DATA_SPECIFIED_EXISTS
            webPeriod.StopUpdateSpecified = true;
#endif
            return webPeriod;
        }

        /// <summary>
        /// Get the requested period object.
        /// </summary>
        /// <param name='periodId'>Id of requested period.</param>
        /// <returns>Requested period.</returns>
        public static Period GetPeriod(PeriodId periodId)
        {
            return GetPeriods().Get((Int32)periodId);
        }

        /// <summary>
        /// Get all period objects.
        /// </summary>
        /// <returns>All periods.</returns>
        public static PeriodList GetPeriods()
        {
            PeriodList periods = null;

            for (Int32 getAttempts = 0; (periods.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadPeriods();
                periods = Periods;
            }
            return periods;
        }

        /// <summary>
        /// Get all period objects of a sertain period type.
        /// </summary>
        /// <param name="periodTypeId">Id of the period type.</param>
        /// <returns>All periods of the requested type.</returns>
        public static PeriodList GetPeriods(Int32 periodTypeId)
        {
            PeriodList periods = new PeriodList();

            for (Int32 getAttempts = 0; (periods.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadPeriods();
            }

            foreach (Period period in Periods)
            {
                if (period.PeriodType.Id == periodTypeId)
                {
                    periods.Add(period);
                }
            }
            return periods;
        }

        /// <summary>
        /// Get all period objects of a sertain period type.
        /// </summary>
        /// <param name="periodTypeId">Id enum of the period type.</param>
        /// <returns>All periods of the requested type.</returns>
        public static PeriodList GetPeriods(PeriodTypeId periodTypeId)
        {
            PeriodList periods = new PeriodList();

            for (Int32 getAttempts = 0; (periods.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadPeriods();
            }

            foreach (Period period in Periods)
            {
                if (period.PeriodType.Id == (Int32)periodTypeId)
                {
                    periods.Add(period);
                }
            }
            return periods;
        }

        /// <summary>
        /// Get a requested period type object.
        /// </summary>
        /// <param name="periodTypeId">Id of the period type.</param>
        /// <returns>A period type.</returns>
        public static PeriodType GetPeriodType(Int32 periodTypeId)
        {
            return GetPeriodTypes().Get(periodTypeId);
        }

        /// <summary>
        /// Get the requested period type object.
        /// </summary>
        /// <param name='periodTypeId'>Id of requested period type.</param>
        /// <returns>Requested period type.</returns>
        public static PeriodType GetPeriodType(PeriodTypeId periodTypeId)
        {
            return GetPeriodTypes().Get((Int32)periodTypeId);
        }

        /// <summary>
        /// Get all period type objects.
        /// </summary>
        /// <returns>All period types.</returns>
        public static PeriodTypeList GetPeriodTypes()
        {
            PeriodTypeList periodTypes = null;

            for (Int32 getAttempts = 0; (periodTypes.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadPeriodTypes();
                periodTypes = PeriodTypes;
            }
            return periodTypes;
        }


        /// <summary>
        /// Get all public periods.
        /// This method selects all public periods of the default period type correponding to Swedish Redlist.
        /// </summary>
        /// <returns>All public periods.</returns>
        public static PeriodList GetPublicPeriods()
        {
            PeriodList publicPeriods = GetPublicPeriods(DEFAULT_PERIOD_TYPE);

            return publicPeriods;
        }

        /// <summary>
        /// Get all public periods of a certain period type.
        /// </summary>
        /// <param name="periodTypeId">Id of the period type.</param>
        /// <returns>All public periods of the requested period type.</returns>
        public static PeriodList GetPublicPeriods(Int32 periodTypeId)
        {
            PeriodList publicPeriods = new PeriodList();

            for (Int32 getAttempts = 0; (publicPeriods.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadPeriods();
            }

            foreach (Period period in PublicPeriods)
            {
                if (period.PeriodType.Id == periodTypeId)
                {
                    publicPeriods.Add(period);
                }
            }

            return publicPeriods;
        }

        /// <summary>
        /// Get all public periods of a certain period type.
        /// </summary>
        /// <param name="periodTypeId">Id enum of the period type.</param>
        /// <returns>All public periods of the requested period type.</returns>
        public static PeriodList GetPublicPeriods(PeriodTypeId periodTypeId)
        {
            PeriodList publicPeriods = new PeriodList();

            for (Int32 getAttempts = 0; (publicPeriods.IsEmpty()) && (getAttempts < 3); getAttempts++)
            {
                LoadPeriods();
                if (PublicPeriods.IsNotEmpty())
                {
                    foreach (Period period in PublicPeriods)
                    {
                        if (period.PeriodType.Id == (Int32)periodTypeId)
                        {
                            publicPeriods.Add(period);
                        }
                    }
                }
            }

            return publicPeriods;
        }

        /// <summary>
        /// Set possible period values.
        /// </summary>
        /// <param name='periods'>Possible period values.</param>
        public static void InitialisePeriods(PeriodList periods)
        {
            LoadPeriods(periods);
        }

        /// <summary>
        /// Get periods from web service.
        /// </summary>
        private static void LoadPeriods()
        {
            PeriodList periods;

            if (Periods.IsNull())
            {
                // Get data from web service.
                periods = new PeriodList();
                foreach (WebPeriod webPeriod in WebServiceClient.GetPeriods())
                {
                    periods.Add(new Period(webPeriod.Id,
                                           webPeriod.Year,
                                           webPeriod.Name,
                                           webPeriod.Information,
                                           webPeriod.StopUpdate,
                                           webPeriod.PeriodTypeId));
                }
                LoadPeriods(periods);
            }
        }

        /// <summary>
        /// Update cached period information.
        /// </summary>
        /// <param name="periods">Updated periods.</param>
        private static void LoadPeriods(PeriodList periods)
        {
            Period currentPublicPeriod;
            PeriodList publicPeriods;

            if (periods.IsNotEmpty())
            {
                currentPublicPeriod = null;
                publicPeriods = new PeriodList();
                foreach (Period period in periods)
                {
                    if (period.StopUpdate < DateTime.Today)
                    {
                        if (period.PeriodType.Id == (Int32)DEFAULT_PERIOD_TYPE)
                        {
                            currentPublicPeriod = period;
                        }
                        publicPeriods.Add(period);
                    }
                }

                CurrentPublicPeriod = currentPublicPeriod;
                Periods = periods;
                PublicPeriods = publicPeriods;
            }
        }

        /// <summary>
        /// Get period types from web service.
        /// </summary>
        private static void LoadPeriodTypes()
        {
            PeriodTypeList periodTypes;

            if (PeriodTypes.IsNull())
            {
                // Get data from web service.
                periodTypes = new PeriodTypeList();
                foreach (WebPeriodType webPeriodType in WebServiceClient.GetPeriodTypes())
                {
                    periodTypes.Add(new PeriodType(webPeriodType.Id,
                                                   webPeriodType.Name,
                                                   webPeriodType.Description));
                }
                PeriodTypes = periodTypes;
            }
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        private static void RefreshCache()
        {
            CurrentPublicPeriod = null;
            Periods = null;
            PublicPeriods = null;
        }
    }
}
