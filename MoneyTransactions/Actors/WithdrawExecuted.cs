namespace MoneyTransactions.Actors
{
    internal class WithdrawExecuted
    {
        public WithdrawExecuted(decimal amount)
        {
            Amount = amount;
        }

        public decimal Amount { get; internal set; }
    }
}