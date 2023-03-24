using DomainLayer.DomainModel;
using DomainLayer.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataMapper
{
    public class InstitutionMapper : AbstractMapper<Institution>
    {
        private const string tableName = "Instituce";
        private static readonly string[] columnNames = { "Email", "Telefon", "Ulice", "Cislo_popisne", "Mesto", "PSC" };

        private const string SQL_FIND = "SELECT i.*,o.Nazev,o.HierarchieId.ToString() FROM " + tableName + " i JOIN Objekt o ON i.Id=o.Id WHERE o.SmazanoOd IS NULL";
        private const string SQL_FIND_TOWNS = "SELECT DISTINCT i.Mesto FROM " + tableName + " i JOIN Objekt o ON i.Id=o.Id WHERE o.SmazanoOd IS NULL";

        private StructuralObjectMapper structuralObjectMapper = new StructuralObjectMapper();

        public InstitutionMapper() : base(tableName, columnNames, Methods.All)
        {
        }

        public List<InstitutionDTO> Search(string name, string category, string town)
        {
            List<InstitutionDTO> objects = new List<InstitutionDTO>();
            using (Database db = new Database())
            {
                StringBuilder sb = new StringBuilder("SELECT i.*,o.Nazev,o.HierarchieId.ToString() FROM ");
                sb.Append(tableName);
                sb.Append(" i JOIN Objekt o ON i.Id=o.Id");
                if (category != null && !category.Equals(""))
                    sb.Append(" JOIN Instituce_KategorieInstituce ic ON ic.InstituceId=o.Id JOIN KategorieInstituce c ON c.Id=ic.KategorieInstituceId AND c.Nazev=@Kategorie");
                sb.Append(" WHERE o.SmazanoOd IS NULL");
                if (name != null && !name.Equals(""))
                    sb.Append(" AND o.Nazev LIKE '%' + @Nazev + '%'");
                if (town != null && !town.Equals(""))
                    sb.Append(" AND i.Mesto = @Mesto");
                using (SqlCommand command = db.CreateCommand(sb.ToString()))
                {
                    if (name != null && !name.Equals(""))
                        command.Parameters.AddWithValue("@Nazev", "%" + name + "%");
                    if (category != null && !category.Equals(""))
                        command.Parameters.AddWithValue("@Kategorie", category);
                    if (town != null && !town.Equals(""))
                        command.Parameters.AddWithValue("@Mesto", town);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            objects.Add(MapInstitutionDTO(reader));
                    }
                }
            }
            return objects;
        }
        public List<string> FindTowns()
        {
            List<string> objects = new List<string>();
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand(SQL_FIND_TOWNS))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            objects.Add(reader.GetString(0));
                    }
                }
            }
            return objects;
        }
        public List<InstitutionDTO> FindWithObject()
        {
            List<InstitutionDTO> objects = new List<InstitutionDTO>();
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand(SQL_FIND))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            objects.Add(MapInstitutionDTO(reader));
                    }
                }
            }
            return objects;
        }
        public override int Insert(Institution obj)
        {
            int ret = -1;
            using (Database db = new Database())
            {
                ret = structuralObjectMapper.Insert(obj);

                StringBuilder sb = new StringBuilder("INSERT INTO ");
                sb.Append(tableName);
                sb.Append(" VALUES (@Id");
                for (int i = 0; i < columnNames.Length; i++)
                {
                    sb.Append(", ");
                    sb.Append("@");
                    sb.Append(columnNames[i]);
                }
                sb.Append(")");
                string sql = sb.ToString();
                using (SqlCommand command = db.CreateCommand(sql))
                {
                    command.Parameters.AddWithValue("@Id", ret);
                    PrepareCommand(command, obj);
                    command.ExecuteNonQuery();
                }
            }
            return ret;
        }

        internal InstitutionDTO MapInstitutionDTO(SqlDataReader reader)
        {
            Institution inst = Map(reader);
            InstitutionDTO obj = new InstitutionDTO()
            {
                Categories = inst.Categories,
                Email = inst.Email,
                HouseNumber = inst.HouseNumber,
                PhoneNumber = inst.PhoneNumber,
                PostalCode = inst.PostalCode,
                Street = inst.Street,
                Town = inst.Town
            };
            obj.Name = reader.GetString(reader.FieldCount - 2);
            obj.HierarchyId = reader.GetString(reader.FieldCount - 1);
            return obj;
        }

        internal override Institution Map(SqlDataReader reader)
        {
            int i = 0;
            Institution newObject = new Institution();
            newObject.Email = reader.GetString(++i);
            if (!reader.IsDBNull(++i))
                newObject.PhoneNumber = reader.GetInt64(i);
            if (!reader.IsDBNull(++i))
                newObject.Street = reader.GetString(i);
            newObject.HouseNumber = reader.GetString(++i);
            newObject.Town = reader.GetString(++i);
            newObject.PostalCode = reader.GetInt32(++i);
            newObject.Name = reader.GetString(++i);
            return newObject;
        }

        internal override void PrepareCommand(SqlCommand command, Institution obj)
        {
            command.Parameters.AddWithValue("@Email", obj.Email);
            command.Parameters.AddWithValue("@Telefon", (object) obj.PhoneNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("@Ulice", (object) obj.Street ?? DBNull.Value);
            command.Parameters.AddWithValue("@Cislo_popisne", obj.HouseNumber);
            command.Parameters.AddWithValue("@Mesto", obj.Town);
            command.Parameters.AddWithValue("@PSC", obj.PostalCode);
        }
    }
}
