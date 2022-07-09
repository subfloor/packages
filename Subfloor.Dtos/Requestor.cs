using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subfloor.Dtos
{
    public class Requestor
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? QueueName { get; set; }
    }
}
