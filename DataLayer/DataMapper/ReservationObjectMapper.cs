using DomainLayer.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataMapper
{
    public class ReservationObjectMapper : AbstractMapper<ReservationObject>
    {
        private const string tableName = "RezervacniObjekt";
        private static readonly string[] columnNames = { "Cena", "Pocet", "TypRezervace" };

        public ReservationObjectMapper() : base(tableName, columnNames, Methods.All)
        {
        }

        internal override ReservationObject Map(SqlDataReader reader)
        {
            int i = 0;
            ReservationObject newObject = new ReservationObject();
            newObject.Price = reader.GetInt32(++i);
            newObject.Quantity = reader.GetInt32(++i);
            newObject.Type = reader.GetInt16(++i);
            return newObject;
        }

        internal override void PrepareCommand(SqlCommand command, ReservationObject obj)
        {
            command.Parameters.AddWithValue("@Cena", obj.Price);
            command.Parameters.AddWithValue("@Pocet", obj.Quantity);
            command.Parameters.AddWithValue("@TypRezervace", obj.Type);
        }
    }
}
