using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DomainModel
{
    public class ReservationObject : StructuralObject
    {
        public ReservationObject()
        {
        }

        public ReservationObject(int id, string name, int price, int quantity, short type, string hierarchyId, DateTime? deletedFrom) : 
            base(id, name, hierarchyId, deletedFrom)
        {
            Price = price;
            Quantity = quantity;
            Type = type;
        }

        public int Price { get; set; }
        public int Quantity { get; set; }
        public short Type { get; set; }
    }
}
