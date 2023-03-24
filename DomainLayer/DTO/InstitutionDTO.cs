using DomainLayer.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTO
{
    public class InstitutionDTO
    {
        public string Email { get; set; }
        public string HouseNumber { get; set; }
        public int PostalCode { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public List<InstitutionCategory> Categories { get; set; }
        public long? PhoneNumber { get; set; }

        public string Name { get; set; }
        public string HierarchyId { get; set; }
    }
}
