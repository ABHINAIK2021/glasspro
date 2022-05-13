using System.Configuration;
using System.Data.SqlClient;

namespace MyApp.Models
{
    public class DBConnection
    {
        private SqlConnection SqlConn = null;
        public SqlConnection GetConnection
        {
            get { return SqlConn; }
            set { SqlConn = value; }
        }

        public DBConnection()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["DefualtConnectionString"].ConnectionString;
            SqlConn = new SqlConnection(ConnectionString);
        }
    }
}