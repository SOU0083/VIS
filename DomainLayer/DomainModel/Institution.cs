using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DomainModel
{
    public class Institution : StructuralObject, IUser
    {
        public Institution()
        {
        }

        public Institution(int id, string name, string email, long? phoneNumber, string street, string houseNumber, string town, int postalCode, 
            List<InstitutionCategory> categories, string hierarchyId, DateTime? deletedFrom) : base(id, name, hierarchyId, deletedFrom)
        {
            Email = email;
            PhoneNumber = phoneNumber;
            Street = street;
            HouseNumber = houseNumber;
            Town = town;
            PostalCode = postalCode;
            Categories = categories;
        }

        
        public string Email { get; set; }
        public string HouseNumber { get; set; }
        public int PostalCode { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public List<InstitutionCategory> Categories { get; set; }
        public long? PhoneNumber { get; set; }

        public bool HasPhoneNumber() => PhoneNumber != null;

        public void WriteEmail(string text)
        {

        }

        public override DateTime? DeletedFrom
        {
            get
            {
                return _DeletedFrom;
            }
            set
            {
                if (value > DateTime.Now)
                {
                    throw new NotSupportedException();
                }
                _DeletedFrom = value;
            }
        }
    }
}
