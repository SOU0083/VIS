namespace DomainLayer.DomainModel
{
    public interface IUser : IEntity
    {
        long? PhoneNumber { get; set; }
        string Email { get; set; }
        string HouseNumber { get; set; }
        int PostalCode { get; set; }
        string Street { get; set; }
        string Town { get; set; }

        bool HasPhoneNumber();
        void WriteEmail(string text);
    }
}