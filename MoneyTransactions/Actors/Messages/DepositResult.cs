namespace MoneyTransactions
{
    public class DepositResult
    {
        public Result Result { get; }

        public DepositResult(Result result)
        {
            Result = result;
        }
    }
}