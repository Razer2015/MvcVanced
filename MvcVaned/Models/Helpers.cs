using MvcVanced.Helpers;
using System.Data.Common;

namespace MvcVanced.Models
{
    public class Helpers
    {
        public static DbConnection GetRDSConnection() {
            var appConfig = ConnectionSettings.AppSettings;
            var con = DbProviderFactories.GetFactory("System.Data.SqlClient").CreateConnection();

            string dbname = appConfig["RDS_DB_NAME"];

#if DEBUG
            dbname = null;
#endif

            if (!string.IsNullOrEmpty(dbname)) {
                string username = appConfig["RDS_USERNAME"];
                string password = appConfig["RDS_PASSWORD"];
                string hostname = appConfig["RDS_HOSTNAME"];
                string port = appConfig["RDS_PORT"];

                con.ConnectionString = "Data Source=" + hostname + ";Initial Catalog=" + dbname + ";User ID=" + username + ";Password=" + password + ";";
            }
            else {
                con.ConnectionString = (@"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=aspnet-MvcVanced-fefdc1f0-bd81-4ce9-b712-93a062e01031;Integrated Security=SSPI;AttachDBFilename=|DataDirectory|\aspnet-MvcVanced-fefdc1f0-bd81-4ce9-b712-93a062e01031.mdf;");
            }

            return (con);
        }
    }
}