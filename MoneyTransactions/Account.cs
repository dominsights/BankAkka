using MoneyTransactions.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions
{
    public class Account : Entity
    {
        public Account(Guid accountId, decimal balance, Client client) : base(accountId)
        {
            Balance = balance;
            Client = client;
        }

        public decimal Balance { get; }
        public Client Client { get; }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Balance, Client);
        }
    }
}
