using Bankoki_client_server.Shared;
using Google.Protobuf.WellKnownTypes;
using System.Net.Http.Headers;

namespace Bankoki_client_server_.Shared
{
    public class Accounts
    {
        public string AccountNumber { get; set; }
         = string.Empty;
        public string Name { get; set; }
         = string.Empty;
        public bool Open { get; set; }
        public DateOnly OpenDate { get; set; }
        public DateOnly? CloseDate { get; set; } = null;
        public required List<Transaction> History { get; set; }
        public class AccountException : Exception

        {

            public AccountException(string message)

            : base(message)

            {
                string ex = message;
            }

        }
        public double balance()
        {
            int balance = 0;
            foreach( Transaction transaction in History)
            {
                balance = transaction.sum(balance);
            }
            return balance/100.0;

        }
        public void cancelAccount()
        {
            if (this.Open)
            {
                if( this.balance() == 0.0)
                {
                    this.CloseDate = DateOnly.FromDateTime(DateTime.Now);
                    this.Open = false;
                }
                else
                {
                    throw new AccountException( "Tried to close a closed account.");
                }

            }
            else
            {
                throw new AccountException("Tried to close an account with balance.");
            }
        }
        public void addTransaction(Transaction? transaction)
        {
            
            Int64 intTransactionID;
            QueryHandler qh= new QueryHandler();
            long? transactionID= qh.insertTransaction(transaction,this.AccountNumber).Result;
            if (transactionID != null)
            {
                intTransactionID = transactionID.Value;
            }
            else
            {
                throw new AccountException("Transaction failed to insert.");
            }
            transaction = qh.getTransaction((int)intTransactionID).Result;
            if (transaction != null)
            {
                this.History.Append<Transaction?>(transaction);
            }
            else
            {
                throw new AccountException("Failed to get Transaction from Database.");
            }
             
        }
    }

}
    
