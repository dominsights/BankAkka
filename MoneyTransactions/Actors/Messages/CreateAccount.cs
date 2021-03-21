namespace MoneyTransactions
{
    public class CreateAccount
    {


        public CreateAccount()
        {
        }

        public CreateAccount(Client client)
        {
            Client = client;
        }

        public Client Client { get; }
    }
}