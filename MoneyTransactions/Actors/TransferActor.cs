using Akka.Actor;
using MoneyTransactions.Actors.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Actors
{
    public class TransferActor : ReceiveActor
    {
        private IActorRef _sender;
        private TransferMoney _transferMoney;

        public TransferActor()
        {
            Become(Ready);
        }

        private void Ready()
        {
            Receive<TransferMoney>(msg =>
            {
                _sender = Sender;
                _transferMoney = msg;

                Become(WaitingForConfirmation);
                msg.Source.Tell(new Withdraw(msg.Amount));
            });
        }

        private void WaitingForConfirmation()
        {
            Receive<WithdrawResult>(msg => msg.Result == Result.Success, msg =>
            {
                _transferMoney.Destination.Tell(new Deposit(_transferMoney.Amount));
            });

            Receive<WithdrawResult>(msg =>
            {
                _sender.Tell(new TransferResult(Result.Error));
            });

            Receive<DepositResult>(msg =>
            {
                _sender.Tell(new TransferResult(Result.Success));
                Become(Ready);
            });
        }
    }
}
