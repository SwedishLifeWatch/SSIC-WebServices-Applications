using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// Extension methods for the interface IFactorManager.
    /// </summary>    
    public static class FactorManagerExtension
    {
        /// <summary>
        /// Get current red list period.
        /// </summary>
        /// <param name="factorManager">Factor manager.</param>
        /// <param name="userContext">The user context.</param>
        /// <returns>Current red list period.</returns>
        public static IPeriod GetCurrentRedListPeriod(
            this IFactorManager factorManager,
            IUserContext userContext)
        {
            switch (Environment.MachineName)
            {
                case "MONESES-DEV":
                    {
                        return CoreData.FactorManager.GetPeriod(userContext, PeriodId.Year2015);
                    }
                case "MONESES2-1":
                    {
                        return CoreData.FactorManager.GetPeriod(userContext, PeriodId.Year2015);
                    }
                case "LAMPETRA2-1":
                    {
                        return CoreData.FactorManager.GetPeriod(userContext, PeriodId.Year2015);
                    }
                case "LAMPETRA2-2":
                    {
                        return CoreData.FactorManager.GetPeriod(userContext, PeriodId.Year2015);
                    }
                case "SLU010288":
                    {
                        return CoreData.FactorManager.GetPeriod(userContext, PeriodId.Year2015);
                    }
                case "SLU003354":
                    {
                        return CoreData.FactorManager.GetPeriod(userContext, PeriodId.Year2015);
                    }
                case "SLU005060": //Mattias W
                    {
                        return CoreData.FactorManager.GetPeriod(userContext, PeriodId.Year2015);
                    }
                case "SLU010287": //Matts
                    {
                        return CoreData.FactorManager.GetPeriod(userContext, PeriodId.Year2015);
                    }
                default:
                    {
                        return CoreData.FactorManager.GetPeriod(userContext, PeriodId.Year2015);
                    }
            }
        }
    }
}