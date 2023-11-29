using Bankoki_client_server.Shared;
//using Google.Protobuf.WellKnownTypes;
using System.Net.Http.Headers;
using System.Transactions;

namespace Bankoki_client_server_.Shared
{
    public class Accounts
    {
        public string AccountNumber { get; set; }
         = string.Empty;
        public string AccountName { get; set; }
         = string.Empty;
        public bool Open { get; set; }
        public DateOnly OpenDate { get; set; }
        public DateOnly? CloseDate { get; set; } = null;
        public List<Transaction> History { get; set; } = new List<Transaction>();

        public class AccountException : Exception
        {

            public AccountException(string message)

            : base(message)

            {
                string ex = message;
            }

        }
        public Accounts() { }

		public Accounts(string AccNumber)
        {
            AccountNumber = AccNumber;
            QueryHandler qh = new QueryHandler();
            try
            {
				Accounts? temp = qh.getAccountsAsync(AccountNumber).Result;
				//Accounts? temp = qh.getAccountsAsync(AccNumber).Result;

				if (temp != null)
                {
					History = temp.History;
                    OpenDate = temp.OpenDate;
                    CloseDate = temp.CloseDate;
                    AccountName = temp.AccountName;
                    Open = temp.Open;
                }
                else
                {
                    throw new AccountException("The account number was not found in the database.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public double balance()
        {
            int balance = 0;
            foreach (Transaction transaction in History)
            {
                balance = transaction.sum(balance);
            }
            return balance / 100.0;

        }
        public void cancelAccount()
        {
            if (this.Open)
            {
                if (this.balance() == 0.0)
                {
                    this.CloseDate = DateOnly.FromDateTime(DateTime.Now);
                    this.Open = false;
                    QueryHandler qh = new QueryHandler();
                    qh.CloseAccount(this.AccountNumber);
                }
                else
                {
                    throw new AccountException("Tried to close an account with balance.");
                }

            }
            else
            {
                throw new AccountException("Tried to close a closed account.");
            }
        }
        public void addTransaction(Transaction? transaction)
        {

            QueryHandler qh = new QueryHandler();
            long? transactionID = qh.insertTransactionAsync(transaction, this.AccountNumber).Result;
            if (transactionID != null)
            {
                this.appendTransaction2History((int)transactionID.Value);
            }
            else
            {
                throw new AccountException("Transaction failed to insert.");
            }
            

        }
        public void appendTransaction2History(int transactionID)
        {
            this.History.Append<Transaction>(new Transaction(transactionID));
        }

        public void test()
        {
            QueryHandler qh = new QueryHandler();
            //qh.test();
            Console.WriteLine(qh.ConBuild.ConnectionString);
        }
    }
}
    
