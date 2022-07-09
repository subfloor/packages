using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subfloor.Dtos
{
    public class TransactionHistory
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid ActorId { get; set; }
    }
}
