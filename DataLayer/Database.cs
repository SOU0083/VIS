using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Database: IDisposable
    {
        private const string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=db_rezervace;Integrated Security=True;Connect Timeout=30;
            Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private SqlConnection Connection { get; set; }

        public Database()
        {
            Connection = new SqlConnection(conString);
            Connection.Open();
        }
        
        public void Dispose()
        {
            Connection.Dispose();
        }
        
        public SqlCommand CreateCommand(string strCommand)
        {
            return new SqlCommand(strCommand, Connection);
        }
    }
}
