using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subfloor.Dtos
{
    public class ClientPersonRole
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid PersonId { get; set; }
        public Guid RoleId { get; set; }
    }
}
