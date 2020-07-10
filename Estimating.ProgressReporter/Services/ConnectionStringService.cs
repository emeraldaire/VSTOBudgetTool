using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Estimating.ProgressReporter.Services
{
    public class ConnectionStringService
    {
        /// <summary>
        /// Returns an 'SqlConnection' object for the provided database 
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public static SqlConnection GetConnection(string databaseName)
        {
            switch (databaseName)
            {
                case "Estimate":
                    return new SqlConnection(GetConnectionString("Estimate"));
                case "Spectrum":
                    return new SqlConnection(GetConnectionString("Spectrum"));
                case "TimoutTest":
                    return new SqlConnection(GetConnectionString("TimeoutTest"));
                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns a connection string for the specified database.
        /// </summary>
        /// <remarks>
        /// This is the interface for the connection strings defined in application properties.
        /// </remarks>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string databaseName)
        {
            switch (databaseName)
            {
                case "Estimate":
                    return Properties.Settings.Default.Estimate;
                case "Spectrum":
                    return Properties.Settings.Default.SPECTRUM;
                default:
                    return null;
            }
        }

    }
}
