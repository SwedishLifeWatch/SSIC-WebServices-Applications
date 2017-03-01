using System;
using System.Net;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of the different types of installation that is used.
    /// </summary>
    public enum InstallationType
    {
        /// <summary>
        /// Development test server for team Artportalen.
        /// </summary>
        ArtportalenTest,

        /// <summary>
        /// Development with local hosting of web applications or web services.
        /// </summary>
        LocalTest,

        /// <summary>
        /// Production servers.
        /// </summary>
        Production,

        /// <summary>
        /// Old test server. This option will be removed when the new test servers
        /// are up and running.
        /// </summary>
        ServerTest,

        /// <summary>
        /// Development test server for team Species Fact.
        /// </summary>
        SpeciesFactTest,

        /// <summary>
        /// Test server used for system testing.
        /// </summary>
        SystemTest,

        /// <summary>
        /// Development test server for team Two Blueberries.
        /// </summary>
        TwoBlueberriesTest
    }

    /// <summary>
    /// Class used to hold information that other parts of the code
    /// can use to distinguish between different configurations.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public static class Configuration
    {
        /// <summary>
        /// Static constructor.
        /// </summary>
        static Configuration()
        {
            CountryId = CountryId.Sweden;
#if DEBUG
            Debug = true;
#else
            Debug = false;
#endif

            // For debugging EVA in production enviorment make sure that InstallationType = InstallationType.Production in code below then data is collected from Artfakta in 
            // sql-esox2-1.artdatadb.slu.se\artinst3

#if ARTPORTALEN_TEST
            InstallationType = InstallationType.ArtportalenTest;

            // Instruct WCF to accept all certificates (like self signed certs)
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, errors) => true;
#else
            if (Debug)
            {
                InstallationType = InstallationType.ServerTest;

                // Instruct WCF to accept all certificates (like self signed certs).
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, errors) => true;
            }
            else
            {
                InstallationType = InstallationType.Production;
            }
#endif
#if IS_ALL_TESTS_RUN
            IsAllTestsRun = true;
#else
            IsAllTestsRun = false;
#endif
        }

        /// <summary>
        /// Get configured country.
        /// </summary>
        public static CountryId CountryId { get; set; }

        /// <summary>
        /// Test if debug is enabled or not.
        /// </summary>
        public static Boolean Debug { get; set; }

        /// <summary>
        /// Test if all automatic test should be run or not.
        /// </summary>
        public static Boolean IsAllTestsRun { get; set; }

        /// <summary>
        /// Get installation type.
        /// </summary>
        public static InstallationType InstallationType { get; set; }

        /// <summary>
        /// Set installation type.
        /// Debug flag and machine name is used to decide which installation type to use.
        /// </summary>
        public static void SetInstallationType()
        {
#if !ARTPORTALEN_TEST
            if (Debug)
            {
                switch (Environment.MachineName)
                {
                    case "ARTFAKTA-DEV": // Team Species Fact test server.
                        InstallationType = InstallationType.SpeciesFactTest;
                        break;

                    case "MONESES-ST": // System test server.
                        InstallationType = InstallationType.SystemTest;
                        break;

                    case "SLU011837": // 
                        InstallationType = InstallationType.TwoBlueberriesTest;
                        break;

                    case "SLU010288": // 
                        InstallationType = InstallationType.SpeciesFactTest;
                        break;

                    case "SLU011360": // 
                        InstallationType = InstallationType.SpeciesFactTest;
                        break;

                    case "SLU012767": // 
                        InstallationType = InstallationType.SpeciesFactTest;
                        break;

                    case "SLU005126": // 
                        InstallationType = InstallationType.SpeciesFactTest;
                        break;

                    case "SLU010940": // 
                        InstallationType = InstallationType.SpeciesFactTest;
                        break;

                    case "SLU004816": // 
                        InstallationType = InstallationType.SpeciesFactTest;
                        break;

                    case "SLU011896": // 
                        InstallationType = InstallationType.SpeciesFactTest;
                        break;

                    case "SLU012177": // 
                        InstallationType = InstallationType.SpeciesFactTest;
                        break;

                    case "SLW-DEV": // Team Two Blueberries test server.
                        InstallationType = InstallationType.TwoBlueberriesTest;
                        break;

                    case "SLU012925": // 
                        InstallationType = InstallationType.TwoBlueberriesTest;
                        break;

                    case "SLU011730": // 
                        InstallationType = InstallationType.TwoBlueberriesTest;
                        break;

                    case "SLU004994": // 
                        InstallationType = InstallationType.TwoBlueberriesTest;
                        break;

                    case "SLU011161": // 
                        InstallationType = InstallationType.TwoBlueberriesTest;
                        break;

                    default:
                        InstallationType = InstallationType.ServerTest;
                        break;
                }
            }
            else
            {
                InstallationType = InstallationType.Production;
            }
#endif
        }
    }
}
