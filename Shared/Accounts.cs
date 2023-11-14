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
    public DateTime CloseDate { get; set; }
    public required List<Transaction> History { get; set; }
}
}
