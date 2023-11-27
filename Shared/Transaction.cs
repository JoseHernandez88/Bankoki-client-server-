using Bankoki_client_server.Shared;
using static Bankoki_client_server_.Shared.Accounts;

namespace Bankoki_client_server_.Shared
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public string Origin { get; set; }
        =string.Empty;
        public int Ammount {  get; set; }
        public bool Credit { get; set; }
        public DateOnly TransactionDate { get; set; }
        public Transaction() { }
        
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
                    this.Ammount = temp.Ammount;
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
        public int signedAmmount()
        {
            return this.Credit ? this.Ammount : -1 * this.Ammount;
        }
        public int sum(int term)
        {
            
            return this.Credit ? term+this.Ammount : term-this.Ammount;
        }
        public int sum(Transaction transaction)
        {
            return this.Credit ? transaction.signedAmmount() + this.Ammount : transaction.signedAmmount() - this.Ammount;
        }

        
}
}
