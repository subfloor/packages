using System;
using System.Collections.Generic;

namespace Subfloor.Dtos
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public TransactionStatus Status { get; set; }
        public Guid RequestorId { get; set; }
        public Guid ResponderId { get; set; }
        public string? RequestorData { get; set; }
        public string? ResponderData { get; set; }
    }

    public enum TransactionStatus
    {
        RequestorSubmitted = 0,
        BrokerReceived = 1,
        ResponderNotified = 2,
        ResponderAcknowledged = 3,
        ResponderProcessing = 4,
        ResponderCompleted = 5,
        RequestorNotified = 6,
        RequestorAcknowledged = 7,
        Failed = 10
    }

}
