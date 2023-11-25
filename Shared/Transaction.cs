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
