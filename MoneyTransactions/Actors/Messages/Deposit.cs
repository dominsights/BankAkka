using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Actors.Messages
{
    public class Deposit
    {
        public Deposit(decimal amount, Guid senderId, string senderFullName)
        {
            Amount = amount;
            SenderId = senderId;
            SenderFullName = senderFullName;
        }

        public decimal Amount { get; }
        public Guid SenderId { get; }
        public string SenderFullName { get; }
    }
}
