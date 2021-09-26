using Akka.Actor;
using MoneyTransactions.Foundation;
using static MoneyTransactions.Actors.AccountActor;

namespace MoneyTransactions.Actors
{
    public class TransferActor : ReceiveActor
    {

        public record TransferMoney(decimal Amount, Account Source, Account Destination);

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

                var source = Context.ActorSelection("/user/bank/" + msg.Source.Id);

                Become(ExecutingTransfer);
                source.Tell(new Withdraw(msg.Amount));
            });
        }

        private void ExecutingTransfer()
        {
            Receive<Result<Withdraw>>(msg => msg.Status == Status.Success, msg =>
            {
                var destination = Context.ActorSelection("/user/bank/" + _transferMoney.Destination.Id);
                destination.Tell(new Deposit(_transferMoney.Amount));
            });

            Receive<Result<Withdraw>>(msg =>
            {
                _sender.Tell(new Result<TransferMoney>(Status.Error));
            });

            Receive<Result<Deposit>>(msg =>
            {
                _sender.Tell(new Result<TransferMoney>(Status.Success));
                Become(Ready);
            });
        }
    }
}
