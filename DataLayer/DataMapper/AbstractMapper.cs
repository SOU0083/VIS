using DomainLayer.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataMapper
{
    public abstract class AbstractMapper<T> where T:IEntity
    {
        [Flags]
        protected enum Methods
        {
            None = 0x00, Delete = 0x01, Insert = 0x02, Update = 0x04, Find = 0x08, FindById = 0x10,
            All = Delete | Insert | Update | Find | FindById
        }

        private readonly string tableName;
        private readonly string[] columnNames;  //Without Id
        private readonly Methods methods;

        protected AbstractMapper(string tableName, string[] columnNames, Methods methods)
        {
            this.tableName = tableName;
            this.columnNames = columnNames;
            this.methods = methods;
        }

        public virtual bool Delete(int id)
        {
            if ((methods & Methods.Delete) == 0)
                throw new NotSupportedException();
            bool ret = false;
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand("DELETE FROM " + tableName + " WHERE Id = @id"))
                {
                    command.Parameters.AddWithValue("@id", id);
                    ret = (command.ExecuteNonQuery() == 1);
                }
            }
            return ret;
        }
        public virtual bool Update(T obj)
        {
            if ((methods & Methods.Update) == 0)
                throw new NotSupportedException();
            bool ret = false;
            using (Database db = new Database())
            {
                StringBuilder sb = new StringBuilder("UPDATE ");
                sb.Append(tableName);
                sb.Append(" SET ");
                for (int i = 0; i < columnNames.Length; i++)
                {
                    if (i != 0)
                        sb.Append(", ");
                    sb.Append(columnNames[i]);
                    sb.Append("=@");
                    sb.Append(columnNames[i]);
                }
                sb.Append(" WHERE Id = @id");
                string sql = sb.ToString();
                using (SqlCommand command = db.CreateCommand(sql))
                {
                    command.Parameters.AddWithValue("@id", obj.Id);
                    PrepareCommand(command, obj);
                    ret = (command.ExecuteNonQuery() == 1);
                }
            }
            return ret;
        }
        public virtual int Insert(T obj)
        {
            if ((methods & Methods.Insert) == 0)
                throw new NotSupportedException();
            using (Database db = new Database())
            {
                StringBuilder sb = new StringBuilder("INSERT INTO ");
                sb.Append(tableName);
                sb.Append(" VALUES (");
                for (int i = 0; i < columnNames.Length; i++)
                {
                    if (i != 0)
                        sb.Append(", ");
                    sb.Append("@");
                    sb.Append(columnNames[i]);
                }
                sb.Append(");" +
                    "SELECT CAST(SCOPE_IDENTITY() AS INT);");
                string sql = sb.ToString();
                using (SqlCommand command = db.CreateCommand(sql))
                {
                    PrepareCommand(command, obj);
                    object o = command.ExecuteScalar();
                    if (o == null)
                        return -1;
                    return (int) o;
                }
            }
        }
        public virtual List<T> Find()
        {
            if ((methods & Methods.Find) == 0)
                throw new NotSupportedException();
            List<T> objects = new List<T>();
            using (Database db = new Database())
            {
                string sql = "SELECT * FROM " + tableName;
                if (typeof(IDeleted).IsAssignableFrom(typeof(T)))
                    sql += " WHERE SmazanoOd IS NULL";
                using (SqlCommand command = db.CreateCommand(sql))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T obj = Map(reader);
                            obj.Id = reader.GetInt32(0);
                            objects.Add(obj);
                        }
                    }
                }
            }
            return objects;
        }
        public virtual T Find(int id)
        {
            if ((methods & Methods.FindById) == 0)
                throw new NotSupportedException();
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand("SELECT * FROM " + tableName + " WHERE Id = @id"))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            T obj = Map(reader);
                            obj.Id = id;
                            return obj;
                        }
                    }
                }
            }
            return default(T);
        }
        internal abstract void PrepareCommand(SqlCommand command, T obj);
        internal abstract T Map(SqlDataReader reader);
    }
}
