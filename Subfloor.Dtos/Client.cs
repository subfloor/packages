using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subfloor.Dtos
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Client_Id { get; set; }
        public string Name { get; set; }
        public List<Role> Roles { get; set; }
    }
}
