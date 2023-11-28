using Bankoki_client_server.Shared;
using static Bankoki_client_server_.Shared.Accounts;

namespace Bankoki_client_server_.Shared
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public string Origin { get; set; }
        =string.Empty;
        public int Amount {  get; set; }
        public bool Credit { get; set; }
        public DateOnly TransactionDate { get; set; }
        public Transaction() { }

        public Transaction( string origin, int amount, bool credit)
        {
            Origin = origin;
            Amount = amount;
            Credit = credit;
            TransactionDate = DateOnly.FromDateTime(DateTime.Now);
         }
        
        public Transaction(int TransactionID)
        {
            this.TransactionID = TransactionID;
            QueryHandler qh = new QueryHandler();
            try
            {
                Transaction? temp = qh.getTransactionAsync(this.TransactionID).Result;
                if (temp != null)
                {
                    this.Origin = temp.Origin;
                    this.Amount = temp.Amount;
                    this.Credit = temp.Credit;
                    this.TransactionDate = temp.TransactionDate;
                }
                else
                {
                    throw new AccountException("The transaction was not found in the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public int signedAmount()
        {
            return this.Credit ? this.Amount : -1 * this.Amount;
        }
        public int sum(int term)
        {
            
            return this.Credit ? term+this.Amount : term-this.Amount;
        }
        public int sum(Transaction transaction)
        {
            return this.Credit ? transaction.signedAmount() + this.Amount : transaction.signedAmount() - this.Amount;
        }

        
}
}
