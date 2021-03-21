namespace MoneyTransactions
{
    public class Withdraw
    {
        public decimal Amount { get; }

        public Withdraw(decimal amount)
        {
            this.Amount = amount;
        }
    }
}