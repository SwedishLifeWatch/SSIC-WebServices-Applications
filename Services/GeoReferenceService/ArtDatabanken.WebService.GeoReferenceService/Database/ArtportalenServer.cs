using System;
using System.Data;
using System.Data.SqlClient;
using ArtDatabanken.Database;
using NotUsedCommandBuilder = System.Data.SqlClient.SqlCommandBuilder;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Database;
using SqlCommandBuilder = ArtDatabanken.Database.SqlCommandBuilder;

namespace ArtDatabanken.WebService.GeoReferenceService.Database
{
    /// <summary>
    /// Database interface for the Artportalen database.
    /// </summary>
    public class ArtportalenServer : WebServiceDataServer
    {
        /// <summary>
        /// Connect to the database.
        /// </summary>
        protected override void Connect()
        {
            CipherString cipherString = new CipherString();
            String connectionString;

            // Opens the database connection.
            switch (Environment.MachineName)
            {
                case "ARTSERVICE2-1": // New production web service server.
                    connectionString = "";
                    break;

                case "ARTSERVICE2-2": // New production web service server.
                    connectionString = "";
                    break;

                case "MONESES-DEV": // Test Web Server
                    connectionString = null;
                    break;

                case "SILURUS2-1": // Production Web Server
                    connectionString = null;
                    break;

                case "SILURUS2-2": // Production Web Server
                    connectionString = null;
                    break;

                case "SLU011837": // 
                    connectionString = null;
                    break;

                case "SLU002760": // 
                    connectionString = null;
                    break;

                case "MATLOU8470WW7": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALHCOFDGPCNONAMEDJDCIAMKCOKLCFBPFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEDBGDMLGFLOPKPONFABCBPANHFHOGMLKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADOKBKGOMDLNGPMKHNBCJEDALIHOGOEFAKAAAAAAALHIJIIJNHBKHKEMHJGCCLCINLPLJBLLKCCJMDECOBLFOGFHBEBNPLECLEBDPINEGNAEPBKKAIAILOFAMIGOKIPDPILJEDAHKIBDHHBKGBAJDONFCKNANEGOELBNINKDOEFBAJFOKMLDFFDHFEEOFLLBGLNJMDAFBOIGFGHNMIMBMEJDAKEAJNHPKMHGPGIJGBKFJNBEDADKGMLIJFOJAEBEJHEEOEGEHJBIBNGBBFEMEJJGPFOAMLPOKMOCLJNIKALANOODJDJEDHJOKGADKIBMEMOEMHILAHCGMDDLHDBOLNJHENNKBBIKBDJFDHNGHBEAAAAAAKNEIEKBCKGNAIIENEDKDLOMJCBALMPLFEHMEDLGH";
                    break;

                default:
                    throw new ApplicationException("Unknown web server " + Environment.MachineName);
            }

            Connection = new SqlConnection(cipherString.DecryptText(connectionString));
            Connection.Open();

            if (Connection.State != ConnectionState.Open)
            {
                throw new ApplicationException("Could not connect to database.");
            }
        }

        /// <summary>
        /// Get region related information from ArtPortalen.
        /// </summary>
        /// <returns>
        /// An open data reader with three result sets: regions, region categories and region types.
        /// </returns>
        public DataReader GetRegionInformation()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetRegions");
            return GetReader(commandBuilder, CommandBehavior.Default);
        }
    }
}
