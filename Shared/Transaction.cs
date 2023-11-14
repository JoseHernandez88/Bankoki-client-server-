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
}
}
