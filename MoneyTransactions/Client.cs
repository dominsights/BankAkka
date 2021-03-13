using MoneyTransactions.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions
{
    public class Client : Entity
    {
        public Client(Guid userId, string firstName, string lastName) : base(userId)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string FullName => $"{FirstName} {LastName}";

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FirstName, LastName);
        }
    }
}
