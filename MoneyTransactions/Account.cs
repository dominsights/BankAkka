using MoneyTransactions.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions
{
    public class Account : Entity
    {
        public Account(Guid accountId, decimal balance, User user) : base(accountId)
        {
            Balance = balance;
            User = user;
        }

        public decimal Balance { get; }
        public User User { get; }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Balance, User);
        }
    }
}
