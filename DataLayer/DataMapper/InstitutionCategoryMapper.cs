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
    public class InstitutionCategoryMapper : AbstractMapper<InstitutionCategory>
    {
        private const string tableName = "KategorieInstituce";
        private static readonly string[] columnNames = { "Nazev" };

        private const string SQL_FIND_STATISTICS = @"SELECT t.Nazev, COUNT(*) AS Pocet
            FROM Objekt o2
            JOIN(
                SELECT o.HierarchieId, ki.Nazev, ki.Id
                FROM Objekt o
                JOIN Instituce i ON o.Id= i.Id
                JOIN Instituce_KategorieInstituce iki ON iki.InstituceId= o.Id
                JOIN KategorieInstituce ki ON ki.Id= iki.KategorieInstituceId
                WHERE o.SmazanoOd IS NULL) AS t ON o2.HierarchieId.IsDescendantOf(t.HierarchieId) = 1 
            JOIN Rezervace r ON r.RezervacniObjektId=o2.Id
            WHERE r.SmazanoOd IS NULL
            GROUP BY t.Id, t.Nazev";
        private const string SQL_FIND_NAMES = "SELECT Nazev FROM " + tableName;

        public InstitutionCategoryMapper() : base(tableName, columnNames, Methods.All)
        {
        }

        public List<InstitutionCategoryStatisticsDTO> FindStatistics()
        {
            List<InstitutionCategoryStatisticsDTO> objects = new List<InstitutionCategoryStatisticsDTO>();
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand(SQL_FIND_STATISTICS))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int i = -1;
                            InstitutionCategoryStatisticsDTO newObject = new InstitutionCategoryStatisticsDTO();
                            newObject.Name = reader.GetString(++i);
                            newObject.Count = reader.GetInt32(++i);
                            objects.Add(newObject);
                        }
                    }
                }
            }
            return objects;
        }
        public List<string> FindNames()
        {
            List<string> objects = new List<string>();
            using (Database db = new Database())
            {
                using (SqlCommand command = db.CreateCommand(SQL_FIND_NAMES))
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

        internal override InstitutionCategory Map(SqlDataReader reader)
        {
            int i = 0;
            InstitutionCategory newObject = new InstitutionCategory();
            newObject.Name = reader.GetString(++i);
            return newObject;
        }

        internal override void PrepareCommand(SqlCommand command, InstitutionCategory obj)
        {
            command.Parameters.AddWithValue("@Nazev", obj.Name);
        }
    }
}
