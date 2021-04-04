using Akka.Actor;
using Akka.Persistence;
using MoneyTransactions.Actors.Messages;
using MoneyTransactions.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Actors
{
    public class AccountActor : PersistentActor
    {
        public AccountActor(Account account)
        {
            Account = account;
        }

        public Account Account { get; }

        public override string PersistenceId => Account.Id.ToString();

        protected override bool ReceiveCommand(object message)
        {
            switch(message)
            {
                case CheckBalance checkBalance:
                    Sender.Tell(new BalanceStatus(Account.Balance));
                    return true;
                case Deposit deposit:
                    var depositExecutedEvent = new DepositExecuted(deposit.Amount);
                    Persist(depositExecutedEvent, HandleEvent);
                    return true;
                case Withdraw withdraw:
                    var withdrawExecutedEvent = new WithdrawExecuted(withdraw.Amount);
                    Persist(withdrawExecutedEvent, HandleEvent);
                    return true;
                default:
                    return false;
            }
        }

        private void HandleEvent(DepositExecuted @event)
        {
            Account.Deposit(@event.Amount);
            Sender.Tell(new Result<Deposit>(Status.Success));
        }

        private void HandleEvent(WithdrawExecuted @event)
        {
            var result = Account.Withdraw(@event.Amount);
            Sender.Tell(new Result<Withdraw>(result));
        }

        protected override bool ReceiveRecover(object message)
        {
            switch(message)
            {
                case DepositExecuted deposit:
                    HandleEvent(deposit);
                    return true;
                case WithdrawExecuted withdraw:
                    HandleEvent(withdraw);
                    return true;
                default:
                    return false;
            }
        }
    }
}
