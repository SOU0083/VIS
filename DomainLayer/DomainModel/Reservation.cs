using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DomainModel
{
    public class Reservation : IEntity, IDeleted
    {
        public Reservation()
        {
        }

        public Reservation(int id, Customer reservation_Customer, ReservationObject reservation_ReservationObject, Event reservation_Event, DateTime? from, DateTime? to, DateTime? deletedFrom)
        {
            Id = id;
            Reservation_Customer = reservation_Customer;
            Reservation_ReservationObject = reservation_ReservationObject;
            Reservation_Event = reservation_Event;
            From = from;
            To = to;
            DeletedFrom = deletedFrom;
        }

        public int Id { get; set; }
        public Customer Reservation_Customer { get; set; }
        public ReservationObject Reservation_ReservationObject { get; set; }
        public Event Reservation_Event { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public DateTime? DeletedFrom { get; set; }

        public bool HasStarted() => From <= DateTime.UtcNow;
        public bool HasEnded() => To <= DateTime.UtcNow;
        public bool IsInProgress() => (HasStarted() && !HasEnded());

        public bool IsDeleted() => DeletedFrom != null;
    }
}
