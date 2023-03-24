using DomainLayer.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataMapper
{
    public class CustomerMapper : AbstractMapper<Customer>
    {
        private const string tableName = "Zakaznik";
        private static readonly string[] columnNames = { "Jmeno", "Prijmeni", "Email", "Telefon", "Ulice", "Cislo_popisne", "Mesto", "PSC" };

        public CustomerMapper() : base(tableName, columnNames, Methods.All)
        {
        }

        internal override Customer Map(SqlDataReader reader)
        {
            int i = 0;
            Customer newObject = new Customer();
            newObject.FirstName = reader.GetString(++i);
            newObject.Surname = reader.GetString(++i);
            newObject.Email = reader.GetString(++i);
            if (!reader.IsDBNull(++i))
                newObject.PhoneNumber = reader.GetInt64(i);
            if (!reader.IsDBNull(++i))
                newObject.Street = reader.GetString(i);
            newObject.HouseNumber = reader.GetString(++i);
            newObject.Town = reader.GetString(++i);
            newObject.PostalCode = reader.GetInt32(++i);
            return newObject;
        }

        internal override void PrepareCommand(SqlCommand command, Customer obj)
        {
            command.Parameters.AddWithValue("@Jmeno", obj.FirstName);
            command.Parameters.AddWithValue("@Prijmeni", obj.Surname);
            command.Parameters.AddWithValue("@Email", obj.Email);
            command.Parameters.AddWithValue("@Telefon", (object) obj.PhoneNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("@Ulice", (object) obj.Street ?? DBNull.Value);
            command.Parameters.AddWithValue("@Cislo_popisne", obj.HouseNumber);
            command.Parameters.AddWithValue("@Mesto", obj.Town);
            command.Parameters.AddWithValue("@PSC", obj.PostalCode);
        }
    }
}
