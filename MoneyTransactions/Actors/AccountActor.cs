using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Actors
{
    public class AccountActor : ReceiveActor
    {
        public AccountActor(Account account)
        {
            Account = account;
        }

        public Account Account { get; }
    }
}
