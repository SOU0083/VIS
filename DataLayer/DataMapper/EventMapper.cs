using DomainLayer.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataMapper
{
    public class EventMapper : AbstractMapper<Event>
    {
        private const string tableName = "Udalost";
        private static readonly string[] columnNames = { "Nazev", "ObjektId", "Start", "Konec", "RezervaceOd", "RezervaceDo", "SmazanoOd" };

        private const string SQL_CAN_RESERVE = "SELECT 1 FROM " + tableName + " u JOIN RezervacniObjekt ro ON ro.Id=u.ObjektId WHERE (SELECT COUNT(*) AS Pocet FROM Rezervace r WHERE r.UdalostId=u.Id AND r.SmazanoOd IS NULL) < ro.Pocet AND u.Id=@Id";
        private const string SQL_FIND_TOWNS = "SELECT DISTINCT e. FROM " + tableName + " e WHERE o.SmazanoOd IS NULL";
        private const string SQL_FIND_DESCENDANTS = "SELECT e.* FROM " + tableName + " e JOIN Objekt o ON e.ObjektId=o.Id WHERE e.SmazanoOd IS NULL AND o.HierarchieId.IsDescendantOf(CAST(@HierarchieId AS HIERARCHYID)) = 1";
        private const string SQL_DELETE = "UPDATE " + tableName + " SET SmazanoOd = GETDATE() WHERE Id = @id";
        private const string SQL_DELETE_DESCENDANTS = "UPDATE Udalost SET SmazanoOd = GETDATE() WHERE Id IN (SELECT u.Id FROM Udalost u JOIN Objekt o ON u.ObjektId=o.Id WHERE u.SmazanoOd IS NULL AND o.HierarchieId.IsDescendantOf(CAST(@HierarchieId AS HIERARCHYID)) = 1)";

        public EventMapper() : base(tableName, columnNames, Methods.Insert | Methods.Update | Methods.FindById | Methods.Delete)
        {
        }

        public bool CanReserve(int id)
        {
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand(SQL_CAN_RESERVE))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return true;
                    }
                }
            }
            return false;
        }

        public override bool Delete(int id)
        {
            bool ret = false;
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand(SQL_DELETE))
                {
                    command.Parameters.AddWithValue("@id", id);
                    ret = (command.ExecuteNonQuery() == 1);
                }
            }
            return ret;
        }

        public List<Event> Search(DateTime dateFrom, DateTime dateTo, int maxPrice)
        {
            List<Event> objects = new List<Event>();
            using (Database db = new Database())
            {
                StringBuilder sb = new StringBuilder("SELECT e.*, o.Nazev FROM ");
                sb.Append(tableName);
                sb.Append(" e JOIN Objekt o ON o.Id=e.ObjektId");
                if (maxPrice > 0)
                    sb.Append(" JOIN RezervacniObjekt r ON r.Id=e.ObjektId AND r.Cena <= @Price");
                sb.Append(" WHERE e.SmazanoOd IS NULL");
                if (dateFrom != null && !dateFrom.Equals(""))
                    sb.Append(" AND e.Start >= @From");
                if (dateTo != null && !dateTo.Equals(""))
                    sb.Append(" AND e.Start <= @To");
                using (SqlCommand command = db.CreateCommand(sb.ToString()))
                {
                    if (maxPrice > 0)
                        command.Parameters.AddWithValue("@Price", maxPrice);
                    if (dateFrom != null && !dateFrom.Equals(""))
                        command.Parameters.AddWithValue("@From", dateFrom);
                    if (dateTo != null && !dateTo.Equals(""))
                        command.Parameters.AddWithValue("@To", dateTo);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Event e = MapEvent(reader);
                            e.Event_Object.Name = reader.GetString(reader.FieldCount-1);
                            objects.Add(e);
                        }
                    }
                }
            }
            return objects;
        }
        public List<Event> FindTypes(string path)
        {
            List<Event> objects = new List<Event>();
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand(SQL_FIND_DESCENDANTS))
                {
                    command.Parameters.AddWithValue("@HierarchieId", path);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            objects.Add(MapEvent(reader));
                    }
                }
            }
            return objects;
        }

        public List<Event> FindDescendantsEvents(string path)
        {
            List<Event> objects = new List<Event>();
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand(SQL_FIND_DESCENDANTS))
                {
                    command.Parameters.AddWithValue("@HierarchieId", path);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            objects.Add(MapEvent(reader));
                    }
                }
            }
            return objects;
        }
        public bool DeleteDescendantsEvents(string path)
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

        internal Event MapEvent(SqlDataReader reader)
        {
            Event obj = Map(reader);
            obj.Id = reader.GetInt32(0);
            return obj;
        }
        internal override Event Map(SqlDataReader reader)
        {
            int i = 0;
            Event newObject = new Event();
            newObject.Name = reader.GetString(++i);
            newObject.Event_Object = new StructuralObject();
            newObject.Event_Object.Id = reader.GetInt32(++i);
            newObject.Start = reader.GetDateTime(++i);
            newObject.End = reader.GetDateTime(++i);
            newObject.CanReserveFrom = reader.GetDateTime(++i);
            newObject.CanReserveTo = reader.GetDateTime(++i);
            if (!reader.IsDBNull(++i))
                newObject.DeletedFrom = reader.GetDateTime(i);
            return newObject;
        }

        internal override void PrepareCommand(SqlCommand command, Event obj)
        {
            command.Parameters.AddWithValue("@Nazev", obj.Name);
            command.Parameters.AddWithValue("@ObjektId", obj.Event_Object.Id);
            command.Parameters.AddWithValue("@Start", obj.Start);
            command.Parameters.AddWithValue("@Konec", obj.End);
            command.Parameters.AddWithValue("@RezervaceOd", obj.CanReserveFrom);
            command.Parameters.AddWithValue("@RezervaceDo", obj.CanReserveTo);
            command.Parameters.AddWithValue("@SmazanoOd", (object)obj.DeletedFrom ?? DBNull.Value);
        }
    }
}
