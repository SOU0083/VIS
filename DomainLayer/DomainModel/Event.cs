using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DomainModel
{
    public class Event: IEntity, IDeleted
    {
        public Event()
        {
        }
        public Event(int id, string name, StructuralObject event_Object, DateTime start, DateTime end, DateTime canReserveFrom, DateTime canReserveTo, DateTime? deletedFrom)
        {
            Name = name;
            Id = id;
            Event_Object = event_Object;
            Start = start;
            End = end;
            CanReserveFrom = canReserveFrom;
            CanReserveTo = canReserveTo;
            DeletedFrom = deletedFrom;
        }

        public string Name { get; set; }
        public StructuralObject Event_Object { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime CanReserveFrom { get; set; }
        public DateTime CanReserveTo { get; set; }
        public DateTime? DeletedFrom { get; set; }
        public int Id { get; set; }

        public bool IsDeleted() => DeletedFrom != null;

        public bool HasReservationStarted() => CanReserveFrom <= DateTime.UtcNow;
        public bool HasReservationEnded() => CanReserveTo <= DateTime.UtcNow;
        public bool CanReserve() => (HasReservationStarted() && !HasReservationEnded());

        public bool HasStarted() => Start <= DateTime.UtcNow;
        public bool HasEnded() => End <= DateTime.UtcNow;
        public bool IsInProgress() => (HasStarted() && !HasEnded());
        public int ProgressPercentage()
        {
            if (!IsInProgress())
                throw new Exception();
            return (int) ((DateTime.UtcNow - Start).TotalMinutes / (End - Start).TotalMinutes);
        }
    }
}
