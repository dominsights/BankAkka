using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Actors
{
    public class BankActor : ReceiveActor
    {
        public record CreateAccount(Client Client);
        public record CreateAccountResult(Account Account, Status Status);

        public BankActor()
        {
            Receive<CreateAccount>(msg =>
            {
                var account = new Account(Guid.NewGuid(), 0m, msg.Client);
                Context.ActorOf(Props.Create(() => new AccountActor(account)), account.Id.ToString());
                Sender.Tell(new CreateAccountResult(account, Status.Success));
            });
        }
    }
}
