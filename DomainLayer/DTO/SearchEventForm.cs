using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTO
{
    public class SearchEventForm
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Price { get; set; }
    }
}
