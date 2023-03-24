using DomainLayer.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataMapper
{
    public class ReservationMapper: AbstractMapper<Reservation>
    {
        private const string tableName = "Rezervace";
        private static readonly string[] columnNames = { "ZakaznikId", "RezervacniObjektId", "UdalostId", "Od", "Do", "SmazanoOd" };

        private const string SQL_FIND_CUSTOMER = "SELECT r.*, ro.Cena,o.Nazev,e.Nazev FROM Zakaznik z JOIN Rezervace r ON r.ZakaznikId=z.Id JOIN RezervacniObjekt ro ON ro.Id=r.RezervacniObjektId JOIN Objekt o ON o.Id=ro.Id JOIN Udalost e ON e.Id=r.UdalostId WHERE z.Id=@Id AND r.SmazanoOd IS NULL";
        private const string SQL_FIND_EVENT = "SELECT c.*,r.Id FROM Zakaznik c JOIN " + tableName + " r ON r.ZakaznikId=c.Id JOIN Udalost e ON e.Id=r.UdalostId WHERE e.Id=@Id AND r.SmazanoOd IS NULL";
        private const string SQL_FIND_DESCENDANTS = "SELECT c.*,r.Id FROM Zakaznik c JOIN " + tableName + @" r ON r.ZakaznikId=c.Id AND r.SmazanoOd IS NULL JOIN Objekt o ON o.Id=r.RezervacniObjektId 
            WHERE r.SmazanoOd IS NULL AND o.HierarchieId.IsDescendantOf(CAST(@HierarchieId AS HIERARCHYID)) = 1";
        private const string SQL_DELETE_EVENT = "UPDATE " + tableName + " SET SmazanoOd = GETDATE() WHERE Id IN (SELECT r.Id FROM Rezervace r JOIN Udalost e ON e.Id=r.UdalostId WHERE r.SmazanoOd IS NULL AND e.Id=@Id)";
        private const string SQL_DELETE_DESCENDANTS = "UPDATE " + tableName + " SET SmazanoOd = GETDATE() WHERE Id IN (SELECT r.Id FROM Rezervace r JOIN Objekt o ON r.RezervacniObjektId=o.Id WHERE r.SmazanoOd IS NULL AND o.HierarchieId.IsDescendantOf(CAST(@HierarchieId AS HIERARCHYID)) = 1)";

        public ReservationMapper() : base(tableName, columnNames, Methods.All)
        {
        }

        public List<Reservation> FindCustomerReservations(int id)
        {
            List<Reservation> objects = new List<Reservation>();
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand(SQL_FIND_CUSTOMER))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Reservation obj = Map(reader);
                            obj.Id = reader.GetInt32(0);
                            objects.Add(obj);
                            obj.Reservation_ReservationObject.Price = reader.GetInt32(reader.FieldCount - 3);
                            obj.Reservation_ReservationObject.Name = reader.GetString(reader.FieldCount - 2);
                            obj.Reservation_Event.Name = reader.GetString(reader.FieldCount - 1);
                        }
                    }
                }
            }
            return objects;
        }
        public List<Reservation> FindEventReservations(int id)
        {
            List<Reservation> objects = new List<Reservation>();
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand(SQL_FIND_EVENT))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Reservation obj = new Reservation();
                            CustomerMapper mapper = new CustomerMapper();
                            obj.Reservation_Customer = mapper.Map(reader);
                            obj.Id = reader.GetInt32(reader.FieldCount-1);
                            objects.Add(obj);
                        }
                    }
                }
            }
            return objects;
        }
        public List<Reservation> FindDescendantsReservations(string path)
        {
            List<Reservation> objects = new List<Reservation>();
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand(SQL_FIND_DESCENDANTS))
                {
                    command.Parameters.AddWithValue("@HierarchieId", path);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Reservation obj = new Reservation();
                            CustomerMapper mapper = new CustomerMapper();
                            obj.Reservation_Customer = mapper.Map(reader);
                            obj.Id = reader.GetInt32(reader.FieldCount - 1);
                            objects.Add(obj);
                        }
                    }
                }
            }
            return objects;
        }
        public bool DeleteEventReservations(int id)
        {
            bool ret = false;
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand(SQL_DELETE_EVENT))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    ret = (command.ExecuteNonQuery() == 1);
                }
            }
            return ret;
        }
        public bool DeleteDescendantsReservations(string path)
        {
            bool ret = false;
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand(SQL_DELETE_DESCENDANTS))
                {
                    command.Parameters.AddWithValue("@HierarchieId", path);
                    ret = (command.ExecuteNonQuery() == 1);
                }
            }
            return ret;
        }

        internal override Reservation Map(SqlDataReader reader)
        {
            int i = 0;
            Reservation newObject = new Reservation();
            newObject.Reservation_Customer = new Customer();
            newObject.Reservation_Customer.Id = reader.GetInt32(++i);
            newObject.Reservation_ReservationObject = new ReservationObject();
            newObject.Reservation_ReservationObject.Id = reader.GetInt32(++i);
            newObject.Reservation_Event = new Event();
            newObject.Reservation_Event.Id = reader.GetInt32(++i);
            if (!reader.IsDBNull(++i))
                newObject.From = reader.GetDateTime(i);
            if (!reader.IsDBNull(++i))
                newObject.To = reader.GetDateTime(i);
            if (!reader.IsDBNull(++i))
                newObject.DeletedFrom = reader.GetDateTime(i);
            return newObject;
        }

        internal override void PrepareCommand(SqlCommand command, Reservation obj)
        {
            command.Parameters.AddWithValue("@ZakaznikId", obj.Reservation_Customer.Id);
            command.Parameters.AddWithValue("@RezervacniObjektId", obj.Reservation_ReservationObject.Id);
            command.Parameters.AddWithValue("@UdalostId", obj.Reservation_Event.Id);
            command.Parameters.AddWithValue("@Od", (object)obj.From ?? DBNull.Value);
            command.Parameters.AddWithValue("@Do", (object)obj.To ?? DBNull.Value);
            command.Parameters.AddWithValue("@SmazanoOd", (object)obj.DeletedFrom ?? DBNull.Value);
        }
    }
}
