using System;

namespace PersonalFianceTracker
{
    internal enum TransactionType
    {
        Income,
        Expense
    }

    internal class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public TransactionType Type { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; } = "";

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}