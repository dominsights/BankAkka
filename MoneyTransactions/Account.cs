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

        public decimal Balance { get; private set; }

        public Status Withdraw(decimal amount)
        {
            if(Balance >= amount)
            {
                Balance -= amount;
                return Status.Success;
            } else
            {
                return Status.Error;
            }
        }

        public Client Client { get; }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Balance, Client);
        }

        public void Deposit(decimal amount)
        {
            if(amount > 0)
            {
                Balance += amount;
            } else
            {
                throw new InvalidOperationException($"{nameof(amount)} can't be 0 or negative.");
            }
        }
    }
}
