using System;

namespace DomainLayer.DomainModel
{
    public class StructuralObject: IEntity, IDeleted
    {
        public StructuralObject()
        {
        }

        public StructuralObject(int id, string name, string hierarchyId, DateTime? deletedFrom)
        {
            Id = id;
            Name = name;
            HierarchyId = hierarchyId;
            DeletedFrom = deletedFrom;
        }

        public string Name { get; set; }
        public string HierarchyId { get; set; }
        public int Id { get; set; }

        protected DateTime? _DeletedFrom;
        public virtual DateTime? DeletedFrom
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
        public bool IsDeleted() => DeletedFrom != null;
    }
}