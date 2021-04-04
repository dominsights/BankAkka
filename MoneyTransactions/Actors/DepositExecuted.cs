namespace MoneyTransactions.Actors
{
    internal class DepositExecuted
    {
        public decimal Amount { get; set; }

        public DepositExecuted(decimal amount)
        {
            Amount = amount;
        }
    }
}