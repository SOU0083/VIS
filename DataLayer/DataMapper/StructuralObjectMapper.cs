using DomainLayer.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataMapper
{
    public class StructuralObjectMapper : AbstractMapper<StructuralObject>
    {
        private const string tableName = "Objekt";
        private static readonly string[] columnNames = { "HierarchieId", "Nazev", "SmazanoOd" };

        private const string SQL_FIND = "SELECT Id, HierarchieId.ToString(), Nazev, SmazanoOd FROM " + tableName + " WHERE Id = @id";
        private const string SQL_FIND_DESCENDANTS = "SELECT Id, HierarchieId.ToString(), Nazev, SmazanoOd FROM " + tableName + " WHERE SmazanoOd IS NULL AND HierarchieId.IsDescendantOf(CAST(@HierarchieId AS HIERARCHYID)) = 1 ORDER BY HierarchieId";
        private const string SQL_DELETE = "UPDATE " + tableName + " SET SmazanoOd = GETDATE() WHERE SmazanoOd IS NULL AND HierarchieId.IsDescendantOf(CAST(@HierarchieId AS HIERARCHYID)) = 1";
        private const string SQL_INSERT = "INSERT INTO " + tableName + @" VALUES (CAST(@HierarchieId AS hierarchyid), @Nazev, @SmazanoOd);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

        public StructuralObjectMapper() : base(tableName, columnNames, Methods.Insert | Methods.Update | Methods.FindById)
        {
        }

        public override StructuralObject Find(int id)
        {
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand(SQL_FIND))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            StructuralObject obj = Map(reader);
                            obj.Id = id;
                            return obj;
                        }
                    }
                }
            }
            return null;
        }
        public List<StructuralObject> FindDescendants(string path)
        {
            List<StructuralObject> objects = new List<StructuralObject>();
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand(SQL_FIND_DESCENDANTS))
                {
                    command.Parameters.AddWithValue("@HierarchieId", path);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StructuralObject obj = Map(reader);
                            obj.Id = reader.GetInt32(0);
                            objects.Add(obj);
                        }
                    }
                }
            }
            return objects;
        }
        public bool DeleteDescendants(string path)
        {
            bool ret = false;
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand(SQL_DELETE))
                {
                    command.Parameters.AddWithValue("@HierarchieId", path);
                    ret = (command.ExecuteNonQuery() == 1);
                }
            }
            return ret;
        }

        public override int Insert(StructuralObject obj)
        {
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand(SQL_INSERT))
                {
                    PrepareCommand(command, obj);
                    object o = command.ExecuteScalar();
                    if (o == null)
                        return -1;
                    return (int)o;
                }
            }
        }

        internal override StructuralObject Map(SqlDataReader reader)
        {
            int i = 0;
            StructuralObject newObject = new StructuralObject();
            newObject.HierarchyId = reader.GetString(++i);
            newObject.Name = reader.GetString(++i);
            if (!reader.IsDBNull(++i))
                newObject.DeletedFrom = reader.GetDateTime(i);
            return newObject;
        }

        internal override void PrepareCommand(SqlCommand command, StructuralObject obj)
        {
            command.Parameters.AddWithValue("@HierarchieId", obj.HierarchyId);
            command.Parameters.AddWithValue("@Nazev", obj.Name);
            command.Parameters.AddWithValue("@SmazanoOd", (object)obj.DeletedFrom ?? DBNull.Value);
        }
    }
}
