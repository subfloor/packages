using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subfloor.Dtos
{
    public class Person
    {
        public Guid Id { get; set; }
        public Guid IdentityId { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
    }
}
