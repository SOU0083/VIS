using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DomainModel
{
    public interface IDeleted
    {
        DateTime? DeletedFrom { get; set; }

        bool IsDeleted();
    }
}
