using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DomainModel
{
    public class Customer: IUser
    {
        public Customer()
        {
        }

        public Customer(int id, string firstName, string surname, long? phoneNumber, string email, string street, string houseNumber, string town, int postalCode)
        {
            Id = id;
            FirstName = firstName;
            Surname = surname;
            PhoneNumber = phoneNumber;
            Email = email;
            Street = street;
            HouseNumber = houseNumber;
            Town = town;
            PostalCode = postalCode;
        }

        public string FirstName { get; set; }
        public string Surname { get; set; }
        public long? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string HouseNumber { get; set; }
        public int PostalCode { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public int Id { get; set; }

        public bool HasPhoneNumber() => PhoneNumber != null;

        public void WriteEmailAndPayBack(string text)
        {
            this.WriteEmail(text);
        }
        public void WriteEmail(string text)
        {

        }
    }
}
